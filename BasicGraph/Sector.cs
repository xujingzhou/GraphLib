// ============================================================================
// 类名：CSector
// 说明：扇形图元,包括：扇形、弧形和弓形
// 附注：属性按类中设置自定义顺序在属性页中显示，使用PropertySorter类实现，
//		 在被使用类上声明[TypeConverter(typeof(PropertySorter))]，每个属性上设置声明
//		 PropertyOrder(ID)[其中ID为要进行自定义排序的数字序号，如1-10]既可。
// 编写：徐景周
// 日期：2006.03.16
// =============================================================================
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using Jurassic.Graph.GraphCore;
using Jurassic.Graph.Drawer.Symbol;	
using Jurassic.Graph.Base;
using Jurassic.Graph.Drawer.Plotter;
using Jurassic.Graph.Coordinate;
using Jurassic.Graph.EarthSystem;

namespace Jurassic.Graph.BasicGraph
{
	/// <summary>
	/// 样式，包括：扇形、弧形和弓形
	/// </summary>
	public enum SectorType
	{
		/// <summary>
		/// 扇形
		/// </summary>
		Sector,	
	
		/// <summary>
		/// 弧形
		/// </summary>
		Arc,	

		/// <summary>
		/// 弓形
		/// </summary>
		Arch				
	}

	/// <summary>
	/// 扇形、弧形、弓形图元，jingzhou xu,2006.3.27
	/// </summary>
	[TypeConverter(typeof(PropertySorter))]
	public class CSector : Jurassic.Graph.BasicGraph.CGraphRectRangeBase
	{
		#region 成员变量

		/// <summary>
		/// 是绘制扇形、弧形或弓形
		/// </summary>
		private SectorType m_DrawType	= SectorType.Sector;		

		/// <summary>
		/// 填充、边界样式
		/// </summary>
		private CSurfaceStyle m_Style	= new CSurfaceStyle();		

		/// <summary>
		/// 起始角度 
		/// </summary>
		private float m_startAngle		= 0.0F;

		/// <summary>
		/// 起始到终止间弧度值
		/// </summary>
		private float m_sweepAngle		= 270.0F;

		#endregion

		#region 属性

		[BrowsableAttribute(false)]
		/// <summary>
		/// 几何类型，扇形、弧形、弓形。
		/// </summary>
		public override GMTypes GMType
		{
			get
			{
				switch( m_DrawType )
				{
					case SectorType.Arch:
						return GMTypes.Arch;

					case SectorType.Arc:
						return GMTypes.Arc;

					case SectorType.Sector:
						return GMTypes.Sector;

					default:
						return GMTypes.Sector;
				}

			}
		}

		/// <summary>
		/// 是绘制是绘制扇形、弧形或弓形
		/// </summary>
		[Description("绘制样式"),Category("快捷外观"),PropertyOrder(0)]
		public SectorType DrawType
		{
			get
			{
				return m_DrawType;
			}
			set
			{
				m_DrawType = value;
			}
		}

		/// <summary>
		/// 是否绘制边界
		/// </summary>
		[Description("是否绘制边界线"),Category("快捷外观"),PropertyOrder(1)]
		public bool IsBorderVisible
		{
			get
			{
				return m_Style.SurfaceCurveStyle.Enable;
			}
			set
			{
				m_Style.SurfaceCurveStyle.Enable = value;
			}
		}

		/// <summary>
		/// 是否填充
		/// </summary>
		[Description("是否进行内部填充"),Category("快捷外观"),PropertyOrder(2)]
		public bool IsFill
		{
			get
			{
				return m_Style.SurfaceFillStyel.Enable;
			}
			set
			{
				m_Style.SurfaceFillStyel.Enable = value;
			}
		}

		/// <summary>
		/// 边线颜色
		/// </summary>
		[Description("边界线颜色"),Category("快捷外观"),PropertyOrder(3)]
		public Color Color
		{
			get
			{
				return m_Style.SurfaceCurveStyle.CurveColor;
			}
			set
			{
				m_Style.SurfaceCurveStyle.CurveColor = value;
			}
		}

		/// <summary>
		///  边线画笔宽度
		/// </summary>
		[Description("边界线宽度"),Category("快捷外观"),PropertyOrder(4)]
		public float PenWidth
		{
			get
			{
				return m_Style.SurfaceCurveStyle.CurveWidth;
			}
			set
			{
				m_Style.SurfaceCurveStyle.CurveWidth = value;
			}
		}

		/// <summary>
		///  边界线画笔线形
		/// </summary>
		[Description("边界线线形"),Category("快捷外观"),PropertyOrder(5)]
		public DashStyle PenStyle
		{
			get
			{
				// 间断是否是虚线或曲线库
				if( m_Style.SurfaceCurveStyle.GetType() == typeof(CDashLineStyle) )
					return ((CDashLineStyle)m_Style.SurfaceCurveStyle).LineType;
				else
					return DashStyle.Solid;
			}
			set
			{
				((CDashLineStyle)m_Style.SurfaceCurveStyle).LineType = value;
			}
		}

		/// <summary>
		/// 起始角度.
		/// </summary>
		[Description("起始角度值"),Category("快捷外观"),PropertyOrder(5)]
		[RefreshProperties(RefreshProperties.All)]
		public float StartAngle
		{
			get 
			{ 
				return m_startAngle;
			}
			set 
			{ 
				m_startAngle = value; 
			}
		}

		/// <summary>
		/// 起始到终止间弧度值
		/// </summary>
		[Description("旋转角度值"),Category("快捷外观"),PropertyOrder(6)]
		[RefreshProperties(RefreshProperties.All)]
		public float SweepAngle
		{
			get 
			{ 
				return m_sweepAngle; 
			}
			set 
			{ 
				m_sweepAngle = value; 
			}
		}

		// ----------------------------------------------------------------------------------
		// 以下属性只为属性页中显示、设置所用，内部不用，jingzhou xu.2006.4.6

		/// <summary>
		/// 面样式, jingzhou xu
		/// </summary>
		[Description("面样式"),PropertyOrder(11)]
		public CSurfaceStyle SurfaceStyle
		{
			get
			{
				return m_Style;
			}
			set
			{
				m_Style = value;
			}
		}

		// ----------------------------------------------------------------------------------

		#endregion

		#region 涵数

		/// <summary>
		/// 默认构造
		/// </summary>
		public CSector()
		{
			Bounds						= new CRectD( 0, 0, 1, 1 );

			// 填充、边界样式
			m_Style.SurfaceCurveStyle	= new CDashLineStyle();
			m_Style.SurfaceFillStyel	= new CSolidColorFillStyle();
		}

		/// <summary>
		/// 指定矩形构造
		/// </summary>
		/// <param name="rctF">矩形</param>
		public CSector( RectangleF rctF )
		{
			Bounds						= new CRectD( rctF );

			// 填充、边界样式
			m_Style.SurfaceCurveStyle	= new CDashLineStyle();
			m_Style.SurfaceFillStyel	= new CSolidColorFillStyle();
		}

		/// <summary>
		/// 指定矩形范围构造
		/// </summary>
		/// <param name="x">左</param>
		/// <param name="y">上</param>
		/// <param name="width">宽</param>
		/// <param name="height">高</param>
		public CSector( int x, int y, int width, int height )
		{
			Bounds						= new CRectD( x, y, width, height );
			
			// 填充、边界样式
			m_Style.SurfaceCurveStyle	= new CDashLineStyle();
			m_Style.SurfaceFillStyel	= new CSolidColorFillStyle();
		}

		/// <summary>
		///  利用路径绘制弓形,jingzhou xu, 2006.03.01
		/// </summary>
		/// <param name=plotter>绘制机</param>
		/// <param name="rectF">矩形</param>
		public void DrawArch( IPlotter plotter, RectangleF rectF )
		{
			GraphicsPath path = new GraphicsPath( FillMode.Winding );
			
			path.AddArc( rectF, this.StartAngle, this.SweepAngle );

			// 封闭路径
			path.CloseFigure();

			// ----------------------------------------------------
			// 涉及圆弧，在自身中进行绘制操作
			Pen pen			= new Pen( Color, (float)PenWidth );
			// 边界线线形，jingzhou xu,2005.9.2
			pen.DashStyle	= PenStyle;

			// 绘制路径边界线
			if( IsBorderVisible )
				plotter.PlotterG.DrawPath( pen, path );

			pen.Dispose();
			// ----------------------------------------------------

			// 用路径填充内部
			if( IsFill )
				plotter.DrawPolygon( path, m_Style.SurfaceFillStyel );
		}

		/// <summary>
		/// 扇形、弧形共用绘制涵数，jingzhou xu，2006.03.31
		/// </summary>
		/// <param name="plotter">绘制机</param>
		/// <param name="rectF">矩形</param>
		public void DrawSectorOrArc( IPlotter plotter, RectangleF rectF )
		{
			GraphicsPath path = new GraphicsPath( FillMode.Winding );

			switch( m_DrawType )		
			{
				case SectorType.Sector:		// 扇形
					path.AddPie( rectF.X, rectF.Y, rectF.Width, rectF.Height, this.StartAngle, this.SweepAngle );
					break;
					
				case SectorType.Arc:		// 弧形
					path.AddArc( rectF, this.StartAngle, this.SweepAngle );
					break;

				default:
					path.AddPie( rectF.X, rectF.Y, rectF.Width, rectF.Height, this.StartAngle, this.SweepAngle );
					break;
			}

			// ----------------------------------------------------
			// 涉及圆弧，在自身中进行绘制操作
			Pen pen			= new Pen( Color, (float)PenWidth );
			// 边界线线形，jingzhou xu,2005.9.2
			pen.DashStyle	= PenStyle;

			// 绘制路径边界线
			if( IsBorderVisible )
				plotter.PlotterG.DrawPath( pen, path );

			pen.Dispose();
			// ----------------------------------------------------

			// 用路径填充(弧不填充)内部
			if( m_DrawType != SectorType.Arc && IsFill )
				plotter.DrawPolygon( path, m_Style.SurfaceFillStyel );

		}

		/// <summary>
		/// 绘制扇形、弧形、弓形共用涵数
		/// </summary>
		/// <param name=plotter>绘制机</param>
		/// <param name="rectF">矩形</param>
		public void DrawCommon( IPlotter plotter, RectangleF rectF )
		{
			if( m_DrawType == SectorType.Arch )					// 弓形
				DrawArch( plotter, rectF );
			else												// 扇形或弧形
				DrawSectorOrArc( plotter, rectF );
			
		}
		
		/// <summary>
		/// 绘制旋转图元，jingzhou xu，2006.03.31
		/// </summary>
		/// <param name="plotter">绘制机</param>
		/// <param name="fAngle">旋转角度</param>
		public void DrawRotate( IPlotter plotter, float fAngle )
		{
			
			// 旋转处理，jingzhou xu
			CPointD ptRotateOrg		= Bounds.CenterPoint.Clone();	// 获取图元的旋转基准点
	
			// 图元的包容矩形(存储坐标)
			CRectD rcBound			= Bounds.Clone();	

			// ----------------------------------------------------------------
			if( fAngle != 0.0f )
			{
				// 图元旋转方式更新，jingzhou xu,2005.10.24
				plotter.PlotterG.TranslateTransform( (float)ptRotateOrg.X, (float)ptRotateOrg.Y );
				plotter.PlotterG.RotateTransform( fAngle );
				rcBound.Offset( -ptRotateOrg );
			}

			// 调用共用绘制涵数，jingzhou xu
			DrawCommon( plotter, rcBound.toRectangleF() );
			// ----------------------------------------------------------------
			
		}

		/// <summary>
		/// 正常绘制
		/// </summary>
		/// <param name="plotter">绘制机</param>
		/// <param name="status">状态</param>
		public override void Draw( IPlotter plotter, DrawStatus status )
		{
			if( status == DrawStatus.Selected )
			{
				// 绘制选中轮廓线
				DrawOutline( plotter, status );
				return;
			}

			Graphics g			= plotter.PlotterG;

			SmoothingMode sMode	= g.SmoothingMode ;
			g.SmoothingMode		= SmoothingMode.AntiAlias ;
	
			GraphicsState state	= g.Save();

			// -------------------------------------------------------------------------------------------------
			// 边界笔宽从毫米转换为世界坐标，进行绘制
			float oldPenWidth	= m_Style.SurfaceCurveStyle.CurveWidth;
			m_Style.SurfaceCurveStyle.CurveWidth = (float)CUnitCovert.Convert( plotter.Coordinate.MapUnit, enumLinearMmeasure.Millimeter, m_Style.SurfaceCurveStyle.CurveWidth );
			plotter.Coordinate.TransformLen( CoordinateSpaceEx.World, CoordinateSpaceEx.Map, m_Style.SurfaceCurveStyle.CurveWidth );

			// 填充中符号填充中大小从毫米转换为世界坐标，进行绘制
			CSizeD oldSize = new CSizeD();
			if( m_Style.SurfaceFillStyel.GetType() == typeof(CSymbolFillStyle) )			// 符号填充
			{
				oldSize	= ((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size.Clone();	
				((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size.Width	= CUnitCovert.Convert( plotter.Coordinate.MapUnit, enumLinearMmeasure.Millimeter, ((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size.Width );
				((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size.Height	= CUnitCovert.Convert( plotter.Coordinate.MapUnit, enumLinearMmeasure.Millimeter, ((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size.Height );
				plotter.Coordinate.TransformSize( CoordinateSpaceEx.World, CoordinateSpaceEx.Map, ((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size );
				((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size.Width = Math.Abs(((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size.Width);
				((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size.Height = Math.Abs(((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size.Height);
			}
			// ---------------------------------------------------------------------------------------------------

			// 角度不为0，旋转绘制，否则正常绘制.
			if( Angle != 0.0f )
			{

				DrawRotate( plotter, Angle );
			}
			else
			{

				// ---------------------------------------------------------------------------------------------------------------
				CRectD rcD	= Bounds.Clone();
				
				// 调用共用绘制涵数，jingzhou xu
				DrawCommon( plotter, rcD.toRectangleF() );
				// ---------------------------------------------------------------------------------------------------------------
			}

			// ---------------------------------------------------------------------------------------------------
			// 恢复符号填充为原始单位
			if( m_Style.SurfaceFillStyel.GetType() == typeof(CSymbolFillStyle) )			// 符号填充
			{
				((CSymbolFillStyle)m_Style.SurfaceFillStyel).PointStyle.Size = oldSize;
			}
			// 恢复线宽为原始单位
			m_Style.SurfaceCurveStyle.CurveWidth = oldPenWidth;
			// ---------------------------------------------------------------------------------------------------

			g.Restore( state );
			g.SmoothingMode	= sMode ;	
		}

		/// <summary>
		/// 移动，由于Bounds返回的是Clone，故此外用成员m_Bound。jingzhou xu
		/// </summary>
		/// <param name="offset">移动量</param>
		public override void Move( CPointD offset )
		{
			
			m_Bound.Offset( offset );
			
		}

		/// <summary>
		/// 旋转图元
		/// </summary>
		/// <param name="fAngle">旋转角度</param>
		public override void Rotate( float fAngle )
		{
		}

		#endregion

		#region 辅助涵数

		
		#endregion

	}
}
