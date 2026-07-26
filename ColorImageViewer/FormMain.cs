using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ColorImageViewProject;
using System.Threading;

// 2025/11/17 ドラッグアンドドロップでは、カウント使用時にはカウント追加する。（カウント使用時はフォームが表示されないので、このケースは現状では発生しない）


namespace ColorImageViewer
{
	/// <summary>
	/// メインフォームクラスです</summary>
	/// <remarks>コマンド引数がある場合には、メイン画面は透明とし、全てのフォームが閉じたときにクローズする。</remarks>
	public partial class FormMain : Form
	{
		/// <summary>起動時ファイル指定の場合の開いているフォーム数を保持します</summary>
		private int _count;

		/// <summary>起動時に指定された画像ファイルパス群を保持します</summary>
		private string[] _initialFilePaths;

		/// <summary>画像読み込み中かどうかを保持します</summary>
		private bool _isOpeningImages;

		/// <summary>起動時画像読み込み中かどうかを保持します</summary>
		private bool _isOpeningInitialFiles;


		/// <summary>
		/// コンストラクタです</summary>
		/// <param name="args">起動時の引数</param>
		public FormMain(string[] args )
		{
			InitializeComponent();

			try
			{
                _count = 0;
				if (args == null || args.Length < 1)
				{
					// ドラッグアンドドロップなどで後からファイルを開くためのフォームとして機能する。
					return;
				}

				_initialFilePaths = args.ToArray();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"例外が発生しました。 Message={ex.Message}, Stack={ex.StackTrace}", "例外発生", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		/// <summary>
		/// ロード時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private async void FormMain_Load(object sender, EventArgs e)
		{
			if (_initialFilePaths != null && _initialFilePaths.Length > 0)
			{
				var filePaths = _initialFilePaths;
				_initialFilePaths = null;
				await OpenFilesAsync(filePaths, true);
				return;
			}

			// 起動時画像ファイルが指定されている場合には、透明にしてメインフォームが見えないようにする
			SetFormVisible();
		}


		/// <summary>
		/// _countの値によりフォームの表示・非表示を設定します</summary>
		private void SetFormVisible()
		{
			var isHidden = _count > 0 || _isOpeningInitialFiles;
			(this.Visible, this.Opacity, this.Enabled) = isHidden ? ( false, 0, false ) : ( true, 1.00, true );

			if( this.Visible )
			{
			    this.WindowState = FormWindowState.Normal;
				this.BringToFront();
				this.Activate();
			}
		}

		/// <summary>
		/// 内部カウンターをクリアしてメインフォーム表示モードにします</summary>
		public void ClearCounter()
		{
			_count = 0;
			SetFormVisible();
		}


		/// <summary>
		/// 画像ファイルを開きます</summary>
		/// <param name="filePaths">ファイルパス群</param>
		/// <param name="isInit">初期化時フラグ</param>
        public void OpenFiles(string[] filePaths, bool isInit)
        {
			_ = OpenFilesAsync(filePaths, isInit);
        }


		/// <summary>
		/// 画像ファイルを非同期で開きます</summary>
		/// <param name="filePaths">ファイルパス群</param>
		/// <param name="isInit">初期化時フラグ</param>
		private async Task OpenFilesAsync(string[] filePaths, bool isInit)
		{
			if (filePaths == null || filePaths.Length < 1) return;

			if (_isOpeningImages)
			{
				MessageBox.Show("画像読み込み中です。完了してから再度実行してください。", "画像読み込み", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			_isOpeningImages = true;
			if (isInit)
			{
				_isOpeningInitialFiles = true;
				SetFormVisible();
			}

			try
			{
				var result = await FormColorImageS.OpenImageFilesAsync(this, filePaths, this.Icon, OnClosedImageForm);
				if (_count > 0 || isInit) _count += result.OpenCount;
				ShowImageLoadResult(result);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"例外が発生しました。 Message={ex.Message}, Stack={ex.StackTrace}", "例外発生", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if (isInit) _isOpeningInitialFiles = false;
				_isOpeningImages = false;
				SetFormVisible();
			}
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



        /// <summary>起動時ファイル指定のフォームが閉じたときのイベントです</summary>
        /// <remarks>残りカウントを減算し、0以下になればフォームを閉じる</remarks>
        private void OnClosedImageForm()
		{
			if( _count > 0 )
			{
				if (--_count <= 0) Close();
			}
		}


		/// <summary>
		/// ドラッグドロップされたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private async void FormMain_DragDrop(object sender, DragEventArgs e)
		{
			//ドロップされたデータがFileDrop型か調べる
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var args = (string[])e.Data.GetData(DataFormats.FileDrop);
				await OpenFilesAsync(args, false);
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
			//そうであればドロップ効果をMoveにする
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				if (((string[])e.Data.GetData(DataFormats.FileDrop)).Length >= 1)
				{
					e.Effect = DragDropEffects.Move;
				}

			}
		}
	}
}
