using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorImageViewProject
{
	/// <summary>
	/// プラグインで保持する設定値の基底クラスを定義します
	/// </summary>
	public abstract class PP_PluginConfigData:IPP_PluginConfigData
	{
		/// <summary>区切り文字キーワード(項目）の定義です</summary>
		public const string Keyword_Item = "_^^_";
		/// <summary>区切り文字キーワード(値）の定義です</summary>
		public const string Keyword_Value = "_==_";
		/// <summary>区切り文字キーワード(配列）の定義です　【推奨】</summary>
		/// <remarks>配列についてはこれ以外にプラグインで自由に決定しても良いが、その場合には項目区切り文字とは一致しないことが必要</remarks>
		public const string Keyword_Array = "_++_";
		/// <summary> 区切り文字キーワード（プラグイン間）【推奨】</summary>
		/// <remarks>内部では使用されることはないが、外側のプログラムでプラグイン間をつなぐ区切り文字として使用することを推奨する</remarks>
		public const string Keyword_Plugin = "__|__";
		/// <summary> 無効フラグマークキーワード</summary>
		public const string Keyword_MarkOff = "__Off__";


		//  (無効フラグマーク）プラグイン名１_^^_項目１_==_項目1の値_^^_項目２_==_項目2の値__|__プラグイン２・・・・


		/// <summary>
		/// コンストラクタです
		/// </summary>
		public PP_PluginConfigData()
		{
		}


		/// <summary>優先する入力先のキーワードを保持します</summary>
		abstract public string InputDataKey { get; set; }

		/// <summary>優先する出力先のキーワードを保持します</summary>
		abstract public string OutputDataKey { get; set; }

		/// <summary>設定値文字列を生成します</summary>
		/// <param name="pluginName">名称</param>
		/// <returns>生成した設定値文字列</returns>
		abstract public string ToConfigString(string pluginName);

		/// <summary>設定値文字列を反映します</summary>
		/// <param name="configString">設定値文字列</param>
		/// <param name="powFlag">強制フラグ trueの場合には完全に一致しなくても一致する部分のみ設定する</param>
		/// <returns>成否</returns>
		abstract public bool FromConfigString(string configString, bool powFlag);


		/// <summary>
		/// 設定文字列から辞書および名称を取得します。
		/// </summary>
		/// <param name="pluginName">プラグイン名称</param>
		/// <param name="configString">設定文字列</param>
		/// <returns>項目と項目値の辞書<string,string></returns>
		/// <remarks>設定文字列フォーマットはプラグイン名称を先頭に区切り文字キーワードで分割された1行の文字列とする</remarks>
		public static Dictionary<string, string> ConfigStringToDictionary(string configString, out string pluginName)
		{
			pluginName = null;
			var wstrs = configString.Split(new String[] { Keyword_Item }, StringSplitOptions.None );

			// 先頭の名称を取り出した後、残りのペアで辞書を作る。項目値に 区切り文字（Array)はこの時点では展開しない
			pluginName = wstrs[0];
			wstrs = wstrs.Skip(1).ToArray();
			var dict = new Dictionary<string, string>();
			try
			{
				foreach (var pairstr in wstrs)
				{
					var pss = pairstr.Split(new String[] { Keyword_Value }, StringSplitOptions.None);
					// 項目名と項目値に分かれるはず。　そうでないものはエラー
					if (pss.Length == 2)
					{
						dict[pss[0]] = pss[1];
					}
					else
					{

						throw new Exception("設定項目がペアになっていない " + pairstr);
					}
				}
			}
			catch
			{
				dict = null;
			}
			return dict;
		}

		
		/// <summary>
		/// 設定文字列からプラグイン名称を取得します。
		/// </summary>
		/// <param name="configString">設定文字列</param>
		/// <returns>プラグイン名称 無効(エラー）の場合にはnull</returns>
		/// <remarks>設定文字列フォーマットはプラグイン名称を先頭に区切り文字キーワードで分割された1行の文字列とする</remarks>
		public static string GetPluginNameFromConfigString(string configString)
		{
			var wstrs = configString.Split(new String[] { Keyword_Item }, StringSplitOptions.None);
			// 先頭にある名称を返す。だだし MarkOff（無効フラグ）がついている場合があるので、その場合はその部分を除外して返す。
			return wstrs[0].StartsWith(Keyword_MarkOff) ? wstrs[0].Remove(0, Keyword_MarkOff.Length) : wstrs[0];
		}


	}
}
