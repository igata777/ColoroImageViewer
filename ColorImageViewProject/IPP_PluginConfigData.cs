using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// プラグインを作成する場合、特に指定が無い場合には DataPackage.AnaData.ReferenceImages["TemporaryImage"]を
// 入出力のキーワードとすることが推奨される。
// ただし、以下のインターフェースを実装したものについては優先するキーワードの変更ができるよう、各プラグインで対応する。
// 入力については ReferenceImagesに適当な入力が無い場合には、オリジナルデータ(DataPackage.InputData.ImageData)

// DisplyaResultImageは実行後の表示ボタンなどを強制表示する・しないなどの設定に利用できるenumを定義。

namespace ColorImageViewProject
{
	/// <summary>プラグインに入出力データの指示機能を追加するインターフェースです</summary>
	/// <remarks>プラグイン固有の設定値にこのインターフェースを追加すると、入出力についての指定が可能となる</remarks>
	public interface IPP_PluginConfigData
	{
		/// <summary>優先する入力先のキーワードを保持します</summary>
		string InputDataKey { get; set; }

		/// <summary>優先する出力先のキーワードを保持します</summary>
		string OutputDataKey { get; set; }

		/// <summary>設定値文字列を生成します</summary>
		/// <param name="pluginName">名称</param>
		/// <returns>生成した設定値文字列</returns>
		string ToConfigString( string pluginName );

		/// <summary>設定値文字列を反映します</summary>
		/// <param name="configString">設定値文字列</param>
		/// <param name="powFlag">強制フラグ trueの場合には完全に一致しなくても一致する部分のみ設定する</param>
		/// <returns>成否</returns>
		bool FromConfigString(string configString, bool powFlag);

	
	}


	// -------------------------------------------------------------------------------------------------
	/// <summary>実行後イメージ表示設定用</summary>
	public enum DisplayResultImageConfig : int
	{
		/// <summary>表示しません</summary>
		Off = 0,
		/// <summary>表示しません</summary>
		On = 1,
		/// <summary>表示しません</summary>
		DebugModeOnly = 2
	}

}
