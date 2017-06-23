using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;		// ����

using Jurassic.Graph.Base;

namespace TestSymbol
{
	/// <summary>
	/// ��jGMA���������㷨������Ҫ���ܽ��в���
	/// ��д���쾰�ܣ�2006.2.27
	/// </summary>
	public class TestGeoAlgorithm
	{
		#region ��Ա����

		public CPointD m_ptAStart, m_ptAEnd, m_ptBStart, m_ptBEnd;

		#endregion

		#region ���캭��

		public TestGeoAlgorithm()
		{
//			m_ptAStart = m_ptAEnd = m_ptBStart = m_ptBEnd = new CPointD();
		}

		#endregion

		#region �ۺϲ���

		/// <summary>
		/// �ۺϲ���
		/// </summary>
		public bool TestAll()
		{
			string strResult = "���Խ����\n";
			if( Test_DotProduce() )
				strResult += "Test_DotProduce������� ==> �ɹ�\n";
			else
				strResult += "Test_DotProduce������� ==> ʧ��\n";

			if( Test_CrossProduce() )
				strResult += "Test_CrossProduce������� ==> �ɹ�\n";
			else
				strResult += "Test_CrossProduce������� ==> ʧ��\n";

			// ����ָ���
			strResult += "==============================================================\n";

			if( Test_Distance() )
				strResult += "Test_Distance������� ==> �ɹ�\n";
			else
				strResult += "Test_Distance������� ==> ʧ��\n";

			if( Test_Distance2() )
				strResult += "Test_Distance2���벻�������� ==> �ɹ�\n";
			else
				strResult += "Test_Distance2���벻�������� ==> ʧ��\n";

			if( Test_Dist_Pnt2Line() )
				strResult += "Test_Dist_Pnt2Sect����ֱ�ߵľ������ ==> �ɹ�\n";
			else
				strResult += "Test_Dist_Pnt2Sect����ֱ�ߵľ������ ==> ʧ��\n";

			if( Test_Dist_Pnt2Sect() )
				strResult += "Test_Dist_Pnt2Sect�����߶εľ������ ==> �ɹ�\n";
			else
				strResult += "Test_Dist_Pnt2Sect�����߶εľ������ ==> ʧ��\n";

			if( Test_Dist_Pnt2Polyline() )
				strResult += "Test_Dist_Pnt2Polyline�������ߡ����ߡ�����ε����������� ==> �ɹ�\n";
			else
				strResult += "Test_Dist_Pnt2Polyline�������ߡ����ߡ�����ε����������� ==> ʧ��\n";

			// ����ָ���
			strResult += "==============================================================\n";

			if( Test_Side_Inflexion() )
				strResult += "Test_Side_Inflexion���߶εĹ����ж� ==> �ɹ�\n";
			else
				strResult += "Test_Side_Inflexion���߶εĹ����ж� ==> ʧ��\n";

			if( Test_Side_PntOnLine() )
				strResult += "Test_Side_PntOnLine��������ֱ�ߵ���һ�� ==> �ɹ�\n";
			else
				strResult += "Test_Side_PntOnLine��������ֱ�ߵ���һ�� ==> ʧ��\n";

			if( Test_Side_PntOnPolyline() )
				strResult += "Test_Side_PntOnPolyline�����������ߵ���һ�� ==> �ɹ�\n";
			else
				strResult += "Test_Side_PntOnPolyline�����������ߵ���һ�� ==> ʧ��\n";

			if( Test_Side_PolylineOnLine() )
				strResult += "Test_Side_PolylineOnLine������ֱ�ߵ���һ�� ==> �ɹ�\n";
			else
				strResult += "Test_Side_PolylineOnLine������ֱ�ߵ���һ�� ==> ʧ��\n";

			if( Test_Side_RectOnLine() )
				strResult += "Test_Side_RectOnLine������ֱ�ߵ���һ�� ==> �ɹ�\n";
			else
				strResult += "Test_Side_RectOnLine������ֱ�ߵ���һ�� ==> ʧ��\n";

			if( Test_Side_SectOnLine() )
				strResult += "Test_Side_SectOnLine�߶���ֱ�ߵ���һ�� ==> �ɹ�\n";
			else
				strResult += "Test_Side_SectOnLine�߶���ֱ�ߵ���һ�� ==> ʧ��\n";

			// ����ָ���
			strResult += "==============================================================\n";

			if( Test_IsPntOnPnt() )
				strResult += "Test_IsPntOnPnt���Ƿ�����һ���� ==> �ɹ�\n";
			else
				strResult += "Test_IsPntOnPnt���Ƿ�����һ���� ==> ʧ��\n";

			if( Test_IsPntOnSect() )
				strResult += "Test_IsPntOnSect���Ƿ����߶��� ==> �ɹ�\n";
			else
				strResult += "Test_IsPntOnSect���Ƿ����߶��� ==> ʧ��\n";

			if( Test_IsPntOnLine() )
				strResult += "Test_IsPntOnLine���Ƿ���ֱ���� ==> �ɹ�\n";
			else
				strResult += "Test_IsPntOnLine���Ƿ���ֱ���� ==> ʧ��\n";

			if( Test_IsPntOnPolyline() )
				strResult += "Test_IsPntOnPolyline���Ƿ��������� ==> �ɹ�\n";
			else
				strResult += "Test_IsPntOnPolyline���Ƿ��������� ==> ʧ��\n";

			if( Test_IsPntInPolygon() )
				strResult += "Test_IsPntInPolygon���Ƿ��ڶ�����ڲ� ==> �ɹ�\n";
			else
				strResult += "Test_IsPntInPolygon���Ƿ��ڶ�����ڲ� ==> ʧ��\n";

			if( Test_IsPntInRect() )
				strResult += "Test_IsPntInRect���Ƿ��ھ����ڲ� ==> �ɹ�\n";
			else
				strResult += "Test_IsPntInRect���Ƿ��ھ����ڲ� ==> ʧ��\n";

			if( Test_IsPntInCircle() )
				strResult += "Test_IsPntInCircle���Ƿ���Բ�� ==> �ɹ�\n";
			else
				strResult += "Test_IsPntInCircle���Ƿ���Բ�� ==> ʧ��\n";

			// ����ָ���
			strResult += "==============================================================\n";

			if( Test_IsSectCrossSect() )
				strResult += "Test_IsSectCrossSect���߶��Ƿ��ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsSectCrossSect���߶��Ƿ��ཻ ==> ʧ��\n";

			if( Test_IsSectCrossLine() )
				strResult += "Test_IsSectCrossLine�߶���ֱ���Ƿ��ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsSectCrossLine�߶���ֱ���Ƿ��ཻ ==> ʧ��\n";

			if( Test_IsLineCrossLine() )
				strResult += "Test_IsLineCrossLineֱ����ֱ���Ƿ��ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsLineCrossLineֱ����ֱ���Ƿ��ཻ ==> ʧ��\n";

			if( Test_IsSectCrossPolyline() )
				strResult += "Test_IsSectCrossPolyline�߶��������Ƿ��ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsSectCrossPolyline�߶��������Ƿ��ཻ ==> ʧ��\n";

			if( Test_IsLineCrossPolyline() )
				strResult += "Test_IsLineCrossPolylineֱ���������Ƿ��ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsLineCrossPolylineֱ���������Ƿ��ཻ ==> ʧ��\n";

			if( Test_IsPolylineCrossPolyline() )
				strResult += "Test_IsPolylineCrossPolyline�����Ƿ�����һ�����ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsPolylineCrossPolyline�����Ƿ�����һ�����ཻ ==> ʧ��\n";

			if( Test_IsSectCrossRect() )
				strResult += "Test_IsSectCrossRect�߶��Ƿ�������ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsSectCrossRect�߶��Ƿ�������ཻ ==> ʧ��\n";

			if( Test_IsLineCrossRect() )
				strResult += "Test_IsLineCrossRectֱ���Ƿ�������ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsLineCrossRectֱ���Ƿ�������ཻ ==> ʧ��\n";

			if( Test_IsPolylineCrossRect() )
				strResult += "Test_IsPolylineCrossRect�����Ƿ�������ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsPolylineCrossRect�����Ƿ�������ཻ ==> ʧ��\n";

			if( Test_IsSectCrossPolygon() )
				strResult += "Test_IsSectCrossPolygon�߶��Ƿ��������ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsSectCrossPolygon�߶��Ƿ��������ཻ ==> ʧ��\n";

			if( Test_IsLineCrossPolygon() )
				strResult += "Test_IsLineCrossPolygonֱ���Ƿ��������ཻ ==> �ɹ�\n";
			else
				strResult += "Test_IsLineCrossPolygonֱ���Ƿ��������ཻ ==> ʧ��\n";

			// ����ָ���
			strResult += "==============================================================\n";

			if( Test_IsSectInRect() )
				strResult += "Test_IsSectInRect�߶��Ƿ��ھ����ڲ� ==> �ɹ�\n";
			else
				strResult += "Test_IsSectInRect�߶��Ƿ��ھ����ڲ� ==> ʧ��\n";

			if( Test_IsPolylineInRect() )
				strResult += "Test_IsPolylineInRect�߶Ρ����ߡ�������Ƿ��ھ����ڲ� ==> �ɹ�\n";
			else
				strResult += "Test_IsPolylineInRect�߶Ρ����ߡ�������Ƿ��ھ����ڲ� ==> ʧ��\n";

			if( Test_IsRectInRect() )
				strResult += "Test_IsRectInRect�����Ƿ��ھ����ڲ� ==> �ɹ�\n";
			else
				strResult += "Test_IsRectInRect�����Ƿ��ھ����ڲ� ==> ʧ��\n";

			if( Test_IsCircleInRect() )
				strResult += "Test_IsCircleInRectԲ�Ƿ��ھ����ڲ� ==> �ɹ�\n";
			else
				strResult += "Test_IsCircleInRectԲ�Ƿ��ھ����ڲ� ==> ʧ��\n";

			if( Test_IsSectInPolygon() )
				strResult += "Test_IsSectInPolygon�߶��Ƿ��ڶ�����ڲ� ==> �ɹ�\n";
			else
				strResult += "Test_IsSectInPolygon�߶��Ƿ��ڶ�����ڲ� ==> ʧ��\n";

			if( Test_IsPolylineInPolygon() )
				strResult += "Test_IsPolylineInPolygon�����Ƿ��ڶ���������� ==> �ɹ�\n";
			else
				strResult += "Test_IsPolylineInPolygon�����Ƿ��ڶ���������� ==> ʧ��\n";

			if( Test_IsPolygonInPolygon() )
				strResult += "Test_IsPolygonInPolygon������Ƿ��ڶ���������� ==> �ɹ�\n";
			else
				strResult += "Test_IsPolygonInPolygon������Ƿ��ڶ���������� ==> ʧ��\n";

			if( Test_IsRectInPolygon() )
				strResult += "Test_IsRectInPolygon�����Ƿ��ڶ���������� ==> �ɹ�\n";
			else
				strResult += "Test_IsRectInPolygon�����Ƿ��ڶ���������� ==> ʧ��\n";

			if( Test_IsCircleInPolygon() )
				strResult += "Test_IsCircleInPolygonԲ�Ƿ��ڶ���������� ==> �ɹ�\n";
			else
				strResult += "Test_IsCircleInPolygonԲ�Ƿ��ڶ���������� ==> ʧ��\n";

			if( Test_IsPolylineInCircle() )
				strResult += "Test_IsPolylineInCircle�߶Ρ����ߡ�������Ƿ���Բ�� ==> �ɹ�\n";
			else
				strResult += "Test_IsPolylineInCircle�߶Ρ����ߡ�������Ƿ���Բ�� ==> ʧ��\n";

			if( Test_IsCircleInCircle() )
				strResult += "Test_IsCircleInCircleԲһ�Ƿ���Բ���� ==> �ɹ�\n";
			else
				strResult += "Test_IsCircleInCircleԲһ�Ƿ���Բ���� ==> ʧ��\n";

			// ����ָ���
			strResult += "==============================================================\n";

			if( Test_CP_Pnt2Sect() )
				strResult += "Test_CP_Pnt2Sect�㵽�߶ε�������� ==> �ɹ�\n";
			else
				strResult += "Test_CP_Pnt2Sect�㵽�߶ε�������� ==> ʧ��\n";

			if( Test_CP_Pnt2Line( ) )
				strResult += "Test_CP_Pnt2Line�㵽ֱ�ߵĴ��� ==> �ɹ�\n";
			else
				strResult += "Test_CP_Pnt2Line�㵽ֱ�ߵĴ��� ==> ʧ��\n";

			if( Test_CP_Pnt2Polyline() )
				strResult += "Test_CP_Pnt2Polyline�㵽���ߡ����Ρ�����ε�������� ==> �ɹ�\n";
			else
				strResult += "Test_CP_Pnt2Polyline�㵽���ߡ����Ρ�����ε�������� ==> ʧ��\n";

			if( Test_CP_Sect2Sect() )
				strResult += "Test_CP_Sect2Sect���߶εĽ��� ==> �ɹ�\n";
			else
				strResult += "Test_CP_Sect2Sect���߶εĽ��� ==> ʧ��\n";

			if( Test_CP_Line2Line() )
				strResult += "Test_CP_Line2Line��ֱ�ߵĽ��� ==> �ɹ�\n";
			else
				strResult += "Test_CP_Line2Line��ֱ�ߵĽ��� ==> ʧ��\n";

/*			if( Test_CP_Sect2Polygon() )
				strResult += "Test_CP_Sect2Polygon�߶������ߡ����Ρ�����εĽ��� ==> �ɹ�\n";
			else
				strResult += "Test_CP_Sect2Polygon�߶������ߡ����Ρ�����εĽ��� ==> ʧ��\n";
*/

			if( Test_CP_Pnt2Circle() )
				strResult += "Test_CP_Pnt2Circle�㵽Բ��������뽻�� ==> �ɹ�\n";
			else
				strResult += "Test_CP_Pnt2Circle�㵽Բ��������뽻�� ==> ʧ��\n";

			if( Test_CP_Sect2Circle() )
				strResult += "Test_CP_Sect2Circle�߶λ�ֱ����Բ���� ==> �ɹ�\n";
			else
				strResult += "Test_CP_Sect2Circle�߶λ�ֱ����Բ���� ==> ʧ��\n";

			MessageBox.Show( strResult, "���������㷨��Ԫ����" );

			return true;
		}

		#endregion

		#region ������

		/// <summary>
		/// �������
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
		/// �������
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

		#region �������

		/// <summary>
		/// �������
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
		/// ��������pt1,pt2֮��ľ���ƽ������ʱ�жϳ��̣����ÿ�������
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
		/// ����ֱ�ߵľ������
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
		/// �����߶εľ������
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
		/// �������ߡ����ߡ�����ε�����������
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
		
		#region ��λ����

		/// <summary>
		/// ���߶εĹ����ж�
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
	
			// ������
			if( dbResult != -1.0 )
				return false;

			return true;
		}

		/// <summary>
		/// ��������ֱ�ߵ���һ��
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
	
			// ���
			if( dbResult != -1.0 )
				return false;

			return true;
		}
		
		/// <summary>
		/// �����������ߵ���һ��
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
	
			// ���
			if( dbResult != -1.0 )
				return false;

			return true;
		}

		/// <summary>
		///  �ж�������ֱ�ߵ���һ��
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
	
			// �Ҳ�
			if( dbResult != 1.0 )
				return false;

			return true;
		}

		/// <summary>
		///  �жϾ�����ֱ�ߵ���һ��
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
	
			// �Ҳ�
			if( dbResult != 1.0 )
				return false;

			return true;
		}

		/// <summary>
		///  �ж��߶���ֱ�ߵ���һ��
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
	
			// �Ҳ�
			if( dbResult != 1.0 )
				return false;

			return true;
		}

		#endregion

		#region �����

		/// <summary>
		/// ���Ƿ��ڵ���
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// ���Ƿ����߶���
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// ���Ƿ���ֱ����
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// ���Ƿ��ھ����ڲ�
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// ���Ƿ���������
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// ���Ƿ��ڶ�����ڲ�
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// ���Ƿ���Բ��
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		#endregion
		
		#region �ཻ����

		/// <summary>
		/// ���߶��Ƿ��ཻ
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
	
			// ���ཻ
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// �߶���ֱ���Ƿ��ཻ
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
	
			// ���ཻ
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		///  �ж��߶��Ƿ�������ཻ
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
	
			// ���ཻ
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// ֱ����ֱ���Ƿ��ཻ
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
	
			// ���ཻ
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		///  �ж�ֱ���Ƿ�������ཻ
		/// </summary>
		public bool Test_IsLineCrossPolyline()
		{
			// 1.ƽ��
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
	
			// ���ཻ
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// ���ߡ����ߡ�������Ƿ�����һ�����ߡ����ߡ�������ཻ
		/// </summary>
		public bool Test_IsPolylineCrossPolyline()
		{
			// 1.ƽ��
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
	
			// ���ཻ
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// �ж��߶��Ƿ�;����ཻ
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// �ж�ֱ���Ƿ�;����ཻ
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// �ж������Ƿ�;����ཻ
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// �ж��߶��Ƿ�Ͷ�����ཻ
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// �ж�ֱ�߶��Ƿ�Ͷ�����ཻ
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		#endregion

		#region �Ƿ����ڲ�����

		/// <summary>
		/// �߶��Ƿ��ھ����ڲ�
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// �߶Ρ����ߡ�������Ƿ��ھ����ڲ�
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// ����rctB�Ƿ��ھ���rctA�ڲ�
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// Բ�Ƿ��ھ����ڲ�
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// �߶��Ƿ��ڶ����������
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// �����Ƿ��ڶ����������
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		/// <summary>
		/// ������Ƿ��ڶ����������
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
			arPtB.Add( new CPointD( 0, 0 ) );				// ���������Ǳպϵģ������һ��Ӧ�͵�һ���غϣ�jingzhou xu
			bool bResult	= jGMA.IsPolygonInPolygon( arPtA, arPtB, null, null );	
	
			// ����
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// �����Ƿ��ڶ����������
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// Բ�Ƿ��ڶ����������
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// �߶Ρ����ߡ�������Ƿ���Բ��
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}
		
		/// <summary>
		/// Բһ�Ƿ���Բ����
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
	
			// ����
			if( !bResult )
				return false;

			return true;
		}

		#endregion
		
		#region �������

		/// <summary>
		/// �㵽�߶ε��������
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
		/// ��㵽ֱ�ߵĴ���
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
		/// �㵽���ߡ����Ρ�����ε��������
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
		/// ���߶εĽ���
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
		/// ��������ֱ�ߵĽ�������
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
		/// �߶������ߡ����Ρ�����εĽ���
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
		/// �㵽Բ��������뽻��
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
		/// �߶λ�ֱ����Բ����
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
