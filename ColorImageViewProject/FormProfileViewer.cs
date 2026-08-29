using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace ColorImageViewProject
{
	/// <summary>
	/// プロファイルのグラフ表示のためのフォームクラスです</summary>
	public partial class FormProfileViewer : Form
	{
		/// <summary>ピクチャーボックス用ビットマップ</summary>
		private Bitmap _bmpWorkPbox;
		/// <summary>ピクチャーボックスグラフ用Graphics</summary>
		private Graphics _graWorkPbox;
		/// <summary>ピクチャーボックスグラフ用Matrix</summary>
		private Matrix _matWorkPbox;

		/// <summary>表示するデータ（色の集合）</summary>
		private Color[] _colorDatas;

		/// <summary>最後にクリックした点のインデックスを保持します。（null時は非表示） </summary>
		private double? _lastDist;

		/// <summary>プロファイル2点測定の開始位置を保持します</summary>
		private double? _measureDistB;

		/// <summary>プロファイル2点測定の終了位置を保持します</summary>
		private double? _measureDistE;

		/// <summary>プロファイル全体統計を表示するテキストボックスを保持します</summary>
		private TextBox _tboxProfileStatistics;

		/// <summary>プロファイル2点測定結果を表示するテキストボックスを保持します</summary>
		private TextBox _tboxMeasureStatistics;

		/// <summary>プロファイルのピント指標を表示するテキストボックスを保持します</summary>
		private TextBox _tboxProfileFocus;

		/// <summary指定されたピクセルデータ全体の距離（長さ）を保持します</summary>
		/// <remarks>指定されたピクセルデータ群は傾きがあるラインの場合には、ピクセル距離とピクセル数が一致しないため外側から指定する</remarks>
		private double _distDatas;


		/// <summary>表示ポイント（Gray:輝度）</summary>
		private PointF[] _posGraphGray;
		/// <summary>表示ポイント（赤成分）</summary>
		private PointF[] _posGraphColR;
		/// <summary>表示ポイント（緑成分）</summary>
		private PointF[] _posGraphColG;
		/// <summary>表示ポイント（青成分）</summary>
		private PointF[] _posGraphColB;
		/// <summary>表示ポイント（H成分）</summary>
		private PointF[] _posGraphColHw;
		/// <summary>表示ポイント（S成分）</summary>
		private PointF[] _posGraphColS;

		/// <summary>クローズしようとしたときに呼び出されるアクションを保持します。nullの場合には何も行いません </summary>
		private Action _onClosingAction;

		/// <summary>クリックしたときに呼び出されるアクションを保持します。nullの場合には何も行いません。 OnClickAction( double dis/*距離*/ ) </summary>
		private Action<double> _onClickAction;

		/// <summary>2点測定範囲を変更したときに呼び出されるアクションを保持します</summary>
		private Action<double?, double?> _onMeasureRangeAction;

		/// <summary>前面表示モードを変更したときに呼び出されるアクションを保持します</summary>
		private Action<ProfileViewerForegroundMode> _onForegroundModeChangedAction;

		/// <summary>プロファイルモードを切り替えるときに呼び出されるアクションを保持します</summary>
		private Action _onToggleProfileModeAction;

		/// <summary>前面表示モードで参照する親フォームを保持します</summary>
		private Form _foregroundOwner;

		/// <summary>現在の前面表示モードを保持します</summary>
		private ProfileViewerForegroundMode _foregroundMode = ProfileViewerForegroundMode.ParentForeground;

		/// <summary>前面表示モードメニューを保持します</summary>
		private ToolStripMenuItem _foregroundModeMenuItem;

		/// <summary>画像情報画面の前面表示メニューを保持します</summary>
		private ToolStripMenuItem _parentForegroundMenuItem;

		/// <summary>常に最前面表示メニューを保持します</summary>
		private ToolStripMenuItem _alwaysTopMostMenuItem;

		/// <summary>通常表示メニューを保持します</summary>
		private ToolStripMenuItem _normalForegroundMenuItem;

		/// <summary>クローズ処理を無視するフラグです</summary>
		public bool IsIgnoreClose { get; set; }


		/// <summary>
		/// コンストラクタです</summary>
		/// <param name="datas">データ(未定の場合null）</param>
		/// <param name="title">タイトル</param>
		/// <param name="onClosing">終了時のコールバックアクション</param>
		/// <param name="distance">距離。（省略時は0.0=データの長さと同じ）</param>
		/// <param name="ic">アイコン</param>
		/// <param name="onClick">クリック時のコールバックアクション　　OnClick(int idx)</param>
		/// <param name="onMeasureRange">2点測定範囲変更時のコールバックアクション</param>
		/// <param name="onForegroundModeChanged">前面表示モード変更時のコールバックアクション</param>
		/// <param name="onToggleProfileMode">プロファイルモード切替時のコールバックアクション</param>
		public FormProfileViewer(Color[] datas, string title, Action onClosing = null, double distance = 0.0, Icon ic = null, Action<double> onClick = null, Action<double?, double?> onMeasureRange = null, Action<ProfileViewerForegroundMode> onForegroundModeChanged = null, Action onToggleProfileMode = null)
		{
			InitializeComponent();
			InitializeProfileAnalysisControls();
			InitializeForegroundModeMenu();

			_matWorkPbox = new Matrix();
			SetData(datas, distance, false);
			_onClosingAction = onClosing;
			IsIgnoreClose = true;

			this.Text = title;
			if (ic != null) this.Icon = ic;
			_onClickAction = onClick;
			_onMeasureRangeAction = onMeasureRange;
			_onForegroundModeChangedAction = onForegroundModeChanged;
			_onToggleProfileModeAction = onToggleProfileMode;
			_lastDist = null;

			// キーを一旦フォームが受け取る
			this.KeyPreview = true;
		}


		/// <summary>
		/// プロファイル解析情報表示用コントロールを初期化します</summary>
		private void InitializeProfileAnalysisControls()
		{
			const int extraHeight = 84;
			var graphHeight = pboxGraph.Height;
			var graphAnchor = pboxGraph.Anchor;
			var lowerControls = Controls.Cast<Control>().Where(w => w != pboxGraph && w.Top >= pboxGraph.Bottom).ToArray();
			var lowerControlAnchors = lowerControls.Select(w => (Control: w, Anchor: w.Anchor)).ToArray();
			SuspendLayout();

			pboxGraph.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			foreach (var item in lowerControlAnchors)
			{
				item.Control.Anchor = item.Anchor & ~AnchorStyles.Bottom;
			}

			ClientSize = new Size(ClientSize.Width, ClientSize.Height + extraHeight);
			pboxGraph.Height = graphHeight;
			pboxGraph.Anchor = graphAnchor;
			foreach (var item in lowerControlAnchors)
			{
				item.Control.Top += extraHeight;
				item.Control.Anchor = item.Anchor;
			}

			_tboxProfileStatistics = CreateProfileAnalysisTextBox(pboxGraph.Bottom + 4);
			_tboxMeasureStatistics = CreateProfileAnalysisTextBox(_tboxProfileStatistics.Bottom + 3);
			_tboxProfileFocus = CreateProfileAnalysisTextBox(_tboxMeasureStatistics.Bottom + 3);
			Controls.Add(_tboxProfileStatistics);
			Controls.Add(_tboxMeasureStatistics);
			Controls.Add(_tboxProfileFocus);

			ResumeLayout(false);
			PerformLayout();
		}


		/// <summary>
		/// プロファイル解析情報表示用テキストボックスを生成します</summary>
		/// <param name="top">上端位置</param>
		/// <returns>プロファイル解析情報表示用テキストボックス</returns>
		private TextBox CreateProfileAnalysisTextBox(int top)
		{
			return new TextBox
			{
				Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
				BackColor = SystemColors.Window,
				BorderStyle = BorderStyle.FixedSingle,
				Location = new Point(pboxGraph.Left, top),
				Name = $"tboxProfileAnalysis{top}",
				ReadOnly = true,
				Size = new Size(pboxGraph.Width, 19),
				TabStop = false
			};
		}


		/// <summary>
		/// 前面表示モードメニューを初期化します</summary>
		private void InitializeForegroundModeMenu()
		{
			_parentForegroundMenuItem = CreateForegroundModeMenuItem("画像情報画面の前面", ProfileViewerForegroundMode.ParentForeground);
			_alwaysTopMostMenuItem = CreateForegroundModeMenuItem("常に最前面", ProfileViewerForegroundMode.AlwaysTopMost);
			_normalForegroundMenuItem = CreateForegroundModeMenuItem("通常表示", ProfileViewerForegroundMode.Normal);
			_foregroundModeMenuItem = new ToolStripMenuItem("前面表示");
			_foregroundModeMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _parentForegroundMenuItem, _alwaysTopMostMenuItem, _normalForegroundMenuItem });
			contextMenuStrip1.Items.Add(new ToolStripSeparator());
			contextMenuStrip1.Items.Add(_foregroundModeMenuItem);
			contextMenuStrip1.Opening += contextMenuStrip1_Opening;
			UpdateForegroundModeMenuItems();
		}


		/// <summary>
		/// 前面表示モードメニュー項目を生成します</summary>
		/// <param name="text">表示テキスト</param>
		/// <param name="foregroundMode">前面表示モード</param>
		/// <returns>前面表示モードメニュー項目</returns>
		private ToolStripMenuItem CreateForegroundModeMenuItem(string text, ProfileViewerForegroundMode foregroundMode)
		{
			var menuItem = new ToolStripMenuItem(text)
			{
				CheckOnClick = false,
				Tag = foregroundMode
			};
			menuItem.Click += ForegroundModeMenuItem_Click;
			return menuItem;
		}


		/// <summary>
		/// コンテキストメニューが開くときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			UpdateForegroundModeMenuItems();
		}


		/// <summary>
		/// 前面表示モードメニュー項目がクリックされたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void ForegroundModeMenuItem_Click(object sender, EventArgs e)
		{
			if (sender is not ToolStripMenuItem menuItem) return;
			if (menuItem.Tag is not ProfileViewerForegroundMode foregroundMode) return;
			_onForegroundModeChangedAction?.Invoke(foregroundMode);
			ApplyForegroundMode(_foregroundOwner, foregroundMode);
			UpdateForegroundModeMenuItems();
		}


		/// <summary>
		/// 前面表示モードメニュー項目のチェック状態を更新します</summary>
		private void UpdateForegroundModeMenuItems()
		{
			if (_parentForegroundMenuItem == null || _alwaysTopMostMenuItem == null || _normalForegroundMenuItem == null) return;
			_parentForegroundMenuItem.Checked = _foregroundMode == ProfileViewerForegroundMode.ParentForeground;
			_alwaysTopMostMenuItem.Checked = _foregroundMode == ProfileViewerForegroundMode.AlwaysTopMost;
			_normalForegroundMenuItem.Checked = _foregroundMode == ProfileViewerForegroundMode.Normal;
		}


		/// <summary>
		/// ロード時の処理です
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void FormProfileViewer_Load(object sender, EventArgs e)
		{
			ResetWorkPbox();
			SetData(_colorDatas, _distDatas);

		}


		/// <summary>
		/// 前面表示モードを適用してフォームを表示します</summary>
		/// <param name="owner">親フォーム</param>
		/// <param name="foregroundMode">前面表示モード</param>
		public void ShowWithForegroundMode(Form owner, ProfileViewerForegroundMode foregroundMode)
		{
			ApplyForegroundMode(owner, foregroundMode);
			if (Visible == false)
			{
				if (foregroundMode == ProfileViewerForegroundMode.ParentForeground && owner != null) Show(owner);
				else Show();
			}
			else
			{
				if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
				if (foregroundMode != ProfileViewerForegroundMode.Normal) BringToFront();
			}
		}


		/// <summary>
		/// 前面表示モードを適用します</summary>
		/// <param name="owner">親フォーム</param>
		/// <param name="foregroundMode">前面表示モード</param>
		public void ApplyForegroundMode(Form owner, ProfileViewerForegroundMode foregroundMode)
		{
			if (owner != null) _foregroundOwner = owner;
			_foregroundMode = foregroundMode;
			var targetOwner = _foregroundOwner;
			if (Owner != null && (foregroundMode != ProfileViewerForegroundMode.ParentForeground || Owner != targetOwner))
			{
				Owner.RemoveOwnedForm(this);
			}

			if (foregroundMode == ProfileViewerForegroundMode.ParentForeground && targetOwner != null && Owner != targetOwner)
			{
				targetOwner.AddOwnedForm(this);
			}

			TopMost = foregroundMode == ProfileViewerForegroundMode.AlwaysTopMost;
			UpdateForegroundModeMenuItems();
		}


		/// <summary>
		/// クローズ時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void FormProfileViewer_FormClosed(object sender, FormClosedEventArgs e)
		{
			// 破棄
			_graWorkPbox?.Dispose();
			_bmpWorkPbox?.Dispose();
			_matWorkPbox?.Dispose();

			_graWorkPbox = null;
			_bmpWorkPbox = null;
			_matWorkPbox = null;

			if (_onClosingAction != null) _onClosingAction();
		}


		/// <summary>
		/// クローズしようとしたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void FormProfileViewer_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (IsIgnoreClose == false || e.CloseReason != CloseReason.UserClosing) return;

			e.Cancel = true;
			if (Visible)
			{
				this.Hide();
			}
		}


		/// <summary>
		/// データをセットします</summary>
		/// <param name="datas">データ群</param>
		/// <param name="distance">距離</param>
		/// <param name="isDraw">描画を行う。（初期化時を除き、通常はtrue）</param>
		public void SetData(Color[] datas, double distance, bool isDraw = true)
		{
			_posGraphColR = _posGraphColG = _posGraphColB = _posGraphColHw = _posGraphColS = null;
			labelIdxColInf.Text = "";
			_lastDist = null;
			_measureDistB = _measureDistE = null;

			// 変数群にセットする。　無効の場合には黒の256データとしてセットする。
			_colorDatas = datas ?? new Color[0]; // (datas ?? ( Enumerable.Repeat( Color.Black, 256 ))).ToArray();
			_distDatas = (datas == null || datas.Length == 0) ? 256.0 : ((distance == 0.0) ? (double)_colorDatas.Length : distance);

			if (isDraw == false) return;

			if (datas != null)
			{
				_posGraphGray = _colorDatas.Select((c, i) => new PointF((float)(i * _distDatas / datas.Length), (float)RgbToGray(c.R, c.G, c.B))).ToArray();
				// 一つでもグレーでないものがあればRGBデータをセット。
				if (_colorDatas.Any(c => c.R != c.B || c.R != c.G))
				{
					_posGraphColR = _colorDatas.Select((c, i) => new PointF((float)(i * _distDatas / datas.Length), (float)c.R)).ToArray();
					_posGraphColG = _colorDatas.Select((c, i) => new PointF((float)(i * _distDatas / datas.Length), (float)c.G)).ToArray();
					_posGraphColB = _colorDatas.Select((c, i) => new PointF((float)(i * _distDatas / datas.Length), (float)c.B)).ToArray();
					_posGraphColHw = _colorDatas.Select((c, i) => new PointF((float)(i * _distDatas / datas.Length), (float)c.GetHue())).ToArray(); // 0-360
					_posGraphColS = _colorDatas.Select((c, i) => new PointF((float)(i * _distDatas / datas.Length), (float)c.GetSaturation() * 255)).ToArray();
				}
			}
			else
			{
				_posGraphGray = _posGraphColG = _posGraphColB = _posGraphColHw = _posGraphColS = new PointF[0];
			}

			// テキスト情報
			if (datas == null || datas.Length == 0)
			{
				tboxDataQ.Text = "----";
				labelDist.Visible = tboxDist.Visible = false;
			}
			else
			{
				tboxDataQ.Text = datas.Length.ToString();
				if (distance > 0.0)
				{
					tboxDist.Text = distance.ToString("##0.00");
					labelDist.Visible = tboxDist.Visible = true;
				}
				else labelDist.Visible = tboxDist.Visible = false;
			}

			UpdateProfileAnalysisText();

			// データ描画
			DrawData();
		}


		/// <summary>
		/// グラフデータを描画します</summary>
		public void DrawData()
		{
			if (_matWorkPbox == null || _bmpWorkPbox == null || _graWorkPbox == null) return;
			if (_bmpWorkPbox.Width <= 0 || _bmpWorkPbox.Height <= 0 || _distDatas <= 0.0) return;
			// 座標設定
			_matWorkPbox.Reset();
			_matWorkPbox.Translate(0f, _bmpWorkPbox.Height, MatrixOrder.Prepend);              // 平行移動
			var dstep = (_colorDatas == null) ? 1.0f : (float)((double)_bmpWorkPbox.Width / (double)_distDatas);
			_matWorkPbox.Scale(dstep, -_bmpWorkPbox.Height / 255.0f, MatrixOrder.Prepend);     // スケール変更（上下反転）.

			DrawAllGraph();

			Refresh();
		}


		/// <summary>
		/// グラフを描画します。（有効な全データ対象）</summary>
		private void DrawAllGraph()
		{
			if (_graWorkPbox == null || _bmpWorkPbox == null || _matWorkPbox == null) return;
			if (_bmpWorkPbox.Width <= 0 || _bmpWorkPbox.Height <= 0) return;

			_graWorkPbox.Clear(Color.White);

			var ofsH = 0;
			if (rbtn120.Checked) ofsH = 120;
			else if (rbtn240.Checked) ofsH = 240;


			// グリッドラインの描画
			// グリッド(描画がチェックされていてかつ0-255データ表示が行われるときにはグリッドを描画する
			var cw = new CheckBox[] { cboxIsDrawColR, cboxIsDrawColG, cboxIsDrawColB, cboxIsDrawGray, cboxIsDrawColS };
			var isValueGridVisible = cboxIsDrawGridline.Checked && cw.Any(c => c.Checked);
			var isProfileLengthGridVisible = cboxIsDrawGridline.Checked || cboxIsDrawGridlineH.Checked;
			var profileLengthScaleStep = CreateScaleStep(_distDatas, 8);
			if (isValueGridVisible)
			{
				// 横線
				for (var yy = 0; yy < 255; yy += 10)
				{
					var pov = new PointF[] { new PointF(0, yy) };
					_matWorkPbox.TransformPoints(pov);
					using (var pen = new Pen((yy % 50 == 0) ? Color.LightBlue : Color.LightGray, (yy % 100 == 0) ? 2f : 1f))
					{
						_graWorkPbox.DrawLine(pen, 0f, pov[0].Y, _bmpWorkPbox.Width, pov[0].Y);
					}
				}

			}

			// グリッド(H)描画がチェックされていてかつH(色相）表示が行われるときには色相用グリッドを描画する
			if (cboxIsDrawGridlineH.Checked && cboxIsDrawColH.Checked)
			{
				// H用横線  360°
				using (var pen = new Pen(Color.LightGreen, 2f))
				using (var pen2 = new Pen(Color.LightGreen, 1f))
				{
					pen2.DashStyle = DashStyle.DashDot;
					_graWorkPbox.DrawLine(pen, 0f, (float)_bmpWorkPbox.Height / 3f * 1f, _bmpWorkPbox.Width, (float)_bmpWorkPbox.Height / 3f * 1f);
					_graWorkPbox.DrawLine(pen, 0f, (float)_bmpWorkPbox.Height / 3f * 2f, _bmpWorkPbox.Width, (float)_bmpWorkPbox.Height / 3f * 2f);
					for (int i = 1; i < 36; i++)
					{
						_graWorkPbox.DrawLine((i % 12 == 0) ? pen : pen2, 0f, (float)_bmpWorkPbox.Height / 36f * i, _bmpWorkPbox.Width, (float)_bmpWorkPbox.Height / 36f * i);
					}
				}
			}

			// どちらかのグリッドを表示する場合には縦(データ数）のグリッドラインを描画する
			if (isProfileLengthGridVisible)
			{
				DrawProfileLengthGridLines(profileLengthScaleStep);
			}

			// データの描画
			// B,G,R,Grayの順で描画する。（上書きされるので、同じ位置では後側が優先となる）
			if (cboxIsDrawColB.Checked && _posGraphColB != null) DrawGraphMat(_posGraphColB, new Pen(Color.Blue));
			if (cboxIsDrawColG.Checked && _posGraphColG != null) DrawGraphMat(_posGraphColG, new Pen(Color.Green));
			if (cboxIsDrawColR.Checked && _posGraphColR != null) DrawGraphMat(_posGraphColR, new Pen(Color.Red));
			if (cboxIsDrawGray.Checked && _posGraphGray != null) DrawGraphMat(_posGraphGray, new Pen(Color.Black));
			if (cboxIsDrawColS.Checked && _posGraphColS != null) DrawGraphMat(_posGraphColS, new Pen(Color.DarkCyan));
			// _posGraphColHwだけはy軸に 0-360が入っており、かつベースを 0, 120, 240を切り替えが可能。彩度が0ならばベースラインに落とす
			if (cboxIsDrawColH.Checked && _posGraphColHw != null)
			{
				var posGraphColH = _posGraphColHw.Select((hw, i) => new PointF(hw.X, _posGraphColS[i].Y == 0 ? 0 : ((hw.Y + 360 - ofsH) % 360) / 360f * 256f)).ToArray();
				DrawGraphMat(posGraphColH, new Pen(Color.Magenta));
			}

			DrawProfileCursorMarks();
			DrawScaleLabels(isValueGridVisible, isProfileLengthGridVisible, profileLengthScaleStep);
		}


		/// <summary>
		/// プロファイル長さ方向のグリッド線を描画します</summary>
		/// <param name="scaleStep">目盛間隔</param>
		private void DrawProfileLengthGridLines(double scaleStep)
		{
			if (scaleStep <= 0.0 || _distDatas <= 0.0) return;

			using (var vPen = new Pen(Color.LightGray, 1f))
			{
				for (var xx = 0.0; xx <= _distDatas + scaleStep / 2.0; xx += scaleStep)
				{
					var xValue = Math.Min(xx, _distDatas);
					var pov = new PointF[] { new PointF((float)xValue, 0) };
					_matWorkPbox.TransformPoints(pov);
					_graWorkPbox.DrawLine(vPen, pov[0].X, 0f, pov[0].X, _bmpWorkPbox.Height);
					if (xValue >= _distDatas) break;
				}
			}
		}


		/// <summary>
		/// グラフ目盛ラベルを描画します</summary>
		/// <param name="isValueScaleVisible">輝度系Y目盛を表示する場合true</param>
		/// <param name="isProfileLengthScaleVisible">プロファイル長さ方向目盛を表示する場合true</param>
		/// <param name="profileLengthScaleStep">プロファイル長さ方向目盛間隔</param>
		private void DrawScaleLabels(bool isValueScaleVisible, bool isProfileLengthScaleVisible, double profileLengthScaleStep)
		{
			if (_graWorkPbox == null || _bmpWorkPbox == null || _matWorkPbox == null) return;
			if (_bmpWorkPbox.Width < 40 || _bmpWorkPbox.Height < 30) return;

			using (var font = new Font(Font.FontFamily, 8f))
			using (var brush = new SolidBrush(Color.FromArgb(210, Color.DimGray)))
			using (var backBrush = new SolidBrush(Color.FromArgb(210, Color.White)))
			using (var tickPen = new Pen(Color.FromArgb(180, Color.DimGray), 1f))
			{
				if (isValueScaleVisible) DrawValueScaleLabels(font, brush, backBrush, tickPen);
				if (isProfileLengthScaleVisible) DrawProfileLengthScaleLabels(profileLengthScaleStep, font, brush, backBrush, tickPen);
			}
		}


		/// <summary>
		/// 輝度系Y目盛ラベルを描画します</summary>
		/// <param name="font">描画フォント</param>
		/// <param name="brush">文字ブラシ</param>
		/// <param name="backBrush">背景ブラシ</param>
		/// <param name="tickPen">目盛ペン</param>
		private void DrawValueScaleLabels(Font font, Brush brush, Brush backBrush, Pen tickPen)
		{
			foreach (var value in new[] { 0, 50, 100, 150, 200, 250 })
			{
				var y = ValueToGraphY(value);
				_graWorkPbox.DrawLine(tickPen, 0f, y, 6f, y);
				DrawScaleLabel(value.ToString(), 8f, y - 7f, font, brush, backBrush);
			}
		}


		/// <summary>
		/// プロファイル長さ方向目盛ラベルを描画します</summary>
		/// <param name="scaleStep">目盛間隔</param>
		/// <param name="font">描画フォント</param>
		/// <param name="brush">文字ブラシ</param>
		/// <param name="backBrush">背景ブラシ</param>
		/// <param name="tickPen">目盛ペン</param>
		private void DrawProfileLengthScaleLabels(double scaleStep, Font font, Brush brush, Brush backBrush, Pen tickPen)
		{
			if (scaleStep <= 0.0 || _distDatas <= 0.0) return;

			var lastRight = -1000f;
			for (var xx = 0.0; xx <= _distDatas + scaleStep / 2.0; xx += scaleStep)
			{
				var xValue = Math.Min(xx, _distDatas);
				var x = DistanceToGraphX(xValue);
				var text = FormatScaleValue(xValue);
				var size = _graWorkPbox.MeasureString(text, font);
				var left = x - size.Width / 2f;
				if (left > lastRight + 4f || xValue >= _distDatas)
				{
					_graWorkPbox.DrawLine(tickPen, x, _bmpWorkPbox.Height - 6f, x, _bmpWorkPbox.Height);
					DrawScaleLabel(text, left, _bmpWorkPbox.Height - size.Height - 2f, font, brush, backBrush);
					lastRight = left + size.Width;
				}

				if (xValue >= _distDatas) break;
			}
		}


		/// <summary>
		/// 目盛ラベルを描画します</summary>
		/// <param name="text">表示テキスト</param>
		/// <param name="x">描画X位置</param>
		/// <param name="y">描画Y位置</param>
		/// <param name="font">描画フォント</param>
		/// <param name="brush">文字ブラシ</param>
		/// <param name="backBrush">背景ブラシ</param>
		private void DrawScaleLabel(string text, float x, float y, Font font, Brush brush, Brush backBrush)
		{
			var size = _graWorkPbox.MeasureString(text, font);
			var drawX = Math.Max(1f, Math.Min(_bmpWorkPbox.Width - size.Width - 1f, x));
			var drawY = Math.Max(1f, Math.Min(_bmpWorkPbox.Height - size.Height - 1f, y));
			_graWorkPbox.FillRectangle(backBrush, drawX, drawY, size.Width, size.Height);
			_graWorkPbox.DrawString(text, font, brush, drawX, drawY);
		}


		/// <summary>
		/// 目盛値を読みやすい文字列に整形します</summary>
		/// <param name="value">目盛値</param>
		/// <returns>目盛表示文字列</returns>
		private string FormatScaleValue(double value)
		{
			if (Math.Abs(value - Math.Round(value)) < 0.001) return value.ToString("0");
			if (Math.Abs(value) >= 10.0) return value.ToString("0.0");
			return value.ToString("0.00");
		}


		/// <summary>
		/// 読みやすい目盛間隔を生成します</summary>
		/// <param name="maxValue">最大値</param>
		/// <param name="targetTickCount">目標目盛数</param>
		/// <returns>目盛間隔</returns>
		private double CreateScaleStep(double maxValue, int targetTickCount)
		{
			if (maxValue <= 0.0) return 1.0;

			var roughStep = maxValue / Math.Max(1, targetTickCount);
			var magnitude = Math.Pow(10.0, Math.Floor(Math.Log10(roughStep)));
			var normalized = roughStep / magnitude;
			if (normalized <= 1.0) return magnitude;
			if (normalized <= 2.0) return magnitude * 2.0;
			if (normalized <= 5.0) return magnitude * 5.0;
			return magnitude * 10.0;
		}


		/// <summary>
		/// 値をグラフY座標に変換します</summary>
		/// <param name="value">値</param>
		/// <returns>グラフY座標</returns>
		private float ValueToGraphY(double value)
		{
			var pov = new PointF[] { new PointF(0f, (float)value) };
			_matWorkPbox.TransformPoints(pov);
			return pov[0].Y;
		}


		/// <summary>
		/// プロファイルグラフ上のカーソル・2点測定位置を描画します</summary>
		private void DrawProfileCursorMarks()
		{
			if (_graWorkPbox == null || _matWorkPbox == null || _bmpWorkPbox == null) return;

			if (_measureDistB != null && _measureDistE != null)
			{
				var xB = DistanceToGraphX(_measureDistB.Value);
				var xE = DistanceToGraphX(_measureDistE.Value);
				var left = Math.Min(xB, xE);
				var right = Math.Max(xB, xE);
				using (var brush = new SolidBrush(Color.FromArgb(28, Color.Orange)))
				using (var penB = new Pen(Color.DarkOrange, 1f))
				using (var penE = new Pen(Color.DarkRed, 1f))
				{
					penB.DashStyle = DashStyle.Dash;
					penE.DashStyle = DashStyle.Dash;
					_graWorkPbox.FillRectangle(brush, left, 0f, right - left, _bmpWorkPbox.Height);
					_graWorkPbox.DrawLine(penB, xB, 0f, xB, _bmpWorkPbox.Height);
					_graWorkPbox.DrawLine(penE, xE, 0f, xE, _bmpWorkPbox.Height);
				}
			}

			if (_lastDist != null && _lastDist >= 0)
			{
				using (var iPen = new Pen(Color.Orange, 1f))
				{
					var x = DistanceToGraphX(_lastDist.Value);
					_graWorkPbox.DrawLine(iPen, x, 0f, x, _bmpWorkPbox.Height);
				}
			}
		}


		/// <summary>
		/// プロファイル距離をグラフX座標に変換します</summary>
		/// <param name="dist">プロファイル距離</param>
		/// <returns>グラフX座標</returns>
		private float DistanceToGraphX(double dist)
		{
			var pov = new PointF[] { new PointF((float)dist, 0f) };
			_matWorkPbox.TransformPoints(pov);
			return pov[0].X;
		}


		/// <summary>
		/// グラフ（指定データ）を描画します</summary>
		/// <param name="poss">ポイントデータ</param>
		/// <param name="pen">ペン</param>
		private void DrawGraph(PointF[] poss, Pen pen)
		{
			using (pen)
			{
				if (_graWorkPbox == null || poss == null || poss.Length == 0) return;
				_graWorkPbox.DrawLines(pen, poss);
			}
		}

		/// <summary>
		/// Mat変換、グラフ（指定データ）を描画します</summary>
		/// <param name="poss">ポイントデータ</param>
		/// <param name="pen">ペン</param>
		private void DrawGraphMat(PointF[] poss, Pen pen)
		{
			using (pen)
			{
				if (_graWorkPbox == null || poss == null || poss.Length == 0) return;
				var pv = poss.ToArray();
				_matWorkPbox.TransformPoints(pv);
				_graWorkPbox.DrawLines(pen, pv);
			}
		}


		/// <summary>
		/// プロット（指定データ）を描画します</summary>
		/// <param name="poss">ポイントデータ</param>
		/// <param name="pen">ペン</param>
		private void DrawPosPlot(PointF[] poss, Pen pen)
		{
			using (pen)
			{
				if (_graWorkPbox == null || poss == null || poss.Length == 0) return;
				foreach (var po in poss) _graWorkPbox.DrawEllipse(pen, new RectangleF(po.X - 1f, po.Y - 1f, 2f, 2f));
			}
		}


		/// <summary>
		/// RGB値からグレー値（輝度値）を算出する</summary>
		/// <param name="rr">赤成分</param>
		/// <param name="gg">緑成分</param>
		/// <param name="bb">青成分</param>
		/// <returns>グレー値（輝度）</returns>
		private int RgbToGray(int rr, int gg, int bb)
		{
			return (int)(rr * 0.299 + gg * 0.587 + bb * 0.114);
		}


		/// <summary>
		/// 指定したIcon（の複製）をアイコンとして設定します</summary>
		/// <param name="icon">アイコン</param>
		public void SetIcon(Icon icon)
		{
			this.Icon = (Icon)icon.Clone();
			Refresh();
		}


		/// <summary>
		/// ピクチャーボックス用ワークを現在の状態に合わせリセットします</summary>
		private void ResetWorkPbox()
		{
			// 元のワークは破棄し、ピクチャーボックスと同じサイズのビットマップを生成する。
			var pbox = pboxGraph as PictureBox;
			var oldGraphics = _graWorkPbox;
			var oldBitmap = _bmpWorkPbox;
			_graWorkPbox = null;
			_bmpWorkPbox = null;
			pboxGraph.Image = null;
			oldGraphics?.Dispose();
			oldBitmap?.Dispose();
			if (pbox == null || pbox.Width <= 0 || pbox.Height <= 0) return;

			_bmpWorkPbox = new Bitmap(pbox.Width, pbox.Height, PixelFormat.Format24bppRgb);
			_graWorkPbox = Graphics.FromImage(_bmpWorkPbox);
			pboxGraph.Image = _bmpWorkPbox;


		}



		/// <summary>
		/// ピクチャーボックスのサイズが変化したときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		/// <remarks>描画用ワークを再作成する</remarks>
		private void pboxGraph_SizeChanged(object sender, EventArgs e)
		{
			// ピクチャーボックス用ワークを現在の状態に合わせリセット
			ResetWorkPbox();


			// 再描画
			if (_graWorkPbox != null)
			{
				DrawData();
			}

		}


		/// <summary>
		/// 表示選択のチェックボックスに変更があったときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		/// <remarks>再描画を行う</remarks>
		private void OnChangeRbtnSelect(object sender, EventArgs e)
		{
			// 再描画
			if (_graWorkPbox != null)
			{
				DrawData();
			}
		}


		/// <summary>
		/// H-Baseラジオボタンに変更があったときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void rbtn0_CheckedChanged(object sender, EventArgs e)
		{
			if (((RadioButton)sender).Checked == false) return;
			// 再描画
			if (_graWorkPbox != null)
			{
				DrawData();
			}
		}


		/// <summary>「 グラフ画像をファイルに保存」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void グラフ画像をファイルに保存ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_bmpWorkPbox == null) return;
			SaveImageFile(_bmpWorkPbox);
		}


		/// <summary>「グラフ画像をクリップボードにコピー」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void グラフ画像をクリップボードにコピーToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_bmpWorkPbox == null) return;
			Clipboard.SetDataObject(_bmpWorkPbox);

		}


		/// <summary>「グラフ画像を画像情報画面で表示」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void グラフ画像を画像情報画面で表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_bmpWorkPbox == null) return;
			var frm = new FormColorImageS(_bmpWorkPbox, this.Text + "(Copy)", false, -1, -1, null, null, this.Icon);
			frm.Show();
		}


		/// <summary>「 フォーム全体画像をファイルに保存」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void フォーム全体画像をファイルに保存ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var wbmp = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb))
			{
				this.DrawToBitmap(wbmp, new Rectangle(0, 0, this.Width, this.Height));
				SaveImageFile(wbmp);
			}
		}


		/// <summary>「フォーム全体画像をクリップボードにコピー」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void フォーム全体画像をクリップボードにコピーToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 領域取得では、クリップボードにコピーした後もインスタンスは残しておく必要があるため usingは使用しない。
			var wbmp = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb);
			this.DrawToBitmap(wbmp, new Rectangle(0, 0, this.Width, this.Height));
			Clipboard.SetDataObject(wbmp);
		}


		/// <summary>「フォーム全体画像を画像情報画面で表示」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void フォーム全体画像を画像情報画面で表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var wbmp = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb))
			{
				this.DrawToBitmap(wbmp, new Rectangle(0, 0, this.Width, this.Height));
				var frm = new FormColorImageS(wbmp, this.Text + "(Copy)", false, -1, -1, null, null, this.Icon);
				frm.Show();
			}
		}


		/// <summary>画像イメージを名前をつけて保存</summary>
		/// <param name="bmp">画像イメージ</param>
		private void SaveImageFile(Bitmap bmp)
		{
			var sfd = new SaveFileDialog();

			//はじめのファイル名を指定する
			sfd.FileName = "プロファイルImage";
			//はじめに表示されるフォルダを指定する
			sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			//[ファイルの種類]に表示される選択肢を指定する
			sfd.Filter = UtilityGraphics.ImageSaveFileDialogFilter;
			//[ファイルの種類]ではじめに「PNGファイル」が選択されているようにする
			sfd.FilterIndex = 1;
			sfd.DefaultExt = "png";
			sfd.AddExtension = true;
			//タイトルを設定する
			sfd.Title = "保存ファイルを選択してください";
			//ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
			sfd.RestoreDirectory = true;
			//ダイアログを表示する
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				//OKボタンがクリックされたとき
				if (UtilityGraphics.TryResolveImageSaveFormat(sfd.FileName, sfd.FilterIndex, out var saveFileName, out var fmt) == false ||
					UtilityGraphics.SaveBitmap(bmp, saveFileName, fmt) == false)
				{
					MessageBox.Show(String.Format("ファイル（{0}）書き込みが失敗しました", Path.GetFileName(sfd.FileName)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}


		/// <summary>
		/// Click時に表示するカラー情報文字列に変換します</summary>
		/// <param name="col">色</param>
		/// <returns>色情報文字列</returns>
		private string ColorInfo(Color col)
		{
			var hsb = UtilityGraphics.HsvFromRgb(col.R, col.G, col.B);
			return $"RGB({col.R}, {col.G}, {col.B}),  HSB({hsb.H}, {hsb.S}, {hsb.V})";
		}


		/// <summary>
		/// プロファイル解析情報表示を更新します</summary>
		private void UpdateProfileAnalysisText()
		{
			if (_tboxProfileStatistics == null || _tboxMeasureStatistics == null || _tboxProfileFocus == null) return;
			_tboxProfileStatistics.Text = CreateProfileStatisticsText("全体", 0, (_colorDatas?.Length ?? 0) - 1);
			_tboxMeasureStatistics.Text = CreateProfileMeasureText();
			_tboxProfileFocus.Text = CreateProfileFocusText("ピント", 0, (_colorDatas?.Length ?? 0) - 1);
		}


		/// <summary>
		/// プロファイル統計表示文字列を生成します</summary>
		/// <param name="title">表示タイトル</param>
		/// <param name="bgnIdx">開始インデックス</param>
		/// <param name="endIdx">終了インデックス</param>
		/// <returns>プロファイル統計表示文字列</returns>
		private string CreateProfileStatisticsText(string title, int bgnIdx, int endIdx)
		{
			if (_colorDatas == null || _colorDatas.Length == 0) return $"{title}: ----";

			var bgn = Math.Max(0, Math.Min(bgnIdx, endIdx));
			var end = Math.Min(_colorDatas.Length - 1, Math.Max(bgnIdx, endIdx));
			if (end < bgn) return $"{title}: ----";

			var count = end - bgn + 1;
			var minGray = Int32.MaxValue;
			var maxGray = Int32.MinValue;
			var sumGray = 0.0;
			var sumGray2 = 0.0;
			var sumR = 0.0;
			var sumG = 0.0;
			var sumB = 0.0;
			for (var i = bgn; i <= end; i++)
			{
				var col = _colorDatas[i];
				var gray = RgbToGray(col.R, col.G, col.B);
				if (gray < minGray) minGray = gray;
				if (gray > maxGray) maxGray = gray;
				sumGray += gray;
				sumGray2 += (double)gray * gray;
				sumR += col.R;
				sumG += col.G;
				sumB += col.B;
			}

			var meanGray = sumGray / count;
			var varianceGray = Math.Max(0.0, sumGray2 / count - meanGray * meanGray);
			var sdGray = Math.Sqrt(varianceGray);
			return $"{title}: N={count}, Gray[min/max/mean/sd]={minGray}/{maxGray}/{meanGray:0.0}/{sdGray:0.0}, RGB平均=({sumR / count:0.0}, {sumG / count:0.0}, {sumB / count:0.0})";
		}


		/// <summary>
		/// プロファイルピント指標表示文字列を生成します</summary>
		/// <param name="title">表示タイトル</param>
		/// <param name="bgnIdx">開始インデックス</param>
		/// <param name="endIdx">終了インデックス</param>
		/// <returns>プロファイルピント指標表示文字列</returns>
		private string CreateProfileFocusText(string title, int bgnIdx, int endIdx)
		{
			if (TryCalculateProfileFocusMetrics(bgnIdx, endIdx, out var adjacentSquareMean, out var adjacentRms, out var laplacianSquareMean) == false) return $"{title}: ----";
			return $"{title}: 隣接輝度差二乗平均={FormatMetricValue(adjacentSquareMean)}, 平方根={FormatMetricValue(adjacentRms)}, ラプラシアン二乗平均={FormatMetricValue(laplacianSquareMean)}";
		}


		/// <summary>
		/// プロファイルピント指標を算出します</summary>
		/// <param name="bgnIdx">開始インデックス</param>
		/// <param name="endIdx">終了インデックス</param>
		/// <param name="adjacentSquareMean">隣接輝度差二乗平均</param>
		/// <param name="adjacentRms">隣接輝度差二乗平均の平方根</param>
		/// <param name="laplacianSquareMean">ラプラシアン二乗平均</param>
		/// <returns>算出できた場合true</returns>
		private bool TryCalculateProfileFocusMetrics(int bgnIdx, int endIdx, out double adjacentSquareMean, out double adjacentRms, out double laplacianSquareMean)
		{
			adjacentSquareMean = Double.NaN;
			adjacentRms = Double.NaN;
			laplacianSquareMean = Double.NaN;
			if (_colorDatas == null || _colorDatas.Length < 2) return false;

			var bgn = Math.Max(0, Math.Min(bgnIdx, endIdx));
			var end = Math.Min(_colorDatas.Length - 1, Math.Max(bgnIdx, endIdx));
			if (end - bgn + 1 < 2) return false;

			var grays = new int[end - bgn + 1];
			for (var i = bgn; i <= end; i++)
			{
				var col = _colorDatas[i];
				grays[i - bgn] = RgbToGray(col.R, col.G, col.B);
			}

			var adjacentCount = 0;
			var adjacentSquareSum = 0.0;
			for (var i = 1; i < grays.Length; i++)
			{
				var diff = grays[i] - grays[i - 1];
				adjacentSquareSum += (double)diff * diff;
				adjacentCount++;
			}
			adjacentSquareMean = adjacentSquareSum / adjacentCount;
			adjacentRms = Math.Sqrt(adjacentSquareMean);

			if (grays.Length >= 3)
			{
				var laplacianCount = 0;
				var laplacianSquareSum = 0.0;
				for (var i = 1; i < grays.Length - 1; i++)
				{
					var laplacian = grays[i - 1] - 2 * grays[i] + grays[i + 1];
					laplacianSquareSum += (double)laplacian * laplacian;
					laplacianCount++;
				}
				laplacianSquareMean = laplacianSquareSum / laplacianCount;
			}

			return true;
		}


		/// <summary>
		/// 指標値を表示用文字列に変換します</summary>
		/// <param name="value">指標値</param>
		/// <returns>表示文字列</returns>
		private string FormatMetricValue(double value)
		{
			if (Double.IsNaN(value) || Double.IsInfinity(value)) return "----";
			if (Math.Abs(value) >= 1000.0) return value.ToString("0");
			if (Math.Abs(value) >= 100.0) return value.ToString("0.0");
			return value.ToString("0.00");
		}


		/// <summary>
		/// プロファイル2点測定表示文字列を生成します</summary>
		/// <returns>プロファイル2点測定表示文字列</returns>
		private string CreateProfileMeasureText()
		{
			if (_colorDatas == null || _colorDatas.Length == 0) return "2点: ----";
			if (_measureDistB == null || _measureDistE == null) return "2点: グラフをクリックしてA/Bを指定。ドラッグでB更新。Escでクリア。";

			var idxB = DistanceToIndex(_measureDistB.Value);
			var idxE = DistanceToIndex(_measureDistE.Value);
			if (idxB < 0 || idxE < 0) return "2点: グラフをクリックしてA/Bを指定。ドラッグでB更新。Escでクリア。";

			var colB = _colorDatas[idxB];
			var colE = _colorDatas[idxE];
			var grayB = RgbToGray(colB.R, colB.G, colB.B);
			var grayE = RgbToGray(colE.R, colE.G, colE.B);
			var deltaDist = _measureDistE.Value - _measureDistB.Value;
			var deltaGray = grayE - grayB;
			var slope = Math.Abs(deltaDist) > Double.Epsilon ? deltaGray / deltaDist : 0.0;
			var rangeText = CreateProfileStatisticsText("範囲", idxB, idxE);
			return $"2点: A={idxB}, B={idxE}, ΔIdx={idxE - idxB}, ΔX={deltaDist:0.00}, Gray={grayB}->{grayE}, ΔGray={deltaGray}, 傾き={slope:0.000}; {rangeText}";
		}


		/// <summary>
		/// プロファイル距離をデータインデックスに変換します</summary>
		/// <param name="dist">プロファイル距離</param>
		/// <returns>データインデックス</returns>
		private int DistanceToIndex(double dist)
		{
			if (_colorDatas == null || _colorDatas.Length == 0 || _distDatas <= 0.0) return -1;
			var idx = (int)(dist / _distDatas * _colorDatas.Length);
			if (idx < 0) return 0;
			if (idx >= _colorDatas.Length) return _colorDatas.Length - 1;
			return idx;
		}


		/// <summary>
		/// プロファイルグラフ座標から距離とインデックスを取得します</summary>
		/// <param name="x">グラフX座標</param>
		/// <param name="y">グラフY座標</param>
		/// <param name="dist">プロファイル距離</param>
		/// <param name="idx">データインデックス</param>
		/// <returns>取得できた場合はtrue</returns>
		private bool TryGetProfilePosition(int x, int y, out double dist, out int idx)
		{
			dist = 0.0;
			idx = -1;
			if (_colorDatas == null || _colorDatas.Length == 0 || _matWorkPbox == null) return false;

			using (var inv = _matWorkPbox.Clone())
			{
				if (inv.IsInvertible == false) return false;
				inv.Invert();
				var pv = new PointF[] { new PointF(x, y) };
				inv.TransformPoints(pv);
				dist = pv[0].X;
				idx = DistanceToIndex(dist);
				return dist >= 0.0 && dist < _distDatas && idx >= 0 && idx < _colorDatas.Length;
			}
		}


		/// <summary>
		/// プロファイル2点測定位置を更新します</summary>
		/// <param name="dist">プロファイル距離</param>
		/// <param name="isDrag">ドラッグ更新の場合true</param>
		private void UpdateMeasureDistance(double dist, bool isDrag)
		{
			if (isDrag && _measureDistB != null)
			{
				_measureDistE = dist;
				return;
			}

			if (_lastDist == null)
			{
				_measureDistB = dist;
				_measureDistE = null;
				return;
			}

			_measureDistB = _lastDist;
			_measureDistE = dist;
		}


		/// <summary>
		/// キーが押されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void FormProfileViewer_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				// Escを押されたときには、インデックス表示をクリアする
				case Keys.Escape:
					labelIdxColInf.Text = "";
					_lastDist = null;
					_measureDistB = _measureDistE = null;
					UpdateProfileAnalysisText();
					_onMeasureRangeAction?.Invoke(null, null);
					DrawAllGraph();
					Refresh();
					e.Handled = true;
					break;
				case Keys.B:
					if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
						(Control.ModifierKeys & Keys.Shift) != Keys.Shift &&
						(Control.ModifierKeys & Keys.Alt) != Keys.Alt &&
						_onToggleProfileModeAction != null)
					{
						var invoker = _foregroundOwner != null && _foregroundOwner.IsDisposed == false && _foregroundOwner.IsHandleCreated ? _foregroundOwner : this;
						invoker.BeginInvoke(new Action(() => _onToggleProfileModeAction()));
						e.Handled = true;
					}
					break;
			}
		}



		/// <summary>
		/// マウスダウン時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void pboxGraph_MouseDown(object sender, MouseEventArgs e)
		{
			SetProfileCursorFromMouse(e, false);
		}



		/// <summary>
		/// マウスが移動したときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void pboxGraph_MouseMove(object sender, MouseEventArgs e)
		{
			if (_colorDatas == null || _colorDatas.Length == 0) return;

			// 左ボタンが押されているときには、クリック(MouseDown)が連続したものとして処理する。
			if ((e.Button & MouseButtons.Left)== MouseButtons.Left)
			{
				SetProfileCursorFromMouse(e, true);
			}
		}


		/// <summary>
		/// マウス位置からプロファイルカーソルと2点測定位置を更新します</summary>
		/// <param name="e">マウスイベント情報</param>
		/// <param name="isDrag">ドラッグ更新の場合true</param>
		private void SetProfileCursorFromMouse(MouseEventArgs e, bool isDrag)
		{
			if (_colorDatas == null || _colorDatas.Length == 0 || ((e.Button & MouseButtons.Left) != MouseButtons.Left)) return;
			if (TryGetProfilePosition(e.X, e.Y, out var dist, out var idx) == false) return;
			if (_lastDist != null && Math.Abs(_lastDist.Value - dist) < 0.000001) return;

			var previousLastDist = _lastDist;
			UpdateMeasureDistance(dist, isDrag);
			var measureDistB = _measureDistB;
			var measureDistE = _measureDistE;
			_onClickAction?.Invoke(dist);
			_lastDist = previousLastDist;
			_measureDistB = measureDistB;
			_measureDistE = measureDistE;
			_onMeasureRangeAction?.Invoke(measureDistB, measureDistE);
			_lastDist = previousLastDist;
			_measureDistB = measureDistB;
			_measureDistE = measureDistE;
			idx = DistanceToIndex(dist);
			if (idx < 0 || idx >= _colorDatas.Length) return;

			labelIdxColInf.Text = $"Idx={idx},  {ColorInfo(_colorDatas[idx])}";
			_lastDist = dist;
			UpdateProfileAnalysisText();
			DrawAllGraph();
			Refresh();
		}
	}
}
