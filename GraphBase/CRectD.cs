using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel; 

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// 存储一组浮点数，共四个，表示一个矩形的位置和大小。
    /// 编写：徐景周，2006.4.10
	/// </summary>
	/// <remarks>
	/// 注意：由于CRectD现在是类，不是结构。故在赋值给其它变量时，最好用new，如：CRectD rctBnd = new CRectD( Bounds );
	/// 否则，如果只用：CRectD rctBnd = Bounds的话(相当于控制权转移)，在后面改变rctBnd值时，如：rctBnd.Offset( -new CPointD(100, 100) );
	/// Bounds的值也会随之改变，如同也执行了相同操作：Bounds.Offset( -new CPointD(100, 100) )，下次再读取Bounds值时就会不对。或者可以直接加用
	/// Clone()涵数，可不用每次new的麻烦。jingzhou xu,2006.4.6
	/// </remarks>
	[Serializable]
	[ComVisible(true)]
	public class CRectD
	{
		#region 字段

		/// <summary>
		/// 矩形左上角的 x 坐标。
		/// </summary>
		public double X			= 0.0;	
		/// <summary>
		/// 矩形左上角的 y 坐标。
		/// </summary>
		public double Y			= 0.0;		
		/// <summary>
		/// 矩形的宽度。
		/// </summary>
		public double Width		= 0.0;	
		/// <summary>
		/// 矩形的高度。
		/// </summary>
		public double Height	= 0.0;		

		/// <summary>
		/// 表示 CSizeD 类的、成员数据未被初始化的新实例。
		/// </summary>
		public static CRectD Empty 
		{
			get
			{
				return  new CRectD( );
			}
		}

		#endregion

		#region 构造函数

		/// <summary>
		/// CRectD构造涵数
		/// </summary>
		public CRectD()
		{
			X = Y = Width = Height = 0.0;
		}

		/// <summary>
		/// 用指定的位置和大小初始化 CRectD 类的新实例。
		/// </summary>
		/// <param name="location">它表示矩形区域的左上角。</param>
		/// <param name="size">它表示矩形区域的宽度和高度。</param>
		public CRectD( CPointD location, CSizeD size )
		{
			X		= location.X;
			Y		= location.Y;
			Width	= size.Width;
			Height	= size.Height;
		}

		/// <summary>
		/// 用指定的位置和大小初始化 CRectD 类的新实例。
		/// </summary>
		/// <param name="x">矩形左上角的 x 坐标。</param>
		/// <param name="y">矩形左上角的 y 坐标。</param>
		/// <param name="width">矩形的宽度。</param>
		/// <param name="height">矩形的高度。</param>
		public CRectD( double x, double y, double width, double height )
		{
			X		= x;
			Y		= y;
			Width	= width;
			Height	= height;
		}

		/// <summary>
		/// 从指定的现有 CRectD 初始化 CRectD 类的新实例。
		/// </summary>
		/// <param name="rect">从中创建新 CRectD 的 CRectD。</param>
		public CRectD( CRectD rect )
		{
			X		= rect.X;
			Y		= rect.Y;
			Width	= rect.Width;
			Height	= rect.Height;
		}

		/// <summary>
		/// 从指定的现有 RectangleF 初始化 CRectD 类的新实例。
		/// </summary>
		/// <param name="rect">从中创建新 CRectD 的 RectangleF。</param>
		public CRectD( RectangleF rect )
		{
			X		= rect.Left;
			Y		= rect.Top;
			Width	= rect.Width;
			Height	= rect.Height;
		}

		#endregion
		
		#region 属性

		/// <summary>
		/// 测试此 CRectD 的所有数值属性是否都具有零值。
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
		/// 获取或设置此 CRectD 类左上角的坐标。
		/// </summary>
		public CPointD Location
		{
			get
			{
				return new CPointD( X, Y );
			}
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		/// <summary>
		/// 获取或设置此 CRectD 的大小。
		/// </summary>
		public CSizeD Size
		{
			get
			{
				return new CSizeD( Width, Height );
			}
			set
			{
				Width	= value.Width;
				Height	= value.Height;
			}
		}

		/// <summary>
		/// 获取此 CRectD 左边缘的 x 坐标。
		/// </summary>
		public double Left
		{
			get
			{
				return X;
			}
			set
			{
				X = value;
			}
		}

		/// <summary>
		/// 获取此 CRectD 上边缘的 y 坐标。
		/// </summary>
		public double Top
		{
			get
			{
				return Y;
			}
			set
			{
				Y = value;
			}
		}

		/// <summary>
		/// 获取此 CRect 右边缘的 x 坐标。
		/// </summary>
		public double Right
		{
			get
			{
				return X + Width;
			}
		}

		/// <summary>
		/// 获取此 CRectD 下边缘的 y 坐标。
		/// </summary>
		public double Bottom
		{
			get
			{
				return Y + Height;
			}
		}

		/// <summary>
		/// 获取此 CRectD 左上角
		/// </summary>
		/// <returns></returns>
		public CPointD TopLeft
		{
			get
			{
				return Location;
			}
		}

		/// <summary>
		/// 右上角
		/// </summary>
		public CPointD TopRight
		{
			get
			{
				return new CPointD( Right, Top  );
			}
		}

		/// <summary>
		/// 获取此 CRectD 右下角
		/// </summary>
		public CPointD BottomRight
		{
			get
			{
				return new CPointD( Right, Bottom );
			}
		}

		/// <summary>
		/// 左下角
		/// </summary>
		public CPointD BottomLeft
		{
			get
			{
				return new CPointD( Left, Bottom );
			}
		}

		/// <summary>
		/// 中心点
		/// </summary>
		public CPointD CenterPoint
		{
			get
			{
				return new CPointD( ( Left + Right) / 2.0, (Top + Bottom) / 2.0 );
			}
		}

		#endregion 

		#region 公共方法

		/// <summary>
		/// 复制自身,克隆，用此方法可以直接象使用结构一样，直接复制不用每次忘记new而产生错误。jingzhou xu,2006.4.6
		/// </summary>
		/// <returns></returns>
		public CRectD Clone()
		{
			return new CRectD( X, Y, Width, Height );
		}

		/// <summary>
		/// 将 CRectD 转换为 RectangleF
		/// </summary>
		/// <returns></returns>
		public RectangleF toRectangleF()
		{
			return new RectangleF( (float)X, (float)Y, (float)Width, (float)Height );
		}

		/// <summary>
		/// 将 CRectD 转换为 Rectangle
		/// </summary>
		/// <returns></returns>
		public Rectangle toRectangle()
		{
			return new Rectangle( (int)(X>0?X+0.5:X-0.5), (int)(Y>0?Y+0.5:Y-0.5), (int)(Width+0.5), (int)(Height+0.5) );
		}

		/// <summary>
		/// 比较两个矩形是否大致相同
		/// </summary>
		/// <param name="rect1"></param>
		/// <param name="rect2"></param>
		/// <returns></returns>
		public static bool IsEquals( CRectD rect1, CRectD rect2 )
		{
			return ( (Math.Abs( rect1.X - rect2.X) < jGMC.EPSLN) && 
				(Math.Abs( rect1.Y - rect2.Y) < jGMC.EPSLN) && 
				(Math.Abs( rect1.Width - rect2.Width) < jGMC.EPSLN) && 
				(Math.Abs( rect1.Height - rect2.Height) < jGMC.EPSLN) );
		}

		/// <summary>
		/// 比较两个矩形是否大致相同
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public bool IsEquals( CRectD rect )
		{
			return CRectD.IsEquals( this, rect );
		}

		/// <summary>
		/// 确定指定的点是否包含在此 CRectD 内。
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Contains( double x, double y )
		{
			return	( x >= X ) && 
				( x <= Right ) &&
				( y >= Y ) &&
				( y <= Bottom);  
		}

		/// <summary>
		/// 确定指定的点是否包含在此 CRectD 内。
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public bool Contains( CPointD pt )
		{
			return Contains( pt.X, pt.Y );
		}

		/// <summary>
		/// 确定 rect 表示的矩形区域是否完全包含在此 CRect 内。
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public bool Contains( CRectD rect )
		{
			return Contains( rect.TopLeft ) && Contains( rect.BottomRight );
		}

		/// <summary>
		/// 将指定矩形赋值到当前矩形,jingzhou xu,2006.4.10
		/// </summary>
		/// <param name="srcRect">源矩形</param>
		public void CopyRect( CRectD srcRect )
		{
			X		= srcRect.X;
			Y		= srcRect.Y;
			Width	= srcRect.Width;
			Height	= srcRect.Height;
		}

		/// <summary>
		/// 创建一个 CRectD ，它的左上角和右下角都位于指定位置。
		/// </summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public static CRectD FromLTRB( double left, double top, double right, double bottom )
		{
			return new CRectD( Math.Min(left,right), Math.Min(top,bottom), 
				Math.Abs(right - left), Math.Abs(bottom - top) );
		}

		/// <summary>
		/// 创建并返回指定 CRectD 的放大副本。该副本被放大指定的量。不修改原始矩形。
		/// </summary>
		/// <param name="rect">要复制的 CRectD。不修改此矩形。</param>
		/// <param name="x">矩形副本的水平放大量。</param>
		/// <param name="y">矩形副本的垂直放大量。</param>
		/// <returns></returns>
		public static CRectD Inflate( CRectD rect, double x, double y )
		{
			return new CRectD( rect.Left - x, rect.Top - y, rect.Width + x*2.0, rect.Height + y*2.0 );
		}

		/// <summary>
		/// 将此 CRectD 放大指定量。
		/// </summary>
		/// <param name="x">水平放大量</param>
		/// <param name="y">垂直放大量</param>
		public void Inflate( double x, double y )
		{
			X		-= x;
			Y		-= y;
			Width	+= x*2.0;
			Height	+= y*2.0;
		}

		/// <summary>
		/// 将此 CRectD 放大指定量。
		/// </summary>
		/// <param name="size">此矩形的放大量。</param>
		public void Inflate( CSizeD size )
		{
			Inflate( size.Width, size.Height );
		}

		/// <summary>
		/// 两矩形交集
		/// </summary>
		/// <param name="a">矩形一</param>
		/// <param name="b">矩形二</param>
		/// <returns>交集</returns>
		public static CRectD Intersect( CRectD a, CRectD b )
		{
			CRectD rect = new CRectD();
			rect.X = a.X > b.X? a.X:b.X;
			rect.Y  = a.Y > b.Y? a.Y:b.Y;
			rect.Width = (a.Right < b.Right? a.Right:b.Right) - rect.X;
			if ( rect.Width < 0.0 )
			{
				rect.Width = 0.0;
				rect.Width = 0.0;
				return CRectD.Empty;
			}
			rect.Height = (a.Bottom < b.Bottom? a.Bottom:b.Bottom) - rect.Y;
			if ( rect.Height < 0.0 )
			{
				rect.Width = 0.0;
				rect.Width = 0.0;
				return CRectD.Empty;
			}
			return rect;
		}

		/// <summary>
		/// 将此 CRectD 替换为其自身与指定的 CRectD 的交集。
		/// </summary>
		/// <param name="rect"></param>
		public void Intersect( CRectD rect )
		{
			double r = Right;
			double b = Bottom;
			X = X > rect.X? X:rect.X;
			Y  = Y > rect.Y? Y:rect.Y;
			Width = ( r<rect.Right? r:rect.Right ) - X;
			if ( Width < 0.0 )
			{
				Width = 0.0;
				Height = 0.0;
				return;
			}
			Height = ( b<rect.Bottom? b:rect.Bottom ) - Y;
			if ( Height < 0.0 )
			{
				Width = 0.0;
				Height = 0.0;
				return;
			}
		}

		/// <summary>
		/// 确定此矩形是否与 rect 相交。jingzhou xu,2006.4.10
		/// </summary>
		/// <param name="rect">要测试的矩形。</param>
		/// <returns></returns>
		public bool IntersectsWith( CRectD rect )
		{
			// 指定矩形是否和当前矩形相交,jingzhou xu
			return jGMA.IsRectCrossRect( new CRectD( X, Y, Width, Height ), rect );

/*			double l = X > rect.X? X:rect.X;
			double t = Y > rect.Y? Y:rect.Y;
			double w = ( Right < rect.Right? Right:rect.Right ) - l;
			double h = ( Bottom < rect.Bottom? Bottom:rect.Bottom ) - t;

			return ( w<0.0 || h<0.0 );
*/
		}

		/// <summary>
		/// 确定点是否在矩形中
		/// </summary>
		/// <param name="pnt">要测试的点</param>
		/// <returns></returns>
		public bool IntersectsWith( CPointD pnt )
		{
			if( ( pnt.X >= X ) && 
				( pnt.X <= Right ) &&
				( pnt.Y >= Y )&&
				( pnt.Y <= Bottom ) ) 
			{
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// 矩形正规化,jingzhou xu
		/// </summary>
		public void NormalizeRect()
		{
			if( Left > Right )
			{
				Left	= Right;
				Width	= Math.Abs( Width );
			}

			if( Top > Bottom )
			{
				Top		= Bottom;
				Height	= Math.Abs( Height );
			}
		}

		/// <summary>
		/// 将此矩形的位置调整指定的量。
		/// </summary>
		/// <param name="x">水平偏移该位置的量。</param>
		/// <param name="y">垂直偏移该位置的量。</param>
		public void Offset( double x, double y )
		{
			X += x;
			Y += y;
		}

		/// <summary>
		/// 将此矩形的位置调整 CSizeD 指定的量。
		/// </summary>
		/// <param name="size">偏移该位置的量</param>
		public void Offset( CSizeD size )
		{
			Offset( size.Width, size.Height );
		}

		/// <summary>
		/// 将此矩形的位置调整指定的量。
		/// </summary>
		/// <param name="pos">偏移该位置的量。</param>
		public void Offset( CPointD pos )
		{
			Offset( pos.X, pos.Y );
		}

		/// <summary>
		/// 创建第三个矩形，它是能够同时包含形成并集的两个矩形的可能的最小矩形。
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static CRectD Union( CRectD a, CRectD b )
		{
			CRectD rect = a.Clone();
			rect.Union(b);
			return rect;
		}

		/// <summary>
		/// 将此 CRectD 替换为其自身与指定的 CRectD 的并集。
		/// </summary>
		/// <param name="rect"></param>
		public void Union( CRectD rect )
		{
			double r = Right;
			double b = Bottom;
			X = X < rect.X? X:rect.X;
			Y = Y < rect.Y? Y:rect.Y;					

			double right = Math.Max(r,rect.X+rect.Width);
			double bottom = Math.Max(b,rect.Y+rect.Height);
			this.Width = right -X;
			this.Height = bottom - Y;
		}


		Matrix m_AngleMatrix = new Matrix();
		/// <summary>
		/// 获取矩形(如图*号构成的矩形)以某点为基准点(如图#)旋转以后，
		/// 四角构成的包容矩形(如图的实线矩形) 张潋 20050511
		/// 
		///	   -------*---------------
		///	   |    *	*            |
		///	   |  *	      *          |
		///	   |*	        *        |
		///	   |  *           *      |
		///	   |    *     #     *    |
		///	   |      *           *  |
		///	   |        *           *|
		///	   |          *       *  |
		///	   |            *   *    |
		///	   |______________*______|
		/// </summary>
		/// <param name="Angle">旋转角度</param>
		/// <param name="AnglePoint">旋转的基准点</param>
		/// <returns></returns>
		public CRectD GetAngleRect(float Angle,PointF AnglePoint)
		{
			CRectD result = this.Clone();

			m_AngleMatrix.RotateAt(Angle,AnglePoint);
			
			PointF[] pnts = {
								new PointF((float)result.TopLeft.X,(float)result.TopLeft.Y),
								new PointF((float)result.TopRight.X,(float)result.TopRight.Y),
								new PointF((float)result.BottomLeft.X,(float)result.BottomLeft.Y),
								new PointF((float)result.BottomRight.X,(float)result.BottomRight.Y)
							};
			m_AngleMatrix.TransformPoints(pnts);
			double rx = Math.Min(Math.Min(Math.Min(pnts[0].X,pnts[1].X),pnts[2].X),pnts[3].X);
			double ry = Math.Min(Math.Min(Math.Min(pnts[0].Y,pnts[1].Y),pnts[2].Y),pnts[3].Y);
			double rw = Math.Max(Math.Max(Math.Max(pnts[0].X,pnts[1].X),pnts[2].X),pnts[3].X) - rx;
			double rh = Math.Max(Math.Max(Math.Max(pnts[0].Y,pnts[1].Y),pnts[2].Y),pnts[3].Y) - ry;
			result = new CRectD(rx,ry,rw,rh);
			result.NormalizeRect();

			return result;
		}

		
		/// <summary>
		/// 获取矩形(如图*号构成的矩形)以某点为基准点(如图#)旋转以后，
		/// 四角构成的包容矩形(如图的实线矩形)
		/// </summary>
		/// <param name="angle">旋转角度</param>
		/// <returns>旋转以后的包容矩形</returns>
		/// <remarks>从矩形中心旋转</remarks>
		public CRectD GetAngleRect(double angle)
		{
			return GetAngleRect((float)angle, this.CenterPoint.toPointF());
		}


		#endregion

		#region 公共运算符

		/// <summary>
		/// 将指定 RectangleF 转换为 CRectD。
		/// </summary>
		/// <param name="rect">RectangleF矩形</param>
		/// <returns>CRectD矩形</returns>
		public static implicit operator CRectD( RectangleF rect )
		{
			return new CRectD( rect );
		}

		#endregion 

	}
}
