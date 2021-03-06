using System;
using System.Drawing;
using System.Collections;

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// 几何算法类,实现常用的几何运算。
	/// </summary>
	/// <remarks>
	/// <code>
	/// 1、涉及到的形状：pnt点、Sect线段、Line直线、Polyline折线、Polygon多边形、Rect矩形、Circle圆。
	///	2、在判断类方法中，On在...上；In在...内；Cross相交
	/// 3、所有IsXXXInYYY表示形状XXX完全在形状YYY内。
	/// 4、所有IsXXXCrossYYY表示形状XXX完全在形状YYY内，或相交。
	/// 5、所有的方法都不进行包容矩形的初步判断，如果需要，在调用方法前自行进行包容矩形判断。if( rcBoundA.IntersectsWith( rcBoundB ) == true ) Method(...);
	/// 6、如果方法中带有包容矩形参数，如果是null，方法内部计算矩形，可能进行包容矩形初步判断；
	///		如果是错误的矩形，会导致结果不确定
	///	7、用IList代表点数组（Polyline、Polygon），使用者自己维护点数组含义的正确性，集合内必须是CPointD、Polyline的Count大于1、Polygon的Count大于2。
	///	8、如果不正确地使用了IList，可能会导致异常（InvalidCastException、IndexOutOfRangeException、NullReferenceException）
	///	</code>
	/// </remarks>
	//  编写：徐景周，2005.12.20
	public class jGMA
	{
		#region 求距离 Dist_
		/// <summary>
		/// 计算两点(x1,y1),(x2,y2)之间的距离
		/// </summary>
		/// <param name="x1">点一水平位置</param>
		/// <param name="y1">点一垂直位置</param>
		/// <param name="x2">点二水平位置</param>
		/// <param name="y2">点二垂直位置</param>
		/// <returns>距离</returns>
		public static double Distance(double x1,double y1,double x2,double y2)
		{
			return Math.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1));
		}

		/// <summary>
		/// 计算两点pt1,pt2之间的距离
		/// </summary>
		/// <param name="pt1">点一</param>
		/// <param name="pt2">点二</param>
		/// <returns>距离</returns>
		public static double Distance( CPointD pt1, CPointD pt2)
		{
			return Distance( pt1.X, pt1.Y, pt2.X, pt2.Y );
		}

		/// <summary>
		/// 计算两点pt1,pt2之间的距离平方。有时判断长短，不用开方即可。
		/// </summary>
		/// <param name="pt1">点一</param>
		/// <param name="pt2">点二</param>
		/// <returns>计算两点pt1,pt2之间的距离平方</returns>
		public static double Distance2(CPointD pt1, CPointD pt2)
		{
			return (pt1.Y - pt2.Y) * (pt1.Y - pt2.Y) + (pt1.X - pt2.X) *(pt1.X - pt2.X);
		}

		/// <summary>
		///  点与直线的距离
		/// </summary>
		/// <param name="ptCurrent">待计算的目标点</param>
		/// <param name="ptStart">直线起点</param>
		/// <param name="ptEnd">直线终点</param>
		/// <returns>点与直线的距离</returns>
		public static double Dist_Pnt2Line( CPointD ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			//	直线方程
			double	A, B, C;
			LineFomuleFrom2Pnt( ptStart, ptEnd, out A, out B, out C );
			//	点到直线距离 |Ax0 + By0 + C|/Sqrt(A*A + B*B)
			double	dist = Math.Sqrt(A*A + B*B);
			if ( DoubleEquals( dist, 0.0 ) )			// 两点重合
				return Distance( ptCurrent, ptStart );

			return Math.Abs( A * ptCurrent.X + B * ptCurrent.Y + C ) / dist;
		}

		/// <summary>
		/// 求点到线段的最近距离
		/// </summary>
		/// <param name="ptCurrent">待计算的目标点</param>
		/// <param name="ptStart">直线起点</param>
		/// <param name="ptEnd">直线终点</param>
		/// <returns>点到线段的最近距离</returns>
		/// <remarks>如果点到直线的垂点不在线段内，取到两个端点的最短距离</remarks>
		public static double Dist_Pnt2Sect( CPointD ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			double dist = Dist_Pnt2Line( ptCurrent, ptStart, ptEnd );
			double distEnd = Distance( ptCurrent, ptStart );
			dist = Math.Min( distEnd, dist );
			distEnd = Distance( ptCurrent, ptEnd );

			return Math.Min( distEnd, dist );
		}

		/// <summary>
		/// 求点与折线的最近距离
		/// </summary>
		/// <param name="ptCurrent">待计算的目标点</param>
		/// <param name="Polyline">折线有序点数组</param>
		/// <returns>点与折线的最近距离</returns>
		public static double Dist_Pnt2Polyline( CPointD ptCurrent, IList Polyline )
		{
			double dist = 0.0;
			for( int nCount	= 0; nCount < Polyline.Count; ++nCount )
			{
				if ( nCount == 0 ) 
					dist = Distance2( ptCurrent, (CPointD)Polyline[0] );
				else
					dist = Math.Min( dist, Distance2( (CPointD)Polyline[nCount-1] , (CPointD)Polyline[nCount] ) );
			}

			return Math.Sqrt(dist);
		}

		/// <summary>
		/// 求点与折线的最近距离
		/// </summary>
		/// <param name="ptCurrent">待计算的目标点</param>
		/// <param name="Polygon">折线、矩形、多边形有序点数组</param>
		/// <param name="rcBound">Polygon的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <returns>点与折线、曲线、多边形的最近距离</returns>
		public static double Dist_Pnt2Polygon( CPointD ptCurrent, IList Polygon, CRectD rcBound )
		{
			if ( rcBound == null ) 
				rcBound = BoundRect( Polygon );

			if ( rcBound.Contains( rcBound ) )
				if ( IsPntInPolygon( ptCurrent, Polygon, rcBound ) ) 
					return 0.0;

			return Dist_Pnt2Polyline( ptCurrent, Polygon );
		}

		#endregion

		#region 求角度	Angle_
		/// <summary>
		/// 计算直线pt1pt2的方向倾角
		/// </summary>
		/// <param name="pt1">起始点坐标</param>
		/// <param name="pt2">结束点坐标</param>
		/// <returns>
		/// <code>
		/// 以弧度计量的角度 θ，它满足 (-π, π)，而且 tan(θ) = y/ x，其中 (x, y) 是笛卡尔平面上的点。请看下面： 
		///	如果 (x, y) 在第一象限，则 (0, π/2)。 
		///	如果 (x, y) 在第二象限，则 (π/2, π)。 
		///	如果 (x, y) 在第三象限，则 (-π,  -π/2)。 
		///	如果 (x, y) 在第四象限，则 (-π/2 0)。
		///	</code>
		///	</returns>
		public static double Angle_Line(CPointD pt1,CPointD pt2)
		{
			return Math.Atan2(pt2.Y - pt1.Y,pt2.X - pt1.X);
		}

		/// <summary>
		/// 计算直线ln1和直线ln2的夹角
		/// </summary>
		/// <param name="ln1Start">直线1的起始点坐标</param>
		/// <param name="ln1End">直线1的结束点坐标</param>
		/// <param name="ln2Start">直线2的起始点坐标</param>
		/// <param name="ln2End">直线2的结束点坐标</param>
		/// <returns>以弧度计量的角度 θ，它满足 (-π, π)，
		///	如果 L1到L2为逆时针，则 (0, π)。 
		///	如果 L1到L2为顺时针，则 (-π, 0)。
		///	</returns> 
		public static double Angle_2Line( CPointD ln1Start, CPointD ln1End, CPointD ln2Start, CPointD ln2End )
		{
			double	A1, B1, C1;
			LineFomuleFrom2Pnt( ln1Start, ln1End, out A1, out B1, out C1 );
			double	A2, B2, C2;
			LineFomuleFrom2Pnt( ln2Start, ln2End, out A2, out B2, out C2 );

			return Math.Atan2( A1*B2-A2*B1, A1*A2+B1*B2 );
		}

		/// <summary>
		/// 计算矢量线段pt1pt2的方向倾角
		/// </summary>
		/// <param name="pt1">起始点坐标</param>
		/// <param name="pt2">结束点坐标</param>
		/// <returns>矢量线段pt1pt2与水平方向的夹角，单位度（0,360）</returns>
		public static double SectDirection(CPointD pt1,CPointD pt2)
		{
			double dblAngle = Math.Atan2(pt2.Y - pt1.Y,pt2.X - pt1.X);
			while( dblAngle < 0 )
				dblAngle += 2 * jGMC.PI;

			return 	dblAngle*jGMC.R2D;
		}

		/// <summary>
		/// 计算线段pt1pt2与线段pt2pt3的切线方向
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <param name="pt3"></param>
		/// <returns>切线与水平方向夹角,单位度</returns>
		public static double TanDirection(CPointD pt1,CPointD pt2,CPointD pt3)
		{
			double dblAngle1= SectDirection(pt1,pt2);
			double dblAngle2= SectDirection(pt3,pt2);

			double dblRet = SectDirection(pt2,pt3)-(dblAngle2  - dblAngle1) / 2  + 90 ;
			while(dblRet < 0)
				dblRet +=360;
			while(dblRet > 360)
				dblRet -=360;

			return 	dblRet;
		}
		
		#endregion

		#region 求方位	Side_

		/// <summary>
		/// 折线段的拐向判断
		/// </summary>
		/// <param name="ptStart">折线段的第一个端点</param>
		/// <param name="ptCorner">折线段的拐点</param>
		/// <param name="ptEnd">折线段的第二个端点</param>
		/// <returns>返回值：-1--拐向在左侧；0--没有拐向，三点共线；1--拐向在右侧。</returns>
		public static int Side_Inflexion( CPointD ptStart, CPointD ptCorner, CPointD ptEnd )
		{
			// 若(p2 - p0) × (p1 - p0) > 0,则p0p1在p1点拐向右侧后得到p1p2。
			// 若(p2 - p0) × (p1 - p0) < 0,则p0p1在p1点拐向左侧后得到p1p2。
			// 若(p2 - p0) × (p1 - p0) = 0,则p0、p1、p2三点共线。	
			double CrossData = CrossProduce( (ptEnd - ptStart), (ptCorner - ptStart) );
			if ( DoubleEquals( CrossData, 0.0 ) ) return 0;	//	没有拐向，三点共线
			if( CrossData > 0 )
				return 1;	//	拐向在右侧
			else
				return -1;	//	拐向在左侧
		}

		/// <summary>
		/// 判断点在直线的哪一边
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="ptStart">直线起始点</param>
		/// <param name="ptEnd">直线终止点</param>
		/// <returns>返回值： -1--左侧；0--在线上；1--右侧。</returns>
		public static int Side_PntOnLine( CPointD  ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			return Side_Inflexion( ptStart, ptCurrent, ptEnd );
		}

		/// <summary>
		/// 判断点在区线的哪一边。先找出点到曲线数组中那一线段距离最近，再利用Side_Inflexion()判断点在线段那一边
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="Polyline">曲线有序点数组</param>
		/// <returns>返回值： -1--左侧；0--在线上；1--右侧。</returns>
		public static int Side_PntOnPolyline( CPointD  ptCurrent, IList Polyline )
		{
			// 寻找最近距离的线段的两端点
			CPointD ptStartNear = (CPointD)Polyline[0];
			CPointD ptEndNear	= (CPointD)Polyline[1];
			double dist0 = Dist_Pnt2Sect( ptCurrent, ptStartNear, ptEndNear );
			for( int nCount	= 2; nCount < Polyline.Count; ++nCount )
			{
				double dist = Dist_Pnt2Sect( ptCurrent, (CPointD)Polyline[nCount-1], (CPointD)Polyline[nCount] );
				if( dist0 > dist )
				{
					dist0 = dist;
					ptStartNear = (CPointD)Polyline[nCount-1];
					ptEndNear	= (CPointD)Polyline[nCount];
				}
			}

			// 判断拐向
			return Side_Inflexion( ptStartNear, ptCurrent, ptEndNear );
		}

		/// <summary>
		/// 判断折线在直线的哪一边
		/// </summary>
		/// <param name="Polyline">曲线有序点数组</param>
		/// <param name="ptStart">直线起始点</param>
		/// <param name="ptEnd">直线终止点</param>
		/// <returns>返回值： -1--左侧；0--交叉；1--右侧。</returns>
		public static int Side_PolylineOnLine( IList Polyline, CPointD ptStart, CPointD ptEnd )
		{
			int nSide = Side_PntOnLine( (CPointD)Polyline[0], ptStart, ptEnd );
			if ( 0 == nSide ) 
				return 0;										//	折线的节点在线上，判为交叉
			for( int n = 1; n< Polyline.Count; n++ )
			{
				int nSide1 = Side_PntOnLine( (CPointD)Polyline[n], ptStart, ptEnd );
				if ( nSide1 == 0 || nSide1 != nSide ) 
					return 0;									//	折线的节点在线上，或与上一个节点不在直线的同一边，判为交叉。
			}

			return nSide;
		}

		/// <summary>
		/// 判断矩形在直线的哪一侧
		/// </summary>
		/// <param name="rect">待判断的矩形</param>
		/// <param name="ptStart">直线起始点</param>
		/// <param name="ptEnd">直线终止点</param>
		/// <returns>返回值： -1--左侧；0--交叉；1--右侧。</returns>
		public static int Side_RectOnLine( CRectD rect, CPointD ptStart, CPointD ptEnd )
		{
			int nSide = Side_PntOnLine( rect.TopLeft, ptStart, ptEnd );
			if ( 0 == nSide ) 
				return 0;	//	折线的节点在线上，判为交叉

			int nSide1 = Side_PntOnLine( rect.TopRight, ptStart, ptEnd );
			if ( nSide1 == 0 || nSide1 != nSide ) 
				return 0;	//	折线的节点在线上，或与上一个节点不在直线的同一边，判为交叉。
			nSide1 = Side_PntOnLine( rect.BottomRight, ptStart, ptEnd );
			if ( nSide1 == 0 || nSide1 != nSide ) 
				return 0;	//	折线的节点在线上，或与上一个节点不在直线的同一边，判为交叉。
			nSide1 = Side_PntOnLine( rect.BottomLeft, ptStart, ptEnd );
			if ( nSide1 == 0 || nSide1 != nSide ) 
				return 0;	//	折线的节点在线上，或与上一个节点不在直线的同一边，判为交叉。

			return nSide;
		}

		/// <summary>
		/// 判断线段在直线的哪一侧
		/// </summary>
		/// <param name="ptSect1">线段起始点</param>
		/// <param name="ptSect2">线段终止点</param>
		/// <param name="ptLn1">直线起始点</param>
		/// <param name="ptLn2">直线终止点</param>
		/// <returns></returns>
		public static int Side_SectOnLine( CPointD ptSect1, CPointD ptSect2, CPointD ptLn1, CPointD ptLn2 )
		{
			int nSide = Side_PntOnLine( ptSect1, ptLn1, ptLn2 );
			if ( 0 == nSide ) 
				return 0;									//	折线的节点在线上，判为交叉

			int nSide1 = Side_PntOnLine( ptSect2, ptLn1, ptLn2 );
			if ( nSide1 == 0 || nSide1 != nSide ) return 0;	//	折线的节点在线上，或与上一个节点不在直线的同一边，判为交叉。

			return nSide;
		}

		#endregion

		#region 相交关系	Is
		//------------------------------------
		//	点 与其他形状的关系
		//------------------------------------
		//	点	--〉点
		/// <summary>
		/// 点是否在点上
		/// </summary>
		/// <param name="pt1">目标点</param>
		/// <param name="pt2">比较点</param>
		/// <returns>true：两点重合</returns>
		public static bool IsPntOnPnt( CPointD pt1, CPointD pt2 )
		{
			return CPointD.IsEquals( pt1, pt2 );
		}

		//	点 --〉线
		/// <summary>
		/// 点是否在直线上
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="ptStart">直线上的起始点2</param>
		/// <param name="ptEnd">直线上的终止点</param>
		/// <returns>点是否在直线上</returns>
		public static bool  IsPntOnLine( CPointD  ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			return Side_Inflexion( ptStart, ptCurrent, ptEnd ) == 0;
		}

		/// <summary>
		///  点是否在线段上
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="ptStart">线段上的起始点2</param>
		/// <param name="ptEnd">线段上的终止点</param>
		/// <returns>点是否在线段上</returns>
		public static bool  IsPntOnSect( CPointD ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			if( IsPntOnLine( ptCurrent , ptStart ,ptEnd) )
				return	( ( ptCurrent.X >= Math.Min(ptStart.X,ptEnd.X) ) &&
					( ptCurrent.X <= Math.Max(ptStart.X,ptEnd.X) ) &&
					( ptCurrent.Y >= Math.Min(ptStart.Y,ptEnd.Y) ) &&
					( ptCurrent.Y <= Math.Max(ptStart.Y,ptEnd.Y) ) );
			else
				return false;
		}

		/// <summary>
		/// 点是否在曲线上
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="Polyline">曲线</param>
		/// <returns></returns>
		public static bool IsPntOnPolyline( CPointD ptCurrent, IList Polyline )
		{
			for( int n = 1; n < Polyline.Count; n++ )
				if ( IsPntOnSect( ptCurrent, (CPointD)Polyline[n-1], (CPointD)Polyline[n] ) ) 
					return true;

			return false;
		}

		//	点 --〉 形状
		/// <summary>
		/// 判断点是否在矩形内部
		/// </summary>
		/// <param name="ptCurrent">点</param>
		/// <param name="rect">矩形</param>
		/// <returns>矩形是否包含点</returns>
		public static bool IsPntInRect( CPointD ptCurrent, CRectD rect )
		{
			return rect.Contains( ptCurrent );
		}

		/// <summary>
		/// 点是否在圆内，计算圆心到该点的距离，如果小于等于半径则该点在圆内。 
		/// </summary>
		/// <param name="ptCurrent">判断点坐标</param>
		/// <param name="ptCirCenter">圆心</param>
		/// <param name="radius">圆半径</param>
		/// <returns>点是否在圆内</returns>
		public static bool IsPntInCircle( CPointD ptCurrent, CPointD ptCirCenter, double radius )
		{
			// 圆心到判断点距离
			return ( Distance2( ptCurrent, ptCirCenter ) <= radius*radius ) ? true : false;
		}

		//	点 --〉 多边形
		/// <summary>
		/// 判断点是否在多边形中:
		/// </summary>
		/// <param name="ptCurrent">点p</param>
		/// <param name="Polygon">多边形顶点的有序数组</param>
		/// <param name="rcBound">Polygon的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <returns>点是否在多边形区域内</returns>
		/// <remarks>
		/// 以点P为端点，向左方作射线L，由于多边形是有界的，所以射线L的左端一定在多边形外，	
		/// 考虑沿着L从无穷远处开始自左向右移动，遇到和多边形的第一个交点的时候，进入
		/// 到了多边形的内部，遇到第二个交点的时候，离开了多边形，……
		/// 所以很容易看出当L和多边形的交点数目C是奇数的时候，P在多边形内，是偶数的话P在多边形外。
		/// </remarks>
		public static bool IsPntInPolygon( CPointD ptCurrent, IList Polygon, CRectD rcBound )
		{
			if ( rcBound == null )
			{
				rcBound = BoundRect( Polygon );
				//	初步判断
				if ( ! rcBound.Contains( ptCurrent ) )
					return false;
			}

			CPointD L		= new CPointD( ptCurrent.X*10000, ptCurrent.Y );
			int CrossCont	= 0;
			for( int nCount	= 1; nCount < Polygon.Count; ++nCount )
			{
				if( IsPntOnSect( ptCurrent, (CPointD)Polygon[nCount], (CPointD)Polygon[nCount-1] ) )
				{
					return true;
				}

				if( ((CPointD)Polygon[nCount]).Y != ((CPointD)Polygon[nCount-1]).Y )
				{
					if( IsPntOnSect( (CPointD)Polygon[nCount], ptCurrent, L) && ((CPointD)Polygon[nCount]).Y > ((CPointD)Polygon[nCount-1]).Y )
					{
						CrossCont++;
					}
					else if( IsPntOnSect((CPointD)Polygon[nCount-1], ptCurrent, L) && ((CPointD)Polygon[nCount-1]).Y > ((CPointD)Polygon[nCount]).Y )
					{
						CrossCont++;
					}
					else if( IsSectCrossSect(ptCurrent, L, (CPointD)Polygon[nCount-1],(CPointD)Polygon[nCount]) )
					{
						CrossCont++;
					}
				}
			}

			if( CrossCont % 2 == 1 )
				return true;
			else
				return false;


/*			CPointD	P1 = new CPointD( ptCurrent );
			CPointD P2 = new CPointD( ptCurrent.X, rcBound.Top - 1000.0 );		//	P1，P2构成垂线
			CPointD	P3 = null;
			CPointD P4 = null;
		
			int		CrossPointNumber =0;
			double	MinX,MaxX,MinY;

			int nSize = Polygon.Count;
			for ( int i = 0; i < nSize; i++ )
			{
				P3 = (CPointD)Polygon[i];
				P4 = ( i + 1 < nSize ) ? (CPointD)Polygon[i + 1]: (CPointD)Polygon[0];
				if ( CPointD.IsEquals( P3, P4 ) ) continue;						//	两个点重合， 不算
				MinX =  Math.Min( P3.X, P4.X );
				MaxX = 	Math.Max( P3.X, P4.X );
				MinY = 	Math.Min( P3.Y, P4.Y );
				if ( P1.X < MinX || P1.X > MaxX ||  P1.Y < MinY ) continue;		//	垂线不在线段范围之内，可以不算

				if ( DoubleEquals( ptCurrent.X, MinX) || DoubleEquals( ptCurrent.X, MaxX ) )			
					P2.X = P1.X = ptCurrent.X - jGMC.EPSLN;						//	垂线过线段端点，垂线要偏移个微量
				else
					P2.X = P1.X = ptCurrent.X;

				CrossPointNumber += ( IsSectCrossSect(P1, P2, P3, P4) ? 1 : 0);	//	判断垂线和线段是否交叉
			}

			return ( (CrossPointNumber % 2) > 0 ) ? true: false;
*/
		}


		//------------------------------------
		//	线 与其他形状的关系
		//------------------------------------
		//	线 --〉 线
		/// <summary>
		/// 两线段是否相交
		/// </summary>
		/// <param name="ptAStart">线段A的起始点</param>
		/// <param name="ptAEnd">线段A的终止点</param>
		/// <param name="ptBStart">线段B起始点</param>
		/// <param name="ptBEnd">线段B终止点</param>
		/// <returns>线段是否相交</returns>
		public static bool IsSectCrossSect( CPointD ptAStart,CPointD ptAEnd ,CPointD ptBStart ,CPointD ptBEnd )
		{
			//	( P1 - Q1 ) × ( Q2 - Q1 )  *  ( Q2 - Q1 ) × ( P2 - Q1 )  ≥  0
			//	( Q1 - P1 ) × ( P2 - P1 )  *  ( P2 - P1 ) × ( Q2 - P1 )  ≥  0
			return ( CrossProduce(ptAStart - ptBStart, ptBEnd - ptBStart) * 
				CrossProduce(ptBEnd   - ptBStart, ptAEnd - ptBStart) >= 0.0 &&
				CrossProduce(ptBStart - ptAStart, ptAEnd - ptAStart) *
				CrossProduce(ptAEnd   - ptAStart, ptBEnd - ptAStart) >= 0.0 );
		}

		/// <summary>
		/// 线段与直线相交
		/// </summary>
		/// <param name="ptAStart">线段起始点</param>
		/// <param name="ptAEnd">线段终止点</param>
		/// <param name="ptBStart">直接起始点</param>
		/// <param name="ptBEnd ">直线终止点</param>
		/// <returns>线段是否与直线相交</returns>
		public static bool  IsSectCrossLine( CPointD ptAStart, CPointD ptAEnd ,CPointD ptBStart ,CPointD ptBEnd  )
		{
			//	( P1 - Q1 ) × ( Q2 - Q1 ) * ( Q2 - Q1 ) × ( P2 - Q1 ) ≥ 0
			return ( CrossProduce( (ptAStart - ptBStart), (ptBEnd - ptBStart) ) * 
				CrossProduce( (ptBEnd - ptBStart),   (ptAEnd - ptBStart) ) >= 0 );
		}

		/// <summary>
		/// 判断线段是否和折线相交
		/// </summary>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <param name="Polyline">折线顶点的有序数组</param>
		/// <returns>线段是否和折线相交</returns>
		/// <remarks>一一判断线段是否和组成折线的线段是否相交</remarks>
		public static bool IsSectCrossPolyline( CPointD ptStart, CPointD ptEnd, IList Polyline )
		{
			for ( int n = 1; n<Polyline.Count; n++ )
			{
				CPointD pt1 = (CPointD)Polyline[n-1];
				CPointD pt2 = (CPointD)Polyline[n];
				if ( IsSectCrossSect( ptStart, ptEnd, pt1, pt2 ) ) return true;
			}
			return false;
		}

		/// <summary>
		/// 判断直线与直线是否相交
		/// </summary>
		/// <param name="ptAStart">直线A的起始点</param>
		/// <param name="ptAEnd">直线A的终止点</param>
		/// <param name="ptBStart">直接B起始点</param>
		/// <param name="ptBEnd">直线B终止点</param>
		/// <returns>直线与直线是否相交</returns>
		/// <remarks>对无限延伸的两直线来说，只要两直线不是平行线(重合除外)，必要交点，所以只需判断两直线是否平行既可</remarks>
		public static bool IsLineCrossLine( CPointD ptAStart, CPointD ptAEnd, CPointD ptBStart, CPointD ptBEnd )
		{
			if ( DoubleEquals( CrossProduce( ( ptAEnd-ptAStart ), ( ptBEnd-ptBStart ) ) , 0.0 ) )	//	平行
			{
				//	求是否重合,直线A 上任何一点在直线B 上，就是重合
				if ( IsPntOnLine( ptAStart, ptBStart, ptBEnd ) )
					return true;
				return false;
			}
			return true;

			/*
						//	求两条直线的直线方程
						double A1, B1, C1;
						double A2, B2, C2;
						LineFomuleFrom2Pnt( ptAStart, ptAEnd, out A1, out B1, out C1 );
						LineFomuleFrom2Pnt( ptBStart, ptBEnd, out A2, out B2, out C2 );
						// A1/A2 = B1/B2 != C1/C2 平行且不重合
						// A1/A2 = B1/B2 = C1/C2 重合
						double E01, E02, E11, E12;
						E01 = A1 * B2; E02 = B1 * A2;
						E11 = B1 * C2; E12 = C1 * B2;
						if ( DoubleEquals( E01, E02 ) )	//	平行
						{
							if ( DoubleEquals( E11, E12 ) )	//	重合
								return true;
							else 
								return false;
						}
						else
							return false;
			*/
		}

		/// <summary>
		/// 判断直线是否和折线相交
		/// </summary>
		/// <param name="ptStart">直线的一点</param>
		/// <param name="ptEnd">直线的另一点</param>
		/// <param name="Polyline">折线顶点的有序数组</param>
		/// <returns>直线是否和折线相交</returns>
		/// <remarks>一一判断直线是否和组成折线的线段是否相交</remarks>
		public static bool IsLineCrossPolyline( CPointD ptStart, CPointD ptEnd, IList Polyline )
		{
			for ( int n = 1; n<Polyline.Count; n++ )
			{
				CPointD pt1 = (CPointD)Polyline[n-1];
				CPointD pt2 = (CPointD)Polyline[n];
				if ( IsSectCrossLine( pt1, pt2, ptStart, ptEnd ) ) 
					return true;
			}

			return false;
		}

		/// <summary>
		/// 判断折线是否与另一个折线相交
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( rcBoundA.IntersectsWith( rcBoundB ) == true ) IsPolylineCrossPolyline(...);
		/// </code>
		/// </summary>
		/// <param name="PolylineA">折线A有序点数组</param>
		/// <param name="PolylineB">折线B有序点数组</param>
		/// <returns>折线是否与另一个折线相交，存在返回真，否则返回假</returns>
		/// <remarks>利用IsSectCrossSect涵数分别求折线中每一线段是否与另一个折线存在交点, 假设在使用此方法之前已进行了包容矩形比较</remarks>
		public static bool IsPolylineCrossPolyline( IList PolylineA, IList PolylineB )
		{
			for ( int nA = 1; nA < PolylineA.Count; nA++ )
			{
				for ( int nB = 1; nB < PolylineB.Count; nB++ )
				{
					if ( IsSectCrossSect( (CPointD)PolylineA[nA-1], (CPointD)PolylineA[nA], (CPointD)PolylineB[nB-1], (CPointD)PolylineB[nB] )   )
						return true;
				}
			}

			return false;
		}

		//	线 --〉 形状
		/// <summary>
		/// 判断线段是否在矩形中	
		/// </summary>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <param name="rect">矩形</param>
		/// <returns>是否在矩形内</returns>
		/// <remarks>两个端点必须都在矩形内</remarks>
		public static bool IsSectInRect(  CPointD ptStart, CPointD ptEnd, CRectD rect )
		{
			return ( rect.Contains( ptStart ) && rect.Contains( ptEnd ) );
		}

		/// <summary>
		/// 判断线段是否和矩形相交
		/// </summary>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <param name="rect">矩形</param>
		/// <returns>线段是否和矩形相交</returns>
		public static bool IsSectCrossRect( CPointD ptStart, CPointD ptEnd, CRectD rect )
		{
			if ( IsSectCrossSect( ptStart, ptEnd, rect.TopLeft, rect.TopRight ) ) 
				return true;
			if ( IsSectCrossSect( ptStart, ptEnd, rect.TopRight, rect.BottomRight ) ) 
				return true;
			if ( IsSectCrossSect( ptStart, ptEnd, rect.BottomRight, rect.BottomLeft ) ) 
				return true;
			if ( IsSectCrossSect( ptStart, ptEnd, rect.BottomLeft, rect.TopLeft ) ) 
				return true;

			return IsSectInRect( ptStart, ptEnd, rect );
		}

		/// <summary>
		/// 判断直线是否和矩形相交
		/// </summary>
		/// <param name="ptStart">直线起始点</param>
		/// <param name="ptEnd">直线终止点</param>
		/// <param name="rect">矩形</param>
		/// <returns>直线是否和矩形相交</returns>
		public static bool IsLineCrossRect( CPointD ptStart, CPointD ptEnd, CRectD rect )
		{
			if ( IsSectCrossLine( rect.TopLeft, rect.TopRight, ptStart, ptEnd ) ) 
				return true;
			if ( IsSectCrossLine( rect.TopRight, rect.BottomRight, ptStart, ptEnd ) ) 
				return true;
			if ( IsSectCrossLine( rect.BottomRight, rect.BottomLeft, ptStart, ptEnd ) ) 
				return true;
			if ( IsSectCrossLine( rect.BottomLeft, rect.TopLeft, ptStart, ptEnd ) ) 
				return true;

			return false;
		}

		/// <summary>
		/// 判断线段、折线、多边形是否在矩形中。
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( rect.IntersectsWith( rcBound ) == true ) IsPolylineInRect(...);
		/// </code>
		/// </summary>
		/// <param name="Polyline">点数组</param>
		/// <param name="rect">矩形</param>
		/// <returns>是否在矩形内</returns>
		public static bool IsPolylineInRect( IList Polyline, CRectD rect )
		{
					
			foreach( CPointD p in Polyline )
			{
				if( !rect.Contains( p ) ) return false;
			}

			return true;			
		}

		/// <summary>
		/// 判断折线是否和矩形相交
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( rect.Contains( rcBound ) == true ) true;
		/// if( rect.IntersectsWith( rcBound ) == true ) IsPolylineInRect(...);
		/// </code>
		/// </summary>
		/// <param name="Polyline">点数组</param>
		/// <param name="rect">矩形</param>
		/// <returns>折线是否和矩形相交</returns>
		/// <remarks>折线的每一个线段是否和矩形相交</remarks>
		public static bool IsPolylineCrossRect( IList Polyline, CRectD rect )
		{
			for ( int n = 1; n<Polyline.Count; n++ )
			{
				if ( IsSectCrossRect( (CPointD)Polyline[n-1], (CPointD)Polyline[n], rect ) ) 
					return true;
			}

			return false;
		}

		/// <summary>
		///  判断折线是否在圆内
		/// </summary>
		/// <param name="Polyline">线段、折线、矩形、多边形的有序点数组坐标</param>
		/// <param name="ptCirCenter">圆心</param>
		/// <param name="radius">圆半径</param>
		/// <returns>折线是否在圆内</returns>
		/// <remarks>因为圆是凸集，所以只要判断是否每个顶点都在圆内即可。</remarks>
		public static bool IsPolylineInCircle( IList Polyline, CPointD ptCirCenter, double radius )
		{
			foreach( CPointD pt in Polyline )
			{
				if( !IsPntInCircle( pt, ptCirCenter, radius ) )
					return false;
			}

			return true;
		}


		///	线 --〉 多边形
		/// <summary>
		/// 判断线段是否在多边形内
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( rcBound.Contains( ptStart ) == true 并且 rcBound.Contains( ptEnd ) ) IsSectInPolygon( ... );
		/// </code>
		/// </summary>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <param name="Polygon">多边形顶点的有序数组</param>
		/// <param name="rcBound">Polygon的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <returns>线段是否在多边形区域内</returns>
		/// <remarks>
		/// 线段在多边形内的一个必要条件是线段的两个端点都在多边形内，但由于多边形可能为凹，
		/// 所以这不能成为判断的充分条件。如果线段和多边形的某条边内交（两线段内交是指两线段相交且交点不在两线段的端点），
		/// 因为多边形的边的左右两侧分属多边形内外不同部分，所以线段一定会有一部分在多边形外。于是我们得到线段在多边
		/// 形内的第二个必要条件：线段和多边形的所有边都不内交。线段和多边形交于线段的两端点并不会影响线段是否在多边形内；
		/// 但是如果多边形的某个顶点和线段相交，还必须判断两相邻交点之间的线段是否包含于多边形内部。 因此我们可以先求出所
		/// 有和线段相交的多边形的顶点，然后按照X-Y坐标排序(X坐标小的排在前面，对于X坐标相同的点，Y坐标小的排在前面，这种
		/// 排序准则也是为了保证水平和垂直情况的判断正确)，这样相邻的两个点就是在线段上相邻的两交点，如果任意相邻两点的中
		/// 点也在多边形内，则该线段一定在多边形内。
		/// </remarks>
		public static bool IsSectInPolygon( CPointD ptStart, CPointD ptEnd, IList Polygon, CRectD rcBound )
		{
			if ( rcBound == null ) 
				rcBound = BoundRect( Polygon );

			// 两端点是否在多边形中
			if( !IsPntInPolygon( ptStart, Polygon, rcBound ) || !IsPntInPolygon( ptEnd, Polygon, rcBound ) )
				return false;

			//	现在两个端点都在多边形内
			//	进行相交判断交叉，考虑端点落在线上的特例
			ArrayList ptsIntersect = new ArrayList();	//	重合点集合
			for( int nCount	= 1; nCount < Polygon.Count; ++nCount )
			{
				CPointD pt1 = (CPointD)Polygon[nCount - 1];
				CPointD pt2 = (CPointD)Polygon[nCount];
				// 线段的某个端点在s上
				if( IsPntOnSect( ptStart, pt1, pt2 ) )
				{
					ptsIntersect.Add( ptStart );
				}
				else if( IsPntOnSect( ptEnd, pt1, pt2 ) )
				{
					ptsIntersect.Add( ptEnd );
				}
					// s的某个端点在线段PQ上
				else if( IsPntOnSect( pt1, ptStart, ptEnd  ) )
				{
					ptsIntersect.Add( pt1 );
				}
				else if( IsPntOnSect( pt2, ptStart, ptEnd ) )
				{
					ptsIntersect.Add( pt2 );
				}
					// s和线段PQ相交,这时候已经可以肯定是内交了
				else if( IsSectCrossSect( ptStart, ptEnd, pt1, pt2 ) )
				{
					return false;
				}
			}
			if ( 0 == ptsIntersect.Count ) 
				return true;							//	没有重合点

			// 将ptsIntersect中的点按照X-Y坐标排序;
			ptsIntersect.Sort( new jGMA.CPointDComparer() );
			// 判断ptsIntersect中每两个相邻点的中点是否在多边形中
			CPointD ptMiddle = new CPointD();
			for( int nCount	= 1; nCount < ptsIntersect.Count; ++nCount )
			{
				CPointD pt1 = (CPointD)ptsIntersect[nCount - 1];
				CPointD pt2 = (CPointD)ptsIntersect[nCount];
				ptMiddle.X = (pt1.X + pt2.X)/2.0;
				ptMiddle.Y = (pt1.Y + pt2.Y)/2.0;
				if( !IsPntInPolygon( ptMiddle, Polygon, rcBound ) )
					return false;
			}

			return true;
		}
	
		/// <summary>
		/// 判断线段是否和多边形相交
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( rcBound.Contains( ptStart ) == true ||  rcBound.Contains( ptEnd ) ) IsSectCrossPolygon( ... );
		/// </code>
		/// </summary>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <param name="Polygon">多边形顶点的有序数组</param>
		/// <param name="rcBound">Polygon的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <returns>线段是否和多边形相交</returns>
		public static bool IsSectCrossPolygon( CPointD ptStart, CPointD ptEnd, IList Polygon, CRectD rcBound )
		{
			if ( rcBound == null )
				rcBound = BoundRect( Polygon );

			// 两端点中的任何一个在多边形中，就算交叉
			if( IsPntInPolygon( ptStart, Polygon, rcBound ) || IsPntInPolygon( ptEnd, Polygon, rcBound ) )
				return true;

			//	现在两个端点都在多边形外，进行相交判断交叉，端点落在线上算作交叉
			for( int nCount	= 1; nCount < Polygon.Count; ++nCount )
			{
				CPointD pt1 = (CPointD)Polygon[nCount - 1];
				CPointD pt2 = (CPointD)Polygon[nCount];
				if( IsSectCrossSect( ptStart, ptEnd, pt1, pt2 ) )
					return true;
			}

			return false;
		}

		/// <summary>
		/// 判断直线段是否和多边形相交
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( Side_RectOnLine( rcBound, ptStart, ptEnd ) == 0 ) IsLineCrossPolygon( ... );
		/// </code>
		/// </summary>
		/// <param name="ptStart">直线起始点</param>
		/// <param name="ptEnd">直线终止点</param>
		/// <param name="Polygon">多边形顶点的有序数组</param>
		/// <returns>直线是否和多边形相交</returns>
		public static bool IsLineCrossPolygon( CPointD ptStart, CPointD ptEnd, IList Polygon )
		{
			//	进行相交判断交叉，端点落在线上算作交叉
			for( int nCount	= 1; nCount < Polygon.Count; ++nCount )
			{
				CPointD pt1 = (CPointD)Polygon[nCount - 1];
				CPointD pt2 = (CPointD)Polygon[nCount];
				if( IsSectCrossLine( pt1, pt2, ptStart, ptEnd ) )
					return true;
			}

			return false;
		}

		/// <summary>
		/// 判断折线是否在多边形区域内
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( rcPolygonBound.IntersectsWith( rcPolylineBound ) ) IsPolylineInPolygon( ... );
		/// </code>
		/// </summary>
		/// <param name="Polyline">折线顶点有序数组</param>
		/// <param name="Polygon">多边形顶点的有序数组</param>
		/// <param name="rcPolygonBound">Polygon的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <returns>折线是否在多边形区域内</returns>
		/// <remarks>只要判断折线的每条线段是否都在多边形内即可。设折线有m条线段，多边形有n个顶点，则该算法的时间复杂度为O(m*n)。</remarks>
		public static bool IsPolylineInPolygon( IList Polyline, IList Polygon, CRectD rcPolygonBound )
		{
			if ( rcPolygonBound == null ) 
				rcPolygonBound = BoundRect( Polygon );

			for( int nCount	= 1; nCount < Polyline.Count; ++nCount )
			{
				CPointD pt1 = (CPointD)Polyline[nCount-1];
				CPointD pt2 = (CPointD)Polyline[nCount];
				if( !IsSectInPolygon( pt1, pt2, Polygon, rcPolygonBound ) )
					return false;
			}

			return true;
		}

		/// <summary>
		/// 判断折线是否和多边形区域相交
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( rcPolygonBound.IntersectsWith( rcPolylineBound ) ) IsPolylineInPolygon( ... );
		/// </code>
		/// </summary>
		/// <param name="Polyline">折线顶点有序数组</param>
		/// <param name="Polygon">多边形顶点的有序数组</param>
		/// <param name="rcPolygonBound">Polygon的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <returns>折线是否和多边形区域相交</returns>
		/// <remarks>只要判断折线的每条线段是否都在多边形内即可。设折线有m条线段，多边形有n个顶点，则该算法的时间复杂度为O(m*n)。</remarks>
		public static bool IsPolylineCrossPolygon( IList Polyline, IList Polygon, CRectD rcPolygonBound )
		{
			if ( rcPolygonBound == null ) 
				rcPolygonBound = BoundRect( Polygon );

			for( int nCount	= 1; nCount < Polyline.Count; ++nCount )
			{
				CPointD pt1 = Polyline[nCount-1] as CPointD;
				CPointD pt2 = Polyline[nCount] as CPointD;
				if( IsSectCrossPolygon( pt1, pt2, Polygon, rcPolygonBound ) )
					return true;
			}

			return false;
		}
	

		//------------------------------------
		//	多边形 与其他形状的关系
		//------------------------------------
		//	多边形 --〉 多边形
		/// <summary>
		///  判断多边形A是否在多边形B区域内
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( rcBoundB.Contains( rcBoundA ) ) IsPolygonInPolygon( ... );
		/// </code>
		/// </summary>
		/// <param name="PolygonA">多边形A顶点有序数组</param>
		/// <param name="PolygonB">多边形B顶点的有序数组</param>
		/// <param name="rcBoundA">多边形A的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <param name="rcBoundB">多边形B的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <returns>多边形是否在多边形区域内</returns>
		/// <remarks>只要判断多边形A的折线是否都在多边形B内即可。</remarks>
		public static bool IsPolygonInPolygon( IList PolygonA, IList PolygonB, CRectD rcBoundA, CRectD rcBoundB )
		{
			if ( rcBoundB == null ) 
				rcBoundB = BoundRect( PolygonB );
			if ( false == IsPolylineInPolygon( PolygonA, PolygonB, rcBoundB ) )
				return false;
			//	考虑多边形首尾点;
			CPointD pt1 = (CPointD)PolygonA[PolygonA.Count-1];
			CPointD pt2 = (CPointD)PolygonA[0];
			if ( pt1.IsEquals( pt2 ) ) 
				return true;

			return IsSectInPolygon( pt1, pt2, PolygonB, rcBoundB );
		}

		/// <summary>
		///  判断多边形A是否和多边形B交叉
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( rcBoundA.IntersectsWith( rcBoundB ) ) IsPolygonCrossPolygon( ... );
		/// </code>
		/// </summary>
		/// <param name="PolygonA">多边形A顶点有序数组</param>
		/// <param name="PolygonB">多边形B顶点的有序数组</param>
		/// <param name="rcBoundA">多边形A的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <param name="rcBoundB">多边形B的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <returns>多边形A是否和多边形B交叉</returns>
		/// <remarks>A 在B 内、AB交叉、B 在A 内</remarks>
		public static bool IsPolygonCrossPolygon( IList PolygonA, IList PolygonB, CRectD rcBoundA, CRectD rcBoundB )
		{
			if ( rcBoundB == null ) 
				rcBoundB = BoundRect( PolygonB );
			if ( IsPolylineCrossPolygon( PolygonA, PolygonB, rcBoundB ) ) 
			{
				//	考虑多边形首尾点;
				CPointD pt1 = (CPointD)PolygonA[PolygonA.Count-1];
				CPointD pt2 = (CPointD)PolygonA[0];
				if ( pt1.IsEquals( pt2 ) ) 
					return true;
				if ( IsSectCrossPolygon( pt1, pt2, PolygonB, rcBoundB ) )
					return true;
			}

			//	以上只能判断出A 在 B 中，或AB交叉
			//	还要，排除A 包含B 的可能
			if ( IsPntInPolygon( (CPointD)PolygonB[0], PolygonA, rcBoundA ) )
				return true;

			return false;
		}

		//------------------------------------
		//	矩形、圆 与其他形状的关系
		//------------------------------------
		//	矩形 --〉
		/// <summary>
		/// 判断矩形是否在矩形中
		/// </summary>
		/// <param name="rect1">矩形1</param>
		/// <param name="rect2">矩形2</param>
		/// <returns>rect1是否包含 reect2</returns>
		public static bool IsRectInRect( CRectD rect1, CRectD rect2 )
		{
			return rect1.Contains( rect2 );
		}

		/// <summary>
		/// 判断矩形1是否和矩形2相交,jingzhou xu,2006.4.10
		/// </summary>
		/// <param name="rect1">矩形1</param>
		/// <param name="rect2">矩形2</param>
		/// <returns>rect1是否和 reect2相交</returns>
		public static bool IsRectCrossRect( CRectD rect1, CRectD rect2 )
		{
			bool bResult = false;

			// 四边有相交情况
			if( IsSectCrossRect( rect1.TopLeft, rect1.TopRight, rect2 ) )
				bResult = true;
			else if( IsSectCrossRect( rect1.TopRight, rect1.BottomRight, rect2 ) )
				bResult = true;
			else if( IsSectCrossRect( rect1.BottomRight, rect1.BottomLeft, rect2 ) )
				bResult = true;
			else if( IsSectCrossRect( rect1.BottomLeft, rect1.TopLeft, rect2 ) )
				bResult = true;
			else if( IsRectInRect( rect1, rect2 ) )		// 不相交，但包含在内情况
				bResult = true;

			return bResult;
		}

		/// <summary>
		/// 判断矩形是否在多边形内。
		/// 没有进行初步判断，为加快速度，请在使用此方法之前进行包容矩形比较。
		/// <code>
		/// if( rcBound.Contains( rect ) ) IsRectInPolygon( ... );
		/// </code>
		/// </summary>
		/// <param name="rect">矩形</param>
		/// <param name="Polygon">多边形顶点的有序数组</param>
		/// <param name="rcBound">Polygon的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <returns>矩形是否在多边形区域内</returns>
		public static bool IsRectInPolygon( CRectD rect, IList Polygon, CRectD rcBound )
		{
			// 将矩形四点转换为多边形顶点数组
			ArrayList	ptsRect = new ArrayList();
			ptsRect.Add( rect.TopLeft );
			ptsRect.Add( rect.TopRight );
			ptsRect.Add( rect.BottomRight );
			ptsRect.Add( rect.BottomLeft );

			// 判断多边形是否在多边形中
			return IsPolygonInPolygon( ptsRect, Polygon, rect, rcBound );
		}

		//	圆 --〉
		/// <summary>
		/// 判断圆是否在矩形中
		/// </summary>
		/// <param name="ptCirCenter">圆心</param>
		/// <param name="radius">圆半径</param>
		/// <param name="rect">矩形</param>
		/// <returns>矩形是否包含圆</returns>
		public static bool IsCircleInRect( CPointD ptCirCenter, double radius, CRectD rect )
		{
			// 圆在矩形中的充要条件是：圆心在矩形中且圆的半径小于等于圆心到矩形四边的距离的最小值
			if( IsPntInRect( ptCirCenter, rect ) == true )
			{
				double distanceTop		= Dist_Pnt2Line( ptCirCenter, new CPointD(rect.Left, rect.Top),new CPointD(rect.Right, rect.Top) );
				double distanceLeft		= Dist_Pnt2Line( ptCirCenter, new CPointD(rect.Left, rect.Top),new CPointD(rect.Left, rect.Bottom) );
				double distanceBottom	= Dist_Pnt2Line( ptCirCenter, new CPointD(rect.Left, rect.Bottom),new CPointD(rect.Right, rect.Bottom) );
				double distanceRight	= Dist_Pnt2Line( ptCirCenter, new CPointD(rect.Right, rect.Bottom),new CPointD(rect.Right, rect.Top) );
				double miner1			= Math.Min( distanceTop, distanceLeft );
				double miner2			= Math.Min( distanceBottom, distanceRight );
				double miner			= Math.Min( miner1, miner2 );
				if( radius <= miner )
					return true;
				else
					return false;
			}
			else
				return false;
		}

		/// <summary>
		/// 判断圆是否在多边形区域内
		/// </summary>
		/// <param name="ptCirCenter">圆心</param>
		/// <param name="radius">圆半径</param>
		/// <param name="Polygon">多边形顶点的有序数组</param>
		/// <param name="rcBound">Polygon的包容矩形，如果是null，计算矩形，如果是错误的矩形，会导致结果不确定</param>
		/// <returns>圆是否在多边形区域内</returns>
		/// <remarks>只要圆心在多边形中，并且计算圆心到多边形的每条边的最短距离，如果该距离大于等于圆半径则该圆在多边形内。
		/// </remarks>
		public static bool IsCircleInPolygon( CPointD ptCirCenter, double radius, IList Polygon, CRectD rcBound )
		{
			// 圆心是否在多边形内
			if( !IsPntInPolygon( ptCirCenter, Polygon, rcBound ) )
				return false;

			// 计算圆心到多边形各边的最短距离
			for( int nCount = 2; nCount < Polygon.Count; ++nCount )
			{
				if ( radius > Dist_Pnt2Sect( ptCirCenter, (CPointD)Polygon[nCount], (CPointD)Polygon[nCount-1] ) )
					return false;
			}

			return true;
		}

		/// <summary>
		/// 设两圆为O1,O2，半径分别为r1, r2，要判断O2是否在O1内。先比较r1，r2的大小，
		/// 如果r1〈r2则O2不可能在O1内；否则如果两圆心的距离大于r1 - r2 ，则O2不在O1内；否则O2在O1内。
		/// </summary>
		/// <param name="lhsCirCenter">第一个圆圆心</param>
		/// <param name="lhsRadius">第一个圆圆半径</param>
		/// <param name="rhsCirCenter">第二个圆圆心</param>
		/// <param name="rhsRadius">第二个圆圆半径</param>
		/// <returns>圆一是否在圆二内</returns>
		public static bool IsCircleInCircle( CPointD lhsCirCenter, double lhsRadius, CPointD rhsCirCenter, double rhsRadius )
		{
			if( lhsRadius > rhsRadius )
				return false;
			if( Distance( lhsCirCenter, rhsCirCenter ) > rhsRadius-lhsRadius )
				return false;

			return true;
		}

		#endregion

		#region 求交点	CP_
		/// <summary>
		/// 求点到线段的最近点
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <returns>点到线段的最近交点</returns>
		/// <remarks>
		///  如果该线段平行于X轴（Y轴），则过点point作该线段所在直线的垂线，垂足很容易求得，然后计算出垂足，
		///  如果垂足在线段上则返回垂足，否则返回离垂足近的端点；如果该线段不平行于X轴也不平行于Y轴，则斜率
		///  存在且不为0。设线段的两端点为pt1和pt2，斜率为：k = ( pt2.Y - pt1. y ) / (pt2.X - pt1.X );该直线
		///  方程为：y = k* ( x - pt1.X) + pt1.Y。其垂线的斜率为 - 1 / k，垂线方程为：y = (-1/k) * (x - point.X) + point.Y。
		///  联立两直线方程解得：x = ( k^2 * pt1.X + k * (point.Y - pt1.Y ) + point.X ) / ( k^2 + 1) ，y = k * ( x - pt1.X) + pt1.Y;
		///  然后再判断垂足是否在线段上，如果在线段上则返回垂足；如果不在则计算两端点到垂足的距离，选择距离垂足较近的端点返回。
		/// </remarks>
		public static CPointD CP_Pnt2Sect( CPointD ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			CPointD ptVertical = CP_Pnt2Line( ptCurrent, ptStart, ptEnd );	// 垂足
			if ( ptVertical.IsEmpty )
			{
				ptVertical = ptStart;
			}
				// 判断垂足是否在线段上，是则返回垂足
			else if( !IsPntOnSect( ptVertical, ptStart, ptEnd ) )
				ptVertical = ( Distance2( ptStart, ptVertical ) < Distance2( ptEnd, ptVertical ) ) ? ptStart : ptEnd;

			return ptVertical;
		}

		/// <summary>
		/// 求点到直线的垂足
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="ptStart">直线的一点</param>
		/// <param name="ptEnd">直线的另一点</param>
		/// <returns>点到直线的垂点</returns>
		public static CPointD CP_Pnt2Line( CPointD ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			double A,B,C,Av,Bv,Cv;
			LineFomuleFrom2Pnt( ptStart, ptEnd, out A, out B, out C );
			LineFomuleOfPntVerticalToLine( ptCurrent, ptStart, ptEnd, out Av, out Bv, out Cv );

			return CrossPointOf2Line( A, B, C, Av, Bv, Cv );	// 垂足
		}

		/// <summary>
		///  点到折线最近交点
		/// </summary>
		/// <param name="ptCurrent">当前点</param>
		/// <param name="Polyline">折线有序点数组</param>
		/// <returns>点到折线的最近交点</returns>
		/// <remarks>
		///  只要分别计算点到每条线段的最近点，记录最近距离，取其中最近距离最小的点即可。 
		/// </remarks>
		public static CPointD CP_Pnt2Polyline( CPointD ptCurrent, IList Polyline )
		{
			CPointD ptV = CP_Pnt2Sect( ptCurrent, (CPointD)Polyline[0], (CPointD)Polyline[1] );
			double dist = Distance2( ptCurrent, ptV );
			// 记录点到每条线段的最近点
			for( int nCount	= 1; nCount < Polyline.Count; ++nCount )
			{
				CPointD pt	= CP_Pnt2Sect( ptCurrent, (CPointD)Polyline[nCount-1], (CPointD)Polyline[nCount] );
				double d	= Distance2( ptCurrent, pt );
				if ( dist > d )
				{
					dist	= d;
					ptV		= pt;
				}
			}

			return ptV;
		}

		/// <summary>
		/// 点到圆的最近距离是否有交点
		/// </summary>
		/// <param name="ptCurrent">当前点</param>
		/// <param name="ptCirCenter">圆心点</param>
		/// <param name="radius">圆半径</param>
		/// <param name="ptResult">返回点到圆的最近距离的交点，没有交点时为(0,0)</param>
		/// <returns>点到圆的最近距离是否有交点</returns>
		/// <remarks>
		/// <code>
		/// 如果该点在圆心，因为圆心到圆周任一点的距离相等，返回UNDEFINED。
		/// 连接点P和圆心O，如果PO平行于X轴，则根据P在O的左边还是右边计算出
		/// 最近点的横坐标为centerPoint.X - radius 或 centerPoint.X + radius。
		/// 如果PO平行于Y轴，则根据P在O的上边还是下边计算出最近点的纵坐标为 
		/// centerPoint.Y + radius或 centerPoint.Y - radius。如果PO不平行于X轴和Y轴，
		/// 则PO的斜率存在且不为0，这时直线PO斜率为k = （ P.Y - O.Y ）/ ( P.X - O.X )。
		/// 直线PO的方程为：y = k * ( x - P.X) + P.Y。设圆方程为:(x - O.X ) ^2 + ( y - O.Y ) ^2 = r ^2，
		/// 联立两方程组可以解出直线PO和圆的交点，取其中离P点较近的交点即可。
		/// 求解后的交点坐标为：
		/// x1 = sqrt( r^2 / ( 1 + ( (P.Y - O.Y)/(P.X - O.X) )^2 ) ) + O.X; 
		/// x2 = -sqrt( r^2 / ( 1 + ( (P.Y - O.Y)/(P.X - O.X) )^2 ) ) + O.X;
		/// y1 = sqrt( r^2 / ( 1 + ( (P.X - O.X)/(P.Y - O.Y) )^2 ) ) + O.Y;
		/// y2 = -sqrt( r^2 / ( 1 + ( (P.X - O.X)/(P.Y - O.Y) )^2 ) ) + O.Y;
		/// </code>
		/// </remarks>
		public static bool CP_Pnt2Circle( CPointD ptCurrent, CPointD ptCirCenter, double radius , out CPointD ptResult )
		{
			ptResult = new CPointD( );
			
			// 如果该点在圆心，因为圆心到圆周任一点的距离相等，返回false
			if( ptCurrent.IsEquals( ptCirCenter ) )
				return false;

			// 连接点P和圆心O，如果PO平行于X轴，则根据P在O的左边还是右边计算出最近点的横坐标为centerPoint.X - radius 或 centerPoint.X + radius
			if( DoubleEquals( ptCurrent.Y, ptCirCenter.Y ) )
			{
				if( ptCurrent.X < ptCirCenter.X )
					ptResult.Set( ptCirCenter.X - radius, ptCurrent.Y );
				else
					ptResult.Set( ptCirCenter.X + radius, ptCurrent.Y );
			}
				// 如果PO平行于Y轴，则根据P在O的上边还是下边计算出最近点的纵坐标为centerPoint.Y + radius或 centerPoint.Y - radius
			else if( DoubleEquals( ptCurrent.X, ptCirCenter.X ) )
			{
				if( ptCurrent.Y < ptCirCenter.Y )
					ptResult.Set( ptCurrent.X, ptCirCenter.Y - radius );
				else
					ptResult.Set( ptCurrent.X, ptCirCenter.Y + radius );

			}
				// 如果PO不平行于X轴和Y轴，则PO的斜率存在且不为0,联立两方程组可以解出直线PO和圆的交点，取其中离P点较近的交点即可
			else
			{
				double X1 =  Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptCurrent.Y - ptCirCenter.Y)/(ptCurrent.X - ptCirCenter.X), 2.0 ) ) ) + ptCirCenter.X; 
				double X2 = -Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptCurrent.Y - ptCirCenter.Y)/(ptCurrent.X - ptCirCenter.X), 2.0 ) ) ) + ptCirCenter.X; 
				double Y1 =  Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptCurrent.X - ptCirCenter.X)/(ptCurrent.Y - ptCirCenter.Y), 2.0 ) ) ) + ptCirCenter.Y; 
				double Y2 = -Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptCurrent.X - ptCirCenter.X)/(ptCurrent.Y - ptCirCenter.Y), 2.0 ) ) ) + ptCirCenter.Y;
				if( Distance( new CPointD( X1, Y1 ), ptCurrent ) < Distance( new CPointD( X2, Y2 ), ptCurrent ) )
					ptResult.Set( X1, Y1 );
				else
					ptResult.Set( X2, Y2 );
			}

			return true;
		}

		/// <summary>
		/// 线段和圆的交点。
		/// <code>
		/// 设圆心为O，圆半径为r，直线（或线段）L上的两点为P1,P2。 
		///
		/// 1. 如果L是线段且P1，P2都包含在圆O内，则没有交点；否则进行下一步。 
		///
		/// 2. 如果L平行于Y轴， 
		///		a) 计算圆心到L的距离dis；
		///		b) 如果dis > r 则L和圆没有交点；
		///		c) 利用勾股定理，可以求出两交点坐标，但要注意考虑L和圆的相切情况。
		///
		///	3. 如果L平行于X轴，做法与L平行于Y轴的情况类似； 
		///
		///	4. 如果L既不平行X轴也不平行Y轴，可以求出L的斜率K，然后列出L的点斜式方程，和圆方程联立即可求解出L和圆的两个交点； 
		///
		///	5. 如果L是线段，对于2，3，4中求出的交点还要分别判断是否属于该线段的范围内。
		///	
		///	注：
		///	直线P1,P2斜率为k = （P2.Y – P1.Y ）/ (P2.X – P1.X )
		///	直线P1,P2的方程为：y = k * ( x - P1.X) + P1.Y
		///	设圆心O方程为:(x - O.X ) ^2 + ( y - O.Y ) ^2 = r ^2
		///	求解方程组的结果交点为：
		///	X1 =  sqrt( r^2 / ( 1+( ( P1.X-P2.X )/( P2.Y-P1.Y) )^2 ) ) + O.X;
		///	X2 = -sqrt( r^2 / ( 1+( ( P1.X-P2.X )/( P2.Y-P1.Y) )^2 ) ) + O.X;
		///	Y1 =  sqrt( r^2 / ( 1+( ( P2.Y-P1.Y )/( P1.X-P2.X) )^2 ) ) + O.Y;
		///	Y2 = -sqrt( r^2 / ( 1+( ( P2.Y-P1.Y )/( P1.X-P2.X) )^2 ) ) + O.Y;
		///	</code>
		/// </summary>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <param name="ptCirCenter">圆心点</param>
		/// <param name="radius">圆半径</param>
		/// <param name="arrCrossResult">返回线段或直线与圆的交点数组</param>
		/// <returns>线段或直线与圆是否有交点</returns>
		public static bool CP_Sect2Circle( CPointD ptStart, CPointD ptEnd, CPointD ptCirCenter, double radius , out ArrayList arrCrossResult )
		{
			arrCrossResult 	= new ArrayList();
			CPointD ptCross = new CPointD( 0, 0 );

			// 如果L是线段且P1，P2都包含在圆O内，则没有交点
			if( IsPntInCircle( ptStart, ptCirCenter, radius ) && IsPntInCircle( ptEnd, ptCirCenter, radius ) )
				return false;

			double dbDistance = Dist_Pnt2Sect( ptCirCenter, ptStart, ptEnd );
			// 如果L平行于Y轴
			if( ptStart.X == ptEnd.X )
			{			
				// 计算圆心到L的距离dis,如果dis > r 则L和圆没有交点
				if( dbDistance > radius )
				{
					return false;
				}

				// 相切情况下交点只有一个，位于圆心左或右
				if ( dbDistance == radius )
				{
					ptCross = new CPointD( ptStart.X, ptCirCenter.Y );
					arrCrossResult.Add( ptCross );
				}
					// 否则有两个交点，利用勾股定理，可以求出两交点坐标[ Y1:sqrt( r^2 - distance^2 ),Y2:-sqrt( r^2 - distance^2 ) ]
				else
				{
					double Y = Math.Sqrt( Math.Pow( radius, 2.0 ) - Math.Pow( dbDistance, 2.0 ) );

					ptCross = new CPointD( ptStart.X, ptCirCenter.Y - Y );
					arrCrossResult.Add( ptCross );
					ptCross = new CPointD( ptStart.X, ptCirCenter.Y + Y );
					arrCrossResult.Add( ptCross );
				}

			}
				// 如果L平等于X轴
			else if( ptStart.Y == ptEnd.Y )
			{
				// 计算圆心到L的距离dis,如果dis > r 则L和圆没有交点
				if( dbDistance > radius )
				{
					return false;
				}

				// 相切情况下交点只有一个，位于圆心上或下
				if ( dbDistance == radius )
				{
					ptCross = new CPointD( ptCirCenter.X, ptStart.Y );
					arrCrossResult.Add( ptCross );
				}
					// 否则有两个交点，利用勾股定理，可以求出两交点坐标[ X1:sqrt( r^2 - distance^2 ),X2:-sqrt( r^2 - distance^2 ) ]
				else
				{
					double X	= Math.Sqrt( Math.Pow( radius, 2.0 ) - Math.Pow( dbDistance, 2.0 ) );

					ptCross		= new CPointD( ptCirCenter.X - X, ptStart.Y );
					arrCrossResult.Add( ptCross );
					ptCross		= new CPointD( ptCirCenter.X + X, ptStart.Y );
					arrCrossResult.Add( ptCross );
				}

			}
				// 如果L既不平行X轴也不平行Y轴，可以求出L的斜率K，然后列出L的点斜式方程，和圆方程联立即可求解出L和圆的两个交点
			else
			{
				double X1	= -Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptStart.X - ptEnd.X)/(ptEnd.Y - ptStart.Y), 2.0 ) ) ) + ptCirCenter.X; 
				double X2	=  Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptStart.X - ptEnd.X)/(ptEnd.Y - ptStart.Y), 2.0 ) ) ) + ptCirCenter.X;  
				double Y1	= -Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptEnd.Y - ptStart.Y)/(ptStart.X - ptEnd.X), 2.0 ) ) ) + ptCirCenter.Y;
				double Y2	=  Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptEnd.Y - ptStart.Y)/(ptStart.X - ptEnd.X), 2.0 ) ) ) + ptCirCenter.Y; 
			
				ptCross		= new CPointD( X1, Y1 );
				arrCrossResult.Add( ptCross );
				ptCross		= new CPointD( X2, Y2 );
				arrCrossResult.Add( ptCross );
			}

			// 如果L是线段，对于求出的交点还要分别判断是否属于该线段的范围内
			for( int nCount = 0; nCount < arrCrossResult.Count; ++nCount )
			{
				if( !IsPntOnSect( (CPointD)arrCrossResult[nCount], ptStart, ptEnd ) )
					return false;
			}
			
			return true;
		}		
		
		/// <summary>
		/// 计算两条直线的交点坐标
		/// </summary>
		/// <param name="ptAStart">直线1的坐标</param>
		/// <param name="ptAEnd"></param>
		/// <param name="ptBStart">直线2的坐标</param>
		/// <param name="ptBEnd"></param>
		/// <returns>两直线的交点坐标,若两线平行，返回CPointD.Empty</returns>
		public static CPointD CP_Line2Line( CPointD ptAStart, CPointD ptAEnd, CPointD ptBStart, CPointD ptBEnd )
		{
			// 不相交，置交点为CPointD.Empty
//			CPointD ptResult = CPointD.Empty;
//
//			if( !IsSectCrossSect( ptAStart, ptAEnd, ptBStart, ptBEnd ) )
//			{
//				// 不相交，置交点为CPointD.Empty
//				return ptResult;
//			}
//
//			// 求线段A与线段B的斜率Ka和Kb
//			double Ka = ( ptAEnd.Y - ptAStart.Y ) / ( ptAEnd.X - ptAStart.X );
//			double Kb = ( ptBEnd.Y - ptBStart.Y ) / ( ptBEnd.X - ptBStart.X );
//
//			// 线段A平行于Y轴
//			if( ptAStart.X == ptAEnd.X )
//			{
//				// 线段B也平行于Y轴
//				if( ptBStart.X == ptBEnd.X )
//				{
//					// 当两线段都平行于Y轴时，判断是否共线
//					if( IsPntOnSect( ptAStart, ptBStart, ptBEnd ) )
//					{
//						// 共线，并且点ptAStart在线段B中
//						ptResult = ptAStart;
//	
//					}
//					else if( IsPntOnSect( ptAEnd, ptBStart, ptBEnd ) )
//					{
//						// 共线，并且点ptAEnd在线段B中
//						ptResult = ptAEnd;
//						
//					}
//					else if( IsPntOnSect( ptBStart, ptAStart, ptAEnd ) )
//					{
//						// 共线，并且点ptBStart在线段A中
//						ptResult = ptBStart;
//						
//					} 
//					else if( IsPntOnSect( ptBEnd, ptAStart, ptAEnd ) )
//					{
//						// 共线，并且点ptBEnd在线段A中
//						ptResult = ptBEnd;
//						
//					} 
//					
//				}	// The end of "if( ptBStart.X == ptBEnd.X )"
//					// 线段B不平行于Y轴
//				else
//				{
//					// 若线段B不平行于Y轴，则交点横坐标为ptAStart的横坐标，代入到线段B的直线方程中可以计算出交点纵坐标
//					ptResult = new CPointD( ptAStart.X, (Kb * ( ptAStart.X - ptBStart.X) + ptBStart.Y) );
//				}
//
//				return ptResult;
//
//			}		// The end of "if( ptAStart.X == ptAEnd.X )"
//				// 线段A不平行于Y轴，线段B平行于Y轴
//			else if( ( ptAStart.X != ptAEnd.X ) && ( ptBStart.X == ptBEnd.X ) )
//			{
//				// 如果ptAStart和ptAEnd横坐标不同，但是ptBStart和ptBEnd横坐标相同，即线段B平行于Y轴，则交点横坐标为ptBStart
//				// 的横坐标，代入到线段A的直线方程中可以计算出交点纵坐标
//				ptResult = new CPointD( ptBStart.X, (Ka * ( ptBStart.X - ptAStart.X) + ptAStart.Y) );
//
//				return ptResult;
//			}
//
//
//			// 线段A平行于X轴
//			if( ptAStart.Y == ptAEnd.Y )
//			{
//				// 线段B也平行于X轴
//				if( ptBStart.Y == ptBEnd.Y )
//				{
//					// 当两线段都平行于X轴时，判断是否共线
//					if( IsPntOnSect( ptAStart, ptBStart, ptBEnd ) )
//					{
//						// 共线，并且点ptAStart在线段B中
//						ptResult = ptAStart;
//						
//					}
//					else if( IsPntOnSect( ptAEnd, ptBStart, ptBEnd ) )
//					{
//						// 共线，并且点ptAEnd在线段B中
//						ptResult = ptAEnd;
//						
//					}
//					else if( IsPntOnSect( ptBStart, ptAStart, ptAEnd ) )
//					{
//						// 共线，并且点ptBStart在线段A中
//						ptResult = ptBStart;
//						
//					} 
//					else if( IsPntOnSect( ptBEnd, ptAStart, ptAEnd ) )
//					{
//						// 共线，并且点ptBEnd在线段A中
//						ptResult = ptBEnd;
//						
//					} 
//					
//				}	// The end of "if( ptBStart.Y == ptBEnd.Y )"
//					// 线段B不平行于X轴
//				else
//				{
//					// 若线段B不平行于X轴，则交点纵坐标为ptAStart的纵坐标，代入到线段B的直线方程中可以计算出
//					// 交点横坐标(x = (y - ptBStart.Y + k*ptBStart.X) / k)
//					ptResult = new CPointD( ((ptAStart.Y - ptBStart.Y + Kb*ptBStart.X) / Kb), ptAStart.Y );
//					
//				}
//
//				return ptResult;
//
//			}		// The end of "if( ptAStart.Y == ptAEnd.Y )"
//				// 线段A不平行于X轴，线段B平行于X轴
//			else if( ( ptAStart.Y != ptAEnd.Y ) && ( ptBStart.Y == ptBEnd.Y ) )
//			{
//				// 如果ptAStart和ptAEnd纵坐标不同，但是ptBStart和ptBEnd纵坐标相同，即线段B平行于X轴，则交点纵坐
//				// 标为ptBStart的纵坐标，代入到线段A的直线方程中可以计算出交点横坐标(x = (y - ptAStart.Y + k*ptAStart.X) / k)
//				ptResult = new CPointD( ((ptBStart.Y - ptAStart.Y + Ka*ptAStart.X) / Ka), ptBStart.Y );
//				return ptResult;
//			}
//
//
//			// 斜率相等的情况
//			if( Ka == Kb )
//			{
//				if( IsPntOnSect( ptAStart, ptBStart, ptBEnd ) )
//				{
//					// 共线，并且点ptAStart在线段B中
//					ptResult = ptAStart;
//					
//				}
//				else if( IsPntOnSect( ptAEnd, ptBStart, ptBEnd ) )
//				{
//					// 共线，并且点ptAEnd在线段B中
//					ptResult = ptAEnd;
//					
//				}
//				else if( IsPntOnSect( ptBStart, ptAStart, ptAEnd ) )
//				{
//					// 共线，并且点ptBStart在线段A中
//					ptResult = ptBStart;
//					
//				} 
//				else if( IsPntOnSect( ptBEnd, ptAStart, ptAEnd ) )
//				{
//					// 共线，并且点ptBEnd在线段A中
//					ptResult = ptBEnd;
//					
//				} 
//
//				return ptResult;
//
//			}
//
//			// 剩下的情况就是线段A和线段B的斜率均存在且不为0的情况，联立两直线的方程组可以解出交点来，求得：
//			// x = ( Ka*ptAStart.X - Kb*ptBStart.X + ptBStart.Y - ptAStart.Y ) / (Ka + Kb);
//			// y = Kb* ( x - ptBStart.X) + ptBStart.Y;
//			double X = ( Ka*ptAStart.X - Kb*ptBStart.X + ptBStart.Y - ptAStart.Y ) / (Ka + Kb);
//			double Y = Kb* ( X - ptBStart.X) + ptBStart.Y;
//			ptResult = new CPointD( X, Y );
//		
//			return ptResult;

/*			double A1,B1,C1,A2,B2,C2;
			LineFomuleFrom2Pnt( pt1, pt2, out A1, out B1, out C1 );
			LineFomuleFrom2Pnt( pt1, pt2, out A2, out B2, out C2 );

			return CrossPointOf2Line( A1, B1, C1, A2, B2, C2 );
*/
			//郭正坤 2006-05-23 修改
			double A1,B1,C1,A2,B2,C2;
			LineFomuleFrom2Pnt( ptAStart, ptAEnd, out A1, out B1, out C1 );
			LineFomuleFrom2Pnt( ptBStart, ptBEnd, out A2, out B2, out C2 );

			return CrossPointOf2Line( A1, B1, C1, A2, B2, C2 );
		}

		/// <summary>
		/// 计算两条线段的交点坐标
		/// </summary>
		/// <param name="pt1">直线1的坐标pt1,pt2</param>
		/// <param name="pt2"></param>
		/// <param name="pt3">直线2的坐标pt3,pt4</param>
		/// <param name="pt4"></param>
		/// <param name="dT1"> 交点在直线1上的位置系数。</param>
		/// <param name="dT2">交点在直线1上的位置系数。</param>
		/// <returns>两线段的交点坐标</returns>
		public static CPointD CP_Sect2Sect( CPointD pt1, CPointD pt2, CPointD pt3, CPointD pt4, ref double dT1, ref double dT2 )
		{
			CPointD	tmpPt = new CPointD( 0.0, 0.0 );
		
			double A11 = ( pt2.X - pt1.X );
			double A21 = ( pt2.Y - pt1.Y );
			double A12 =-( pt4.X - pt3.X );
			double A22 =-( pt4.Y - pt3.Y );
			double B1  = ( pt3.X - pt1.X );
			double B2  = ( pt3.Y - pt1.Y );
			double	tmpVal = A11 * A22 - A12 * A21;
			if ( DoubleEquals(tmpVal,0.0) == true )
			{
				// 曲线平行
				tmpPt.X = (pt2.X + pt3.X) / 2.0f;
				tmpPt.Y = (pt2.Y + pt3.Y) / 2.0f;
				dT1		= 0.0;	dT2 = 0.0;
			}
			else
			{
				dT1		= (B1 * A22 - A12 * B2) / tmpVal; 
				dT2		= (A11 * B2 - A21 * B1) / tmpVal;
				tmpPt.X = pt1.X + dT1 * ( pt2.X - pt1.X );
				tmpPt.Y = pt1.Y + dT1 * ( pt2.Y - pt1.Y );
			}

			return tmpPt;
		}

		#endregion

		#region 杂项
		/// <summary>
		/// 判断两个double型数据是否相等,浮动范围在jGMC.EPSLN内
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		public static bool DoubleEquals(double value1,double value2)
		{
			return Math.Abs( value1 - value2) <= jGMC.EPSLN;
		}

		/// <summary>
		/// 求包容矩形
		/// </summary>
		/// <param name="Polyline">待求包容矩形的折线或多边形顶点的有序数组</param>
		/// <returns>包容的最小矩形</returns>
		public static CRectD BoundRect( IList Polyline )
		{
			double x1 = 0.0, y1 = 0.0, x2 = 0.0, y2 = 0.0;
			int nCount = 0;
			CPointD pnt;
			for( nCount=0; nCount<Polyline.Count; nCount++ )
			{
				pnt = Polyline[nCount] as CPointD;
				if ( pnt != null )
				{
					x1 = x2 = pnt.X;
					y1 = y2 = pnt.Y;
					break;
				}
			}
			for( int n=nCount; n<Polyline.Count; n++ )
			{
				pnt = Polyline[n] as CPointD;
				if ( pnt != null )
				{
					x1 = Math.Min( x1, pnt.X );
					y1 = Math.Min( y1, pnt.Y );
					x2 = Math.Max( x2, pnt.X );
					y2 = Math.Max( y2, pnt.Y );
				}
			}
			return CRectD.FromLTRB( x1, y1, x2, y2 );
		}

		/// <summary>
		/// 向量点积计算
		/// </summary>
		/// <param name="lhs">向量1</param>
		/// <param name="rhs">向量2</param>
		/// <returns>返回double值</returns>
		public static double DotProduce (CPointD lhs, CPointD rhs)
		{
			return lhs.X * rhs.X + lhs.Y * rhs.Y;
		}

		/// <summary>
		/// 向量叉积计算
		/// </summary>
		/// <param name="lhs">向量</param>
		/// <param name="rhs">向量</param>
		/// <returns>标量</returns>
		public static double CrossProduce (CPointD lhs, CPointD rhs)
		{
			return  lhs.X*rhs.Y - rhs.X*lhs.Y;
		}

		/// <summary>
		/// 由直线的两个点直线方程的一般式（Ax+By+C=0）的系数。
		/// </summary>
		/// <param name="pt1">直线上的一点</param>
		/// <param name="pt2">直线上的另一点</param>
		/// <param name="A">一般式系数A</param>
		/// <param name="B">一般式系数B</param>
		/// <param name="C">一般式系数C</param>
		public static void LineFomuleFrom2Pnt( CPointD pt1, CPointD pt2, out double A, out double B, out double C )
		{
			A = pt2.Y - pt1.Y;
			B = pt1.X - pt2.X;
			C = -pt1.X*A - pt1.Y*B;
		}

		/// <summary>
		/// 求点到直线的垂线的直线方程的一般式（Ax+By+C=0）的系数。
		/// </summary>
		/// <param name="pt">垂线通过的点</param>
		/// <param name="pt1">直线上的一点</param>
		/// <param name="pt2">直线上的另一点</param>
		/// <param name="A">一般式系数A</param>
		/// <param name="B">一般式系数B</param>
		/// <param name="C">一般式系数C</param>
		public static void LineFomuleOfPntVerticalToLine( CPointD pt, CPointD pt1, CPointD pt2, out double A, out double B, out double C )
		{
			A = pt1.X - pt2.X;
			B = pt1.Y - pt2.Y;
			C = -A*pt.X -B*pt.Y;
		}

		/// <summary>
		/// 求两条直线的交叉点
		/// </summary>
		/// <param name="A1">直线1的一般式系数A</param>
		/// <param name="B1">直线1的一般式系数B</param>
		/// <param name="C1">直线1的一般式系数C</param>
		/// <param name="A2">直线2的一般式系数A</param>
		/// <param name="B2">直线2的一般式系数B</param>
		/// <param name="C2">直线2的一般式系数C</param>
		/// <returns>如果两线相交，返回交叉点；如果不相交，返回CPointD.Empty</returns>
		public static CPointD CrossPointOf2Line( double A1, double B1, double C1, double A2, double B2, double C2 )
		{
			CPointD pt;
			double v = A1*B2 - A2*B1;
			if ( DoubleEquals( v, 0.0 ) )
				pt = CPointD.Empty;
			else
				pt = new CPointD( (B1*C2-B2*C1)/v, (C1*A2-C2*A1)/v );

			return pt;
		}

		#endregion

		#region 受保护或私有方法
		#endregion

		#region 内部类

		/// <exclude/>
		/// <summary>
		/// 用于点数组按X大小排序，如果X相等，比较Y大小
		/// </summary>
		protected class CPointDComparer : IComparer  
		{
			int IComparer.Compare( Object x, Object y )  
			{
				CPointD pt1 = x as CPointD;
				CPointD pt2 = y as CPointD;
				if ( pt1 == null || pt2 == null ) return 0;
				if ( pt1.IsEquals( pt2 ) ) return 0;
				if( pt1.X > pt2.X )
					return 1;
				else if( DoubleEquals( pt1.X, pt2.X) && pt1.Y > pt2.Y )
					return 1;
				else
					return -1;
			}
		}

		#endregion 

		#region 未处理
		/*		
				/// <summary>
				/// 计算矢量线段的正弦余弦值
				/// </summary>
				/// <param name="pt1">矢量线段的起点坐标</param>
				/// <param name="pt2">矢量线段的终点坐标</param>
				/// <param name="SinX">矢量线段的正弦值</param>
				/// <param name="CosX">矢量线段的余弦值</param>
				/// <returns>正常时返回true，出错时返回false</returns>
				public static bool CalcSinCos( CPointD pt1, CPointD pt2, out double SinX, out double CosX)
				{
					SinX = CosX = 0.0;
					double	len = Distance(pt1, pt2);
					if (DoubleEquals(len,0) == true)
					{//距离为0,即两个点重合,进行处理
						SinX = CosX = 0.0;
						return false;
					}
					SinX = (pt2.Y - pt1.Y) / len;
					CosX = (pt2.X - pt1.X) / len;
					return true;
				}
				/// <summary>
				/// 指定的基准点按给定的方向偏移len的新点的坐标
				/// </summary>
				/// <param name="pt">原始点坐标</param>
				/// <param name="len">偏移距离</param>
				/// <param name="SinX">偏移方向的正弦值</param>
				/// <param name="CosX">偏移方向的余弦值</param>
				/// <returns>偏移后的新坐标</returns>
				/// 备注:算法:设直线的基准点坐标为p(x0, y0)，直线的倾角为
				///		  alpha，插入点与基准点的距离为len，则：
				///			x = x0 + cos(alpha) * len;
				///			y = y0 + sin(alpha) * len;
				/// 
				public static CPointD OffsetPnt(CPointD pt, double len, double SinX, double CosX)
				{
					CPointD	tmpPt = new CPointD(0.0,0.0);
	
					double dX = len * CosX;
					double dY = len * SinX;
					tmpPt.X = pt.X + dX;
					tmpPt.Y = pt.Y + dY;
					return tmpPt;
				}
				/// <summary>
				/// 计算偏移
				/// </summary>
				/// <param name="orgPoint">原始点</param>
				/// <param name="OffsetAngle">偏移角度</param>
				/// <param name="OffsetH">切线防线的偏移量</param>
				/// <param name="OffsetV">法向方向的偏移量</param>
				/// <returns></returns>
				public static CPointD OffsetPntEx(CPointD orgPoint,double OffsetAngle,double OffsetH,double OffsetV)
				{
					double X0 = orgPoint.X;
					double Y0 = orgPoint.Y;
					double fAngle = OffsetAngle / 180.0 * Math.PI;//将偏移方向角度从度转换为弧度

					//计算沿切线偏移后的坐标
					X0 += OffsetH * Math.Cos(OffsetAngle / 180.0 * Math.PI);
					Y0 += OffsetH * Math.Sin(OffsetAngle / 180.0 * Math.PI);


					//计算法向方向偏移后的坐标
					X0 += OffsetV * Math.Sin(fAngle / 180 * Math.PI);
					Y0 += OffsetV * Math.Cos(fAngle / 180 * Math.PI);

					return new CPointD(X0,Y0);
				}
				/// <summary>
				///  旋转一个点
				/// </summary>
				/// <param name="ptOrg">指定的旋转原点</param>
				/// <param name="pt">待旋转的数据点</param>
				/// <param name="cosx">旋转角的余弦值</param>
				/// <param name="sinx">旋转角的正弦值</param>
				/// <returns>旋转后的坐标点</returns>
				public static CPointD RotatePoint( CPointD ptOrg, CPointD pt, double cosx, double sinx )
				{			
					CPointD v3		= pt - ptOrg;
					Size size		= new Size((int)v3.X,(int)v3.Y);			
					CPointD	tmpPt	= new CPointD(0,0);
					tmpPt.X			= size.Width * cosx - size.Height * sinx + ptOrg.X;
					tmpPt.Y			= size.Width * sinx + size.Height * cosx + ptOrg.Y;
					return tmpPt;
				}

				/// <summary>
				/// 计算两条共线的线段的交点：
				///  对于两条共线的线段，它们之间的位置关系有下图所示的几种情况。图(a)中两条线段没有交点；图 (b) 和 (d) 中两条线段有无穷焦点；
				///  图 (c) 中两条线段有一个交点。设line1是两条线段中较长的一条，line2是较短的一条，如果line1包含了line2的两个端点，则是图(d)
				///  的情况，两线段有无穷交点；如果line1只包含line2的一个端点，那么如果line1的某个端点等于被line1包含的line2的那个端点，则是图
				///  (c)的情况，这时两线段只有一个交点，否则就是图(b)的情况，两线段也是有无穷的交点；如果line1不包含line2的任何端点，则是图(a)
				///  的情况，这时两线段没有交点。  
				///  
				///  计算线段或直线与线段的交点: 
				///  设一条线段为L0 = P1P2，另一条线段或直线为L1 = Q1Q2 ，要计算的就是L0和L1的交点。
				///  1． 首先判断L0和L1是否相交（方法已在前文讨论过），如果不相交则没有交点，否则说明L0和L1一定有交点，下面就将L0和L1都看作直线来考虑。 
				///  
				///	 2． 如果P1和P2横坐标相同，即L0平行于Y轴 
				///		a) 若L1也平行于Y轴， 
				///			i. 若P1的纵坐标和Q1的纵坐标相同，说明L0和L1共线，假如L1是直线的话他们有无穷的交点，假如L1是线段的话可用"计算两条共线线段的交点"的算法求他们的交点（该方法在前文已讨论过）；
				///			ii. 否则说明L0和L1平行，他们没有交点； 
				///		b) 若L1不平行于Y轴，则交点横坐标为P1的横坐标，代入到L1的直线方程中可以计算出交点纵坐标； 
				///
				///	 3． 如果P1和P2横坐标不同，但是Q1和Q2横坐标相同，即L1平行于Y轴，则交点横坐标为Q1的横坐标，代入到L0的直线方程中可以计算出交点纵坐标； 
				///
				///	 4． 如果P1和P2纵坐标相同，即L0平行于X轴 
				///		a) 若L1也平行于X轴， 
				///			i. 若P1的横坐标和Q1的横坐标相同，说明L0和L1共线，假如L1是直线的话他们有无穷的交点，假如L1是线段的话可用"计算两条共线线段的交点"的算法求他们的交点（该方法在前文已讨论过）；
				///			ii. 否则说明L0和L1平行，他们没有交点； 
				///		b) 若L1不平行于X轴，则交点纵坐标为P1的纵坐标，代入到L1的直线方程中可以计算出交点横坐标； 
				///
				///	 5． 如果P1和P2纵坐标不同，但是Q1和Q2纵坐标相同，即L1平行于X轴，则交点纵坐标为Q1的纵坐标，代入到L0的直线方程中可以计算出交点横坐标； 
				///
				///	 6． 剩下的情况就是L1和L0的斜率均存在且不为0的情况 
				///	   a) 计算出L0的斜率K0，L1的斜率K1 ； 
				///    b) 如果K1 = K2  
				///			  i. 如果Q1在L0上，则说明L0和L1共线，假如L1是直线的话有无穷交点，假如L1是线段的话可用"计算两条共线线段的交点"的算法求他们的交点（该方法在前文已讨论过）；
				///			  ii. 如果Q1不在L0上，则说明L0和L1平行，他们没有交点。
				///	   c) 联立两直线的方程组可以解出交点来
				///	   
				///	   线段A：斜率为：Ka = ( ptAEnd.Y - ptAStart.Y ) / (ptAEnd.X - ptAStart.X );该直线方程为：y = Ka* ( x - ptAStart.X) + ptAStart.Y
				///	   线段B：斜率为：Kb = ( ptBEnd.Y - ptBStart.Y ) / (ptBEnd.X - ptBStart.X );该直线方程为：y = Kb* ( x - ptBStart.X) + ptBStart.Y
				/// </summary>
				/// <param name="ptAStart">线段A起始点</param>
				/// <param name="ptAEnd">线段A终止点</param>
				/// <param name="ptBStart">线段B起始点</param>
				/// <param name="ptBEnd">线段B终止点</param>
				/// <param name="ptResult">返回交点坐标(注：目前对于两线段相交时共线问题，有无穷交点情况下，只返回被包涵线段两端任意一点坐标。)</param>
				/// <returns>两线段是否有交点</returns>
				public static bool CP_Sect2Sect( CPointD ptAStart, CPointD ptAEnd, CPointD ptBStart, CPointD ptBEnd, out CPointD ptResult )
				{
					// 不相交，置交点为(0,0)
					ptResult = new CPointD( 0, 0 );

					if( !IsSectCrossSect( ptAStart, ptAEnd, ptBStart, ptBEnd ) )
					{
						// 不相交，置交点为(0,0)，返回假
						return false;
					}

					// 求线段A与线段B的斜率Ka和Kb
					double Ka = ( ptAEnd.Y - ptAStart.Y ) / ( ptAEnd.X - ptAStart.X );
					double Kb = ( ptBEnd.Y - ptBStart.Y ) / ( ptBEnd.X - ptBStart.X );

					// 线段A平行于Y轴
					if( ptAStart.X == ptAEnd.X )
					{
						// 线段B也平行于Y轴
						if( ptBStart.X == ptBEnd.X )
						{
							// 当两线段都平行于Y轴时，判断是否共线
							if( IsPntOnSect( ptAStart, ptBStart, ptBEnd ) )
							{
								// 共线，并且点ptAStart在线段B中
								ptResult = ptAStart;
								return true;
							}
							else if( IsPntOnSect( ptAEnd, ptBStart, ptBEnd ) )
							{
								// 共线，并且点ptAEnd在线段B中
								ptResult = ptAEnd;
								return true;
							}
							else if( IsPntOnSect( ptBStart, ptAStart, ptAEnd ) )
							{
								// 共线，并且点ptBStart在线段A中
								ptResult = ptBStart;
								return true;
							} 
							else if( IsPntOnSect( ptBEnd, ptAStart, ptAEnd ) )
							{
								// 共线，并且点ptBEnd在线段A中
								ptResult = ptBEnd;
								return true;
							} 
							else
							{
								// 不共线，平行没有交点
								return false;
							} 
						}	// The end of "if( ptBStart.X == ptBEnd.X )"
							// 线段B不平行于Y轴
						else
						{
							// 若线段B不平行于Y轴，则交点横坐标为ptAStart的横坐标，代入到线段B的直线方程中可以计算出交点纵坐标
							ptResult = new CPointD( ptAStart.X, (Kb * ( ptAStart.X - ptBStart.X) + ptBStart.Y) );
							return true;
						}
					}		// The end of "if( ptAStart.X == ptAEnd.X )"
						// 线段A不平行于Y轴，线段B平行于Y轴
					else if( ( ptAStart.X != ptAEnd.X ) && ( ptBStart.X == ptBEnd.X ) )
					{
						// 如果ptAStart和ptAEnd横坐标不同，但是ptBStart和ptBEnd横坐标相同，即线段B平行于Y轴，则交点横坐标为ptBStart
						// 的横坐标，代入到线段A的直线方程中可以计算出交点纵坐标
						ptResult = new CPointD( ptBStart.X, (Ka * ( ptBStart.X - ptAStart.X) + ptAStart.Y) );
						return true;
					}


					// 线段A平行于X轴
					if( ptAStart.Y == ptAEnd.Y )
					{
						// 线段B也平行于X轴
						if( ptBStart.Y == ptBEnd.Y )
						{
							// 当两线段都平行于X轴时，判断是否共线
							if( IsPntOnSect( ptAStart, ptBStart, ptBEnd ) )
							{
								// 共线，并且点ptAStart在线段B中
								ptResult = ptAStart;
								return true;
							}
							else if( IsPntOnSect( ptAEnd, ptBStart, ptBEnd ) )
							{
								// 共线，并且点ptAEnd在线段B中
								ptResult = ptAEnd;
								return true;
							}
							else if( IsPntOnSect( ptBStart, ptAStart, ptAEnd ) )
							{
								// 共线，并且点ptBStart在线段A中
								ptResult = ptBStart;
								return true;
							} 
							else if( IsPntOnSect( ptBEnd, ptAStart, ptAEnd ) )
							{
								// 共线，并且点ptBEnd在线段A中
								ptResult = ptBEnd;
								return true;
							} 
							else
							{
								// 不共线，平行没有交点
								return false;
							} 
						}	// The end of "if( ptBStart.Y == ptBEnd.Y )"
							// 线段B不平行于X轴
						else
						{
							// 若线段B不平行于X轴，则交点纵坐标为ptAStart的纵坐标，代入到线段B的直线方程中可以计算出
							// 交点横坐标(x = (y - ptBStart.Y + k*ptBStart.X) / k)
							ptResult = new CPointD( ((ptAStart.Y - ptBStart.Y + Kb*ptBStart.X) / Kb), ptAStart.Y );
							return true;
						}
					}		// The end of "if( ptAStart.Y == ptAEnd.Y )"
						// 线段A不平行于X轴，线段B平行于X轴
					else if( ( ptAStart.Y != ptAEnd.Y ) && ( ptBStart.Y == ptBEnd.Y ) )
					{
						// 如果ptAStart和ptAEnd纵坐标不同，但是ptBStart和ptBEnd纵坐标相同，即线段B平行于X轴，则交点纵坐
						// 标为ptBStart的纵坐标，代入到线段A的直线方程中可以计算出交点横坐标(x = (y - ptAStart.Y + k*ptAStart.X) / k)
						ptResult = new CPointD( ((ptBStart.Y - ptAStart.Y + Ka*ptAStart.X) / Ka), ptBStart.Y );
						return true;
					}


					// 斜率相等的情况
					if( Ka == Kb )
					{
						if( IsPntOnSect( ptAStart, ptBStart, ptBEnd ) )
						{
							// 共线，并且点ptAStart在线段B中
							ptResult = ptAStart;
							return true;
						}
						else if( IsPntOnSect( ptAEnd, ptBStart, ptBEnd ) )
						{
							// 共线，并且点ptAEnd在线段B中
							ptResult = ptAEnd;
							return true;
						}
						else if( IsPntOnSect( ptBStart, ptAStart, ptAEnd ) )
						{
							// 共线，并且点ptBStart在线段A中
							ptResult = ptBStart;
							return true;
						} 
						else if( IsPntOnSect( ptBEnd, ptAStart, ptAEnd ) )
						{
							// 共线，并且点ptBEnd在线段A中
							ptResult = ptBEnd;
							return true;
						} 
						else
						{
							// 不共线，平行没有交点
							return false;
						} 

					}

					// 剩下的情况就是线段A和线段B的斜率均存在且不为0的情况，联立两直线的方程组可以解出交点来，求得：
					// x = ( Ka*ptAStart.X - Kb*ptBStart.X + ptBStart.Y - ptAStart.Y ) / (Ka + Kb);
					// y = Kb* ( x - ptBStart.X) + ptBStart.Y;
					double X = ( Ka*ptAStart.X - Kb*ptBStart.X + ptBStart.Y - ptAStart.Y ) / (Ka + Kb);
					double Y = Kb* ( X - ptBStart.X) + ptBStart.Y;
					ptResult = new CPointD( X, Y );
		
					return true;

				}

				/// <summary>
				///  分别求与每条边的交点即可。 
				/// </summary>
				/// <param name="ptStart">线段起始点</param>
				/// <param name="ptEnd">线段终止点</param>
				/// <param name="Polyline">折线、矩形、多边形有序点数组</param>
				/// <param name="arrCrossResult">线段或直线与折线、矩形、多边形的交点数组</param>
				/// <returns>线段是否与折线、矩形、多边形有交点</returns>
				public static bool CP_Sect2Polygon( CPointD ptStart, CPointD ptEnd, ArrayList Polyline, out ArrayList arrCrossResult )
				{
					arrCrossResult = new ArrayList();
					CPointD ptCrossResult;
					for( int nCount = 1; nCount < Polyline.Count; ++nCount )
					{
						if( CP_Sect2Sect( ptStart, ptEnd, (CPointD)Polyline[nCount-1], (CPointD)Polyline[nCount], out ptCrossResult ) )
						{
							arrCrossResult.Add( ptCrossResult );
						}
					}

					if( arrCrossResult.Count == 0 )
						return false;
					else
						return true;
				}

			*/
		#endregion
	}
}
