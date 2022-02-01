using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.ComponentModel;			// [Category]等の指定
using System.Reflection;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Text.RegularExpressions;


namespace ColorImageViewProject
{
	/// <summary>環境設定データ</summary>
	[TypeConverter(typeof(PropertyDisplayConverter))]
	public class CompositeLocalSettings:IComparable
	{
		/// <summary>無条件にチェック画面（再解析画面）を出すか否か</summary>
		private bool _isChecksUnconditionally = true;						// Defaultデータフォルダ.
		/// <summary>再解析で成功と見なす一致画像数</summary>
		private int _numberOfImages = 3;
 
		// ---- private ----

		// ---------- public メソッド ----------

		/// <summary> デフォルトコンストラクタです</summary>
		public CompositeLocalSettings()										// コンストラクタ
		{
			SetDefaultValue();
		}


		#region 全メンバー対象メソッド


		/// <summary>コンストラクタです（同一オブジェクト生成用）</summary>
		/// <param name="isChkUncond">無条件にチェック画面（再解析画面）を出すか否か</param>
		/// <param name="numOfImages">再解析で成功と見なす一致画像数</param>
		private CompositeLocalSettings( bool isChkUncond, int numOfImages )
		{
			// 全メンバーのコピー
			_isChecksUnconditionally = true;						// Defaultデータフォルダ.
			_numberOfImages = 3;
		}


		/// <summary> デフォルト値のセット </summary>
		public void SetDefaultValue()
		{
			_isChecksUnconditionally = true;						// Defaultデータフォルダ.
			_numberOfImages = 3;

		}

		/// <summary>全メンバー比較を行います　（内部処理用）</summary>
		/// <param name="isChkUncond">無条件にチェック画面（再解析画面）を出すか否か</param>
		/// <param name="numOfImages">再解析で成功と見なす一致画像数</param>
		private int CompareAllMenber( bool isChkUncond, int numOfImages )
		{
			// 全メンバーの比較
			if( _isChecksUnconditionally != isChkUncond ) return (_isChecksUnconditionally== true)? 1: -1;

			var w = _numberOfImages - numOfImages;
			if( w != 0 ) return w;

			return 0;					// 一致
		}


		#endregion // 全メンバー対象メソッド




		/// <summary> 設定ファイルをロードします </summary>
		/// <param title="fileName">ファイル名（パス）</param>
		/// <returns>成否を返します。（成功：true  失敗:false）</returns>
		public bool LoadSettingData(string fileName)			// 設定値の読み込み
		{
			bool ret = true;
			MethodBase methodbase = MethodBase.GetCurrentMethod();
			XmlTextReader reader = null;

			if (File.Exists(fileName) == false)
			{
				// ファイルが存在しなかったので、デフォルト値をセット
				SetDefaultValue();
				return ret;
			}
			try
			{
				reader = new XmlTextReader(fileName);
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (reader.LocalName.Equals("IsChecksUnconditionally"))
						{
							string str = reader.ReadString();
							bool w;
							if (bool.TryParse(str, out w))
							{
								_isChecksUnconditionally = w;
							}
						}
						if (reader.LocalName.Equals("NumberOfImages"))
						{
							string str = reader.ReadString();
							int w;
							if (int.TryParse(str, out w))
							{
								_numberOfImages = w;
							}
						}

					}
				}
			}
			catch
			{
				ret = false;
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
					reader = null;
				}
			}
			return ret;
		}


		/// <summary> 設定値をロードします。(デフォルトファイル名） </summary>
		/// <returns>成否を返します。（成功：true  失敗:false）</returns>
		public bool LoadSettingData()						// 引数無し設定値ロード
		{
			return LoadSettingData(GetDefaultFileName());			// デフォルトの設定値ファイルをロード
		}


		/// <summary>設定値デフォルトファイルパスを取得します</summary>
		/// <returns>設定値デフォルトファイルパス</returns>
		private string GetDefaultFileName()
		{
			// デフォルトファイルパスは「実行フォルダ」の"CompositeSettings.xml"
			var directory = Application.StartupPath;
			return Path.Combine(directory, "CompositeSettings.xml");
		}


		/// <summary> 設定値を保存します </summary>
		/// <param title="fileName">設定値ファイル名</param>
		/// <returns>成否を返します。（成功：true  失敗:false）</returns>
		public bool SaveSettingData(string fileName)		// 設定値の書き込み
		{
			bool ret = true;
			XmlTextWriter writer = null;
			try
			{
				writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
				writer.Formatting = Formatting.Indented;
				writer.WriteStartDocument(true);
				writer.WriteStartElement("Settings-Composite");
				writer.WriteElementString("IsChecksUnconditionally", _isChecksUnconditionally.ToString() );
				writer.WriteElementString("NumberOfImages", _numberOfImages.ToString());

				writer.WriteEndDocument();
				writer.Flush();
			}
			catch {
				ret = false;
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
					writer = null;
				}
			}
			return ret;
		}



		/// <summary>デフォルトファイル名に設定値を保存します </summary>
		/// <returns>成否を返します。（成功：true  失敗:false）</returns>
		public bool SaveSettingData()						// 引数無し設定値セーブ
		{
			return SaveSettingData(GetDefaultFileName());			// デフォルトの設定値ファイルにセーブ
		}




		/// <summary>
		/// 同一の値を持つ(別）オブジェクトを生成して返します
		/// </summary>
		/// <returns>同一値の新規オブジェクト</returns>
		public CompositeLocalSettings Clone()
		{
			return new CompositeLocalSettings( _isChecksUnconditionally, _numberOfImages );
		}



		/// <summary>比較した値を返します (IComparable)</summary>
		/// <param title="obj">比較対象</param>
		/// <returns>比較値 (自分 - obj ）</returns>
		public int CompareTo( object obj )
		{
			var dst = (CompositeLocalSettings)obj;
			int w = dst.CompareAllMenber( _isChecksUnconditionally, _numberOfImages );
			return -w;		// 正負を逆にする　（比較対象側から CompareAllMenber()を呼び出しているため）
		}

		// ----------- プロパティ ----------

		[Category("１．全般")]
		[Description("１回目の解析時に強制的にチェック画面（再解析画面）を表示するか否か")]
		[DefaultValue(true)]
		[PropertyDisplayName("01.IsChecksUnconditionally")]
		/// <summary>データフォルダの位置を指定します</summary>
		public bool IsChecksUnconditionally
		{
			get {
				return _isChecksUnconditionally;
			}
			set { _isChecksUnconditionally = value; }
		}

		[Category("１．全般")]
		[Description("再解析で「成功」と見なす最低画像枚数。ただし０は全部画像")]
		[DefaultValue(3)]
		[PropertyDisplayName("データ種別")]
		/// <summary> 入力画像データ種別を取得または設定します </summary>
		public int NumberOfImages
		{
			get { return _numberOfImages; }
			set	{ _numberOfImages = value;	}
		}





	// ---------- private ----------

	}

	// □□□　PropertyGrid表示用属性クラス □□□
    /// <summary>
    /// プロパティ表示名を外部から設定するための属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    class PropertyDisplayNameAttribute : Attribute
    {
		/// <summary>表示名称を保持します</summary>
		private string myPropertyDisplayName;


		/// <summary>コンストラクタです</summary>
		/// <param title="title"></param>
		public PropertyDisplayNameAttribute( string name )
		{
			myPropertyDisplayName = name;
		}

        /// <summary>表示名称を取得します</summary>
		public string PropertyDisplayName
		{
			get
			{
				return myPropertyDisplayName;
			}
		}
    }



    /// <summary>
    /// プロパティ表示名でPropertyDisplayPropertyDescriptorクラスを使用するために
    /// TypeConverter属性に指定するためのTypeConverter派生クラス。
    /// </summary>
    public class PropertyDisplayConverter : TypeConverter
    { 
		/// <summary>
		/// コンストラクタです
		/// </summary>
		public PropertyDisplayConverter()
		{
		}


		public override PropertyDescriptorCollection GetProperties( ITypeDescriptorContext context, object instance, Attribute[] filters )
		{
			PropertyDescriptorCollection collection = new PropertyDescriptorCollection( null );

			PropertyDescriptorCollection properies = TypeDescriptor.GetProperties( instance, filters, true );
			foreach( PropertyDescriptor desc in properies )
			{
				collection.Add( new PropertyDisplayPropertyDescriptor( desc ) );
			}

			return collection;
		}

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
	}

    /// <summary>
    /// プロパティの説明（＝情報）を提供するクラス。DisplayNameをカスタマイズする。
    /// </summary>
    public class PropertyDisplayPropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor oneProperty;

        public PropertyDisplayPropertyDescriptor(PropertyDescriptor desc) : base(desc)
        {
            oneProperty = desc;
        }

        public override bool CanResetValue(object component)
        {
            return oneProperty.CanResetValue(component);
        }

		public override Type ComponentType
		{
			get
			{
				return oneProperty.ComponentType;
			}
		}

		public override object GetValue( object component )
		{
			return oneProperty.GetValue( component );
		}

		public override string Description
		{
			get
			{
				return oneProperty.Description;
			}
		}

		public override string Category
		{
			get
			{
				return oneProperty.Category;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return oneProperty.IsReadOnly;
			}
		}

		public override void ResetValue( object component )
		{
			oneProperty.ResetValue( component );
		}

		public override bool ShouldSerializeValue( object component )
		{
			return oneProperty.ShouldSerializeValue( component );
		}

		public override void SetValue( object component, object value )
		{
			oneProperty.SetValue( component, value );
		}

		public override Type PropertyType
		{
			get
			{
				return oneProperty.PropertyType;
			}
		}

		public override string DisplayName
		{
			get
			{
				PropertyDisplayNameAttribute attrib = 
                            (PropertyDisplayNameAttribute)oneProperty.Attributes[typeof( PropertyDisplayNameAttribute )];
				if( attrib != null )
				{
					return attrib.PropertyDisplayName;
				}

				return oneProperty.DisplayName;
			}
		}

    }


}
