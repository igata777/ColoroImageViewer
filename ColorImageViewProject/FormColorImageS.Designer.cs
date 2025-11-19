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
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormColorImageS));
			pboxColImage = new System.Windows.Forms.PictureBox();
			contextMenuStripL = new System.Windows.Forms.ContextMenuStrip(components);
			表示画像を保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			表示画像をクリップボードにコピーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			表示画像を画像情報画面で表示scalPboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			元画像をファイルに保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			元画像をクリップボードにコピーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			元画像を画像情報画面で表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			表示情報をクリップボードへToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			クリップボードの表示情報インポートToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			余白の削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			FormClorImageS設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			表示枠情報を別の画像情報画面に表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			プロファイル数値入力ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			labelDisplayRate = new System.Windows.Forms.Label();
			contextMenuStripRate = new System.Windows.Forms.ContextMenuStrip(components);
			指定倍率ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			等倍100ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			fitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			labelMouseCursor1 = new System.Windows.Forms.Label();
			contextMenuStripDispInfo = new System.Windows.Forms.ContextMenuStrip(components);
			表示サイズ指定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			labelDispInfo = new System.Windows.Forms.Label();
			tboxProfInfo = new System.Windows.Forms.TextBox();
			contextMenuStripProfData = new System.Windows.Forms.ContextMenuStrip(components);
			cSVファイル出力ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			cSVクリップボードToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			トリミングToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			数値入力ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			画像出力VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			labelMouseCursor3 = new System.Windows.Forms.Label();
			labelAverageColor = new System.Windows.Forms.Label();
			lblSubPx = new System.Windows.Forms.Label();
			lblEditTitle = new System.Windows.Forms.Label();
			gboxProfileMode = new System.Windows.Forms.GroupBox();
			rbtnProfModeBox = new System.Windows.Forms.RadioButton();
			rbtnProfModeLine = new System.Windows.Forms.RadioButton();
			rbtnProfModeOff = new System.Windows.Forms.RadioButton();
			cboxFix = new System.Windows.Forms.CheckBox();
			labelProfSize = new System.Windows.Forms.Label();
			labelMouseCursor2 = new System.Windows.Forms.Label();
			labelMouseCursorXY = new System.Windows.Forms.Label();
			lblImageSize = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			tboxEditTitle = new System.Windows.Forms.TextBox();
			btnChangeTitle = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)pboxColImage).BeginInit();
			contextMenuStripL.SuspendLayout();
			contextMenuStripRate.SuspendLayout();
			contextMenuStripDispInfo.SuspendLayout();
			contextMenuStripProfData.SuspendLayout();
			gboxProfileMode.SuspendLayout();
			SuspendLayout();
			// 
			// pboxColImage
			// 
			pboxColImage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			pboxColImage.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			pboxColImage.ContextMenuStrip = contextMenuStripL;
			pboxColImage.Location = new System.Drawing.Point(31, 35);
			pboxColImage.Margin = new System.Windows.Forms.Padding(2);
			pboxColImage.Name = "pboxColImage";
			pboxColImage.Size = new System.Drawing.Size(968, 606);
			pboxColImage.TabIndex = 0;
			pboxColImage.TabStop = false;
			pboxColImage.DragDrop += FormMain_DragDrop;
			pboxColImage.DragEnter += FormMain_DragEnter;
			pboxColImage.DoubleClick += pboxColImage_DoubleClick;
			pboxColImage.Resize += pboxColImage_Resize;
			// 
			// contextMenuStripL
			// 
			contextMenuStripL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 表示画像を保存ToolStripMenuItem, 表示画像をクリップボードにコピーToolStripMenuItem, 表示画像を画像情報画面で表示scalPboxToolStripMenuItem, toolStripSeparator1, 元画像をファイルに保存ToolStripMenuItem, 元画像をクリップボードにコピーToolStripMenuItem, 元画像を画像情報画面で表示ToolStripMenuItem, toolStripSeparator2, 表示情報をクリップボードへToolStripMenuItem, クリップボードの表示情報インポートToolStripMenuItem, toolStripSeparator3, 余白の削除ToolStripMenuItem, toolStripSeparator4, toolStripSeparator5, FormClorImageS設定ToolStripMenuItem, 表示枠情報を別の画像情報画面に表示ToolStripMenuItem, toolStripSeparator6, プロファイル数値入力ToolStripMenuItem });
			contextMenuStripL.Name = "contextMenuStripL";
			contextMenuStripL.Size = new System.Drawing.Size(327, 304);
			// 
			// 表示画像を保存ToolStripMenuItem
			// 
			表示画像を保存ToolStripMenuItem.Name = "表示画像を保存ToolStripMenuItem";
			表示画像を保存ToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			表示画像を保存ToolStripMenuItem.Text = "表示画像をファイルに保存";
			表示画像を保存ToolStripMenuItem.Click += 表示画像をファイルに保存ToolStripMenuItem_Click;
			// 
			// 表示画像をクリップボードにコピーToolStripMenuItem
			// 
			表示画像をクリップボードにコピーToolStripMenuItem.Name = "表示画像をクリップボードにコピーToolStripMenuItem";
			表示画像をクリップボードにコピーToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			表示画像をクリップボードにコピーToolStripMenuItem.Text = "表示画像をクリップボードにコピー";
			表示画像をクリップボードにコピーToolStripMenuItem.Click += 表示画像をクリップボードにコピーToolStripMenuItem_Click;
			// 
			// 表示画像を画像情報画面で表示scalPboxToolStripMenuItem
			// 
			表示画像を画像情報画面で表示scalPboxToolStripMenuItem.Name = "表示画像を画像情報画面で表示scalPboxToolStripMenuItem";
			表示画像を画像情報画面で表示scalPboxToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			表示画像を画像情報画面で表示scalPboxToolStripMenuItem.Text = "表示画像を画像情報画面で表示";
			表示画像を画像情報画面で表示scalPboxToolStripMenuItem.Click += 表示画像を画像情報画面で表示ToolStripMenuItem_Click;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(323, 6);
			// 
			// 元画像をファイルに保存ToolStripMenuItem
			// 
			元画像をファイルに保存ToolStripMenuItem.Name = "元画像をファイルに保存ToolStripMenuItem";
			元画像をファイルに保存ToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			元画像をファイルに保存ToolStripMenuItem.Text = "元画像をファイルに保存(&S)";
			元画像をファイルに保存ToolStripMenuItem.Click += 元画像をファイルに保存ToolStripMenuItem_Click;
			// 
			// 元画像をクリップボードにコピーToolStripMenuItem
			// 
			元画像をクリップボードにコピーToolStripMenuItem.Name = "元画像をクリップボードにコピーToolStripMenuItem";
			元画像をクリップボードにコピーToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			元画像をクリップボードにコピーToolStripMenuItem.Text = "元画像をクリップボードにコピー(&C)";
			元画像をクリップボードにコピーToolStripMenuItem.Click += 元画像をクリップボードにコピーToolStripMenuItem_Click;
			// 
			// 元画像を画像情報画面で表示ToolStripMenuItem
			// 
			元画像を画像情報画面で表示ToolStripMenuItem.Name = "元画像を画像情報画面で表示ToolStripMenuItem";
			元画像を画像情報画面で表示ToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			元画像を画像情報画面で表示ToolStripMenuItem.Text = "元画像を画像情報画面で表示(&V)";
			元画像を画像情報画面で表示ToolStripMenuItem.Click += 元画像を画像情報画面で表示ToolStripMenuItem_Click;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new System.Drawing.Size(323, 6);
			// 
			// 表示情報をクリップボードへToolStripMenuItem
			// 
			表示情報をクリップボードへToolStripMenuItem.Name = "表示情報をクリップボードへToolStripMenuItem";
			表示情報をクリップボードへToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			表示情報をクリップボードへToolStripMenuItem.Text = "表示情報をクリップボードへ [ Ctrl +Shift + C ]";
			表示情報をクリップボードへToolStripMenuItem.Click += 表示情報をクリップボードへToolStripMenuItem_Click;
			// 
			// クリップボードの表示情報インポートToolStripMenuItem
			// 
			クリップボードの表示情報インポートToolStripMenuItem.Name = "クリップボードの表示情報インポートToolStripMenuItem";
			クリップボードの表示情報インポートToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			クリップボードの表示情報インポートToolStripMenuItem.Text = "クリップボードの表示情報インポート [ Ctrl +Shift + V ]";
			クリップボードの表示情報インポートToolStripMenuItem.Click += クリップボードの表示情報インポートToolStripMenuItem_Click;
			// 
			// toolStripSeparator3
			// 
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new System.Drawing.Size(323, 6);
			// 
			// 余白の削除ToolStripMenuItem
			// 
			余白の削除ToolStripMenuItem.Name = "余白の削除ToolStripMenuItem";
			余白の削除ToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			余白の削除ToolStripMenuItem.Text = "余白の削除(Alt+ダブルクリック）";
			余白の削除ToolStripMenuItem.Click += 余白の削除ToolStripMenuItem_Click;
			// 
			// toolStripSeparator4
			// 
			toolStripSeparator4.Name = "toolStripSeparator4";
			toolStripSeparator4.Size = new System.Drawing.Size(323, 6);
			// 
			// toolStripSeparator5
			// 
			toolStripSeparator5.Name = "toolStripSeparator5";
			toolStripSeparator5.Size = new System.Drawing.Size(323, 6);
			// 
			// FormClorImageS設定ToolStripMenuItem
			// 
			FormClorImageS設定ToolStripMenuItem.Name = "FormClorImageS設定ToolStripMenuItem";
			FormClorImageS設定ToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			FormClorImageS設定ToolStripMenuItem.Text = "設定";
			FormClorImageS設定ToolStripMenuItem.Click += FormClorImageS設定ToolStripMenuItem_Click;
			// 
			// 表示枠情報を別の画像情報画面に表示ToolStripMenuItem
			// 
			表示枠情報を別の画像情報画面に表示ToolStripMenuItem.Name = "表示枠情報を別の画像情報画面に表示ToolStripMenuItem";
			表示枠情報を別の画像情報画面に表示ToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			表示枠情報を別の画像情報画面に表示ToolStripMenuItem.Text = "表示枠情報を別の画像情報画面に表示";
			表示枠情報を別の画像情報画面に表示ToolStripMenuItem.Click += 表示枠情報を別の画像情報画面に表示ToolStripMenuItem_Click;
			// 
			// toolStripSeparator6
			// 
			toolStripSeparator6.Name = "toolStripSeparator6";
			toolStripSeparator6.Size = new System.Drawing.Size(323, 6);
			// 
			// プロファイル数値入力ToolStripMenuItem
			// 
			プロファイル数値入力ToolStripMenuItem.Name = "プロファイル数値入力ToolStripMenuItem";
			プロファイル数値入力ToolStripMenuItem.Size = new System.Drawing.Size(326, 22);
			プロファイル数値入力ToolStripMenuItem.Text = "プロファイル（数値入力）";
			プロファイル数値入力ToolStripMenuItem.Click += 数値入力ToolStripMenuItem_Click;
			// 
			// labelDisplayRate
			// 
			labelDisplayRate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			labelDisplayRate.AutoSize = true;
			labelDisplayRate.ContextMenuStrip = contextMenuStripRate;
			labelDisplayRate.Location = new System.Drawing.Point(891, 11);
			labelDisplayRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			labelDisplayRate.Name = "labelDisplayRate";
			labelDisplayRate.Size = new System.Drawing.Size(27, 15);
			labelDisplayRate.TabIndex = 1;
			labelDisplayRate.Text = "----";
			toolTip1.SetToolTip(labelDisplayRate, "表示倍率。右クリックで倍率に関するコンテキストメニュー表示");
			// 
			// contextMenuStripRate
			// 
			contextMenuStripRate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 指定倍率ToolStripMenuItem, 等倍100ToolStripMenuItem, fitToolStripMenuItem });
			contextMenuStripRate.Name = "contextMenuStripRate";
			contextMenuStripRate.Size = new System.Drawing.Size(191, 70);
			// 
			// 指定倍率ToolStripMenuItem
			// 
			指定倍率ToolStripMenuItem.Name = "指定倍率ToolStripMenuItem";
			指定倍率ToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			指定倍率ToolStripMenuItem.Text = "指定倍率";
			指定倍率ToolStripMenuItem.Click += 指定倍率ToolStripMenuItem_Click;
			// 
			// 等倍100ToolStripMenuItem
			// 
			等倍100ToolStripMenuItem.Name = "等倍100ToolStripMenuItem";
			等倍100ToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			等倍100ToolStripMenuItem.Text = "等倍(100%)";
			等倍100ToolStripMenuItem.Click += 等倍100ToolStripMenuItem_Click;
			// 
			// fitToolStripMenuItem
			// 
			fitToolStripMenuItem.Name = "fitToolStripMenuItem";
			fitToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			fitToolStripMenuItem.Text = "フィット（ダブルクリック）";
			fitToolStripMenuItem.Click += fitToolStripMenuItem_Click;
			// 
			// labelMouseCursor1
			// 
			labelMouseCursor1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			labelMouseCursor1.AutoSize = true;
			labelMouseCursor1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			labelMouseCursor1.Location = new System.Drawing.Point(126, 668);
			labelMouseCursor1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			labelMouseCursor1.Name = "labelMouseCursor1";
			labelMouseCursor1.Size = new System.Drawing.Size(53, 12);
			labelMouseCursor1.TabIndex = 2;
			labelMouseCursor1.Text = "--------";
			toolTip1.SetToolTip(labelMouseCursor1, "カーソル上の画像情報");
			// 
			// contextMenuStripDispInfo
			// 
			contextMenuStripDispInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 表示サイズ指定ToolStripMenuItem });
			contextMenuStripDispInfo.Name = "contextMenuStripDispInfo";
			contextMenuStripDispInfo.Size = new System.Drawing.Size(151, 26);
			// 
			// 表示サイズ指定ToolStripMenuItem
			// 
			表示サイズ指定ToolStripMenuItem.Name = "表示サイズ指定ToolStripMenuItem";
			表示サイズ指定ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			表示サイズ指定ToolStripMenuItem.Text = "表示サイズ指定";
			表示サイズ指定ToolStripMenuItem.Click += 表示サイズ指定ToolStripMenuItem_Click;
			// 
			// labelDispInfo
			// 
			labelDispInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			labelDispInfo.ContextMenuStrip = contextMenuStripDispInfo;
			labelDispInfo.Location = new System.Drawing.Point(840, 690);
			labelDispInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			labelDispInfo.Name = "labelDispInfo";
			labelDispInfo.Size = new System.Drawing.Size(152, 21);
			labelDispInfo.TabIndex = 4;
			labelDispInfo.Text = "[xxxxx,xxxxx]";
			labelDispInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
			toolTip1.SetToolTip(labelDispInfo, "表示ピクチャーボックス枠の大きさ[幅,高さ]。右クリックで固定値に設定可能");
			// 
			// tboxProfInfo
			// 
			tboxProfInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			tboxProfInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			tboxProfInfo.ContextMenuStrip = contextMenuStripProfData;
			tboxProfInfo.Location = new System.Drawing.Point(341, 689);
			tboxProfInfo.Margin = new System.Windows.Forms.Padding(4);
			tboxProfInfo.Name = "tboxProfInfo";
			tboxProfInfo.ReadOnly = true;
			tboxProfInfo.Size = new System.Drawing.Size(179, 23);
			tboxProfInfo.TabIndex = 6;
			tboxProfInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			toolTip1.SetToolTip(tboxProfInfo, "右クリックで対象データの出力が可能です");
			// 
			// contextMenuStripProfData
			// 
			contextMenuStripProfData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { cSVファイル出力ToolStripMenuItem, cSVクリップボードToolStripMenuItem, toolStripSeparator7, トリミングToolStripMenuItem, 数値入力ToolStripMenuItem, 画像出力VToolStripMenuItem });
			contextMenuStripProfData.Name = "contextMenuStripProfData";
			contextMenuStripProfData.Size = new System.Drawing.Size(169, 120);
			contextMenuStripProfData.Opening += contextMenuStripProfData_Opening;
			// 
			// cSVファイル出力ToolStripMenuItem
			// 
			cSVファイル出力ToolStripMenuItem.Name = "cSVファイル出力ToolStripMenuItem";
			cSVファイル出力ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			cSVファイル出力ToolStripMenuItem.Text = "CSVファイル出力(&S)";
			cSVファイル出力ToolStripMenuItem.Click += cSVファイル出力ToolStripMenuItem_Click;
			// 
			// cSVクリップボードToolStripMenuItem
			// 
			cSVクリップボードToolStripMenuItem.Name = "cSVクリップボードToolStripMenuItem";
			cSVクリップボードToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			cSVクリップボードToolStripMenuItem.Text = "CSVクリップボード(&C)";
			cSVクリップボードToolStripMenuItem.Click += cSVクリップボード出力ToolStripMenuItem_Click;
			// 
			// toolStripSeparator7
			// 
			toolStripSeparator7.Name = "toolStripSeparator7";
			toolStripSeparator7.Size = new System.Drawing.Size(165, 6);
			// 
			// トリミングToolStripMenuItem
			// 
			トリミングToolStripMenuItem.Name = "トリミングToolStripMenuItem";
			トリミングToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			トリミングToolStripMenuItem.Text = "トリミング(&T)";
			トリミングToolStripMenuItem.Click += トリミングToolStripMenuItem_Click;
			// 
			// 数値入力ToolStripMenuItem
			// 
			数値入力ToolStripMenuItem.Name = "数値入力ToolStripMenuItem";
			数値入力ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			数値入力ToolStripMenuItem.Text = "数値入力(&N)";
			数値入力ToolStripMenuItem.Click += 数値入力ToolStripMenuItem_Click;
			// 
			// 画像出力VToolStripMenuItem
			// 
			画像出力VToolStripMenuItem.Name = "画像出力VToolStripMenuItem";
			画像出力VToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			画像出力VToolStripMenuItem.Text = "画像出力(&V)";
			画像出力VToolStripMenuItem.Click += 画像出力VToolStripMenuItem_Click;
			// 
			// labelMouseCursor3
			// 
			labelMouseCursor3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			labelMouseCursor3.AutoSize = true;
			labelMouseCursor3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			labelMouseCursor3.Location = new System.Drawing.Point(126, 711);
			labelMouseCursor3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			labelMouseCursor3.Name = "labelMouseCursor3";
			labelMouseCursor3.Size = new System.Drawing.Size(53, 12);
			labelMouseCursor3.TabIndex = 10;
			labelMouseCursor3.Text = "--------";
			toolTip1.SetToolTip(labelMouseCursor3, "画像情報 YCrCb(0-255,-128-127,-128-127)");
			// 
			// labelAverageColor
			// 
			labelAverageColor.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			labelAverageColor.AutoSize = true;
			labelAverageColor.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			labelAverageColor.Location = new System.Drawing.Point(338, 716);
			labelAverageColor.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			labelAverageColor.Name = "labelAverageColor";
			labelAverageColor.Size = new System.Drawing.Size(53, 12);
			labelAverageColor.TabIndex = 2;
			labelAverageColor.Text = "--------";
			toolTip1.SetToolTip(labelAverageColor, "ラインまたはエリアの平均色をしめします");
			// 
			// lblSubPx
			// 
			lblSubPx.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			lblSubPx.Location = new System.Drawing.Point(840, 711);
			lblSubPx.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblSubPx.Name = "lblSubPx";
			lblSubPx.Size = new System.Drawing.Size(149, 21);
			lblSubPx.TabIndex = 4;
			lblSubPx.Text = "(--------,--------)";
			lblSubPx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			toolTip1.SetToolTip(lblSubPx, "マウスカーソル位置をサブピクセル表現します");
			// 
			// lblEditTitle
			// 
			lblEditTitle.AutoSize = true;
			lblEditTitle.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Bold);
			lblEditTitle.ForeColor = System.Drawing.Color.Red;
			lblEditTitle.Location = new System.Drawing.Point(286, 5);
			lblEditTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblEditTitle.Name = "lblEditTitle";
			lblEditTitle.Size = new System.Drawing.Size(87, 20);
			lblEditTitle.TabIndex = 11;
			lblEditTitle.Text = "タイトル変更";
			toolTip1.SetToolTip(lblEditTitle, "Ctrl + T タイトル変更");
			lblEditTitle.Visible = false;
			// 
			// gboxProfileMode
			// 
			gboxProfileMode.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			gboxProfileMode.Controls.Add(rbtnProfModeBox);
			gboxProfileMode.Controls.Add(rbtnProfModeLine);
			gboxProfileMode.Controls.Add(rbtnProfModeOff);
			gboxProfileMode.Location = new System.Drawing.Point(547, 656);
			gboxProfileMode.Margin = new System.Windows.Forms.Padding(4);
			gboxProfileMode.Name = "gboxProfileMode";
			gboxProfileMode.Padding = new System.Windows.Forms.Padding(4);
			gboxProfileMode.Size = new System.Drawing.Size(245, 56);
			gboxProfileMode.TabIndex = 5;
			gboxProfileMode.TabStop = false;
			gboxProfileMode.Text = "ProfileMode";
			// 
			// rbtnProfModeBox
			// 
			rbtnProfModeBox.AutoSize = true;
			rbtnProfModeBox.Location = new System.Drawing.Point(175, 24);
			rbtnProfModeBox.Margin = new System.Windows.Forms.Padding(4);
			rbtnProfModeBox.Name = "rbtnProfModeBox";
			rbtnProfModeBox.Size = new System.Drawing.Size(45, 19);
			rbtnProfModeBox.TabIndex = 0;
			rbtnProfModeBox.Text = "Box";
			rbtnProfModeBox.UseVisualStyleBackColor = true;
			rbtnProfModeBox.CheckedChanged += OnChangeProfileMode;
			// 
			// rbtnProfModeLine
			// 
			rbtnProfModeLine.AutoSize = true;
			rbtnProfModeLine.Location = new System.Drawing.Point(98, 22);
			rbtnProfModeLine.Margin = new System.Windows.Forms.Padding(4);
			rbtnProfModeLine.Name = "rbtnProfModeLine";
			rbtnProfModeLine.Size = new System.Drawing.Size(47, 19);
			rbtnProfModeLine.TabIndex = 0;
			rbtnProfModeLine.Text = "Line";
			rbtnProfModeLine.UseVisualStyleBackColor = true;
			rbtnProfModeLine.CheckedChanged += OnChangeProfileMode;
			// 
			// rbtnProfModeOff
			// 
			rbtnProfModeOff.AutoSize = true;
			rbtnProfModeOff.Checked = true;
			rbtnProfModeOff.Location = new System.Drawing.Point(27, 22);
			rbtnProfModeOff.Margin = new System.Windows.Forms.Padding(4);
			rbtnProfModeOff.Name = "rbtnProfModeOff";
			rbtnProfModeOff.Size = new System.Drawing.Size(41, 19);
			rbtnProfModeOff.TabIndex = 0;
			rbtnProfModeOff.TabStop = true;
			rbtnProfModeOff.Text = "Off";
			rbtnProfModeOff.UseVisualStyleBackColor = true;
			rbtnProfModeOff.CheckedChanged += OnChangeProfileMode;
			// 
			// cboxFix
			// 
			cboxFix.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			cboxFix.AutoSize = true;
			cboxFix.Location = new System.Drawing.Point(823, 663);
			cboxFix.Margin = new System.Windows.Forms.Padding(4);
			cboxFix.Name = "cboxFix";
			cboxFix.Size = new System.Drawing.Size(41, 19);
			cboxFix.TabIndex = 7;
			cboxFix.Text = "Fix";
			cboxFix.UseVisualStyleBackColor = true;
			cboxFix.CheckedChanged += cboxFix_CheckedChanged;
			// 
			// labelProfSize
			// 
			labelProfSize.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			labelProfSize.Location = new System.Drawing.Point(439, 654);
			labelProfSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			labelProfSize.Name = "labelProfSize";
			labelProfSize.Size = new System.Drawing.Size(82, 26);
			labelProfSize.TabIndex = 8;
			labelProfSize.Text = "----------";
			labelProfSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelMouseCursor2
			// 
			labelMouseCursor2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			labelMouseCursor2.AutoSize = true;
			labelMouseCursor2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			labelMouseCursor2.Location = new System.Drawing.Point(126, 689);
			labelMouseCursor2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			labelMouseCursor2.Name = "labelMouseCursor2";
			labelMouseCursor2.Size = new System.Drawing.Size(53, 12);
			labelMouseCursor2.TabIndex = 2;
			labelMouseCursor2.Text = "--------";
			// 
			// labelMouseCursorXY
			// 
			labelMouseCursorXY.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			labelMouseCursorXY.AutoSize = true;
			labelMouseCursorXY.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			labelMouseCursorXY.Location = new System.Drawing.Point(24, 668);
			labelMouseCursorXY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			labelMouseCursorXY.Name = "labelMouseCursorXY";
			labelMouseCursorXY.Size = new System.Drawing.Size(53, 12);
			labelMouseCursorXY.TabIndex = 2;
			labelMouseCursorXY.Text = "--------";
			// 
			// lblImageSize
			// 
			lblImageSize.AutoSize = true;
			lblImageSize.Location = new System.Drawing.Point(102, 12);
			lblImageSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblImageSize.Name = "lblImageSize";
			lblImageSize.Size = new System.Drawing.Size(32, 15);
			lblImageSize.TabIndex = 9;
			lblImageSize.Text = "-----";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(24, 12);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(59, 15);
			label2.TabIndex = 9;
			label2.Text = "画像サイズ";
			// 
			// tboxEditTitle
			// 
			tboxEditTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tboxEditTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			tboxEditTitle.Font = new System.Drawing.Font("メイリオ", 9F);
			tboxEditTitle.Location = new System.Drawing.Point(397, 1);
			tboxEditTitle.Margin = new System.Windows.Forms.Padding(4);
			tboxEditTitle.Name = "tboxEditTitle";
			tboxEditTitle.Size = new System.Drawing.Size(389, 25);
			tboxEditTitle.TabIndex = 12;
			tboxEditTitle.Visible = false;
			tboxEditTitle.KeyDown += tboxEditTitle_KeyDown;
			// 
			// btnChangeTitle
			// 
			btnChangeTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnChangeTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnChangeTitle.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Bold);
			btnChangeTitle.ForeColor = System.Drawing.Color.Red;
			btnChangeTitle.Location = new System.Drawing.Point(798, 1);
			btnChangeTitle.Margin = new System.Windows.Forms.Padding(4);
			btnChangeTitle.Name = "btnChangeTitle";
			btnChangeTitle.Size = new System.Drawing.Size(55, 29);
			btnChangeTitle.TabIndex = 13;
			btnChangeTitle.Text = "確定";
			btnChangeTitle.UseVisualStyleBackColor = true;
			btnChangeTitle.Visible = false;
			btnChangeTitle.Click += btnChangeTitle_Click;
			// 
			// FormColorImageS
			// 
			AllowDrop = true;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(1031, 739);
			Controls.Add(btnChangeTitle);
			Controls.Add(tboxEditTitle);
			Controls.Add(lblEditTitle);
			Controls.Add(labelMouseCursor3);
			Controls.Add(label2);
			Controls.Add(lblImageSize);
			Controls.Add(labelProfSize);
			Controls.Add(cboxFix);
			Controls.Add(tboxProfInfo);
			Controls.Add(gboxProfileMode);
			Controls.Add(lblSubPx);
			Controls.Add(labelDispInfo);
			Controls.Add(labelMouseCursor2);
			Controls.Add(labelMouseCursorXY);
			Controls.Add(labelAverageColor);
			Controls.Add(labelMouseCursor1);
			Controls.Add(labelDisplayRate);
			Controls.Add(pboxColImage);
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			Margin = new System.Windows.Forms.Padding(2);
			Name = "FormColorImageS";
			Text = "画像情報画面";
			FormClosed += FormColorImageS_FormClosed;
			Load += FormColorImageS_Load;
			DragDrop += FormMain_DragDrop;
			DragEnter += FormMain_DragEnter;
			Paint += FormColorImageS_Paint;
			KeyDown += FormColorImageS_KeyDown;
			KeyUp += FormColorImageS_KeyUp;
			((System.ComponentModel.ISupportInitialize)pboxColImage).EndInit();
			contextMenuStripL.ResumeLayout(false);
			contextMenuStripRate.ResumeLayout(false);
			contextMenuStripDispInfo.ResumeLayout(false);
			contextMenuStripProfData.ResumeLayout(false);
			gboxProfileMode.ResumeLayout(false);
			gboxProfileMode.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
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
		private System.Windows.Forms.Label lblEditTitle;
		private System.Windows.Forms.TextBox tboxEditTitle;
		private System.Windows.Forms.Button btnChangeTitle;
	}
}