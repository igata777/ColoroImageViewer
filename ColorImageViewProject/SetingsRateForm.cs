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
	/// <summary>倍率設定フォームクラスです</summary>
	public partial class SetingsRateForm : Form
	{
		/// <summary>倍率を取得します</summary>
		/// <remarks>フォームからの応答がOKの場合のみ有効です</remarks>
		public float Rate { get; private set; }

		/// <summary>フォーカス取得後、最初のキー入力をチェックするためのフラグです。</summary>
		private bool _initKeyFlag = true;


		/// <summary>コンストラクタです</summary>
		/// <param name="rate">現在のRate</param>
		public SetingsRateForm(float rate)
		{
			InitializeComponent();

			Rate = rate;
		}



		/// <summary>テキストボックスにフォーカスが入ったときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void textBoxRate_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).SelectAll();
			_initKeyFlag = true;
		}

		/// <summary>テキストボックスがフォーカスを失ったときの処理です</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBoxRate_Leave(object sender, EventArgs e)
		{
			_initKeyFlag = true;
			float w;
			if (float.TryParse(textBoxRate.Text, out w) == false || w <= 0.0 || w>= 10000.0 )
			{
				MessageBox.Show("入力倍率が不適当です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			Rate = w/ 100.0f;
			SetTextBoxRate();
		}


		/// <summary>「キャンセルボタン」がクリックされたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}


		/// <summary>「決定ボタン」がクリックされたときの処理です</summary>
		/// <param name="sender">送信元情報(n.u)</param>
		/// <param name="e">イベント情報(n.u)</param>
		/// <remarks></remarks>
		private void btnOK_Click(object sender, EventArgs e)
		{
			float w;
			if (float.TryParse(textBoxRate.Text, out w) == true)
			{
				Rate = (float)(w / 100);
				DialogResult = System.Windows.Forms.DialogResult.OK;
				this.Close();
			}
			else
			{
				MessageBox.Show("不適当な倍率です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			SetTextBoxRate();
		}


		/// <summary>テキストボックスに倍率を書き込みます</summary>
		private void SetTextBoxRate()
		{
			textBoxRate.Text = (Rate * 100.0).ToString("##0.0");
		}

		/// <summary>ロード時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void SetingsRateForm_Load(object sender, EventArgs e)
		{
			SetTextBoxRate();
			_initKeyFlag = true;
			textBoxRate.SelectAll();
		}


		/// <summary>キーが押された直後のイベント処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void textBoxRate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (_initKeyFlag == true)
			{
				textBoxRate.SelectAll();
			}
			_initKeyFlag = false;
		}

		/// <summary>キー入力時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void textBoxRate_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				btnOK_Click(null,null);
			}
		}


	}
}
