namespace ColorImageViewProject
{
	partial class SetingsRateForm
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.textBoxRate = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(131, 67);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(212, 67);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "決定";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// textBoxRate
			// 
			this.textBoxRate.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textBoxRate.Location = new System.Drawing.Point(162, 16);
			this.textBoxRate.Name = "textBoxRate";
			this.textBoxRate.Size = new System.Drawing.Size(89, 23);
			this.textBoxRate.TabIndex = 0;
			this.textBoxRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxRate.Enter += new System.EventHandler(this.textBoxRate_Enter);
			this.textBoxRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxRate_KeyDown);
			this.textBoxRate.Leave += new System.EventHandler(this.textBoxRate_Leave);
			this.textBoxRate.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBoxRate_PreviewKeyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(257, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(11, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "%";
			// 
			// SetingsRateForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(299, 109);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxRate);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Name = "SetingsRateForm";
			this.Text = "倍率設定";
			this.Load += new System.EventHandler(this.SetingsRateForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox textBoxRate;
		private System.Windows.Forms.Label label1;
	}
}