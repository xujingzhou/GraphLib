using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using Jurassic.Graph.Base;
namespace Jurassic.Graph.Drawer.Symbol
{
	/// <summary>
	/// 符号曲线样式。
	/// </summary>
	public enum CurveType
	{
		/// <summary>
		/// 绘边界线
		/// </summary>
		Border,		
		/// <summary>
		/// 面充填
		/// </summary>
		Fill,	
		/// <summary>
		/// 绘边界线和面充填
		/// </summary>
		Both				
	}
/*
	/// <summary>
	///  曲线光滑方法。
	/// </summary>
	public enum SmoothMothodName
	{
		Parabola,
		Bezier,
		Spline,
		none				
	}
*/
	/// <summary>
	/// 曲线符号，从CSymbolElement派生。
    /// 编写：徐景周，2006.4.10
	/// </summary>
	public class CSymCurve : Jurassic.Graph.Drawer.Symbol.CSymbolElement
	{
	//	CPointDCollection m_pts = new CPointDCollection();
		
		#region 成员变量

		/// <summary>
		/// 点数组
		/// </summary>
		public ArrayList	m_pts				= new ArrayList();

		/// <summary>
		/// 曲线绘制类型
		/// </summary>
		public CurveType 	m_nType				= CurveType.Border;					
		/// <summary>
		/// 边线的线型
		/// </summary>
		public DashStyle	m_nBorderStyle		= DashStyle.Solid;		

		/// <summary>
		/// 边线的宽度
		/// </summary>
		public double		m_fBorderWidth		= 0.1;					
		/// <summary>
		/// 边线的颜色
		/// </summary>
		public Color		m_clrBorderColor	= Color.Black;			
		/// <summary>
		/// 填充颜色
		/// </summary>
		public Color		m_clrFillColor		= Color.Transparent;	 

		#endregion

		/// <summary>
		/// CSymCurve构造涵数，无参
		/// </summary>
		public CSymCurve()
		{
		}

		/// <summary>
		/// 从xml元素读取数据
		/// </summary>
		/// <param name="elm">包含数据的xml节点</param>
		/// <returns>是否成功</returns>
		public override bool Read( System.Xml.XmlElement elm )
		{
			string attr;

			attr			= elm.GetAttribute( "type" );
			m_nType			= (CurveType)Enum.Parse( typeof(CurveType), attr, true );

			attr			= elm.GetAttribute("borderstyle");
			m_nBorderStyle	= (DashStyle)Enum.Parse( typeof(DashStyle), attr, true );

			attr			= elm.GetAttribute("borderwidth");
			m_fBorderWidth	= double.Parse( attr );

			// 边界色
			attr			= elm.GetAttribute( "bordercolor" );
			Color color		= Color.Empty;
			try
			{
				color		= Color.FromArgb( Convert.ToInt32( attr, 16 ) );
			}
			catch
			{
				color		= Color.FromName( attr ); 
			}
			m_clrBorderColor= color;

			// 填充色
			attr			= elm.GetAttribute( "fillcolor" );
			color			= Color.Empty;
			try
			{
				color		= Color.FromArgb( Convert.ToInt32( attr, 16 ) );
			}
			catch
			{
				color		= Color.FromName( attr ); 
			}
			m_clrFillColor	= color;

		//	m_strSmoothType = elm.GetAttribute("smoothtype");
		//	m_ISmooth = CCurveStyleComposite.CreateSmoothInstance(m_strSmoothType);


			PointF pt		= new PointF();

			XmlNode currNode = elm.FirstChild;		// <Point> Node

			while (currNode != null)
			{
				attr = ( (XmlElement)currNode ).GetAttribute( "x", "" );
				pt.X = float.Parse( attr );
				attr = ( (XmlElement)currNode ).GetAttribute( "y", "" );
				pt.Y = float.Parse( attr );
				m_pts.Add(pt);
				currNode = currNode.NextSibling;
			}

			return true;
		}

		/// <summary>
		/// 将数据写到xml元素
		/// </summary>
		/// <param name="elm">包含数据的xml节点</param>
		/// <returns>是否成功</returns>
		public override bool Write( System.Xml.XmlElement elm )
		{
			elm.SetAttribute( "type", "", m_nType.ToString() );
			elm.SetAttribute( "borderstyle", "", m_nBorderStyle.ToString() );
			elm.SetAttribute( "borderwidth", "", m_fBorderWidth.ToString() );
			elm.SetAttribute( "bordercolor", "", m_clrBorderColor.ToArgb().ToString("X") );				// 转成16进制
			elm.SetAttribute( "fillcolor", "", m_clrFillColor.ToArgb().ToString("X") );
	//		elm.SetAttribute( "smoothtype", "", m_strSmoothType.ToString() );

			for( int i = 0; i < m_pts.Count; i++ )
			{
				XmlElement DataElem = elm.OwnerDocument.CreateElement( "Point", elm.NamespaceURI );		// 创建子节点DataPoint

				XmlAttribute attr = DataElem.SetAttributeNode( "x", "" );
				attr.Value	= ( (PointF)m_pts[i] ).X.ToString();
				attr		= DataElem.SetAttributeNode( "y", "" );
				attr.Value	= ( (PointF)m_pts[i] ).Y.ToString();

				elm.AppendChild( DataElem );
			}
			
			return true;
		}
	}
}
