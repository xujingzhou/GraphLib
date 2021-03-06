using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using Jurassic.Graph.Base;
namespace Jurassic.Graph.Drawer.Symbol
{
	public enum CurveType
	{
		Border,				//绘线
		Fill,				//面充填
		Both				//绘线和面充填
	}
	//曲线光滑方法的中文名称
	//为在属性页上显示中文使用
	public enum SmoothMothodName
	{
		Parabola,
		Bezier,
		Spline,
		none				
	}
	/// <summary>
	/// 
	/// </summary>
	public class CSymCurve : Jurassic.Graph.Drawer.Symbol.CSymbolElement
	{
	//	CPointDCollection m_pts = new CPointDCollection();
		public ArrayList m_pts = new ArrayList();

		#region 成员变量

		/// <summary>
		/// 属性变量
		/// </summary>
		/// 
		public	CurveType 	m_nType	= CurveType.Border;				//曲线绘制类型
		public DashStyle	m_nBorderStyle		= DashStyle.Solid;	//边线的线型

		public double		m_fBorderWidth		= 0.1;				//边线的宽度(单位：毫米)
		public Color		m_clrBorderColor	= Color.Black;		//边线的颜色
		public Color		m_clrFillColor		= Color.Transparent;	//填充颜色

		#endregion
		public CSymCurve()
		{
		}
		/// <summary>
		/// 从xml元素读取数据
		/// </summary>
		/// <param name="elm"></param>
		/// <returns></returns>
		public override bool Read( System.Xml.XmlDocument doc,System.Xml.XmlElement elm)
		{
			string attr;

			attr = elm.GetAttribute("type");
			m_nType = (CurveType)Enum.Parse(typeof(CurveType), attr);

			attr = elm.GetAttribute("borderstyle");
			m_nBorderStyle = (DashStyle)Enum.Parse(typeof(DashStyle), attr);

			attr = elm.GetAttribute("borderwidth");
			m_fBorderWidth = double.Parse(attr);

			attr = elm.GetAttribute("bordercolor");
			m_clrBorderColor = Color.FromArgb( Convert.ToInt32(attr, 16) );		//读16进制数据
			attr = elm.GetAttribute("fillcolor");
			m_clrFillColor = Color.FromArgb( Convert.ToInt32(attr, 16) );

		//	m_strSmoothType = elm.GetAttribute("smoothtype");
		//	m_ISmooth = CCurveStyleComposite.CreateSmoothInstance(m_strSmoothType);


			PointF pt = new PointF();

			XmlNode currNode = elm.FirstChild;		//<Point> Node

			while (currNode != null)
			{
				attr = ( (XmlElement)currNode ).GetAttribute("x", "");
				pt.X = float.Parse(attr);
				attr = ( (XmlElement)currNode ).GetAttribute("y", "");
				pt.Y = float.Parse(attr);
				m_pts.Add(pt);
				currNode = currNode.NextSibling;
			}
			return true;
		}
		/// <summary>
		/// 将数据写到xml元素
		/// </summary>
		/// <param name="elm"></param>
		/// <returns></returns>
		public override bool Write(System.Xml.XmlDocument doc,System.Xml.XmlElement elm)
		{
			elm.SetAttribute( "type", "", m_nType.ToString() );
			elm.SetAttribute( "borderstyle", "", m_nBorderStyle.ToString() );
			elm.SetAttribute( "borderwidth", "", m_fBorderWidth.ToString() );
			elm.SetAttribute( "bordercolor", "", m_clrBorderColor.ToArgb().ToString("X") );		//转成16进制
			elm.SetAttribute( "fillcolor", "", m_clrFillColor.ToArgb().ToString("X") );
	//		elm.SetAttribute( "smoothtype", "", m_strSmoothType.ToString() );


			
			for (int i = 0; i < m_pts.Count; i++)
			{
				XmlElement DataElem = doc.CreateElement("Point",elm.NamespaceURI);		//创建子节点DataPoint

				XmlAttribute attr = DataElem.SetAttributeNode("x", "");
				attr.Value = ((PointF)m_pts[i]).X.ToString();
				attr = DataElem.SetAttributeNode("y", "");
				attr.Value = ((PointF)m_pts[i]).Y.ToString();

				elm.AppendChild(DataElem);
			}
			
			return true;
		}
	}
}
