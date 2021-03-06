using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel; 

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// 表示在二维平面中定义点的、浮点 x 和 y 坐标的有序对。
    /// 编写：徐景周，2006.4.10
	/// </summary>
	/// <remarks>
	/// 注意：由于CPointD现在是类，不是结构。故在赋值给其它变量时，最好用new，如：CPointD ptHit = new CPointD( rect.TopLeft );
	/// 否则，如果只用：CPointD ptHit = pt的话(相当于控制权转移)，在后面改变ptHit值时，如：ptHit.X = _ptsTmp[0].X;
	/// pt的值也会随之改变，如同也执行了相同操作：pt.X = _ptsTmp[0].X;，下次再读取pt值时就会不对。或者可以直接加用
	/// Clone()涵数，可不用每次new的麻烦。jingzhou xu,2006.4.6
	/// </remarks>
	[Serializable]
	[ComVisible(true)]
	public class CPointD
	{
		#region 字段

		/// <summary>
		/// CPointD点水平方向位置
		/// </summary>
		public double X = 0.0f;
		/// <summary>
		/// CPointD点垂直方向位置
		/// </summary>
		public double Y = 0.0f;
		/// <summary>
		/// 表示 CPointD 类的、成员数据未被初始化的新实例。
		/// </summary>
		public static readonly CPointD Empty = new CPointD( );

		#endregion

		#region 构造函数

		/// <summary>
		/// 		/// CPointD构造涵数，无参
		/// </summary>
		public CPointD()
		{
			X = Y = 0; // double.NaN;
		}

		/// <summary>
		/// 从指定的现有 CPointD 初始化 CPointD 类的新实例。
		/// </summary>
		/// <param name="pt">从中创建新 CPointD 的 CPointD。</param>
		public CPointD( CPointD  pt)
		{
			X = pt.X;
			Y = pt.Y;
		}

		/// <summary>
		/// 从指定的现有 PointF 初始化 CPointD 类的新实例。
		/// </summary>
		/// <param name="pt">从中创建新 CPointD 的 PointF。</param>
		public CPointD( PointF  pt)
		{
			X = pt.X;
			Y = pt.Y;
		}

		/// <summary>
		/// 从指定的 SizeD 初始化 CPointD 类的新实例。
		/// </summary>
		/// <param name="size">从中初始化此 CPointD 的 SizeD。</param>
		public CPointD( CSizeD size)
		{
			X = size.Width;
			Y = size.Height;
		}

		/// <summary>
		/// 用指定坐标初始化 CPointD 类的新实例。
		/// </summary>
		/// <param name="x">该点的水平位置。</param>
		/// <param name="y">该点的垂直位置。</param>
		public CPointD(double x,double y)
		{
			X = x;
			Y = y;
		}

		#endregion

		#region 属性

		/// <summary>
		/// 获取一个值，该值指示此 CPointD 是否为空。
		/// </summary>
		[BrowsableAttribute(false)]
		public bool IsEmpty
		{
			get
			{
				return ( X == 0.0 && Y == 0.0 );
			}
		}

		#endregion 

		#region 公共方法

		/// <summary>
		/// 复制自身,克隆，用此方法可以直接象使用结构一样，直接复制不用每次忘记new而产生错误。jingzhou xu，2006.4.6
		/// </summary>
		/// <returns></returns>
		public CPointD Clone()
		{
			return new CPointD( X, Y );
		}

		/// <summary>
		/// 为CPointD赋新值
		/// </summary>
		/// <param name="x">点水平位置</param>
		/// <param name="y">点垂直位置</param>
		public void Set( double x, double y )
		{
			X = x; 
			Y = y;
		}

		/// <summary>
		/// 将 CPointD 转换为 PointF
		/// </summary>
		/// <returns>转换后新值。</returns>
		public PointF toPointF()
		{
			return  new PointF((float)X,(float)Y);
		}

		/// <summary>
		/// 将 CPointD 转换为 Point
		/// </summary>
		/// <returns>转换后新值。</returns>
		public Point toPoint()
		{
			return  new Point((int)(X),(int)(Y));
		}

		/// <summary>
		/// 将CPointD偏移到指定位置。
		/// </summary>
		/// <param name="x">点水平位置</param>
		/// <param name="y">点垂直位置</param>
		public void Offset(double x, double y)
		{
			X += x;
			Y += y;
		}

		/// <summary>
		/// 将CPointD偏移到指定位置。
		/// </summary>
		/// <param name="size">点偏移到指定大小的位置</param>
		public void Offset( CSizeD  size)
		{
			Offset( size.Width, size.Height );
		}

		/// <summary>
		/// 比较两个点是否大致重合
		/// </summary>
		/// <param name="pt1">第一个点</param>
		/// <param name="pt2">第二个点</param>
		/// <returns></returns>
		public static bool IsEquals( CPointD pt1, CPointD pt2 )
		{
			return ((Math.Abs(pt1.X - pt2.X) < jGMC.EPSLN) && (Math.Abs(pt1.Y - pt2.Y) <  jGMC.EPSLN));
		}

		/// <summary>
		/// 比较给定的 CPointD 对象是否和此 CPointD 对象大致重合
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public bool IsEquals( CPointD pt )
		{
			return CPointD.IsEquals( this, pt );
		}

		#endregion

		#region 公共运算符

		/// <summary>
		/// 将 CPointD 平移给定 SizeD
		/// </summary>
		/// <param name="p">平移点</param>
		/// <param name="s">要平移的大小</param>
		/// <returns>平移后新点</returns>
		public static CPointD operator + ( CPointD p, CSizeD s )
		{
			return new CPointD( p.X+s.Width, p.Y+s.Height);
		}

		/// <summary>
		/// 将 CPointD 平移给定点
		/// </summary>
		/// <param name="pt1">平移点</param>
		/// <param name="pt2">平移到的点位置</param>
		/// <returns>平移后新点</returns>
		public static CPointD operator + ( CPointD pt1, CPointD pt2)
		{
			return new CPointD( pt1.X + pt2.X ,pt1.Y + pt2.Y );
		}

		/// <summary>
		/// 将 CPointD 平移给定 SizeD 的负数。
		/// </summary>
		/// <param name="p">平移点</param>
		/// <param name="s">要平移到的大小</param>
		/// <returns>平移后新点</returns>
		public static CPointD operator - ( CPointD p, CSizeD s)
		{
			return new CPointD( p.X - s.Width, p.Y - s.Height );
		}

		/// <summary>
		/// 将 CPointD 平移给定 CPointD 的负数。
		/// </summary>
		/// <param name="pt1">平移点</param>
		/// <param name="pt2">要平移到的点</param>
		/// <returns>平移后新点</returns>
		public static CPointD operator - ( CPointD pt1, CPointD pt2)
		{
			return new CPointD( pt1.X - pt2.X ,pt1.Y - pt2.Y );
		}

		// --------------------------------------------------------------------------------------
		/// <summary>
		/// 向量乘以标量：向量 X 标量
		/// </summary>
		/// <param name="pt">向量</param>
		/// <param name="scale">标量</param>
		/// <returns>向量</returns>
		public static CPointD operator * ( CPointD pt , double  scale)
		{
			return new CPointD(pt.X*scale, pt.Y*scale);	
		}

		/// <summary>
		/// 标量乘以向量：标量 X  向量
		/// </summary>
		/// <param name="scale">标量</param>
		/// <param name="pt">向量</param>
		/// <returns>向量</returns>
		public static CPointD operator * ( double  scale, CPointD pt )
		{
			return pt*scale;
		}

		/// <summary>
		/// 向量除以标量：向量 / 标量
		/// </summary>
		/// <param name="pt">向量</param>
		/// <param name="scale">标量</param>
		/// <returns>向量</returns>
		public static CPointD operator / ( CPointD pt , double  scale)
		{
			if(scale != 0)
				return new CPointD(pt.X/scale, pt.Y/scale);
			else
				return CPointD.Empty;
		}
		// --------------------------------------------------------------------------------------

		/// <summary>
		///  负数(-)操作符重载
		/// </summary>
		/// <param name="p">点</param>
		/// <returns>新点</returns>
		public static CPointD operator - (CPointD p)
		{
			return new CPointD( -p.X, -p.Y );
		}

		/// <summary>
		/// 将指定 PointF 转换为 CPointD。
		/// </summary>
		/// <param name="pt">浮点数坐标点</param>
		/// <returns></returns>
		public static implicit operator CPointD( PointF pt )
		{
			return new CPointD(pt);
		}

		/// <summary>
		/// 将指定 Point 转换为 CPointD。
		/// </summary>
		/// <param name="pt">整数坐标点</param>
		/// <returns></returns>
		public static implicit operator CPointD( Point pt )
		{
			return new CPointD((double)pt.X, (double)pt.Y);
		}

		#endregion 

		#region 重载基类

		/// <summary>
		/// 把向量输出成字符串
		/// </summary>
		/// <returns>字符串</returns>
		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder( "(", 50 );
			sb.Append( X );
			sb.Append( "," );	// 增加个逗号	ahr	2006-04-11
			sb.Append( Y );
			sb.Append( ")" );
			return sb.ToString();;
		}

		#endregion 
	}
}
