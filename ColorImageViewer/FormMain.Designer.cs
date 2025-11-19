namespace ColorImageViewer
{
	partial class FormMain
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			label1 = new System.Windows.Forms.Label();
			SuspendLayout();
			// 
			// label1
			// 
			label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			label1.Location = new System.Drawing.Point(80, 48);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(325, 16);
			label1.TabIndex = 0;
			label1.Text = "ここに画像イメージをドラッグアンドドロップして下さい";
			label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormMain
			// 
			AllowDrop = true;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			ClientSize = new System.Drawing.Size(548, 119);
			Controls.Add(label1);
			DoubleBuffered = true;
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			Name = "FormMain";
			Text = "メインフォーム";
			Load += FormMain_Load;
			DragDrop += FormMain_DragDrop;
			DragEnter += FormMain_DragEnter;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label label1;
	}
}

