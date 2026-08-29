using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;



// 2018/10/23 拡大・縮小メソッド追加。(アプリケーションからCtrl + ±　キーで拡大／縮小に対応）。
// 2017/12/25 （ScalablePboxの機能追加に合わせ）従来のFit方法(全面)に縦Fit, 横Fitを追加  
// 2018/08/15 プロファイル機能の追加。
// 2018/08/17 表示情報を拡張するため、フォーマットをプラグイン文字列設定値と合わせる
// 2018/09/29 1. HSV表示　　2.Ctrl+↑　等で不透明度を変更可能とした　　3. ショートカット Ctrl+Sなどで画像の保存など
// 2019/03/15 画像サイズを左上に表示
// 2019/11/14 カーソル上の色情報・YCbCr値を表示に追加 labelMouseCursor3
// 2019/11/22 設定に BackColor　背景（画像のない部分）の色を追加。
// 2020/04/13 プロファイル表示不具合対応、プロファイル位置カーソル機能追加、Ctrl+SPCでプロファイル再表示、Ctrl+クリック時自動でラインモードにする
// 2021/03/19 プロファイルからのCSV出力でB要素値がR要素になっていた不具合を修正　OutCsvfilePixels()内
// 2021/03/26 ボックス・ラインモードで対象ピクセルをCSVファイルだけでなく、クリップボードコピーにも対応。
// 2021/03/26 ボックス・ラインモード時に対象ピクセルの平均色を表示
// 2021/03/26 ボックス・ラインモード時にプロファイル範囲（始点・終点または全体）を移動可能とした。
// 2021/03/28 プロファイル移動モードの不具合修正。＋　OnClick, OnMouseMoveでのリターン値をtrueにして、再描画指定をコメントアウト。→影響様子見。
// 2021/05/03 Close時に _scalePbox を Disposeする様修正
// 2022/10/05 ウィンドウ位置を Ctrl+ALT+上下左右キー　で1ピクセル単位で移動できるようにした
// 2022/10/05 Ctrl+Shift+Cで取得する情報にウィンドウ位置を加え、 Ctrl+ShiftALT+Vでウィンドウ位置も反映するようにした。(Altなしの Ctrl+Shift+Vは今まで通り位置は移動しない）
// 2022/10/19 ファイル保存時のデフォルトファイル名を "Image"から　フォームのText の内容に変更d。
// 2022/10/19 フォームテキスト（タイトル）の変更 （Ctrl+ t)
// 2023/12/08 プロファイル指定時のカーソル表示（表示サイズを固定とした）とデータ不整合の修正
// 2025/07/07 .NET8版で（45度から225度までの直線）プロファイルが表示されない不具合を修正。
// 2025/11/18 Drag & Drop で　他の画像をサブウィンドウで開く　動作を追加。


namespace ColorImageViewProject
{
	/// <summary>
	/// プロファイルダイアログの前面表示モードです</summary>
	public enum ProfileViewerForegroundMode
	{
		/// <summary>画像情報画面の前面に表示します</summary>
		ParentForeground,

		/// <summary>常に最前面に表示します</summary>
		AlwaysTopMost,

		/// <summary>通常のウィンドウとして表示します</summary>
		Normal
	}


	/// <summary>カラー情報表示用フォームクラスです</summary>
	public partial class FormColorImageS : Form
	{
		/// <summary>ScalablePboxのインスタンスを保持します</summary>
		private ScalablePbox _scalePbox;

		/// <summary>表示対象画像(24bit)を保持します</summary>
		private Bitmap _sourceImage;

		/// <summary>ドラッグドロップ画像を読み込み中かどうかを保持します</summary>
		private bool _isOpeningDroppedImages;

		/// <summary>平均色画像表示メニューを保持します</summary>
		private ToolStripMenuItem _averageColorImageToolStripMenuItem;

		/// <summary>キー入力待ちするか否かの情報を保持します</summary>
		private bool _isHitAnyKey;
		/// <summary>設定情報を保持します</summary>
		private ColImageSetteings _settings;
		
		/// <summary>初期表示情報文字列を保持します。初期表示後はnullでクリアします</summary>
		private string _initDispInfoString = null;
		/// <summary>なぜかラベルの表示がずれるための暫定対応。(調整用）labesDispInfo </summary>
		private Point _debLabelPos;

		/// <summary>フォームをクローズするときに呼び出されるアクションを保持します。</summary>
		private Action _actOnClosed;

		/// <summary>フォームを表示したときに呼び出されるアクションを保持します。</summary>
		private Action _actOnOpened;

		// <param name="isFitWidth"></param>
		/// <summary>画像フィット情報を保持します。　横フィットを指定：true, 縦フィット：false, 全面：null</summary>
		private bool? _isFitImageWidth;


		/// <summary>プロファイル・ライン用ビューワーフォームを保持します</summary>
		private FormProfileViewer _formProfViewerLine;
		/// <summary>垂直プロファイル用（水平方向に計算）ビューワーフォームを保持します</summary>
		private FormProfileViewer _formProfViewerBoxH;
		/// <summary>水平プロファイル用（垂直方向に計算）ビューワーフォームを保持します</summary>
		private FormProfileViewer _formProfViewerBoxV;

		/// <summary>Lineモード・簡易距離計測モードポイント（開始点）</summary>
		private Point? _measureModePointB = null;

		/// <summary>Lineモード・簡易距離計測モードポイント（終了点）</summary>
		private Point? _measureModePointE = null;

		/// <summary>Lineモード／BoxモードのときのCtrlモード判定(Ctrlが押されたまま）を示します。</summary>
		private bool _isMeasureMode = false;

		
		/// <summary>プロファイルモード（null:なし、true:ラインモード、 false:ボックスモード）を保持します</summary>
		private bool? _isProfModeLine;

		/// <summary>プロファイルモード切替時に指定範囲を保持するかどうかを保持します</summary>
		private bool _isKeepProfileRangeOnModeChange;

		/// <summary>プロファイル範囲移動モードフラグです。　全体を移動：true, Bgn側のみ移動:false, 移動モードでない場合nullを示すフラグ</summary>
		/// <remarks>PointEを移動するのは通常のMeasureModeで対応するため、本フラグはnullの状態とする</remarks>
		private bool? _isProfPosMovAll;
		/// <summary>_isProPosMoveAllがnullでなくなったときのクリック位置を保存します（_isProfPosMoveAllがnullの場合には無効）</summary>
		private Point _profPosMovClickPos;
		/// <summary>_isProPosMoveAllがnullでなくなったときの始点位置を保存します（_isProfPosMoveAllがnullの場合には無効）</summary>
		private Point _profPosMovOrgPosB;
		/// <summary>_isProPosMoveAllがnullでなくなったときの終点位置を保存します（_isProfPosMoveAllがnullの場合には無効）</summary>
		private Point _profPosMovOrgPosE;

		/// <summary>プロファイル更新を最後に行った時刻を保持します</summary>
		private DateTime _lastProfileUpdateTime = DateTime.MinValue;

		/// <summary>プロファイル更新を保留しているかどうかを保持します</summary>
		private bool _isProfileUpdateDeferred;

		/// <summary>ドラッグ中のプロファイル更新間隔をミリ秒で保持します</summary>
		private const int ProfileDragUpdateIntervalMilliseconds = 100;




		/// <summary>フォームの不透明度を決定する番号を保持します。(0-7) 0:30%, 1:40% ・・7:100%</summary>
		private int _opacityNumber;
		/// <summary>フォームの不透明度を決定する番号最大数＝分割数</summary>
		const	int MaxOpcityNumber = 9;
		/// <summary>フォームの不透明度最小 Opcity値（不透明度）</summary>
		const double MinOpcityValue = 0.1;

		/// <summary>ラインプロファイル用カーソルオフセット(距離）を保持します</summary>
		/// <remarks>ラインは斜めがあるので距離（double型)で表す</remarks>
		private double? _pcLine;

		/// <summary>ラインプロファイル用範囲開始オフセットを保持します</summary>
		private double? _pcLineRangeB;

		/// <summary>ラインプロファイル用範囲終了オフセットを保持します</summary>
		private double? _pcLineRangeE;

		/// <summary>ボックス水平プロファイル用カーソルオフセットを保持します</summary>
		private int? _pcBoxH;

		/// <summary>ボックス水平プロファイル用範囲開始オフセットを保持します</summary>
		private int? _pcBoxHRangeB;

		/// <summary>ボックス水平プロファイル用範囲終了オフセットを保持します</summary>
		private int? _pcBoxHRangeE;

		/// <summary>ボックス垂直プロファイル用カーソルオフセットを保持します</summary>
		private int? _pcBoxV;

		/// <summary>ボックス垂直プロファイル用範囲開始オフセットを保持します</summary>
		private int? _pcBoxVRangeB;

		/// <summary>ボックス垂直プロファイル用範囲終了オフセットを保持します</summary>
		private int? _pcBoxVRangeE;


		/// <summary>コンストラクタです</summary>
		/// <param name="bmp">表示対象画像</param>
		/// <param name="title">タイトル</param>
		/// <param name="isHitAnyKey">入力待ち</param>
		/// <param name="posX">初期表示位置 x</param>
		/// <param name="posY">初期表示位置 y</param>
		/// <param name="initDispInfoString">初期表示情報文字列（クリップボード保存と同じフォーマット）</param>
		/// <param name="setdata">設定情報</param>
		/// <param name="icon">Iconの指定</param>
		/// <param name="onClosed">フォームをクローズするときに呼び出されるアクションを指定します</param>
		/// <param name="isFitWidth">画像フィット情報（初期表示情報がないときのみ有効）　横フィットを指定：true, 縦フィット：false, 全面：null（無指定時）</param>
		/// <param name="isProfileFunction">プロファイル機能関連のコントロールの有効・無効（デフォルト有効）</param>
		/// <param name="isTakeOwnership">指定画像の所有権を受け取るかどうか</param>
		/// <param name="onOpened">フォームを表示したときに呼び出されるアクションを指定します</param>
		/// <remarks>isTakeOwnershipがfalseの場合、指定画像・設定値などの指定パラメータはコンストラクタ内でクローンを生成するため、指定後は呼び出し側で破棄可能。</remarks>
//		[SupportedOSPlatform("windows")]
		public FormColorImageS(Bitmap bmp, string title, bool isHitAnyKey = false, int posX = -1, int posY = -1, string initDispInfoString= null, ColImageSetteings setdata= null, Icon icon= null, Action　onClosed = null, bool? isFitWidth= null, bool isProfileFunction= true, bool isTakeOwnership = false, Action onOpened = null )
		{
			InitializeComponent();
			InitializeClipboardImageOpenMenuItem();
			InitializeAverageColorImageMenuItem();
			FormClosing += FormColorImageS_FormClosing;

			_debLabelPos = new Point(this.Width - labelDispInfo.Location.X, this.Height - labelDispInfo.Location.Y);
			
			_settings = (setdata != null)? (ColImageSetteings)setdata.Clone() : new ColImageSetteings();
			this.Text = title;
			_sourceImage = isTakeOwnership ? bmp : (Bitmap)bmp.Clone();
			_isHitAnyKey = isHitAnyKey;

			// 位置が指定されていれば、指定位置に表示する
			if (posX >= 0 && posY >= 0 )
			{
				StartPosition = FormStartPosition.Manual;
				this.Location = new Point(posX, posY);
			}
			_initDispInfoString = initDispInfoString;

			if (icon != null) this.Icon = (Icon)icon.Clone();

			_actOnClosed = onClosed;
			_actOnOpened = onOpened;

			_isFitImageWidth = isFitWidth;

			// 24ビット画像で無い場合には、プロファイル機能は禁止する。（現状24ビット画像のみ対応しているため）
			if (bmp.PixelFormat != PixelFormat.Format24bppRgb) gboxProfileMode.Enabled = false;

			// プロファイル機能コントロールの有効・無効
			tboxProfInfo.Visible = labelProfSize.Visible = gboxProfileMode.Visible = isProfileFunction;

			// 
			_opacityNumber = MaxOpcityNumber;     // 100% = this.Opacity = _opacityNumber * (1.0-MinOpcityValue)/MaxOpcityNumber + MinOpcityValue; 
			ClearProfileCursorMarks();

		}


		/// <summary>フォームの生成と表示を行います</summary>
		/// <param name="bmp">表示対象画像</param>
		/// <param name="title">タイトル</param>
		/// <param name="isHitAnyKey">入力待ち</param>
		/// <param name="posX">初期表示位置 x</param>
		/// <param name="posY">初期表示位置 y</param>
		/// <param name="isOffsetPos">前回とのオフセット位置</param>
		/// <param name="initDispInfoString">初期表示情報文字列（クリップボード保存と同じフォーマット）</param>
		/// <param name="setdata">設定情報</param>
		/// <param name="icon">Iconの指定</param>
		static public void CreateForm(Bitmap bmp, string title, bool isHitAnyKey = false, int posX = -1, int posY = -1, string initDispInfoString = null, ColImageSetteings setdata = null, Icon icon = null)
		{
			var frm = new FormColorImageS( bmp, title, isHitAnyKey, posX, posY, initDispInfoString, setdata, icon );
			if (isHitAnyKey) frm.ShowDialog();
			else frm.Show();
		}


		/// <summary>
		/// クリップボード画像を新しい表示フォームとして開きます</summary>
		/// <param name="owner">メッセージ表示時のオーナー</param>
		/// <param name="icon">表示フォームに設定するアイコン</param>
		/// <param name="onClosed">表示フォームクローズ時に呼び出すアクション</param>
		/// <param name="onOpened">表示フォームオープン時に呼び出すアクション</param>
		/// <returns>画像を開けた場合はtrue</returns>
		[SupportedOSPlatform("windows")]
		static public bool OpenClipboardImage(Form owner, Icon icon, Action onClosed = null, Action onOpened = null)
		{
			Bitmap bmp = null;
			try
			{
				if (Clipboard.ContainsImage() == false)
				{
					ShowClipboardImageMessage(owner, "クリップボードに画像がありません。", MessageBoxIcon.Information);
					return false;
				}

				using (var image = Clipboard.GetImage())
				{
					if (image == null)
					{
						ShowClipboardImageMessage(owner, "クリップボード画像を取得できませんでした。", MessageBoxIcon.Warning);
						return false;
					}

					bmp = CreateBitmapFromClipboardImage(image);
				}

				var frm = new FormColorImageS(bmp, CreateClipboardImageTitle(), false, -1, -1, null, null, icon, onClosed, null, true, true, onOpened);
				bmp = null;
				frm.Show();
				onOpened?.Invoke();
				return true;
			}
			catch (Exception ex)
			{
				ShowClipboardImageMessage(owner, $"クリップボード画像を開けませんでした。 Message={ex.Message}", MessageBoxIcon.Error);
				return false;
			}
			finally
			{
				bmp?.Dispose();
			}
		}


		/// <summary>
		/// クリップボード画像関連のメッセージを表示します</summary>
		/// <param name="owner">メッセージ表示時のオーナー</param>
		/// <param name="message">メッセージ</param>
		/// <param name="icon">メッセージアイコン</param>
		private static void ShowClipboardImageMessage(Form owner, string message, MessageBoxIcon icon)
		{
			const string title = "クリップボード画像";
			if (owner != null && owner.IsDisposed == false && owner.Visible)
			{
				MessageBox.Show(owner, message, title, MessageBoxButtons.OK, icon);
				return;
			}

			MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
		}


		/// <summary>
		/// クリップボード画像から表示用Bitmapを生成します</summary>
		/// <param name="image">クリップボードから取得した画像</param>
		/// <returns>表示用Bitmap</returns>
		private static Bitmap CreateBitmapFromClipboardImage(Image image)
		{
			var bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
			try
			{
				if (image.HorizontalResolution > 0 && image.VerticalResolution > 0)
				{
					bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
				}
			}
			catch
			{
				// 解像度情報が不正な画像でも表示は継続する。
			}

			using (var g = Graphics.FromImage(bmp))
			{
				g.Clear(Color.White);
				g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
			}

			return bmp;
		}


		/// <summary>
		/// クリップボード画像用の表示タイトルを生成します</summary>
		/// <returns>表示タイトル</returns>
		private static string CreateClipboardImageTitle()
		{
			return $"ClipboardImage_{DateTime.Now:yyyyMMdd_HHmmss_fff}";
		}


		/// <summary>
		/// 指定された画像ファイル群を非同期で読み込み、表示フォームとして開きます</summary>
		/// <param name="owner">進捗フォームのオーナー</param>
		/// <param name="filePaths">画像ファイルパス群</param>
		/// <param name="icon">表示フォームに設定するアイコン</param>
		/// <param name="onClosed">表示フォームクローズ時に呼び出すアクション</param>
		/// <param name="onOpened">表示フォームオープン時に呼び出すアクション</param>
		/// <returns>画像オープン結果</returns>
		[SupportedOSPlatform("windows")]
		static public async Task<ImageFilesOpenResult> OpenImageFilesAsync(Form owner, IEnumerable<string> filePaths, Icon icon, Action onClosed = null, Action onOpened = null)
		{
			var files = filePaths?.Where(w => String.IsNullOrWhiteSpace(w) == false).ToArray() ?? Array.Empty<string>();
			var result = new ImageFilesOpenResult();
			var failureList = new List<(string FileName, string Reason)>();
			if (files.Length < 1)
			{
				return result;
			}

			using (var cancelSource = new CancellationTokenSource())
			using (var progressForm = new FormImageLoadProgress())
			{
				progressForm.CancelRequested += (sender, e) => cancelSource.Cancel();
				progressForm.SetProgress(0, files.Length, "");
				ShowImageLoadProgressForm(progressForm, owner);

				for (var i = 0; i < files.Length; i++)
				{
					if (cancelSource.IsCancellationRequested)
					{
						result.IsCanceled = true;
						break;
					}

					var filePath = files[i];
					var displayName = Path.GetFileName(filePath);
					progressForm.SetCurrentFile(i + 1, files.Length, displayName);

					var loadResult = await Task.Run(() =>
					{
						var isSuccess = UtilityGraphics.TryLoadBitmapFilePF(filePath, PixelFormat.Format24bppRgb, out var bmp, out var fileName, out var failureReason);
						return (IsSuccess: isSuccess, Bmp: bmp, FileName: fileName, FailureReason: failureReason);
					});

					if (cancelSource.IsCancellationRequested)
					{
						loadResult.Bmp?.Dispose();
						result.IsCanceled = true;
						break;
					}

					if (loadResult.IsSuccess)
					{
						var bmp = loadResult.Bmp;
						loadResult.Bmp = null;
						try
						{
							progressForm.SetCurrentFile(i + 1, files.Length, $"{loadResult.FileName} 表示準備中");
							var frm = new FormColorImageS(bmp, loadResult.FileName, false, -1, -1, null, null, icon, onClosed, null, true, true, onOpened);
							bmp = null;
							frm.Show();
							onOpened?.Invoke();
							result.OpenCount++;
						}
						finally
						{
							bmp?.Dispose();
						}
					}
					else
					{
						failureList.Add((loadResult.FileName, loadResult.FailureReason));
					}

					progressForm.SetProgress(i + 1, files.Length, loadResult.FileName);
				}

				progressForm.Close();
			}

			result.Failures = failureList.ToArray();
			return result;
		}


		/// <summary>
		/// 画像読み込み進捗フォームを表示します</summary>
		/// <param name="progressForm">進捗フォーム</param>
		/// <param name="owner">進捗フォームのオーナー</param>
		private static void ShowImageLoadProgressForm(FormImageLoadProgress progressForm, Form owner)
		{
			if (owner != null && owner.IsDisposed == false && owner.Visible)
			{
				progressForm.Show(owner);
			}
			else
			{
				progressForm.StartPosition = FormStartPosition.CenterScreen;
				progressForm.Show();
			}
			progressForm.Refresh();
		}


		/// <summary>
		/// 複数画像読み込み結果を表示します</summary>
		/// <param name="result">画像オープン結果</param>
		private static void ShowImageLoadResult(ImageFilesOpenResult result)
		{
			if (result == null) return;
			if (result.OpenCount > 0 && result.IsCanceled == false && (result.Failures == null || result.Failures.Length == 0)) return;

			var failures = result.Failures ?? Array.Empty<(string FileName, string Reason)>();
			var message = UtilityGraphics.CreateLoadBitmapFilesMessage(result.OpenCount, failures, result.IsCanceled);
			if (String.IsNullOrWhiteSpace(message)) message = "有効な画像ファイルが見つかりませんでした。";

			var icon = result.OpenCount == 0 && result.IsCanceled == false ? MessageBoxIcon.Error : MessageBoxIcon.Warning;
			var title = result.OpenCount == 0 && result.IsCanceled == false ? "画像読み込みエラー" : "画像読み込み結果";
			MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
		}


		/// <summary>フォームロード時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void FormColorImageS_Load(object sender, EventArgs e)
		{
			_scalePbox = new ScalablePbox(this, pboxColImage, (mat, rate) =>
			{
				labelDisplayRate.Text = String.Format("{0:###0.0 %}", rate);
				Refresh();
				return false;
			},
				OnPboxClick,
				OnMouseMove,
				null,
				OnPaintPbox
			);
			//　ScalablePbox規定値・および設定
			_scalePbox.MaxRateMainImage = 100.0f;
			_scalePbox.MinRateMainImage = 0.125f;
			_scalePbox.BasePbox.BackColor = _settings != null ? _settings.BackColor : Color.Black ;
			_scalePbox.InterpolationMode = _settings!= null ? _settings.MainInterpolationMode: InterpolationMode.NearestNeighbor;
			_scalePbox.InterpolationModeReduce = _settings != null ? _settings.MainInterpolationModeReduce : InterpolationMode.HighQualityBilinear;
			_scalePbox.ChangeMainImage(_sourceImage, false);
			_scalePbox.SizeChanged();

			lblImageSize.Text= _sourceImage.Size.ToString();

			// 初期表示状態の指定
			var iniDisp = false;
			if (_initDispInfoString != null )
			{
				if (ParseDispInfoString(_initDispInfoString, out float xx, out float yy, out float rate, out int width, out int height, out Dictionary<string, string> dic))
				{
					// 取得した情報に合わせて、画面や表示状態を調整する。（必須項目）
					iniDisp = AdjustFromDispInfo(xx, yy, rate, width, height);

					// オプション項目の設定
					AdjustOptionFromDic(dic, false);
				}
				_initDispInfoString = null;
			}
			if( iniDisp == false )
			{
				FitImage( _isFitImageWidth );
			}
			labelDispInfo.Text = String.Format("[ {0} x {1} ]", pboxColImage.Width, pboxColImage.Height);

			// 計測モード、PointB,Eをクリア
			ClearMeasureMode();

			// Fixモードの反映
			_scalePbox.IsFixed = cboxFix.Checked;

			// キーを一旦フォームが受け取る
			this.KeyPreview = true;

			// Drag & Drop （ Pboxはデザイナから指定できないため ）
			pboxColImage.AllowDrop = true;
		}


		/// <summary>
		/// クリップボード画像を開くメニュー項目を追加します</summary>
		private void InitializeClipboardImageOpenMenuItem()
		{
			var menuItem = new ToolStripMenuItem("クリップボード画像を新しいビューで開く");
			menuItem.Click += クリップボード画像を新しいビューで開くToolStripMenuItem_Click;
			contextMenuStripL.Items.Insert(0, new ToolStripSeparator());
			contextMenuStripL.Items.Insert(0, menuItem);
		}


		/// <summary>
		/// 平均色画像表示メニュー項目を追加します</summary>
		private void InitializeAverageColorImageMenuItem()
		{
			_averageColorImageToolStripMenuItem = new ToolStripMenuItem("平均色画像を表示(&A)");
			_averageColorImageToolStripMenuItem.Click += 平均色画像を表示ToolStripMenuItem_Click;
			contextMenuStripProfData.Items.Add(_averageColorImageToolStripMenuItem);
		}


		/// <summary>
		/// クリップボード画像を新しいビューで開きます</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void クリップボード画像を新しいビューで開くToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenClipboardImage(this, this.Icon, _actOnClosed, _actOnOpened);
		}


		/// <summary>
		/// 平均色画像を新しいビューで開きます</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 平均色画像を表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Bitmap bmp = null;
			try
			{
				if (TryCreateAverageColorImage(out bmp, out var title) == false) return;

				var frm = new FormColorImageS(bmp, title, false, -1, -1, null, _settings, this.Icon, _actOnClosed, null, true, true, _actOnOpened);
				bmp = null;
				ShowLinkedImageForm(frm);
			}
			catch (Exception ex)
			{
				MessageBox.Show(String.Format("平均色画像表示で例外発生 Message={0}, Stack={1}", ex.Message, ex.StackTrace), "例外", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				bmp?.Dispose();
			}
		}


		/// <summary>
		/// 関連画像フォームを表示し、表示完了アクションを呼び出します</summary>
		/// <param name="frm">表示対象フォーム</param>
		private void ShowLinkedImageForm(FormColorImageS frm)
		{
			frm.Show();
			_actOnOpened?.Invoke();
		}


		/// <summary>
		/// 指定したIcon（の複製）をアイコンとして設定します
		/// </summary>
		/// <param name="icon">アイコン</param>
		[SupportedOSPlatform("windows")]
		public void SetIcon(Icon icon)
		{
			this.Icon = (Icon)icon.Clone();
			Refresh();
		}


		/// <summary>
		/// ドラッグドロップされたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		/// <remarks>画像ファイルを別ウィンドウ（サブウィンドウ）で開きます</remarks>
		private async void FormMain_DragDrop(object sender, DragEventArgs e)
		{
            try
            {
                //ドロップされたデータがFileDrop型か調べる
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
					if (_isOpeningDroppedImages)
					{
						MessageBox.Show("画像読み込み中です。完了してから再度実行してください。", "画像読み込み", MessageBoxButtons.OK, MessageBoxIcon.Information);
						return;
					}

                    var args = (string[])e.Data.GetData(DataFormats.FileDrop);
					_isOpeningDroppedImages = true;
					try
					{
						var result = await OpenImageFilesAsync(this, args, this.Icon, _actOnClosed, _actOnOpened);
						ShowImageLoadResult(result);
					}
					finally
					{
						_isOpeningDroppedImages = false;
					}
                }
            }
            catch
            {
                MessageBox.Show("指定したファイルをサブウィンドウで表示できません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// ドラッグされたときの処理です</summary>
        /// <param name="sender">送信元情報</param>
        /// <param name="e">イベント情報</param>
        private void FormMain_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;
			//ドラッグされているデータがstring型に変換できるか調べ、
			//そうであればドロップ効果をCopyにする
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				if (((string[])e.Data.GetData(DataFormats.FileDrop)).Length >= 1)
				{
					e.Effect = DragDropEffects.Copy;
				}

			}
		}



		/// <summary>メインピクチャーボックス(ScalablePbox)のクリック時の処理です</summary>
		/// <param name="x">元データのX座標</param>
		/// <param name="y">元データのY座標</param>
		/// <param name="col">表示している位置の色情報</param>
		/// <param name="down">ボタン・ダウンの場合真</param>
		/// <param name="isLeft">左ボタンの場合真</param>
		/// <param name="posF">詳細位置（未使用）</param>
		/// <returns>再描画要求の有無（本ケースでは常にfalse）</returns>
		[SupportedOSPlatform("windows")]
		private bool OnPboxClick(int x, int y, Color col, bool down, bool isLeft, PointF posF)
		{

			// 関係するモードでなければ、リターンするコードを追加予定

			// 表示条件に合わない場合、何もしない
			if (IsPointInSourceImage(x, y) == false)
			{
				return false;
			}

			// ボタンが上がった時のイベントおよび左クリック以外は無視
			if (down == false || isLeft == false)
			{
				var needsProfileUpdate = down == false && (_isMeasureMode || _isProfPosMovAll != null || _isProfileUpdateDeferred);
				if (_isProfPosMovAll != null)
				{
					_isProfPosMovAll = null;
					_isMeasureMode = false;
				}
				if (down == false)
				{
					_isMeasureMode = false;
					_isProfileUpdateDeferred = false;
					_lastProfileUpdateTime = DateTime.MinValue;
				}
				return needsProfileUpdate;
			}

			// 非プロファイルモードで Ctrl+クリック（コントロールキーが押されているかつダウン）時の振る舞い→ラインモードにする
			if (_isProfModeLine == null && (Control.ModifierKeys & Keys.Control) == Keys.Control && down)
			{
				rbtnProfModeLine.Checked = true;
				// すぐに表示されるとラインを引くのに邪魔なので消しておく、マウスup時（ポイント決定時）に再表示させて対応※1（またはCtrl+SPCで表示させる）
				_formProfViewerLine.Hide();
			}

			// プロファイルモードで Ctrl+クリック（コントロールキーが押されているかつダウン）時の振る舞い
			if (_isProfModeLine != null && (Control.ModifierKeys & Keys.Control) == Keys.Control && down)
			{
				ClearProfileCursorMarks();
				if (_measureModePointB == null)
				{
					_measureModePointB = new Point(x, y);
				}
				else if (_isMeasureMode || _measureModePointE == null)
				{
					_measureModePointE = new Point(x, y);
				}
				// 計測モードでなく、B,Eともにセットされている場合
				else
				{
					// （プロファイル範囲の）移動モード条件に一致する場合→プロファイル範囲移動モード条件セット
					if (CheckProfPosMov(x, y))
					{
					}
					// 移動モード条件に一致しない場合
					else
					{
						// PointBから指定開始する
						_measureModePointB = new Point(x, y);
						_measureModePointE = null;
					}
				}
				_isMeasureMode = true;
			}
			else
			{
				_isMeasureMode = false;
			}
			//pboxColImage.Refresh();
			//return false;
			return true;
		}


		/// <summary>
		/// CTRL+クリックされた位置で（プロファイル範囲の）移動モード条件をチェックし、一致すれば条件をセットする</summary>
		/// <param name="x">クリック位置　ｘ</param>
		/// <param name="y">クリック位置　ｙ</param>
		/// <returns>移動モード移行(true:移行した false:移行せず）</returns>
		private bool CheckProfPosMov(int x, int y)
		{
			double NpLimit = Math.Max(1.0, 5.0 / _scalePbox.DispRate);      // 5px→実距離に変換
			if (_isMeasureMode) { _isProfPosMovAll = null; return false; }
			var pos = new Point(x, y);

			// End点に近いときには　計測モード・PointEが未決定であることにする
			if (_measureModePointE != null && UtilityGraphics.CalcDistance(pos, _measureModePointE.Value) <= NpLimit)
			{
				_measureModePointE = null;
				_isMeasureMode = true;
				_isProfPosMovAll = null;
				return true;
			}
			// 開始点に近いときは、 _isProfPosMovAll←falseをセット(全体移動ではなく、開始点移動である旨指示）
			else if (_measureModePointB != null && UtilityGraphics.CalcDistance(pos, _measureModePointB.Value) <= NpLimit)
			{
				_measureModePointB = pos;
				_isProfPosMovAll = false;       // PointB移動
				_profPosMovClickPos = pos;
				_profPosMovOrgPosB = _measureModePointB.Value;
			}
			// ラインプロファイル中でそのラインに近いときは、 _isProfPosMovAll←trueをセット(全体移動である旨指示）
			else if (_isProfModeLine == true && _measureModePointB != null && _measureModePointE != null && UtilityGraphics.DistanceLineToPoint(_measureModePointB.Value, _measureModePointE.Value, pos) < NpLimit)
			{
				_isProfPosMovAll = true;       // 全体移動
				_profPosMovClickPos = pos;
				_profPosMovOrgPosB = _measureModePointB.Value;
				_profPosMovOrgPosE = _measureModePointE.Value;
			}
			// ボックスプロファイル中でその矩形内のときは、 _isProfPosMovAll←trueをセット(全体移動である旨指示）
			else if (_isProfModeLine == false && _measureModePointB != null && _measureModePointE != null && new Rectangle(Math.Min(_measureModePointB.Value.X, _measureModePointE.Value.X), Math.Min(_measureModePointB.Value.Y, _measureModePointE.Value.Y), Math.Abs(_measureModePointB.Value.X - _measureModePointE.Value.X) + 1, Math.Abs(_measureModePointB.Value.Y - _measureModePointE.Value.Y) + 1).Contains(pos))
			{
				_isProfPosMovAll = true;       // 全体移動
				_profPosMovClickPos = pos;
				_profPosMovOrgPosB = _measureModePointB.Value;
				_profPosMovOrgPosE = _measureModePointE.Value;
			}


			return _isProfPosMovAll != null;
		}



		/// <summary>マウスカーソル移動時の処理です</summary>
		/// <param name="x">X位置</param>
		/// <param name="y">Y位置</param>
		/// <param name="col">色情報</param>
		/// <param name="posF">詳細位置</param>
		/// <param name="isDown">マウスボタン状態（ダウン時真）未使用</param>
		[SupportedOSPlatform("windows")]
		private bool OnMouseMove(int x, int y, Color col, bool isDown, PointF posF )
		{
			labelMouseCursorXY.Text = String.Format("( {0,4},{1,4} )", x, y);
			lblSubPx.Text =  $"( {posF.X.ToString("0.00").PadLeft(8)} , {posF.Y.ToString("0.00").PadLeft(8)} )";
			labelMouseCursor1.Text= String.Format( "RGB [{0,3},{1,3},{2,3}]  ({3,3})",col.R, col.G, col.B, RgbToGray(col.R,col.G,col.B) );
			int hh, ss, vv;
			UtilityGraphics.HsvFromRgb(col.R, col.G, col.B, out hh, out ss, out vv);
			labelMouseCursor2.Text = String.Format("HSV [{0,3},{1,3},{2,3}]", hh, ss, vv);
			var ybc = UtilityGraphics.YCrCbFromRGB(col.R, col.G, col.B);
			labelMouseCursor3.Text = $"YCrCb [{ybc.Y,4},{ybc.Cr,4},{ybc.Cb,4}]";

			// 測定モード(Ctrl押下）中の移動
			if ( _isMeasureMode== true)
			{
			   if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
			   {
				   _isMeasureMode = false;
				   return false;
			   }
			   // 表示条件に合わない場合、何もしない
			   if (IsPointInSourceImage(x, y) == false)
			   {
				   return false;
			   }

				// 移動モード中でない場合の処理
				if (_isProfPosMovAll == null)
				{
					// ポイント(Begin)が設定されていれば、現在の場所を(End)に設定する
					if (_measureModePointB != null)
					{
						// シフトも同時に押されていれば、開始点から水平または垂直の位置に終了ポイントを設定する。（カーソル自体は移動しない）
						if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
						{
							if (Math.Abs(_measureModePointB.Value.X - x) < Math.Abs(_measureModePointB.Value.Y - y)) x = _measureModePointB.Value.X;
							else y = _measureModePointB.Value.Y;
						}
						_measureModePointE = new Point(x, y);
					}
				}
				else if (_isProfPosMovAll == true)
				{
					var ofsX = x - _profPosMovClickPos.X;
					var ofsY = y - _profPosMovClickPos.Y;
					// シフトも同時に押されていれば、開始点から水平または垂直の位置に終了ポイントを設定する。（カーソル自体は移動しない）
					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						if (Math.Abs(ofsX) < Math.Abs(ofsY)) ofsX = 0;
						else ofsY = 0;
					}

					var posB = new Point( _profPosMovOrgPosB.X + ofsX, _profPosMovOrgPosB.Y + ofsY );
					var posE = new Point(_profPosMovOrgPosE.X + ofsX, _profPosMovOrgPosE.Y + ofsY);

					if (IsPointInSourceImage(posB) && IsPointInSourceImage(posE))
					{
						_measureModePointB = posB;
						_measureModePointE = posE;
					}
				}
				else if (_isProfPosMovAll == false)
				{
					var ofsX = x - _profPosMovClickPos.X;
					var ofsY = y - _profPosMovClickPos.Y;
					// シフトも同時に押されていれば、開始点から水平または垂直の位置に終了ポイントを設定する。（カーソル自体は移動しない）
					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						if (Math.Abs(ofsX) < Math.Abs(ofsY)) ofsX = 0;
						else ofsY = 0;
					}
					var posB = new Point(_profPosMovOrgPosB.X + ofsX, _profPosMovOrgPosB.Y + ofsY);
					if (IsPointInSourceImage(posB)) _measureModePointB = posB;
				}

			}

		   return true;
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
		/// 指定座標が元画像内かどうかを取得します</summary>
		/// <param name="x">X座標</param>
		/// <param name="y">Y座標</param>
		/// <returns>画像内の場合true</returns>
		private bool IsPointInSourceImage(int x, int y)
		{
			return IsPointInBitmap(_sourceImage, x, y);
		}


		/// <summary>
		/// 指定座標が元画像内かどうかを取得します</summary>
		/// <param name="point">座標</param>
		/// <returns>画像内の場合true</returns>
		private bool IsPointInSourceImage(Point point)
		{
			return IsPointInSourceImage(point.X, point.Y);
		}


		/// <summary>
		/// 指定座標がビットマップ内かどうかを取得します</summary>
		/// <param name="bmp">ビットマップ</param>
		/// <param name="x">X座標</param>
		/// <param name="y">Y座標</param>
		/// <returns>ビットマップ内の場合true</returns>
		private bool IsPointInBitmap(Bitmap bmp, int x, int y)
		{
			return bmp != null && x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height;
		}


		/// <summary>
		/// 指定矩形がビットマップ内かどうかを取得します</summary>
		/// <param name="bmp">ビットマップ</param>
		/// <param name="rect">矩形</param>
		/// <returns>ビットマップ内の場合true</returns>
		private bool IsRectInBitmap(Bitmap bmp, Rectangle rect)
		{
			return bmp != null && rect.Width > 0 && rect.Height > 0 && rect.Left >= 0 && rect.Top >= 0 && rect.Right <= bmp.Width && rect.Bottom <= bmp.Height;
		}


		/// <summary>
		/// ズーム時の動作です</summary>
		/// <param name="isFitWidth">横フィットを指定：true, 縦フィット：false, 全面：null（無指定時）</param>
		/// <returns>調整後の表示倍率</returns>
		/// <remarks>メソッド FitImageとの違いはダイアログ余白の調整を行わないこと（画像の拡大縮小のみ）</remarks>
		public float ZoomImage(bool? isFitWidth = null)
		{
			if (_scalePbox.IsFixed) return _scalePbox.DispRate;

			// 画像をフィットさせる。
			return _scalePbox.Zoom(isFitWidth);
		}


		/// <summary>
		/// フィット時の動作です</summary>
		/// <param name="isFitWidth">横フィットを指定：true, 縦フィット：false, 全面：null（無指定時）</param>
		/// <returns>調整後の表示倍率</returns>
		/// <remarks>メソッド ZoomImageとの違いはダイアログ余白の調整を行い、ダイアログ自体のサイズも変更する</remarks>
		[SupportedOSPlatform("windows")]
		public float FitImage(bool? isFitWidth = null)
		{
			if (_scalePbox.IsFixed) return _scalePbox.DispRate;

			// 画像をフィットさせる。
			var rate = _scalePbox.Zoom(isFitWidth);
			// 余白を調整する
			var newWidth = (int)(_sourceImage.Width * rate);
			var newHeight = (int)(_sourceImage.Height * rate);
			this.Width -= _scalePbox.BasePbox.Width - newWidth;
			this.Height -= _scalePbox.BasePbox.Height - newHeight;
			return rate;
		}


		/// <summary>ピクチャーボックスをダブルクリックしたときの処理です。（画像をフィットします）</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		/// <remarks>通常は画像を画面に合わせる。Altキーが押されている場合には逆に余白を削除して画像に合わせる</remarks>
		[SupportedOSPlatform("windows")]
		private void pboxColImage_DoubleClick(object sender, EventArgs e)
		{
			// シフトキーが押されていた場合には余白削除
			if( (ModifierKeys & Keys.Alt) == Keys.Alt )
			{
				AdjustPictureBoxSize();
			}
			else{
				// 全面フィット
				FitImage();
			}
		}

		/// <summary>
		/// 「表示画像をファイルに保存」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 表示画像をファイルに保存ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (pboxColImage.Image == null) return;
			SaveImageFile((Bitmap)pboxColImage.Image);
		}


		/// <summary>画像イメージを名前をつけて保存</summary>
		/// <param name="bmp">画像イメージ</param>
		/// <param name="defName">デフォルトのファイル名（指定が無い場合、タイトルから取得）</param>
		[SupportedOSPlatform("windows")]
		private void SaveImageFile(Bitmap bmp, string defName=null )
		{
			var sfd = new SaveFileDialog();


			//はじめのファイル名を指定する
			sfd.FileName = defName ?? this.Text;
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
		/// 「表示画像をクリップボードにコピー」が選択されたときの処理です
		/// </summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 表示画像をクリップボードにコピーToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (pboxColImage.Image == null) return;
			Clipboard.SetData(  DataFormats.Bitmap, pboxColImage.Image);
		}



		/// <summary>「表示画像を画像情報画面で表示」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 表示画像を画像情報画面で表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("表示部分以外の領域はトリミングされますが、よろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
			{
				var str = FormColorImageS.CreateDispInfoString(0, 0, 1.0f, _scalePbox.BasePbox.Width, _scalePbox.BasePbox.Height);
				var frm = new FormColorImageS((Bitmap)pboxColImage.Image, this.Text + "(Copy)", false, -1, -1, str, _settings, this.Icon, _actOnClosed, onOpened: _actOnOpened);
				ShowLinkedImageForm(frm);
			}
		}


		/// <summary>「 元画像をファイルに保存」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 元画像をファイルに保存ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_sourceImage == null) return;
			SaveImageFile(_sourceImage);
		}


		/// <summary>「元画像をクリップボードにコピー」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 元画像をクリップボードにコピーToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_sourceImage == null) return;
			Clipboard.SetData(DataFormats.Bitmap, _sourceImage);
		}


		/// <summary>「元画像を画像情報画面で表示」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 元画像を画像情報画面で表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_sourceImage == null) return;
			var rect = _scalePbox.GetDispRect();
			var str = FormColorImageS.CreateDispInfoString(rect.X, rect.Y, _scalePbox.DispRate, _scalePbox.BasePbox.Width, _scalePbox.BasePbox.Height);
			var frm = new FormColorImageS(_sourceImage, this.Text + "(Copy)", false, -1, -1, str, _settings, this.Icon, _actOnClosed, onOpened: _actOnOpened);
			ShowLinkedImageForm(frm);
		}


		/// <summary>キーアップ時に呼び出されます</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void FormColorImageS_KeyUp(object sender, KeyEventArgs e)
		{
			if (_isHitAnyKey == true)
			{
				Close();
			}

			// コントロールキーのアップ時、もしプロファイルモードであれば(隠れている）フォームを表示する
			if ((e.KeyCode & Keys.ControlKey) == Keys.ControlKey )
			{
				if (_isProfModeLine != null)
				{
					if (_isProfModeLine == true)
					{
						ShowProfileViewer(_formProfViewerLine);
					}
					else
					{
						ShowProfileViewer(_formProfViewerBoxH);
						ShowProfileViewer(_formProfViewerBoxV);
					}
				}

				// 移動モードであればキャンセル
				_isProfPosMovAll = null;

			}
		}


		// =====　表示情報文字列  =====
		// BasePosX		原点位置の元画像座標(x位置)		float
		// BasePosY		原点位置の元画像座標(y位置)		float
		// Rate			表示拡大率						float
		// Width		表示枠　幅						int
		// Height		表示枠　高さ					int		
		//  -- option
		// IsProfModeLine プロファイルラインモード		bool
		// ProfPosB			プロファイルモード開始点		Point
		// ProfPosE			プロファイルモード終了点		Point
		// LocPos			ウィンドウロケーション位置		Point




		/// <summary>表示情報文字列を生成します（必須項目のみ）</summary>
		/// <param name="xx">原点位置の元画像座標(x位置)</param>
		/// <param name="yy">原点位置の元画像座標(y位置)</param>
		/// <param name="rate">表示拡大率</param>
		/// <param name="width">表示枠（幅）</param>
		/// <param name="height">表示枠（高さ）</param>
		/// <returns>生成した表示情報文字列</returns>
		/// <remarks>FormColorImageS独自フォーマット</remarks>
		static public string CreateDispInfoString(float xx, float yy, float rate, int width, int height)
		{
			var sList = new List<string>();
			sList.Add("表示情報文字列");
			// 必須
			sList.Add("BasePosX" + PP_PluginConfigData.Keyword_Value + xx.ToString());
			sList.Add("BasePosY" + PP_PluginConfigData.Keyword_Value + yy.ToString());
			sList.Add("Rate" + PP_PluginConfigData.Keyword_Value + rate.ToString());
			sList.Add("Width" + PP_PluginConfigData.Keyword_Value + width.ToString());
			sList.Add("Height" + PP_PluginConfigData.Keyword_Value + height.ToString());
			return String.Join(PP_PluginConfigData.Keyword_Item, sList);
		}


		/// <summary>表示情報文字列を生成します（オプション項目含む）</summary>
		/// <param name="xx">原点位置の元画像座標(x位置)</param>
		/// <param name="yy">原点位置の元画像座標(y位置)</param>
		/// <param name="rate">表示拡大率</param>
		/// <param name="width">表示枠（幅）</param>
		/// <param name="height">表示枠（高さ）</param>
		/// <param name="isProfModeLine">プロファイルモード</param>
		/// <param name="profPosB">プロファイルモード時開始点</param>
		/// <param name="profPosE">プロファイルモード時終了点</param>
		/// <param name="locPos">ウィンドウロケーション</param>
		/// <returns>生成した表示情報文字列</returns>
		/// <remarks>FormColorImageS独自フォーマット（オプション含む）</remarks>
		static public string CreateDispInfoStringEx(float xx, float yy, float rate, int width, int height, bool? isProfModeLine, Point? profPosB, Point? profPosE, Point? locPos )
		{
			var sList = new List<string>();
			sList.Add("表示情報文字列");
			// 必須
			sList.Add("BasePosX" + PP_PluginConfigData.Keyword_Value + xx.ToString());
			sList.Add("BasePosY" + PP_PluginConfigData.Keyword_Value + yy.ToString());
			sList.Add("Rate" + PP_PluginConfigData.Keyword_Value + rate.ToString());
			sList.Add("Width" + PP_PluginConfigData.Keyword_Value + width.ToString());
			sList.Add("Height" + PP_PluginConfigData.Keyword_Value + height.ToString());
			// オプション
			if (isProfModeLine != null) sList.Add("IsProfModeLine" + PP_PluginConfigData.Keyword_Value + isProfModeLine.Value.ToString());
			if (profPosB != null) sList.Add("ProfPosB"	+ PP_PluginConfigData.Keyword_Value + profPosB.Value.ToString());
			if (profPosE != null) sList.Add("ProfPosE"	+ PP_PluginConfigData.Keyword_Value + profPosE.Value.ToString());
			if (locPos	 != null) sList.Add("LocPos"	+ PP_PluginConfigData.Keyword_Value + locPos.Value.ToString());
			// オプション
			return String.Join(PP_PluginConfigData.Keyword_Item, sList);
		}


		/// <summary>表示情報をクリップボードへ書き込みます</summary>
		/// <param name="xx">原点位置の元画像座標(x位置)</param>
		/// <param name="yy">原点位置の元画像座標(y位置)</param>
		/// <param name="rate">表示拡大率</param>
		/// <param name="width">表示枠（幅）</param>
		/// <param name="height">表示枠（高さ）</param>
		/// <remarks>FormColorImageS独自フォーマット</remarks>
		[SupportedOSPlatform("windows")]
		static public void DispInfoToClipBoard( float xx, float yy, float rate, int width, int height )
		{
			var info = CreateDispInfoString(xx, yy, rate, width, height);
			Clipboard.SetDataObject(info,true);
		}


		/// <summary>表示情報をクリップボードへ書き込みます</summary>
		/// <param name="xx">原点位置の元画像座標(x位置)</param>
		/// <param name="yy">原点位置の元画像座標(y位置)</param>
		/// <param name="rate">表示拡大率</param>
		/// <param name="width">表示枠（幅）</param>
		/// <param name="height">表示枠（高さ）</param>
		/// <remarks>FormColorImageS独自フォーマット</remarks>
		[SupportedOSPlatform("windows")]
		static public void DispInfoToClipBoardEx(float xx, float yy, float rate, int width, int height, bool? isProfModeLine, Point? profPosB, Point? profPosE, Point? locPos)
		{
			var info = CreateDispInfoStringEx(xx, yy, rate, width, height, isProfModeLine, profPosB, profPosE, locPos);
			Clipboard.SetDataObject(info, true);
		}

		/// <summary>表示情報文字列をパースし、必須情報を取得します</summary>
		/// <param name="str">表示情報文字列（FormColorImageS独自フォーマット）</param>
		/// <param name="xx">原点位置の元画像座標(x位置)</param>
		/// <param name="yy">原点位置の元画像座標(y位置)</param>
		/// <param name="rate">表示拡大率</param>
		/// <param name="width">表示枠（幅）</param>
		/// <param name="height">表示枠（高さ）</param>
		/// <param name="dic">辞書</param>
		/// <returns>成否</returns>
		static public bool ParseDispInfoString( string str, out float xx, out float yy, out float rate, out int width, out int height, out Dictionary<string,string>dic)
		{
			dic = null;
			xx = yy = width = height = int.MinValue;
			rate = 0.0f;
			if (String.IsNullOrEmpty(str)) return false;
			string ww;
			dic = PP_PluginConfigData.ConfigStringToDictionary(str, out ww);
			if (dic == null) return false;

			string wStr = "";
			int wInt = 0;
			float wFlt = 0.0f;

			// BasePosX		原点位置の元画像座標(x位置)		int
			if ((dic.TryGetValue("BasePosX", out wStr) && float.TryParse(wStr, out wFlt)) == true) xx = wFlt;

			// BasePosY		原点位置の元画像座標(y位置)		int
			if ((dic.TryGetValue("BasePosY", out wStr) && float.TryParse(wStr, out wFlt)) == true) yy = wFlt;

			// Rate			表示拡大率						float
			if ((dic.TryGetValue("Rate", out wStr) && float.TryParse(wStr, out wFlt)) == true) rate = wFlt;

			// Width		表示枠　幅						int
			if ((dic.TryGetValue("Width", out wStr) && int.TryParse(wStr, out wInt)) == true) width = wInt;

			// Height		表示枠　高さ					int							
			if ((dic.TryGetValue("Height", out wStr) && int.TryParse(wStr, out wInt)) == true) height = wInt;

			// 非必須項目（オプション、LocPosなどはここでは取り出さない）

			return true;
		}


		/// <summary>
		/// 文字列(ToString()により変換された）をPoint構造体にパースします</summary>
		/// <param name="str">PointインスタンスをToString()により変換された文字列</param>
		/// <returns>パース成否</returns>
		static bool ParsePoint(string str, out Point pos)
		{
			pos = new Point();
			var _regParsePoint = new Regex(@"X=(?<X>[\+\-\d]+),[ ]*Y=(?<Y>[\+\-\d]+)");

			try
			{
				var m = _regParsePoint.Match(str);
				if (m.Success)
				{
					var txstr = m.Groups["X"].Value;
					var tystr = m.Groups["Y"].Value;
					int ww;
					if (int.TryParse(txstr, out ww) == false) return false;
					pos.X = ww;
					if (int.TryParse(tystr, out ww) == false) return false;
					pos.Y = ww;
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}



		/// <summary>クリップボードに書き込まれた表示情報を取得します</summary>
		/// <param name="xx">原点位置の元画像座標(x位置)</param>
		/// <param name="yy">原点位置の元画像座標(y位置)</param>
		/// <param name="rate">表示拡大率</param>
		/// <param name="width">表示枠（幅）</param>
		/// <param name="height">表示枠（高さ）</param>
		/// <returns>成否</returns>
		/// <remarks>FormColorImageS独自フォーマット</remarks>
		[SupportedOSPlatform("windows")]
		static public bool DispInfoFromClipBoard(out float xx, out float yy, out float rate, out int width, out int height, out Dictionary<string,string>dic)
		{
			// クリップボードから（テキスト）データを取得。
			IDataObject data = Clipboard.GetDataObject();
			string str = "";
			xx = yy = width = height = -1;
			rate = 0.0f;
			dic = null;
			if (data.GetDataPresent(DataFormats.Text))
			{
				str = (string)data.GetData(DataFormats.Text);
				return ParseDispInfoString(str, out xx, out yy, out rate, out width, out height, out dic);
			}
			// エラー
			return false;
		}



		/// <summary>「表示情報をクリップボードへ」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 表示情報をクリップボードへToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var rect = _scalePbox.GetDispRect();
			DispInfoToClipBoardEx( rect.X, rect.Y, _scalePbox.DispRate, _scalePbox.BasePbox.Width, _scalePbox.BasePbox.Height, _isProfModeLine, _measureModePointB, _measureModePointE, this.Location);
		}



		/// <summary>表示情報に従い、画面表示を調整します</summary>
		/// <param name="xx">原点位置の元画像座標(x位置)</param>
		/// <param name="yy">原点位置の元画像座標(y位置)</param>
		/// <param name="rate">表示拡大率</param>
		/// <param name="width">表示枠（幅）</param>
		/// <param name="height">表示枠（高さ）</param>
		/// <returns>成否</returns>
		[SupportedOSPlatform("windows")]
		private bool AdjustFromDispInfo(float xx, float yy, float rate, int width, int height)
		{
			// まず、ウィンドウの大きさを合わせる
			var newWidth = this.Width + width - _scalePbox.BasePbox.Width;
			var newHeight = this.Height + height - _scalePbox.BasePbox.Height;
			if (newWidth <= 10 || newHeight <= 10 || rate < 0.01 || rate > 1000.0)
			{
				MessageBox.Show(String.Format("不適当な表示情報であるため、無視します  newWidth={0}, newHeight={1}", newWidth, newHeight), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			this.Width = newWidth;
			this.Height = newHeight;
			_scalePbox.MoveDispPos(xx, yy, 0, 0, rate);		// 元画像の(tx,ty)を倍率rateで(0,0)から表示
			_scalePbox.DrawMainImage();
			return true;
		}


		/// <summary>「クリップボードの表示情報インポート」が選択されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		/// <remarks>メソッド内でALTキーの状態を中でチェックして処理に反映させている（LocPosを有効とするか）</remarks>
		[SupportedOSPlatform("windows")]
		private void クリップボードの表示情報インポートToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// クリップボードから表示情報データを取得。
			if( DispInfoFromClipBoard( out float tx, out float ty, out float rate, out int width, out int height, out Dictionary<string, string> dic )== true )
			{
				// Altキーの押し下げ状態。　Alt押し下げ時には、LocPos（ウィンドウ位置）の反映を行う。
				var isAlt = (Control.ModifierKeys & Keys.Alt) == Keys.Alt;
				// オプション項目の設定
				AdjustOptionFromDic(dic, isAlt);

				// 取得した情報に合わせて、画面や表示状態を調整する。（必須項目）
				AdjustFromDispInfo(tx, ty, rate, width, height);

				// 再描画
				_scalePbox.DrawMainImage();

				// オプション項目の設定（※）
				// 原因は判明していないが、先頭で行ったこの処理をここで再度実行しないとオプション（プロファイル）の情報が空で表示される。
				AdjustOptionFromDic(dic, isAlt);
	
				Refresh();
			}
			else
			{
				MessageBox.Show("不適当な表示情報であるため、無視します", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}


		/// <summary>
		/// オプション項目の設定を行います</summary>
		/// <param name="dic">辞書</param>
		/// <param name="isEnableLocPos">LocPos項目を有効（反映）する</param>
		[SupportedOSPlatform("windows")]
		private void AdjustOptionFromDic( Dictionary<string,string>dic, bool isEnableLocPos)
		{
			string wStr;
			bool wBool= false;
			var wPos= new Point();
			if ((dic.TryGetValue("IsProfModeLine", out wStr) && bool.TryParse(wStr, out wBool)) == true)
			{
				_isProfModeLine = wBool;
				if ((dic.TryGetValue("ProfPosB", out wStr) && ParsePoint(wStr, out wPos)) == true) _measureModePointB = wPos;
				if ((dic.TryGetValue("ProfPosE", out wStr) && ParsePoint(wStr, out wPos)) == true) _measureModePointE = wPos;
				if (_isProfModeLine == true)	rbtnProfModeLine.Checked = true;
				else							rbtnProfModeBox.Checked = true;
			}
			else
			{
				_isProfModeLine = null;
				_measureModePointB = null;
				_measureModePointE = null;
				rbtnProfModeOff.Checked = true;
			}

			// LocPos項目を有効（反映）する
			if (isEnableLocPos)
			{
				if ((dic.TryGetValue("LocPos", out wStr) && ParsePoint(wStr, out wPos)) == true)
				{
					//Formのあるディスプレイの範囲内であれば、位置を移動
					if (Screen.FromControl(this).Bounds.Contains(wPos)) this.Location = wPos;
				}
			}


		}


		/// <summary>
		/// フォームを閉じる前の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void FormColorImageS_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				DeleteProfileForm();
			}
			catch
			{
				// 終了操作を優先する。
			}
		}


		/// <summary>クローズ時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void FormColorImageS_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				DeleteProfileForm();
			}
			catch
			{
			}

			try
			{
				_scalePbox?.Dispose();
			}
			catch
			{
			}
			finally
			{
				_scalePbox = null;
			}

			try
			{
				_sourceImage?.Dispose();
			}
			catch
			{
			}
			finally
			{
				_sourceImage = null;
			}

			// 終了時アクションが指定されていれば呼び出す。
			try
			{
				_actOnClosed?.Invoke();
			}
			catch
			{
			}
		}

		/// <summary>「Fit」が指定されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void fitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 全面フィット
			FitImage();
		}

		/// <summary>「等倍100%」が指定されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 等倍100ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_scalePbox.SetRate(1.00f);
		}

		/// <summary>「倍率指定」が指定されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 指定倍率ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var frm = new SetingsRateForm(_scalePbox.DispRate );
			var ret = frm.ShowDialog();
			if (ret == System.Windows.Forms.DialogResult.OK)
			{
				_scalePbox.SetRate(frm.Rate);
				_scalePbox.DrawMainImage();
				AdjustPictureBoxSize();
			}
		}


		/// <summary>ピクチャーボックスサイズを調整し,余白を除きます</summary>
		[SupportedOSPlatform("windows")]
		private void AdjustPictureBoxSize()
		{
			// 余白を取得
			int sT, sB, sL, sR;
			_scalePbox.GetSpeceSize(out sT, out sL, out sB, out sR);

			// 必要があれば原点移動する。
			if (sT > 0 || sL > 0)
			{
				var rect = _scalePbox.GetDispRect();
				float xx = sL > 0 ? rect.X : 0;
				float yy = sT > 0 ? rect.Y : 0;
				_scalePbox.MoveDispPos(0, 0, xx, yy);
			}
			this.Width -= sL + sR;
			this.Height -= sT + sB;
		}


		/// <summary>「余白の削除」が指定されたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void 余白の削除ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AdjustPictureBoxSize();
		}

		/// <summary>
		/// ピクチャーボックスのサイズが変更されたときのイベント処理です。表示情報等の変更
		/// </summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void pboxColImage_Resize(object sender, EventArgs e)
		{
			labelDispInfo.Text = String.Format("[ {0} x {1} ]", pboxColImage.Width, pboxColImage.Height);
		}


		/// <summary>
		/// 表示情報文字列コンテキストメニュー「表示サイズ指定」が指定されたときの処理です
		/// </summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 表示サイズ指定ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				//現在フォームが存在しているディスプレイを取得し、指定可能な最大サイズとする
				var s = Screen.FromControl(this);
				//ディスプレイの高さと幅を取得
				int maxh = s.Bounds.Height;
				int maxw = s.Bounds.Width;

				var frm = new SettingSizeForm(pboxColImage.Width, pboxColImage.Height, maxw, maxh);
				if (frm.ShowDialog() != DialogResult.OK) return;

				// ウィンドウの大きさを合わせる
				var newWidth = this.Width + frm.ValueWidth - _scalePbox.BasePbox.Width;
				var newHeight = this.Height + frm.ValueHeight - _scalePbox.BasePbox.Height;
				if (newWidth <= 10 || newHeight <= 10 || newWidth > maxw || newHeight > maxh)
				{
					MessageBox.Show(String.Format("不適当な表示情報であるため、無視します  newWidth={0}, newHeight={1}", newWidth, newHeight), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				this.Width = newWidth;
				this.Height = newHeight;
				Refresh();
			}
			catch (Exception ex)
			{
				MessageBox.Show(String.Format( "例外が発生しました。　Message={0},  Stack={1}", ex.Message, ex.StackTrace ));
			}
		}


		/// <summary>
		/// 表示情報文字列コンテキストメニュー「FormClorImageS設定」が指定されたときの処理です
		/// </summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void FormClorImageS設定ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var frm = new FormConfigurationData(_settings);
			if (frm.ShowDialog() == DialogResult.OK && frm.Settings is ColImageSetteings)
			{
				_settings = (ColImageSetteings)frm.Settings;
				OnChangeSettings();
			}
		}


		/// <summary>
		/// 設定が変更されたときの処理です
		/// </summary>
		[SupportedOSPlatform("windows")]
		private void OnChangeSettings()
		{
			_scalePbox.InterpolationMode = _settings.MainInterpolationMode;
			_scalePbox.InterpolationModeReduce = _settings.MainInterpolationModeReduce;
			_scalePbox.BasePbox.BackColor = _settings.BackColor;
			ApplyProfileViewerForegroundMode();
			_scalePbox.DrawMainImage();
		}


		/// <summary>
		/// プロファイルダイアログの前面表示モードを適用します</summary>
		private void ApplyProfileViewerForegroundMode()
		{
			ApplyProfileViewerForegroundMode(_formProfViewerLine);
			ApplyProfileViewerForegroundMode(_formProfViewerBoxH);
			ApplyProfileViewerForegroundMode(_formProfViewerBoxV);
		}


		/// <summary>
		/// プロファイルダイアログの前面表示モードを適用します</summary>
		/// <param name="profileViewer">プロファイルダイアログ</param>
		private void ApplyProfileViewerForegroundMode(FormProfileViewer profileViewer)
		{
			if (profileViewer == null || profileViewer.IsDisposed) return;
			profileViewer.ApplyForegroundMode(this, _settings.ProfileViewerForegroundMode);
		}


		/// <summary>
		/// プロファイルダイアログを前面表示モードに従って表示します</summary>
		/// <param name="profileViewer">プロファイルダイアログ</param>
		private void ShowProfileViewer(FormProfileViewer profileViewer)
		{
			if (profileViewer == null || profileViewer.IsDisposed) return;
			profileViewer.ShowWithForegroundMode(this, _settings.ProfileViewerForegroundMode);
		}


		/// <summary>
		/// プロファイルダイアログから前面表示モードが変更されたときの処理です</summary>
		/// <param name="foregroundMode">前面表示モード</param>
		private void OnProfileViewerForegroundModeChanged(ProfileViewerForegroundMode foregroundMode)
		{
			_settings.ProfileViewerForegroundMode = foregroundMode;
			ApplyProfileViewerForegroundMode();
		}


		/// <summary>
		/// 「表示枠情報を別の画像情報画面に表示」が指定されたときの処理です
		/// </summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 表示枠情報を別の画像情報画面に表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( _sourceImage== null ) return;
			using (var bmp = _sourceImage.Clone(new Rectangle(0, 0, _sourceImage.Width, _sourceImage.Height), PixelFormat.Format24bppRgb))
			{
				using (var gra = Graphics.FromImage(bmp))
				using (var pen = new Pen(Color.Red, 3.0f) { Alignment = PenAlignment.Outset })
				{
					// 表示領域の太線はPenAlignmentは外側に描画するためOutsetを指定（ただし.NETの不具合で Outsetでも中央描画してしまうとのこと2022.04.01現在）
					pen.Alignment = PenAlignment.Outset;	
					gra.DrawRectangleFs(pen, new[] { _scalePbox.GetDispRect() });
				}
				var frm = new FormColorImageS(bmp, this.Text + "表示領域", false, -1, -1, null, _settings, null, _actOnClosed, onOpened: _actOnOpened);
				ShowLinkedImageForm(frm);
			}

		}


		/// <summary>
		/// フォームペイント時の処理です</summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		private void FormColorImageS_Paint(object sender, PaintEventArgs e)
		{
			// その他
			var str = "";
			if (_measureModePointB != null)
			{
				var bx = _measureModePointB.Value.X;
				var by = _measureModePointB.Value.Y;
				if (_measureModePointE != null)
				{
					// PEND　描画あとで
					//////var ex = _measureModePointE.Value.X;
					//////var ey = _measureModePointE.Value.Y;

					//////// 単位変換不要な場合（ピクセルサイズ指定なしとありの場合
					//////var pixSize = CommonValues.ConfigData.PixelSize;
					//////var pixUnit = CommonValues.ConfigData.PixelSizeUnit;
					//////var len = Math.Sqrt((bx - ex) * (bx - ex) + (by - ey) * (by - ey));
					//////if (pixSize == 1.0 && String.IsNullOrEmpty(pixUnit))
					//////{
					//////    str = string.Format("({0},{1}) - ({2},{3})  距離:{4} pixcel", bx.ToString().PadLeft(4), by.ToString().PadLeft(4), ex.ToString().PadLeft(4), ey.ToString().PadLeft(4), len.ToString("##0.000").PadLeft(9));

					//////}
					//////else
					//////{
					//////    var lenPstr = (len * pixSize).ToString();
					//////    str = string.Format("({0},{1}) - ({2},{3})  距離:{4} pixcel　{5} {6}", bx.ToString().PadLeft(4), by.ToString().PadLeft(4), ex.ToString().PadLeft(4), ey.ToString().PadLeft(4), Math.Sqrt((bx - ex) * (bx - ex) + (by - ey) * (by - ey)).ToString("##0.000").PadLeft(9), lenPstr.Length > 7 ? lenPstr.Substring(0, 7) : lenPstr, pixUnit == null ? "" : pixUnit);
					//////}
				}
				else
				{
					str = string.Format("({0},{1})", bx.ToString().PadLeft(4), by.ToString().PadLeft(4));
				}
			}


		}



		/// <summary>ScalablePboxからの描画時コールバック処理です。 </summary>
		/// <param name="gra">描画対象Grahics</param>
		/// <param name="siz">画像サイズ</param>
		[SupportedOSPlatform("windows")]
		private void OnPaintPbox(Graphics gra, Size siz)
		{
			// 距離測定用のカーソル表示
			DispMeasureMode(gra);

			// プロファイルフォーム生成・描画
			if (ShouldUpdateProfileFormOnPaint())
			{
				CreateProfileForm(false);
			}
		}


		/// <summary>
		/// 描画時にプロファイルフォームを更新するかどうかを取得します</summary>
		/// <returns>更新する場合true</returns>
		private bool ShouldUpdateProfileFormOnPaint()
		{
			if (IsProfileDragUpdateThrottleTarget() == false)
			{
				_isProfileUpdateDeferred = false;
				_lastProfileUpdateTime = DateTime.MinValue;
				return true;
			}

			var now = DateTime.UtcNow;
			if ((now - _lastProfileUpdateTime).TotalMilliseconds >= ProfileDragUpdateIntervalMilliseconds)
			{
				_lastProfileUpdateTime = now;
				_isProfileUpdateDeferred = false;
				return true;
			}

			_isProfileUpdateDeferred = true;
			return false;
		}


		/// <summary>
		/// ドラッグ中のプロファイル更新間引き対象かどうかを取得します</summary>
		/// <returns>間引き対象の場合true</returns>
		private bool IsProfileDragUpdateThrottleTarget()
		{
			return _isMeasureMode && _isProfModeLine == false && _measureModePointB != null && _measureModePointE != null;
		}


		/// <summar>計測モード時の描画を行います</summary>
		[SupportedOSPlatform("windows")]
		private void DispMeasureMode(Graphics g)
		{
			var profVisible = false;
			try
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				var mat = _scalePbox.Mat.Clone();
				// 十字カーソルのサイズは固定で10pixel（表示座標上）
				var dR = (float)_scalePbox.DispRate;
				var sizeCursol = (float)24.0 / dR;	// (float)Math.Ceiling(20.0 / dR);
				var widLine = _settings.ProfileSelectionLineWidth / dR;		// (float)Math.Ceiling( 2.0 / dR);
				var widCursor = _settings.ProfilePointCursorLineWidth / dR;	// (float)Math.Ceiling( 1.0 / dR);
				var pcSize = (float)4.0 / dR;		// (float)Math.Ceiling( 4.0 / dR);   


				// データがなしの場合やプロファイルモードでない場合には何もしない。

				// PointBが定義されている
				if (_measureModePointB != null)
				{
					var poBv = (Point)_measureModePointB.Value;
					if (_measureModePointE == null)
					{
						CrossCursor(g, (Point)_measureModePointB.Value, sizeCursol, _settings.ProfileStartCursorColor, widCursor);
						tboxProfInfo.Text = String.Format("({0,4},{1,4} ) - ", _measureModePointB.Value.X, _measureModePointB.Value.Y);
					}
					else
					{
						var pB = _measureModePointB.Value;
						var pE = _measureModePointE.Value;
						var poEv = new Point[] { _measureModePointE.Value };
						DispMeasureCursor(g, pB, pE, _settings.ProfileSelectionLineColor, widLine, pcSize, _isProfModeLine.Value, _settings.ProfileGuideCursorColor);
						CrossCursor(g, pB, sizeCursol, _settings.ProfileStartCursorColor, widCursor);
						CrossCursor(g, pE, sizeCursol, _settings.ProfileEndCursorColor, widCursor);
						tboxProfInfo.Text = String.Format("({0,4},{1,4} ) - ({2,4},{3,4} ) ", pB.X, pB.Y, pE.X, pE.Y);
						// ラインの場合には2点の距離を、ボックス時は2点の差分を表示する
						labelProfSize.Text = _isProfModeLine.Value == true ? String.Format("{0:#0.00}", UtilityGraphics.CalcDistance(pB, pE)) : String.Format("({0}, {1})", pE.X - pB.X, pE.Y - pB.Y);
					}
					profVisible = true;
				}
			}
			finally
			{
				tboxProfInfo.Visible =　labelProfSize.Visible = profVisible;
			}
		}





		/// <summary>
		/// 十字カーソルを表示します
		/// </summary>
		/// <param name="po">中心座標</param>
		/// <param name="siz">大きさ</param>
		/// <param name="col">色</param>
		/// <param name="wid">線の幅</param>
		/// <param name="g">Graphics</param>
		[SupportedOSPlatform("windows")]
		private void CrossCursor(Graphics g, Point po, float siz, Color col, float wid)
		{
			using (var pen = new Pen(col, wid))
			{
				g.DrawLine(pen, po.X - siz / 2.0f, po.Y, po.X + siz / 2, po.Y);
				g.DrawLine(pen, po.X, po.Y - siz / 2.0f, po.X, po.Y + siz / 2.0f);
			}
		}


		/// <summary>
		/// プロファイル表示カーソル情報をクリアします</summary>
		private void ClearProfileCursorMarks()
		{
			ClearProfilePointCursors();
			_pcLineRangeB = _pcLineRangeE = null;
			_pcBoxHRangeB = _pcBoxHRangeE = null;
			_pcBoxVRangeB = _pcBoxVRangeE = null;
		}


		/// <summary>
		/// プロファイル表示の1点カーソル情報をクリアします</summary>
		private void ClearProfilePointCursors()
		{
			_pcLine = null;
			_pcBoxH = null;
			_pcBoxV = null;
		}





		/// <summary>計測モード・計測ラインの表示</summary>
		/// <param name="g">グラフィックス</param>
		/// <param name="pob">開始位置</param>
		/// <param name="poe">終了位置</param>
		/// <param name="col">色</param>
		/// <param name="wid">線の幅</param>
		/// <param name="pcSiz">ポイントカーソルサイズ</param>
		/// <param name="isLine">true:ライン false:ボックス</param>
		/// <param name="guideColor">補助カーソル色</param>
		[SupportedOSPlatform("windows")]
		private void DispMeasureCursor(Graphics g, Point pob, Point poe, Color col, float wid, float pcSiz, bool isLine, Color guideColor)
		{
			using (var pen = new Pen(col, wid))
			{
				pen.StartCap = pen.EndCap = LineCap.ArrowAnchor;

				// モードによりLineまたはRectangleを描画する
				if (isLine)
				{
					g.DrawLine(pen, pob, poe);
					using (var penC = new Pen(guideColor, wid))
					{
						DrawLineProfileRangeCursors(g, pob, poe, penC, pcSiz);
					}

					if (_pcLine != null)
					{
						DrawLineProfilePointCursor(g, pob, poe, _pcLine.Value, pcSiz, guideColor);
					}
				}
				else
				{
					using (var penC = new Pen(guideColor, wid))
					{
						var rect = CreateFRectFromMeasurePoint(pob, poe); // 描画用
						g.DrawRectangle(pen, rect);
						DrawBoxProfileRangeCursors(g, rect, penC);
						if (_pcBoxH != null) g.DrawLine(penC, rect.X + _pcBoxH.Value, rect.Y, rect.X + _pcBoxH.Value, rect.Y + rect.Height);
						if (_pcBoxV != null) g.DrawLine(penC, rect.X, rect.Y + _pcBoxV.Value, rect.X + rect.Width, rect.Y + _pcBoxV.Value);
					}
				}
			}
		}


		/// <summary>
		/// ラインプロファイル範囲カーソルを描画します</summary>
		/// <param name="g">グラフィックス</param>
		/// <param name="pob">開始位置</param>
		/// <param name="poe">終了位置</param>
		/// <param name="pen">描画ペン</param>
		/// <param name="pcSiz">ポイントカーソルサイズ</param>
		private void DrawLineProfileRangeCursors(Graphics g, Point pob, Point poe, Pen pen, float pcSiz)
		{
			if (_pcLineRangeB == null || _pcLineRangeE == null) return;
			if (TryGetLineProfileUnitVector(pob, poe, out var xx, out var yy) == false) return;

			DrawLineProfileRangeCursor(g, pob, xx, yy, _pcLineRangeB.Value, pen, pcSiz);
			DrawLineProfileRangeCursor(g, pob, xx, yy, _pcLineRangeE.Value, pen, pcSiz);
		}


		/// <summary>
		/// ラインプロファイル範囲カーソル1本を描画します</summary>
		/// <param name="g">グラフィックス</param>
		/// <param name="pob">開始位置</param>
		/// <param name="xx">ライン方向X単位ベクトル</param>
		/// <param name="yy">ライン方向Y単位ベクトル</param>
		/// <param name="dist">開始位置からの距離</param>
		/// <param name="pen">描画ペン</param>
		/// <param name="pcSiz">ポイントカーソルサイズ</param>
		private void DrawLineProfileRangeCursor(Graphics g, Point pob, double xx, double yy, double dist, Pen pen, float pcSiz)
		{
			var cx = pob.X + (float)(xx * dist);
			var cy = pob.Y + (float)(yy * dist);
			var nx = -yy;
			var ny = xx;
			var halfLength = pcSiz * 3.0f;
			g.DrawLine(pen, cx - (float)(nx * halfLength), cy - (float)(ny * halfLength), cx + (float)(nx * halfLength), cy + (float)(ny * halfLength));
		}


		/// <summary>
		/// ラインプロファイル1点カーソルを描画します</summary>
		/// <param name="g">グラフィックス</param>
		/// <param name="pob">開始位置</param>
		/// <param name="poe">終了位置</param>
		/// <param name="dist">開始位置からの距離</param>
		/// <param name="pcSiz">ポイントカーソルサイズ</param>
		/// <param name="color">描画色</param>
		private void DrawLineProfilePointCursor(Graphics g, Point pob, Point poe, double dist, float pcSiz, Color color)
		{
			if (TryGetLineProfileUnitVector(pob, poe, out var xx, out var yy) == false) return;
			var rr = new RectangleF(pob.X + (float)(xx * dist) - pcSiz, pob.Y + (float)(yy * dist) - pcSiz, pcSiz * 2, pcSiz * 2);
			using (var brush = new SolidBrush(color))
			{
				g.FillRectangle(brush, rr);
			}
		}


		/// <summary>
		/// ラインプロファイルの単位ベクトルを取得します</summary>
		/// <param name="pob">開始位置</param>
		/// <param name="poe">終了位置</param>
		/// <param name="xx">ライン方向X単位ベクトル</param>
		/// <param name="yy">ライン方向Y単位ベクトル</param>
		/// <returns>取得できた場合はtrue</returns>
		private bool TryGetLineProfileUnitVector(Point pob, Point poe, out double xx, out double yy)
		{
			xx = poe.X - pob.X;
			yy = poe.Y - pob.Y;
			var ww = Math.Sqrt(xx * xx + yy * yy);
			if (ww <= Double.Epsilon) return false;

			xx /= ww;
			yy /= ww;
			return true;
		}


		/// <summary>
		/// ボックスプロファイル範囲カーソルを描画します</summary>
		/// <param name="g">グラフィックス</param>
		/// <param name="rect">描画対象矩形</param>
		/// <param name="pen">描画ペン</param>
		private void DrawBoxProfileRangeCursors(Graphics g, RectangleF rect, Pen pen)
		{
			if (_pcBoxHRangeB != null && _pcBoxHRangeE != null)
			{
				DrawBoxProfileVerticalRangeCursor(g, rect, _pcBoxHRangeB.Value, pen);
				DrawBoxProfileVerticalRangeCursor(g, rect, _pcBoxHRangeE.Value, pen);
			}

			if (_pcBoxVRangeB != null && _pcBoxVRangeE != null)
			{
				DrawBoxProfileHorizontalRangeCursor(g, rect, _pcBoxVRangeB.Value, pen);
				DrawBoxProfileHorizontalRangeCursor(g, rect, _pcBoxVRangeE.Value, pen);
			}
		}


		/// <summary>
		/// ボックスプロファイル範囲の垂直カーソルを描画します</summary>
		/// <param name="g">グラフィックス</param>
		/// <param name="rect">描画対象矩形</param>
		/// <param name="offset">矩形左端からのオフセット</param>
		/// <param name="pen">描画ペン</param>
		private void DrawBoxProfileVerticalRangeCursor(Graphics g, RectangleF rect, int offset, Pen pen)
		{
			var x = Math.Max(rect.Left, Math.Min(rect.Right, rect.X + offset));
			g.DrawLine(pen, x, rect.Top, x, rect.Bottom);
		}


		/// <summary>
		/// ボックスプロファイル範囲の水平カーソルを描画します</summary>
		/// <param name="g">グラフィックス</param>
		/// <param name="rect">描画対象矩形</param>
		/// <param name="offset">矩形上端からのオフセット</param>
		/// <param name="pen">描画ペン</param>
		private void DrawBoxProfileHorizontalRangeCursor(Graphics g, RectangleF rect, int offset, Pen pen)
		{
			var y = Math.Max(rect.Top, Math.Min(rect.Bottom, rect.Y + offset));
			g.DrawLine(pen, rect.Left, y, rect.Right, y);
		}


		/// <summary>
		/// プロファイルモードのラジオボタンの変更時の処理です</summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		private void OnChangeProfileMode(object sender, EventArgs e)
		{
			var rbtn = sender as RadioButton;
			if (rbtn == null|| rbtn.Checked== false ) return;

			SetProfileMode();

			if (_isKeepProfileRangeOnModeChange == false)
			{
				// 計測モード、PointB,Eをクリアし、プロファイル用フォームが開いていれば破棄する
				ClearMeasureMode();
			}
			else
			{
				_isMeasureMode = false;
				_isProfPosMovAll = null;
				DeleteProfileForm();
			}

			if (_isProfModeLine == null)
			{
				_scalePbox.DrawMainImage();
			}
			else
			{
				// プロファイルモードに従い、フォームを生成する
				CreateProfileForm(true);
			}
		}


		/// <summary>
		/// プロファイルモードをセットします</summary>
		/// <remarks>ラジオボタンの状態→メンバ_isProfileMode</remarks>
		[SupportedOSPlatform("windows")]
		private void SetProfileMode()
		{
			if (rbtnProfModeOff.Checked)		_isProfModeLine = null;
			else if (rbtnProfModeLine.Checked)	_isProfModeLine = true;
			else								_isProfModeLine = false;
		}


		/// <summary>
		/// 指定済みのプロファイル範囲を維持してLine/Boxを切り替えます</summary>
		/// <returns>切り替えた場合true</returns>
		[SupportedOSPlatform("windows")]
		private bool ToggleProfileMode()
		{
			if (IsProfileModeToggleKeyTarget() == false) return false;

			var pB = _measureModePointB.Value;
			var pE = _measureModePointE.Value;
			if (pB.X == pE.X || pB.Y == pE.Y) return false;

			try
			{
				_isKeepProfileRangeOnModeChange = true;
				if (_isProfModeLine.Value)
				{
					rbtnProfModeBox.Checked = true;
				}
				else
				{
					rbtnProfModeLine.Checked = true;
				}
			}
			finally
			{
				_isKeepProfileRangeOnModeChange = false;
			}

			_scalePbox.DrawMainImage();
			Refresh();
			return true;
		}


		/// <summary>
		/// プロファイルモード切替キーの対象状態かどうかを取得します</summary>
		/// <returns>対象状態の場合true</returns>
		private bool IsProfileModeToggleKeyTarget()
		{
			return IsProfileRangeSpecified();
		}


		/// <summary>
		/// プロファイル範囲が指定済みかどうかを取得します</summary>
		/// <returns>指定済みの場合true</returns>
		private bool IsProfileRangeSpecified()
		{
			return _sourceImage != null && _isProfModeLine != null && _measureModePointB != null && _measureModePointE != null;
		}


		/// <summary>
		/// MesureMode（ProfMode時のCtrl状態）をクリアします</summary>
		private void ClearMeasureMode()
		{
			// 計測モード、PointB,Eをクリア
			_isMeasureMode = false;
			_measureModePointB = _measureModePointE = null;

			_isProfPosMovAll = null;
			_isProfileUpdateDeferred = false;
			_lastProfileUpdateTime = DateTime.MinValue;

			DeleteProfileForm();
		}


		/// <summary>
		/// プロファイル用フォームを削除します</summary>
		private void DeleteProfileForm()
		{
			// nullでないプロファイル用フォームがあれば、破棄
			if (_formProfViewerLine != null)
			{
				_formProfViewerLine.IsIgnoreClose = false;
				_formProfViewerLine.Dispose();
				_formProfViewerLine = null;
			}
			if (_formProfViewerBoxH != null)
			{
				_formProfViewerBoxH.IsIgnoreClose = false;
				_formProfViewerBoxH.Dispose();
				_formProfViewerBoxH = null;
			}
			if (_formProfViewerBoxV != null)
			{
				_formProfViewerBoxV.IsIgnoreClose = false;
				_formProfViewerBoxV.Dispose();
				_formProfViewerBoxV = null;
			}
			ClearProfileCursorMarks();
		}


		/// <summary>
		/// ラインプロファイル1点カーソル変更時の処理です</summary>
		/// <param name="dist">ライン始点からの距離</param>
		private void OnLineProfileCursorChanged(double dist)
		{
			_pcLine = dist;
		}


		/// <summary>
		/// ラインプロファイル範囲変更時の処理です</summary>
		/// <param name="distB">範囲開始位置</param>
		/// <param name="distE">範囲終了位置</param>
		private void OnLineProfileRangeChanged(double? distB, double? distE)
		{
			_pcLineRangeB = distB;
			_pcLineRangeE = distE;
			if (distB == null && distE == null) _pcLine = null;
			_scalePbox.DrawMainImage();
		}


		/// <summary>
		/// ボックス水平プロファイル1点カーソル変更時の処理です</summary>
		/// <param name="xofs">矩形左端からのオフセット</param>
		private void OnBoxHProfileCursorChanged(double xofs)
		{
			_pcBoxH = (int)Math.Round(xofs);
		}


		/// <summary>
		/// ボックス水平プロファイル範囲変更時の処理です</summary>
		/// <param name="xofsB">範囲開始位置</param>
		/// <param name="xofsE">範囲終了位置</param>
		private void OnBoxHProfileRangeChanged(double? xofsB, double? xofsE)
		{
			_pcBoxHRangeB = RoundProfileOffset(xofsB);
			_pcBoxHRangeE = RoundProfileOffset(xofsE);
			if (xofsB == null && xofsE == null) _pcBoxH = null;
			_scalePbox.DrawMainImage();
		}


		/// <summary>
		/// ボックス垂直プロファイル1点カーソル変更時の処理です</summary>
		/// <param name="yofs">矩形上端からのオフセット</param>
		private void OnBoxVProfileCursorChanged(double yofs)
		{
			_pcBoxV = (int)Math.Round(yofs);
		}


		/// <summary>
		/// ボックス垂直プロファイル範囲変更時の処理です</summary>
		/// <param name="yofsB">範囲開始位置</param>
		/// <param name="yofsE">範囲終了位置</param>
		private void OnBoxVProfileRangeChanged(double? yofsB, double? yofsE)
		{
			_pcBoxVRangeB = RoundProfileOffset(yofsB);
			_pcBoxVRangeE = RoundProfileOffset(yofsE);
			if (yofsB == null && yofsE == null) _pcBoxV = null;
			_scalePbox.DrawMainImage();
		}


		/// <summary>
		/// プロファイル距離を整数オフセットに変換します</summary>
		/// <param name="offset">プロファイル距離</param>
		/// <returns>整数オフセット</returns>
		private int? RoundProfileOffset(double? offset)
		{
			if (offset == null) return null;
			return (int)Math.Round(offset.Value);
		}


		/// <summary>
		/// 平均色を求めます</summary>
		/// <param name="cols">色情報群</param>
		/// <returns>平均色</returns>
		private Color AverageColor(Color[] cols)
		{
			long tR = 0;
			long tG = 0;
			long tB = 0;
			foreach (var c in cols)
			{
				tR += c.R;
				tG += c.G;
				tB += c.B;
			}
			return Color.FromArgb((int)((double)tR / cols.Length + 0.5), (int)((double)tG / cols.Length + 0.5), (int)((double)tB / cols.Length + 0.5));
		}

		/// <summary>
		/// 平均色を求め、RGBを示す文字列に変換します</summary>
		/// <param name="cols">色情報群</param>
		/// <returns>平均色文字列 (R,G,B)</returns>
		private string AverageColorString(Color[] cols)
		{
			var c = AverageColor(cols);
			return RgbColorString(c);
		}


		/// <summary>
		/// RGBを示す文字列に変換します</summary>
		/// <param name="col">色情報</param>
		/// <returns>RGB文字列</returns>
		private string RgbColorString(Color col)
		{
			return $"[{col.R.ToString().PadLeft(3)},{col.G.ToString().PadLeft(3)},{col.B.ToString().PadLeft(3)}]";
		}


		/// <summary>
		/// プロファイル範囲の平均色で塗りつぶした画像を生成します</summary>
		/// <param name="bmp">生成したビットマップ</param>
		/// <param name="title">表示タイトル</param>
		/// <returns>生成できた場合true</returns>
		[SupportedOSPlatform("windows")]
		private bool TryCreateAverageColorImage(out Bitmap bmp, out string title)
		{
			bmp = null;
			title = "";
			if (TryCalculateCurrentProfileAverageColor(out var avgColor, out var rect) == false) return false;

			bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);
			using (var gra = Graphics.FromImage(bmp))
			{
				gra.Clear(avgColor);
			}

			var modeName = _isProfModeLine == true ? "Line" : "Box";
			title = $"平均色_{modeName}_{rect.Width}x{rect.Height}_{RgbColorString(avgColor).Replace(' ', '_')}";
			return true;
		}


		/// <summary>
		/// 現在のプロファイル範囲の平均色を算出します</summary>
		/// <param name="avgColor">平均色</param>
		/// <param name="rect">始点と終点を対角とする矩形</param>
		/// <returns>算出できた場合true</returns>
		[SupportedOSPlatform("windows")]
		private bool TryCalculateCurrentProfileAverageColor(out Color avgColor, out Rectangle rect)
		{
			avgColor = Color.Empty;
			rect = Rectangle.Empty;
			if (IsProfileRangeSpecified() == false) return false;

			rect = CreateRectFromMeasurePoint(_measureModePointB.Value, _measureModePointE.Value);
			if (IsRectInBitmap(_sourceImage, rect) == false) return false;

			if (_isProfModeLine == true)
			{
				var datas = CalcLineProfile(out _);
				if (datas == null || datas.Length == 0) return false;

				avgColor = AverageColor(datas);
				return true;
			}

			return TryCalculateAverageColorFromRect(_sourceImage, rect, out avgColor);
		}


		/// <summary>
		/// 指定矩形の平均色を算出します</summary>
		/// <param name="bmp">ビットマップ</param>
		/// <param name="rect">対象矩形</param>
		/// <param name="avgColor">平均色</param>
		/// <returns>算出できた場合true</returns>
		[SupportedOSPlatform("windows")]
		private unsafe bool TryCalculateAverageColorFromRect(Bitmap bmp, Rectangle rect, out Color avgColor)
		{
			avgColor = Color.Empty;
			if (IsRectInBitmap(bmp, rect) == false) return false;

			var bpp = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
			if (bpp < 3) return false;

			var sumB = 0L;
			var sumG = 0L;
			var sumR = 0L;
			var count = (long)rect.Width * rect.Height;
			try
			{
				var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
				try
				{
					var bmpScan0 = (byte*)bmpData.Scan0;
					var bmpStride = bmpData.Stride;
					for (var yy = 0; yy < rect.Height; yy++)
					{
						var sp = bmpScan0 + (yy + rect.Y) * bmpStride + rect.X * bpp;
						for (var xx = 0; xx < rect.Width; xx++)
						{
							sumB += sp[0];
							sumG += sp[1];
							sumR += sp[2];
							sp += bpp;
						}
					}
				}
				finally
				{
					bmp.UnlockBits(bmpData);
				}

				avgColor = Color.FromArgb(
					(int)Math.Round((double)sumR / count, MidpointRounding.AwayFromZero),
					(int)Math.Round((double)sumG / count, MidpointRounding.AwayFromZero),
					(int)Math.Round((double)sumB / count, MidpointRounding.AwayFromZero));
				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// プロファイルモードに従い、プロファイル用フォームを生成します</summary>
		/// <param name="isForce">すでに存在する場合、強制的にShowを実施する</param>
		[SupportedOSPlatform("windows")]
		private void CreateProfileForm(bool isForce )
		{
			// カーソルクリア
			ClearProfilePointCursors();

			if (_isProfModeLine == null) 
			{
				labelAverageColor.Text = "";
				return;
			}

			// ラインモード
			if (  _isProfModeLine.Value == true )
			{
				var pData = CalcLineProfile( out var distance );
				if (_formProfViewerLine == null)
				{
					_formProfViewerLine = new FormProfileViewer(pData, "ラインプロファイル", null, distance, this.Icon, OnLineProfileCursorChanged, OnLineProfileRangeChanged, OnProfileViewerForegroundModeChanged, () => ToggleProfileMode());
					ShowProfileViewer(_formProfViewerLine);
				}
				else{
					_formProfViewerLine.SetData(pData, distance);
					if (isForce) ShowProfileViewer(_formProfViewerLine);
				}
				// データがあれば平均値を表示（なければクリア）
				labelAverageColor.Text = (pData != null && pData.Any()) ? $"平均色 {AverageColorString(pData)}" : "";
			}

			// ボックスモード
			if (_isProfModeLine.Value == false)
			{
				var pDataH = CalcBoxHProfile();
				if (_formProfViewerBoxH == null)
				{
					_formProfViewerBoxH = new FormProfileViewer(pDataH, "水平プロファイル", null, 0.0, this.Icon, OnBoxHProfileCursorChanged, OnBoxHProfileRangeChanged, OnProfileViewerForegroundModeChanged, () => ToggleProfileMode());
					ShowProfileViewer(_formProfViewerBoxH);
				}
				else
				{
					_formProfViewerBoxH.SetData(pDataH, 0.0);
					if (isForce) ShowProfileViewer(_formProfViewerBoxH);
				}

				var pDataV = CalcBoxVProfile();
				if (_formProfViewerBoxV == null)
				{
					_formProfViewerBoxV = new FormProfileViewer(pDataV, "垂直プロファイル", null, 0.0, this.Icon, OnBoxVProfileCursorChanged, OnBoxVProfileRangeChanged, OnProfileViewerForegroundModeChanged, () => ToggleProfileMode());
					ShowProfileViewer(_formProfViewerBoxV);
				}
				else
				{
					_formProfViewerBoxV.SetData(pDataV, 0.0);
					if (isForce) ShowProfileViewer(_formProfViewerBoxV);
				}

				// データがあれば平均値を表示（なければクリア）
				labelAverageColor.Text = TryCalculateCurrentProfileAverageColor(out var avgColor, out _) ? $"平均色 {RgbColorString(avgColor)}" : "";

			}

		}


		/// <summary>計測モード群の状態から、ラインプロファイルの計算を行います</summary>
		/// <returns>ラインプロファイル値</returns>
		private Color[] CalcLineProfile(out double distance)
		{
			distance = 0;
			if (_measureModePointB == null || _measureModePointE == null) return null;
			var bx = _measureModePointB.Value.X;
			var by = _measureModePointB.Value.Y;
			var ex = _measureModePointE.Value.X;
			var ey = _measureModePointE.Value.Y;
			if (IsPointInSourceImage(bx, by) == false || IsPointInSourceImage(ex, ey) == false) return null;

			distance = Math.Sqrt((bx - ex) * (bx - ex) + (by - ey) * (by - ey));
			return GetLinePixels(_sourceImage, bx, by, ex, ey);
		}


		/// <summary>計測モード群の状態から、ボックスプロファイル(H)の計算を行います</summary>
		/// <returns>ボックスプロファイル(H)値</returns>
		private Color[] CalcBoxHProfile()
		{
			if (_measureModePointB == null || _measureModePointE == null) return null;
			var rect = CreateRectFromMeasurePoint(_measureModePointB.Value, _measureModePointE.Value);
			if (IsRectInBitmap(_sourceImage, rect) == false) return null;

			return GetProfileColorsH(_sourceImage, rect);
		}


		/// <summary>計測モード群の状態から、ボックスプロファイル(V)の計算を行います</summary>
		/// <returns>ボックスプロファイル(V)値</returns>
		private Color[] CalcBoxVProfile()
		{
			if (_measureModePointB == null || _measureModePointE == null) return null;
			var rect = CreateRectFromMeasurePoint(_measureModePointB.Value, _measureModePointE.Value);
			if (IsRectInBitmap(_sourceImage, rect) == false) return null;

			return GetProfileColorsV(_sourceImage, rect);
		}

		/// <summary>計測モード群の状態から、ボックスプロファイルを取得します</summary>
		/// <returns>ボックスプロファイル値（ボックス内全ピクセル）</returns>
		private Color[] GetBoxProfile()
		{
			if (_measureModePointB == null || _measureModePointE == null) return null;
			var rect = CreateRectFromMeasurePoint(_measureModePointB.Value, _measureModePointE.Value);
			if (IsRectInBitmap(_sourceImage, rect) == false) return null;

			return GetBoxPixels(_sourceImage, rect);
		}


		/// <summary>
		/// 画像内から矩形（水平方向の情報、縦に平均を取る）水平プロファイルを取得します</summary>
		/// <param name="bmp">ビットマップ</param>
		/// <param name="rect">矩形</param>
		/// <returns>水平プロファイル</returns>
		[SupportedOSPlatform("windows")]
		private unsafe Color[] GetProfileColorsH(Bitmap bmp, Rectangle rect)
		{
			try
			{
				if (IsRectInBitmap(bmp, rect) == false) return null;

				var bpp = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
				if (bpp < 3) return null;

				var ret = new Color[rect.Width];

				var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
				try
				{
					var bmpScan0 = (byte*)bmpData.Scan0;
					var bmpStride = bmpData.Stride;

					// 座標群からそれぞれの位置の色を加算する
					for (var ixx = 0; ixx < rect.Width; ixx++)
					{
						var xx = rect.X + ixx;
						var sumB = 0L;
						var sumG = 0L;
						var sumR = 0L;
						for (var iyy = 0; iyy < rect.Height; iyy++)
						{
							var yy = rect.Y + iyy;
							var sp = bmpScan0 + yy * bmpStride + xx * bpp;
							sumB += sp[0];
							sumG += sp[1];
							sumR += sp[2];
						}
						var b = (int)Math.Round(((double)sumB / rect.Height), MidpointRounding.AwayFromZero);
						var g = (int)Math.Round(((double)sumG / rect.Height), MidpointRounding.AwayFromZero);
						var r = (int)Math.Round(((double)sumR / rect.Height), MidpointRounding.AwayFromZero);
						ret[ixx] = Color.FromArgb(r, g, b);
					}
					return ret;
				}
				catch
				{
					throw;
				}
				finally
				{
					bmp.UnlockBits(bmpData);
				}
			}
			catch( Exception ex )
			{
				MessageBox.Show(String.Format("例外 Message={0}, Stack={1}", ex.Message, ex.StackTrace));
				return null;		// エラー
			}
		}



		/// <summary>
		/// 画像内から矩形（垂直方向の情報、横に平均を取る）垂直プロファイルを取得します</summary>
		/// <param name="bmp">ビットマップ</param>
		/// <param name="rect">矩形</param>
		/// <returns>垂直プロファイル</returns>
		[SupportedOSPlatform("windows")]
		private unsafe Color[] GetProfileColorsV(Bitmap bmp, Rectangle rect)
		{
			try
			{
				if (IsRectInBitmap(bmp, rect) == false) return null;

				var bpp = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
				if (bpp < 3) return null;

				var ret = new Color[rect.Height];

				var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
				try
				{
					var bmpScan0 = (byte*)bmpData.Scan0;
					var bmpStride = bmpData.Stride;

					// 座標群からそれぞれの位置の色を加算する
					for (var iyy = 0; iyy < rect.Height; iyy++)
					{
						var yy = rect.Y + iyy;
						var sumB = 0L;
						var sumG = 0L;
						var sumR = 0L;
						for (var ixx = 0; ixx < rect.Width; ixx++)
						{
							var xx = rect.X + ixx;
							var sp = bmpScan0 + yy * bmpStride + xx * bpp;
							sumB += sp[0];
							sumG += sp[1];
							sumR += sp[2];
						}
						var b = (int)Math.Round(((double)sumB / rect.Width), MidpointRounding.AwayFromZero);
						var g = (int)Math.Round(((double)sumG / rect.Width), MidpointRounding.AwayFromZero);
						var r = (int)Math.Round(((double)sumR / rect.Width), MidpointRounding.AwayFromZero);
						ret[iyy] = Color.FromArgb(r, g, b);
					}
					return ret;
				}
				catch
				{
					throw;
				}
				finally
				{
					bmp.UnlockBits(bmpData);
				}
			}
			catch
			{
				return null;		// エラー
			}
		}


		/// <summary>画像内から矩形内の点（ピクセル情報）群を抽出します</summary>
		/// <param name="bmp">ビットマップ</param>
		/// <param name="bx">開始点 X</param>
		/// <param name="by">開始点 Y</param>
		/// <param name="ex">終了点 X</param>
		/// <param name="ey">終了点 Y</param>
		/// <returns>抽出したピクセル群。エラーの場合 null</returns>
		/// <remarks>修了点は含む</remarks>
		[SupportedOSPlatform("windows")]
		private unsafe Color[] GetBoxPixels(Bitmap bmp, int bx, int by, int ex, int ey)
		{
			try
			{
				// 矩形を作成
				var rect = CreateRectFromMeasurePoint(bx, by, ex, ey);
				if (IsRectInBitmap(bmp, rect) == false) return null;

				var bpp = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
				if (bpp < 3) return null;

				var count = (long)rect.Width * rect.Height;
				if (count > Int32.MaxValue) return null;

				var ret = new Color[(int)count];
				var cnt = 0;

				var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
				try
				{
					var bmpScan0 = (byte*)bmpData.Scan0;
					var bmpStride = bmpData.Stride;

					// 座標群からそれぞれの位置のカラー群に変換する
					for (var yy = 0; yy < rect.Height; yy++)
					{
						var sp = bmpScan0 + (yy + rect.Y) * bmpStride + rect.X * bpp;
						for (var xx = 0; xx < rect.Width; xx++)
						{
							ret[cnt++] = Color.FromArgb(sp[2], sp[1], sp[0]);			// sp・・B,G,Rの順
							sp += bpp;
						}
					}

					return ret;
				}
				catch
				{
					throw;
				}
				finally
				{
					bmp.UnlockBits(bmpData);
				}
			}
			catch
			{
				return null;		// エラー
			}
		}


		/// <summary>画像内から矩形内の点（ピクセル情報）群を抽出します</summary>
		/// <param name="bmp">ビットマップ</param>
		/// <param name="rect">矩形</param>
		/// <returns>抽出したピクセル群。エラーの場合 null</returns>
		/// <remarks>Rect終点は含まない</remarks>
		private unsafe Color[] GetBoxPixels(Bitmap bmp, Rectangle rect)
		{
			return GetBoxPixels(bmp, rect.X, rect.Y, rect.X + rect.Width - 1, rect.Y + rect.Height - 1);
		}



		/// <summary>画像内からラインが通過する点（ピクセル情報）群を抽出します</summary>
		/// <param name="bmp">ビットマップ</param>
		/// <param name="bx">開始点 X</param>
		/// <param name="by">開始点 Y</param>
		/// <param name="ex">終了点 X</param>
		/// <param name="ey">終了点 Y</param>
		/// <returns>抽出したピクセル群。エラーの場合 null</returns>
		[SupportedOSPlatform("windows")]
		private unsafe Color[] GetLinePixels(Bitmap bmp, int bx, int by, int ex, int ey)
		{
			try
			{
				if (IsPointInBitmap(bmp, bx, by) == false || IsPointInBitmap(bmp, ex, ey) == false) return null;

				var bpp = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
				if (bpp < 3) return null;

				// 直線の通る点群を求める
				var poss = CreatePossLine(bx, by, ex, ey);

				var ret = new Color[poss.Length];

				var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
				try
				{
					var bmpScan0 = (byte*)bmpData.Scan0;
					var bmpStride = bmpData.Stride;

					// 座標群からそれぞれの位置のカラー群に変換する
					for (var i = 0; i < poss.Length; i++)
					{
						var p = poss[i];
						if (IsPointInBitmap(bmp, p.X, p.Y) == false) return null;

						var sp = bmpScan0 + p.Y * bmpStride + p.X * bpp;
						ret[i] = Color.FromArgb(sp[2], sp[1], sp[0]);			// sp・・B,G,Rの順
					}

					return ret;
				}
				catch
				{
					throw;
				}
				finally
				{
					bmp.UnlockBits(bmpData);
				}
			}
			catch
			{
				return null;		// エラー
			}
		}



		/// <summary>
		/// 2点間を通るラインの座標群を生成します</summary>
		/// <param name="bx">開始点 X</param>
		/// <param name="by">開始点 Y</param>
		/// <param name="ex">終了点 X</param>
		/// <param name="ey">終了点 Y</param>
		/// <returns>ライン座標群</returns>
		/// <remarks>終了点も含む</remarks>
		private Point[] CreatePossLine(int bx, int by, int ex, int ey)
		{
			var list = new List<Point>();

			if (bx == ex && by == ey) throw new Exception(String.Format("CreatePossLine()メソッドの引数は異なる2点である必要があります ({0},{1})", bx, by));

			var stepX = (bx == ex) ? 0 : (ex - bx) / Math.Abs(ex - bx);
			var stepY = (by == ey) ? 0 : (ey - by) / Math.Abs(ey - by);

			// 2点を通る直線の式・パラメータを求めておく   直線の一般式 aaX + bbY + cc = 0
			GetParamAxByC(new Point(bx, by), new Point(ex, ey), out var aa, out var bb, out var cc);

			//　変化量の大きい方を基準にして進める
			var isReverse = false;
			if (Math.Abs(bx - ex) > Math.Abs(by - ey))
			{
				if (stepX < 0)	(bx, ex, stepX, isReverse) = (ex, bx, -stepX, true);
			
				for (int xx = bx; xx <= ex; xx += stepX)
				{
					// Y= -(aaX + cc)/bb;
					var yy = (int)Math.Round(-(aa * xx + cc) / bb, MidpointRounding.AwayFromZero);      // 一般的な四捨五入。　第2引数がないと5を丸めるとき、偶数への丸めとなる
					list.Add(new Point(xx, yy));
				}
			}
			else
			{
				if (stepY < 0) (by, ey, stepY, isReverse) = (ey, by, -stepY, true);
				for (int yy = by; yy <= ey; yy += stepY)
				{
					// X= -(bbY + cc)/aa;
					var xx = (int)Math.Round(-(bb * yy + cc) / aa, MidpointRounding.AwayFromZero);      // 一般的な四捨五入。　第2引数がないと5を丸めるとき、偶数への丸めとなる
					list.Add(new Point(xx, yy));
				}
			}
			if (isReverse ) list.Reverse();
			return list.ToArray();
		}


		/// <summary>
		/// 2点を指定して直線の一般式 aX + bY + c = 0 のa,b,cパラメータを算出する
		/// </summary>
		/// <param name="p0"> 点0</param>
		/// <param name="p1"> 点1</param>
		/// <param name="a"> 算出パラメータ a　【out】 </param>
		/// <param name="b"> 算出パラメータ b　【out】 </param>
		/// <param name="c"> 算出パラメータ c　【out】 </param>
		private void GetParamAxByC(Point p0, Point p1, out double a, out double b, out double c)
		{
			// 直線式 ax + by + c = 0として a,b,c決定
			a = (double)p1.Y - p0.Y;
			b = (double)p0.X - p1.X;                      // a,b 符号入れ替え
			if (a < 0)
			{
				a = -a;
				b = -b;
			}
			// ax + by + c= 0　で始点を通る cを求める。
			c = -a * p0.X - b * p0.Y;
		}


		/// <summary>元画像の開始点・終了点からRectangleを生成します</summary>
		/// <param name="bX">開始点X</param>
		/// <param name="bY">開始点Y</param>
		/// <param name="eX">終了点X</param>
		/// <param name="eY">終了点Y</param>
		/// <returns>生成した矩形(= Rectangle) </returns>
		/// <remarks>開始点・終了点ともに含むとする。また矩形(Rectangleの終点は次の点の開始箇所に線を引く）</remarks>
		private Rectangle CreateRectFromMeasurePoint(int bX, int bY, int eX, int eY)
		{
			int minX, maxX, minY, maxY;

			// x,yともに小さい座標を基点（左上）として矩形を生成する。
			if (bX < eX) { minX = bX; maxX = eX; }
			else { minX = eX; maxX = bX; }

			if (bY < eY) { minY = bY; maxY = eY; }
			else { minY = eY; maxY = bY; }

			return new Rectangle(minX, minY, (maxX - minX) + 1, (maxY - minY) + 1);
		}


		/// <summary>開始点・終了点からRectangleを生成します</summary>
		/// <param name="bgn">開始点</param>
		/// <param name="end">終了点</param>
		/// <returns>生成した矩形(= RectangleF) </returns>
		private Rectangle CreateRectFromMeasurePoint(Point bgn, Point end)
		{
			return CreateRectFromMeasurePoint(bgn.X, bgn.Y, end.X, end.Y);
		}


		/// <summary>元画像の開始点・終了点からBox描画用のRectangleFを生成します</summary>
		/// <param name="bX">開始点X</param>
		/// <param name="bY">開始点Y</param>
		/// <param name="eX">終了点X</param>
		/// <param name="eY">終了点Y</param>
		/// <returns>生成した矩形(= Rectangle) </returns>
		/// <remarks>開始点・終了点ともに含むとする。また矩形(RectangleFの終点は次の点の開始箇所に線を引く）</remarks>
		private RectangleF CreateFRectFromMeasurePoint(int bX, int bY, int eX, int eY)
		{
			int minX, maxX, minY, maxY;

			// x,yともに小さい座標を基点（左上）として矩形を生成する。
			if( bX < eX ) {	minX = bX;	maxX = eX;	}
			else				{	minX = eX;	maxX = bX;	}

			if (bY < eY) 	{	minY = bY;	maxY = eY;	}
			else				{	minY = eY;	maxY = bY;	}

			return new RectangleF(minX, minY, (maxX - minX) + 1, (maxY - minY) + 1);
		}


		/// <summary>開始点・終了点からBox描画用のRectangleFを生成します</summary>
		/// <param name="bgn">開始点</param>
		/// <param name="end">終了点</param>
		/// <returns>生成した矩形(= RectangleF) </returns>
		private RectangleF CreateFRectFromMeasurePoint(Point bgn, Point end)
		{
			return CreateFRectFromMeasurePoint(bgn.X, bgn.Y, end.X, end.Y);
		}


		/// <summary>「CSVファイル出力」が選択されたときの処理です</summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void cSVファイル出力ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_sourceImage== null || _isProfModeLine== null || _measureModePointB == null || _measureModePointE == null) return;

			try
			{
				var bx = _measureModePointB.Value.X;
				var by = _measureModePointB.Value.Y;
				var ex = _measureModePointE.Value.X;
				var ey = _measureModePointE.Value.Y;

				string[] headers;		// ヘッダー行
				Color[] datas;			// ピクセルデータ
				string fname;			// デフォルトファイル名
				if (_isProfModeLine.Value)
				{
					// ラインモードの場合には、2点間を結ぶライン上のピクセル（必ずしも描画ラインと同じではない）を出力する。タイトルに含まれる一部の','は'_'に置き換えておく
					headers = new string[] { String.Format("Line ({0},{1}) - ({2},{3})", bx, by, ex, ey).Replace(',', '_'), "R, G, B" };
					datas = GetLinePixels(_sourceImage, bx, by, ex, ey);
					fname = String.Format("ProfLineData{0}.CSV", DateTime.Now.ToString("yyyyMMddHHmmss"));
				}
				else
				{
					// ボックスモードの場合には、2点を対角とする矩形内のピクセルを出力する。タイトルに含まれる一部の','は'_'に置き換えておく
					headers = new string[] { String.Format("Box ({0},{1}) - ({2},{3})　{4}x{5}", bx, by, ex, ey, Math.Abs(ex - bx) + 1, Math.Abs(ey - by) + 1).Replace(',', '_'), "R, G, B" };
					datas = GetBoxPixels(_sourceImage, bx, by, ex, ey);
					fname = String.Format("ProfBoxData{0}.CSV", DateTime.Now.ToString("yyyyMMddHHmmss"));
				}
				OutCsvfilePixels(datas, headers, fname);
			}
			catch (Exception eee)
			{
				MessageBox.Show(String.Format("データ出力（CSVファイル" +
					"）で例外発生 Message={0}, Stack={1}", eee.Message, eee.StackTrace), "例外", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>「CSVファイル出力」が選択されたときの処理です</summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void cSVクリップボード出力ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_sourceImage == null || _isProfModeLine == null || _measureModePointB == null || _measureModePointE == null) return;

			try
			{
				var bx = _measureModePointB.Value.X;
				var by = _measureModePointB.Value.Y;
				var ex = _measureModePointE.Value.X;
				var ey = _measureModePointE.Value.Y;

				string[] headers;       // ヘッダー行
				Color[] datas;          // ピクセルデータ
				if (_isProfModeLine.Value)
				{
					// ラインモードの場合には、2点間を結ぶライン上のピクセル（必ずしも描画ラインと同じではない）を出力する。タイトルに含まれる一部の','は'_'に置き換えておく
					headers = new string[] { String.Format("Line ({0},{1}) - ({2},{3})", bx, by, ex, ey).Replace(',', '_'), "R\t G\t B" };
					datas = GetLinePixels(_sourceImage, bx, by, ex, ey);
				}
				else
				{
					// ボックスモードの場合には、2点を対角とする矩形内のピクセルを出力する。タイトルに含まれる一部の','は'_'に置き換えておく
					headers = new string[] { String.Format("Box ({0},{1}) - ({2},{3})　{4}x{5}", bx, by, ex, ey, Math.Abs(ex - bx) + 1, Math.Abs(ey - by) + 1).Replace(',', '_'), "R\t G\t B" };
					datas = GetBoxPixels(_sourceImage, bx, by, ex, ey);
				}
				var w = String.Join("\r\n", CreatePxCsvFormat(datas, headers, "\t"));

				Clipboard.SetDataObject(w, true);
			}
			catch (Exception eee)
			{
				MessageBox.Show(String.Format("データ出力（クリップボード）で例外発生 Message={0}, Stack={1}", eee.Message, eee.StackTrace), "例外", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// ピクセル群およびヘッダーを指定してCSVフォーマット文字列群を生成します</summary>
		/// <param name="cols"></param>
		/// <param name="headers"></param>
		/// <param name="pad">連結文字列（通常、ファイル出力時はカンマ[=デフォルト]、クリップボード時はタブを指定する）</param>
		/// <returns></returns>
		private string[] CreatePxCsvFormat(Color[] cols, string[] headers, string pad= "," )
		{
			var w = cols.Select(c => String.Format("{1}{0}{2}{0}{3}", pad, c.R, c.G, c.B)).ToArray();
			if (headers != null && headers.Any())
			{
				w = headers.Concat(w).ToArray();
			}
			return w;
		}


		/// <summary>
		/// 指定されたピクセルデータをCSV形式で出力します</summary>
		/// <param name="datas">ピクセルデータ群</param>
		/// <param name="headers">ヘッダー行（複数）。不要の場合にはnullを指定する</param>
		/// <param name="defname">デフォルトのファイル名</param>
		/// <returns>成否</returns>
		/// <remarks>ファイルダイアログで出力先を決定する</remarks>
		[SupportedOSPlatform("windows")]
		private bool OutCsvfilePixels(Color[] datas, string[] headers, string defname)
		{
			try
			{
				var sfd = new SaveFileDialog();

				sfd.FileName = defname;
				sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				sfd.Filter = "CSVファイル(*.csv)|*.csv|TXTファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
				sfd.FilterIndex = 1;
				sfd.Title = "保存ファイルを選択してください";
				sfd.RestoreDirectory = true;
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					var w = CreatePxCsvFormat(datas, headers, ", ");
					Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // .NET6対応
					File.WriteAllLines(sfd.FileName, w, Encoding.GetEncoding("Shift_JIS"));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(String.Format("ファイル出力で例外発生 Message={0}, Stack={1}", ex.Message, ex.StackTrace), "ファイル出力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			return false;
		}



		/// <summary>「トリミング」が選択されたときの処理です</summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void トリミングToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_isProfModeLine == null || _isProfModeLine == true) return;

			// 開始点、終了点から矩形を生成する。
			var rect = CreateRectFromMeasurePoint(_measureModePointB.Value, _measureModePointE.Value);

			using (var bmp = _sourceImage.Clone(rect, PixelFormat.Format24bppRgb))
			{
				var frm = new FormColorImageS(bmp, String.Format("トリミング {0}", rect.ToString()).Replace( ',', '_'), false, -1, -1, null, null, this.Icon, _actOnClosed, onOpened: _actOnOpened);
				ShowLinkedImageForm(frm);
			}
		}


		/// <summary>
		/// ライン・ボックス指定時に「画像出力」が選択されたときの処理です</summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 画像出力VToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_sourceImage == null || _isProfModeLine != false || _measureModePointB == null || _measureModePointE == null) return;

			try
			{
				// ボックスモード時以外は無視、ボックス内の画像を生成
				var bx = _measureModePointB.Value.X;
				var by = _measureModePointB.Value.Y;
				var ex = _measureModePointE.Value.X;
				var ey = _measureModePointE.Value.Y;
				var rect = CreateRectFromMeasurePoint(_measureModePointB.Value, _measureModePointE.Value);
				using (var wbmp = _sourceImage.Clone(rect, _sourceImage.PixelFormat))
				{
					var fname = $"{this.Text}_Trim_({bx}_{by})_{Math.Abs(ex-bx)+1}x{Math.Abs(ey-by)+1}";
					OutBmpfilePixels(wbmp, fname);
				}
			}
			catch (Exception eee)
			{
				MessageBox.Show(String.Format("データ出力で例外発生 Message={0}, Stack={1}", eee.Message, eee.StackTrace), "例外", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		/// 指定された画像データを出力します</summary>
		/// <param name="bmp">ビットマップ</param>
		/// <param name="defname">デフォルトのファイル名</param>
		/// <returns>成否</returns>
		/// <remarks>ファイルダイアログで出力先を決定する</remarks>
		[SupportedOSPlatform("windows")]
		private bool OutBmpfilePixels(Bitmap bmp, string defname)
		{
			try
			{
				var sfd = new SaveFileDialog();

				sfd.FileName = defname;
				sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				sfd.Filter = UtilityGraphics.ImageSaveFileDialogFilter;
				sfd.FilterIndex = 1;
				sfd.DefaultExt = "png";
				sfd.AddExtension = true;
				sfd.Title = "保存ファイルを選択してください";
				sfd.RestoreDirectory = true;
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					if (UtilityGraphics.TryResolveImageSaveFormat(sfd.FileName, sfd.FilterIndex, out var saveFileName, out var imgfmt) == false ||
						UtilityGraphics.SaveBitmap(bmp, saveFileName, imgfmt) == false)
					{
						MessageBox.Show(String.Format("ファイル（{0}）書き込みが失敗しました", Path.GetFileName(sfd.FileName)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					return true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(String.Format("ファイル出力で例外発生 Message={0}, Stack={1}", ex.Message, ex.StackTrace), "ファイル出力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			return false;
		}



		/// <summary>
		/// コンテキステメニュー（contextMenuStripProfData）が開くときの処理です</summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		/// <remarks>メニュー項目「トリミング」はモードがボックスモードの場合のみ有効とする</remarks>
		[SupportedOSPlatform("windows")]
		private void contextMenuStripProfData_Opening(object sender, CancelEventArgs e)
		{
			トリミングToolStripMenuItem.Enabled = (_isProfModeLine!= null && _isProfModeLine.Value== false);
			if (_averageColorImageToolStripMenuItem != null) _averageColorImageToolStripMenuItem.Enabled = IsProfileRangeSpecified();
		}


		/// <summary>
		/// Fixモードチェックボックスが変化したときの処理です</summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		/// <remarks>スケーラブルピクチャーボックスに固定モード状態を反映する</remarks>
		[SupportedOSPlatform("windows")]
		private void cboxFix_CheckedChanged(object sender, EventArgs e)
		{
			var cbx = sender as CheckBox;
			if( cbx == null || _scalePbox== null ) return;
			_scalePbox.IsFixed = cbx.Checked;
		}


		/// <summary>
		/// キーが押されたときの処理です </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void FormColorImageS_KeyDown(object sender, KeyEventArgs e)
		{
			//　コントロールが押されているとき。
			if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
			{
				// Ctrl+Shift
				if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
				{
					switch (e.KeyCode)
					{
						case Keys.V:
							クリップボードの表示情報インポートToolStripMenuItem_Click(sender, null);
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
						case Keys.C:
							表示情報をクリップボードへToolStripMenuItem_Click(sender, null);
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
						case Keys.Up:
							_opacityNumber = MaxOpcityNumber;   // 100%
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							SetOpacity();
							break;
						case Keys.Down:
							_opacityNumber = 0;         // 最低不透明度
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							this.Opacity = 0.0;         // 完全に透明（押している間だけ→離すと最低不透明度に）
														//SetOpacity();
							break;
						case Keys.Add:
						case Keys.Oemplus:
							_scalePbox.RatePlus(true);
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
						case Keys.Subtract:
						case Keys.OemMinus:
							_scalePbox.RateMinus(true);
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
					}
				}
				// Ctrl+ ALT （ロケーション）  Shiftなし
				else if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
				{
					var loc = this.Location;
					var scr = Screen.FromControl(this).Bounds;
					Point np = new Point(-1, -1);
					// Ctrl+ Alt + 上下左右キーでウィンドウの位置を移動する
					switch (e.KeyCode)
					{
						case Keys.Up:
							np = new Point(this.Location.X, this.Location.Y - 1);
							break;
						case Keys.Down:
							np = new Point(this.Location.X, this.Location.Y + 1);
							break;
						case Keys.Left:
							np = new Point(this.Location.X - 1, this.Location.Y);
							break;
						case Keys.Right:
							np = new Point(this.Location.X + 1, this.Location.Y);
							break;
					}
					if (np.X >= 0 && np.Y >= 0 && scr.Contains(np)) this.Location = np;
				}
				else
				{
					// Ctrl + キーによるショートカット
					switch (e.KeyCode)
					{
						case Keys.V:
							元画像を画像情報画面で表示ToolStripMenuItem_Click(sender, null);
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
						case Keys.C:
							元画像をクリップボードにコピーToolStripMenuItem_Click(sender, null);
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
						case Keys.S:
							元画像をファイルに保存ToolStripMenuItem_Click(sender, null);
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
						case Keys.B:
							if (IsProfileModeToggleKeyTarget())
							{
								ToggleProfileMode();
								e.Handled = true;           // 以下のコントロールにキーを渡さない
							}
							break;
						case Keys.T:
							EditTitleMode();
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
						case Keys.Up:
							if (_opacityNumber < MaxOpcityNumber - 1) _opacityNumber++;
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							SetOpacity();
							break;
						case Keys.Down:
							if (_opacityNumber > 0) _opacityNumber--;
							SetOpacity();
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
						case Keys.Add:
						case Keys.Oemplus:
							_scalePbox.RatePlus(false);
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
						case Keys.Subtract:
						case Keys.OemMinus:
							_scalePbox.RateMinus(false);
							e.Handled = true;           // 以下のコントロールにキーを渡さない
							break;
						case Keys.Space:
							// Ctrl+スペースで一度閉じたプロファイルを再表示する
							if (_isProfModeLine != null)
							{
								if (_isProfModeLine == true)
								{
									ShowProfileViewer(_formProfViewerLine);
								}
								else
								{
									ShowProfileViewer(_formProfViewerBoxH);
									ShowProfileViewer(_formProfViewerBoxV);
								}
							}
							break;
						default:
							// コントロールキーだけの押下
							if (e.KeyCode == Keys.Control)
							{
								// プロファイル中かつ指定中でない場合・・
								if (_isProfModeLine != null)
								{
								}

							}
							break;
					}
				}
			}
			// Ctrlが押されていない場合（単独キー）
			else
			{
				switch (e.KeyCode)
				{
					// エスケープキー
					case Keys.Escape:
						// タイトル編集中（モード）の場合、
						if (lblEditTitle.Visible == true)
						{
							EditTitleMode();
						}
						break;
				}
			}
		}

		/// <summary>
		/// フォームの不透明度を設定します。</summary>
		[SupportedOSPlatform("windows")]
		private void SetOpacity()
		{
			this.Opacity = _opacityNumber * (1.0 - MinOpcityValue) / MaxOpcityNumber + MinOpcityValue;
		}



		/// <summary>
		/// 「数値入力」が選択されたときの処理です</summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void 数値入力ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var frm = new FormDirectNumerical(_sourceImage.Size,  _measureModePointB, _measureModePointE);
			if (frm.ShowDialog() == DialogResult.OK)
			{
				var w = frm.GetArea();
				rbtnProfModeOff.Checked = true;
				if (w.Bgn.X == w.End.X || w.Bgn.Y == w.End.Y) rbtnProfModeLine.Checked = true;
				else rbtnProfModeBox.Checked = true;

				_measureModePointB = w.Bgn;
				_measureModePointE = w.End;
				_scalePbox.DrawMainImage();
				Refresh();
			}
		}



		/// <summary>
		/// エディットタイトル開始時に保存した、変更時点でのタイトルを保持します </summary>
		private string _saveOldTitle;


		/// <summary>
		/// エディットタイトル（タイトルの修正）モードの変更を行います</summary>
		/// <param name="dRet">モード終了時において破棄して終了の選択肢の有無 (Yes/No/Cancel  or OK/Cancel)</param>
		[SupportedOSPlatform("windows")]
		private void EditTitleMode( bool isOption_DoNotReflect= true  )
		{
			// エディットタイトルモードがオフ　(lblEditTitle.Visibleを判定に用いる）
			if (lblEditTitle.Visible == false)
			{
				// 関連コントロールを Visible にして、現在のタイトルを保存
				lblEditTitle.Visible = tboxEditTitle.Visible = btnChangeTitle.Visible = true;
				tboxEditTitle.Text = _saveOldTitle = this.Text;
				tboxEditTitle.Focus();
			}
			else
			{
				// 変更されていれば確認ダイアログを出す。結果により｛修正内容を反映・破棄・編集を続ける｝のいずれかを実施。
				var eTit = tboxEditTitle.Text.Trim();
				if (eTit != _saveOldTitle.Trim())
				{
					var btn = isOption_DoNotReflect ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.OKCancel;
					var msg = isOption_DoNotReflect ? "テキストタイトルを変更しますか？ (Yes: 変更  No: 破棄 Cancel:編集を続ける）" : "タイトル変更を確定します。";
					var ret =  MessageBox.Show( msg , "変更確認", btn, MessageBoxIcon.Question);
					if (ret == DialogResult.Cancel) return;                 // モードはオンのままリターンする
					if (ret == DialogResult.Yes || ret== DialogResult.OK ) this.Text = eTit;
				}
				//　表示を消す。　＝モードオフ。
				lblEditTitle.Visible = tboxEditTitle.Visible = btnChangeTitle.Visible = false;
			}
		}


		/// <summary>
		/// エディットタイトル「確定」ボタンがクリックされたときの処理です</summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		[SupportedOSPlatform("windows")]
		private void btnChangeTitle_Click(object sender= null, EventArgs e= null)
		{
			// エディット終了。　確認ダイアログでは「破棄」の選択肢は除く
			EditTitleMode( false );
		}


		/// <summary>
		/// エディットタイトルテキストボックスでキーが押されたときの処理です </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント情報</param>
		/// <remarks>キーがEnterなら、「確定」ボタンをクリックしたものとみなす</remarks>
		[SupportedOSPlatform("windows")]
		private void tboxEditTitle_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				btnChangeTitle_Click();
			}
		}
	}




	// --- FormClorImageS用設定クラス

	/// <summary>
	/// FormClorImageS用設定クラスです
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(PropertyDisplayConverter))]
	public class ColImageSetteings : ICloneable, IComparable
	{

		/// <summary> 表示画像の拡大・縮小時の方法（デフォルト・非縮小時） </summary>
		private InterpolationMode _mainInterpolationMode = InterpolationMode.NearestNeighbor;

		/// <summary> 表示画像の拡大・縮小時の方法（縮小時） </summary>
		private InterpolationMode _mainInterpolationModeReduce = InterpolationMode.HighQualityBilinear;

		/// <summary> 背景（画像のない部分）の色 </summary>
		private Color _backColor = Color.Black;

		/// <summary>プロファイルダイアログの前面表示モード</summary>
		private ProfileViewerForegroundMode _profileViewerForegroundMode = ProfileViewerForegroundMode.ParentForeground;

		/// <summary>プロファイル範囲線の色</summary>
		private Color _profileSelectionLineColor = Color.Yellow;

		/// <summary>プロファイル始点カーソルの色</summary>
		private Color _profileStartCursorColor = Color.Red;

		/// <summary>プロファイル終点カーソルの色</summary>
		private Color _profileEndCursorColor = Color.GreenYellow;

		/// <summary>プロファイル補助カーソルの色</summary>
		private Color _profileGuideCursorColor = Color.Orange;

		/// <summary>プロファイル範囲線の太さ</summary>
		private float _profileSelectionLineWidth = 3.0f;

		/// <summary>プロファイル始点・終点カーソル線の太さ</summary>
		private float _profilePointCursorLineWidth = 2.0f;

		/// <summary>プロファイル線幅の最小値</summary>
		private const float MinProfileLineWidth = 0.5f;

		/// <summary>プロファイル線幅の最大値</summary>
		private const float MaxProfileLineWidth = 20.0f;


		/// <summary>
		/// コンストラクタです
		/// </summary>
		public ColImageSetteings()
		{
			SetDefaultValue();
		}


		/// <summary>
		/// 全メンバ指定のコンストラクタです
		/// </summary>
		/// <param name="interpolMode">表示画像の拡大・縮小時の方法</param>
		/// <param name="interpolModeR">縮小時の表示画像の拡大・縮小時の方法</param>
		/// <param name="bkCol">背景色</param>
		/// <param name="profileViewerForegroundMode">プロファイルダイアログの前面表示モード</param>
		public ColImageSetteings(InterpolationMode interpolMode, InterpolationMode interpolModeR, Color bkCol, ProfileViewerForegroundMode profileViewerForegroundMode = ProfileViewerForegroundMode.ParentForeground)
		{
			_mainInterpolationMode = interpolMode;
			_mainInterpolationModeReduce = interpolModeR;
			_backColor = bkCol;
			_profileViewerForegroundMode = profileViewerForegroundMode;
		}


		/// <summary> デフォルト値のセット </summary>
		[SupportedOSPlatform("windows")]
		public void SetDefaultValue()
		{
			// 最近傍法をデフォルトとする
			_mainInterpolationMode = InterpolationMode.NearestNeighbor;
			// 縮小時はハイクオリティーバイリニアをデフォルトとする
			_mainInterpolationModeReduce = InterpolationMode.HighQualityBilinear;

			_backColor = Color.Black;
			_profileViewerForegroundMode = ProfileViewerForegroundMode.ParentForeground;
			_profileSelectionLineColor = Color.Yellow;
			_profileStartCursorColor = Color.Red;
			_profileEndCursorColor = Color.GreenYellow;
			_profileGuideCursorColor = Color.Orange;
			_profileSelectionLineWidth = 3.0f;
			_profilePointCursorLineWidth = 2.0f;
		}


		/// <summary>
		/// 全メンバ指定比較メソッドです
		/// </summary>
		/// <param name="interpolMode">表示画像の拡大・縮小時の方法</param>
		/// <param name="interpolModeR">縮小時の表示画像の拡大・縮小時の方法</param>
		/// <param name="bkCol">背景色</param>
		/// <param name="profileViewerForegroundMode">プロファイルダイアログの前面表示モード</param>
		/// <returns>比較結果 0:一致</returns>
		public int CompareAllMenber(InterpolationMode mainInterpolationMode, InterpolationMode mainInterpolationModeR, Color bkCol, ProfileViewerForegroundMode profileViewerForegroundMode = ProfileViewerForegroundMode.ParentForeground)
		{
			var w = (int)_mainInterpolationMode.CompareTo(mainInterpolationMode);
			if (w != 0) return w;

			w = (int)_mainInterpolationModeReduce.CompareTo(mainInterpolationModeR);
			if (w != 0) return w;

			w = _backColor.ToArgb() - bkCol.ToArgb();
			if (w != 0) return w;

			w = _profileViewerForegroundMode.CompareTo(profileViewerForegroundMode);
			if (w != 0) return w;

			return w;
		}


		/// <summary>
		/// プロファイル線幅を設定可能範囲に収めます</summary>
		/// <param name="value">線幅</param>
		/// <param name="defaultValue">既定値</param>
		/// <returns>補正後の線幅</returns>
		private static float NormalizeProfileLineWidth(float value, float defaultValue)
		{
			if (float.IsNaN(value) || float.IsInfinity(value)) return defaultValue;
			return Math.Max(MinProfileLineWidth, Math.Min(MaxProfileLineWidth, value));
		}


		/// <summary>
		/// クローンを生成します
		/// </summary>
		/// <returns>生成されたクローンオブジェクト</returns>
		public object Clone()
		{
			var clone = new ColImageSetteings(_mainInterpolationMode, _mainInterpolationModeReduce, _backColor, _profileViewerForegroundMode);
			clone.ProfileSelectionLineColor = _profileSelectionLineColor;
			clone.ProfileStartCursorColor = _profileStartCursorColor;
			clone.ProfileEndCursorColor = _profileEndCursorColor;
			clone.ProfileGuideCursorColor = _profileGuideCursorColor;
			clone.ProfileSelectionLineWidth = _profileSelectionLineWidth;
			clone.ProfilePointCursorLineWidth = _profilePointCursorLineWidth;
			return clone;
		}

		/// <summary>
		/// オブジェクトを比較します
		/// </summary>
		/// <param name="obj">比較対象オブジェクト</param>
		/// <returns>比較結果</returns>
		public int CompareTo(object obj)
		{
			var dst = (ColImageSetteings)obj;
			var w = CompareAllMenber(dst.MainInterpolationMode, dst.MainInterpolationModeReduce, dst.BackColor, dst.ProfileViewerForegroundMode);
			if (w != 0) return w;

			w = _profileSelectionLineColor.ToArgb() - dst.ProfileSelectionLineColor.ToArgb();
			if (w != 0) return w;

			w = _profileStartCursorColor.ToArgb() - dst.ProfileStartCursorColor.ToArgb();
			if (w != 0) return w;

			w = _profileEndCursorColor.ToArgb() - dst.ProfileEndCursorColor.ToArgb();
			if (w != 0) return w;

			w = _profileGuideCursorColor.ToArgb() - dst.ProfileGuideCursorColor.ToArgb();
			if (w != 0) return w;

			w = _profileSelectionLineWidth.CompareTo(dst.ProfileSelectionLineWidth);
			if (w != 0) return w;

			w = _profilePointCursorLineWidth.CompareTo(dst.ProfilePointCursorLineWidth);
			return w;

		}

		[Category("１．表示設定")]
		[Description("表示画像の拡大・縮小時の方法（デフォルト・非縮小時）")]
		[DefaultValue(InterpolationMode.NearestNeighbor)]
		[PropertyDisplayName("01.描画／補間方式")]
		/// <summary> 表示画像の拡大・縮小時の方法を取得または設定します </summary>
		public InterpolationMode MainInterpolationMode
		{
			set { _mainInterpolationMode = value; }
			get { return _mainInterpolationMode; }
		}

		[Category("１．表示設定")]
		[Description("縮小時の表示画像の拡大・縮小時の方法")]
		[DefaultValue(InterpolationMode.HighQualityBilinear)]
		[PropertyDisplayName("02.描画／補間方式（縮小時）")]
		/// <summary> 表示画像の拡大・縮小時の方法を取得または設定します </summary>
		public InterpolationMode MainInterpolationModeReduce
		{
			set { _mainInterpolationModeReduce = value; }
			get { return _mainInterpolationModeReduce; }
		}
		[Category("１．表示設定")]
		[Description("背景色（画像のない部分）の色を取得または設定します")]
		[DefaultValue(0)]
		[PropertyDisplayName("03.背景色")]
		/// <summary> 背景色（画像のない部分）の色を取得または設定します </summary>
		public Color BackColor
		{
			set { _backColor = value; }
			get { return _backColor; }
		}

		[Category("２．プロファイル表示")]
		[Description("プロファイルダイアログの前面表示モード。ParentForeground: 画像情報画面の前面、AlwaysTopMost: 常に最前面、Normal: 通常表示")]
		[DefaultValue(ProfileViewerForegroundMode.ParentForeground)]
		[PropertyDisplayName("04.プロファイルダイアログ前面表示")]
		/// <summary>プロファイルダイアログの前面表示モードを取得または設定します</summary>
		public ProfileViewerForegroundMode ProfileViewerForegroundMode
		{
			set { _profileViewerForegroundMode = value; }
			get { return _profileViewerForegroundMode; }
		}

		[Category("３．プロファイル指定表示")]
		[Description("プロファイル範囲を示す線または矩形の色")]
		[DefaultValue(typeof(Color), "Yellow")]
		[PropertyDisplayName("01.範囲線色")]
		/// <summary>プロファイル範囲線の色を取得または設定します</summary>
		public Color ProfileSelectionLineColor
		{
			set { _profileSelectionLineColor = value; }
			get { return _profileSelectionLineColor; }
		}

		[Category("３．プロファイル指定表示")]
		[Description("プロファイル始点十字カーソルの色")]
		[DefaultValue(typeof(Color), "Red")]
		[PropertyDisplayName("02.始点カーソル色")]
		/// <summary>プロファイル始点カーソルの色を取得または設定します</summary>
		public Color ProfileStartCursorColor
		{
			set { _profileStartCursorColor = value; }
			get { return _profileStartCursorColor; }
		}

		[Category("３．プロファイル指定表示")]
		[Description("プロファイル終点十字カーソルの色")]
		[DefaultValue(typeof(Color), "GreenYellow")]
		[PropertyDisplayName("03.終点カーソル色")]
		/// <summary>プロファイル終点カーソルの色を取得または設定します</summary>
		public Color ProfileEndCursorColor
		{
			set { _profileEndCursorColor = value; }
			get { return _profileEndCursorColor; }
		}

		[Category("３．プロファイル指定表示")]
		[Description("プロファイルビュー上のカーソルに対応して画像上に表示する補助線の色")]
		[DefaultValue(typeof(Color), "Orange")]
		[PropertyDisplayName("04.補助カーソル色")]
		/// <summary>プロファイル補助カーソルの色を取得または設定します</summary>
		public Color ProfileGuideCursorColor
		{
			set { _profileGuideCursorColor = value; }
			get { return _profileGuideCursorColor; }
		}

		[Category("３．プロファイル指定表示")]
		[Description("プロファイル範囲を示す線または矩形の太さ。表示倍率に応じて見かけの太さを保つよう補正します")]
		[DefaultValue(3.0f)]
		[PropertyDisplayName("05.範囲線太さ")]
		/// <summary>プロファイル範囲線の太さを取得または設定します</summary>
		public float ProfileSelectionLineWidth
		{
			set { _profileSelectionLineWidth = NormalizeProfileLineWidth(value, 3.0f); }
			get { return _profileSelectionLineWidth; }
		}

		[Category("３．プロファイル指定表示")]
		[Description("プロファイル始点・終点十字カーソルの線の太さ。表示倍率に応じて見かけの太さを保つよう補正します")]
		[DefaultValue(2.0f)]
		[PropertyDisplayName("06.始点終点線太さ")]
		/// <summary>プロファイル始点・終点カーソル線の太さを取得または設定します</summary>
		public float ProfilePointCursorLineWidth
		{
			set { _profilePointCursorLineWidth = NormalizeProfileLineWidth(value, 2.0f); }
			get { return _profilePointCursorLineWidth; }
		}

	}
}
