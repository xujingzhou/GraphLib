using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;		// 调试

using Jurassic.Graph.Base;

namespace TestSymbol
{
	/// <summary>
	/// 对jGMA基础几何算法类中主要功能进行测试
	/// 编写：徐景周，2006.2.27
	/// </summary>
	public class TestGeoAlgorithm
	{
		#region 成员变量

		public CPointD m_ptAStart, m_ptAEnd, m_ptBStart, m_ptBEnd;

		#endregion

		#region 构造涵数

		public TestGeoAlgorithm()
		{
//			m_ptAStart = m_ptAEnd = m_ptBStart = m_ptBEnd = new CPointD();
		}

		#endregion

		#region 综合测试

		/// <summary>
		/// 综合测试
		/// </summary>
		public bool TestAll()
		{
			string strResult = "测试结果：\n";
			if( Test_DotProduce() )
				strResult += "Test_DotProduce点积测试 ==> 成功\n";
			else
				strResult += "Test_DotProduce点积测试 ==> 失败\n";

			if( Test_CrossProduce() )
				strResult += "Test_CrossProduce叉积测试 ==> 成功\n";
			else
				strResult += "Test_CrossProduce叉积测试 ==> 失败\n";

			// 分类分隔符
			strResult += "==============================================================\n";

			if( Test_Distance() )
				strResult += "Test_Distance距离测试 ==> 成功\n";
			else
				strResult += "Test_Distance距离测试 ==> 失败\n";

			if( Test_Distance2() )
				strResult += "Test_Distance2距离不开方测试 ==> 成功\n";
			else
				strResult += "Test_Distance2距离不开方测试 ==> 失败\n";

			if( Test_Dist_Pnt2Line() )
				strResult += "Test_Dist_Pnt2Sect点与直线的距离测试 ==> 成功\n";
			else
				strResult += "Test_Dist_Pnt2Sect点与直线的距离测试 ==> 失败\n";

			if( Test_Dist_Pnt2Sect() )
				strResult += "Test_Dist_Pnt2Sect点与线段的距离测试 ==> 成功\n";
			else
				strResult += "Test_Dist_Pnt2Sect点与线段的距离测试 ==> 失败\n";

			if( Test_Dist_Pnt2Polyline() )
				strResult += "Test_Dist_Pnt2Polyline点与折线、曲线、多边形的最近距离测试 ==> 成功\n";
			else
				strResult += "Test_Dist_Pnt2Polyline点与折线、曲线、多边形的最近距离测试 ==> 失败\n";

			// 分类分隔符
			strResult += "==============================================================\n";

			if( Test_Side_Inflexion() )
				strResult += "Test_Side_Inflexion折线段的拐向判断 ==> 成功\n";
			else
				strResult += "Test_Side_Inflexion折线段的拐向判断 ==> 失败\n";

			if( Test_Side_PntOnLine() )
				strResult += "Test_Side_PntOnLine点在有向直线的那一边 ==> 成功\n";
			else
				strResult += "Test_Side_PntOnLine点在有向直线的那一边 ==> 失败\n";

			if( Test_Side_PntOnPolyline() )
				strResult += "Test_Side_PntOnPolyline点在有向曲线的那一边 ==> 成功\n";
			else
				strResult += "Test_Side_PntOnPolyline点在有向曲线的那一边 ==> 失败\n";

			if( Test_Side_PolylineOnLine() )
				strResult += "Test_Side_PolylineOnLine折线在直线的那一边 ==> 成功\n";
			else
				strResult += "Test_Side_PolylineOnLine折线在直线的那一边 ==> 失败\n";

			if( Test_Side_RectOnLine() )
				strResult += "Test_Side_RectOnLine矩形在直线的那一边 ==> 成功\n";
			else
				strResult += "Test_Side_RectOnLine矩形在直线的那一边 ==> 失败\n";

			if( Test_Side_SectOnLine() )
				strResult += "Test_Side_SectOnLine线段在直线的那一边 ==> 成功\n";
			else
				strResult += "Test_Side_SectOnLine线段在直线的那一边 ==> 失败\n";

			// 分类分隔符
			strResult += "==============================================================\n";

			if( Test_IsPntOnPnt() )
				strResult += "Test_IsPntOnPnt点是否在另一点上 ==> 成功\n";
			else
				strResult += "Test_IsPntOnPnt点是否在另一点上 ==> 失败\n";

			if( Test_IsPntOnSect() )
				strResult += "Test_IsPntOnSect点是否在线段上 ==> 成功\n";
			else
				strResult += "Test_IsPntOnSect点是否在线段上 ==> 失败\n";

			if( Test_IsPntOnLine() )
				strResult += "Test_IsPntOnLine点是否在直线上 ==> 成功\n";
			else
				strResult += "Test_IsPntOnLine点是否在直线上 ==> 失败\n";

			if( Test_IsPntOnPolyline() )
				strResult += "Test_IsPntOnPolyline点是否在曲线上 ==> 成功\n";
			else
				strResult += "Test_IsPntOnPolyline点是否在曲线上 ==> 失败\n";

			if( Test_IsPntInPolygon() )
				strResult += "Test_IsPntInPolygon点是否在多边形内部 ==> 成功\n";
			else
				strResult += "Test_IsPntInPolygon点是否在多边形内部 ==> 失败\n";

			if( Test_IsPntInRect() )
				strResult += "Test_IsPntInRect点是否在矩形内部 ==> 成功\n";
			else
				strResult += "Test_IsPntInRect点是否在矩形内部 ==> 失败\n";

			if( Test_IsPntInCircle() )
				strResult += "Test_IsPntInCircle点是否在圆内 ==> 成功\n";
			else
				strResult += "Test_IsPntInCircle点是否在圆内 ==> 失败\n";

			// 分类分隔符
			strResult += "==============================================================\n";

			if( Test_IsSectCrossSect() )
				strResult += "Test_IsSectCrossSect两线段是否相交 ==> 成功\n";
			else
				strResult += "Test_IsSectCrossSect两线段是否相交 ==> 失败\n";

			if( Test_IsSectCrossLine() )
				strResult += "Test_IsSectCrossLine线段与直线是否相交 ==> 成功\n";
			else
				strResult += "Test_IsSectCrossLine线段与直线是否相交 ==> 失败\n";

			if( Test_IsLineCrossLine() )
				strResult += "Test_IsLineCrossLine直线与直线是否相交 ==> 成功\n";
			else
				strResult += "Test_IsLineCrossLine直线与直线是否相交 ==> 失败\n";

			if( Test_IsSectCrossPolyline() )
				strResult += "Test_IsSectCrossPolyline线段与折线是否相交 ==> 成功\n";
			else
				strResult += "Test_IsSectCrossPolyline线段与折线是否相交 ==> 失败\n";

			if( Test_IsLineCrossPolyline() )
				strResult += "Test_IsLineCrossPolyline直线与折线是否相交 ==> 成功\n";
			else
				strResult += "Test_IsLineCrossPolyline直线与折线是否相交 ==> 失败\n";

			if( Test_IsPolylineCrossPolyline() )
				strResult += "Test_IsPolylineCrossPolyline折线是否与另一折线相交 ==> 成功\n";
			else
				strResult += "Test_IsPolylineCrossPolyline折线是否与另一折线相交 ==> 失败\n";

			if( Test_IsSectCrossRect() )
				strResult += "Test_IsSectCrossRect线段是否与矩形相交 ==> 成功\n";
			else
				strResult += "Test_IsSectCrossRect线段是否与矩形相交 ==> 失败\n";

			if( Test_IsLineCrossRect() )
				strResult += "Test_IsLineCrossRect直线是否与矩形相交 ==> 成功\n";
			else
				strResult += "Test_IsLineCrossRect直线是否与矩形相交 ==> 失败\n";

			if( Test_IsPolylineCrossRect() )
				strResult += "Test_IsPolylineCrossRect折线是否与矩形相交 ==> 成功\n";
			else
				strResult += "Test_IsPolylineCrossRect折线是否与矩形相交 ==> 失败\n";

			if( Test_IsSectCrossPolygon() )
				strResult += "Test_IsSectCrossPolygon线段是否与多边形相交 ==> 成功\n";
			else
				strResult += "Test_IsSectCrossPolygon线段是否与多边形相交 ==> 失败\n";

			if( Test_IsLineCrossPolygon() )
				strResult += "Test_IsLineCrossPolygon直线是否与多边形相交 ==> 成功\n";
			else
				strResult += "Test_IsLineCrossPolygon直线是否与多边形相交 ==> 失败\n";

			// 分类分隔符
			strResult += "==============================================================\n";

			if( Test_IsSectInRect() )
				strResult += "Test_IsSectInRect线段是否在矩形内部 ==> 成功\n";
			else
				strResult += "Test_IsSectInRect线段是否在矩形内部 ==> 失败\n";

			if( Test_IsPolylineInRect() )
				strResult += "Test_IsPolylineInRect线段、折线、多边形是否在矩形内部 ==> 成功\n";
			else
				strResult += "Test_IsPolylineInRect线段、折线、多边形是否在矩形内部 ==> 失败\n";

			if( Test_IsRectInRect() )
				strResult += "Test_IsRectInRect矩形是否在矩形内部 ==> 成功\n";
			else
				strResult += "Test_IsRectInRect矩形是否在矩形内部 ==> 失败\n";

			if( Test_IsCircleInRect() )
				strResult += "Test_IsCircleInRect圆是否在矩形内部 ==> 成功\n";
			else
				strResult += "Test_IsCircleInRect圆是否在矩形内部 ==> 失败\n";

			if( Test_IsSectInPolygon() )
				strResult += "Test_IsSectInPolygon线段是否在多边形内部 ==> 成功\n";
			else
				strResult += "Test_IsSectInPolygon线段是否在多边形内部 ==> 失败\n";

			if( Test_IsPolylineInPolygon() )
				strResult += "Test_IsPolylineInPolygon折线是否在多边形区域内 ==> 成功\n";
			else
				strResult += "Test_IsPolylineInPolygon折线是否在多边形区域内 ==> 失败\n";

			if( Test_IsPolygonInPolygon() )
				strResult += "Test_IsPolygonInPolygon多边形是否在多边形区域内 ==> 成功\n";
			else
				strResult += "Test_IsPolygonInPolygon多边形是否在多边形区域内 ==> 失败\n";

			if( Test_IsRectInPolygon() )
				strResult += "Test_IsRectInPolygon矩形是否在多边形区域内 ==> 成功\n";
			else
				strResult += "Test_IsRectInPolygon矩形是否在多边形区域内 ==> 失败\n";

			if( Test_IsCircleInPolygon() )
				strResult += "Test_IsCircleInPolygon圆是否在多边形区域内 ==> 成功\n";
			else
				strResult += "Test_IsCircleInPolygon圆是否在多边形区域内 ==> 失败\n";

			if( Test_IsPolylineInCircle() )
				strResult += "Test_IsPolylineInCircle线段、折线、多边形是否在圆内 ==> 成功\n";
			else
				strResult += "Test_IsPolylineInCircle线段、折线、多边形是否在圆内 ==> 失败\n";

			if( Test_IsCircleInCircle() )
				strResult += "Test_IsCircleInCircle圆一是否在圆二内 ==> 成功\n";
			else
				strResult += "Test_IsCircleInCircle圆一是否在圆二内 ==> 失败\n";

			// 分类分隔符
			strResult += "==============================================================\n";

			if( Test_CP_Pnt2Sect() )
				strResult += "Test_CP_Pnt2Sect点到线段的最近交点 ==> 成功\n";
			else
				strResult += "Test_CP_Pnt2Sect点到线段的最近交点 ==> 失败\n";

			if( Test_CP_Pnt2Line( ) )
				strResult += "Test_CP_Pnt2Line点到直线的垂足 ==> 成功\n";
			else
				strResult += "Test_CP_Pnt2Line点到直线的垂足 ==> 失败\n";

			if( Test_CP_Pnt2Polyline() )
				strResult += "Test_CP_Pnt2Polyline点到折线、矩形、多边形的最近交点 ==> 成功\n";
			else
				strResult += "Test_CP_Pnt2Polyline点到折线、矩形、多边形的最近交点 ==> 失败\n";

			if( Test_CP_Sect2Sect() )
				strResult += "Test_CP_Sect2Sect两线段的交点 ==> 成功\n";
			else
				strResult += "Test_CP_Sect2Sect两线段的交点 ==> 失败\n";

			if( Test_CP_Line2Line() )
				strResult += "Test_CP_Line2Line两直线的交点 ==> 成功\n";
			else
				strResult += "Test_CP_Line2Line两直线的交点 ==> 失败\n";

/*			if( Test_CP_Sect2Polygon() )
				strResult += "Test_CP_Sect2Polygon线段与折线、矩形、多边形的交点 ==> 成功\n";
			else
				strResult += "Test_CP_Sect2Polygon线段与折线、矩形、多边形的交点 ==> 失败\n";
*/

			if( Test_CP_Pnt2Circle() )
				strResult += "Test_CP_Pnt2Circle点到圆的最近距离交点 ==> 成功\n";
			else
				strResult += "Test_CP_Pnt2Circle点到圆的最近距离交点 ==> 失败\n";

			if( Test_CP_Sect2Circle() )
				strResult += "Test_CP_Sect2Circle线段或直线与圆交点 ==> 成功\n";
			else
				strResult += "Test_CP_Sect2Circle线段或直线与圆交点 ==> 失败\n";

			MessageBox.Show( strResult, "基础几何算法单元测试" );

			return true;
		}

		#endregion

		#region 积测试

		/// <summary>
		/// 点积测试
		/// </summary>
		public bool Test_DotProduce()
		{
			m_ptAStart = new CPointD( 1, 1 );
			m_ptBStart = new CPointD( 2, 2 );
			double dbResult = jGMA.DotProduce( m_ptAStart, m_ptBStart );

			if( dbResult != 4.0 )
				return false;

			return true;
		}
		
		/// <summary>
		/// 叉积测试
		/// </summary>
		public bool Test_CrossProduce()
		{
			m_ptAStart = new CPointD( 1, 1 );
			m_ptBStart = new CPointD( 2, 2 );
			double dbResult = jGMA.CrossProduce( m_ptAStart, m_ptBStart );	

			if( dbResult != 0.0 )
				return false;

			return true;
		}

		#endregion

		#region 距离测试

		/// <summary>
		/// 距离测试
		/// </summary>
		public bool Test_Distance()
		{
			// 1.
			m_ptAStart = new CPointD( 1, 1 );
			m_ptBStart = new CPointD( 1, 1 );
			Debug.Assert( jGMA.Distance( m_ptAStart, m_ptBStart ) == 0.0 );

			// 2.
			m_ptAStart = new CPointD( 1, 1 );
			m_ptBStart = new CPointD( 1, 3 );
			double dbResult = jGMA.Distance( m_ptAStart, m_ptBStart );	

			if( dbResult != 2.0 )
				return false;

			return true;
		}

		/// <summary>
		/// 计算两点pt1,pt2之间的距离平方。有时判断长短，不用开方即可
		/// </summary>
		public bool Test_Distance2()
		{
			// 1.
			m_ptAStart = new CPointD( 1, 1 );
			m_ptBStart = new CPointD( 1, 1 );
			Debug.Assert( jGMA.Distance2( m_ptAStart, m_ptBStart ) == 0.0 );

			// 2.
			m_ptAStart = new CPointD( 1, 1 );
			m_ptBStart = new CPointD( 1, 3 );
			double dbResult = jGMA.Distance2( m_ptAStart, m_ptBStart );	
	
			if( dbResult != 4.0 )
				return false;

			return true;
		}

		/// <summary>
		/// 点与直线的距离测试
		/// </summary>
		public bool Test_Dist_Pnt2Line()
		{
			// 1.
			m_ptAStart	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 0, 0 );
			m_ptBEnd	= new CPointD( 0, 0 );
			Debug.Assert( jGMA.Dist_Pnt2Line( m_ptAStart, m_ptBStart, m_ptBEnd ) == 0.0 );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptBStart	= new CPointD( 2, 2 );
			m_ptBEnd	= new CPointD( 2, 0 );
			double dbResult = jGMA.Dist_Pnt2Line( m_ptAStart, m_ptBStart, m_ptBEnd );	
	
			if( dbResult != 1.0 )
				return false;

			return true;
		}

		/// <summary>
		/// 点与线段的距离测试
		/// </summary>
		public bool Test_Dist_Pnt2Sect()
		{
			// 1.
			m_ptAStart	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 0, 0 );
			m_ptBEnd	= new CPointD( 0, 0 );
			Debug.Assert( jGMA.Dist_Pnt2Sect( m_ptAStart, m_ptBStart, m_ptBEnd ) == 0.0 );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptBStart	= new CPointD( 2, 2 );
			m_ptBEnd	= new CPointD( 2, 0 );
			double dbResult = jGMA.Dist_Pnt2Sect( m_ptAStart, m_ptBStart, m_ptBEnd );	
	
			if( dbResult != 1.0 )
				return false;

			return true;
		}

		/// <summary>
		/// 点与折线、曲线、多边形的最近距离测试
		/// </summary>
		public bool Test_Dist_Pnt2Polyline()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			Debug.Assert( jGMA.Dist_Pnt2Polyline( m_ptAStart, arPt ) == 0.0 );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			arPt		= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 1, 0 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			double dbResult = jGMA.Dist_Pnt2Polyline( m_ptAStart, arPt );	
	
			if( dbResult != 1.0 )
				return false;

			return true;
		}
		
		#endregion
		
		#region 方位测试

		/// <summary>
		/// 折线段的拐向判断
		/// </summary>
		public bool Test_Side_Inflexion()
		{
			// 1.
			m_ptAStart	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 0, 0 );
			m_ptBEnd	= new CPointD( 0, 0 );
			Debug.Assert( jGMA.Side_Inflexion( m_ptBStart, m_ptAStart, m_ptBEnd ) == 0.0 );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptBStart	= new CPointD( 2, 2 );
			m_ptBEnd	= new CPointD( 2, 0 );
			double dbResult = jGMA.Side_Inflexion( m_ptBStart, m_ptAStart, m_ptBEnd );	
	
			// 拐向左
			if( dbResult != -1.0 )
				return false;

			return true;
		}

		/// <summary>
		/// 点在有向直线的那一边
		/// </summary>
		public bool Test_Side_PntOnLine()
		{
			// 1.
			m_ptAStart	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 0, 0 );
			m_ptBEnd	= new CPointD( 0, 0 );
			Debug.Assert( jGMA.Side_PntOnLine( m_ptBStart, m_ptAStart, m_ptBEnd ) == 0.0 );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptBStart	= new CPointD( 2, 2 );
			m_ptBEnd	= new CPointD( 2, 0 );
			double dbResult = jGMA.Side_PntOnLine( m_ptAStart, m_ptBStart, m_ptBEnd );	
	
			// 左侧
			if( dbResult != -1.0 )
				return false;

			return true;
		}
		
		/// <summary>
		/// 点在有向曲线的那一边
		/// </summary>
		public bool Test_Side_PntOnPolyline()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			Debug.Assert( jGMA.Side_PntOnPolyline( m_ptAStart, arPt ) == 0.0 );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			arPt		= new ArrayList();
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 3, 1 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			double dbResult = jGMA.Side_PntOnPolyline( m_ptAStart, arPt );	
	
			// 左侧
			if( dbResult != -1.0 )
				return false;

			return true;
		}

		/// <summary>
		///  判断折线在直线的哪一边
		/// </summary>
		public bool Test_Side_PolylineOnLine()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 0, 0 );
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			Debug.Assert( jGMA.Side_PolylineOnLine( arPt, m_ptAStart, m_ptAEnd ) == 0.0 );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptAEnd	= new CPointD( 1, 0 );
			arPt		= new ArrayList();
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 3, 1 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			double dbResult = jGMA.Side_PolylineOnLine( arPt, m_ptAStart, m_ptAEnd );	
	
			// 右侧
			if( dbResult != 1.0 )
				return false;

			return true;
		}

		/// <summary>
		///  判断矩形在直线的哪一侧
		/// </summary>
		public bool Test_Side_RectOnLine()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 0, 0 );
			CRectD rct		= new CRectD( 0, 0, 0, 0 );
			Debug.Assert( jGMA.Side_RectOnLine( rct, m_ptAStart, m_ptAEnd ) == 0.0 );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptAEnd	= new CPointD( 1, 0 );
			rct			= new CRectD( 2, 0, 2, 2 );
			double dbResult = jGMA.Side_RectOnLine( rct, m_ptAStart, m_ptAEnd );	
	
			// 右侧
			if( dbResult != 1.0 )
				return false;

			return true;
		}

		/// <summary>
		///  判断线段在直线的哪一侧
		/// </summary>
		public bool Test_Side_SectOnLine()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 0, 0 );
			m_ptBStart		= new CPointD( 0, 0 );
			m_ptBEnd		= new CPointD( 0, 0 );
			Debug.Assert( jGMA.Side_SectOnLine( m_ptBStart, m_ptBEnd, m_ptAStart, m_ptAEnd ) == 0.0 );

			// 2.
			m_ptAStart		= new CPointD( 1, 1 );
			m_ptAEnd		= new CPointD( 1, 0 );
			m_ptBStart		= new CPointD( 2, 2 );
			m_ptBEnd		= new CPointD( 2, 0 );
			double dbResult = jGMA.Side_SectOnLine( m_ptBStart, m_ptBEnd, m_ptAStart, m_ptAEnd );	
	
			// 右侧
			if( dbResult != 1.0 )
				return false;

			return true;
		}

		#endregion

		#region 点测试

		/// <summary>
		/// 点是否在点上
		/// </summary>
		public bool Test_IsPntOnPnt()
		{
			// 1.
			m_ptAStart	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 1, 1 );
			Debug.Assert( jGMA.IsPntOnPnt( m_ptAStart, m_ptBStart ) != true );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptBStart	= new CPointD( 1, 1 );
			bool bResult = jGMA.IsPntOnPnt( m_ptAStart, m_ptBStart );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 点是否在线段上
		/// </summary>
		public bool Test_IsPntOnSect()
		{
			// 1.
			m_ptAStart	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 1, 2 );
			m_ptBEnd	= new CPointD( 1, 0 );
			Debug.Assert( jGMA.IsPntOnPnt( m_ptAStart, m_ptBStart ) != true );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptBStart	= new CPointD( 1, 2 );
			m_ptBEnd	= new CPointD( 1, 0 );
			bool bResult = jGMA.IsPntOnSect( m_ptAStart, m_ptBStart, m_ptBEnd );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// 点是否在直线上
		/// </summary>
		public bool Test_IsPntOnLine()
		{
			// 1.
			m_ptAStart	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 1, 2 );
			m_ptBEnd	= new CPointD( 1, 0 );
			Debug.Assert( jGMA.IsPntOnPnt( m_ptAStart, m_ptBStart ) != true );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptBStart	= new CPointD( 1, 2 );
			m_ptBEnd	= new CPointD( 1, 0 );
			bool bResult = jGMA.IsPntOnLine( m_ptAStart, m_ptBStart, m_ptBEnd );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 点是否在矩形内部
		/// </summary>
		public bool Test_IsPntInRect()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			RectangleF rct	= new RectangleF( 1, 0, 2, 2 );
			Debug.Assert( jGMA.IsPntInRect( m_ptAStart, rct ) != true );

			// 2.
			m_ptAStart		= new CPointD( 1, 1 );
			rct				= new RectangleF( 1, 0, 2, 2 );
			bool bResult	= jGMA.IsPntInRect( m_ptAStart, rct );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 点是否在曲线上
		/// </summary>
		public bool Test_IsPntOnPolyline()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 1 );
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 1, 1 ) );
			Debug.Assert( jGMA.IsPntOnPolyline( m_ptAStart, arPt ) != true );

			// 2.
			m_ptAStart		= new CPointD( 1, 1 );
			arPt			= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 1, 1 ) );
			bool bResult	= jGMA.IsPntOnPolyline( m_ptAStart, arPt );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 点是否在多边形内部
		/// </summary>
		public bool Test_IsPntInPolygon()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 1 );
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 1, 1 ) );
			Debug.Assert( jGMA.IsPntOnPolyline( m_ptAStart, arPt ) != true );

			// 2.
			m_ptAStart		= new CPointD( 1, 1 );
			arPt			= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 0, 2 ) );
			bool bResult	= jGMA.IsPntInPolygon( m_ptAStart, arPt, null );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 点是否在圆内
		/// </summary>
		public bool Test_IsPntInCircle()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptBStart		= new CPointD( 2, 2 );			
			Debug.Assert( jGMA.IsPntInCircle( m_ptAStart, m_ptBStart, 2 ) != true );

			// 2.
			m_ptAStart		= new CPointD( 1, 1 );
			m_ptBStart		= new CPointD( 2, 2 );			
			bool bResult	= jGMA.IsPntInCircle( m_ptAStart, m_ptBStart, 2 );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		#endregion
		
		#region 相交测试

		/// <summary>
		/// 两线段是否相交
		/// </summary>
		public bool Test_IsSectCrossSect()
		{
			// 1.
			m_ptAStart	= new CPointD( 1, 0 );
			m_ptAEnd	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 1, 2 );
			m_ptBEnd	= new CPointD( 1, 1 );	
			Debug.Assert( jGMA.IsSectCrossSect( m_ptAStart,m_ptAEnd, m_ptBStart, m_ptBEnd ) != true );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptAEnd	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 1, 2 );
			m_ptBEnd	= new CPointD( 1, 0 );
			bool bResult = jGMA.IsSectCrossSect( m_ptAStart,m_ptAEnd, m_ptBStart, m_ptBEnd );	
	
			// 不相交
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 线段与直线是否相交
		/// </summary>
		public bool Test_IsSectCrossLine()
		{
			// 1.
			m_ptAStart	= new CPointD( 1, 0 );
			m_ptAEnd	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 1, 1 );
			m_ptBEnd	= new CPointD( 0, 1 );	
			Debug.Assert( jGMA.IsSectCrossLine( m_ptAStart,m_ptAEnd, m_ptBStart, m_ptBEnd ) != true );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptAEnd	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 1, 2 );
			m_ptBEnd	= new CPointD( 1, 0 );
			bool bResult = jGMA.IsSectCrossLine( m_ptAStart,m_ptAEnd, m_ptBStart, m_ptBEnd );	
	
			// 不相交
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		///  判断线段是否和折线相交
		/// </summary>
		public bool Test_IsSectCrossPolyline()
		{
			// 1.
			m_ptAStart	= new CPointD( 1, 0 );
			m_ptAEnd	= new CPointD( 2, 0 );
			ArrayList arPt = new ArrayList();
			arPt.Add( new CPointD( 1, 1 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			Debug.Assert( jGMA.IsSectCrossPolyline( m_ptAStart, m_ptAEnd, arPt ) != true );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptAEnd	= new CPointD( 0, 0 );
			arPt		= new ArrayList();
			arPt.Add( new CPointD( 1, 1 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			bool bResult = jGMA.IsSectCrossPolyline( m_ptAStart, m_ptAEnd, arPt );	
	
			// 不相交
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 直线与直线是否相交
		/// </summary>
		public bool Test_IsLineCrossLine()
		{
			// 1.
			m_ptAStart	= new CPointD( 0, 0 );
			m_ptAEnd	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 2, 2 );
			m_ptBEnd	= new CPointD( 1, 0 );
			Debug.Assert( jGMA.IsLineCrossLine( m_ptAStart,m_ptAEnd, m_ptBStart, m_ptBEnd ) != true );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptAEnd	= new CPointD( 0, 0 );
			m_ptBStart	= new CPointD( 2, 2 );
			m_ptBEnd	= new CPointD( 1, 0 );
			bool bResult = jGMA.IsLineCrossLine( m_ptAStart,m_ptAEnd, m_ptBStart, m_ptBEnd );	
	
			// 不相交
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		///  判断直线是否和折线相交
		/// </summary>
		public bool Test_IsLineCrossPolyline()
		{
			// 1.平行
			m_ptAStart	= new CPointD( 1, 0 );
			m_ptAEnd	= new CPointD( 2, 1 );
			ArrayList arPt = new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 1, 1 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			Debug.Assert( jGMA.IsLineCrossPolyline( m_ptAStart, m_ptAEnd, arPt ) != true );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			m_ptAEnd	= new CPointD( 0, 0 );
			arPt		= new ArrayList();
			arPt.Add( new CPointD( 1, 1 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			bool bResult = jGMA.IsLineCrossPolyline( m_ptAStart, m_ptAEnd, arPt );	
	
			// 不相交
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 折线、曲线、多边形是否与另一个折线、曲线、多边形相交
		/// </summary>
		public bool Test_IsPolylineCrossPolyline()
		{
			// 1.平行
			ArrayList arPtA = new ArrayList();
			arPtA.Add( new CPointD( 1, 0 ) );
			arPtA.Add( new CPointD( 0, 0 ) );
			ArrayList arPtB = new ArrayList();
			arPtB.Add( new CPointD( 3, 2 ) );
			arPtB.Add( new CPointD( 3, 1 ) );
			arPtB.Add( new CPointD( 1, 1 ) );
			Debug.Assert( jGMA.IsPolylineCrossPolyline( arPtA, arPtB ) != true );

			// 2.
			arPtA			= new ArrayList();
			arPtA.Add( new CPointD( 1, 1 ) );
			arPtA.Add( new CPointD( 0, 0 ) );
			arPtA.Add( new CPointD( 2, 2 ) );
			arPtB			= new ArrayList();
			arPtB.Add( new CPointD( 3, 2 ) );
			arPtB.Add( new CPointD( 3, 1 ) );
			arPtB.Add( new CPointD( 1, 1 ) );
			bool bResult = jGMA.IsPolylineCrossPolyline( arPtA, arPtB );	
	
			// 不相交
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// 判断线段是否和矩形相交
		/// </summary>
		public bool Test_IsSectCrossRect()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 0, 1 );
			RectangleF rct	= new RectangleF( 1, 0, 2, 2 );
			Debug.Assert( jGMA.IsSectCrossRect( m_ptAStart, m_ptAEnd, rct ) != true );

			// 2.
			m_ptAStart		= new CPointD( 1, 1 );
			m_ptAEnd		= new CPointD( 2, 2 );
			rct				= new RectangleF( 1, 0, 2, 2 );
			bool bResult	= jGMA.IsSectCrossRect( m_ptAStart, m_ptAEnd, rct );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 判断直线是否和矩形相交
		/// </summary>
		public bool Test_IsLineCrossRect()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 0, 1 );
			RectangleF rct	= new RectangleF( 1, 0, 2, 2 );
			Debug.Assert( jGMA.IsLineCrossRect( m_ptAStart, m_ptAEnd, rct ) != true );

			// 2.
			m_ptAStart		= new CPointD( 1, 1 );
			m_ptAEnd		= new CPointD( 4, 4 );
			rct				= new RectangleF( 1, 0, 2, 2 );
			bool bResult	= jGMA.IsLineCrossRect( m_ptAStart, m_ptAEnd, rct );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 判断折线是否和矩形相交
		/// </summary>
		public bool Test_IsPolylineCrossRect()
		{
			// 1.
			ArrayList arPt = new ArrayList();
			arPt.Add( new CPointD( 1, 0 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			RectangleF rct	= new RectangleF( 1, 1, 2, 2 );
			Debug.Assert( jGMA.IsPolylineCrossRect( arPt, rct ) != true );

			// 2.
			arPt			= new ArrayList();
			arPt.Add( new CPointD( 1, 1 ) );
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			rct				= new RectangleF( 1, 0, 2, 2 );
			bool bResult	= jGMA.IsPolylineCrossRect( arPt, rct );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 判断线段是否和多边形相交
		/// </summary>
		public bool Test_IsSectCrossPolygon()
		{
			// 1.
			ArrayList arPt = new ArrayList();
			arPt.Add( new CPointD( 1, 1 ) );
			arPt.Add( new CPointD( 1, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 0, 1 );
			Debug.Assert( jGMA.IsSectCrossPolygon( m_ptAStart, m_ptAEnd, arPt, null ) != true );

			// 2.
			arPt			= new ArrayList();
			arPt.Add( new CPointD( 1, 1 ) );
			arPt.Add( new CPointD( 1, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 4, 4 );
			bool bResult	= jGMA.IsSectCrossPolygon( m_ptAStart, m_ptAEnd, arPt, null );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 判断直线段是否和多边形相交
		/// </summary>
		public bool Test_IsLineCrossPolygon()
		{
			// 1.
			ArrayList arPt = new ArrayList();
			arPt.Add( new CPointD( 1, 1 ) );
			arPt.Add( new CPointD( 1, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 0, 1 );
			Debug.Assert( jGMA.IsLineCrossPolygon( m_ptAStart, m_ptAEnd, arPt ) != true );

			// 2.
			arPt			= new ArrayList();
			arPt.Add( new CPointD( 1, 1 ) );
			arPt.Add( new CPointD( 1, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 4, 4 );
			bool bResult	= jGMA.IsLineCrossPolygon( m_ptAStart, m_ptAEnd, arPt );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		#endregion

		#region 是否在内部测试

		/// <summary>
		/// 线段是否在矩形内部
		/// </summary>
		public bool Test_IsSectInRect()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 0, 0 );
			RectangleF rct	= new RectangleF( 1, 0, 2, 2 );
			Debug.Assert( jGMA.IsSectInRect( m_ptAStart, m_ptAEnd, rct ) != true );

			// 2.
			m_ptAStart		= new CPointD( 1, 1 );
			m_ptAEnd		= new CPointD( 2, 2 );
			rct				= new RectangleF( 1, 0, 2, 2 );
			bool bResult	= jGMA.IsSectInRect( m_ptAStart, m_ptAEnd, rct );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 线段、折线、多边形是否在矩形内部
		/// </summary>
		public bool Test_IsPolylineInRect()
		{
			// 1.
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 0, 1 ) );
			arPt.Add( new CPointD( 0, 2 ) );
			RectangleF rct	= new RectangleF( 1, 0, 2, 2 );
			Debug.Assert( jGMA.IsPolylineInRect( arPt, rct ) != true );

			// 2.
			arPt			= new ArrayList();
			arPt.Add( new CPointD( 1, 1 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 3, 2 ) );
			rct				= new RectangleF( 1, 0, 2, 2 );
			bool bResult	= jGMA.IsPolylineInRect( arPt, rct );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// 矩形rctB是否在矩形rctA内部
		/// </summary>
		public bool Test_IsRectInRect()
		{
			// 1.
			RectangleF rctA	= new RectangleF( 1, 0, 2, 2 );
			RectangleF rctB	= new RectangleF( 0, 0, 1, 1 );
			Debug.Assert( jGMA.IsRectInRect( rctA, rctB ) != true );

			// 2.
			rctA			= new RectangleF( 1, 0, 2, 2 );
			rctB			= new RectangleF( 1, 1, 1, 1 );
			bool bResult	= jGMA.IsRectInRect( rctA, rctB );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 圆是否在矩形内部
		/// </summary>
		public bool Test_IsCircleInRect()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			RectangleF rct	= new RectangleF( 1, 1, 1, 1 );
			Debug.Assert( jGMA.IsCircleInRect( m_ptAStart, 1, rct ) != true );

			// 2.
			m_ptAStart		= new CPointD( 1, 1 );
			rct				= new RectangleF( 0, 0, 2, 2 );
			bool bResult	= jGMA.IsCircleInRect( m_ptAStart, 1, rct );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 线段是否在多边形区域内
		/// </summary>
		public bool Test_IsSectInPolygon()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 0, 1 );
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 1, 0 ) );
			arPt.Add( new CPointD( 3, 0 ) );
			arPt.Add( new CPointD( 3, 3 ) );
			arPt.Add( new CPointD( 0, 3 ) );
			Debug.Assert( jGMA.IsSectInPolygon( m_ptAStart, m_ptAEnd, arPt, null ) != true );

			// 2.
			m_ptAStart		= new CPointD( 0, 0 );
			m_ptAEnd		= new CPointD( 3, 3 );
			arPt			= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 3, 0 ) );
			arPt.Add( new CPointD( 3, 3 ) );
			arPt.Add( new CPointD( 0, 3 ) );
			bool bResult	= jGMA.IsSectInPolygon( m_ptAStart, m_ptAEnd, arPt, null );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 折线是否在多边形区域内
		/// </summary>
		public bool Test_IsPolylineInPolygon()
		{
			// 1.
			ArrayList arPtA = new ArrayList();
			arPtA.Add( new CPointD( 0, 0 ) );
			arPtA.Add( new CPointD( 0, 1 ) );
			arPtA.Add( new CPointD( 0, -1 ) );
			ArrayList arPtB	= new ArrayList();
			arPtB.Add( new CPointD( 1, 0 ) );
			arPtB.Add( new CPointD( 3, 0 ) );
			arPtB.Add( new CPointD( 3, 3 ) );
			arPtB.Add( new CPointD( 0, 3 ) );
			Debug.Assert( jGMA.IsPolylineInPolygon( arPtA, arPtB, null ) != true );

			// 2.
			arPtA	= new ArrayList();
			arPtA.Add( new CPointD( 0, 0 ) );
			arPtA.Add( new CPointD( 1, 1 ) );
			arPtA.Add( new CPointD( 2, 1 ) );
			arPtB	= new ArrayList();
			arPtB.Add( new CPointD( 0, 0 ) );
			arPtB.Add( new CPointD( 3, 0 ) );
			arPtB.Add( new CPointD( 3, 3 ) );
			arPtB.Add( new CPointD( 0, 3 ) );
			bool bResult = jGMA.IsPolylineInPolygon( arPtA, arPtB, null );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// 多边形是否在多边形区域内
		/// </summary>
		public bool Test_IsPolygonInPolygon()
		{
			// 1.
			ArrayList arPtA = new ArrayList();
			arPtA.Add( new CPointD( 0, 0 ) );
			arPtA.Add( new CPointD( 0, 1 ) );
			arPtA.Add( new CPointD( 0, -1 ) );
			ArrayList arPtB	= new ArrayList();
			arPtB.Add( new CPointD( 1, 0 ) );
			arPtB.Add( new CPointD( 3, 0 ) );
			arPtB.Add( new CPointD( 3, 3 ) );
			arPtB.Add( new CPointD( 0, 3 ) );
			Debug.Assert( jGMA.IsPolygonInPolygon( arPtA, arPtB, null, null ) != true );

			// 2.
			arPtA	= new ArrayList();
			arPtA.Add( new CPointD( 0, 0 ) );
			arPtA.Add( new CPointD( 1, 1 ) );
			arPtA.Add( new CPointD( 0, 1 ) );
			arPtB	= new ArrayList();
			arPtB.Add( new CPointD( 0, 0 ) );
			arPtB.Add( new CPointD( 3, 0 ) );
			arPtB.Add( new CPointD( 3, 3 ) );
			arPtB.Add( new CPointD( 0, 3 ) );
			arPtB.Add( new CPointD( 0, 0 ) );				// 如果多边形是闭合的，则最后一点应和第一点重合，jingzhou xu
			bool bResult	= jGMA.IsPolygonInPolygon( arPtA, arPtB, null, null );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// 矩形是否在多边形区域内
		/// </summary>
		public bool Test_IsRectInPolygon()
		{
			// 1.
			RectangleF rct	= new RectangleF( 0, 0, -2, -2 );
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 3, 0 ) );
			arPt.Add( new CPointD( 3, 3 ) );
			arPt.Add( new CPointD( 0, 3 ) );
			arPt.Add( new CPointD( 0, 0 ) );	
			Debug.Assert( jGMA.IsRectInPolygon( rct, arPt, null ) != true );

			// 2.
			rct		= new RectangleF( 1, 1, 2, 2 );
			arPt	= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 3, 0 ) );
			arPt.Add( new CPointD( 3, 3 ) );
			arPt.Add( new CPointD( 0, 3 ) );
			arPt.Add( new CPointD( 0, 0 ) );				
			bool bResult	= jGMA.IsRectInPolygon( rct, arPt, null );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// 圆是否在多边形区域内
		/// </summary>
		public bool Test_IsCircleInPolygon()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 4, 0 ) );
			arPt.Add( new CPointD( 3, 3 ) );
			arPt.Add( new CPointD( 0, 4 ) );
			arPt.Add( new CPointD( 0, 0 ) );	
			Debug.Assert( jGMA.IsCircleInPolygon( m_ptAStart, 1, arPt, null ) != true );

			// 2.
			m_ptAStart	= new CPointD( 1, 1 );
			arPt		= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 4, 0 ) );
			arPt.Add( new CPointD( 3, 3 ) );
			arPt.Add( new CPointD( 0, 4 ) );
			arPt.Add( new CPointD( 0, 0 ) );				
			bool bResult	= jGMA.IsCircleInPolygon( m_ptAStart, 1, arPt, null );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// 线段、折线、多边形是否在圆内
		/// </summary>
		public bool Test_IsPolylineInCircle()
		{
			// 1.
			ArrayList arPt = new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 0, 2 ) );
			m_ptAStart		= new CPointD( 0, 0 );	
			Debug.Assert( jGMA.IsPolylineInCircle( arPt, m_ptAStart, 1 ) != true );

			// 2.
			arPt			= new ArrayList();
			arPt.Add( new CPointD( 0, 0 ) );
			arPt.Add( new CPointD( 2, 0 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			arPt.Add( new CPointD( 0, 2 ) );
			m_ptAStart		= new CPointD( 2, 2 );	
			bool bResult	= jGMA.IsPolylineInCircle( arPt, m_ptAStart, 4 );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// 圆一是否在圆二内
		/// </summary>
		public bool Test_IsCircleInCircle()
		{
			// 1.
			m_ptAStart		= new CPointD( 0, 0 );	
			m_ptBStart		= new CPointD( 2, 2 );
			Debug.Assert( jGMA.IsCircleInCircle( m_ptAStart, 1, m_ptBStart, 3 ) != true );

			// 2.
			m_ptAStart		= new CPointD( 1, 1 );	
			m_ptBStart		= new CPointD( 2, 2 );
			bool bResult	= jGMA.IsCircleInCircle( m_ptAStart, 1, m_ptBStart, 3 );	
	
			// 不在
			if( !bResult )
				return false;

			return true;
		}

		#endregion
		
		#region 交点测试

		/// <summary>
		/// 点到线段的最近交点
		/// </summary>
		public bool Test_CP_Pnt2Sect()
		{
			m_ptAStart		= new CPointD( 1, 1 );	
			m_ptBStart		= new CPointD( 2, 0 );
			m_ptBEnd		= new CPointD( 2, 2 );
			CPointD ptResult= jGMA.CP_Pnt2Sect( m_ptAStart, m_ptBStart, m_ptBEnd );	
	
			CPointD ptValue = new CPointD( 2.0, 1.0 );
			if( !ptResult.IsEquals( ptValue ) )
				return false;

			return true;
		}

		/// <summary>
		/// 求点到直线的垂足
		/// </summary>
		public bool Test_CP_Pnt2Line()
		{
			m_ptAStart		= new CPointD( 1, 1 );	
			m_ptBStart		= new CPointD( 2, 0 );
			m_ptBEnd		= new CPointD( 2, 2 );
			CPointD ptResult= jGMA.CP_Pnt2Line( m_ptAStart, m_ptBStart, m_ptBEnd );	
	
			CPointD ptValue = new CPointD( 2.0, 1.0 );
			if( !ptResult.IsEquals( ptValue ) )
				return false;

			return true;
		}
		
		/// <summary>
		/// 点到折线、矩形、多边形的最近交点
		/// </summary>
		public bool Test_CP_Pnt2Polyline()
		{
			m_ptAStart		= new CPointD( 1, 1 );	
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 2, 0 ) );
			arPt.Add( new CPointD( 3, 3 ) );
			arPt.Add( new CPointD( 1, 2 ) );
			CPointD ptResult= jGMA.CP_Pnt2Polyline( m_ptAStart, arPt );	
	
			CPointD ptValue = new CPointD( 1, 2 );
			if( !ptResult.IsEquals( ptValue ) )
				return false;

			return true;
		}
		
		/// <summary>
		/// 两线段的交点
		/// </summary>
		public bool Test_CP_Sect2Sect()
		{
			m_ptAStart		= new CPointD( 1, 0 );	
			m_ptAEnd		= new CPointD( 1, 2 );
			m_ptBStart		= new CPointD( 2, 0 );
			m_ptBEnd		= new CPointD( 1, 1 );
			double dbA = 0, dbB = 0;
			CPointD ptResult = jGMA.CP_Sect2Sect( m_ptAStart, m_ptAEnd, m_ptBStart, m_ptBEnd, ref dbA, ref dbB );	
	
			CPointD ptValue = new CPointD( 1, 1 );
			if( !ptResult.IsEquals( ptValue ) )
				return false;

			return true;
		}

		/// <summary>
		/// 计算两条直线的交点坐标
		/// </summary>
		public bool Test_CP_Line2Line()
		{
			m_ptAStart		= new CPointD( 1, 1 );	
			m_ptAEnd		= new CPointD( 1, 2 );
			m_ptBStart		= new CPointD( 2, 0 );
			m_ptBEnd		= new CPointD( 1, 2 );
			CPointD ptResult = jGMA.CP_Line2Line( m_ptAStart, m_ptAEnd, m_ptBStart, m_ptBEnd );	
	
			CPointD ptValue = new CPointD( 1.0, 2.0 );
			if( !ptResult.IsEquals( ptValue ) )
				return false;

			return true;
		}
		
		/// <summary>
		/// 线段与折线、矩形、多边形的交点
		/// </summary>
/*		public bool Test_CP_Sect2Polygon()
		{
			m_ptAStart		= new CPointD( 1, 1 );	
			m_ptAEnd		= new CPointD( 1, 2 );
			ArrayList arPt	= new ArrayList();
			arPt.Add( new CPointD( 2, 0 ) );
			arPt.Add( new CPointD( 0, 2 ) );
			arPt.Add( new CPointD( 2, 2 ) );
			ArrayList arResult;
			bool bResult = jGMA.CP_Sect2Polygon( m_ptAStart, m_ptAEnd, arPt, out arResult );	
	
			if( !bResult )
				return false;
			ArrayList arValue = new ArrayList();
			arValue.Add( new CPointD( 1, 1 ) );
			arValue.Add( new CPointD( 1, 2 ) );
			if( !(CPointD)arResult[0].IsEquals( (CPointD)arValue[0] ) && !(CPointD)arResult[1].IsEquals( (CPointD)arValue[1] ) )
				return false;

			return true;
		}
*/		
		/// <summary>
		/// 点到圆的最近距离交点
		/// </summary>
		public bool Test_CP_Pnt2Circle()
		{
			m_ptAStart		= new CPointD( 1, 1 );	
			m_ptBStart		= new CPointD( 2, 1 );
			CPointD ptResult;
			bool bResult = jGMA.CP_Pnt2Circle( m_ptAStart, m_ptBStart, 1, out ptResult );	
	
			if( !bResult )
				return false;
			CPointD ptValue = new CPointD( 1, 1 );
			if( !ptResult.IsEquals( ptValue ) )
				return false;

			return true;
		}
		
		/// <summary>
		/// 线段或直线与圆交点
		/// </summary>
		public bool Test_CP_Sect2Circle()
		{
			m_ptAStart		= new CPointD( 1, 1 );	
			m_ptAEnd		= new CPointD( 1, 3 );
			m_ptBStart		= new CPointD( 2, 2 );
			ArrayList arResult;
			bool bResult = jGMA.CP_Sect2Circle( m_ptAStart, m_ptAEnd,  m_ptBStart, 1, out arResult );	
	
			if( !bResult )
				return false;
			ArrayList arValue = new ArrayList();
			arValue.Add( new CPointD( 1, 2 ) );
			if( !((CPointD)arResult[0]).IsEquals( (CPointD)arValue[0] ) )
				return false;

			return true;
		}
		
		#endregion
	}
}
