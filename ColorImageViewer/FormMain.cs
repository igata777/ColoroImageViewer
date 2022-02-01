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

				// 引数がある場合には、それらがすべて閉じられるまで透明フォームとして存在する。画像表示フォームから抜ける場合には OnClosedFormが呼ばれるようにしておく。
				var bmps = args.Select(fp => new { Bmp = UtilityGraphics.LoadBitmapPF(fp, PixelFormat.Format24bppRgb), Fn = Path.GetFileName(fp) });
				var frms = bmps.Select(w => new FormColorImageS(w.Bmp, Path.GetFileName(w.Fn), false, -1, -1, null, null, this.Icon, OnClosedForm)).ToArray();
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
			if (_count > 0)
			{
				this.Visible = false;
				this.Opacity = 0;
				this.Enabled = false;
			}
		}


		/// <summary>起動時ファイル指定のフォームが閉じたときのイベントです</summary>
		/// <remarks>残りカウントを減算し、0以下になればフォームを閉じる</remarks>
		private void OnClosedForm()
		{
			if (--_count <= 0) Close();
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
				var frms = bmps.Select(w => new FormColorImageS(w.Bmp, Path.GetFileName(w.Fn), false, -1, -1, null, null, this.Icon, null)).ToArray();
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
