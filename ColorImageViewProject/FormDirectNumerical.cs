using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ColorImageViewProject
{
	public partial class FormDirectNumerical : Form
	{
		/// <summary>
		/// 画像サイズを保持します</summary>
		private Size _imgSize;

		/// <summary>
		/// コンストラクタです</summary>
		/// <param name="imgSize">画像サイズ</param>
		/// <param name="bgn">開始点初期値</param>
		/// <param name="end">終了点初期値</param>
		public FormDirectNumerical( Size imgSize, Point? bgn, Point? end )
		{
			InitializeComponent();

			TopMost = true;
			_imgSize = imgSize;
			nudX0.Minimum = nudY0.Minimum = nudX1.Minimum = nudY1.Minimum = 0;
			nudX0.Maximum = nudX1.Maximum = imgSize.Width-1;
			nudY0.Maximum = nudY1.Maximum = imgSize.Height-1;
			nudX0.Value = bgn == null ? 0 : bgn.Value.X;
			nudY0.Value = bgn == null ? 0 : bgn.Value.Y;
			nudX1.Value = end == null ? 0 : end.Value.X;
			nudY1.Value = end == null ? 0 : end.Value.Y;
		}


		/// <summary>
		/// 「キャンセル」がクリックされたときの処理です。</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}


		/// <summary>
		/// 「確定」がクリックされたときの処理です。</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void btnSet_Click(object sender, EventArgs e)
		{
			DialogResult = (nudX0.Value != nudX1.Value || nudY0.Value != nudY1.Value)? DialogResult.OK : DialogResult.Cancel;
			Close();
		}


		/// <summary>
		/// 領域を取得します</summary>
		/// <returns>設定された領域</returns>
		public (Point Bgn, Point End) GetArea()
		{
			int minX, minY, maxX, maxY;
			if (nudX0.Value < nudX1.Value)
			{
				minX = (int)nudX0.Value;
				maxX = (int)nudX1.Value;
			}
			else
			{
				maxX = (int)nudX0.Value;
				minX = (int)nudX1.Value;
			}
			if (nudY0.Value < nudY1.Value)
			{
				minY = (int)nudY0.Value;
				maxY = (int)nudY1.Value;
			}
			else
			{
				maxY = (int)nudY0.Value;
				minY = (int)nudY1.Value;
			}
			return (new Point(minX, minY), new Point(maxX, maxY));
		}
	}
}
