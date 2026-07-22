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
		public FormProfileViewer(Color[] datas, string title, Action onClosing = null, double distance = 0.0, Icon ic = null, Action<double> onClick = null)
		{
			InitializeComponent();

			_matWorkPbox = new Matrix();
			SetData(datas, distance, false);
			_onClosingAction = onClosing;
			IsIgnoreClose = true;

			this.Text = title;
			if (ic != null) this.Icon = ic;
			_onClickAction = onClick;
			_lastDist = null;

			// キーを一旦フォームが受け取る
			this.KeyPreview = true;
		}


		/// <summary>
		/// ロード時の処理です
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void FormProfileViewer_Load(object sender, EventArgs e)
		{
			PP_Utility.SetTopMostWithoutFocus(this);
			ResetWorkPbox();
			SetData(_colorDatas, _distDatas);

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
			e.Cancel = IsIgnoreClose;
			if (IsIgnoreClose)
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

			// データ描画
			DrawData();
		}


		/// <summary>
		/// グラフデータを描画します</summary>
		public void DrawData()
		{
			if (_matWorkPbox == null) return;
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
			_graWorkPbox.Clear(Color.White);

			var ofsH = 0;
			if (rbtn120.Checked) ofsH = 120;
			else if (rbtn240.Checked) ofsH = 240;


			// グリッドラインの描画
			// グリッド(描画がチェックされていてかつ0-255データ表示が行われるときにはグリッドを描画する
			var cw = new CheckBox[] { cboxIsDrawColR, cboxIsDrawColG, cboxIsDrawColB, cboxIsDrawGray, cboxIsDrawColS };
			if (cboxIsDrawGridline.Checked && cw.Any(c => c.Checked))
			{
				// 横線
				for (int yy = 0; yy < 255; yy += 10)
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
			if (cboxIsDrawGridline.Checked || cboxIsDrawGridlineH.Checked)
			{
				// 縦線、データ100毎に(最大100本）
				using (var vPen = new Pen(Color.LightGray, 1f))
				{
					for (int xx = 0; xx < 10000; xx += 100)
					{
						var pov = new PointF[] { new PointF(xx, 0) };
						_matWorkPbox.TransformPoints( pov );
						_graWorkPbox.DrawLine(vPen, pov[0].X, (float)0, pov[0].X, (float)_bmpWorkPbox.Height);
					}
				}
			}

			// インデックスの表示
			if (_lastDist != null && _lastDist >= 0)
			{
				using (var iPen = new Pen(Color.Orange, 1f))
				{
					var pov = new PointF[] { new PointF((float)_lastDist.Value, 0) };
					_matWorkPbox.TransformPoints(pov);
					_graWorkPbox.DrawLine(iPen, pov[0].X, (float)0, pov[0].X, (float)_bmpWorkPbox.Height);
				}
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
			if (_graWorkPbox != null) _graWorkPbox.Dispose();
			if (_bmpWorkPbox != null) _bmpWorkPbox.Dispose();
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
			sfd.Filter = "PNGファイル(*.png)|*.png|JPGファイル(*.jpg)|*.jpg|ビットマップファイル(*.bmp)|*.bmp|TIFFファイル(*.tif)|*.tif|すべてのファイル(*.*)|*.*";
			//[ファイルの種類]ではじめに「PNGファイル」が選択されているようにする
			sfd.FilterIndex = 1;
			//タイトルを設定する
			sfd.Title = "保存ファイルを選択してください";
			//ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
			sfd.RestoreDirectory = true;
			//ダイアログを表示する
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				//OKボタンがクリックされたとき
				// 拡張子に強制的にあわせる場合には、ここにコードを入れること（現在はインデックスにより決定される）
				var it = new ImageFormat[] { ImageFormat.Png, ImageFormat.Jpeg, ImageFormat.Bmp, ImageFormat.Tiff };
				ImageFormat fmt = (sfd.FilterIndex > 0 && sfd.FilterIndex <= it.Length) ? it[sfd.FilterIndex - 1] : ImageFormat.Png;
				bmp.Save(sfd.FileName, fmt);
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
					DrawAllGraph();
					Refresh();
					break;
			}
		}



		/// <summary>
		/// マウスダウン時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void pboxGraph_MouseDown(object sender, MouseEventArgs e)
		{

			if (_colorDatas == null || _colorDatas.Length == 0 || ((e.Button & MouseButtons.Left)!= MouseButtons.Left) ) return;
			using (var inv = _matWorkPbox.Clone())
			{
				inv.Invert();
				var pv = new PointF[] { new PointF(e.X, e.Y) };
				inv.TransformPoints(pv);
				var dis = pv[0].X;
				var idx = (int)(pv[0].X / _distDatas * _colorDatas.Length);
				if (_lastDist != null && _lastDist.Value == dis) { return; }
				if (dis >= 0 && dis < _distDatas && idx< _colorDatas.Length)
				{
					if (_onClickAction != null) _onClickAction(dis);
					labelIdxColInf.Text = $"Idx={idx},  {ColorInfo(_colorDatas[idx])}";
					_lastDist = dis;
					DrawAllGraph();
					Refresh();
				}
			}
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
				pboxGraph_MouseDown(sender, e);
			}
		}
	}
}
