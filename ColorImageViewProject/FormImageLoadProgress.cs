using System;
using System.Drawing;
using System.Windows.Forms;

namespace ColorImageViewProject
{
	/// <summary>
	/// 画像読み込み進捗フォームです</summary>
	public class FormImageLoadProgress : Form
	{
		/// <summary>処理状況ラベルを保持します</summary>
		private readonly Label _labelStatus;

		/// <summary>ファイル名ラベルを保持します</summary>
		private readonly Label _labelFileName;

		/// <summary>進捗バーを保持します</summary>
		private readonly ProgressBar _progressBar;

		/// <summary>キャンセルボタンを保持します</summary>
		private readonly Button _btnCancel;

		/// <summary>キャンセル要求イベントを保持します</summary>
		public event EventHandler CancelRequested;


		/// <summary>
		/// コンストラクタです</summary>
		public FormImageLoadProgress()
		{
			Text = "画像読み込み";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			ControlBox = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			ClientSize = new Size(420, 142);

			_labelStatus = new Label
			{
				AutoSize = false,
				Location = new Point(16, 14),
				Size = new Size(388, 22),
				Text = "画像を読み込んでいます..."
			};

			_labelFileName = new Label
			{
				AutoEllipsis = true,
				Location = new Point(16, 42),
				Size = new Size(388, 22),
				Text = ""
			};

			_progressBar = new ProgressBar
			{
				Location = new Point(16, 70),
				Size = new Size(388, 20),
				Minimum = 0,
				Maximum = 1,
				Value = 0,
				MarqueeAnimationSpeed = 30
			};

			_btnCancel = new Button
			{
				Location = new Point(310, 104),
				Size = new Size(94, 28),
				Text = "キャンセル",
				UseVisualStyleBackColor = true
			};
			_btnCancel.Click += BtnCancel_Click;

			Controls.Add(_labelStatus);
			Controls.Add(_labelFileName);
			Controls.Add(_progressBar);
			Controls.Add(_btnCancel);
		}


		/// <summary>
		/// 処理中ファイルの進捗表示を更新します</summary>
		/// <param name="currentFileNumber">処理中ファイル番号</param>
		/// <param name="total">総件数</param>
		/// <param name="fileName">処理中ファイル名</param>
		public void SetCurrentFile(int currentFileNumber, int total, string fileName)
		{
			var max = Math.Max(1, total);
			var value = Math.Min(Math.Max(1, currentFileNumber), max);

			_progressBar.Style = ProgressBarStyle.Marquee;
			_labelStatus.Text = $"{value} / {max} 件 読み込み中";
			_labelFileName.Text = fileName ?? "";
			Refresh();
		}


		/// <summary>
		/// 進捗表示を更新します</summary>
		/// <param name="current">処理済み件数</param>
		/// <param name="total">総件数</param>
		/// <param name="fileName">処理中ファイル名</param>
		public void SetProgress(int current, int total, string fileName)
		{
			var max = Math.Max(1, total);
			var value = Math.Min(Math.Max(0, current), max);

			_progressBar.Style = ProgressBarStyle.Blocks;
			_progressBar.Maximum = max;
			_progressBar.Value = value;
			_labelStatus.Text = $"{value} / {max} 件";
			_labelFileName.Text = fileName ?? "";
			Refresh();
		}


		/// <summary>
		/// キャンセルボタンがクリックされた時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void BtnCancel_Click(object sender, EventArgs e)
		{
			_btnCancel.Enabled = false;
			_labelStatus.Text = "キャンセル中...";
			CancelRequested?.Invoke(this, EventArgs.Empty);
		}
	}
}
