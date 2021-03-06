using System;
using System.Drawing;
using System.Collections;

namespace Jurassic.Graph.Base
{

	/// <summary>
	/// CGeoAlgorithm 的摘要说明。
	/// 几何算法类,实现常用的几何运算
	/// 修改：徐景周，2005.12.20
	/// </summary>
	public class CGeoAlgorithm
	{
		const double PEI			= 3.1415926;
		public const double EPSL	= 10e-6;		// 浮点零的波动范围

		public CGeoAlgorithm()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 判断两个double型数据是否相等,浮动范围在EPSL内
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		public static bool Equals(double value1, double value2)
		{
			if( Math.Abs( value1 - value2) < EPSL )
				return true;
			else 
				return false;
		}

		/// <summary>
		/// 判断点pt1和pt2是否重合
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		public static bool Equals(CPointD pt1, CPointD pt2)
		{
			if( (Math.Abs( pt1.x - pt2.x ) < EPSL) &&
				(Math.Abs( pt1.y - pt2.y ) < EPSL) )
				return true;
			else 
				return false;
		}

		/// <summary>
		/// 计算矢量线段的正弦余弦值
		/// </summary>
		/// <param name="pt1">矢量线段的起点坐标</param>
		/// <param name="pt2">矢量线段的终点坐标</param>
		/// <param name="SinX">矢量线段的正弦值</param>
		/// <param name="CosX">矢量线段的余弦值</param>
		/// <returns>正常时返回true，出错时返回false</returns>
		public static bool CalcSinCos(CPointD pt1, CPointD pt2, out double SinX, out double CosX)
		{
			SinX = CosX = 0.0;
			double	len = Distance(pt1, pt2);
			if( Equals(len,0) == true )
			{
				// 距离为0,即两个点重合,进行处理
				SinX = CosX = 0.0;
				return false;
			}

			SinX = (pt2.y - pt1.y) / len;
			CosX = (pt2.x - pt1.x) / len;

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
	
			double dX	= len * CosX;
			double dY	= len * SinX;
			tmpPt.x		= pt.x + dX;
			tmpPt.y		= pt.y + dY;

			return tmpPt;
		}

		/// <summary>
		/// 计算两条直线的交点坐标
		/// </summary>
		/// <param name="pt1">直线1的坐标pt1,pt2</param>
		/// <param name="pt2"></param>
		/// <param name="pt3">直线2的坐标pt3,pt4</param>
		/// <param name="pt4"></param>
		/// <param name="dT1"> 交点在直线1上的位置系数。</param>
		/// <param name="dT2">交点在直线1上的位置系数。</param>
		/// <returns>两直线的交点坐标</returns>
		public static CPointD CP_Sect2Sect(CPointD pt1, CPointD pt2, CPointD pt3, CPointD pt4, ref double dT1, ref double dT2)
		{
			CPointD	tmpPt	= new CPointD(0.0f,0.0f);
		
			double A11		= (double)( pt2.x - pt1.x );
			double A21		= (double)( pt2.y - pt1.y);
			double A12		= -(double)( pt4.x - pt3.x );
			double A22		= -(double)( pt4.y - pt3.y );
			double B1		= (double)( pt3.x - pt1.x );
			double B2		= (double)( pt3.y - pt1.y );
			double	tmpVal	= A11 * A22 - A12 * A21;
			if (Equals(tmpVal,0) == true)
			{
				// 曲线平行
				tmpPt.x = (pt2.x + pt3.x) / 2.0f;
				tmpPt.y = (pt2.y + pt3.y) / 2.0f;
				dT1		= EPSL;	dT2 = EPSL;
			}
			else
			{
				dT1		= (B1 * A22 - A12 * B2) / tmpVal; 
				dT2		= (A11 * B2 - A21 * B1) / tmpVal;
				tmpPt.x = pt1.x + (float)( dT1 * ( pt2.x - pt1.x) );
				tmpPt.y = pt1.y + (float)( dT1 * ( pt2.y - pt1.y) );
			}

			return tmpPt;
		}

		/// <summary>
		/// 计算矢量线段pt1pt2的方向
		/// </summary>
		/// <param name="pt1">起始点坐标</param>
		/// <param name="pt2">结束点坐标</param>
		/// <returns>矢量线段pt1pt2的倾角,角度范围从0-360度</returns>
		public static double SectDirection(CPointD pt1,CPointD pt2)
		{
			double dblAngle = Math.Atan2(pt2.y - pt1.y,pt2.x - pt1.x);
			if(dblAngle < 0)
				dblAngle += 2 * Math.PI;
			dblAngle = dblAngle / Math.PI * 180;
			return 	dblAngle;
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
			double X0 = orgPoint.x;
			double Y0 = orgPoint.y;
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
		/// 计算线段pt1pt2与线段pt2pt3的切线方向
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <param name="pt3"></param>
		/// <returns>切线与水平方向夹角,单位度</returns>
		public static double TanDirection(CPointD pt1,CPointD pt2,CPointD pt3)
		{
			double dblAngle1 = SectDirection(pt1,pt2);
			double dblAngle2 = SectDirection(pt3,pt2);

			double dblRet = SectDirection(pt2,pt3)-(dblAngle2  - dblAngle1) / 2  + 90 ;
			while(dblRet < 0)
				dblRet +=360;
			while(dblRet > 360)
				dblRet -=360;
			return 	dblRet;
		}

		// --------------------------------------------------------------------------------------------------------------------

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
			Size size		= new Size((int)v3.x,(int)v3.y);			
			CPointD	tmpPt	= new CPointD(0,0);
			tmpPt.x			= size.Width * cosx - size.Height * sinx + ptOrg.x;
			tmpPt.y			= size.Width * sinx + size.Height * cosx + ptOrg.y;
			return tmpPt;
		}

		/// --------------------------------------------------------------------------
		/// <summary>
		/// 向量点积计算
		/// </summary>
		/// <param name="lhs">向量1</param>
		/// <param name="rhs">向量2</param>
		/// <returns>返回double值</returns>
		public static double DotProduce (CPointD lhs, CPointD rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y;
		}

		/// <summary>
		/// 向量叉积计算
		/// </summary>
		/// <param name="lhs">向量</param>
		/// <param name="rhs">向量</param>
		/// <returns>标量</returns>
		public static double CrossProduce (CPointD lhs, CPointD rhs)
		{
			return  lhs.x*rhs.y - rhs.x*lhs.y;
		}

		/// <summary>
		/// 计算两点pt1,pt2之间的距离平方。有时判断长短，不用开方即可。
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns>计算两点pt1,pt2之间的距离平方</returns>
		public static double Distance2(CPointD pt1,CPointD pt2)
		{
			double dblLength = 0.0f;
			dblLength = (pt1.y - pt2.y) * (pt1.y - pt2.y) + (pt1.x - pt2.x) *(pt1.x - pt2.x);

			return dblLength;

		}
		/// --------------------------------------------------------------------------

		/// <summary>
		/// 计算两点pt1,pt2之间的距离
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		public static double Distance(CPointD pt1,CPointD pt2)
		{
			double dblLength = 0.0f;
			dblLength = Math.Sqrt( (pt1.y - pt2.y) * (pt1.y - pt2.y) + (pt1.x - pt2.x) *(pt1.x - pt2.x) );

			return dblLength;

		}

		/// <summary>
		/// 计算两点pt1,pt2之间的距离
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		public static double Distance(PointF pt1,PointF pt2)
		{
			double dblLength = 0.0f;
			dblLength = Math.Sqrt((pt1.Y - pt2.Y) * (pt1.Y - pt2.Y) +
				(pt1.X - pt2.X) *(pt1.X - pt2.X));

			return dblLength;
		}

		/// <summary>
		/// 计算两点(x1,y1),(x2,y2)之间的距离
		/// 计算两点
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <returns></returns>
		public static double Distance(double x1,double y1,double x2,double y2)
		{
			double dblLength = 0.0;
			dblLength = Math.Sqrt((y2 - y1) * (y2 - y1) +
				(x2 - x1) *
				(x2 - x1));

			return dblLength;
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
/*			double	a, b, c;
			double	dist;
			if ( ptStart == ptEnd )		// 两点重合
				return 0;

			// 计算两点间距离
			dist	= Distance( ptStart, ptEnd );
			a		= ( ptEnd.y - ptStart.y ) / dist;
			b		= ( ptStart.x - ptEnd.x ) / dist;
			c		= ( ptEnd.x * ptStart.y - ptStart.x * ptEnd.y );

			return ( a * ptCurrent.x + b * ptCurrent.y + c );
*/

			return Dist_Pnt2Sect( ptCurrent, ptStart, ptEnd );
		}

		/// <summary>
		///  利用CP_Pnt2Sect()涵数求出点到线段的最近交点，然后再求出两点间的距离既可
		/// </summary>
		/// <param name="ptCurrent">待计算的目标点</param>
		/// <param name="ptStart">直线起点</param>
		/// <param name="ptEnd">直线终点</param>
		/// <returns>点到线段的最近距离</returns>
		public static double Dist_Pnt2Sect( CPointD ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			CPointD ptCrossNear = CP_Pnt2Sect( ptCurrent, ptStart, ptEnd );

			return Distance( ptCurrent, ptCrossNear );
		}

		/// <summary>
		///  利用CP_Pnt2Polygon()涵数求出点到折线、曲线、多边形各线段中的最小的距离返回既可，在内部共点时返回0
		/// </summary>
		/// <param name="ptCurrent">待计算的目标点</param>
		/// <param name="arrPolyline">折线、矩形、多边形有序点数组</param>
		/// <returns>点与折线、曲线、多边形的最近距离</returns>
		public static double Dist_Pnt2Polyline( CPointD ptCurrent, ArrayList arrPolyline )
		{
			// 点是否在线段上，是返回0
			for( int nCount	= 1; nCount < arrPolyline.Count; ++nCount )
			{
				if( IsPntOnSect( ptCurrent, (CPointD)arrPolyline[nCount-1], (CPointD)arrPolyline[nCount] ) )
					return 0;
			}

			CPointD ptCrossNear = CP_Pnt2Polygon( ptCurrent, arrPolyline );

			return Distance( ptCurrent, ptCrossNear );
		}

		/// <summary>
		/// 折线段的拐向判断
		/// </summary>
		/// <param name="ptStart">折线段的第一个端点</param>
		/// <param name="ptCorner">折线段的拐点</param>
		/// <param name="ptEnd">折线段的第二个端点</param>
		/// <returns>返回值： 0 – 拐向在右侧； 1 – 拐向在左侧； 2 – 没有拐向，三点共线； -1 – 未知。</returns>
		public static int PolylineCorner( CPointD ptStart, CPointD ptCorner, CPointD ptEnd )
		{
			// 若(p2 - p0) × (p1 - p0) > 0,则p0p1在p1点拐向右侧后得到p1p2。
			// 若(p2 - p0) × (p1 - p0) < 0,则p0p1在p1点拐向左侧后得到p1p2。
			// 若(p2 - p0) × (p1 - p0) = 0,则p0、p1、p2三点共线。	
			double CrossData = CrossProduce( (ptEnd - ptStart), (ptCorner - ptStart) );
			if( CrossData > 0 )
			{
				return 0;			// p0p1在p1点拐向右侧后得到p1p2。
			}
			else if( CrossData < 0 )
			{
				return 1;			// p0p1在p1点拐向左侧后得到p1p2。
			}
			else if( CrossData == 0 )
			{
				return 2;			// p0、p1、p2三点共线。
			}
			else
				return -1;
		}

		/// <summary>
		/// 利用PolylineCorner()拐向判断涵数，来判断点在直线那一边
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="ptStart">直线起始点</param>
		/// <param name="ptEnd">直线终止点</param>
		/// <returns>返回值： 0 – 在右侧； 1 – 在左侧； 2 – 三点共线； -1 – 未知。</returns>
		public static int PntOnLineSide( CPointD  ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			return PolylineCorner( ptStart, ptCurrent, ptEnd );
		}

		/// <summary>
		/// 先找出点到曲线数组中那一线段距离最近，再利用PolylineCorner()判断点在线段那一边
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="arrPolyline">曲线有序点数组</param>
		/// <returns>返回值： 0 – 在右侧； 1 – 在左侧； 2 – 三点共线； -1 – 未知。</returns>
		public static int PntOnCurveSide( CPointD  ptCurrent, ArrayList arrPolyline )
		{
			// 寻找最近距离的线段的两端点
			CPointD ptStartNear = (CPointD)arrPolyline[0];
			CPointD ptEndNear	= (CPointD)arrPolyline[1];
			for( int nCount	= 2; nCount < arrPolyline.Count; ++nCount )
			{
				if( Dist_Pnt2Sect( ptCurrent, ptStartNear, ptEndNear ) > Dist_Pnt2Sect( ptCurrent, (CPointD)arrPolyline[nCount-1], (CPointD)arrPolyline[nCount] ) )
				{
					ptStartNear = (CPointD)arrPolyline[nCount-1];
					ptEndNear	= (CPointD)arrPolyline[nCount];
				}
			}

			// 判断拐向
			return PolylineCorner( ptStartNear, ptCurrent, ptEndNear );
		}

		/// <summary>
		/// 点是否在直线上
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="ptStart">直线上的起始点2</param>
		/// <param name="ptEnd">直线上的终止点</param>
		/// <returns>点是否在直线上</returns>
		public static bool  IsPntOnLine( CPointD  ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			if( CrossProduce(ptCurrent - ptStart,ptEnd - ptStart) == 0 )
				return	true;
			else
				return false;
		}

		/// <summary>
		///  点是否在线段上
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="ptStart">线段上的起始点2</param>
		/// <param name="ptEnd">线段上的终止点</param>
		/// <returns>点是否在线段上</returns>
		public static bool  IsPntOnSect( CPointD  ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			if( IsPntOnLine(ptCurrent , ptStart ,ptEnd) )
			{
				if( (Math.Min(ptStart.x,ptEnd.x) <= ptCurrent.x ) && (ptCurrent.x <= Math.Max(ptStart.x,ptEnd.x) ) && (Math.Min(ptStart.y,ptEnd.y) <= ptCurrent.y) && (ptCurrent.y <= Math.Max(ptStart.y,ptEnd.y)) )
				{
					return true;
				}
　				else 
				{
					return false;
				}
			}
			else
			{
				return false;
			}
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
			if( CrossProduce((ptAStart - ptBStart),(ptBEnd  - ptBStart)) * CrossProduce((ptBEnd  - ptBStart),(ptAEnd - ptBStart)) >= 0 )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

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
			if( CrossProduce(ptAStart - ptBStart, ptBEnd - ptBStart) * CrossProduce(ptBEnd - ptBStart, ptAEnd - ptBStart) >= 0 &&
				CrossProduce(ptBStart - ptAStart, ptAEnd - ptAStart) * CrossProduce(ptAEnd - ptAStart, ptBEnd - ptAStart) >= 0 )
			{
				return true;
			}
			else
			{
				return false;
			}	
		}

		/// <summary>
		/// 对无限延伸的两直线来说，只要两直线不是平行线，必要交点，所以只需判断两直线是否平行既可
		/// </summary>
		/// <param name="ptAStart">直线A的起始点</param>
		/// <param name="ptAEnd">直线A的终止点</param>
		/// <param name="ptBStart">直接B起始点</param>
		/// <param name="ptBEnd">直线B终止点</param>
		/// <returns>直线与直线是否相交</returns>
		public static bool IsLineCrossLine( CPointD ptAStart, CPointD ptAEnd, CPointD ptBStart, CPointD ptBEnd )
		{
			// 求直线A与直线B的斜率Ka和Kb
			double Ka = ( ptAEnd.y - ptAStart.y ) / ( ptAEnd.x - ptAStart.x );
			double Kb = ( ptBEnd.y - ptBStart.y ) / ( ptBEnd.x - ptBStart.x );

			// 斜率相等的情况，两直线平行
			if( Ka == Kb )
			{
				if( IsPntOnSect( ptAStart, ptBStart, ptBEnd ) )
				{
					// 共线，并且点ptAStart在直线B中
					return true;
				}
				else if( IsPntOnSect( ptAEnd, ptBStart, ptBEnd ) )
				{
					// 共线，并且点ptAEnd在直线B中
					return true;
				}
				else if( IsPntOnSect( ptBStart, ptAStart, ptAEnd ) )
				{
					// 共线，并且点ptBStart在直线A中
					return true;
				} 
				else if( IsPntOnSect( ptBEnd, ptAStart, ptAEnd ) )
				{
					// 共线，并且点ptBEnd在直线A中
					return true;
				} 
				else
				{
					// 不共线，平行没有交点
					return false;
				} 

			}

			// 其它两直线不平行情况，有交点
			return true;
		}

		/// <summary>
		/// 利用CP_Sect2Polygon()涵数分别求折线中每一线段是否与多边形存在交点
		/// </summary>
		/// <param name="arrPolylineA">(折线、曲线、多边形)A有序点数组</param>
		/// <param name="arrPolylineB">(折线、曲线、多边形)B有序点数组</param>
		/// <returns>折线、曲线、多边形是否与另一个折线、曲线、多边形相交，存在返回真，否则返回假</returns>
		public static bool IsPolylineCrossPolyline( ArrayList arrPolylineA, ArrayList arrPolylineB )
		{
			ArrayList ptsCrossResult;
			for( int nCount = 1; nCount < arrPolylineA.Count; ++nCount )
			{
				// 线段有交点
				if( CP_Sect2Polygon( (CPointD)arrPolylineA[nCount-1], (CPointD)arrPolylineA[nCount], arrPolylineB, out ptsCrossResult ) )
				{
					return true;
				}
			}

			// 无交点
			return false;
		}

		/// <summary>
		/// 判断点是否在矩形内部
		/// </summary>
		/// <param name="ptCurrent">点</param>
		/// <param name="rectf">矩形</param>
		/// <returns>矩形是否包含点</returns>
		public static bool IsPntInRect( CPointD ptCurrent, RectangleF rectf )
		{
			//只要判断该点的横坐标和纵坐标是否夹在矩形的左右边和上下边之间
			if( ptCurrent.x >= Math.Min(rectf.Left,rectf.Right) && ptCurrent.x <= Math.Max(rectf.Left,rectf.Right)
				&& ptCurrent.y <= Math.Max(rectf.Top,rectf.Bottom) && ptCurrent.y >= Math.Min(rectf.Top, rectf.Bottom) )
			{
				return  true;
			}
			else
			{
				return false;
			}			
		}

		/// <summary>
		/// 判断线段是否在矩形内部
		/// </summary>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <param name="rectf">矩形</param>
		/// <returns>是否在矩形内</returns>
		public static bool IsSectInRect(  CPointD ptStart, CPointD ptEnd, RectangleF rectf )
		{
					
			if( IsPntInRect(ptStart, rectf) == false )
				return false;

			if( IsPntInRect(ptEnd, rectf) == false )
				return false;

			return true;			
		}

		/// <summary>
		/// 判断线段、折线、多边形是否在矩形内部	
		/// </summary>
		/// <param name="PointAarry">点数组</param>
		/// <param name="rectf">矩形</param>
		/// <returns>是否在矩形内</returns>
		public static bool IsPolylineInRect( ArrayList arrPolyline, RectangleF rectf )
		{
					
			foreach( CPointD p in arrPolyline )
			{
				if( IsPntInRect(p,rectf) == false )
					return false;
			}

			return true;			
		}

		/// <summary>
		/// 判断矩形是否在另一个矩形内部
		/// </summary>
		/// <param name="rectA">矩形A</param>
		/// <param name="rectB">矩形B</param>
		/// <returns>矩形rectB是否在矩形rectA内部</returns>
		public static bool IsRectInRect( RectangleF rectA, RectangleF rectB )
		{
			if( rectA.Contains(rectB) == true )
			{
				return true;	// rectB完全包含在rectA内
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 判断圆是否在矩形内部
		/// </summary>
		/// <param name="CirCenter">圆心</param>
		/// <param name="r">圆半径</param>
		/// <param name="rectf">矩形</param>
		/// <returns>矩形是否包含圆</returns>
		public static bool IsCircleInRect( CPointD CirCenter, double radius, RectangleF rectf )
		{
			// 圆在矩形中的充要条件是：圆心在矩形中且圆的半径小于等于圆心到矩形四边的距离的最小值
			if( IsPntInRect( CirCenter, rectf ) == true )
			{
				double distanceTop		= Dist_Pnt2Line( CirCenter, new CPointD(rectf.Left,rectf.Top),new CPointD(rectf.Right,rectf.Top) );
				double distanceLeft		= Dist_Pnt2Line( CirCenter, new CPointD(rectf.Left,rectf.Top),new CPointD(rectf.Left,rectf.Bottom) );
				double distanceBottom	= Dist_Pnt2Line( CirCenter, new CPointD(rectf.Left,rectf.Bottom),new CPointD(rectf.Right,rectf.Bottom) );
				double distanceRight	= Dist_Pnt2Line( CirCenter, new CPointD(rectf.Right,rectf.Bottom),new CPointD(rectf.Right,rectf.Top) );
				double miner1			= Math.Min( distanceTop,distanceLeft );
				double miner2			= Math.Min( distanceBottom,distanceRight );
				double miner			= Math.Min( miner1,miner2 );
				if( radius <= miner )
				{
					return true;
				}
				else
				{
					return false;
				}


			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// 判断点是否在多边形中:
		/// 以点P为端点，向左方作射线L，由于多边形是有界的，所以射线L的左端一定在多边形外，	
		/// 考虑沿着L从无穷远处开始自左向右移动，遇到和多边形的第一个交点的时候，进入
		/// 到了多边形的内部，遇到第二个交点的时候，离开了多边形，……
		/// 所以很容易看出当L和多边形的交点数目C是奇数的时候，P在多边形内，是偶数的话P在多边形外。
		/// </summary>
		/// <param name="ptCurrent">点p</param>
		/// <param name="arrPolygon">多边形顶点的有序数组</param>
		/// <returns>点是否在多边形区域内</returns>
		public static bool IsPntInPolygon( CPointD ptCurrent, ArrayList arrPolygon )
		{
			CPointD L		= new CPointD( ptCurrent.x*10000, ptCurrent.y );
			int CrossCont	= 0;
			for( int nCount	= 1; nCount < arrPolygon.Count; ++nCount )
			{
				if( IsPntOnSect( ptCurrent, (CPointD)arrPolygon[nCount], (CPointD)arrPolygon[nCount-1] ) )
				{
					return true;
				}

				if( ((CPointD)arrPolygon[nCount]).y != ((CPointD)arrPolygon[nCount-1]).y )
				{
					if( IsPntOnSect( (CPointD)arrPolygon[nCount], ptCurrent, L) && ((CPointD)arrPolygon[nCount]).y >((CPointD)arrPolygon[nCount-1]).y )
					{
						CrossCont++;
					}
					else if( IsPntOnSect((CPointD)arrPolygon[nCount-1], ptCurrent, L) && ((CPointD)arrPolygon[nCount-1]).y >((CPointD)arrPolygon[nCount]).y )
					{
						CrossCont++;
					}
					else if( IsSectCrossSect(ptCurrent,L,(CPointD)arrPolygon[nCount-1],(CPointD)arrPolygon[nCount]) )
					{
						CrossCont++;
					}
				}
			}

			if( CrossCont % 2 == 1 )
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		// --------------------------------------------------------------------------------------------------------------------

		// ---------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 线段在多边形内的一个必要条件是线段的两个端点都在多边形内，但由于多边形可能为凹，
		/// 所以这不能成为判断的充分条件。如果线段和多边形的某条边内交（两线段内交是指两线段相交且交点不在两线段的端点），
		/// 因为多边形的边的左右两侧分属多边形内外不同部分，所以线段一定会有一部分在多边形外。于是我们得到线段在多边
		/// 形内的第二个必要条件：线段和多边形的所有边都不内交。线段和多边形交于线段的两端点并不会影响线段是否在多边形内；
		/// 但是如果多边形的某个顶点和线段相交，还必须判断两相邻交点之间的线段是否包含于多边形内部。 因此我们可以先求出所
		/// 有和线段相交的多边形的顶点，然后按照X-Y坐标排序(X坐标小的排在前面，对于X坐标相同的点，Y坐标小的排在前面，这种
		/// 排序准则也是为了保证水平和垂直情况的判断正确)，这样相邻的两个点就是在线段上相邻的两交点，如果任意相邻两点的中
		/// 点也在多边形内，则该线段一定在多边形内。
		/// </summary>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <param name="arrPolygon">多边形顶点的有序数组</param>
		/// <returns>线段是否在多边形区域内</returns>
		public static bool IsSectInPolygon( CPointD ptStart, CPointD ptEnd, ArrayList arrPolygon )
		{
			// 两端点是否在多边形中
			if( !IsPntInPolygon( ptStart, arrPolygon ) || !IsPntInPolygon( ptEnd, arrPolygon ) )
			{
				return false;
			}

			ArrayList ptsIntersect = new ArrayList();
			for( int nCount	= 1; nCount < arrPolygon.Count; ++nCount )
			{
				// 线段的某个端点在s上
				if( IsPntOnSect( ptStart, (CPointD)arrPolygon[nCount], (CPointD)arrPolygon[nCount - 1] ) )
				{
					ptsIntersect.Add( ptStart );
				}
				else if( IsPntOnSect( ptEnd, (CPointD)arrPolygon[nCount], (CPointD)arrPolygon[nCount - 1] ) )
				{
					ptsIntersect.Add( ptEnd );
				}
					// s的某个端点在线段PQ上
				else if( IsPntOnSect( (CPointD)arrPolygon[nCount], ptStart, ptEnd  ) )
				{
					ptsIntersect.Add( (CPointD)arrPolygon[nCount] );
				}
				else if( IsPntOnSect( (CPointD)arrPolygon[nCount - 1], ptStart, ptEnd ) )
				{
					ptsIntersect.Add( (CPointD)arrPolygon[nCount - 1] );
				}
					// s和线段PQ相交,这时候已经可以肯定是内交了
				else if( IsSectCrossSect( ptStart, ptEnd, (CPointD)arrPolygon[nCount-1], (CPointD)arrPolygon[nCount] ) )
				{
					return false;
				}

			}

			// 将ptsIntersect中的点按照X-Y坐标排序;
			for( int nCount	= 1; nCount < ptsIntersect.Count; ++nCount )
			{
				if( ((CPointD)ptsIntersect[nCount - 1]).x > ((CPointD)ptsIntersect[nCount]).x )
				{
					ptsIntersect.Reverse( nCount-1, 2 );
				}
				else if( ((CPointD)ptsIntersect[nCount - 1]).x == ((CPointD)ptsIntersect[nCount]).x && ((CPointD)ptsIntersect[nCount - 1]).y > ((CPointD)ptsIntersect[nCount]).y )
				{
					ptsIntersect.Reverse( nCount-1, 2 );
				}

			}

			// 判断ptsIntersect中每两个相邻点的中点是否在多边形中
			CPointD ptMiddle;
			for( int nCount	= 1; nCount < ptsIntersect.Count; ++nCount )
			{
				ptMiddle	= new CPointD( (((CPointD)ptsIntersect[nCount - 1]).x + ((CPointD)ptsIntersect[nCount]).x)/2, (((CPointD)ptsIntersect[nCount - 1]).y + ((CPointD)ptsIntersect[nCount]).y)/2 );
				if( !IsPntInPolygon( ptMiddle, arrPolygon ) )
				{
					return false;
				}
			}

			return true;
		}
		
		/// <summary>
		/// 只要判断折线的每条线段是否都在多边形内即可。设折线有m条线段，多边形有n个顶点，则该算法的时间复杂度为O(m*n)。
		/// </summary>
		/// <param name="arrPolyline">折线顶点有序数组</param>
		/// <param name="arrPolygon">多边形顶点的有序数组</param>
		/// <returns>折线是否在多边形区域内</returns>
		public static bool IsPolylineInPolygon( ArrayList arrPolyline, ArrayList arrPolygon )
		{
			for( int nCount	= 1; nCount < arrPolyline.Count; ++nCount )
			{
				if( !IsSectInPolygon( (CPointD)arrPolyline[nCount], (CPointD)arrPolyline[nCount - 1], arrPolygon ) )
					return false;
			}

			return true;
		}
		
		/// <summary>
		/// 只要判断多边形的每条边是否都在多边形内即可。判断一个有m个顶点的多边形是否在一个有n个顶点的多边形内复杂度为O(m*n)。 
		/// </summary>
		/// <param name="arrPolygonA">多边形A顶点有序数组</param>
		/// <param name="arrPolygonB">多边形B顶点的有序数组</param>
		/// <returns>多边形是否在多边形区域内</returns>
		public static bool IsPolygonInPolygon( ArrayList arrPolygonA, ArrayList arrPolygonB )
		{
			for( int nCount	= 1; nCount < arrPolygonA.Count; ++nCount )
			{
				if( !IsSectInPolygon( (CPointD)arrPolygonA[nCount], (CPointD)arrPolygonA[nCount - 1], arrPolygonB ) )
					return false;
			}

			return true;
		}

		/// <summary>
		/// 将矩形转化为多边形，然后再判断是否在多边形内。
		/// </summary>
		/// <param name="rectf">矩形</param>
		/// <param name="arrPolygon">多边形顶点的有序数组</param>
		/// <returns>矩形是否在多边形区域内</returns>
		public static bool IsRectInPolygon( RectangleF rectf, ArrayList arrPolygon )
		{
			// 将矩形四点转换为多边形顶点数组
			ArrayList	ptsRect = new ArrayList();
			ptsRect.Add( new CPointD( rectf.Left,	rectf.Top ) );
			ptsRect.Add( new CPointD( rectf.Right,	rectf.Top ) );
			ptsRect.Add( new CPointD( rectf.Right,	rectf.Bottom ) );
			ptsRect.Add( new CPointD( rectf.Left,	rectf.Bottom ) );

			// 判断多边形是否在多边形中
			return IsPolygonInPolygon( ptsRect, arrPolygon );

		}

		/// <summary>
		/// 只要圆心在多边形中，并且计算圆心到多边形的每条边的最短距离，如果该距离大于等于圆半径则该圆在多边形内。
		/// </summary>
		/// <param name="CirCentre">圆心</param>
		/// <param name="radius">圆半径</param>
		/// <param name="arrPolygon">多边形顶点的有序数组</param>
		/// <returns>圆是否在多边形区域内</returns>
		public static bool IsCircleInPolygon( CPointD CirCenter, double radius, ArrayList arrPolygon )
		{
			// 圆心是否在多边形内
			if( !IsPntInPolygon( CirCenter, arrPolygon ) )
			{
				return false;
			}

			// 计算圆心到多边形各边的最短距离
			double dbMin	= double.MaxValue;
			for( int nCount = 1; nCount < arrPolygon.Count; ++nCount )
			{
				double dbDistance = Dist_Pnt2Sect( CirCenter, (CPointD)arrPolygon[nCount], (CPointD)arrPolygon[nCount-1] );

				if( dbMin > dbDistance )
					dbMin	= dbDistance; 

			}

			// 判断最短距离是否大于等于圆半径
			if( dbMin >= radius )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 计算圆心到该点的距离，如果小于等于半径则该点在圆内。 
		/// </summary>
		/// <param name="ptCurrent">判断点坐标</param>
		/// <param name="CirCentre">圆心</param>
		/// <param name="radius">圆半径</param>
		/// <returns>点是否在圆内</returns>
		public static bool IsPntInCircle( CPointD ptCurrent, CPointD CirCenter, double radius )
		{
			// 圆心到判断点距离
			double dbDistance = Distance( ptCurrent, CirCenter );

			if( dbDistance <= radius )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 因为圆是凸集，所以只要判断是否每个顶点都在圆内即可。 
		/// </summary>
		/// <param name="PointArray">线段、折线、多边形的有序点数组坐标</param>
		/// <param name="CirCentre">圆心</param>
		/// <param name="radius">圆半径</param>
		/// <returns>线段、折线、多边形是否在圆内</returns>
		public static bool IsLineInCircle( ArrayList arrPolyline, CPointD CirCenter, double radius )
		{
			for( int nCount = 0; nCount < arrPolyline.Count; ++nCount )
			{
				if( !IsPntInCircle( (CPointD)arrPolyline[nCount], CirCenter, radius ) )
					return false;

			}

			return true;
		}

		/// <summary>
		/// 设两圆为O1,O2，半径分别为r1, r2，要判断O2是否在O1内。先比较r1，r2的大小，
		/// 如果r1<r2则O2不可能在O1内；否则如果两圆心的距离大于r1 - r2 ，则O2不在O1内；否则O2在O1内。
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

		
		/// <summary>
		///  如果该线段平行于X轴（Y轴），则过点point作该线段所在直线的垂线，垂足很容易求得，然后计算出垂足，
		///  如果垂足在线段上则返回垂足，否则返回离垂足近的端点；如果该线段不平行于X轴也不平行于Y轴，则斜率
		///  存在且不为0。设线段的两端点为pt1和pt2，斜率为：k = ( pt2.y - pt1. y ) / (pt2.x - pt1.x );该直线
		///  方程为：y = k* ( x - pt1.x) + pt1.y。其垂线的斜率为 - 1 / k，垂线方程为：y = (-1/k) * (x - point.x) + point.y。
		///  联立两直线方程解得：x = ( k^2 * pt1.x + k * (point.y - pt1.y ) + point.x ) / ( k^2 + 1) ，y = k * ( x - pt1.x) + pt1.y;
		///  然后再判断垂足是否在线段上，如果在线段上则返回垂足；如果不在则计算两端点到垂足的距离，选择距离垂足较近的端点返回。
		/// </summary>
		/// <param name="ptCurrent">目标点</param>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <returns>点到线段的最近交点</returns>
		public static CPointD CP_Pnt2Sect( CPointD ptCurrent, CPointD ptStart, CPointD ptEnd )
		{
			CPointD ptVertical;														// 垂足

			// 如果线段与X轴平行，计算点到线段所在平行线的垂线相交的垂足
			if( ptStart.y == ptEnd.y )
			{
				ptVertical	= new CPointD( ptCurrent.x, ptStart.y );
			}
				// 如果线段与Y轴平行，计算点到线段所在平行线的垂线相交的垂足
			else if( ptStart.x == ptEnd.x )
			{
				ptVertical	= new CPointD( ptStart.x, ptCurrent.y );
			}
				// 否则线段不平行于X轴和Y轴，计算垂足
			else
			{
				double K	= ( ptEnd.y - ptStart.y ) / ( ptEnd.x - ptStart.x );	// 斜率
				double X	= ( K*K * ptStart.x + K*( ptCurrent.y - ptStart.y ) + ptCurrent.x ) / ( K*K +1 );
				double Y	= K*( X - ptStart.x ) + ptStart.y;

				ptVertical	= new CPointD( X, Y );
			}

			// 判断垂足是否在线段上，是则返回垂足
			if( IsPntOnSect( ptVertical, ptStart, ptEnd ) )
			{
				return ptVertical;
			}
				// 否则，计算两端点到垂足的距离，选择距离垂足较近的端点返回
			else
			{
				if( Distance( ptStart, ptVertical ) < Distance( ptEnd, ptVertical ) )
					return ptStart;
				else
					return ptEnd;

			}

		}

		/// <summary>
		///  只要分别计算点到每条线段的最近点，记录最近距离，取其中最近距离最小的点即可。 
		/// </summary>
		/// <param name="ptCurrent">当前点</param>
		/// <param name="arrPolyline">折线、矩形、多边形有序点数组</param>
		/// <returns>点到折线、矩形、多边形的最近交点</returns>
		public static CPointD CP_Pnt2Polygon( CPointD ptCurrent, ArrayList arrPolyline )
		{
			ArrayList arrPtNear = new ArrayList();
			// 记录点到每条线段的最近点
			for( int nCount	= 1; nCount < arrPolyline.Count; ++nCount )
			{
				arrPtNear.Add( CP_Pnt2Sect( ptCurrent, (CPointD)arrPolyline[nCount-1], (CPointD)arrPolyline[nCount] ) );
			}

			// 取最近距离的最近点
			CPointD ptMin = (CPointD)arrPtNear[0];
			for( int nCount =1; nCount < arrPtNear.Count; ++nCount )
			{
				if( Distance( ptCurrent, ptMin ) > Distance( ptCurrent, (CPointD)arrPtNear[nCount] ) )
				{
					ptMin = (CPointD)arrPtNear[nCount];
				}

			}

			return ptMin;
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
		///	   线段A：斜率为：Ka = ( ptAEnd.y - ptAStart.y ) / (ptAEnd.x - ptAStart.x );该直线方程为：y = Ka* ( x - ptAStart.x) + ptAStart.y
		///	   线段B：斜率为：Kb = ( ptBEnd.y - ptBStart.y ) / (ptBEnd.x - ptBStart.x );该直线方程为：y = Kb* ( x - ptBStart.x) + ptBStart.y
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
			double Ka = ( ptAEnd.y - ptAStart.y ) / ( ptAEnd.x - ptAStart.x );
			double Kb = ( ptBEnd.y - ptBStart.y ) / ( ptBEnd.x - ptBStart.x );

			// 线段A平行于Y轴
			if( ptAStart.x == ptAEnd.x )
			{
				// 线段B也平行于Y轴
				if( ptBStart.x == ptBEnd.x )
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
				}	// The end of "if( ptBStart.x == ptBEnd.x )"
					// 线段B不平行于Y轴
				else
				{
					// 若线段B不平行于Y轴，则交点横坐标为ptAStart的横坐标，代入到线段B的直线方程中可以计算出交点纵坐标
					ptResult = new CPointD( ptAStart.x, (Kb * ( ptAStart.x - ptBStart.x) + ptBStart.y) );
					return true;
				}
			}		// The end of "if( ptAStart.x == ptAEnd.x )"
				// 线段A不平行于Y轴，线段B平行于Y轴
			else if( ( ptAStart.x != ptAEnd.x ) && ( ptBStart.x == ptBEnd.x ) )
			{
				// 如果ptAStart和ptAEnd横坐标不同，但是ptBStart和ptBEnd横坐标相同，即线段B平行于Y轴，则交点横坐标为ptBStart
				// 的横坐标，代入到线段A的直线方程中可以计算出交点纵坐标
				ptResult = new CPointD( ptBStart.x, (Ka * ( ptBStart.x - ptAStart.x) + ptAStart.y) );
				return true;
			}


			// 线段A平行于X轴
			if( ptAStart.y == ptAEnd.y )
			{
				// 线段B也平行于X轴
				if( ptBStart.y == ptBEnd.y )
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
				}	// The end of "if( ptBStart.y == ptBEnd.y )"
					// 线段B不平行于X轴
				else
				{
					// 若线段B不平行于X轴，则交点纵坐标为ptAStart的纵坐标，代入到线段B的直线方程中可以计算出
					// 交点横坐标(x = (y - ptBStart.y + k*ptBStart.x) / k)
					ptResult = new CPointD( ((ptAStart.y - ptBStart.y + Kb*ptBStart.x) / Kb), ptAStart.y );
					return true;
				}
			}		// The end of "if( ptAStart.y == ptAEnd.y )"
				// 线段A不平行于X轴，线段B平行于X轴
			else if( ( ptAStart.y != ptAEnd.y ) && ( ptBStart.y == ptBEnd.y ) )
			{
				// 如果ptAStart和ptAEnd纵坐标不同，但是ptBStart和ptBEnd纵坐标相同，即线段B平行于X轴，则交点纵坐
				// 标为ptBStart的纵坐标，代入到线段A的直线方程中可以计算出交点横坐标(x = (y - ptAStart.y + k*ptAStart.x) / k)
				ptResult = new CPointD( ((ptBStart.y - ptAStart.y + Ka*ptAStart.x) / Ka), ptBStart.y );
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
			// x = ( Ka*ptAStart.x - Kb*ptBStart.x + ptBStart.y - ptAStart.y ) / (Ka + Kb);
			// y = Kb* ( x - ptBStart.x) + ptBStart.y;
			double X = ( Ka*ptAStart.x - Kb*ptBStart.x + ptBStart.y - ptAStart.y ) / (Ka + Kb);
			double Y = Kb* ( X - ptBStart.x) + ptBStart.y;
			ptResult = new CPointD( X, Y );
		
			return true;

		}

		/// <summary>
		///  分别求与每条边的交点即可。 
		/// </summary>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptEnd">线段终止点</param>
		/// <param name="arrPolyline">折线、矩形、多边形有序点数组</param>
		/// <param name="arrCrossResult">线段或直线与折线、矩形、多边形的交点数组</param>
		/// <returns>线段是否与折线、矩形、多边形有交点</returns>
		public static bool CP_Sect2Polygon( CPointD ptStart, CPointD ptEnd, ArrayList arrPolyline, out ArrayList arrCrossResult )
		{
			arrCrossResult = new ArrayList();
			CPointD ptCrossResult;
			for( int nCount = 1; nCount < arrPolyline.Count; ++nCount )
			{
				if( CP_Sect2Sect( ptStart, ptEnd, (CPointD)arrPolyline[nCount-1], (CPointD)arrPolyline[nCount], out ptCrossResult ) )
				{
					arrCrossResult.Add( ptCrossResult );
				}
			}

			if( arrCrossResult.Count == 0 )
				return false;
			else
				return true;
		}

		/// <summary>
		/// 如果该点在圆心，因为圆心到圆周任一点的距离相等，返回UNDEFINED。
		/// 连接点P和圆心O，如果PO平行于X轴，则根据P在O的左边还是右边计算出
		/// 最近点的横坐标为centerPoint.x - radius 或 centerPoint.x + radius。
		/// 如果PO平行于Y轴，则根据P在O的上边还是下边计算出最近点的纵坐标为 
		/// centerPoint.y + radius或 centerPoint.y - radius。如果PO不平行于X轴和Y轴，
		/// 则PO的斜率存在且不为0，这时直线PO斜率为k = （ P.y - O.y ）/ ( P.x - O.x )。
		/// 直线PO的方程为：y = k * ( x - P.x) + P.y。设圆方程为:(x - O.x )^2 + ( y - O.y )^2 = r^2，
		/// 联立两方程组可以解出直线PO和圆的交点，取其中离P点较近的交点即可。
		/// 求解后的交点坐标为：
		/// x1 = sqrt( r^2 / ( 1 + ( (P.y - O.y)/(P.x - O.x) )^2 ) ) + O.x; 
		/// x2 = -sqrt( r^2 / ( 1 + ( (P.y - O.y)/(P.x - O.x) )^2 ) ) + O.x;
		/// y1 = sqrt( r^2 / ( 1 + ( (P.x - O.x)/(P.y - O.y) )^2 ) ) + O.y;
		/// y2 = -sqrt( r^2 / ( 1 + ( (P.x - O.x)/(P.y - O.y) )^2 ) ) + O.y;
		/// </summary>
		/// <param name="ptCurrent">当前点</param>
		/// <param name="CirCenter">圆心点</param>
		/// <param name="radius">圆半径</param>
		/// <param name="ptResult">返回点到圆的最近距离的交点，没有交点时为(0,0)</param>
		/// <returns>点到圆的最近距离是否有交点</returns>
		public static bool CP_Pnt2Circle( CPointD ptCurrent, CPointD CirCenter, double radius , out CPointD ptResult )
		{
			ptResult = new CPointD( 0, 0 );
			
			// 如果该点在圆心，因为圆心到圆周任一点的距离相等，返回false，交点为圆心
			if( ptCurrent == CirCenter )
			{
				ptResult = CirCenter;

				return false;
			}

			// 连接点P和圆心O，如果PO平行于X轴，则根据P在O的左边还是右边计算出最近点的横坐标为centerPoint.x - radius 或 centerPoint.x + radius
			if( ptCurrent.y == CirCenter.y )
			{
				if( ptCurrent.x < CirCenter.x )
					ptResult = new CPointD( CirCenter.x - radius, ptCurrent.y );
				else
					ptResult = new CPointD( CirCenter.x + radius, ptCurrent.y );
			}
				// 如果PO平行于Y轴，则根据P在O的上边还是下边计算出最近点的纵坐标为centerPoint.y + radius或 centerPoint.y - radius
			else if( ptCurrent.x == CirCenter.x )
			{
				if( ptCurrent.y < CirCenter.y )
					ptResult = new CPointD( ptCurrent.x, CirCenter.y - radius );
				else
					ptResult = new CPointD( ptCurrent.x, CirCenter.y + radius );

			}
				// 如果PO不平行于X轴和Y轴，则PO的斜率存在且不为0,联立两方程组可以解出直线PO和圆的交点，取其中离P点较近的交点即可
			else
			{
				double X1 =  Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptCurrent.y - CirCenter.y)/(ptCurrent.x - CirCenter.x), 2.0 ) ) ) + CirCenter.x; 
				double X2 = -Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptCurrent.y - CirCenter.y)/(ptCurrent.x - CirCenter.x), 2.0 ) ) ) + CirCenter.x; 
				double Y1 =  Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptCurrent.x - CirCenter.x)/(ptCurrent.y - CirCenter.y), 2.0 ) ) ) + CirCenter.y; 
				double Y2 = -Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptCurrent.x - CirCenter.x)/(ptCurrent.y - CirCenter.y), 2.0 ) ) ) + CirCenter.y;
				if( Distance( new CPointD( X1, Y1 ), ptCurrent ) < Distance( new CPointD( X2, Y2 ), ptCurrent ) )
					ptResult = new CPointD( X1, Y1 );
				else
					ptResult = new CPointD( X2, Y2 );
			}

			return true;
		}
		
		/// <summary>
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
		///	直线P1,P2斜率为k = （P2.y – P1.y ）/ (P2.x – P1.x )
		///	直线P1,P2的方程为：y = k * ( x - P1.x) + P1.y
		///	设圆心O方程为:(x - O.x )^2 + ( y - O.y )^2 = r^2
		///	求解方程组的结果交点为：
		///	X1 =  sqrt( r^2 / ( 1+( ( P1.x-P2.x )/( P2.y-P1.y) )^2 ) ) + O.x;
		///	X2 = -sqrt( r^2 / ( 1+( ( P1.x-P2.x )/( P2.y-P1.y) )^2 ) ) + O.x;
		///	Y1 =  sqrt( r^2 / ( 1+( ( P2.y-P1.y )/( P1.x-P2.x) )^2 ) ) + O.y;
		///	Y2 = -sqrt( r^2 / ( 1+( ( P2.y-P1.y )/( P1.x-P2.x) )^2 ) ) + O.y;
		/// </summary>
		/// <param name="ptStart">线段起始点</param>
		/// <param name="ptStart">线段终止点</param>
		/// <param name="CirCenter">圆心点</param>
		/// <param name="radius">圆半径</param>
		/// <param name="arrCrossResult">返回线段或直线与圆的交点数组</param>
		/// <returns>线段或直线与圆是否有交点</returns>
		public static bool CP_Sect2Circle( CPointD ptStart, CPointD ptEnd, CPointD CirCenter, double radius , out ArrayList arrCrossResult )
		{
			arrCrossResult 	= new ArrayList();
			CPointD ptCross = new CPointD( 0, 0 );

			// 如果L是线段且P1，P2都包含在圆O内，则没有交点
			if( IsPntInCircle( ptStart, CirCenter, radius ) && IsPntInCircle( ptEnd, CirCenter, radius ) )
				return false;

			double dbDistance = Dist_Pnt2Sect( CirCenter, ptStart, ptEnd );
			// 如果L平行于Y轴
			if( ptStart.x == ptEnd.x )
			{			
				// 计算圆心到L的距离dis,如果dis > r 则L和圆没有交点
				if( dbDistance > radius )
				{
					return false;
				}

				// 相切情况下交点只有一个，位于圆心左或右
				if ( dbDistance == radius )
				{
					ptCross = new CPointD( ptStart.x, CirCenter.y );
					arrCrossResult.Add( ptCross );
				}
					// 否则有两个交点，利用勾股定理，可以求出两交点坐标[ Y1:sqrt( r^2 - distance^2 ),Y2:-sqrt( r^2 - distance^2 ) ]
				else
				{
					double Y = Math.Sqrt( Math.Pow( radius, 2.0 ) - Math.Pow( dbDistance, 2.0 ) );

					ptCross = new CPointD( ptStart.x, CirCenter.y - Y );
					arrCrossResult.Add( ptCross );
					ptCross = new CPointD( ptStart.x, CirCenter.y + Y );
					arrCrossResult.Add( ptCross );
				}

			}
				// 如果L平等于X轴
			else if( ptStart.y == ptEnd.y )
			{
				// 计算圆心到L的距离dis,如果dis > r 则L和圆没有交点
				if( dbDistance > radius )
				{
					return false;
				}

				// 相切情况下交点只有一个，位于圆心上或下
				if ( dbDistance == radius )
				{
					ptCross = new CPointD( CirCenter.x, ptStart.y );
					arrCrossResult.Add( ptCross );
				}
					// 否则有两个交点，利用勾股定理，可以求出两交点坐标[ X1:sqrt( r^2 - distance^2 ),X2:-sqrt( r^2 - distance^2 ) ]
				else
				{
					double X	= Math.Sqrt( Math.Pow( radius, 2.0 ) - Math.Pow( dbDistance, 2.0 ) );

					ptCross		= new CPointD( CirCenter.x - X, ptStart.y );
					arrCrossResult.Add( ptCross );
					ptCross		= new CPointD( CirCenter.x + X, ptStart.y );
					arrCrossResult.Add( ptCross );
				}

			}
				// 如果L既不平行X轴也不平行Y轴，可以求出L的斜率K，然后列出L的点斜式方程，和圆方程联立即可求解出L和圆的两个交点
			else
			{
				double X1	= -Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptStart.x - ptEnd.x)/(ptEnd.y - ptStart.y), 2.0 ) ) ) + CirCenter.x; 
				double X2	=  Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptStart.x - ptEnd.x)/(ptEnd.y - ptStart.y), 2.0 ) ) ) + CirCenter.x;  
				double Y1	= -Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptEnd.y - ptStart.y)/(ptStart.x - ptEnd.x), 2.0 ) ) ) + CirCenter.y;
				double Y2	=  Math.Sqrt( Math.Pow( radius, 2.0 ) / ( 1 + Math.Pow( (ptEnd.y - ptStart.y)/(ptStart.x - ptEnd.x), 2.0 ) ) ) + CirCenter.y; 
			
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
	
		// ---------------------------------------------------------------------------------------------------------------------

	}
}
