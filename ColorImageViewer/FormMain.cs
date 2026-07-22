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

				// 引数がある場合には、それらがすべて閉じられるまで透明フォームとして存在する。画像表示フォームから抜ける場合には OnClosedImageForm が呼ばれるようにしておく。
				_count = OpenImageForms(args, OnClosedImageForm, out var failures);
				ShowImageLoadResult(_count, failures);
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
		private void FormMain_Load(object sender, EventArgs e)
		{
            // 起動時画像ファイルが指定されている場合には、透明にしてメインフォームが見えないようにする
            SetFormVisible();
		}


		/// <summary>
		/// _countの値によりフォームの表示・非表示を設定します</summary>
		private void SetFormVisible()
		{
			(this.Visible, this.Opacity, this.Enabled) = (_count > 0 ) ? ( false, 0, false ) : ( true, 1.00, true );

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
            var openCount = OpenImageForms(filePaths, OnClosedImageForm, out var failures);

            if (openCount == 0)
            {
                ShowImageLoadResult(openCount, failures);
                return;
            }

            if( _count> 0 || isInit ) _count += openCount;

			SetFormVisible();
            ShowImageLoadResult(openCount, failures);
        }


		/// <summary>
		/// 指定された画像ファイル群を表示フォームとして開きます</summary>
		/// <param name="filePaths">ファイルパス群</param>
		/// <param name="onClosed">表示フォームクローズ時に呼び出すアクション</param>
		/// <returns>開いたフォーム数</returns>
        private int OpenImageForms(IEnumerable<string> filePaths, Action onClosed, out (string FileName, string Reason)[] failures)
        {
            var images = UtilityGraphics.LoadBitmapFilesPF(filePaths, PixelFormat.Format24bppRgb, out failures);
            var openCount = 0;
            foreach (var image in images)
            {
                using (image.Bmp)
                {
                    var frm = new FormColorImageS(image.Bmp, image.FileName, false, -1, -1, null, null, this.Icon, onClosed);
                    frm.Show();
                    openCount++;
                }
            }
            return openCount;
        }


        /// <summary>
        /// 複数画像読み込み結果を表示します</summary>
        /// <param name="openCount">開いた画像数</param>
        /// <param name="failures">読み込めなかったファイル名と理由</param>
        private static void ShowImageLoadResult(int openCount, (string FileName, string Reason)[] failures)
        {
            if (failures == null || failures.Length == 0)
            {
                if (openCount == 0) MessageBox.Show("有効な画像ファイルが見つかりませんでした。", "画像読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var message = UtilityGraphics.CreateLoadBitmapFilesMessage(openCount, failures);
            var icon = openCount == 0 ? MessageBoxIcon.Error : MessageBoxIcon.Warning;
            var title = openCount == 0 ? "画像読み込みエラー" : "画像読み込み結果";
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
		private void FormMain_DragDrop(object sender, DragEventArgs e)
		{
			//ドロップされたデータがFileDrop型か調べる
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var args = (string[])e.Data.GetData(DataFormats.FileDrop);
				var openCount = OpenImageForms(args, OnClosedImageForm, out var failures);
				if (openCount == 0)
				{
					ShowImageLoadResult(openCount, failures);
					return;
				}
				if( _count > 0 ) _count += openCount;
				SetFormVisible();
				ShowImageLoadResult(openCount, failures);
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
