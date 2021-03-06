//----------------------------------------------------------
//	集合常数类
//	定义了一些几何元算中常用的常数：最小误差、圆周率、角度与弧度的转换
//-----------------------------------------------------------
using System;
namespace Jurassic.Graph.Base
{
	/// <summary>
	/// 定义一些几何常量。
	/// </summary>
	public sealed class jGMC
	{
		/// <summary>
		/// 小的浮点数，用于计算浮点数误差
		/// </summary>
		public const double EPSLN = 1.0e-10;								// 小的浮点数

		#region 圆周率相关

		/// <summary>
		/// 圆周率
		/// </summary>
		public const double PI		= 3.1415926535897932384626433832795;	// PI
		/// <summary>
		/// 四分之一圆周率
		/// </summary>
		public const double FORTPI	= 3.1415926535897932384626433832795;	// PI/4
		/// <summary>
		/// 二分之一圆周率
		/// </summary>
		public const double HALFPI	= 1.5707963267948966192313216916398;	// PI/2
		/// <summary>
		/// 两倍的圆周率
		/// </summary>
		public const double TWOPI	= 6.283185307179586476925286766559;		// PI*2
		/// <summary>
		/// 度到弧度的转换因子
		/// </summary>
		public const double D2R		= 0.017453292519943295769236907684886;	//// 度到弧度
		/// <summary>
		/// 秒到弧度的转换因子
		/// </summary>
		public const double S2R		= 4.8481368110953599358991410235795e-6;	//// 秒到弧度
		/// <summary>
		/// 弧度到度的转换因子
		/// </summary>
		public const double R2D		= 57.295779513082320876798154814105;	//// 弧度到度
		/// <summary>
		/// 弧度到秒的转换因子
		/// </summary>
		public const double R2S		= 206264.80624709635515647335733078;	//// 弧度到秒

		#endregion 	结束_圆周率相关


	}
}
