namespace ColorImageViewProject
{
	partial class FormDirectNumerical
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
			this.nudX0 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.nudY0 = new System.Windows.Forms.NumericUpDown();
			this.nudX1 = new System.Windows.Forms.NumericUpDown();
			this.nudY1 = new System.Windows.Forms.NumericUpDown();
			this.btnSet = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudX0)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudY0)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudX1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudY1)).BeginInit();
			this.SuspendLayout();
			// 
			// nudX0
			// 
			this.nudX0.Location = new System.Drawing.Point(67, 51);
			this.nudX0.Name = "nudX0";
			this.nudX0.Size = new System.Drawing.Size(56, 19);
			this.nudX0.TabIndex = 0;
			this.nudX0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(49, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(341, 19);
			this.label1.TabIndex = 1;
			this.label1.Text = "（           ,           ) - （           ,           ) ";
			// 
			// nudY0
			// 
			this.nudY0.Location = new System.Drawing.Point(139, 51);
			this.nudY0.Name = "nudY0";
			this.nudY0.Size = new System.Drawing.Size(56, 19);
			this.nudY0.TabIndex = 0;
			this.nudY0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// nudX1
			// 
			this.nudX1.Location = new System.Drawing.Point(240, 51);
			this.nudX1.Name = "nudX1";
			this.nudX1.Size = new System.Drawing.Size(56, 19);
			this.nudX1.TabIndex = 0;
			this.nudX1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// nudY1
			// 
			this.nudY1.Location = new System.Drawing.Point(311, 51);
			this.nudY1.Name = "nudY1";
			this.nudY1.Size = new System.Drawing.Size(56, 19);
			this.nudY1.TabIndex = 0;
			this.nudY1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnSet
			// 
			this.btnSet.Location = new System.Drawing.Point(352, 112);
			this.btnSet.Name = "btnSet";
			this.btnSet.Size = new System.Drawing.Size(75, 34);
			this.btnSet.TabIndex = 2;
			this.btnSet.Text = "確定";
			this.btnSet.UseVisualStyleBackColor = true;
			this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(271, 112);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 34);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// FormDirectNumerical
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(439, 158);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSet);
			this.Controls.Add(this.nudY1);
			this.Controls.Add(this.nudX1);
			this.Controls.Add(this.nudY0);
			this.Controls.Add(this.nudX0);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormDirectNumerical";
			this.Text = "数値入力";
			((System.ComponentModel.ISupportInitialize)(this.nudX0)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudY0)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudX1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudY1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown nudX0;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudY0;
		private System.Windows.Forms.NumericUpDown nudX1;
		private System.Windows.Forms.NumericUpDown nudY1;
		private System.Windows.Forms.Button btnSet;
		private System.Windows.Forms.Button btnCancel;
	}
}