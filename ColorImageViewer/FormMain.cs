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
				var bmps = args.Select(fp => new { Bmp = UtilityGraphics.LoadBitmapPF(fp, PixelFormat.Format24bppRgb), Fn = Path.GetFileName(fp) });
				var frms = bmps.Select(w => new FormColorImageS(w.Bmp, Path.GetFileName(w.Fn), false, -1, -1, null, null, this.Icon, OnClosedImageForm)).ToArray();
				_count = frms.Length;
				foreach (var frm in frms)
				{
					frm.Show();
				}
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
            var validImageFiles = filePaths.Where(IsImageFile).ToArray();

            if (validImageFiles.Length == 0)
            {
                MessageBox.Show("有効な画像ファイルが見つかりませんでした。");
                return;
            }

            // 引数がある場合には、それらがすべて閉じられるまで透明フォームとして存在する。画像表示フォームから抜ける場合には OnClosedImageForm が呼ばれるようにしておく。
            var bmps = filePaths.Select(fp => new { Bmp = UtilityGraphics.LoadBitmapPF(fp, PixelFormat.Format24bppRgb), Fn = Path.GetFileName(fp) });
            var frms = bmps.Select(w => new FormColorImageS(w.Bmp, Path.GetFileName(w.Fn), false, -1, -1, null, null, this.Icon, OnClosedImageForm)).ToArray();
            if( _count> 0 || isInit ) _count += frms.Length;
            foreach (var frm in frms)
            {
                frm.Show();
            }

			SetFormVisible();
        }


		/// <summary>
		/// 拡張子が対象の画像ファイルであるか判定を行います</summary>
		/// <param name="filePath">ファイルパス</param>
		/// <returns>対象画像ファイルである場合、true</returns>
        private bool IsImageFile(string filePath)
        {
            if (!File.Exists(filePath)) return false;

            var extension = Path.GetExtension(filePath).ToLower();
            return new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp" }.Contains(extension);
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
				var bmps = args.Select(fp => new { Bmp = UtilityGraphics.LoadBitmapPF(fp, PixelFormat.Format24bppRgb), Fn = Path.GetFileName(fp) }) ;
				var frms = bmps.Select(w => new FormColorImageS(w.Bmp, Path.GetFileName(w.Fn), false, -1, -1, null, null, this.Icon, OnClosedImageForm)).ToArray();
				if( _count > 0 ) _count += frms.Length;
				foreach (var frm in frms) frm.Show();
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
