using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ColorImageViewProject
{
	/// <summary>
	/// サイズ指定のためのフォームクラスです
	/// </summary>
	public partial class SettingSizeForm : Form
	{

		#region プロパティ　－－－－
		/// <summary>
		/// 設定した幅の値を取得します
		/// </summary>
		public int Width
		{
			get { return (int)nudWidth.Value; }
		}

		/// <summary>
		/// 設定した高さの値を取得します
		/// </summary>
		public int Height
		{
			get { return (int)nudHeight.Value; }
		}

		#endregion //  プロパティ　－－－－



		/// <summary>
		/// コンストラクタです
		/// </summary>
		/// <param name="initWidth">初期表示幅</param>
		/// <param name="initHeight">初期表示高さ</param>
		/// <param name="maxWidth">最大幅</param>
		/// <param name="maxHeigh">最大高さ</param>
		public SettingSizeForm(int initWidth, int initHeight, int maxWidth, int maxHeigh)
		{
			InitializeComponent();

			// 初期値をセットします
			if (maxWidth < initWidth) maxWidth = initWidth;
			if (maxHeigh < initHeight) maxHeigh = initHeight;
			nudWidth.Maximum = maxWidth;
			nudHeight.Maximum = maxHeigh;
			nudWidth.Value = initWidth;
			nudHeight.Value = initHeight;
		}

		/// <summary>
		/// 「キャンセル」ボタンが押されたときの処理です
		/// </summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}


		/// <summary>
		/// 「決定」ボタンが押されたときの処理です
		/// </summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}



	}
}
