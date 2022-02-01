namespace ColorImageViewProject
{
	partial class FormProfileViewer
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
			this.components = new System.ComponentModel.Container();
			this.pboxGraph = new System.Windows.Forms.PictureBox();
			this.cboxIsDrawGray = new System.Windows.Forms.CheckBox();
			this.cboxIsDrawColR = new System.Windows.Forms.CheckBox();
			this.cboxIsDrawColG = new System.Windows.Forms.CheckBox();
			this.cboxIsDrawColB = new System.Windows.Forms.CheckBox();
			this.cboxIsDrawGridline = new System.Windows.Forms.CheckBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.グラフ画像ファイルに保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.グラフ画像クリップボードToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.グラフ画像表示ツールToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.フォーム全体ファイルに保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.フォーム全体クリップボードToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.フォーム全体表示ツールToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.labelDataQ = new System.Windows.Forms.Label();
			this.tboxDataQ = new System.Windows.Forms.TextBox();
			this.labelDist = new System.Windows.Forms.Label();
			this.tboxDist = new System.Windows.Forms.TextBox();
			this.cboxIsDrawColS = new System.Windows.Forms.CheckBox();
			this.cboxIsDrawColH = new System.Windows.Forms.CheckBox();
			this.gboxHbase = new System.Windows.Forms.GroupBox();
			this.rbtn240 = new System.Windows.Forms.RadioButton();
			this.rbtn120 = new System.Windows.Forms.RadioButton();
			this.rbtn0 = new System.Windows.Forms.RadioButton();
			this.cboxIsDrawGridlineH = new System.Windows.Forms.CheckBox();
			this.labelIdxColInf = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pboxGraph)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.gboxHbase.SuspendLayout();
			this.SuspendLayout();
			// 
			// pboxGraph
			// 
			this.pboxGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pboxGraph.Location = new System.Drawing.Point(13, 11);
			this.pboxGraph.Name = "pboxGraph";
			this.pboxGraph.Size = new System.Drawing.Size(639, 203);
			this.pboxGraph.TabIndex = 0;
			this.pboxGraph.TabStop = false;
			this.pboxGraph.SizeChanged += new System.EventHandler(this.pboxGraph_SizeChanged);
			this.pboxGraph.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pboxGraph_MouseDown);
			this.pboxGraph.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pboxGraph_MouseMove);
			// 
			// cboxIsDrawGray
			// 
			this.cboxIsDrawGray.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cboxIsDrawGray.AutoSize = true;
			this.cboxIsDrawGray.Checked = true;
			this.cboxIsDrawGray.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cboxIsDrawGray.Location = new System.Drawing.Point(248, 245);
			this.cboxIsDrawGray.Name = "cboxIsDrawGray";
			this.cboxIsDrawGray.Size = new System.Drawing.Size(48, 16);
			this.cboxIsDrawGray.TabIndex = 1;
			this.cboxIsDrawGray.Text = "輝度";
			this.cboxIsDrawGray.UseVisualStyleBackColor = true;
			this.cboxIsDrawGray.CheckedChanged += new System.EventHandler(this.OnChangeRbtnSelect);
			// 
			// cboxIsDrawColR
			// 
			this.cboxIsDrawColR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cboxIsDrawColR.AutoSize = true;
			this.cboxIsDrawColR.Location = new System.Drawing.Point(313, 222);
			this.cboxIsDrawColR.Name = "cboxIsDrawColR";
			this.cboxIsDrawColR.Size = new System.Drawing.Size(32, 16);
			this.cboxIsDrawColR.TabIndex = 1;
			this.cboxIsDrawColR.Text = "R";
			this.cboxIsDrawColR.UseVisualStyleBackColor = true;
			this.cboxIsDrawColR.CheckedChanged += new System.EventHandler(this.OnChangeRbtnSelect);
			// 
			// cboxIsDrawColG
			// 
			this.cboxIsDrawColG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cboxIsDrawColG.AutoSize = true;
			this.cboxIsDrawColG.Location = new System.Drawing.Point(349, 222);
			this.cboxIsDrawColG.Name = "cboxIsDrawColG";
			this.cboxIsDrawColG.Size = new System.Drawing.Size(32, 16);
			this.cboxIsDrawColG.TabIndex = 1;
			this.cboxIsDrawColG.Text = "G";
			this.cboxIsDrawColG.UseVisualStyleBackColor = true;
			this.cboxIsDrawColG.CheckedChanged += new System.EventHandler(this.OnChangeRbtnSelect);
			// 
			// cboxIsDrawColB
			// 
			this.cboxIsDrawColB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cboxIsDrawColB.AutoSize = true;
			this.cboxIsDrawColB.Location = new System.Drawing.Point(385, 222);
			this.cboxIsDrawColB.Name = "cboxIsDrawColB";
			this.cboxIsDrawColB.Size = new System.Drawing.Size(32, 16);
			this.cboxIsDrawColB.TabIndex = 1;
			this.cboxIsDrawColB.Text = "B";
			this.cboxIsDrawColB.UseVisualStyleBackColor = true;
			this.cboxIsDrawColB.CheckedChanged += new System.EventHandler(this.OnChangeRbtnSelect);
			// 
			// cboxIsDrawGridline
			// 
			this.cboxIsDrawGridline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cboxIsDrawGridline.AutoSize = true;
			this.cboxIsDrawGridline.Checked = true;
			this.cboxIsDrawGridline.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cboxIsDrawGridline.Location = new System.Drawing.Point(590, 228);
			this.cboxIsDrawGridline.Name = "cboxIsDrawGridline";
			this.cboxIsDrawGridline.Size = new System.Drawing.Size(56, 16);
			this.cboxIsDrawGridline.TabIndex = 1;
			this.cboxIsDrawGridline.Text = "グリッド";
			this.cboxIsDrawGridline.UseVisualStyleBackColor = true;
			this.cboxIsDrawGridline.CheckedChanged += new System.EventHandler(this.OnChangeRbtnSelect);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.グラフ画像ファイルに保存ToolStripMenuItem,
            this.グラフ画像クリップボードToolStripMenuItem,
            this.グラフ画像表示ツールToolStripMenuItem,
            this.toolStripSeparator1,
            this.フォーム全体ファイルに保存ToolStripMenuItem,
            this.フォーム全体クリップボードToolStripMenuItem,
            this.フォーム全体表示ツールToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(245, 142);
			// 
			// グラフ画像ファイルに保存ToolStripMenuItem
			// 
			this.グラフ画像ファイルに保存ToolStripMenuItem.Name = "グラフ画像ファイルに保存ToolStripMenuItem";
			this.グラフ画像ファイルに保存ToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
			this.グラフ画像ファイルに保存ToolStripMenuItem.Text = "グラフ画像→ファイルに保存";
			this.グラフ画像ファイルに保存ToolStripMenuItem.Click += new System.EventHandler(this.グラフ画像をファイルに保存ToolStripMenuItem_Click);
			// 
			// グラフ画像クリップボードToolStripMenuItem
			// 
			this.グラフ画像クリップボードToolStripMenuItem.Name = "グラフ画像クリップボードToolStripMenuItem";
			this.グラフ画像クリップボードToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
			this.グラフ画像クリップボードToolStripMenuItem.Text = "グラフ画像→クリップボード";
			this.グラフ画像クリップボードToolStripMenuItem.Click += new System.EventHandler(this.グラフ画像をクリップボードにコピーToolStripMenuItem_Click);
			// 
			// グラフ画像表示ツールToolStripMenuItem
			// 
			this.グラフ画像表示ツールToolStripMenuItem.Name = "グラフ画像表示ツールToolStripMenuItem";
			this.グラフ画像表示ツールToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
			this.グラフ画像表示ツールToolStripMenuItem.Text = "グラフ画像→表示ツール";
			this.グラフ画像表示ツールToolStripMenuItem.Click += new System.EventHandler(this.グラフ画像を画像情報画面で表示ToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(241, 6);
			// 
			// フォーム全体ファイルに保存ToolStripMenuItem
			// 
			this.フォーム全体ファイルに保存ToolStripMenuItem.Name = "フォーム全体ファイルに保存ToolStripMenuItem";
			this.フォーム全体ファイルに保存ToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
			this.フォーム全体ファイルに保存ToolStripMenuItem.Text = "フォーム全体→ファイルに保存";
			this.フォーム全体ファイルに保存ToolStripMenuItem.Click += new System.EventHandler(this.フォーム全体画像をファイルに保存ToolStripMenuItem_Click);
			// 
			// フォーム全体クリップボードToolStripMenuItem
			// 
			this.フォーム全体クリップボードToolStripMenuItem.Name = "フォーム全体クリップボードToolStripMenuItem";
			this.フォーム全体クリップボードToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
			this.フォーム全体クリップボードToolStripMenuItem.Text = "フォーム全体→クリップボード";
			this.フォーム全体クリップボードToolStripMenuItem.Click += new System.EventHandler(this.フォーム全体画像をクリップボードにコピーToolStripMenuItem_Click);
			// 
			// フォーム全体表示ツールToolStripMenuItem
			// 
			this.フォーム全体表示ツールToolStripMenuItem.Name = "フォーム全体表示ツールToolStripMenuItem";
			this.フォーム全体表示ツールToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
			this.フォーム全体表示ツールToolStripMenuItem.Text = "フォーム全体→表示ツール";
			this.フォーム全体表示ツールToolStripMenuItem.Click += new System.EventHandler(this.フォーム全体画像を画像情報画面で表示ToolStripMenuItem_Click);
			// 
			// labelDataQ
			// 
			this.labelDataQ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelDataQ.AutoSize = true;
			this.labelDataQ.Location = new System.Drawing.Point(19, 243);
			this.labelDataQ.Name = "labelDataQ";
			this.labelDataQ.Size = new System.Drawing.Size(45, 12);
			this.labelDataQ.TabIndex = 2;
			this.labelDataQ.Text = "データ数";
			// 
			// tboxDataQ
			// 
			this.tboxDataQ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tboxDataQ.Location = new System.Drawing.Point(70, 240);
			this.tboxDataQ.Name = "tboxDataQ";
			this.tboxDataQ.ReadOnly = true;
			this.tboxDataQ.Size = new System.Drawing.Size(45, 19);
			this.tboxDataQ.TabIndex = 3;
			this.tboxDataQ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelDist
			// 
			this.labelDist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelDist.AutoSize = true;
			this.labelDist.Location = new System.Drawing.Point(140, 243);
			this.labelDist.Name = "labelDist";
			this.labelDist.Size = new System.Drawing.Size(29, 12);
			this.labelDist.TabIndex = 2;
			this.labelDist.Text = "距離";
			// 
			// tboxDist
			// 
			this.tboxDist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tboxDist.Location = new System.Drawing.Point(176, 240);
			this.tboxDist.Name = "tboxDist";
			this.tboxDist.ReadOnly = true;
			this.tboxDist.Size = new System.Drawing.Size(45, 19);
			this.tboxDist.TabIndex = 3;
			this.tboxDist.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// cboxIsDrawColS
			// 
			this.cboxIsDrawColS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cboxIsDrawColS.AutoSize = true;
			this.cboxIsDrawColS.Location = new System.Drawing.Point(350, 245);
			this.cboxIsDrawColS.Name = "cboxIsDrawColS";
			this.cboxIsDrawColS.Size = new System.Drawing.Size(31, 16);
			this.cboxIsDrawColS.TabIndex = 5;
			this.cboxIsDrawColS.Text = "S";
			this.cboxIsDrawColS.UseVisualStyleBackColor = true;
			this.cboxIsDrawColS.CheckedChanged += new System.EventHandler(this.OnChangeRbtnSelect);
			// 
			// cboxIsDrawColH
			// 
			this.cboxIsDrawColH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cboxIsDrawColH.AutoSize = true;
			this.cboxIsDrawColH.Location = new System.Drawing.Point(313, 245);
			this.cboxIsDrawColH.Name = "cboxIsDrawColH";
			this.cboxIsDrawColH.Size = new System.Drawing.Size(32, 16);
			this.cboxIsDrawColH.TabIndex = 6;
			this.cboxIsDrawColH.Text = "H";
			this.cboxIsDrawColH.UseVisualStyleBackColor = true;
			this.cboxIsDrawColH.CheckedChanged += new System.EventHandler(this.OnChangeRbtnSelect);
			// 
			// gboxHbase
			// 
			this.gboxHbase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.gboxHbase.Controls.Add(this.rbtn240);
			this.gboxHbase.Controls.Add(this.rbtn120);
			this.gboxHbase.Controls.Add(this.rbtn0);
			this.gboxHbase.Location = new System.Drawing.Point(430, 223);
			this.gboxHbase.Name = "gboxHbase";
			this.gboxHbase.Size = new System.Drawing.Size(145, 42);
			this.gboxHbase.TabIndex = 7;
			this.gboxHbase.TabStop = false;
			this.gboxHbase.Text = "H-Base";
			// 
			// rbtn240
			// 
			this.rbtn240.AutoSize = true;
			this.rbtn240.Location = new System.Drawing.Point(98, 19);
			this.rbtn240.Name = "rbtn240";
			this.rbtn240.Size = new System.Drawing.Size(47, 16);
			this.rbtn240.TabIndex = 0;
			this.rbtn240.Text = "240-";
			this.rbtn240.UseVisualStyleBackColor = true;
			this.rbtn240.CheckedChanged += new System.EventHandler(this.rbtn0_CheckedChanged);
			// 
			// rbtn120
			// 
			this.rbtn120.AutoSize = true;
			this.rbtn120.Location = new System.Drawing.Point(48, 19);
			this.rbtn120.Name = "rbtn120";
			this.rbtn120.Size = new System.Drawing.Size(47, 16);
			this.rbtn120.TabIndex = 0;
			this.rbtn120.Text = "120-";
			this.rbtn120.UseVisualStyleBackColor = true;
			this.rbtn120.CheckedChanged += new System.EventHandler(this.rbtn0_CheckedChanged);
			// 
			// rbtn0
			// 
			this.rbtn0.AutoSize = true;
			this.rbtn0.Checked = true;
			this.rbtn0.Location = new System.Drawing.Point(7, 19);
			this.rbtn0.Name = "rbtn0";
			this.rbtn0.Size = new System.Drawing.Size(35, 16);
			this.rbtn0.TabIndex = 0;
			this.rbtn0.TabStop = true;
			this.rbtn0.Text = "0-";
			this.rbtn0.UseVisualStyleBackColor = true;
			this.rbtn0.CheckedChanged += new System.EventHandler(this.rbtn0_CheckedChanged);
			// 
			// cboxIsDrawGridlineH
			// 
			this.cboxIsDrawGridlineH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cboxIsDrawGridlineH.AutoSize = true;
			this.cboxIsDrawGridlineH.Checked = true;
			this.cboxIsDrawGridlineH.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cboxIsDrawGridlineH.Location = new System.Drawing.Point(589, 244);
			this.cboxIsDrawGridlineH.Name = "cboxIsDrawGridlineH";
			this.cboxIsDrawGridlineH.Size = new System.Drawing.Size(64, 16);
			this.cboxIsDrawGridlineH.TabIndex = 1;
			this.cboxIsDrawGridlineH.Text = "グリッドH";
			this.cboxIsDrawGridlineH.UseVisualStyleBackColor = true;
			this.cboxIsDrawGridlineH.CheckedChanged += new System.EventHandler(this.OnChangeRbtnSelect);
			// 
			// labelIdxColInf
			// 
			this.labelIdxColInf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelIdxColInf.AutoSize = true;
			this.labelIdxColInf.Location = new System.Drawing.Point(21, 221);
			this.labelIdxColInf.Name = "labelIdxColInf";
			this.labelIdxColInf.Size = new System.Drawing.Size(0, 12);
			this.labelIdxColInf.TabIndex = 8;
			// 
			// FormProfileViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(664, 270);
			this.ContextMenuStrip = this.contextMenuStrip1;
			this.Controls.Add(this.labelIdxColInf);
			this.Controls.Add(this.gboxHbase);
			this.Controls.Add(this.cboxIsDrawColS);
			this.Controls.Add(this.cboxIsDrawColH);
			this.Controls.Add(this.tboxDist);
			this.Controls.Add(this.labelDist);
			this.Controls.Add(this.tboxDataQ);
			this.Controls.Add(this.labelDataQ);
			this.Controls.Add(this.cboxIsDrawColB);
			this.Controls.Add(this.cboxIsDrawColG);
			this.Controls.Add(this.cboxIsDrawColR);
			this.Controls.Add(this.cboxIsDrawGridlineH);
			this.Controls.Add(this.cboxIsDrawGridline);
			this.Controls.Add(this.cboxIsDrawGray);
			this.Controls.Add(this.pboxGraph);
			this.Name = "FormProfileViewer";
			this.Text = "FormProfileViewer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProfileViewer_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormProfileViewer_FormClosed);
			this.Load += new System.EventHandler(this.FormProfileViewer_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormProfileViewer_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.pboxGraph)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.gboxHbase.ResumeLayout(false);
			this.gboxHbase.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pboxGraph;
		private System.Windows.Forms.CheckBox cboxIsDrawGray;
		private System.Windows.Forms.CheckBox cboxIsDrawColR;
		private System.Windows.Forms.CheckBox cboxIsDrawColG;
		private System.Windows.Forms.CheckBox cboxIsDrawColB;
		private System.Windows.Forms.CheckBox cboxIsDrawGridline;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem グラフ画像ファイルに保存ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グラフ画像クリップボードToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グラフ画像表示ツールToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem フォーム全体ファイルに保存ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem フォーム全体クリップボードToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem フォーム全体表示ツールToolStripMenuItem;
		private System.Windows.Forms.Label labelDataQ;
		private System.Windows.Forms.TextBox tboxDataQ;
		private System.Windows.Forms.Label labelDist;
		private System.Windows.Forms.TextBox tboxDist;
		private System.Windows.Forms.CheckBox cboxIsDrawColS;
		private System.Windows.Forms.CheckBox cboxIsDrawColH;
		private System.Windows.Forms.GroupBox gboxHbase;
		private System.Windows.Forms.RadioButton rbtn240;
		private System.Windows.Forms.RadioButton rbtn120;
		private System.Windows.Forms.RadioButton rbtn0;
		private System.Windows.Forms.CheckBox cboxIsDrawGridlineH;
		private System.Windows.Forms.Label labelIdxColInf;
	}
}