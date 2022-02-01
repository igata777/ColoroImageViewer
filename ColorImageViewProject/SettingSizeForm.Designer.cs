namespace ColorImageViewProject
{
	partial class SettingSizeForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.nudWidth = new System.Windows.Forms.NumericUpDown();
			this.nudHeight = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(210, 76);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 31);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "決定";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(129, 76);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 31);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// nudWidth
			// 
			this.nudWidth.Location = new System.Drawing.Point(43, 32);
			this.nudWidth.Name = "nudWidth";
			this.nudWidth.Size = new System.Drawing.Size(70, 19);
			this.nudWidth.TabIndex = 2;
			this.nudWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// nudHeight
			// 
			this.nudHeight.Location = new System.Drawing.Point(155, 32);
			this.nudHeight.Name = "nudHeight";
			this.nudHeight.Size = new System.Drawing.Size(70, 19);
			this.nudHeight.TabIndex = 3;
			this.nudHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(63, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(17, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "幅";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(179, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(25, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "高さ";
			// 
			// SettingSizeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(309, 119);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nudHeight);
			this.Controls.Add(this.nudWidth);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Name = "SettingSizeForm";
			this.ShowIcon = false;
			this.Text = "サイズ設定";
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.NumericUpDown nudWidth;
		private System.Windows.Forms.NumericUpDown nudHeight;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}