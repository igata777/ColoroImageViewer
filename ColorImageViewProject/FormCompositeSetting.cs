using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IAPLibrary;

namespace IAPLibrary
{
	/// <summary>
	/// 合成・解析ローカル設定画面フォームです
	/// </summary>
	public partial class FormCompositeSetting : Form
	{
		/// <summary>設定値を保持します</summary>
		private CompositeLocalSettings _settings= new CompositeLocalSettings();

		/// <summary>
		/// コンストラクタです
		/// </summary>
		public FormCompositeSetting()
		{
			InitializeComponent();
			this.propertyGrid1.SelectedObject = _settings;
		}

		/// <summary>ロード時の処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void FromMain_Load(object sender, EventArgs e)
		{
			_settings.LoadSettingData();

		}

		/// <summary>確定ボタンをクリックしたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void btnOK_Click(object sender, EventArgs e)
		{
			_settings.SaveSettingData();
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		/// <summary>キャンセルボタンがクリックされたときの処理です</summary>
		/// <param name="sender">送信元情報</param>
		/// <param name="e">イベント情報</param>
		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}


	}
}
