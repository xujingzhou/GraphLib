using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.ComponentModel;
using Jurassic.Graph.Base;

namespace Jurassic.Graph.Drawer.Symbol
{
	/// <summary>
	/// 
	/// </summary>
	public class CSymText : Jurassic.Graph.Drawer.Symbol.CSymbolElement
	{
		#region  变量
		public	string					m_FontName;	        //字体名称
		public	FontStyle				m_FontStyle;		//字体风格		
		public	Color					m_FontColor;		//文字颜色
		public	float					m_FontSize;		    //文字大小
		public	string					m_strText;			//文字内容
		public	double					m_ptX;		        //文字左上角的X坐标
		public	double					m_ptY;		        //文字右上角的Y坐标
	//	public	Font					m_font;		        //属性字体
		#endregion
		public CSymText()
		{
			// 
			// TODO: 在此处添加构造函数逻辑
			//
		}
		public override  PointD RotateOrigin
		{
			get
			{
				return new PointD(m_ptX,m_ptY);
			}
		}
		/// <summary>
		/// 从xml元素读取数据
		/// </summary>
		/// <param name="elm"></param>
		/// <returns></returns>
		public override bool Read( System.Xml.XmlDocument doc,System.Xml.XmlElement elm)
		{
			this.m_strText = elm.GetAttribute("text");			
			this.m_FontName = elm.GetAttribute("fontname");
			string strcolor  = elm.GetAttribute("fontcolor");
			this.m_FontSize = float.Parse(elm.GetAttribute("fontsize"));
			string fontstylestr =  elm.GetAttribute("fontstyle");
			m_ptX = double.Parse(elm.GetAttribute("x"));
			m_ptY = double.Parse(elm.GetAttribute("y"));
			this.Angle = float.Parse(elm.GetAttribute("rotate"));	
			int col=Convert.ToInt32(strcolor,16);
			this.m_FontColor = 	Color.FromArgb(col);			

			FontStyle fontstyle = FontStyle.Regular;
			if(fontstylestr.IndexOf("Bold") > -1)
			{
				fontstyle = fontstyle | FontStyle.Bold;
			}
			if(fontstylestr.IndexOf("Italic") > -1)
			{
				fontstyle = fontstyle | FontStyle.Italic;
			}		
			if(fontstylestr.IndexOf("Regular") > -1)
			{
				fontstyle = fontstyle | FontStyle.Regular;
				string str = fontstyle.ToString();
			}
			if(fontstylestr.IndexOf("Strikeout") > -1)
			{
				fontstyle = fontstyle | FontStyle.Strikeout;
				string str = fontstyle.ToString();
			}
			if(fontstylestr.IndexOf("Underline") > -1)
			{
				fontstyle = fontstyle | FontStyle.Underline;
				string str = fontstyle.ToString();
			}
			this.m_FontStyle = fontstyle;
	//		this.m_font = new Font(this.m_FontName,this.m_FontSize,this.m_FontStyle,GraphicsUnit.Millimeter);

			return true;
		}
		/// <summary>
		/// 将数据写到xml元素
		/// </summary>
		/// <param name="elm"></param>
		/// <returns></returns>
		public override bool Write(System.Xml.XmlDocument doc,System.Xml.XmlElement elm)
		{

			elm.SetAttribute("text","",this.m_strText);

			elm.SetAttribute("fontname","",this.m_FontName);

			elm.SetAttribute("fontstyle","",this.m_FontStyle.ToString());

			elm.SetAttribute("fontcolor","",this.m_FontColor.ToArgb().ToString("X"));		
	
			elm.SetAttribute("fontsize","",this.m_FontSize.ToString());

			elm.SetAttribute("x","",this.m_ptX.ToString());

			elm.SetAttribute("y","",this.m_ptY.ToString());

			elm.SetAttribute("rotate","",this.Angle.ToString());	
			return true;
		}
	}
}
