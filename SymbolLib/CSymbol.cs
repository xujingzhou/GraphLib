using System;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Collections;
using System.ComponentModel;	// 属性页描述

using Jurassic.Graph.Base;

namespace Jurassic.Graph.Drawer.Symbol
{
	/// <summary>
	/// 缩放样式（Zoom -- 放大，OriginalAspect -- 原始比例 ）。
    /// 编写：徐景周，2006.4.10
	/// </summary>
	public enum ScaleStyles
	{ 
		/// <summary>
		/// 放大
		/// </summary>
		Zoom, 

		/// <summary>
		/// 原始比例
		/// </summary>
		OriginalAspect, 

	};

	/// <summary>
	/// 符号类，包括了符号帧、符号图元，从CSymbolElement派生。
	/// </summary>
	//	编写：徐景周，2006.3.8
	public class CSymbol : Jurassic.Graph.Drawer.Symbol.CSymbolElement
	{
		
		#region 私有成员

		/// <summary>
		///  带分类信息的全名
		/// </summary>
		string m_sFullname;

		/// <summary>
		/// 符号的中心点
		/// </summary>
		CPointD m_ptCenter;

		/// <summary>
		///  子图元是否同步缩放
		/// </summary>
		ScaleStyles m_ScaleStyle		= ScaleStyles.OriginalAspect;

		/// <summary>
		///  子图元是否同步旋转
		/// </summary>
		bool m_bRotate					= true;
		
		/// <summary>
		///  宽度
		/// </summary>
		float m_fWidth					= 100;
		/// <summary>
		///  高度
		/// </summary>
		float m_fHeight					= 100;

		/// <summary>
		///  X方向放大系数 
		/// </summary>
		double m_ScaleX					= 1.0f;
		/// <summary>
		///  Y方向放大系数
		/// </summary>
		double m_ScaleY					= 1.0f;

		/// <summary>
		///  符号内的线宽是否可变(有些符号中的图形在符号缩放时，线的宽度不随之变化)
		/// </summary>
		bool m_bVarWidth				= true;

		/// <summary>
		///  符号背景色
		/// </summary>
		Color m_clrBackground			= Color.Transparent;	

		/// <summary>
		///  子符号元素
		/// </summary>
		CSymElmCollections  m_Children	= new  CSymElmCollections();

		/// <summary>
		///  符号帧集合
		/// </summary>
		CSymbolCollections  m_Frames	= new CSymbolCollections();

		#endregion

		#region 属性

		/// <summary>
		/// 符号背景色
		/// </summary>
		//	编写： jingzhou xu, 2005.12.27
		[Description("符号背景色"),Category("外观")]
		public Color BackColor
		{
			get
			{
				return m_clrBackground;
			}
			set
			{
				m_clrBackground = value;
			}
		}

		/// <summary>
		///  符号内的线宽是否可变(有些符号中的图形在符号缩放时，线的宽度不随之变化)
		/// </summary>
		public bool VarWidth
		{
			get
			{
				return m_bVarWidth;
			}

			set
			{
				m_bVarWidth = value;
				foreach( CSymbolElement item in Children )
				{
					if(item is CSymbol)
					{
						((CSymbol)item).m_bVarWidth = value;
					}
				}
			}
		}

		/// <summary>
		/// 得到符号的X缩放比例
		/// 制作符号时此值恒为1.0
		/// 使用符号时可以改变
		/// </summary>
		public double ScaleX
		{
			get
			{
				return m_ScaleX;
			}

			set
			{
				m_ScaleX = value;
				foreach( CSymbolElement item in Children )
				{
					if(item is CSymbol)
					{
						((CSymbol)item).ScaleX = value;
					}
				}
			}
		}

		/// <summary>
		/// 得到符号的Y方向缩放比例
		/// 制作符号时此值恒为1.0
		/// 使用符号时可以改变
		/// </summary>
		public double ScaleY
		{
			get
			{
				return m_ScaleY;
			}

			set
			{
				m_ScaleY = value;
				foreach( CSymbolElement item in Children )
				{
					if(item is CSymbol)
					{
						((CSymbol)item).ScaleY = value;
					}
				}
			}
		}

		/// <summary>
		/// 符号旋转角度值
		/// </summary>
		public override float Angle
		{
			get 
			{ 
				return m_fAngle; 
			}

			set 
			{ 
				m_fAngle = value;
				foreach( CSymbolElement item in Children )
				{
					if(item is CSymbol)
					{
						((CSymbol)item).Angle = value;
					}
				}
				
			}
		}

		/// <summary>
		/// 符号原始宽度
		/// </summary>
		public float Width
		{
			get
			{
				return m_fWidth;
			}

			set
			{
				m_fWidth = value;
			}
		}	

		/// <summary>
		/// 符号原始高度
		/// </summary>
		public float Height
		{
			get
			{
				return m_fHeight;
			}

			set
			{
				m_fHeight = value;
			}
		}	

		/// <summary>
		/// 符号原整全名
		/// </summary>
		public string SymbolFullName
		{
			get
			{
				return m_sFullname;
			}

			set
			{
				m_sFullname = value;
			}
		}	

		/// <summary>
		/// 符号的中心点
		/// </summary>
		public CPointD CenterPoint
		{
			get
			{
				return m_ptCenter;
			}

			set
			{
				m_ptCenter = value;
			}
		}

		/// <summary>
		/// 符号是否旋转
		/// </summary>
		public bool Rotate
		{
			get
			{
				return m_bRotate;
			}

			set
			{
				m_bRotate = value;
			}
		}

		/// <summary>
		/// 符号缩放风格
		/// </summary>
		public ScaleStyles ScaleStyle
		{
			get
			{
				return m_ScaleStyle;
			}

			set
			{
				m_ScaleStyle = value;
			}
		}
	
		/// <summary>
		/// 子符号集合
		/// </summary>
		public CSymElmCollections Children
		{
			get
			{
				return m_Children;
			}
		}

		/// <summary>
		/// 符号帧集合
		/// </summary>
		public CSymbolCollections Frames
		{
			get
			{
				return m_Frames;
			}
		}

		#endregion

		#region 函数

		/// <summary>
		/// CSymbol构造涵数，无参
		/// </summary>
		public CSymbol()
		{
		}

		/// <summary>
		/// 从xml元素读取数据外围属性
		/// </summary>
		/// <param name="node">XML节点</param>
		private void ReadSymbolHead( System.Xml.XmlElement node )
		{
			SymbolFullName	= node.GetAttribute( "fullname" );

			CPointD center	= new CPointD();
			center.X		= double.Parse( node.GetAttribute( "centerx" ) );
			center.Y		= double.Parse( node.GetAttribute( "centery" ) );
			CenterPoint		= center;
			m_ScaleStyle	= (ScaleStyles)Enum.Parse( typeof(ScaleStyles), node.GetAttribute( "zoomctrl" ), true );
			m_bRotate		= bool.Parse( node.GetAttribute( "rotatable" ) );

			VarWidth		= bool.Parse( node.GetAttribute( "variablewidth" ) );

			// 背景色
			string szColor	= node.GetAttribute( "backcolor" );
			Color color		= Color.Empty;
			try
			{
				color		= Color.FromArgb( Convert.ToInt32( szColor, 16 ) );
			}
			catch
			{
				color		= Color.FromName( szColor ); 
			}
			BackColor		= color;

			// 宽度、高度
			m_fWidth		= float.Parse( node.GetAttribute( "width" ) );
			m_fHeight		= float.Parse( node.GetAttribute( "height" ) );

		}

		/// <summary>
		/// 从xml元素读取数据
		/// </summary>
		/// <param name="node">XML节点</param>
		/// <returns>是否成功</returns>
		public override bool Read( System.Xml.XmlElement node )
		{
			ReadSymbolHead( node );

			foreach( XmlElement sub in node )
			{
				CSymbolElement item = null;
				if( sub.Name == "SymText" )
					item = new CSymText();
				else if( sub.Name == "SymCurve" )
					item = new CSymCurve();
				else if( sub.Name == "SymArc" )
					item = new CSymSector();
				else if( sub.Name == "Symbol" )
					item = new CSymbol();
					// ------------------------------------------
					// 符号帧
				else if( sub.Name == "Frames" )
				{
					if( !sub.HasChildNodes )
						continue;	

					foreach( XmlElement subFrame in sub )
					{
						CSymbol symItem = null;
						if( subFrame.Name == "Symbol" )
							symItem = new CSymbol();

						if( symItem != null )
						{
							symItem.Read( subFrame );								
							this.Frames.Add( symItem );	
						}
					}
				}
					// ------------------------------------------
				else
					continue;

				if( item == null )
					continue;	
		
				item.Read( sub );								
				this.Children.Add( item );				
			}

			return true;
		}

		/// <summary>
		/// 将数据写到xml元素
		/// </summary>
		/// <param name="node">XML节点</param>
		/// <returns>是否成功</returns>
		public override bool Write( System.Xml.XmlElement node )
		{
			// 符号全名
			node.SetAttribute( "fullname", "", SymbolFullName );

			// 中心点
			node.SetAttribute( "centerx", "", this.CenterPoint.X.ToString() );
			node.SetAttribute( "centery", "", this.CenterPoint.Y.ToString() );
			// 宽度、高度
			node.SetAttribute( "width", "", m_fWidth.ToString() );
			node.SetAttribute( "height", "", m_fHeight.ToString() );

			node.SetAttribute( "zoomctrl", "", this.m_ScaleStyle.ToString() );
			node.SetAttribute( "rotatable", "", this.m_bRotate.ToString().ToLower() );

			node.SetAttribute( "variablewidth", "", VarWidth.ToString().ToLower() );
			node.SetAttribute( "backcolor", "", BackColor.ToArgb().ToString("X") );

			
			// 递归写入符号及子符号
			foreach( CSymbolElement item in Children )
			{
				string symName = "Symbol";
				if(item is CSymText)
					symName = "SymText";
				else if(item is CSymCurve)
					symName = "SymCurve";
				else if(item is CSymSector)
					symName = "SymArc";

				XmlElement elmSub = node.OwnerDocument.CreateElement( symName, node.NamespaceURI );
				item.Write( elmSub );
				node.AppendChild( elmSub );
			}

			// -----------------------------------------------------------------------------
			// 符号帧
			if( Frames.Count > 0 )
			{
				XmlElement elmFrame = node.OwnerDocument.CreateElement( "Frames", node.NamespaceURI );

				foreach( CSymbol symFrame in Frames )
				{
					XmlElement elmSymbol = node.OwnerDocument.CreateElement( "Symbol", node.NamespaceURI );
					elmFrame.AppendChild( elmSymbol );
					symFrame.Write( elmSymbol );
					node.AppendChild( elmFrame );
				}
			}
			// -----------------------------------------------------------------------------

			return true;
		}

		#endregion

	}
}
