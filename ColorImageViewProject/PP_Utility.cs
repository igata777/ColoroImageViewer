using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

namespace ColorImageViewProject
{
	/// <summary>
	/// ユーティリティクラスです
	/// </summary>
	[Serializable]
	public class PP_Utility
	{
		#region BeginUpdate,EndUpdate ---
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public const int WM_SETREDRAW = 0x000B;
        public const int Win32False = 0;
        public const int Win32True = 1;

        /// <summary>
        /// EndUpdate メソッドが呼ばれるまで、コントロールを再描画しないようにします。
        /// </summary>
        public static void BeginUpdate(Control form) 
		{
            SendMessage(form.Handle, WM_SETREDRAW, Win32False, 0);
        }

        /// <summary>
        /// BeginUpdate メソッドにより中断されていた描画を再開します。
        /// </summary>
        public static void EndUpdate(Control form)
		{
            SendMessage(form.Handle, WM_SETREDRAW, Win32True, 0);
            form.Invalidate();
        }
		#endregion // BeginUpdate,EndUpdate ---


		// ----
		/// <summary>オブジェクトをシリアル化して保存する
		/// </summary>
		/// <param name="fileName">保存ファイルのフルパス</param>
		/// <param name="data">シリアライズデータ</param>
		/// <param name="dataType">データタイプ</param>
		public static void SerializeData(string fileName, object data, Type dataType)
		{

			//XmlSerializerオブジェクトを作成
			//書き込むオブジェクトの型を指定する
			XmlSerializer serializer =
				new XmlSerializer(dataType);
			//ファイルを開く
			FileStream fs = new FileStream(fileName, FileMode.Create);
			//シリアル化し、XMLファイルに保存する
			serializer.Serialize(fs, data);
			//閉じる
			fs.Close();
		}

		/// <summary>
		/// オブジェクト読み込みとデシリアライズを行います
		/// </summary>
		/// <param name="fileName">ファイル名</param>
		/// <param name="dataType">データタイプ</param>
		/// <returns>生成したインスタンス</returns>
		public static object DeserializeData(string fileName, Type dataType)
		{
			//XmlSerializerオブジェクトの作成
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(dataType);

			//ファイルを開く
			FileStream fs = new FileStream(fileName, FileMode.Open);

			//XMLファイルから読み込み、逆シリアル化する
			object obj = serializer.Deserialize(fs);

			//閉じる
			fs.Close();

			return obj;
		}


		/// <summary>
		/// データグリッドの内容をCSVで保存します。（ファイル選択などの共通処理部分のみ。書き込みは指定のFuncで行います）
		/// </summary>
		/// <param name="funcSaveCsv">保存関数 bool Func( DataGridView datagrid, string filePath )</param>
		/// <param name="tailTexts">表の後ろに追加するテキスト (不要の場合はnull）</param>
		/// <param name="headTexts">表の前に追加するテキスト（不要の場合はnull）</param>
		static public void SaveGridToCsv(DataGridView datagridview, Func<DataGridView, string, string[], bool> funcSaveCsv, string[] tailTexts = null, string[] headTexts = null)
		{
			string fname;
			var sfd = new SaveFileDialog();
			sfd.Filter = "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
			//[ファイルの種類]ではじめに
			//「すべてのファイル」が選択されているようにする
			sfd.FilterIndex = 1;
			//タイトルを設定する
			sfd.Title = "保存先のファイルを選択してください";
			if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				fname = sfd.FileName;
				if (funcSaveCsv(datagridview, sfd.FileName, tailTexts) == false)
				{
					MessageBox.Show(String.Format("ファイル（{0}）書き込みが失敗しました", Path.GetFileName(fname)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}


		/// <summary>
		/// DataGridViewの内容をCSVで保存します。
		/// </summary>
		/// <param name="datagridview">データグリッドビュー</param>
		/// <param name="tailTexts">表の後ろに追加するテキスト (不要の場合はnull）</param>
		/// <param name="headTexts">表の前に追加するテキスト（不要の場合はnull）</param>
		static public void SaveGridToCsv(DataGridView datagridview, string[] tailTexts = null, string[] headTexts = null)
		{
			string fname;
			var sfd = new SaveFileDialog();
			sfd.Filter = "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
			//[ファイルの種類]ではじめに
			//「すべてのファイル」が選択されているようにする
			sfd.FilterIndex = 1;
			//タイトルを設定する
			sfd.Title = "保存先のファイルを選択してください";
			if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				fname = sfd.FileName;
				if (ExportCsvFileData(datagridview, sfd.FileName, tailTexts, headTexts) == false)
				{
					MessageBox.Show(String.Format("ファイル（{0}）書き込みが失敗しました", Path.GetFileName(fname)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}


		/// <summary> CSVファイルにExportします </summary>
		/// <param title="filepath">ファイル名</param>
		/// <param name="datagridview">データグリッドビュー</param>
		/// <param name="tailTexts">表の後ろに追加するテキスト (不要の場合はnull）</param>
		/// <param name="headTexts">表の前に追加するテキスト（不要の場合はnull）</param>
		/// <returns>成否を返します</returns>
		static private bool ExportCsvFileData(DataGridView datagridview, string filename, string[] tailTexts = null, string[] headTexts = null)
		{

			bool ret = false;
			try
			{
				using (StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding("Shift-JIS")))
				{
					// 表の前に追加する文字列があれば追加する。
					if (headTexts != null)
					{
						foreach (var tl in headTexts)
						{
							sw.WriteLine(tl.Replace("\r\n", "__").Replace(",", "__").Replace("\r", "__").Replace("\n", "__"));
						}
					}
					//　ヘッダーもDataGridViewから取得する
					sw.WriteLine(String.Join(",", datagridview.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderCell.Value.ToString())));
					foreach (var r in datagridview.Rows.Cast<DataGridViewRow>())
					{
						var line = String.Join(",", r.Cells.Cast<DataGridViewCell>().Select(n => n.Value.ToString().Replace("\r\n", "__").Replace(",", "__").Replace("\r", "__").Replace("\n", "__")));
						sw.WriteLine(line);
					}
					// 後書きがあれば追加する。
					if (tailTexts != null)
					{
						foreach (var tl in tailTexts)
						{
							sw.WriteLine(tl.Replace("\r\n", "__").Replace(",", "__").Replace("\r", "__").Replace("\n", "__"));
						}
					}

					sw.Close();
					ret = true;
				}
			}
			catch
			{
				MessageBox.Show("CSVファイルの書き込みに失敗しました", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				ret = false;
			}
			return ret;
		}




		/// <summary>
		/// クリップボードに(CSV形式で）コピーします
		/// </summary>
		/// <param name="datagirdview">データグリッドビュー</param>
		/// <param name="separator">セパレータ</param>
		/// <param name="tailTexts">表の後ろに追加するテキスト (不要の場合はnull）</param>
		/// <param name="headTexts">表の前に追加するテキスト（不要の場合はnull）</param>
		static public void DataGridDataToClipBoard(DataGridView datagirdview, string separator = "\t", string[] tailTexts = null, string[] headTexts = null)
		{
			using (var sw = new StringWriter())
			{
				// 表の前に塚する文字列があれば追加する。
				if (headTexts != null)
				{
					foreach (var tl in headTexts)
					{
						sw.WriteLine(tl.Replace("\r\n", "__").Replace(separator, "__").Replace("\r", "__").Replace("\n", "__"));
					}
				}
				//　ヘッダーもDataGridViewから取得する
				sw.WriteLine(String.Join(separator, datagirdview.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderCell.Value.ToString().Replace("\r\n", "__").Replace(separator, "__").Replace("\r", "__").Replace("\n", "__"))));
				foreach (var r in datagirdview.Rows.Cast<DataGridViewRow>())
				{
					var line = String.Join(separator, r.Cells.Cast<DataGridViewCell>().Select(n => n.Value.ToString().Replace("\r\n", "__").Replace(separator, "__").Replace("\r", "__").Replace("\n", "__")));
					sw.WriteLine(line);
				}
				// 後書きがあれば追加する。
				if (tailTexts != null)
				{
					foreach (var tl in tailTexts)
					{
						sw.WriteLine(tl.Replace("\r\n", "__").Replace(separator, "__").Replace("\r", "__").Replace("\n", "__"));
					}
				}

				Clipboard.SetDataObject(sw.ToString());
				sw.Close();
			}

		}


		/* 推奨キーワード */

		/// <summary>推奨されるプラグインルートイメージ用キーワードです</summary>
		/// <remarks>RootImageは「元ファイル」を変更したい場合に使用されるキーワードです。DataPackage.AnaData.ReferenceImage[]</remarks>
		public const string IPP_PluginRootDataKey = "_RootImage_";

		/// <summary>推奨されるプラグインデフォルト入出力用キーワードです</summary>
		/// <remarks>DataPackage.AnaData.ReferenceImages["TemporaryImage"]</remarks>
		public const string IPP_PluginDefaultDataKey = "TemporaryImage";


		/// <summary>推奨されるExchangeObjectsにおけるIPPL設定値キーワードです</summary>
		/// <remarks>ExchangeObjects[ExcKey_IPPLSettings]</remarks>
		public const string ExcKey_IPPLSettings = "_IpplSettings_";

		/// <summary>推奨されるExchangeObjectsにおけるプラグイン設定値文字列のキーワードです</summary>
		/// <remarks>ExchangeObjects[ExcKey_IPPSettingString]</remarks>
		public const string ExcKey_IPPSettingString = "_IppSettingString_";

		/// <summary>推奨されるExchangeObjectsにおける「初期設定モード」指定(bool値)のキーワードです</summary>
		/// <remarks>ExchangeObjects[ExcKey_IPPSettingString], プラグイン呼び出し前にメイン画面のチェックボックスの値をセットする</remarks>
		public const string ExcKey_IppIsInitSetting = "_Is_InitSetting_";


		static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);



		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);


		/// <summary>
		/// フォーカスを奪わずに最前面に表示します</summary>
		static public void SetTopMostWithoutFocus(Form form)
		{
			IntPtr HWND_TOPMOST = new IntPtr(-1);
			const uint SWP_NOSIZE = 0x0001;
			const uint SWP_NOMOVE = 0x0002;
			const uint SWP_NOACTIVATE = 0x0010;
			const uint SWP_SHOWWINDOW = 0x0040;
			const uint SWP_NOSENDCHANGING = 0x0400;
			SetWindowPos(form.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSENDCHANGING | SWP_NOSIZE | SWP_SHOWWINDOW);
		}


		#region 平均・標準偏差の算出 ---------------------------------------------------------------

		/// <summary>
		/// 標準偏差および平均値の算出</summary>
		/// <param name="datas">対象データ</param>
		/// <param name="sdval">標準偏差</param>
		/// <param name="mean">平均値</param>
		/// <returns>成否</returns>
		/// <remarks>失敗の場合には両パラメータとも 0.0としておく</remarks>
		public static bool CalcMeanSd(int[] datas, out double sdval, out double mean)
		{
			var ttl = 0.0;
			var ttl2 = 0.0;
			foreach (var va in datas)
			{
				ttl += va;
				ttl2 += va * va;
			}
			sdval = 0.0;
			mean = 0.0;
			if (datas.Length > 1)
			{
				sdval = Math.Sqrt((ttl2 - (ttl * ttl / datas.Length)) / (datas.Length - 1));      // 標準偏差
				mean = ttl / datas.Length;
				return true;
			}
			return false;
		}


		/// <summary>
		/// 標準偏差および平均値の算出</summary>
		/// <param name="datas">対象データ</param>
		/// <param name="sdval">標準偏差</param>
		/// <param name="mean">平均値</param>
		/// <returns>成否</returns>
		/// <remarks>失敗の場合には両パラメータとも 0.0としておく</remarks>
		public static bool CalcMeanSd(float[] datas, out double sdval, out double mean)
		{
			var ttl = 0.0;
			var ttl2 = 0.0;
			foreach (var va in datas)
			{
				ttl += va;
				ttl2 += va * va;
			}
			sdval = 0.0;
			mean = 0.0;
			if (datas.Length > 1)
			{
				sdval = Math.Sqrt((ttl2 - (ttl * ttl / datas.Length)) / (datas.Length - 1));      // 標準偏差
				mean = ttl / datas.Length;
				return true;
			}
			return false;
		}


		/// <summary>
		/// 標準偏差および平均値の算出</summary>
		/// <param name="datas">対象データ</param>
		/// <param name="sdval">標準偏差</param>
		/// <param name="mean">平均値</param>
		/// <returns>成否</returns>
		/// <remarks>失敗の場合には両パラメータとも 0.0としておく</remarks>
		public static bool CalcMeanSd(double[] datas, out double sdval, out double mean)
		{
			var ttl = 0.0;
			var ttl2 = 0.0;
			foreach (var va in datas)
			{
				ttl += va;
				ttl2 += va * va;
			}
			sdval = 0.0;
			mean = 0.0;
			if (datas.Length > 1)
			{
				sdval = Math.Sqrt((ttl2 - (ttl * ttl / datas.Length)) / (datas.Length - 1));      // 標準偏差
				mean = ttl / datas.Length;
				return true;
			}
			return false;
		}

		#endregion // 平均・標準偏差の算出 ---------------------------------------------------------------



		/// <summary>
		/// File.ReadAllLines()の代替メソッドです。　読み込むファイルが他でオープンされていても例外を出さないようにします。 </summary>
		/// <param name="path"> ファイルパス </param>
		/// <param name="enc"> エンコード </param>
		/// <returns> 全ての行。(改行コードは除かれる）　エラーの場合nullを返します </returns>
		static public string[] ReadAllLines(string path, Encoding enc)
		{
			var csvDt = new List<string>();
			try
			{
				using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				using (var tr = new StreamReader(fs, enc))
				{
					string str;
					while ((str = tr.ReadLine()) != null) csvDt.Add(str);
				}
			}
			catch
			{
				return null;                             // Error
			}
			return csvDt.ToArray();
		}


		/// <summary>
		/// フォルダを開きます</summary>
		/// <param name="fpath">フォルダパス</param>
		static public void OpenFolder(string fpath)
		{
			if (Directory.Exists(fpath) == false)
			{
				MessageBox.Show("指定フォルダが存在しません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			System.Diagnostics.Process.Start(fpath);
		}


		/// <summary>
		/// TOPMOSTでメッセージボックスを表示します</summary>
		/// <param name="msg">メッセージ</param>
		/// <param name="cap">キャプション</param>
		/// <param name="btns">ボタン</param>
		/// <param name="icon">アイコン</param>
		static public DialogResult TmMessageBox(string msg, string cap, MessageBoxButtons btns = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Exclamation, MessageBoxDefaultButton defb = MessageBoxDefaultButton.Button1)
		{
			return MessageBox.Show(msg, cap, btns, icon, defb, MessageBoxOptions.DefaultDesktopOnly);
		}
	}


}
