using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IPPLibrary
{
	/// <summary>
	///
	/// </summary>
	/// <remarks>はっきりするまで前の形を保持しておく　</remarks>
	[Serializable]
	public class AA_AParam: IAA_AParam
	{
		public double H{ get;set; }						//	ピーク値
		public double A{ get;set; }						//	面積
		public double W{ get;set; }						//	幅(正規分布幅)
		public double Wa{ get;set; }					//	幅(80%幅)
		public double Wb{ get;set; }					//	幅(70%幅)
		public double Wc{ get;set; }					//	幅(50%幅)
		public double Wd{ get;set; }					//	幅(20%幅)
		public double We{ get;set; }					//	幅(全幅)
		public double G{ get;set; }						//	選択指標
		public double Kurt{ get;set; }					//	尖度
		public Point Head{ get;set; }					//  ROI
		public Point Tail{ get;set; }					//	ROI
		public double Moment0l{ get;set; }
		public double Moment0r{ get;set; }
		public double Moment1l{ get;set; }
		public double Moment1r{ get;set; }
		public double Moment2l{ get;set; }
		public double Moment2r{ get;set; }
		public int Nv{ get;set; }						//	血管番号
		public int Vp{ get;set; }						//	血管での位置
		public double[] Dw{ get;set; }					//	同一血管上の前後のROIでのWdとの差
		public int Edge_dy{ get;set; }					//	エッジからの距離（垂直方向）
		public double Meiko_u{ get;set; }				//	迷光指標(上)
		public double Meiko_l{ get;set; }				//	迷光指標(下)
		public bool Remove_flg_hw_voi{ get;set; }		//  指標hw_voi指標による除去フラグ
		public bool Remove_flg_h1{ get;set; }			//  指標h1 < 0.075による除去フラグ		Dictionary<string, object> NamedObjects;	// 名前付きオブジェクト（拡張用）
		public double Hgb{	get;	set;	}
		public double Hgb_raw{	get;set;	}			// 生のHGB
		public double Voi{	get;set;	}
		public double PBM_alpha{	get;set;}			// 電流輝度補正係数
		public double O2M_alpha{	get;set;}			// 酸素化補正係数
		public Dictionary<string, object> NamedObjects{	get;set;}	// 名前付きオブジェクト（拡張用）

		/// <summary>
		/// コンストラクタです
		/// </summary>
		public AA_AParam()
		{
			Dw= new double[4];
			NamedObjects = new Dictionary<string,object>();
		}


		/// <summary>
		/// クローンを生成します
		/// </summary>
		/// <returns>クローンオブジェクト</returns>
		public object Clone()
		{
			var ret = new AA_AParam();
			ret.H = H;						//	ピーク値
			ret.A = A;						//	面積
			ret.W= W;						//	幅(正規分布幅)
			ret.Wa= Wa;					//	幅(80%幅)
			ret.Wb= Wb;				//	幅(70%幅)
			ret.Wc= Wc;					//	幅(50%幅)
			ret.Wd= Wd;					//	幅(20%幅)
			ret.We= We;				//	幅(全幅)
			ret.G= G;						//	選択指標
			ret.Kurt= Kurt;					//	尖度
			ret.Head = Head; 					//  ROI
			ret.Tail= Tail;					//	ROI
			ret.Moment0l= Moment0l;
			ret.Moment0r= Moment0r;
			ret.Moment1l= Moment1l;
			ret.Moment1r= Moment1r;
			ret.Moment2l= Moment2l;
			ret.Moment2r= Moment2r;
			ret.Nv = Nv;						//	血管番号
			ret.Vp = Vp;						//	血管での位置
			ret.Dw = Dw==null? new double[4] :(double[])Dw.Clone();				//	同一血管上の前後のROIでのWdとの差 dw[4]
			ret.Edge_dy = Edge_dy;					//	エッジからの距離（垂直方向）
			ret.Meiko_u= Meiko_u;				//	迷光指標(上)
			ret.Meiko_l= Meiko_l;				//	迷光指標(下)
			ret.Remove_flg_hw_voi= Remove_flg_hw_voi;		//  指標hw_voi指標による除去フラグ
			ret.Remove_flg_h1 = Remove_flg_h1;			//  指標h1 < 0.075による除去フラグ		Dictionary<string, object> NamedObjects;	// 名前付きオブジェクト（拡張用）
			ret.Hgb= Hgb;
			ret.Hgb_raw= Hgb_raw;			// 生のHGB
			ret.Voi= Voi;
			ret.PBM_alpha= PBM_alpha;			// 電流輝度補正係数
			ret.O2M_alpha= O2M_alpha;			// 酸素化補正係数
			ret.NamedObjects= new Dictionary<string,object>(NamedObjects);   // 名前付きオブジェクト（拡張用）
			return ret;
		}
	}




}
