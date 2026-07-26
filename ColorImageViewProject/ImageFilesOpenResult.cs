using System;

namespace ColorImageViewProject
{
	/// <summary>
	/// 画像ファイル群のオープン結果を保持します</summary>
	public class ImageFilesOpenResult
	{
		/// <summary>開いた画像数を保持します</summary>
		public int OpenCount { get; set; }

		/// <summary>読み込めなかったファイル情報を保持します</summary>
		public (string FileName, string Reason)[] Failures { get; set; } = Array.Empty<(string FileName, string Reason)>();

		/// <summary>キャンセルされたかどうかを保持します</summary>
		public bool IsCanceled { get; set; }
	}
}
