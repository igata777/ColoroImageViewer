using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;

using ColorImageViewProject;


namespace ColorImageViewProject
{
	/// <summary>
	/// 描画ユーティリティクラスです
	/// </summary>
	[Serializable]
	public static class UtilityGraphics
	{
		/// <summary>
		/// ベース色以外の色を重ね描きする
		/// </summary>
		/// <param name="dBmp">書き込み対象ビットマップ</param>
		/// <param name="sBmp">ソースビットマップ</param>
		/// <param name="col">ベース色</param>
		/// <remarks>両画像ともにPixelFormatはFormat24bppRgbとする。（それ以外は無視）</remarks>
		static public unsafe void DrawBitmapColorBase(Bitmap dBmp, Bitmap sBmp, Color col)
		{
			// 条件のチェック
			if( dBmp.Size != sBmp.Size ) return;
			if( dBmp.PixelFormat != PixelFormat.Format24bppRgb || dBmp.PixelFormat!= sBmp.PixelFormat )
			{
				// ピクセルフォーマットがともにFormat24bppRgbでないため無視
				return;
			}

			// 変数の準備 　前提：dBmpとsBmpのサイズが等しいこと
			int colR = col.R;
			int colG = col.G;
			int colB = col.B;
			int width = dBmp.Width;
			int height = dBmp.Height;
			int pixSize = Bitmap.GetPixelFormatSize(dBmp.PixelFormat)/8;		// バイトサイズ.

			// ビットマップアクセス
			BitmapData _imgD = dBmp.LockBits( new Rectangle( 0, 0, dBmp.Width, dBmp.Height ), ImageLockMode.ReadWrite, dBmp.PixelFormat );
			BitmapData _imgS = sBmp.LockBits( new Rectangle( 0, 0, sBmp.Width, sBmp.Height ), ImageLockMode.ReadOnly, sBmp.PixelFormat );

			for( int y= 0; y< height; y++ )
			{
				//アドレス計算 _imgDで代表
				byte* adrD = (byte*)_imgD.Scan0 + _imgD.Stride * y;
				byte* adrS = (byte*)_imgS.Scan0 + _imgS.Stride * y;
				for( int x = 0; x< width; x++, adrD+=pixSize , adrS+=pixSize )
				{
					if( adrS[0]!= colB || adrS[1]!= colG || adrS[2]!= colR )
					{
						adrD[0] = adrS[0];
						adrD[1] = adrS[1];
						adrD[2] = adrS[2];
					}
				}
			}
			dBmp.UnlockBits( _imgD );
			sBmp.UnlockBits( _imgS );
		}


		/// <summary>
		/// ベース色以外の色をブレンド描きする
		/// </summary>
		/// <param name="dBmp">書き込み対象ビットマップ</param>
		/// <param name="sBmp">ソースビットマップ</param>
		/// <param name="col">ベース色</param>
		/// <param name="rate">重ねがき比率( 0 ～ 1)</param>
		/// <remarks>両画像ともにPixelFormatはFormat24bppRgbとする。（それ以外は無視）</remarks>
		static public unsafe void DrawBitmapColorBaseP(Bitmap dBmp, Bitmap sBmp, Color col, double rate)
		{
			// 条件のチェック
			if (dBmp.Size != sBmp.Size) return;
			if (dBmp.PixelFormat != PixelFormat.Format24bppRgb || dBmp.PixelFormat != sBmp.PixelFormat)
			{
				// ピクセルフォーマットがともにFormat24bppRgbでないため無視
				return;
			}

			// 変数の準備 　前提：dBmpとsBmpのサイズが等しいこと
			int colR = col.R;
			int colG = col.G;
			int colB = col.B;
			int width = dBmp.Width;
			int height = dBmp.Height;
			int pixSize = Bitmap.GetPixelFormatSize(dBmp.PixelFormat) / 8;		// バイトサイズ.

			// ビットマップアクセス
			BitmapData _imgD = dBmp.LockBits(new Rectangle(0, 0, dBmp.Width, dBmp.Height), ImageLockMode.ReadWrite, dBmp.PixelFormat);
			BitmapData _imgS = sBmp.LockBits(new Rectangle(0, 0, sBmp.Width, sBmp.Height), ImageLockMode.ReadOnly, sBmp.PixelFormat);

			for (int y = 0; y < height; y++)
			{
				//アドレス計算 _imgDで代表
				byte* adrD = (byte*)_imgD.Scan0 + _imgD.Stride * y;
				byte* adrS = (byte*)_imgS.Scan0 + _imgS.Stride * y;
				for (int x = 0; x < width; x++, adrD += pixSize, adrS += pixSize)
				{
					if (adrS[0] != colB || adrS[1] != colG || adrS[2] != colR)
					{
						adrD[0] = (byte)(adrS[0] * rate + adrD[0] * (1.0 - rate));
						adrD[1] = (byte)(adrS[1] * rate + adrD[1] * (1.0 - rate));
						adrD[2] = (byte)(adrS[2] * rate + adrD[2] * (1.0 - rate));
					}
				}
			}
			dBmp.UnlockBits(_imgD);
			sBmp.UnlockBits(_imgS);
		}




		/// <summary>
		/// XOR重ね描きする
		/// </summary>
		/// <param name="dBmp">書き込み対象ビットマップ</param>
		/// <param name="sBmp">ソースビットマップ</param>
		/// <remarks>アンセーフ</remarks>
		static public unsafe void DrawBitmapColorXOR( Bitmap dBmp, Bitmap sBmp )
		{
			// 条件のチェック
			if( dBmp.Size != sBmp.Size ) return;
			if( dBmp.PixelFormat != PixelFormat.Format24bppRgb || dBmp.PixelFormat!= sBmp.PixelFormat )
			{
				// ピクセルフォーマットがともにFormat24bppRgbでないため無視
				return;
			}

			// 変数の準備 　前提：dBmpとsBmpのサイズが等しいこと
			int width = dBmp.Width;
			int height = dBmp.Height;
			int pixSize = Bitmap.GetPixelFormatSize(dBmp.PixelFormat)/8;		// バイトサイズ.

			// ビットマップアクセス
			BitmapData _imgD = dBmp.LockBits( new Rectangle( 0, 0, dBmp.Width, dBmp.Height ), ImageLockMode.ReadWrite, dBmp.PixelFormat );
			BitmapData _imgS = sBmp.LockBits( new Rectangle( 0, 0, sBmp.Width, sBmp.Height ), ImageLockMode.ReadOnly, sBmp.PixelFormat );

			for( int y= 0; y< height; y++ )
			{
				byte* adrD = (byte*)_imgD.Scan0 + _imgD.Stride * y;
				byte* adrS = (byte*)_imgS.Scan0 + _imgS.Stride * y;
				for( int x = 0; x< width; x++, adrD+=pixSize , adrS+=pixSize )
				{
					adrD[0] ^= adrS[0];
					adrD[1] ^= adrS[1];
					adrD[2] ^= adrS[2];
				}
			}
			dBmp.UnlockBits( _imgD );
			sBmp.UnlockBits( _imgS );
		}



		///////// <summary>
		///////// AA_DrawBitmap群の描画を行います
		///////// </summary>
		///////// <param name="bmp">描画対象Bitmap</param>
		///////// <param name="list">描画するDrawBitmapのリスト</param>
		//////static public void DrawCheckedBitmap(Bitmap bmp, List<PP_DrawBitmap>list)
		//////{
		//////	foreach( var obj in list )
		//////	{
		//////		switch( obj.Type )
		//////		{
		//////		// Normal 上書き
		//////		case PP_DrawBitmap.DrawType.Normal:
		//////			Graphics.FromImage( bmp ).DrawImage( obj.Bitmap, 0, 0 );
		//////			break;
		//////		// 黒をベースに書き込み（黒以外）
		//////		case PP_DrawBitmap.DrawType.BaseBlack:
		//////			UtilityGraphics.DrawBitmapColorBase( bmp, obj.Bitmap, Color.Black );
		//////			break;
		//////		// 白をベースに書き込み（白以外）
		//////		case PP_DrawBitmap.DrawType.BaseWhite:
		//////			UtilityGraphics.DrawBitmapColorBase( bmp, obj.Bitmap, Color.White );
		//////			break;
		//////		// XOR
		//////		case PP_DrawBitmap.DrawType.Xor:
		//////			UtilityGraphics.DrawBitmapColorXOR( bmp, obj.Bitmap );
		//////			break;
		//////		// 描画未サポート
		//////		default:
		//////			break;
		//////		}
		//////	}
		//////}


		///////// <summary>
		///////// AA_DrawBox群の描画を行います
		///////// </summary>
		///////// <param name="bmp">描画対象Bitmap</param>
		///////// <param name="list">描画するAA_DrawBoxのリスト</param>
		///////// <remarks>変形した四角形も対象とするため、DrawPolygon()を用いて描画する</remarks>
		//////static public void DrawCheckedBox(Bitmap bmp, List<PP_DrawBox>list)
		//////{
		//////	var gra = Graphics.FromImage(bmp);
		//////	foreach( var obj in list )
		//////	{
		//////		gra.DrawPolygon( obj.DrawPen, obj.Points );
		//////	}
		//////	gra.Dispose();
		//////}

	

		///////// <summary>
		///////// PP_DrawLine群の描画を行います
		///////// </summary>
		///////// <param name="bmp">描画対象Bitmap</param>
		///////// <param name="list">描画するPP_DrawLineのリスト</param>
		//////static public void DrawCheckedLine(Bitmap bmp, List<PP_DrawLine>list)
		//////{
		//////	var gra = Graphics.FromImage(bmp);
		//////	foreach( var obj in list )
		//////	{
		//////		gra.DrawLine( obj.DrawPen, obj.Points[0], obj.Points[1] );
		//////	}
		//////	gra.Dispose();
		//////}


		
		/// <summary>ピクセルフォーマットを指定したファイルからのピットマップ生成</summary>
		/// <param name="filepath">ファイルパス</param>
		/// <param name="pf">ピクセルフォーマット</param>
		/// <returns>生成したビットマップ</returns>
		static public Bitmap LoadBitmapPF( string filepath, PixelFormat pf )
		{
			using var timing = ProcessingTimings.Measure("ファイル読み込み・デコード・画素形式変換");
			Bitmap ret = null;
			try
			{
				// 読み込んだファイルを削除できるように、ストリームを解放する方式とした。CreateBitmap()を直接呼ぶとClone()で例外が発生するため、展開した
				using (var fs = new System.IO.FileStream(filepath, FileMode.Open, FileAccess.Read))
				using (var tmpbmp = Bitmap.FromStream(fs) as Bitmap)
				{
					if (tmpbmp == null) return null;
					if (tmpbmp.PixelFormat == pf)
					{
						// 単純にClone()だとPixelFormatがARGB(32bit)に変換されてしまう。また、Clone()を用いると、シャローコピーされて本来の目的を達成できない。
						ret = new Bitmap(tmpbmp.Width, tmpbmp.Height, tmpbmp.PixelFormat);
						using (var g = Graphics.FromImage(ret))
						{
							g.DrawImage(tmpbmp, 0, 0, ret.Width, ret.Height);
						}
					}
					else
					{
						// pixelformatが異なる場合にはCloneでもシャローではない模様。
						ret = tmpbmp.Clone(new Rectangle(new Point(0, 0), tmpbmp.Size), pf);
					}
				}
			}
			catch
			{
				// 例外発生時はnullを返す
				ret = null;
			}
			return ret;
		}


		/// <summary>
		/// 指定したファイルが画像ファイルとして扱える拡張子を持つか判定します</summary>
		/// <param name="filepath">ファイルパス</param>
		/// <returns>画像ファイルとして扱える場合、true</returns>
		static public bool IsSupportedImageFile(string filepath)
		{
			if (String.IsNullOrWhiteSpace(filepath) || File.Exists(filepath) == false) return false;

			switch (Path.GetExtension(filepath).ToUpperInvariant())
			{
				case ".JPG":
				case ".JPEG":
				case ".JPE":
				case ".JFIF":
				case ".JFI":
				case ".PNG":
				case ".BMP":
				case ".DIB":
				case ".GIF":
				case ".TIF":
				case ".TIFF":
				case ".WEBP":
				case ".AVIF":
				case ".HEIC":
				case ".HEIF":
					return true;
				default:
					return false;
			}
		}


		/// <summary>
		/// 読み込み可否が実行環境の画像コーデックに依存する画像ファイルか判定します</summary>
		/// <param name="filepath">ファイルパス</param>
		/// <returns>実行環境の画像コーデックに依存する場合、true</returns>
		static public bool IsEnvironmentCodecImageFile(string filepath)
		{
			switch (Path.GetExtension(filepath).ToUpperInvariant())
			{
				case ".WEBP":
				case ".AVIF":
				case ".HEIC":
				case ".HEIF":
					return true;
				default:
					return false;
			}
		}


		/// <summary>画像保存ダイアログ用のフィルター文字列</summary>
		public const string ImageSaveFileDialogFilter = "PNGファイル(*.png)|*.png|JPEGファイル(*.jpg;*.jpeg;*.jpe;*.jfif;*.jfi)|*.jpg;*.jpeg;*.jpe;*.jfif;*.jfi|ビットマップファイル(*.bmp;*.dib)|*.bmp;*.dib|TIFFファイル(*.tif;*.tiff)|*.tif;*.tiff|すべてのファイル(*.*)|*.*";


		/// <summary>
		/// 拡張子から画像保存形式を取得します</summary>
		/// <param name="filepath">ファイルパス</param>
		/// <param name="imgFormat">画像形式</param>
		/// <returns>対応する拡張子の場合、true</returns>
		static public bool TryGetImageFormatFromExtension(string filepath, out ImageFormat imgFormat)
		{
			switch (Path.GetExtension(filepath).ToUpperInvariant())
			{
				case ".JPG":
				case ".JPEG":
				case ".JPE":
				case ".JFIF":
				case ".JFI":
					imgFormat = ImageFormat.Jpeg;
					return true;
				case ".BMP":
				case ".DIB":
					imgFormat = ImageFormat.Bmp;
					return true;
				case ".PNG":
					imgFormat = ImageFormat.Png;
					return true;
				case ".TIF":
				case ".TIFF":
					imgFormat = ImageFormat.Tiff;
					return true;
				case ".GIF":
					imgFormat = ImageFormat.Gif;
					return true;
				default:
					imgFormat = null;
					return false;
			}
		}


		/// <summary>
		/// 保存ダイアログの選択インデックスから画像形式を取得します</summary>
		/// <param name="filterIndex">SaveFileDialog.FilterIndex</param>
		/// <returns>対応する画像形式。すべてのファイルなど画像形式が決まらない場合はnull</returns>
		static public ImageFormat GetImageFormatFromSaveFilterIndex(int filterIndex)
		{
			switch (filterIndex)
			{
				case 1:
					return ImageFormat.Png;
				case 2:
					return ImageFormat.Jpeg;
				case 3:
					return ImageFormat.Bmp;
				case 4:
					return ImageFormat.Tiff;
				default:
					return null;
			}
		}


		/// <summary>
		/// 画像形式の標準拡張子を取得します</summary>
		/// <param name="imgFormat">画像形式</param>
		/// <returns>標準拡張子</returns>
		static public string GetDefaultImageExtension(ImageFormat imgFormat)
		{
			if (imgFormat == null) return ".png";
			if (imgFormat.Guid == ImageFormat.Jpeg.Guid) return ".jpg";
			if (imgFormat.Guid == ImageFormat.Bmp.Guid) return ".bmp";
			if (imgFormat.Guid == ImageFormat.Tiff.Guid) return ".tif";
			if (imgFormat.Guid == ImageFormat.Gif.Guid) return ".gif";
			return ".png";
		}


		/// <summary>
		/// 保存先パスと保存ダイアログ選択から、実際に使用する保存パスと画像形式を決定します</summary>
		/// <param name="filepath">保存先パス</param>
		/// <param name="filterIndex">SaveFileDialog.FilterIndex</param>
		/// <param name="saveFilepath">実際の保存先パス</param>
		/// <param name="imgFormat">実際の画像形式</param>
		/// <returns>保存形式を決定できた場合、true</returns>
		static public bool TryResolveImageSaveFormat(string filepath, int filterIndex, out string saveFilepath, out ImageFormat imgFormat)
		{
			saveFilepath = filepath;
			imgFormat = ImageFormat.Png;
			if (String.IsNullOrWhiteSpace(filepath)) return false;

			if (TryGetImageFormatFromExtension(filepath, out imgFormat)) return true;

			imgFormat = GetImageFormatFromSaveFilterIndex(filterIndex) ?? ImageFormat.Png;
			saveFilepath = Path.ChangeExtension(filepath, GetDefaultImageExtension(imgFormat));
			return true;
		}


		/// <summary>
		/// 指定した画像ファイル群から、読み込みに成功したビットマップを生成します</summary>
		/// <param name="filepaths">画像ファイルパス群</param>
		/// <param name="pf">ピクセルフォーマット</param>
		/// <returns>ビットマップと表示名の組。呼び出し側でBitmapを破棄してください</returns>
		static public (Bitmap Bmp, string FileName)[] LoadBitmapFilesPF(IEnumerable<string> filepaths, PixelFormat pf)
		{
			return LoadBitmapFilesPF(filepaths, pf, out _);
		}


		/// <summary>
		/// 指定した画像ファイルからビットマップを生成します</summary>
		/// <param name="filepath">画像ファイルパス</param>
		/// <param name="pf">ピクセルフォーマット</param>
		/// <param name="bmp">生成したビットマップ</param>
		/// <param name="fileName">表示用ファイル名</param>
		/// <param name="failureReason">読み込めなかった理由</param>
		/// <returns>読み込みに成功した場合、true</returns>
		static public bool TryLoadBitmapFilePF(string filepath, PixelFormat pf, out Bitmap bmp, out string fileName, out string failureReason)
		{
			bmp = null;
			fileName = Path.GetFileName(filepath);
			if (String.IsNullOrWhiteSpace(fileName)) fileName = filepath;
			failureReason = "";

			if (File.Exists(filepath) == false)
			{
				failureReason = "ファイルが見つかりません";
				return false;
			}

			if (IsSupportedImageFile(filepath) == false)
			{
				failureReason = "非対応の拡張子です";
				return false;
			}

			bmp = LoadBitmapPF(filepath, pf);
			if (bmp == null)
			{
				failureReason = IsEnvironmentCodecImageFile(filepath) ? "画像として読み込めません（この形式は実行環境の画像コーデックに依存します）" : "画像として読み込めません";
				return false;
			}

			return true;
		}


		/// <summary>
		/// 指定した画像ファイル群から、読み込みに成功したビットマップと失敗情報を生成します</summary>
		/// <param name="filepaths">画像ファイルパス群</param>
		/// <param name="pf">ピクセルフォーマット</param>
		/// <param name="failures">読み込めなかったファイル名と理由</param>
		/// <returns>ビットマップと表示名の組。呼び出し側でBitmapを破棄してください</returns>
		static public (Bitmap Bmp, string FileName)[] LoadBitmapFilesPF(IEnumerable<string> filepaths, PixelFormat pf, out (string FileName, string Reason)[] failures)
		{
			var images = new List<(Bitmap Bmp, string FileName)>();
			var failureList = new List<(string FileName, string Reason)>();

			if (filepaths == null)
			{
				failures = Array.Empty<(string FileName, string Reason)>();
				return Array.Empty<(Bitmap Bmp, string FileName)>();
			}

			foreach (var filepath in filepaths)
			{
				if (TryLoadBitmapFilePF(filepath, pf, out var bmp, out var fileName, out var failureReason) == false)
				{
					failureList.Add((fileName, failureReason));
					continue;
				}

				images.Add((bmp, fileName));
			}

			failures = failureList.ToArray();
			return images.ToArray();
		}


		/// <summary>
		/// 複数画像読み込み結果のメッセージを作成します</summary>
		/// <param name="successCount">読み込み成功数</param>
		/// <param name="failures">読み込めなかったファイル名と理由</param>
		/// <param name="isCanceled">キャンセルされたかどうか</param>
		/// <returns>表示用メッセージ</returns>
		static public string CreateLoadBitmapFilesMessage(int successCount, (string FileName, string Reason)[] failures, bool isCanceled = false)
		{
			var sb = new StringBuilder();
			if (successCount > 0) sb.AppendLine($"{successCount}件の画像を開きました。");
			if (isCanceled) sb.AppendLine("読み込みはキャンセルされました。");
			if (failures == null || failures.Length == 0) return sb.ToString().TrimEnd();

			sb.AppendLine($"{failures.Length}件のファイルは開けませんでした。");
			foreach (var failure in failures.Take(10))
			{
				sb.AppendLine($"- {failure.FileName}: {failure.Reason}");
			}
			if (failures.Length > 10)
			{
				sb.AppendLine($"... 他 {failures.Length - 10}件");
			}
			return sb.ToString().TrimEnd();
		}



		/// <summary>
		/// 指定したファイルをロックすることなくBitmapを作成する。
		/// </summary>
		/// <param name="filepath">作成元のファイルのパス</param>
		/// <returns>作成したSystem.Drawing.Image。</returns>
		/// <remarks>Bitmap.FromFile()ではストリームが解放されない模様。本メソッドでは明示的にClose()およびusingによるdispose処理を行う</remarks>
		public static Bitmap CreateBitmap(string filepath)
		{
			using (var fs = new System.IO.FileStream(
				filepath,
				System.IO.FileMode.Open,
				System.IO.FileAccess.Read))
			using (var tmpbmp = Bitmap.FromStream(fs) as Bitmap)
			{
				return tmpbmp == null ? null : tmpbmp.Clone(new Rectangle(new Point(0, 0), tmpbmp.Size), tmpbmp.PixelFormat);
			}
		}


		/// <summary>
		/// Bitmapファイルをロックすることなく作成する
		/// </summary>
		/// <param name="bmp">保存するビットマップ</param>
		/// <param name="filepath">ファイルパス</param>
		/// <param name="imgFormat">ファイルフォーマット（指定が無い場合には拡張子で判断、不明な場合にはPNGとする</param>
		/// <returns>成否</returns>
		/// <remarks>Bitmap.Save()ではストリームが解放されているか不明（テストした限りロックされない）。本メソッドでは明示的にClose()およびusingによるdispose処理を行う</remarks>
		public static bool SaveBitmap(Bitmap bmp, string filepath, ImageFormat imgFormat = null)
		{
			try
			{
				// フォーマットが未指定の場合、拡張子で判断する
				if (imgFormat == null)
				{
					if (TryGetImageFormatFromExtension(filepath, out imgFormat) == false) imgFormat = ImageFormat.Png;
				}

				//　ファイルストリームで書き込み、確実にdisposeする(using)
				using (var fs = new System.IO.FileStream(
					filepath,
					System.IO.FileMode.Create,
					System.IO.FileAccess.Write))
				{
					bmp.Save(fs, imgFormat);
					fs.Close();
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Html色文字列からColorに変換を試みます
		/// </summary>
		/// <param name="htmlStr">Html形式色文字列</param>
		/// <param name="getColor">変換した色</param>
		/// <returns>成否</returns>
		/// <remarks>ColorTranslator.FromHtml()のtryまたはチェックの方法がなく、TryParse()の代用として使用する</remarks>
		static public bool TryColorFromHtml(string htmlStr, out Color getColor)
		{
			try
			{
				getColor = ColorTranslator.FromHtml(htmlStr);
				return true;
			}
			catch
			{
			}
			getColor = Color.Black;
			return false;
		}



		/// <summary>
		/// Format8bppIndexedのビットマップでグレーモードのインデックスを設定する
		/// </summary>
		/// <param name="width">作成する画像幅</param>
		/// <param name="height">作成する画像高さ</param>
		/// <returns>8ビットインデックスビットマップ（グレー用のインデックス）　</returns>
		static public Bitmap CreateBitmap8bppGrayMode(int width, int height )
		{
			var bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

			// グレーにするためのパレットの設定  bmp.Palette.Entries[]に直接設定しても反映されない。 内部的には、PalleteプロパティへのSetで反映処理を行っているのかもしれない。
			ColorPalette pal = bmp.Palette;
			for (int i = 0; i < 256; ++i)
			{
				pal.Entries[i] = Color.FromArgb(i, i, i);
			}
			bmp.Palette = pal;
			return bmp;
		}



		/// <summary>
		/// Format24bppのグレー画像を8bppIndex画像に変換します
		/// </summary>
		/// <param name="bmp24">24bppフォーマットBitmap（変換元画像）</param>
		/// <returns>8bppIndexedフォーマットグレー画像</returns>
		/// <remarks>入力される画像はグレーであるとしてG成分のみを利用します。　変換後の8bppビットマップはグレーのインデックスが設定されます</remarks>
		static public unsafe Bitmap ConvertBitmap24bppGrayTo8bpp(Bitmap bmp24)
		{
			var bmp8i = CreateBitmap8bppGrayMode(bmp24.Width, bmp24.Height);

			// 変数の準備 　前提：dBmpとsBmpのサイズが等しいこと
			int width = bmp24.Width;
			int height = bmp24.Height;

			// ビットマップアクセス
			BitmapData _imgS = bmp24.LockBits(new Rectangle(0, 0, bmp24.Width, bmp24.Height), ImageLockMode.ReadOnly, bmp24.PixelFormat);
			BitmapData _imgD = bmp8i.LockBits(new Rectangle(0, 0, bmp8i.Width, bmp8i.Height), ImageLockMode.ReadWrite, bmp8i.PixelFormat);
			try
			{
				for (int y = 0; y < height; y++)
				{
					byte* adrD = (byte*)_imgD.Scan0 + _imgD.Stride * y;
					byte* adrS = (byte*)_imgS.Scan0 + _imgS.Stride * y;
					for (int x = 0; x < width; x++, adrD++, adrS+= 3)
					{
						*adrD = adrS[0];
					}
				}
			}
			finally
			{
				bmp24.UnlockBits(_imgS);
				bmp8i.UnlockBits(_imgD);
			}

			return bmp8i;
		}


		/// <summary>
		/// Format8bppIndexed画像をFormat24bppRgbに変換します。
		/// </summary>
		/// <param name="bitmap8bpp">8bppIndexed画像（グレー）</param>
		/// <returns>Format24bppRgb画像</returns>
		/// <remarks>入力画像はグレー画像インデックスとして扱います。インデックスは参照しません</remarks>
		static public unsafe Bitmap Convert8bppTo24bpp(Bitmap bmp8i)
		{
			var bmp24 = new Bitmap(bmp8i.Width, bmp8i.Height, PixelFormat.Format24bppRgb);


			// 変数の準備 　前提：dBmpとsBmpのサイズが等しいこと
			int width = bmp24.Width;
			int height = bmp24.Height;

			// ビットマップアクセス
			BitmapData _imgD = bmp24.LockBits(new Rectangle(0, 0, bmp24.Width, bmp24.Height), ImageLockMode.ReadWrite, bmp24.PixelFormat);
			BitmapData _imgS = bmp8i.LockBits(new Rectangle(0, 0, bmp8i.Width, bmp8i.Height), ImageLockMode.ReadOnly, bmp8i.PixelFormat);
			try
			{
				for (int y = 0; y < height; y++)
				{
					byte* adrD = (byte*)_imgD.Scan0 + _imgD.Stride * y;
					byte* adrS = (byte*)_imgS.Scan0 + _imgS.Stride * y;
					for (int x = 0; x < width; x++, adrD += 3, adrS++)
					{
						adrD[0] = *adrS;
						adrD[1] = *adrS;
						adrD[2] = *adrS;
					}
				}
			}
			finally
			{
				bmp24.UnlockBits(_imgD);
				bmp8i.UnlockBits(_imgS);
			}


			return bmp24;
		}


		/// <summary>
		/// 2点間の距離を計算します</summary>
		/// <param name="x0">始点X</param>
		/// <param name="y0">始点Y</param>
		/// <param name="x1">終点X</param>
		/// <param name="y1">終点Y</param>
		/// <returns>2点間のユークリッド距離</returns>
		static public double CalcDistance(double x0, double y0, double x1, double y1)
		{
			return Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
		}


		/// <summary>
		/// 2点間の距離を計算します</summary>
		/// <param name="p0">始点</param>
		/// <param name="p1">終点</param>
		/// <returns>2点間のユークリッド距離</returns>
		static public double CalcDistance(Point p0, Point p1)
		{
			return CalcDistance(p0.X, p0.Y, p1.X, p1.Y);
		}


		/// <summary>
		/// 2点間の距離を計算します</summary>
		/// <param name="p0">始点</param>
		/// <param name="p1">終点</param>
		/// <returns>2点間のユークリッド距離</returns>
		static public double CalcDistance(PointF p0, PointF p1)
		{
			return CalcDistance(p0.X, p0.Y, p1.X, p1.Y);
		}


		/// <summary>
		/// 2点を指定して直線の一般式 aX + bY + c = 0 のa,b,cパラメータを算出する</summary>
		/// <param name="p0"> 点0</param>
		/// <param name="p1"> 点1</param>
		/// <returns>Ax + By + C = 0のパラメータ</returns>
		static public (double A, double B, double C) GetParamAxByC(Point p0, Point p1)
		{
			// 直線式 ax + by + c = 0として a,b,c決定
			var a = (double)p1.Y - p0.Y;
			var b = (double)p0.X - p1.X;                      // a,b 符号入れ替え
			if (a < 0)
			{
				a = -a;
				b = -b;
			}
			// ax + by + c= 0　で始点を通る cを求める。
			var c = -a * p0.X - b * p0.Y;
			return (a, b, c);
		}


		/// <summary>
		/// 2点を指定して直線の一般式 aX + bY + c = 0 のa,b,cパラメータを算出する( a^2 + b^2 == 1.0に正規化）　</summary>
		/// <param name="p0"> 点0</param>
		/// <param name="p1"> 点1</param>
		/// <returns>Ax + By + C = 0のパラメータ</returns>
		static public (double A, double B, double C) GetParamNmlAxByC(Point p0, Point p1)
		{
			// 直線式 ax + by + c = 0として a,b,c決定,  a^2 + b^2 == 1.0とする
			var a = (double)p1.Y - p0.Y;
			var b = (double)p0.X - p1.X;                      // a,b 符号入れ替え
			var w = Math.Sqrt(a * a + b * b);
			if (a < 0)
			{
				a = -a / w;
				b = -b / w;
			}

			// ax + by + c= 0　で始点を通る cを求める。
			var c = -a * p0.X - b * p0.Y;
			return (a, b, c);
		}

		/// <summary>線分と指定した点との距離を算出します</summary>
		/// <param name="bgn">線分始点</param>
		/// <param name="end">線分終点</param>
		/// <param name="pos">指定点</param>
		/// <returns>距離</returns>
		static public double DistanceLineToPoint(PointF bgn, PointF end, PointF pos)
		{
			var dx = end.X - bgn.X;
			var dy = end.Y - bgn.Y;
			if (dx == 0 && dy == 0) return Math.Sqrt((pos.X - bgn.X) * (pos.X - bgn.X) + (pos.Y - bgn.Y) * (pos.Y - bgn.Y));
			var r2 = dx * dx + dy * dy;
			var t = (dx * (pos.X - bgn.X) + dy * (pos.Y - bgn.Y)) / (double)r2;
			if (t < 0) return Math.Sqrt((pos.X - bgn.X) * (pos.X - bgn.X) + (pos.Y - bgn.Y) * (pos.Y - bgn.Y));
			if (t > 1) return Math.Sqrt((pos.X - end.X) * (pos.X - end.X) + (pos.Y - end.Y) * (pos.Y - end.Y));
			var cx = (1 - t) * bgn.X + t * end.X;
			var cy = (1 - t) * bgn.Y + t * end.Y;
			return Math.Sqrt((pos.X - cx) * (pos.X - cx) + (pos.Y - cy) * (pos.Y - cy));
		}



		/// <summary>線分と指定した点との距離と線分上の位置を算出します</summary>
		/// <param name="bgn">線分始点</param>
		/// <param name="end">線分終点</param>
		/// <param name="pos">指定点</param>
		/// <returns>距離と線分上の位置</returns>
		static public (double Dist, PointF Pos) ShortestLineToPoint(PointF bgn, PointF end, PointF pos)
		{
			var dx = end.X - bgn.X;
			var dy = end.Y - bgn.Y;
			if (dx == 0 && dy == 0) return (Math.Sqrt((pos.X - bgn.X) * (pos.X - bgn.X) + (pos.Y - bgn.Y) * (pos.Y - bgn.Y)), bgn);
			var r2 = dx * dx + dy * dy;
			var t = (dx * (pos.X - bgn.X) + dy * (pos.Y - bgn.Y)) / (double)r2;
			if (t < 0) return (Math.Sqrt((pos.X - bgn.X) * (pos.X - bgn.X) + (pos.Y - bgn.Y) * (pos.Y - bgn.Y)), bgn);
			if (t > 1) return (Math.Sqrt((pos.X - end.X) * (pos.X - end.X) + (pos.Y - end.Y) * (pos.Y - end.Y)), end);
			var cx = (1 - t) * bgn.X + t * end.X;
			var cy = (1 - t) * bgn.Y + t * end.Y;
			return (Math.Sqrt((pos.X - cx) * (pos.X - cx) + (pos.Y - cy) * (pos.Y - cy)), new PointF((float)cx, (float)cy));
		}


		/// <summary>線分と指定した点との距離を算出します</summary>
		/// <param name="bgn">線分始点</param>
		/// <param name="end">線分終点</param>
		/// <param name="pos">指定点</param>
		/// <returns>距離</returns>
		static public double DistanceLineToPoint(Point bgn, Point end, Point pos)
		{
			return DistanceLineToPoint(new PointF(bgn.X, bgn.Y), new PointF(end.X, end.Y), new PointF(pos.X, pos.Y));
		}



		/// <summary>線分と指定した点との距離と線分上の位置を算出します</summary>
		/// <param name="bgn">線分始点</param>
		/// <param name="end">線分終点</param>
		/// <param name="pos">指定点</param>
		/// <returns>距離と線分上の位置</returns>
		static public (double Dist, PointF Pos) ShortestLineToPoint(Point bgn, Point end, Point pos)
		{
			return ShortestLineToPoint(new PointF(bgn.X, bgn.Y), new PointF(end.X, end.Y), new PointF(pos.X, pos.Y));
		}



		#region ----------  矩形内側をエッジを描画するメソッド (DrawRectangle()は右、下のエッジが外側になるため） ---------

		/// <summary>矩形内側をエッジを描画します</summary>
		/// <param name="gra">描画先Graphics</param>
		/// <param name="pen">描画ペン</param>
		/// <param name="rect">矩形</param>
		/// <returns>描画用に変換されたRectangle</returns>
		/// <remarks>通常のDrawRectagleでは　右側および下側のラインは矩形外部の位置(+1された位置）を描画する仕様になっている。本メソッドはは内側のエッジ上を描画する。</remarks>
		static public Rectangle DrawInsideEdgeRectangle(Graphics gra, Pen pen, Rectangle rect)
		{
			var newrect = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
			gra.DrawRectangle(pen, newrect);
			return newrect;
		}


		/// <summary>矩形群内側をエッジを描画します</summary>
		/// <param name="gra">描画先Graphics</param>
		/// <param name="pen">描画ペン</param>
		/// <param name="rects">矩形群</param>
		/// <returns>描画用に変換されたRectangle</returns>
		/// <remarks>通常のDrawRectaglesでは　右側および下側のラインは矩形外部の位置(+1された位置）を描画する仕様になっている。本メソッドはは内側のエッジ上を描画する。</remarks>
		static public Rectangle[] DrawInsideEdgeRectangles(Graphics gra, Pen pen, Rectangle[] rects)
		{
			var newrects = rects.Select(r => new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1)).ToArray();
			gra.DrawRectangles(pen, newrects);
			return newrects;
		}

		#endregion //  ----------  内側をエッジを描画するメソッド ---------

	
		/// <summary>
		/// DrawRectangleのfloat版拡張メソッドです</summary>
		/// <param name="g">グラフィックス</param>
		/// <param name="pen">ペン</param>
		/// <param name="rect">矩形(float)</param>
		public static void DrawRectangleF(this Graphics g, Pen pen, RectangleF rect)
		{
			g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// DrawRectanglesのfloat版拡張メソッドです</summary>
		/// <param name="g">グラフィックス</param>
		/// <param name="pen">ペン</param>
		/// <param name="rects">矩形(float)群</param>
		public static void DrawRectangleFs(this Graphics g, Pen pen, RectangleF[] rects)
		{
			foreach (var rf in rects) g.DrawRectangle(pen, rf.X, rf.Y, rf.Width, rf.Height);
		}


		/// <summary>
		/// ポリゴン内の点を調べる際に使用するX軸との交差チェックメソッドです。線分がX軸と交差するか、および原点を含むかを返します</summary>
		/// <param name="bp">線分開始点</param>
		/// <param name="ep">線分終了点</param>
		/// <returns>IsCross:X軸と交差する場合true　IsZp:原点を線分中に含む場合true、　IsEpYz:Epの高さが0（PEND)　Yvct:1上向き -1下向き</returns>
		static public (bool IsCross, bool IsZp, bool IsEpYz, int Yvct) CheckY0LineSegment(PointF bp, PointF ep)
		{
			// 差分ベクトルと　Ax + By+ C = 0 のパラメータ算出
			var vct = new PointF(ep.X - bp.X, ep.Y - bp.Y);
			var pA = vct.Y;
			var pB = -vct.X;
			var pC = vct.X * bp.Y - vct.Y * bp.X;

			if (vct.X == 0 && vct.Y == 0) return (false, false, false, 0);

			var yVct = (bp.Y == ep.Y) ? 0 : ((bp.Y > ep.Y) ? -1 : 1);

			// X軸との交点が線分上（bp含まず　ep　PEND）にあるか調べる。結果的に跨ぐ場合には後で調整する
			// pAが0でない＝bpとepで高さの変化がある場合		
			if (pA != 0)
			{
				// X軸(y==0)との交点のx位置
				var zX = -pC / pA;
				if (zX < 0) return (false, false, false, yVct);       // 交点が原点より左にある場合は false
				var f = bp.Y * ep.Y < 0;
				return (f, (bp.Y * ep.Y <= 0) && zX == 0, ep.Y == 0, yVct);
			}
			else
			{
				// 水平線の場合にはクロスとは認めない（ただし原点を含む場合、IsZpのみtrueとなる）
				return (false, bp.X * ep.X <= 0 && ep.Y == 0, ep.Y == 0, yVct);
			}
		}


		/// <summary>
		/// 点群で示されるポリゴン内に点が含まれるか否か反転します。</summary>
		/// <param name="poss">点群（ポリゴン）</param>
		/// <param name="pos">判定対象点</param>
		/// <returns>含まれる場合、真</returns>
		static public bool IsContainsPoly(PointF[] poss, PointF pos)
		{
			var poly = poss.Concat(new PointF[] { poss[0] }).Select(p => new PointF(p.X - pos.X, p.Y - pos.Y)).ToArray();
			var lins = Enumerable.Range(0, poly.Length - 1).Select(i => CheckY0LineSegment(poly[i], poly[i + 1])).ToArray();
			if (lins.Any(inf => inf.IsZp)) return true;         // 線上

			var cnt = 0;
			int? bgnYvct = null;
			int? pendYvct = null;
			foreach (var ww in lins)
			{
				if (ww.IsCross) cnt++;      // X軸の正側を明らかに跨いだ
											// 最後の点がちょうどX軸上(Y==0)ならば今後の動きによってカウントアップする
				if (pendYvct.HasValue && pendYvct * ww.Yvct == 1) cnt++;
				if (ww.IsEpYz == false) pendYvct = null;
				else if (pendYvct == null || pendYvct == 0) pendYvct = ww.Yvct;
				if (bgnYvct == null && ww.Yvct != 0) bgnYvct = ww.Yvct;
			}
			if (pendYvct.HasValue && bgnYvct.HasValue && pendYvct * bgnYvct == 1) cnt++;

			return cnt % 2 != 0;        // 交点が奇数ならば内部

		}


		#region 色変換 --------------------------------------------------------------------
		/// <summary>色相(Hue)表現は0-HueMax【度】で表すこととします</summary>
		public const int HueMax = 360;
		/// <summary>OpenCVにおけるHSVモードにおける色相は0-180</summary>
		public const int HueMaxCv = 180;


		/// <summary>HSVをRGBに変換します</summary>
		/// <param name="hh">色相(0-360)</param>
		/// <param name="s">彩度(0-255)</param>
		/// <param name="v">明度(0-255)</param>
		static public void RgbFromHsv(int h, int s, int v, out int r, out int g, out int b)
		{
			h %= 360;
			int Hi = (int)Math.Floor((double)h / 60) % 6;

			float f = ((float)h / 60) - Hi;
			float p = ((float)v / 255) * (1 - ((float)s / 255));
			float q = ((float)v / 255) * (1 - f * ((float)s / 255));
			float t = ((float)v / 255) * (1 - (1 - f) * ((float)s / 255));
			p *= 255;
			q *= 255;
			t *= 255;

			r = g = b = 0;
			switch (Hi)
			{
				case 0: r = v; g = (int)t; b = (int)p; break;
				case 1: r = (int)q; g = v; b = (int)p; break;
				case 2: r = (int)p; g = v; b = (int)t; break;
				case 3: r = (int)p; g = (int)q; b = v; break;
				case 4: r = (int)t; g = (int)p; b = v; break;
				case 5: r = v; g = (int)p; b = (int)q; break;
			}
		}


		/// <summary>HSVをRGBに変換します</summary>
		/// <param name="h">色相(0-360)</param>
		/// <param name="s">彩度(0-255)</param>
		/// <param name="v">明度(0-255)</param>
		/// <returns>R,G,B値を返します</returns>
		static public (int R, int G, int B) RgbFromHsv(int h, int s, int v)
		{
			int r, g, b;
			RgbFromHsv(h, s, v, out r, out g, out b);
			return (r, g, b);
		}


		/// <summary>RGBをHSVに変換します</summary>
		/// <param name="r">R成分(0-255)</param>
		/// <param name="g">G成分(0-255)</param>
		/// <param name="b">B成分(0-255)</param>
		/// <param name="h">H成分(0-359)</param>
		/// <param name="s">S成分(0-255)</param>
		/// <param name="v">V成分(0-255)</param>
		static public void HsvFromRgb(int r, int g, int b, out int h, out int s, out int v)
		{
			float rr = r / 255.0f;
			float gr = g / 255.0f;
			float br = b / 255.0f;

			var list = new float[] { rr, gr, br };
			var max = list.Max();
			var min = list.Min();

			float hh, sr, vr;
			if (max == min) hh = 0;
			else if (max == rr) hh = (60 * (gr - br) / (max - min) + 360) % 360;
			else if (max == gr) hh = 60 * (br - rr) / (max - min) + 120;
			else hh = 60 * (rr - gr) / (max - min) + 240;

			sr = (max == 0) ? 0 : (max - min) / max;
			vr = max;

			h = (int)hh;
			s = (int)(sr * 255.0 + 0.5);
			v = (int)(vr * 255.0 + 0.5);
		}


		/// <summary>RGBをHSVに変換します</summary>
		/// <param name="r">R成分(0-255)</param>
		/// <param name="g">G成分(0-255)</param>
		/// <param name="b">B成分(0-255)</param>
		/// <returns>HSV値を返します</returns>
		static public (int H, int S, int V) HsvFromRgb(int r, int g, int b)
		{
			int h, s, v;
			HsvFromRgb(r, g, b, out h, out s, out v);
			return (h, s, v);
		}




		/// <summary>
		/// RGB値からグレー値（輝度値）を算出します</summary>
		/// <param name="rr">赤成分</param>
		/// <param name="gg">緑成分</param>
		/// <param name="bb">青成分</param>
		/// <returns>グレー値（輝度）</returns>
		static public int RgbToGray(int rr, int gg, int bb)
		{
			return (int)(rr * 0.299 + gg * 0.587 + bb * 0.114);
		}


		/// <summary>
		/// RGBからYCbCrを算出します</summary>
		/// <param name="r">R成分(0-255)</param>
		/// <param name="g">G成分(0-255)</param>
		/// <param name="b">B成分(0-255)</param>
		/// <returns>Y：0～255, Cb: -128～127, Cr: -128～127</returns>
		/// <remarks>Cb, Crは場合により +128して使用します</remarks>
		static public void YCrCbFromRGB(int r, int g, int b, out int y, out int cb, out int cr)
		{
			y = (int)(0.299 * r + 0.587 * g + 0.114 * b);
			cb = (int)(-0.169 * r - 0.331 * g + 0.500 * b);
			cr = (int)(0.500 * r - 0.419 * g - 0.081 * b);
		}


		/// <summary>
		/// RGBからYCbCrを算出します</summary>
		/// <param name="r">R成分(0-255)</param>
		/// <param name="g">G成分(0-255)</param>
		/// <param name="b">B成分(0-255)</param>
		/// <returns>Y：0～255, Cb: -128～127, Cr: -128～127</returns>
		/// <remarks>Cb, Crは場合により +128して使用します</remarks>
		static public (int Y, int Cb, int Cr) YCrCbFromRGB(int r, int g, int b)
		{
			var y = (int)(0.299 * r + 0.587 * g + 0.114 * b);
			var cb = (int)(-0.169 * r - 0.331 * g + 0.500 * b);
			var cr = (int)(0.500 * r - 0.419 * g - 0.081 * b);
			return (y, cb, cr);
		}
		#endregion // 変換 ---------------------------------------------------------------


	}

}

