namespace ColorImageViewProject
{
	partial class FormConfigurationData
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param title="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigurationData));
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_Set = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid1.Location = new System.Drawing.Point(12, 12);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(384, 505);
			this.propertyGrid1.TabIndex = 0;
			this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(224, 523);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
			this.btn_Cancel.TabIndex = 1;
			this.btn_Cancel.Text = "キャンセル";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
			// 
			// btn_Set
			// 
			this.btn_Set.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Set.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_Set.Location = new System.Drawing.Point(321, 523);
			this.btn_Set.Name = "btn_Set";
			this.btn_Set.Size = new System.Drawing.Size(75, 23);
			this.btn_Set.TabIndex = 1;
			this.btn_Set.Text = "設定";
			this.btn_Set.UseVisualStyleBackColor = true;
			this.btn_Set.Click += new System.EventHandler(this.btn_Set_Click);
			// 
			// FormConfigurationData
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(408, 558);
			this.Controls.Add(this.btn_Set);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.propertyGrid1);
//			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormConfigurationData";
			this.Text = "環境設定";
			this.Load += new System.EventHandler(this.FormSetting_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Button btn_Cancel;
		private System.Windows.Forms.Button btn_Set;
	}
}