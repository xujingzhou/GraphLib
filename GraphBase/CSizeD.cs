using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel; 

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// 存储有序浮点数对，通常为矩形的宽度和高度。
    /// 编写：徐景周，2006.4.10
	/// </summary>
	/// <remarks>
	/// 注意：由于CSizeD现在是类，不是结构。故在赋值给其它变量时，最好用new，如：CSizeD szHit = new CPointD( size );
	/// 否则，如果只是用：CSizeD szHit = size的话(相当于控制权转移)，在后面改变szHit值时，如：szHit.Width = _ptsTmp[0].X;
	/// size的值也会随之改变，如同也执行了相同操作：size.Width = _ptsTmp[0].X;，下次再读取size值时就会不对。或者可以直接加用
	/// Clone()涵数，可不用每次new的麻烦。jingzhou xu,2006.4.6
	/// </remarks>
	[Serializable]
	[ComVisible(true)]
	public class CSizeD
	{

		#region 字段

		/// <summary>
		/// 宽度
		/// </summary>
		public double Width		= 0;
		/// <summary>
		/// 高度
		/// </summary>
		public double Height	= 0;
		/// <summary>
		/// 表示 CSizeD 类的、成员数据未被初始化的新实例。
		/// </summary>
		public static readonly CSizeD Empty = new CSizeD( 0, 0 );

		#endregion

		#region 构造函数

		/// <summary>
		/// CSizeD构造涵数，无参
		/// </summary>
		public CSizeD()
		{
			Width = Height = 0;		// double.NaN
		}

		/// <summary>
		/// 用指定尺寸初始化 CSizeD 类的新实例。
		/// </summary>
		/// <param name="cx">新 CSizeD 的宽度分量。</param>
		/// <param name="cy">新 CSizeD 的高度分量。</param>
		public CSizeD(double cx,double cy)
		{
			Width = cx;
			Height = cy;
		}

		/// <summary>
		/// 从指定的现有 CSizeD 初始化 CSizeD 类的新实例。
		/// </summary>
		/// <param name="size">从中创建新 CSizeD 的 CSizeD。</param>
		public CSizeD(CSizeD size)
		{
			Width = size.Width;
			Height = size.Height;
		}

		/// <summary>
		/// 从指定的现有 SizeF 初始化 CSizeD 类的新实例。
		/// </summary>
		/// <param name="size">从中创建新 CSizeD 的 SizeF。</param>
		public CSizeD(SizeF size)
		{
			Width = size.Width;
			Height = size.Height;
		}

		/// <summary>
		/// 从指定的 CPointD 初始化 CSizeD 类的新实例。
		/// </summary>
		/// <param name="pt">从中初始化此 CSizeD 的 CPointD。</param>
		public CSizeD( CPointD  pt)
		{
			Width = pt.X;
			Height = pt.Y;
		}

		#endregion

		#region 属性

		/// <summary>
		/// 获取一个值，该值指示此 CSizeD 是否为空。
		/// </summary>
		[BrowsableAttribute(false)]
		public bool IsEmpty
		{
			get
			{
				return ( Width <= 0.0 && Height <= 0.0 );
			}
		}

		/// <summary>
		/// 宽度,属性页中修改用，jingzhou xu,2006.4.14
		/// </summary>
		public double WIDTH
		{
			get
			{
				return Width;
			}
			set
			{
				Width = value;
			}
		}

		/// <summary>
		/// 高度，属性页中修改用，jingzhou xu,2006.4.14
		/// </summary>
		public double HEIGHT
		{
			get 
			{
				return Height;
			}
			set
			{
				Height = value;
			}
		}

		#endregion 

		#region 公共方法

		/// <summary>
		/// 复制自身,克隆，用此方法可以直接象使用结构一样，直接复制不用每次忘记new而产生错误。jingzhou xu，2006.4.6
		/// </summary>
		/// <returns></returns>
		public CSizeD Clone()
		{
			return new CSizeD( Width, Height );
		}

		/// <summary>
		/// 将 CPointD 转换为 PointF
		/// </summary>
		/// <returns></returns>
		public SizeF toSizeF()
		{
			return new SizeF( (float)Width, (float)Height );
		}

		/// <summary>
		/// 比较两个尺寸是否大致相同
		/// </summary>
		/// <param name="sz1"></param>
		/// <param name="sz2"></param>
		/// <returns></returns>
		public static bool IsEquals( CSizeD sz1, CSizeD sz2 )
		{
			return ((Math.Abs(sz1.Width - sz2.Width) < jGMC.EPSLN) && (Math.Abs(sz1.Height - sz2.Height) <  jGMC.EPSLN));
		}

		/// <summary>
		/// 比较两个尺寸是否大致相同
		/// </summary>
		/// <param name="sz"></param>
		/// <returns></returns>
		public bool IsEquals( CSizeD sz )
		{
			return CSizeD.IsEquals( this, sz );
		}

		#endregion

		#region 公共运算符

		/// <summary>
		/// 将一个 CSizeD 类实例的宽度和高度与另一个 CSizeD 类实例的宽度和高度相加。
		/// </summary>
		/// <param name="sz1">要相加的第一个 CSizeD。</param>
		/// <param name="sz2">要相加的第二个 CSizeD。</param>
		/// <returns></returns>
		public static CSizeD operator +( CSizeD sz1, CSizeD sz2 )
		{
			return new CSizeD( sz1.Width + sz2.Width, sz1.Height + sz2.Height );
		}

		/// <summary>
		/// 将一个 CSizeD 类实例的宽度和高度与另一个 CSizeD 类实例的宽度和高度相减。
		/// </summary>
		/// <param name="sz1">要相减的第一个 CSizeD。</param>
		/// <param name="sz2">要相减的第二个 CSizeD。</param>
		/// <returns></returns>
		public static CSizeD operator -( CSizeD sz1, CSizeD sz2 )
		{
			return new CSizeD( sz1.Width - sz2.Width, sz1.Height - sz2.Height );
		}

		/// <summary>
		/// -负数操作符重载
		/// </summary>
		/// <param name="sz"></param>
		/// <returns></returns>
		public static CSizeD operator -( CSizeD sz )
		{
			return new CSizeD( -sz.Width, -sz.Height );
		}

		/// <summary>
		/// 将指定的 CSizeD 转换为 CPointD。显式类型转换
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static explicit operator CPointD( CSizeD size )
		{
			return new CPointD( size.Width, size.Height );
		}

		/// <summary>
		/// 将指定 SizeF 转换为 CSizeD。
		/// </summary>
		/// <param name="size">浮点数尺寸</param>
		/// <returns></returns>
		public static implicit operator CSizeD( SizeF size )
		{
			return new CSizeD( size );
		}

		/// <summary>
		/// 将指定 Size 转换为 CSizeD。
		/// </summary>
		/// <param name="size">整数尺寸点</param>
		/// <returns></returns>
		public static implicit operator CSizeD( Size size )
		{
			return new CSizeD( (double)size.Width, (double)size.Height );
		}

		#endregion 

		#region 重载Object

		/// <summary>
		/// 把向量输出成字符串
		/// </summary>
		/// <returns>字符串</returns>
		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder( "(", 50 );
			sb.Append( Width );
			sb.Append( Height );
			sb.Append( ")" );
			return sb.ToString();;
		}

		#endregion 
	}
}
