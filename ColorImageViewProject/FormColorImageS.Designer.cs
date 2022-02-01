namespace ColorImageViewProject
{
	partial class FormColorImageS
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
			this.pboxColImage = new System.Windows.Forms.PictureBox();
			this.contextMenuStripL = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.表示画像を保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.表示画像をクリップボードにコピーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.表示画像を画像情報画面で表示scalPboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.元画像をファイルに保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.元画像をクリップボードにコピーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.元画像を画像情報画面で表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.表示情報をクリップボードへToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.クリップボードの表示情報インポートToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.余白の削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.FormClorImageS設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.表示枠情報を別の画像情報画面に表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.プロファイル数値入力ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.labelDisplayRate = new System.Windows.Forms.Label();
			this.contextMenuStripRate = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.指定倍率ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.等倍100ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.labelMouseCursor1 = new System.Windows.Forms.Label();
			this.contextMenuStripDispInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.表示サイズ指定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.labelDispInfo = new System.Windows.Forms.Label();
			this.tboxProfInfo = new System.Windows.Forms.TextBox();
			this.contextMenuStripProfData = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cSVファイル出力ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cSVクリップボードToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.トリミングToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.数値入力ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.画像出力VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.labelMouseCursor3 = new System.Windows.Forms.Label();
			this.labelAverageColor = new System.Windows.Forms.Label();
			this.gboxProfileMode = new System.Windows.Forms.GroupBox();
			this.rbtnProfModeBox = new System.Windows.Forms.RadioButton();
			this.rbtnProfModeLine = new System.Windows.Forms.RadioButton();
			this.rbtnProfModeOff = new System.Windows.Forms.RadioButton();
			this.cboxFix = new System.Windows.Forms.CheckBox();
			this.labelProfSize = new System.Windows.Forms.Label();
			this.labelMouseCursor2 = new System.Windows.Forms.Label();
			this.labelMouseCursorXY = new System.Windows.Forms.Label();
			this.lblImageSize = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblSubPx = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pboxColImage)).BeginInit();
			this.contextMenuStripL.SuspendLayout();
			this.contextMenuStripRate.SuspendLayout();
			this.contextMenuStripDispInfo.SuspendLayout();
			this.contextMenuStripProfData.SuspendLayout();
			this.gboxProfileMode.SuspendLayout();
			this.SuspendLayout();
			// 
			// pboxColImage
			// 
			this.pboxColImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pboxColImage.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.pboxColImage.ContextMenuStrip = this.contextMenuStripL;
			this.pboxColImage.Location = new System.Drawing.Point(23, 22);
			this.pboxColImage.Margin = new System.Windows.Forms.Padding(2);
			this.pboxColImage.Name = "pboxColImage";
			this.pboxColImage.Size = new System.Drawing.Size(827, 487);
			this.pboxColImage.TabIndex = 0;
			this.pboxColImage.TabStop = false;
			this.pboxColImage.DoubleClick += new System.EventHandler(this.pboxColImage_DoubleClick);
			this.pboxColImage.Resize += new System.EventHandler(this.pboxColImage_Resize);
			// 
			// contextMenuStripL
			// 
			this.contextMenuStripL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.表示画像を保存ToolStripMenuItem,
            this.表示画像をクリップボードにコピーToolStripMenuItem,
            this.表示画像を画像情報画面で表示scalPboxToolStripMenuItem,
            this.toolStripSeparator1,
            this.元画像をファイルに保存ToolStripMenuItem,
            this.元画像をクリップボードにコピーToolStripMenuItem,
            this.元画像を画像情報画面で表示ToolStripMenuItem,
            this.toolStripSeparator2,
            this.表示情報をクリップボードへToolStripMenuItem,
            this.クリップボードの表示情報インポートToolStripMenuItem,
            this.toolStripSeparator3,
            this.余白の削除ToolStripMenuItem,
            this.toolStripSeparator4,
            this.toolStripSeparator5,
            this.FormClorImageS設定ToolStripMenuItem,
            this.表示枠情報を別の画像情報画面に表示ToolStripMenuItem,
            this.toolStripSeparator6,
            this.プロファイル数値入力ToolStripMenuItem});
			this.contextMenuStripL.Name = "contextMenuStripL";
			this.contextMenuStripL.Size = new System.Drawing.Size(281, 304);
			// 
			// 表示画像を保存ToolStripMenuItem
			// 
			this.表示画像を保存ToolStripMenuItem.Name = "表示画像を保存ToolStripMenuItem";
			this.表示画像を保存ToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.表示画像を保存ToolStripMenuItem.Text = "表示画像をファイルに保存";
			this.表示画像を保存ToolStripMenuItem.Click += new System.EventHandler(this.表示画像をファイルに保存ToolStripMenuItem_Click);
			// 
			// 表示画像をクリップボードにコピーToolStripMenuItem
			// 
			this.表示画像をクリップボードにコピーToolStripMenuItem.Name = "表示画像をクリップボードにコピーToolStripMenuItem";
			this.表示画像をクリップボードにコピーToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.表示画像をクリップボードにコピーToolStripMenuItem.Text = "表示画像をクリップボードにコピー";
			this.表示画像をクリップボードにコピーToolStripMenuItem.Click += new System.EventHandler(this.表示画像をクリップボードにコピーToolStripMenuItem_Click);
			// 
			// 表示画像を画像情報画面で表示scalPboxToolStripMenuItem
			// 
			this.表示画像を画像情報画面で表示scalPboxToolStripMenuItem.Name = "表示画像を画像情報画面で表示scalPboxToolStripMenuItem";
			this.表示画像を画像情報画面で表示scalPboxToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.表示画像を画像情報画面で表示scalPboxToolStripMenuItem.Text = "表示画像を画像情報画面で表示";
			this.表示画像を画像情報画面で表示scalPboxToolStripMenuItem.Click += new System.EventHandler(this.表示画像を画像情報画面で表示ToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(277, 6);
			// 
			// 元画像をファイルに保存ToolStripMenuItem
			// 
			this.元画像をファイルに保存ToolStripMenuItem.Name = "元画像をファイルに保存ToolStripMenuItem";
			this.元画像をファイルに保存ToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.元画像をファイルに保存ToolStripMenuItem.Text = "元画像をファイルに保存(&S)";
			this.元画像をファイルに保存ToolStripMenuItem.Click += new System.EventHandler(this.元画像をファイルに保存ToolStripMenuItem_Click);
			// 
			// 元画像をクリップボードにコピーToolStripMenuItem
			// 
			this.元画像をクリップボードにコピーToolStripMenuItem.Name = "元画像をクリップボードにコピーToolStripMenuItem";
			this.元画像をクリップボードにコピーToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.元画像をクリップボードにコピーToolStripMenuItem.Text = "元画像をクリップボードにコピー(&C)";
			this.元画像をクリップボードにコピーToolStripMenuItem.Click += new System.EventHandler(this.元画像をクリップボードにコピーToolStripMenuItem_Click);
			// 
			// 元画像を画像情報画面で表示ToolStripMenuItem
			// 
			this.元画像を画像情報画面で表示ToolStripMenuItem.Name = "元画像を画像情報画面で表示ToolStripMenuItem";
			this.元画像を画像情報画面で表示ToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.元画像を画像情報画面で表示ToolStripMenuItem.Text = "元画像を画像情報画面で表示(&V)";
			this.元画像を画像情報画面で表示ToolStripMenuItem.Click += new System.EventHandler(this.元画像を画像情報画面で表示ToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(277, 6);
			// 
			// 表示情報をクリップボードへToolStripMenuItem
			// 
			this.表示情報をクリップボードへToolStripMenuItem.Name = "表示情報をクリップボードへToolStripMenuItem";
			this.表示情報をクリップボードへToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.表示情報をクリップボードへToolStripMenuItem.Text = "表示情報をクリップボードへ";
			this.表示情報をクリップボードへToolStripMenuItem.Click += new System.EventHandler(this.表示情報をクリップボードへToolStripMenuItem_Click);
			// 
			// クリップボードの表示情報インポートToolStripMenuItem
			// 
			this.クリップボードの表示情報インポートToolStripMenuItem.Name = "クリップボードの表示情報インポートToolStripMenuItem";
			this.クリップボードの表示情報インポートToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.クリップボードの表示情報インポートToolStripMenuItem.Text = "クリップボードの表示情報インポート";
			this.クリップボードの表示情報インポートToolStripMenuItem.Click += new System.EventHandler(this.クリップボードの表示情報インポートToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(277, 6);
			// 
			// 余白の削除ToolStripMenuItem
			// 
			this.余白の削除ToolStripMenuItem.Name = "余白の削除ToolStripMenuItem";
			this.余白の削除ToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.余白の削除ToolStripMenuItem.Text = "余白の削除(Alt+ダブルクリック）";
			this.余白の削除ToolStripMenuItem.Click += new System.EventHandler(this.余白の削除ToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(277, 6);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(277, 6);
			// 
			// FormClorImageS設定ToolStripMenuItem
			// 
			this.FormClorImageS設定ToolStripMenuItem.Name = "FormClorImageS設定ToolStripMenuItem";
			this.FormClorImageS設定ToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.FormClorImageS設定ToolStripMenuItem.Text = "設定";
			this.FormClorImageS設定ToolStripMenuItem.Click += new System.EventHandler(this.FormClorImageS設定ToolStripMenuItem_Click);
			// 
			// 表示枠情報を別の画像情報画面に表示ToolStripMenuItem
			// 
			this.表示枠情報を別の画像情報画面に表示ToolStripMenuItem.Name = "表示枠情報を別の画像情報画面に表示ToolStripMenuItem";
			this.表示枠情報を別の画像情報画面に表示ToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.表示枠情報を別の画像情報画面に表示ToolStripMenuItem.Text = "表示枠情報を別の画像情報画面に表示";
			this.表示枠情報を別の画像情報画面に表示ToolStripMenuItem.Click += new System.EventHandler(this.表示枠情報を別の画像情報画面に表示ToolStripMenuItem_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(277, 6);
			// 
			// プロファイル数値入力ToolStripMenuItem
			// 
			this.プロファイル数値入力ToolStripMenuItem.Name = "プロファイル数値入力ToolStripMenuItem";
			this.プロファイル数値入力ToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
			this.プロファイル数値入力ToolStripMenuItem.Text = "プロファイル（数値入力）";
			this.プロファイル数値入力ToolStripMenuItem.Click += new System.EventHandler(this.数値入力ToolStripMenuItem_Click);
			// 
			// labelDisplayRate
			// 
			this.labelDisplayRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDisplayRate.AutoSize = true;
			this.labelDisplayRate.ContextMenuStrip = this.contextMenuStripRate;
			this.labelDisplayRate.Location = new System.Drawing.Point(764, 6);
			this.labelDisplayRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelDisplayRate.Name = "labelDisplayRate";
			this.labelDisplayRate.Size = new System.Drawing.Size(29, 12);
			this.labelDisplayRate.TabIndex = 1;
			this.labelDisplayRate.Text = "----";
			this.toolTip1.SetToolTip(this.labelDisplayRate, "表示倍率。右クリックで倍率に関するコンテキストメニュー表示");
			// 
			// contextMenuStripRate
			// 
			this.contextMenuStripRate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.指定倍率ToolStripMenuItem,
            this.等倍100ToolStripMenuItem,
            this.fitToolStripMenuItem});
			this.contextMenuStripRate.Name = "contextMenuStripRate";
			this.contextMenuStripRate.Size = new System.Drawing.Size(233, 70);
			// 
			// 指定倍率ToolStripMenuItem
			// 
			this.指定倍率ToolStripMenuItem.Name = "指定倍率ToolStripMenuItem";
			this.指定倍率ToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.指定倍率ToolStripMenuItem.Text = "指定倍率";
			this.指定倍率ToolStripMenuItem.Click += new System.EventHandler(this.指定倍率ToolStripMenuItem_Click);
			// 
			// 等倍100ToolStripMenuItem
			// 
			this.等倍100ToolStripMenuItem.Name = "等倍100ToolStripMenuItem";
			this.等倍100ToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.等倍100ToolStripMenuItem.Text = "等倍(100%)";
			this.等倍100ToolStripMenuItem.Click += new System.EventHandler(this.等倍100ToolStripMenuItem_Click);
			// 
			// fitToolStripMenuItem
			// 
			this.fitToolStripMenuItem.Name = "fitToolStripMenuItem";
			this.fitToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.fitToolStripMenuItem.Text = "フィット（ダブルクリック）";
			this.fitToolStripMenuItem.Click += new System.EventHandler(this.fitToolStripMenuItem_Click);
			// 
			// labelMouseCursor1
			// 
			this.labelMouseCursor1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMouseCursor1.AutoSize = true;
			this.labelMouseCursor1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelMouseCursor1.Location = new System.Drawing.Point(108, 529);
			this.labelMouseCursor1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelMouseCursor1.Name = "labelMouseCursor1";
			this.labelMouseCursor1.Size = new System.Drawing.Size(53, 12);
			this.labelMouseCursor1.TabIndex = 2;
			this.labelMouseCursor1.Text = "--------";
			this.toolTip1.SetToolTip(this.labelMouseCursor1, "カーソル上の画像情報");
			// 
			// contextMenuStripDispInfo
			// 
			this.contextMenuStripDispInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.表示サイズ指定ToolStripMenuItem});
			this.contextMenuStripDispInfo.Name = "contextMenuStripDispInfo";
			this.contextMenuStripDispInfo.Size = new System.Drawing.Size(161, 26);
			// 
			// 表示サイズ指定ToolStripMenuItem
			// 
			this.表示サイズ指定ToolStripMenuItem.Name = "表示サイズ指定ToolStripMenuItem";
			this.表示サイズ指定ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.表示サイズ指定ToolStripMenuItem.Text = "表示サイズ指定";
			this.表示サイズ指定ToolStripMenuItem.Click += new System.EventHandler(this.表示サイズ指定ToolStripMenuItem_Click);
			// 
			// labelDispInfo
			// 
			this.labelDispInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDispInfo.ContextMenuStrip = this.contextMenuStripDispInfo;
			this.labelDispInfo.Location = new System.Drawing.Point(720, 547);
			this.labelDispInfo.Name = "labelDispInfo";
			this.labelDispInfo.Size = new System.Drawing.Size(130, 17);
			this.labelDispInfo.TabIndex = 4;
			this.labelDispInfo.Text = "[xxxxx,xxxxx]";
			this.labelDispInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.toolTip1.SetToolTip(this.labelDispInfo, "表示ピクチャーボックス枠の大きさ[幅,高さ]。右クリックで固定値に設定可能");
			// 
			// tboxProfInfo
			// 
			this.tboxProfInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tboxProfInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tboxProfInfo.ContextMenuStrip = this.contextMenuStripProfData;
			this.tboxProfInfo.Location = new System.Drawing.Point(292, 546);
			this.tboxProfInfo.Name = "tboxProfInfo";
			this.tboxProfInfo.ReadOnly = true;
			this.tboxProfInfo.Size = new System.Drawing.Size(154, 19);
			this.tboxProfInfo.TabIndex = 6;
			this.tboxProfInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.toolTip1.SetToolTip(this.tboxProfInfo, "右クリックで対象データの出力が可能です");
			// 
			// contextMenuStripProfData
			// 
			this.contextMenuStripProfData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cSVファイル出力ToolStripMenuItem,
            this.cSVクリップボードToolStripMenuItem,
            this.toolStripSeparator7,
            this.トリミングToolStripMenuItem,
            this.数値入力ToolStripMenuItem,
            this.画像出力VToolStripMenuItem});
			this.contextMenuStripProfData.Name = "contextMenuStripProfData";
			this.contextMenuStripProfData.Size = new System.Drawing.Size(203, 120);
			this.contextMenuStripProfData.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripProfData_Opening);
			// 
			// cSVファイル出力ToolStripMenuItem
			// 
			this.cSVファイル出力ToolStripMenuItem.Name = "cSVファイル出力ToolStripMenuItem";
			this.cSVファイル出力ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
			this.cSVファイル出力ToolStripMenuItem.Text = "CSVファイル出力(&S)";
			this.cSVファイル出力ToolStripMenuItem.Click += new System.EventHandler(this.cSVファイル出力ToolStripMenuItem_Click);
			// 
			// cSVクリップボードToolStripMenuItem
			// 
			this.cSVクリップボードToolStripMenuItem.Name = "cSVクリップボードToolStripMenuItem";
			this.cSVクリップボードToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
			this.cSVクリップボードToolStripMenuItem.Text = "CSVクリップボード(&C)";
			this.cSVクリップボードToolStripMenuItem.Click += new System.EventHandler(this.cSVクリップボード出力ToolStripMenuItem_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(199, 6);
			// 
			// トリミングToolStripMenuItem
			// 
			this.トリミングToolStripMenuItem.Name = "トリミングToolStripMenuItem";
			this.トリミングToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
			this.トリミングToolStripMenuItem.Text = "トリミング(&T)";
			this.トリミングToolStripMenuItem.Click += new System.EventHandler(this.トリミングToolStripMenuItem_Click);
			// 
			// 数値入力ToolStripMenuItem
			// 
			this.数値入力ToolStripMenuItem.Name = "数値入力ToolStripMenuItem";
			this.数値入力ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
			this.数値入力ToolStripMenuItem.Text = "数値入力(&N)";
			this.数値入力ToolStripMenuItem.Click += new System.EventHandler(this.数値入力ToolStripMenuItem_Click);
			// 
			// 画像出力VToolStripMenuItem
			// 
			this.画像出力VToolStripMenuItem.Name = "画像出力VToolStripMenuItem";
			this.画像出力VToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
			this.画像出力VToolStripMenuItem.Text = "画像出力(&V)";
			this.画像出力VToolStripMenuItem.Click += new System.EventHandler(this.画像出力VToolStripMenuItem_Click);
			// 
			// labelMouseCursor3
			// 
			this.labelMouseCursor3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMouseCursor3.AutoSize = true;
			this.labelMouseCursor3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelMouseCursor3.Location = new System.Drawing.Point(108, 564);
			this.labelMouseCursor3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelMouseCursor3.Name = "labelMouseCursor3";
			this.labelMouseCursor3.Size = new System.Drawing.Size(53, 12);
			this.labelMouseCursor3.TabIndex = 10;
			this.labelMouseCursor3.Text = "--------";
			this.toolTip1.SetToolTip(this.labelMouseCursor3, "画像情報 YCrCb(0-255,-128-127,-128-127)");
			// 
			// labelAverageColor
			// 
			this.labelAverageColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelAverageColor.AutoSize = true;
			this.labelAverageColor.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelAverageColor.Location = new System.Drawing.Point(290, 568);
			this.labelAverageColor.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelAverageColor.Name = "labelAverageColor";
			this.labelAverageColor.Size = new System.Drawing.Size(53, 12);
			this.labelAverageColor.TabIndex = 2;
			this.labelAverageColor.Text = "--------";
			this.toolTip1.SetToolTip(this.labelAverageColor, "ラインまたはエリアの平均色をしめします");
			// 
			// gboxProfileMode
			// 
			this.gboxProfileMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.gboxProfileMode.Controls.Add(this.rbtnProfModeBox);
			this.gboxProfileMode.Controls.Add(this.rbtnProfModeLine);
			this.gboxProfileMode.Controls.Add(this.rbtnProfModeOff);
			this.gboxProfileMode.Location = new System.Drawing.Point(469, 520);
			this.gboxProfileMode.Name = "gboxProfileMode";
			this.gboxProfileMode.Size = new System.Drawing.Size(210, 45);
			this.gboxProfileMode.TabIndex = 5;
			this.gboxProfileMode.TabStop = false;
			this.gboxProfileMode.Text = "ProfileMode";
			// 
			// rbtnProfModeBox
			// 
			this.rbtnProfModeBox.AutoSize = true;
			this.rbtnProfModeBox.Location = new System.Drawing.Point(150, 19);
			this.rbtnProfModeBox.Name = "rbtnProfModeBox";
			this.rbtnProfModeBox.Size = new System.Drawing.Size(43, 16);
			this.rbtnProfModeBox.TabIndex = 0;
			this.rbtnProfModeBox.Text = "Box";
			this.rbtnProfModeBox.UseVisualStyleBackColor = true;
			this.rbtnProfModeBox.CheckedChanged += new System.EventHandler(this.OnChangeProfileMode);
			// 
			// rbtnProfModeLine
			// 
			this.rbtnProfModeLine.AutoSize = true;
			this.rbtnProfModeLine.Location = new System.Drawing.Point(84, 18);
			this.rbtnProfModeLine.Name = "rbtnProfModeLine";
			this.rbtnProfModeLine.Size = new System.Drawing.Size(44, 16);
			this.rbtnProfModeLine.TabIndex = 0;
			this.rbtnProfModeLine.Text = "Line";
			this.rbtnProfModeLine.UseVisualStyleBackColor = true;
			this.rbtnProfModeLine.CheckedChanged += new System.EventHandler(this.OnChangeProfileMode);
			// 
			// rbtnProfModeOff
			// 
			this.rbtnProfModeOff.AutoSize = true;
			this.rbtnProfModeOff.Checked = true;
			this.rbtnProfModeOff.Location = new System.Drawing.Point(23, 18);
			this.rbtnProfModeOff.Name = "rbtnProfModeOff";
			this.rbtnProfModeOff.Size = new System.Drawing.Size(39, 16);
			this.rbtnProfModeOff.TabIndex = 0;
			this.rbtnProfModeOff.TabStop = true;
			this.rbtnProfModeOff.Text = "Off";
			this.rbtnProfModeOff.UseVisualStyleBackColor = true;
			this.rbtnProfModeOff.CheckedChanged += new System.EventHandler(this.OnChangeProfileMode);
			// 
			// cboxFix
			// 
			this.cboxFix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cboxFix.AutoSize = true;
			this.cboxFix.Location = new System.Drawing.Point(700, 525);
			this.cboxFix.Name = "cboxFix";
			this.cboxFix.Size = new System.Drawing.Size(40, 16);
			this.cboxFix.TabIndex = 7;
			this.cboxFix.Text = "Fix";
			this.cboxFix.UseVisualStyleBackColor = true;
			this.cboxFix.CheckedChanged += new System.EventHandler(this.cboxFix_CheckedChanged);
			// 
			// labelProfSize
			// 
			this.labelProfSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelProfSize.Location = new System.Drawing.Point(376, 518);
			this.labelProfSize.Name = "labelProfSize";
			this.labelProfSize.Size = new System.Drawing.Size(70, 21);
			this.labelProfSize.TabIndex = 8;
			this.labelProfSize.Text = "----------";
			this.labelProfSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelMouseCursor2
			// 
			this.labelMouseCursor2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMouseCursor2.AutoSize = true;
			this.labelMouseCursor2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelMouseCursor2.Location = new System.Drawing.Point(108, 546);
			this.labelMouseCursor2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelMouseCursor2.Name = "labelMouseCursor2";
			this.labelMouseCursor2.Size = new System.Drawing.Size(53, 12);
			this.labelMouseCursor2.TabIndex = 2;
			this.labelMouseCursor2.Text = "--------";
			// 
			// labelMouseCursorXY
			// 
			this.labelMouseCursorXY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMouseCursorXY.AutoSize = true;
			this.labelMouseCursorXY.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelMouseCursorXY.Location = new System.Drawing.Point(21, 529);
			this.labelMouseCursorXY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelMouseCursorXY.Name = "labelMouseCursorXY";
			this.labelMouseCursorXY.Size = new System.Drawing.Size(53, 12);
			this.labelMouseCursorXY.TabIndex = 2;
			this.labelMouseCursorXY.Text = "--------";
			// 
			// lblImageSize
			// 
			this.lblImageSize.AutoSize = true;
			this.lblImageSize.Location = new System.Drawing.Point(87, 8);
			this.lblImageSize.Name = "lblImageSize";
			this.lblImageSize.Size = new System.Drawing.Size(35, 12);
			this.lblImageSize.TabIndex = 9;
			this.lblImageSize.Text = "-----";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(21, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 12);
			this.label2.TabIndex = 9;
			this.label2.Text = "画像サイズ";
			// 
			// lblSubPx
			// 
			this.lblSubPx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSubPx.Location = new System.Drawing.Point(720, 564);
			this.lblSubPx.Name = "lblSubPx";
			this.lblSubPx.Size = new System.Drawing.Size(128, 17);
			this.lblSubPx.TabIndex = 4;
			this.lblSubPx.Text = "(--------,--------)";
			this.lblSubPx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.lblSubPx, "マウスカーソル位置をサブピクセル表現します");
			// 
			// FormColorImageS
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 586);
			this.Controls.Add(this.labelMouseCursor3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblImageSize);
			this.Controls.Add(this.labelProfSize);
			this.Controls.Add(this.cboxFix);
			this.Controls.Add(this.tboxProfInfo);
			this.Controls.Add(this.gboxProfileMode);
			this.Controls.Add(this.lblSubPx);
			this.Controls.Add(this.labelDispInfo);
			this.Controls.Add(this.labelMouseCursor2);
			this.Controls.Add(this.labelMouseCursorXY);
			this.Controls.Add(this.labelAverageColor);
			this.Controls.Add(this.labelMouseCursor1);
			this.Controls.Add(this.labelDisplayRate);
			this.Controls.Add(this.pboxColImage);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "FormColorImageS";
			this.Text = "画像情報画面";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormColorImageS_FormClosed);
			this.Load += new System.EventHandler(this.FormColorImageS_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormColorImageS_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormColorImageS_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormColorImageS_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.pboxColImage)).EndInit();
			this.contextMenuStripL.ResumeLayout(false);
			this.contextMenuStripRate.ResumeLayout(false);
			this.contextMenuStripDispInfo.ResumeLayout(false);
			this.contextMenuStripProfData.ResumeLayout(false);
			this.gboxProfileMode.ResumeLayout(false);
			this.gboxProfileMode.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pboxColImage;
		private System.Windows.Forms.Label labelDisplayRate;
		private System.Windows.Forms.Label labelMouseCursor1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripL;
		private System.Windows.Forms.ToolStripMenuItem 表示画像を保存ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 表示画像をクリップボードにコピーToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 表示画像を画像情報画面で表示scalPboxToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 元画像をファイルに保存ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 元画像をクリップボードにコピーToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem 元画像を画像情報画面で表示ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem 表示情報をクリップボードへToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem クリップボードの表示情報インポートToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripRate;
		private System.Windows.Forms.ToolStripMenuItem fitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 等倍100ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 指定倍率ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem 余白の削除ToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripDispInfo;
		private System.Windows.Forms.ToolStripMenuItem 表示サイズ指定ToolStripMenuItem;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem FormClorImageS設定ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 表示枠情報を別の画像情報画面に表示ToolStripMenuItem;
		private System.Windows.Forms.Label labelDispInfo;
		private System.Windows.Forms.GroupBox gboxProfileMode;
		private System.Windows.Forms.RadioButton rbtnProfModeBox;
		private System.Windows.Forms.RadioButton rbtnProfModeLine;
		private System.Windows.Forms.RadioButton rbtnProfModeOff;
		private System.Windows.Forms.TextBox tboxProfInfo;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripProfData;
		private System.Windows.Forms.ToolStripMenuItem cSVファイル出力ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem トリミングToolStripMenuItem;
		private System.Windows.Forms.CheckBox cboxFix;
		private System.Windows.Forms.Label labelProfSize;
		private System.Windows.Forms.Label labelMouseCursor2;
		private System.Windows.Forms.Label labelMouseCursorXY;
		private System.Windows.Forms.Label lblImageSize;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolStripMenuItem 数値入力ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem プロファイル数値入力ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 画像出力VToolStripMenuItem;
		private System.Windows.Forms.Label labelMouseCursor3;
		private System.Windows.Forms.Label labelAverageColor;
		private System.Windows.Forms.ToolStripMenuItem cSVクリップボードToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.Label lblSubPx;
	}
}