// ============================================================================
// 类名：CSymSector
// 说明：符号库专用椭圆、扇形图元
// 编写：徐景周
// 日期：2006.3.2
// =============================================================================
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Jurassic.Graph.Base;

namespace Jurassic.Graph.Drawer.Symbol
{
	/// <summary>
	/// 椭圆、扇形图元，从CSymbolElement派生。
	/// </summary>
	//	编写：徐景周，2006.1.10
	public class CSymSector : Jurassic.Graph.Drawer.Symbol.CSymbolElement
	{
		#region 成员变量

		private bool		m_bDrawArc			= false;			// 是绘制扇形还是弧形(true = 弧形<椭圆>， false = 扇形)
		private bool		m_bBorderVisible	= true;				// 是否绘制边界线
		private bool		m_bFill				= false;			// 是否填充

		private Color		m_clrFill			= Color.Transparent;// 填充色
		private Color		m_clrEdge			= Color.Black;		// 边界颜色
		private float		m_fPenWidth			= 0.1f;				// 边界画笔宽度
		private DashStyle	m_penStyle			= DashStyle.Solid;	// 边界画笔线形，jingzhou xu,2005.9.2

		/// <summary>
		/// 起始角度 
		/// </summary>
		private float		m_fStartAngle		= 0.0F;

		/// <summary>
		/// 起始到终止角度间弧度值
		/// </summary>
		private float		m_fSweepAngle		= 360.0F;

		/// <summary>
		/// 椭圆外包矩形
		/// </summary>
		public CRectD		m_rcBound			= new CRectD( 0, 0, 0, 0);

		#endregion

		#region 属性

		/// <summary>
		/// 是绘制扇形还是弧形(true = 弧形(椭圆)， false = 扇形)
		/// </summary>
		[BrowsableAttribute(false)]
		[Description("绘制弧线还是扇形"),Category("外观")]
		public bool IsDrawArc
		{
			get
			{
				return m_bDrawArc;
			}
			set
			{
				m_bDrawArc = value;
			}
		}

		/// <summary>
		/// 是否绘制边界
		/// </summary>
		[Description("是否绘制边界线"),Category("外观")]
		public bool IsBorderVisible
		{
			get
			{
				return m_bBorderVisible;
			}
			set
			{
				m_bBorderVisible = value;
			}
		}

		/// <summary>
		/// 是否填充
		/// </summary>
		[Description("是否进行内部填充"),Category("外观")]
		public bool IsFill
		{
			get
			{
				return m_bFill;
			}
			set
			{
				m_bFill = value;
			}
		}

		/// <summary>
		/// 填充色
		/// </summary>
		[Description("填充色"),Category("外观")]
		public Color FillColor
		{
			get
			{
				return m_clrFill;
			}
			set
			{
				m_clrFill			= value;
			}
		}

		/// <summary>
		/// 边界颜色
		/// </summary>
		[Description("边界线颜色"),Category("外观")]
		public Color EdgeColor
		{
			get
			{
				return m_clrEdge;
			}
			set
			{
				m_clrEdge = value;
			}
		}

		/// <summary>
		///  画笔宽度
		/// </summary>
		[Description("边界线宽度"),Category("外观")]
		public float PenWidth
		{
			get
			{
				return m_fPenWidth;
			}
			set
			{
				m_fPenWidth = value;
			}
		}

		/// <summary>
		///  边界线画笔线形
		/// </summary>
		[Description("边界线线形"),Category("外观")]
		public DashStyle PenStyle
		{
			get
			{
				return m_penStyle;
			}
			set
			{
				m_penStyle = value;
			}
		}

		/// <summary>
		/// 起始角度.
		/// </summary>
		[Description("起始角度值"),Category("外观")]
		[RefreshProperties(RefreshProperties.All)]
		public float StartAngle
		{
			get 
			{ 
				return m_fStartAngle;
			}
			set 
			{ 
				m_fStartAngle = value; 
				// 当角度大于等于360度时，利用弧线绘制椭圆，jingzhou xu,2005.9.5
				if( Math.Abs( m_fStartAngle ) >= 360.0F )
					m_bDrawArc	= true;
				else
					m_bDrawArc	= false;
			}
		}

		/// <summary>
		/// 起始到终止间弧度值
		/// </summary>
		[Description("旋转角度值"),Category("外观")]
		[RefreshProperties(RefreshProperties.All)]
		public float SweepAngle
		{
			get 
			{ 
				return m_fSweepAngle; 
			}
			set 
			{ 
				m_fSweepAngle = value; 
				// 当角度大于等于360度时，利用弧线绘制椭圆，jingzhou xu,2005.9.5
				if( Math.Abs( m_fSweepAngle ) >= 360.0F )
					m_bDrawArc	= true;
				else
					m_bDrawArc	= false;
			}
		}

		#endregion 

		#region 涵数

		/// <summary>
		/// CSymSector构造涵数，无参
		/// </summary>
		public CSymSector()
		{

			// 初始构造填充画刷
			Initialize();

		}

		/// <summary>
		/// CSymSector构造涵数
		/// </summary>
		/// <param name="x">起点水平位置</param>
		/// <param name="y">起点垂直位置</param>
		/// <param name="width">宽度</param>
		/// <param name="height">高度</param>
        public CSymSector( int x, int y, int width, int height )
        {
			
			// 初始构造填充画刷
			Initialize();
        }

		/// <summary>
		/// 从xml元素读取数据
		/// </summary>
		/// <param name="elm">包含数据的xml节点</param>
		/// <returns>是否成功</returns>
		//	编写：jingzhou xu,2005.9.5
		public override bool Read( System.Xml.XmlElement elm )
		{

			// 是否绘制边线与填充
			string tmpStr			= elm.GetAttribute("type");
			if( String.Compare( tmpStr, "Both", true ) == 0 )
			{
				 m_bBorderVisible	= true;
				 m_bFill			= true;
			}
			else if( String.Compare( tmpStr, "Border", true )  == 0 )
			{
				 m_bBorderVisible	= true;
				 m_bFill			= false;
			}
			else
			{
				 m_bBorderVisible	= false;
				 m_bFill			= true;
			}

			// 类型：椭圆还是扇形
			tmpStr					= elm.GetAttribute("shape");
			if( String.Compare( tmpStr, "Ellipse", true )  == 0 )
			{
				 m_bDrawArc			= true;
			}
			else 
				 m_bDrawArc			= false;
			
			
			// 边线类型
			tmpStr					= elm.GetAttribute("borderstyle");
			if( String.Compare( tmpStr, "Solid", true )  == 0 )
			{
				 m_penStyle			= DashStyle.Solid;
			}
			else if( String.Compare( tmpStr, "Dash", true )  == 0 )
			{
				 m_penStyle			= DashStyle.Dash;
			}
			else if( String.Compare( tmpStr, "Dot", true )  == 0 )
			{
				 m_penStyle			= DashStyle.Dot;
			}
			else if( String.Compare( tmpStr, "DashDot", true )  == 0 )
			{
				 m_penStyle			= DashStyle.DashDot;
			}
			else if( String.Compare( tmpStr, "DashDotDot", true )  == 0 )
			{
				 m_penStyle			= DashStyle.DashDotDot;
			}
			else
			{
				 m_penStyle			= DashStyle.Solid;
			}
			
			// 边界线宽度
			 m_fPenWidth			= float.Parse( elm.GetAttribute( "borderwidth" ) );

			// 边界线颜色
			string strcolor			= elm.GetAttribute( "bordercolor" );
			Color color				= Color.Empty;
			try
			{
				color				= Color.FromArgb( Convert.ToInt32( strcolor, 16 ) );
			}
			catch
			{
				color				= Color.FromName( strcolor ); 
			}
			m_clrEdge				= color;	
			

			// 填充色(用渐变起始色)
			strcolor				= elm.GetAttribute( "fillcolor" );
			color					= Color.Empty;
			try
			{
				color				= Color.FromArgb( Convert.ToInt32( strcolor, 16 ) );
			}
			catch
			{
				color				= Color.FromName( strcolor ); 
			}
			m_clrFill				= color;


			// -----------------------------------------------------------------------------------
			// 圆心坐标
			double dbCenterX, dbCenterY;
			dbCenterX				= double.Parse( elm.GetAttribute( "x" ) );
			dbCenterY				= double.Parse( elm.GetAttribute( "y" ) );

			// 宽度和高度
			double dbWidth, dbHeight;
			dbWidth					= double.Parse( elm.GetAttribute( "width" ) );
			dbHeight				= double.Parse( elm.GetAttribute( "height" ) );


			// 重设新的外包容矩形大小，jingzhou xu
			 m_rcBound				= new CRectD( dbCenterX - dbWidth/2, dbCenterY - dbHeight/2, dbWidth, dbHeight );	
			// -----------------------------------------------------------------------------------

			// 起始角度和起始到终止之间角度(度)
			 m_fStartAngle			= float.Parse( elm.GetAttribute( "start" ) );
			 m_fSweepAngle			= float.Parse( elm.GetAttribute( "end" ) );

			// 旋转角度(度)
			Angle					= float.Parse( elm.GetAttribute( "rotate" ) );
		
			return true;
		}

		/// <summary>
		/// 将数据写到xml元素
		/// </summary>
		/// <param name="elm">包含数据的xml节点</param>
		/// <returns>是否成功</returns>
		//	编写：jingzhou xu,2005.9.5
		public override bool Write( System.Xml.XmlElement elm )
		{

			// 是否绘制边线与填充
			if(  m_bBorderVisible && m_bFill )			// 绘制边界线并填充
				elm.SetAttribute("type", "", "Both");
			else if(  m_bBorderVisible )				// 绘制边界线
				elm.SetAttribute("type", "", "Border");
			else										// 填充
				elm.SetAttribute("type", "", "Fill");

			// 类型：椭圆还是扇形
			if(  m_bDrawArc )
				elm.SetAttribute("shape", "", "Ellipse");
			else
				elm.SetAttribute("shape", "", "Arc");
			
			// 边线类型
			switch(  m_penStyle )
			{
				case DashStyle.Solid:
					elm.SetAttribute("borderstyle", "", "Solid");
					break;

				case DashStyle.Dash:
					elm.SetAttribute("borderstyle", "", "Dash");
					break;

				case DashStyle.Dot:
					elm.SetAttribute("borderstyle", "", "Dot");
					break;

				case DashStyle.DashDot:
					elm.SetAttribute("borderstyle", "", "DashDot");
					break;

				case DashStyle.DashDotDot:
					elm.SetAttribute("borderstyle", "", "DashDotDot");
					break;

				default:
					elm.SetAttribute("borderstyle", "", "Solid");
					break;
			}
			
			// 边界线宽度
			elm.SetAttribute("borderwidth", "",  m_fPenWidth.ToString());

			// 边界线颜色
			elm.SetAttribute("bordercolor", "",  m_clrEdge.ToArgb().ToString("X"));

			// 填充色(用渐变起始色)
			elm.SetAttribute("fillcolor", "",  m_clrFill.ToArgb().ToString("X"));

			// 圆心坐标
			elm.SetAttribute("x", "",  m_rcBound.CenterPoint.X.ToString());
			elm.SetAttribute("y", "", m_rcBound.CenterPoint.Y.ToString());

			// 宽度和高度
			elm.SetAttribute("width", "", m_rcBound.Width.ToString());
			elm.SetAttribute("height", "", m_rcBound.Height.ToString());

			// 起始角度和起始到终止之间角度(度)
			elm.SetAttribute("start", "", m_fStartAngle.ToString());
			elm.SetAttribute("end", "", m_fSweepAngle.ToString());

			// 旋转角度(度)
			elm.SetAttribute("rotate", "", Angle.ToString());

			return true;
		}

		#endregion

		#region 其它涵数

		// --------------------------------------------------------------------------------------------------

		/// <summary>
		/// 初始化
		/// </summary>
		private  void Initialize()
		{

			// 当角度大于等于360度时，利用弧线绘制椭圆，jingzhou xu,2005.9.5
			if( Math.Abs( this.m_fSweepAngle ) >= 360.0F )
				m_bDrawArc	= true;
			else
				m_bDrawArc	= false;

		}
		// --------------------------------------------------------------------------------------------------

		#endregion

	}
}
