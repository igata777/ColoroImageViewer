using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


namespace IAPLibrary
{
	public partial class FormHsvColorDetection : Form
	{
		/// <summary>オリジナルイメージを保持します</summary>
		private Bitmap _originalImage;

		/// <summary>オリジナルファイル名を保持します</summary>
		private string _originalFileName;

		/// <summary>結果イメージを保持します</summary>
		private Bitmap _resultImage;

		// スケーラブルピクチャーボックスを保持します
		private ScalablePbox _scalPbox;
		private ScalablePbox _scalPboxResult;

		/// <summary>２つのピクチャーボックスの相互参照を禁止します。</summary>
		/// <remarks>参照により発生した動作ではtrueとして、相手からの参照を禁止します。</remarks>
		private bool _isPreventionReference = false;

		/// <summary>トラックバー→テキストボックスの辞書を保持します</summary>
		private Dictionary<TrackBar, TextBox> _dictTrackbarToTextbox;
		/// <summary>テキストボックス→トラックバーの辞書を保持します</summary>
		private Dictionary<TextBox, TrackBar> _dictTextboxToTrackbar;

		/// <summary>カーソルの位置を保持します</summary>
		private int _cursorX = 0;
		private int _cursorY = 0;
		private bool _isUseCursor { get { return cboxCursor.Checked; } }
		private int _cursorSize { get { return (int)nudCursorSize.Value; } }
		private Pen _cursorPen;

		/// <summary>オリジナルファイルの輝度平均値を保持します</summary>
		private double _originalImageBrMean;
		/// <summary>オリジナルファイルの輝度SD値を保持します</summary>
		private double _originalImageBrSD;

		/// <summary>リミットラベルのデフォルト値です</summary>
		private const string DefaultLimitLabel= @"default上限(SD比)";
		/// <summary>リミットラベルの反転時の文字列です</summary>
		private const string ReverseLimitLabel = @"default下限(SD比)";

		/// <summary>起動時引数を保持します</summary>
		private string[] _args;

		/// <summary>コンストラクタです</summary>
		/// <param name="args">起動時引数（先頭のみ有効、ファイル名の場合には画像ファイルとして読み込む）</param>
		public FormHsvColorDetection( string [] args )
		{
			InitializeComponent();
			_args = args;
			SetPbox();
			SetPboxResult();
		}


		/// <summary>ロード時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void FormTestProgTORAY_Load(object sender, EventArgs e)
		{
			_dictTextboxToTrackbar = new Dictionary<TextBox, TrackBar>(){
				{ textBoxBgnH, trackBarBgnH },{ textBoxEndH, trackBarEndH },
				{ textBoxBgnS, trackBarBgnS },{ textBoxEndS, trackBarEndS },
				{ textBoxBgnV, trackBarBgnV },{ textBoxEndV, trackBarEndV },
				{ textBoxBgnB, trackBarBgnB },{ textBoxEndB, trackBarEndB }
			};
			_dictTrackbarToTextbox = new Dictionary<TrackBar, TextBox>(){
				{ trackBarBgnH, textBoxBgnH },{ trackBarEndH, textBoxEndH },
				{ trackBarBgnS, textBoxBgnS },{ trackBarEndS, textBoxEndS },
				{ trackBarBgnV, textBoxBgnV },{ trackBarEndV, textBoxEndV },
				{ trackBarBgnB, textBoxBgnB },{ trackBarEndB, textBoxEndB }
			};

			_cursorPen = new Pen(pBoxCursorColor.BackColor, (int)nudCursorWidth.Value);

			// 検出タイプ(Hsv/Brightness)一方のみを有効にする
			foreach (var c in groupBoxHSV.Controls.OfType<Control>()) c.Enabled = radioButtonHsv.Checked;
			foreach (var c in groupBoxBrightness.Controls.OfType<Control>()) c.Enabled = radioButtonBrightness.Checked;

			// リミットラベル
			labelLimit.Text = checkBoxReverse.Checked ? ReverseLimitLabel : DefaultLimitLabel;

			//　起動時引数にファイルが指定されていれば、入力画像として扱う
			if (_args != null && _args.Length > 0 && File.Exists(_args[0]))
			{
				InputDataFile(_args);
			}
		}

		/// <summary>変換処理の実行を行います</summary>
		private void Execute()
		{
			if (_originalImage == null) return;
			if (radioButtonHsv.Checked)
			{
				int min, max;
				if (int.TryParse(textBoxBgnH.Text, out min) == false || int.TryParse(textBoxEndH.Text, out max) == false) return;
				var rangeH = new RangeValue<int>(min, max);
				if (int.TryParse(textBoxBgnS.Text, out min) == false || int.TryParse(textBoxEndS.Text, out max) == false) return;
				var rangeS = new RangeValue<int>(min, max);
				if (int.TryParse(textBoxBgnV.Text, out min) == false || int.TryParse(textBoxEndV.Text, out max) == false) return;
				var rangeV = new RangeValue<int>(min, max);
				if (_resultImage != null) _resultImage.Dispose();
				_resultImage = ColorBlockLib.CreateColorImageF(_originalImage, rangeH, rangeS, rangeV);
			}
			if (radioButtonBrightness.Checked)
			{
				int min, max;
				if (int.TryParse(textBoxBgnB.Text, out min) == false || int.TryParse(textBoxEndB.Text, out max) == false) return;
				var rangeB = new RangeValue<int>(min, max);
				if (_resultImage != null) _resultImage.Dispose();
				_resultImage = ColorBlockLib.CreateColorImage(_originalImage, rangeB );
			}
			SetPboxResult();
		}


		/// <summary>リミット値（上限・場合により下限）をセットします</summary>
		/// <returns>リミット値</returns>
		/// <remarks>結果はトラックバーおよびテキストボックスに反映されます。</remarks>
		private double CalcLimit()
		{
			// デフォルトのBrightness上限値決定
			var w = (double)nudSDrate.Value * _originalImageBrSD;
			var nval = _originalImageBrMean + ((checkBoxReverse.Checked) ? -w:  w);
			if (nval < 0) nval = 0;
			if (nval > 255) nval = 255;

			trackBarBgnB.Value = 0;
			trackBarEndB.Value = 255;
			if (checkBoxReverse.Checked)
			{
				trackBarBgnB.Value = (int)nval;
			}
			else{
				trackBarEndB.Value = (int)nval;
			}
			textBoxEndB.Text = trackBarEndB.Value.ToString();
			textBoxBgnB.Text = trackBarBgnB.Value.ToString();
			return nval;
		}


		/// <summary>ドラッグ＆ドロップされたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void PictBox_DragDrop(object sender, DragEventArgs e)
		{
			//ドロップされたデータがFileDrop型か調べる
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var strs = e.Data.GetData(DataFormats.FileDrop) as string[];
				InputDataFile(strs);
            }
//			Refresh();
		}


		/// <summary>
		/// 入力データを指定します</summary>
		/// <param name="strs">起動時またはドラッグアンドドロップで指定された文字列群。先頭のみ有効とする</param>
		private void InputDataFile(string[] strs)
		{
			if (strs == null || strs.Length != 1 && File.Exists(strs[0]) == false)
			{
				MessageBox.Show("画像ファイル１つのみを指定することができます", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			_originalImage = (Bitmap)Bitmap.FromFile(strs[0]);
			_originalFileName = strs[0];
			// 入力画像のPixelFormatは24bppに合わせる。
			if (_originalImage.PixelFormat != PixelFormat.Format24bppRgb)
			{
				var old = _originalImage;
				if (_originalImage.PixelFormat == PixelFormat.Format8bppIndexed) _originalImage = ColorBlockLib.ConvertPf8ToPf24Simple(_originalImage);
				else if (_originalImage.PixelFormat == PixelFormat.Format32bppArgb || _originalImage.PixelFormat == PixelFormat.Format32bppRgb) _originalImage = ColorBlockLib.ConvertPf32ToPf24Simple(_originalImage);
				else
				{
					MessageBox.Show("入力画像のタイプに対応していません。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				old.Dispose();
			}
			if (ColorBlockLib.CalcBrightnessImage(_originalImage, out _originalImageBrMean, out _originalImageBrSD) == false)
			{
				_originalImageBrMean = _originalImageBrSD = -1.0;
			}
			labelImageBr.Text = String.Format("輝度 平均:{0}, SD:{1}", _originalImageBrMean.ToString("##0.00"), _originalImageBrSD.ToString("##0.00"));

			// デフォルトのBrightness上（下）限値決定
			CalcLimit();

			SetPbox();

			Execute();
		}


		/// <summary> ピクチャーボックス内にドラッグされたときの処理を行います </summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void PictBox_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;
			//ドラッグされているデータがstring型に変換できるか調べ、
			//そうであればドロップ効果をMoveにする
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				if (((string[])e.Data.GetData(DataFormats.FileDrop)).Length > 0 ) 
				{

					e.Effect = DragDropEffects.Move;
				}
			}
		}




		/// <summary>
		/// スケーラブルピクチャーボックス(Main)をラップします。
		/// </summary>
		private void SetPbox()
		{

			if (_scalPbox == null)
			{
				_scalPbox = new ScalablePbox(this, pBoxOrgImage, 
					(mat, rate) =>
					{
						labelDisplayRate.Text = String.Format("{0:###0.0 %}", rate);
						if (_isPreventionReference == false)
						{
							_isPreventionReference = true;
							_scalPboxResult.SetRate(rate);
							_isPreventionReference = false;
						}
						Refresh();
						return false;
					},
					( x, y, col, isDown, isLeft )=>
					{
						if (isDown && isLeft)
						{
							if (_isUseCursor)
							{
								_cursorX = x;
								_cursorY = y;
								_scalPbox.DrawMainImage();
								_scalPboxResult.DrawMainImage();
							}
						}
						return false;
					},
					OnMouseMove,
					(rect, isAll) =>
					{
						if (_scalPboxResult != null)
						{
							var mat = _scalPbox.GetMat().Clone();
							var matr = _scalPboxResult.GetMat();
							if (mat.Elements[0] != matr.Elements[0] || mat.Elements[1] != matr.Elements[1] || mat.Elements[2] != matr.Elements[2] || mat.Elements[3] != matr.Elements[3] || mat.Elements[4] != matr.Elements[4] || mat.Elements[5] != matr.Elements[5])
							{
								if (_isPreventionReference == false)
								{
									_isPreventionReference = true;
									_scalPboxResult.SetMat(mat);
									_scalPboxResult.DrawMainImage();
									_isPreventionReference = false;
								}
							}
						}
						return false;
					},
					(gra, siz) =>
					{
						if (_isUseCursor )
						{
							gra.DrawEllipse(_cursorPen, _cursorX - _cursorSize, _cursorY - _cursorSize, _cursorSize * 2, _cursorSize * 2);
						}
					}
				);
			}
			//　ScalablePbox規定値・および設定
			_scalPbox.MaxRateMainImage = 100.0f;
			_scalPbox.MinRateMainImage = 0.10f;
			_scalPbox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;	// 固定
			if (_originalImage != null)
			{
				_scalPbox.ChangeMainImage(_originalImage);
				_scalPbox.SizeChanged();
			}
		}



		/// <summary>スケーラブルピクチャーボックス(Result)をラップします。</summary>
		private void SetPboxResult()
		{
			if (_scalPboxResult == null)
			{
				_scalPboxResult = new ScalablePbox(this, pBoxResult, null,
					(x, y, col, isDown, isLeft) =>
					{
						if (isDown && isLeft)
						{
							if (_isUseCursor)
							{
								_cursorX = x;
								_cursorY = y;
								_scalPbox.DrawMainImage();
								_scalPboxResult.DrawMainImage();
							}
						}
						return false;
					},
					null, 
					(rect,isAll) =>
					{
						var mat = _scalPbox.GetMat();
						var matr = _scalPboxResult.GetMat().Clone();
//						if (mat.Elements[0] != matr.Elements[0] || mat.Elements[1] != matr.Elements[1] || mat.Elements[2] != matr.Elements[2] || mat.Elements[3] != matr.Elements[3] || mat.Elements[4] != matr.Elements[4] || mat.Elements[5] != matr.Elements[5])
						if (mat.Elements.Select((el,i)=> el != matr.Elements[i]).Any()) 
						{
							if (_isPreventionReference == false)
							{
								_isPreventionReference = true;
								_scalPbox.SetMat(matr);
								_scalPbox.DrawMainImage();
								_isPreventionReference = false;
							}
						}
						return false;
					},
					(gra, siz) =>
					{
						if (_isUseCursor)
						{
							gra.DrawEllipse(_cursorPen, _cursorX - _cursorSize, _cursorY - _cursorSize, _cursorSize * 2, _cursorSize * 2);
						}
					}
				);
			}

			//　ScalablePbox規定値・および設定
			_scalPboxResult.MaxRateMainImage = 100.0f;
			_scalPboxResult.MinRateMainImage = 0.10f;
			_scalPboxResult.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;	// 固定
			if (_resultImage != null)
			{
				_scalPboxResult.ChangeMainImage(_resultImage, _scalPbox.GetMat().Clone());
				_scalPboxResult.SizeChanged();
			}
		}


		/// <summary>輝度(NTSC)を計算します </summary>
		/// <param name="r">赤要素(0-255)</param>
		/// <param name="g">緑要素(0-255)</param>
		/// <param name="b">青要素(0-255)</param>
		/// <returns>輝度(NTSC)値</returns>
		/// <remarks>NTSC方式　Brightness = ( 0.298912 * R + 0.586611 * G + 0.114478 * B )</remarks>
		private int CalBrightnessNTSC(int r, int g, int b)
		{
			return (int)( 0.298912 * r + 0.586611 * g + 0.114478 * b );
		}


		/// <summary>マウスカーソル移動時の処理です</summary>
		/// <param name="x">X位置</param>
		/// <param name="y">Y位置</param>
		/// <param name="col">色情報</param>
		/// <param name="isDown">マウスボタン状態（ダウン時真）未使用</param>
		private bool OnMouseMove(int x, int y, Color col, bool isDown)
		{
			int h, s, v;
			UtilityGraphics.HsvFromRgb(col.R, col.G, col.B, out h, out s, out v);
			if ( _originalImage != null)
			{
				labelMouseCursor.Text = String.Format("( {0,4},{1,4} )  RGB[{2,3},{3,3},{4,3}],  HSV[{5,3},{6,3},{7,3}] B(NTSC):{8,3}", x, y, col.R, col.G, col.B, h, s, v, CalBrightnessNTSC(col.R,col.G,col.B));
			}
			return false;
		}

		/// <summary>範囲指定テキストボックス群のフォーカスが外れたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void textBoxBgnV_Leave(object sender, EventArgs e)
		{
			var tbox = sender as TextBox;
			TrackBar trackbar = _dictTextboxToTrackbar[tbox];
			int w;
			if (int.TryParse(tbox.Text, out w) == true && trackbar.Minimum<= w && w<= trackbar.Maximum )
			{
				trackbar.Value = w;
				Execute();
			}
			else
			{
				tbox.Text = ((int)trackbar.Value).ToString();
			}
		}


		/// <summary>範囲指定テキストボックス群のキー入力時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void textBoxBgnV_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData != Keys.Enter) return;
			var tbox = sender as TextBox;
			TrackBar trackbar = _dictTextboxToTrackbar[tbox];
			int w;
			if (int.TryParse(tbox.Text, out w) == true && trackbar.Minimum <= w && w <= trackbar.Maximum)
			{
				trackbar.Value = w;
				Execute();
			}
			else
			{
				tbox.Text = ((int)trackbar.Value).ToString();
			}
		}

		/// <summary>範囲指定トラックバーがスライドしたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void trackBarBgnV_Scroll(object sender, EventArgs e)
		{
			var trackbar = sender as TrackBar;
			var textbox = _dictTrackbarToTextbox[trackbar];
			textbox.Text = ((int)trackbar.Value).ToString();
			Execute();
		}

		/// <summary>カーソル変更時の処理です</summary>
		private void OnChangeCorsor()
		{
			if (_scalPbox != null && _originalImage != null)
			{
				_scalPbox.DrawMainImage();
			}
			if (_scalPboxResult != null && _resultImage != null)
			{
				_scalPboxResult.DrawMainImage();
			}
		}


		/// <summary>カーソル使用チェックボックスが変更されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void cboxCursor_CheckedChanged(object sender, EventArgs e)
		{
			var cbx = sender as CheckBox;
			foreach (var c in groupBoxCorsor.Controls.OfType<Control>().Where(w => w != cbx)) c.Enabled = cbx.Checked;
			OnChangeCorsor();
		}

		/// <summary>カーソルサイズ（円半径）が変更されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void nudCursorSize_ValueChanged(object sender, EventArgs e)
		{
			OnChangeCorsor();
		}

		/// <summary>カーソル線の太さが変更されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void nudCursorWidth_ValueChanged(object sender, EventArgs e)
		{
			_cursorPen = new Pen(pBoxCursorColor.BackColor, (int)nudCursorWidth.Value);
			OnChangeCorsor();
		}

		/// <summary>カーソルカラーがクリックされたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void pBoxCursorColor_Click(object sender, EventArgs e)
		{
			var cdlg = new ColorDialog();
			cdlg.Color = pBoxCursorColor.BackColor;
			if (cdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				pBoxCursorColor.BackColor = cdlg.Color;
				_cursorPen = new Pen(pBoxCursorColor.BackColor, (int)nudCursorWidth.Value);
				OnChangeCorsor();
				Refresh();
			}
		}

		/// <summary>元画像または結果画像をダブルクリックしたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void pBoxOrgImage_DoubleClick(object sender, EventArgs e)
		{
			// 画面の大きさにフィット(Fit)させます
			_scalPbox.Zoom();
			Refresh();
		}



		/// <summary>元画像で「フィット」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void フィットToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 画面の大きさにフィット(Fit)させます
			_scalPbox.Zoom();
			Refresh();

		}

		/// <summary>元画像で「等倍表示」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 等倍表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 等倍(100%)表示にします
			_scalPbox.OriginalSize();
		}

		/// <summary>元画像で「倍率指定」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 倍率指定ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var frm = new SetingsRateForm(_scalPbox.DispRate);
			var ret = frm.ShowDialog();
			if (ret == System.Windows.Forms.DialogResult.OK)
			{
				_scalPbox.SetRate(frm.Rate);
				_scalPbox.DrawMainImage();
			}
		}

		/// <summary>元画像で「画像データを保存」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 画像データを保存ToolStripMenuItem_Click(object sender, EventArgs e)
		{

			if (_originalImage == null) return;

			//SaveFileDialogクラスのインスタンスを作成
			SaveFileDialog sfd = new SaveFileDialog();

			//はじめのファイル名を指定する
			sfd.FileName = "OriginalImage";
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
				_originalImage.Save(sfd.FileName, fmt);
			}
		}


		/// <summary>「画像データをクリップボードにコピー」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 画像データをクリップボードにコピーCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_originalImage == null) return;
			Clipboard.SetDataObject(_originalImage);
		}


		/// <summary>元画像で「画像データを別画面で表示」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 画像データを別画面で表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ( _originalImage == null) return;
			var frm = new FormColorImageS(_originalImage, "オリジナル画像", false, -1, -1, null, null, this.Icon);
			frm.Show();
		}

		/// <summary>元画像で「表示情報をクリップボードへ」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 表示情報をクリップボードへToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var rect = _scalPbox.GetDispRect();
			FormColorImageS.DispInfoToClipBoard(rect.X, rect.Y, _scalPbox.DispRate, _scalPbox.BasePbox.Width, _scalPbox.BasePbox.Height);
		}

		/// <summary>元画像で「クリップボード表示情報インポート」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void クリップボード表示情報インポートToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int tx, ty, width, height;
			float rate;

			// クリップボードから表示情報データを取得。
			if (FormColorImageS.DispInfoFromClipBoard(out tx, out ty, out rate, out width, out height, out var dic) == true)
			{
				// まず、ウィンドウの大きさを合わせる
				var newWidth = this.Width + width - _scalPbox.BasePbox.Width;
				var newHeight = this.Height + height - _scalPbox.BasePbox.Height;
				if (newWidth <= 10 || newHeight <= 10 || rate < 0.01 || rate > 1000.0)
				{
					MessageBox.Show(String.Format("不適当な表示情報であるため、無視します  newWidth={0}, newHeight={1}", newWidth, newHeight), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				this.Width = newWidth;
				this.Height = newHeight;
				_scalPbox.MoveDispPos(tx, ty, 0, 0, rate);		// 元画像の(tx,ty)を倍率rateで(0,0)から表示
				_scalPbox.DrawMainImage();
			}
			else
			{
				MessageBox.Show("不適当な表示情報であるため、無視します", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}


		/// <summary>
		/// 「表示枠情報を別の画像情報画面に表示」が指定されたときの処理です
		/// </summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 表示枠情報を別の画像情報画面に表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_originalImage == null) return;
			var bmp = _originalImage.Clone(new Rectangle(0, 0, _originalImage.Width, _originalImage.Height), PixelFormat.Format24bppRgb);
			using (var gra = Graphics.FromImage(bmp))
			using (var pen = new Pen(Color.Red, 3.0f))
			{
				// 表示領域の太線はPenAlignmentは外側に太くする
				pen.Alignment = PenAlignment.Outset;
				gra.DrawRectangle(pen, _scalPbox.GetDispRect());
			}
			var frm = new FormColorImageS(bmp, this.Text + "表示領域", false, -1, -1, null, null, this.Icon);
			frm.Show();

		}

		/// <summary>結果画像で「画像データを保存」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 画像データを保存ToolStripMenuItemR_Click(object sender, EventArgs e)
		{
			if (_resultImage == null) return;

			//SaveFileDialogクラスのインスタンスを作成
			SaveFileDialog sfd = new SaveFileDialog();

			//はじめのファイル名を指定する
			sfd.FileName = "ResultImage";
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
				_resultImage.Save(sfd.FileName, fmt);
			}

		}

		/// <summary>結果画像で「画像データをクリップボードにコピー」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報  画像データを別画面で表示</param>
		private void 画像データをクリップボードにコピーToolStripMenuItemR_Click(object sender, EventArgs e)
		{
			if (_resultImage == null) return;
			Clipboard.SetDataObject(_resultImage);
		}


		/// <summary>結果画像で「画像データを別画面で表示」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 画像データを別画面で表示ToolStripMenuItemR_Click(object sender, EventArgs e)
		{
			if (_resultImage == null) return;
			var frm = new FormColorImageS(_resultImage, "結果画像", false, -1, -1, null, null, this.Icon);
			frm.Show();
		}

		/// <summary>ラジオボタンHSVに変化があったときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		/// <remarks>Brightnessに変化があったときも連動するので、HSVのみ変化をチェック・処理します</remarks>
		private void radioButtonHsv_CheckedChanged(object sender, EventArgs e)
		{
			// 検出タイプ(Hsv/Brightness)一方のみを有効にする
			foreach (var c in groupBoxHSV.Controls.OfType<Control>()) c.Enabled = radioButtonHsv.Checked;
			foreach (var c in groupBoxBrightness.Controls.OfType<Control>()) c.Enabled = radioButtonBrightness.Checked;
			Execute();
		}


		/// <summary>デフォルトSD比が変更されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void numericUpDownSDrate_ValueChanged(object sender, EventArgs e)
		{
			var w = (double)((NumericUpDown)sender).Value * _originalImageBrSD;
			var nval = _originalImageBrMean + ((checkBoxReverse.Checked) ? (-w) : w);
			if( nval < 0 ) nval = 0;
			if( nval > 255 ) nval = 255;
			trackBarBgnB.Value = 0;
			trackBarEndB.Value = 255;
			if (checkBoxReverse.Checked)
			{
				trackBarBgnB.Value = (int)nval;
			}
			else
			{
				trackBarEndB.Value = (int)nval;
			}
			textBoxEndB.Text = trackBarEndB.Value.ToString();
			textBoxBgnB.Text = trackBarBgnB.Value.ToString();
			Execute();

		}


		/// <summary>分布CSV→クリップボードボタンがクリックされたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private unsafe void buttonDistrCopy_Click(object sender, EventArgs e)
		{
			if (_originalImage == null) return;
			if (_originalImage.PixelFormat != PixelFormat.Format24bppRgb) return;
			int[] tB = new int[256];
			int[] tH = new int[360];
			int[] tS = new int[256];
			int[] tV = new int[256];

			int width = _originalImage.Width;
			int height = _originalImage.Height;
			var img24 = _originalImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, _originalImage.PixelFormat);
			for (int y = 0; y < height; y++)
			{
				byte* ptr24 = (byte*)img24.Scan0 + img24.Stride * y;
				for (int x = 0; x < width; x++)
				{
					int hh, ss, vv;
					UtilityGraphics.HsvFromRgb(ptr24[2], ptr24[1], ptr24[0], out hh, out ss, out vv);
					tH[hh]++;
					tS[ss]++;
					tV[vv]++;
					int br = (int)(0.298912 * ptr24[2] + 0.586611 * ptr24[1] + 0.114478 * ptr24[0]);
					tB[br]++;
					ptr24 += 3;
				}
			}

			_originalImage.UnlockBits(img24);

			// クリップボードにコピー
			var strs = new StringBuilder();
			strs.Append(String.Format("ファイル【{0}】の色構成分布(HSV + 輝度）\n", Path.GetFileName(_originalFileName)));
			strs.Append("Val\tH（色相）\tS（彩度）\tV（明度）\tB（輝度）\n");
			for (int i = 0; i < 256; i++)
			{
				strs.Append(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", i, tH[i], tS[i], tV[i], tB[i]));
			}
			for (int i = 256; i < 360; i++)
			{
				strs.Append(String.Format("{0}\t{1}\n", i, tH[i]));
			}

			Clipboard.SetDataObject(strs.ToString(), true);


		}

		/// <summary>「欠点低輝度」チェックボックス変更時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void checkBoxReverse_CheckedChanged(object sender, EventArgs e)
		{
			// リミットラベル
			labelLimit.Text = checkBoxReverse.Checked ? ReverseLimitLabel : DefaultLimitLabel;
			CalcLimit();
			Execute();
		}

	}




}
