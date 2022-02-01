using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ColorImageViewProject
{
	/// <summary>設定値を表示するためのフォームクラスです。</summary>
	/// <remarks>IPPLにもあるが、これは汎用性を高くしたもの</remarks>
	public partial class FormConfigurationData : Form
	{
		/// <summary>
		/// 設定値ワーク領域を保持します
		/// </summary>
		private object _workset;

		/// <summary>
		/// 元の設定値のコピーを保持します。
		/// </summary>
		private object _orgData;

		/// <summary>
		/// 設定データを取得します
		/// </summary>
		public object Settings
		{
			get { return CloneObject(_workset); }
		}

		/// <summary>
		/// コンストラクタです
		/// </summary>
		/// <param name="workset">設定データ</param>
		/// <param name="icon">フォームアイコン</param>
		public FormConfigurationData( object workset, Icon icon= null )
		{
			InitializeComponent();

			_workset = workset;
			_orgData = CloneObject(workset); // ディープコピー
			if (icon != null) this.Icon = icon;
		}


		/// <summary>
		/// フォームがロードされた時の処理です
		/// </summary>
		/// <param title="sender">送信元</param>
		/// <param title="e">イベント情報</param>
		private void FormSetting_Load(object sender, EventArgs e)
		{
			this.propertyGrid1.SelectedObject = _workset;
			this.btn_Set.Enabled = false;
		}


		/// <summary>
		/// 「設定」がクリックされた時の処理です
		/// </summary>
		/// <param title="sender">送信元</param>
		/// <param title="e">イベント情報</param>
		private void btn_Set_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}


		/// <summary>
		/// 「キャンセル」がクリックされた時の処理です
		/// </summary>
		/// <param title="sender">送信元</param>
		/// <param title="e">イベント情報</param>
		private void btn_Cancel_Click(object sender, EventArgs e)
		{
			_workset = null;
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}


		/// <summary>
		/// プロパティグリッドの値が変更された時の処理です
		/// </summary>
		/// <param title="sender">送信元</param>
		/// <param title="e">イベント情報</param>
		private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			// 「設定」ボタンは変更があった時のみ有効とする
			var cobj = _workset as IComparable;
			if (cobj != null)
			{
				this.btn_Set.Enabled = (cobj != null) ? cobj.CompareTo(_orgData) != 0 : true;
				Refresh();
			}
		}


		/// <summary>
		/// オブジェクトのディープコピーを返します。
		/// </summary>
		/// <returns>オブジェクト</returns>
		public object CloneObject(object serialObj)
		{
			object clone = null;

			using (var stream = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, serialObj);
				stream.Position = 0;
				clone = formatter.Deserialize(stream);
			}

			return clone;
		}

	}

}
