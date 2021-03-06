//// ============================================================================
//// 类名：CShape
//// 说明：多边形特殊图元，包括：五角星、六边形、等边三角形、矩形、圆角矩形、圆、扇形、弧形和弓形等。
//// 附注：属性按类中设置自定义顺序在属性页中显示，使用PropertySorter类实现，
////		 在被使用类上声明[TypeConverter(typeof(PropertySorter))]，每个属性上设置声明
////		 PropertyOrder(ID)[其中ID为要进行自定义排序的数字序号，如1-10]既可。
//// 编写：徐景周
//// 日期：2006.3.28
//// =============================================================================
//using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.ComponentModel;			// [BrowsableAttribute(false)]支持

//using Jurassic.Graph.Drawer.Symbol;	
//using Jurassic.Graph.Base;

//namespace Jurassic.Graph.BasicGraph
//{

//    /// <summary>
//    /// 样式，包括：五角星、六边形、等边三角形、矩形、圆角矩形、圆、扇形、弧形和弓形。
//    /// </summary>
//    public enum ShapeType
//    {
//        /// <summary>
//        /// 矩形
//        /// </summary>
//        Rectangle,	
	
//        /// <summary>
//        /// 圆角矩形
//        /// </summary>
//        RoundRect,		

//        /// <summary>
//        /// 圆
//        /// </summary>
//        Circle,

//        /// <summary>
//        /// 五角星
//        /// </summary>
//        Star,	
	
//        /// <summary>
//        /// 六边形
//        /// </summary>
//        Hexagon,	

//        /// <summary>
//        /// 等边三角形
//        /// </summary>
//        EquilTriangle,
		
//        /// <summary>
//        /// 扇形
//        /// </summary>
//        Sector,	
	
//        /// <summary>
//        /// 弧形
//        /// </summary>
//        Arc,	

//        /// <summary>
//        /// 弓形
//        /// </summary>
//        Arch				
//    }

//    /// <summary>
//    /// 多边形特殊图元（五角星、六边形、等边三角形、矩形、圆角矩形、圆、扇形、弧形、弓形等。）。
//    /// 徐景周，2006.3.28
//    /// </summary>
//    [TypeConverter(typeof(PropertySorter))]
//    public class CShape 
//    {
//        #region 成员变量

//        /// <summary>
//        /// 多边形类型
//        /// </summary>
//        private ShapeType m_ShapeType	= ShapeType.Star;			 

//        /// <summary>
//        /// 填充、边界样式
//        /// </summary>
//        private CSurfaceStyle m_Style	= new CSurfaceStyle();		

//        /// <summary>
//        /// 起始角度(绘制扇形、弧形或弓形) 
//        /// </summary>
//        private float m_startAngle		= 0.0F;

//        /// <summary>
//        /// 起始到终止间弧度值(绘制扇形、弧形或弓形)
//        /// </summary>
//        private float m_sweepAngle		= 270.0F;

//        #endregion

//        #region 属性

//        [BrowsableAttribute(false)]
//            /// <summary>
//            /// 几何类型，五角星、六边形、等边三角形、矩形、圆角矩形、圆、扇形、弧形、弓形。
//            /// </summary>
//        public override GMTypes GMType
//        {
//            get
//            {
//                switch( m_ShapeType )
//                {
//                        // 矩形
//                    case ShapeType.Rectangle:
//                        return GMTypes.Rectangle;

//                        // 圆角矩形
//                    case ShapeType.RoundRect:
//                        return GMTypes.RoundRect;

//                        // 圆
//                    case ShapeType.Circle:
//                        return GMTypes.Circle;

//                        // 五角星
//                    case ShapeType.Star:
//                        return GMTypes.Star;

//                        // 等边三角形
//                    case ShapeType.EquilTriangle:
//                        return GMTypes.EquilTriangle;

//                        // 六边形
//                    case ShapeType.Hexagon:
//                        return GMTypes.Hexagon;

//                        // 弓形
//                    case ShapeType.Arch:
//                        return GMTypes.Arch;

//                        // 弧形
//                    case ShapeType.Arc:
//                        return GMTypes.Arc;

//                        // 扇形
//                    case ShapeType.Sector:
//                        return GMTypes.Sector;

//                    default:
//                        return GMTypes.Rectangle;
//                }

//            }
//        }

//        /// <summary>
//        /// 是绘制是五角星、六边形、等边三角形、矩形、圆角矩形、圆、扇形、弧形、弓形。
//        /// </summary>
//        [Description("绘制样式"),Category("快捷外观"),PropertyOrder(0)]
//        public ShapeType DrawType
//        {
//            get
//            {
//                return m_ShapeType;
//            }
//            set
//            {
//                m_ShapeType = value;
//            }
//        }

//        /// <summary>
//        /// 是否绘制边界
//        /// </summary>
//        [Description("是否绘制边界线"),Category("快捷外观"),PropertyOrder(1)]
//        public bool IsBorderVisible
//        {
//            get
//            {
//                return m_Style.SurfaceCurveStyle.Enable;
//            }
//            set
//            {
//                m_Style.SurfaceCurveStyle.Enable = value;
//            }
//        }

//        /// <summary>
//        /// 是否填充
//        /// </summary>
//        [Description("是否进行内部填充"),Category("快捷外观"),PropertyOrder(2)]
//        public bool IsFill
//        {
//            get
//            {
//                return m_Style.SurfaceFillStyle.Enable;
//            }
//            set
//            {
//                m_Style.SurfaceFillStyle.Enable = value;
//            }
//        }

//        /// <summary>
//        /// 边线颜色
//        /// </summary>
//        [Description("边界线颜色"),Category("快捷外观"),PropertyOrder(3)]
//        public Color Color
//        {
//            get
//            {
//                return m_Style.SurfaceCurveStyle.CurveColor;
//            }
//            set
//            {
//                m_Style.SurfaceCurveStyle.CurveColor = value;
//            }
//        }

//        /// <summary>
//        ///  边线画笔宽度
//        /// </summary>
//        [Description("边界线宽度"),Category("快捷外观"),PropertyOrder(4)]
//        public float PenWidth
//        {
//            get
//            {
//                return m_Style.SurfaceCurveStyle.CurveWidth;
//            }
//            set
//            {
//                m_Style.SurfaceCurveStyle.CurveWidth = value;
//            }
//        }

//        /// <summary>
//        ///  边界线画笔线形
//        /// </summary>
//        [Description("边界线线形"),Category("快捷外观"),PropertyOrder(5)]
//        public DashStyle PenStyle
//        {
//            get
//            {
//                // 判断是否是虚线或曲线库
//                if( m_Style.SurfaceCurveStyle.GetType() == typeof(CDashLineStyle) )
//                {
//                    // 定制暂不用，jingzhou xu
//                    if( ((CDashLineStyle)m_Style.SurfaceCurveStyle).LineType == DashStyle.Custom )
//                        ((CDashLineStyle)m_Style.SurfaceCurveStyle).LineType = DashStyle.Solid;

//                    return ((CDashLineStyle)m_Style.SurfaceCurveStyle).LineType;
//                }
//                else
//                    return DashStyle.Solid;
//            }
//            set
//            {
//                ((CDashLineStyle)m_Style.SurfaceCurveStyle).LineType = value;
//            }
//        }

//        /// <summary>
//        /// 起始角度(绘制扇形、弧形或弓形) 
//        /// </summary>
//        [Description("起始角度值"),Category("快捷外观"),PropertyOrder(6)]
//        [RefreshProperties(RefreshProperties.All)]
//        public float StartAngle
//        {
//            get 
//            { 
//                return m_startAngle;
//            }
//            set 
//            { 
//                m_startAngle = value; 
//            }
//        }

//        /// <summary>
//        /// 起始到终止间弧度值(绘制扇形、弧形或弓形) 
//        /// </summary>
//        [Description("旋转角度值"),Category("快捷外观"),PropertyOrder(7)]
//        [RefreshProperties(RefreshProperties.All)]
//        public float SweepAngle
//        {
//            get 
//            { 
//                return m_sweepAngle; 
//            }
//            set 
//            { 
//                m_sweepAngle = value; 
//            }
//        }

//        /// <summary>
//        /// 面样式, jingzhou xu
//        /// </summary>
//        [Description("面样式"),PropertyOrder(11)]
//        public CSurfaceStyle SurfaceStyle
//        {
//            get
//            {
//                if( m_styleMatch != null )
//                {
//                    if( m_styleMatch.GetStyle() != null )
//                        m_Style = (CSurfaceStyle)m_styleMatch.GetStyle();				
//                }

//                return m_Style;
//            }
//            set
//            {
//                m_Style = value;
//            }
			
//        }


//        #region StyleMatch zhanglian

//        /// <summary>
//        /// 属性页中显示风格匹配类型
//        /// </summary>
//        [RefreshProperties(RefreshProperties.All)]
//        public StyleSelector StyleMatchType
//        {
//            get
//            {
//                if( m_styleMatch != null )
//                {
//                    if( m_styleMatch.GetType() == typeof(CStyleClassMatch) )
//                    {
//                        return StyleSelector.StyleClass;
//                    }
//                    else if( m_styleMatch.GetType() == typeof(CStyleIDMatch) )
//                    {
//                        return StyleSelector.StyleID;
//                    }
//                    else if( m_styleMatch.GetType() == typeof(CGraphTagMatch) )
//                    {
//                        return StyleSelector.GraphTag;
//                    }
//                    else if( m_styleMatch.GetType() == typeof(CGraphIDMatch) )
//                    {
//                        return StyleSelector.GraphID;						
//                    }
//                    else if( m_styleMatch.GetType() == typeof(CDirectStyle) )
//                    {
//                        return StyleSelector.DirectStyle;
//                    }
//                    else if( m_styleMatch.GetType() == typeof(CExceptionMatch) )
//                    {
//                        return StyleSelector.ExceptionStyle;
//                    }
//                }
//                return StyleSelector.StyleID;
//            }
//            set
//            {
//                if( value == StyleSelector.StyleClass )
//                {
//                    m_styleMatch = new CStyleClassMatch();
//                }
//                else if( value == StyleSelector.StyleID )
//                {
//                    m_styleMatch = new CStyleIDMatch();
//                }
//                else if( value == StyleSelector.GraphTag )
//                {
//                    m_styleMatch = new CGraphTagMatch();
//                    ((CGraphTagMatch)m_styleMatch).TagName = this.GetType().ToString().Replace( this.GetType().Namespace + ".", "" );
//                }
//                else if( value == StyleSelector.GraphID )
//                {
//                    m_styleMatch = new CGraphIDMatch();
//                    ((CGraphIDMatch)m_styleMatch).GraphID = ((IDataItem)this).ID;
//                }
//                else if( value == StyleSelector.DirectStyle )
//                {
//                    m_styleMatch = new CDirectStyle();
//                    ((CDirectStyle)m_styleMatch).DirectStyle = m_Style;
//                }
//                else if( value == StyleSelector.ExceptionStyle )
//                {
//                    m_styleMatch = new CExceptionMatch();
//                    ((CExceptionMatch)m_styleMatch).ExceptionStyleMatch = new CDirectStyle();
//                }

//                m_styleMatch.Owner = (IDataItem)this;
//            }
//        }

//        /// <summary>
//        /// 风格匹配
//        /// </summary>
//        private CStyleMatch m_styleMatch = null;
//        [RefreshProperties(RefreshProperties.All)]
//        public override  CStyleMatch StyleMatch
//        {
//            get
//            {				
//                if( m_styleMatch == null )
//                {
//                    StyleMatchType = StyleSelector.StyleID;
//                }

//                return m_styleMatch;
//            }
//            set
//            {					
//                m_styleMatch = value;
//            }
//        }		

//        #endregion

//        #endregion

//        #region 涵数

//        /// <summary>
//        /// 默认构造
//        /// </summary>
//        public CShape()
//        {
//            Bounds						= new CRectD( 0, 0, 1, 1 );

//            // 填充、边界样式
//            m_Style.SurfaceCurveStyle	= new CDashLineStyle();
//            m_Style.SurfaceFillStyle	= new CSolidColorFillStyle();
//        }

//        /// <summary>
//        /// 指定矩形构造
//        /// </summary>
//        /// <param name="rctF">矩形</param>
//        public CShape( RectangleF rctF )
//        {
//            Bounds						= new CRectD( rctF );

//            // 填充、边界样式
//            m_Style.SurfaceCurveStyle	= new CDashLineStyle();
//            m_Style.SurfaceFillStyle	= new CSolidColorFillStyle();
//        }

//        /// <summary>
//        /// 指定矩形范围构造
//        /// </summary>
//        /// <param name="x">左</param>
//        /// <param name="y">上</param>
//        /// <param name="width">宽</param>
//        /// <param name="height">高</param>
//        public CShape( int x, int y, int width, int height )
//        {
//            Bounds						= new CRectD( x, y, width, height );

//            // 填充、边界样式
//            m_Style.SurfaceCurveStyle	= new CDashLineStyle();
//            m_Style.SurfaceFillStyle	= new CSolidColorFillStyle();
//        }

//        /// <summary>
//        ///  利用路径绘制弓形,jingzhou xu, 2006.03.01
//        /// </summary>
//        /// <param name=plotter>绘制机</param>
//        /// <param name="rectF">矩形</param>
//        public void DrawArch( IPlotter plotter, RectangleF rectF )
//        {
//            GraphicsPath path = new GraphicsPath( FillMode.Winding );
			
//            path.AddArc( rectF, this.StartAngle, this.SweepAngle );

//            // 封闭路径
//            path.CloseFigure();

//            // ----------------------------------------------------
//            // 涉及圆弧，在自身中进行绘制操作
//            Pen pen			= new Pen( Color, (float)PenWidth );
//            // 边界线线形，jingzhou xu,2005.9.2
//            pen.DashStyle	= PenStyle;

//            // 绘制路径边界线
//            if( IsBorderVisible )
//                plotter.PlotterG.DrawPath( pen, path );

//            pen.Dispose();
//            // ----------------------------------------------------

//            // 用路径填充内部
//            if( IsFill )
//                plotter.DrawPolygon( path, SurfaceStyle.SurfaceFillStyle );
//        }

//        /// <summary>
//        /// 扇形绘制涵数，jingzhou xu，2006.03.31
//        /// </summary>
//        /// <param name="plotter">绘制机</param>
//        /// <param name="rectF">矩形</param>
//        public void DrawSector( IPlotter plotter, RectangleF rectF )
//        {
//            GraphicsPath path = new GraphicsPath( FillMode.Winding );

//            path.AddPie( rectF.X, rectF.Y, rectF.Width, rectF.Height, this.StartAngle, this.SweepAngle );

//            // ----------------------------------------------------
//            // 涉及圆弧，在自身中进行绘制操作
//            Pen pen			= new Pen( Color, (float)PenWidth );
//            // 边界线线形，jingzhou xu
//            pen.DashStyle	= PenStyle;

//            // 绘制路径边界线
//            if( IsBorderVisible )
//                plotter.PlotterG.DrawPath( pen, path );

//            pen.Dispose();
//            // ----------------------------------------------------

//            // 用路径填充内部
//            if( IsFill )
//                plotter.DrawPolygon( path, SurfaceStyle.SurfaceFillStyle );

//        }

//        /// <summary>
//        /// 弧形绘制涵数，jingzhou xu，2006.03.31
//        /// </summary>
//        /// <param name="plotter">绘制机</param>
//        /// <param name="rectF">矩形</param>
//        public void DrawArc( IPlotter plotter, RectangleF rectF )
//        {
//            GraphicsPath path = new GraphicsPath( FillMode.Winding );

//            path.AddArc( rectF, this.StartAngle, this.SweepAngle );

//            // ----------------------------------------------------
//            // 涉及圆弧，在自身中进行绘制操作
//            Pen pen			= new Pen( Color, (float)PenWidth );
//            // 边界线线形，jingzhou xu
//            pen.DashStyle	= PenStyle;

//            // 绘制路径边界线
//            if( IsBorderVisible )
//                plotter.PlotterG.DrawPath( pen, path );

//            pen.Dispose();
//            // ----------------------------------------------------

//            // 用路径填充(弧不填充)内部
//            //		if( IsFill )
//            //			plotter.DrawPolygon( path, SurfaceStyle.SurfaceFillStyel );

//        }

//        /// <summary>
//        /// 矩形绘制涵数，jingzhou xu，2006.03.31
//        /// </summary>
//        /// <param name="plotter">绘制机</param>
//        /// <param name="rectF">矩形</param>
//        public void DrawRect( IPlotter plotter, RectangleF rectF )
//        {
//            GraphicsPath path = new GraphicsPath( FillMode.Winding );
			
//            path.AddRectangle( rectF );
//            path.AddLine( rectF.X, rectF.Y + rectF.Height, rectF.X, rectF.Y );

//            // 用路径绘制
//            plotter.DrawPolygon( path, SurfaceStyle);

//        }

//        /// <summary>
//        ///  利用路径绘制圆角矩形,jingzhou xu,2006.4.1
//        /// </summary>
//        /// <param name=plotter>绘制机</param>
//        /// <param name="rectF">矩形</param>
//        /// <param name="sizeF">圆角比例</param>
//        public void DrawRoundedRect( IPlotter plotter, RectangleF rectF, SizeF sizeF )
//        {
//            GraphicsPath path = new GraphicsPath( FillMode.Winding );
			
//            path.AddLine( rectF.Left + sizeF.Width/2, rectF.Top, rectF.Right - sizeF.Width/2, rectF.Top );
//            path.AddArc( rectF.Right - sizeF.Width, rectF.Top, sizeF.Width, sizeF.Height, 270, 90 );

//            path.AddLine( rectF.Right, rectF.Top + sizeF.Height/2, rectF.Right, rectF.Bottom - sizeF.Height/2 );
//            path.AddArc( rectF.Right - sizeF.Width, rectF.Bottom - sizeF.Height, sizeF.Width, sizeF.Height, 0, 90 );

//            path.AddLine( rectF.Right - sizeF.Width/2, rectF.Bottom, rectF.Left + sizeF.Width/2, rectF.Bottom );
//            path.AddArc( rectF.Left, rectF.Bottom - sizeF.Height, sizeF.Width, sizeF.Height, 90, 90 );

//            path.AddLine( rectF.Left, rectF.Bottom - sizeF.Height/2, rectF.Left, rectF.Top + sizeF.Height/2 );
//            path.AddArc( rectF.Left, rectF.Top, sizeF.Width, sizeF.Height, 180, 90 );

//            // 封闭路径
//            path.CloseFigure();

//            // ----------------------------------------------------
//            // 涉及圆弧，在自身中进行绘制操作
//            Pen pen			= new Pen( Color, (float)PenWidth );
//            // 边界线线形，jingzhou xu,2005.9.2
//            pen.DashStyle	= PenStyle;

//            // 绘制路径边界线
//            if( IsBorderVisible )
//                plotter.PlotterG.DrawPath( pen, path );

//            pen.Dispose();
//            // ----------------------------------------------------

//            // 用路径填充内部
//            if( IsFill )
//                plotter.DrawPolygon( path, SurfaceStyle.SurfaceFillStyle );

//        }

//        /// <summary>
//        /// 圆，椭圆绘制涵数，2006.3.31
//        /// </summary>
//        /// <param name="plotter">绘制机</param>
//        /// <param name="rectF">矩形</param>
//        public void DrawCircle( IPlotter plotter, RectangleF rectF )
//        {
//            GraphicsPath path = new GraphicsPath( FillMode.Winding );
			
//            path.AddEllipse( rectF );

//            // ----------------------------------------------------
//            // 涉及圆弧，在自身中进行绘制操作
//            Pen pen			= new Pen( Color, (float)PenWidth );
//            // 边界线线形，jingzhou xu,2005.9.2
//            pen.DashStyle	= PenStyle;

//            // 绘制路径边界线
//            if( IsBorderVisible )
//                plotter.PlotterG.DrawPath( pen, path );

//            pen.Dispose();
//            // ----------------------------------------------------

//            // 用路径填充内部
//            if( IsFill )
//                plotter.DrawPolygon( path, SurfaceStyle.SurfaceFillStyle );

//        }

//        /// <summary>
//        /// 五角星绘制涵数，jingzhou xu，2006.3.31
//        /// </summary>
//        /// <param name="plotter">绘制机</param>
//        /// <param name="rectF">矩形</param>
//        public void DrawStar( IPlotter plotter, RectangleF rectF )
//        {
//            GraphicsPath path	= new GraphicsPath( FillMode.Winding );

//            /*			Point[] apt			= new Point[6];
//                        for( int i = 0; i < apt.Length; i++ )
//                        {
//                            double dAngle	= (i*0.8 - 0.5) * Math.PI;
//                            apt[i]			= new Point( 
//                                (int)( rectF.X + rectF.Width * (0.50 + 0.48 * Math.Cos(dAngle)) ),
//                                (int)( rectF.Y + rectF.Height * (0.50 + 0.48 * Math.Sin(dAngle)) )
//                                );
//                        }
 
//                        path.AddPolygon( apt );
//            */
			
//            // 五角星路径
//            PointF[] ptF = new PointF[10];
//            ptF[0].X = rectF.X + (rectF.Width / 2);
//            ptF[0].Y = rectF.Y;
//            ptF[1].X = rectF.X + (42 * rectF.Width / 64);
//            ptF[1].Y = rectF.Y + (19 * rectF.Height / 64);
//            ptF[2].X = rectF.X + rectF.Width;
//            ptF[2].Y = rectF.Y + (22 * rectF.Height / 64);
//            ptF[3].X = rectF.X + (48 * rectF.Width / 64);
//            ptF[3].Y = rectF.Y + (38 * rectF.Height / 64);
//            ptF[4].X = rectF.X + (52 * rectF.Width / 64);
//            ptF[4].Y = rectF.Y + rectF.Height;
//            ptF[5].X = rectF.X + (rectF.Width / 2);
//            ptF[5].Y = rectF.Y + (52 * rectF.Height / 64);
//            ptF[6].X = rectF.X + (12 * rectF.Width / 64);
//            ptF[6].Y = rectF.Y + rectF.Height;
//            ptF[7].X = rectF.X + rectF.Width / 4;
//            ptF[7].Y = rectF.Y + (38 * rectF.Height / 64);
//            ptF[8].X = rectF.X;
//            ptF[8].Y = rectF.Y + (22 * rectF.Height / 64);
//            ptF[9].X = rectF.X + (22 * rectF.Width / 64);
//            ptF[9].Y = rectF.Y + (19 * rectF.Height / 64);

//            path.AddPolygon( ptF );
////			path.AddLine( ptF[9], ptF[0] );

//            // 用路径绘制
//            plotter.DrawPolygon( path, SurfaceStyle);

//        }

//        /// <summary>
//        /// 六边形绘制涵数，jingzhou xu，2006.3.28
//        /// </summary>
//        /// <param name="plotter">绘制机</param>
//        /// <param name="rectF">矩形</param>
//        public void DrawHexagon( IPlotter plotter, RectangleF rectF )
//        {
//            GraphicsPath path	= new GraphicsPath( FillMode.Winding );
			
//            Point[] apt			= new Point[6];
//            double AnglePortion = ( 2 * Math.PI ) / apt.Length;
//            for( int i = 0; i < apt.Length; i++ )
//            {
//                double Angle	= i * AnglePortion;
//                apt[i]			= new Point( 
//                    (int)( rectF.X + rectF.Width * (0.50 + 0.5 * Math.Cos(Angle)) ),
//                    (int)( rectF.Y + rectF.Height * (0.50 + 0.5 * Math.Sin(Angle)) )
//                    );
//            }
 
//            path.AddPolygon( apt );
//            path.AddLine( apt[0], apt[5] );

//            // 封闭路径
//            path.CloseFigure();

//            // 用路径绘制
//            plotter.DrawPolygon( path, SurfaceStyle);

//        }

//        /// <summary>
//        /// 等边三角形绘制涵数，jingzhou xu，2006.3.29
//        /// </summary>
//        /// <param name="plotter">绘制机</param>
//        /// <param name="rectF">矩形</param>
//        public void DrawEquilTriangle( IPlotter plotter, RectangleF rectF )
//        {
//            GraphicsPath path	= new GraphicsPath( FillMode.Winding );

//            PointF[] aptf	= new PointF[3];
//            if( rectF.Width <= rectF.Height )
//            {
//                // ----------------------------------------------------
//                //
//                //		 _______________
//                //		|				|
//                //		|				|
//                //		|	   /|\		|
//                //		|	  /	| \		|
//                //		|	 / ~|~ \	| height
//                //		|	/ 30|30	\	|
//                //		|  /	|垂  \	|
//                //		| /		|线	  \ |
//                //		|/______|______\|
//                //				width
//                //
//                // ----------------------------------------------------
//                float halfWidth		= rectF.Width/2;
//                // 垂线长度 = halfWidth / tan( 30 )
//                float verticalLine	= (float)( halfWidth / Math.Tan( 30*(Math.PI/180) ) );
//                aptf[0] = new PointF( rectF.X + rectF.Width,	rectF.Y + rectF.Height );
//                aptf[1] = new PointF( rectF.X,					rectF.Y + rectF.Height );
//                aptf[2] = new PointF( rectF.X + halfWidth,		rectF.Y + rectF.Height - verticalLine );
//            }
//            else
//            {
//                // ----------------------------------------------------
//                //
//                //		 _______________________________________
//                //		|				   /|\					|
//                //		|				  / | \					|
//                //		|				 / ~|~ \				|
//                //		|				/ 30|30 \				|
//                //		|			   /    |    \				| height
//                //		|			  /     |	  \				|
//                //		|			 /		|      \			|
//                //		|			/		|	    \			|
//                //		|__________/________|_底线___\__________|
//                //							width
//                //
//                // ----------------------------------------------------
//                // 底线长度 = rectF.Height * tan( 30 )
//                float baseLine = (float)( rectF.Height * Math.Tan( 30*(Math.PI/180) ) );
//                aptf[0] = new PointF( rectF.X + rectF.Width/2,				rectF.Y );
//                aptf[1] = new PointF( rectF.X + rectF.Width/2 - baseLine,	rectF.Y + rectF.Height );
//                aptf[2] = new PointF( rectF.X + rectF.Width/2 + baseLine,	rectF.Y + rectF.Height );
//            }

//            path.AddPolygon( aptf );
//            path.AddLine( aptf[2], aptf[0] );

//            // 封闭路径
//            path.CloseFigure();

//            // 用路径绘制
//            plotter.DrawPolygon( path, SurfaceStyle);

//        }

//        /// <summary>
//        /// 共用绘制涵数，jingzhou xu，2006.05.08
//        /// </summary>
//        /// <param name="plotter">绘制机</param>
//        /// <param name="rectF">矩形</param>
//        public void DrawCommon( IPlotter plotter, RectangleF rectF )
//        {
//            switch( m_ShapeType )
//            {
//                    // 矩形
//                case ShapeType.Rectangle:
//                    DrawRect( plotter, rectF );
//                    break;

//                    // 圆角矩形
//                case ShapeType.RoundRect:
//                    DrawRoundedRect( plotter, rectF, new SizeF(rectF.Width/5, rectF.Height/5) );
//                    break;

//                    // 圆
//                case ShapeType.Circle:
//                    DrawCircle( plotter, rectF );
//                    break;

//                    // 五角星
//                case ShapeType.Star:
//                    DrawStar( plotter, rectF );
//                    break;

//                    // 六边形
//                case ShapeType.Hexagon:
//                    DrawHexagon( plotter, rectF );
//                    break;

//                    // 等边三角形
//                case ShapeType.EquilTriangle:
//                    DrawEquilTriangle( plotter, rectF );
//                    break;

//                    // 弓形
//                case ShapeType.Arch:
//                    DrawArch( plotter, rectF );
//                    break;

//                    // 弧
//                case ShapeType.Arc:
//                    DrawArc( plotter, rectF );
//                    break;

//                    // 扇形
//                case ShapeType.Sector:
//                    DrawSector( plotter, rectF );
//                    break;

//                default:
//                    DrawRect( plotter, rectF );
//                    break;
//            }
//        }

//        /// <summary>
//        /// 绘制旋转图元，jingzhou xu，2006.05.08
//        /// </summary>
//        /// <param name="plotter">绘制机</param>
//        /// <param name="fAngle">旋转角度</param>
//        public void DrawRotate( IPlotter plotter, float fAngle )
//        {
//            // 旋转处理，jingzhou xu
//            CPointD ptRotateOrg	= Bounds.CenterPoint.Clone();			// 获取图元的旋转基准点

//            // 图元的包容矩形
//            CRectD rcBound		= Bounds.Clone();	

//            // ----------------------------------------------------------------
//            if( fAngle != 0.0f )
//            {
//                // 图元旋转方式更新，jingzhou xu,2005.10.24
//                plotter.PlotterG.TranslateTransform( (float)ptRotateOrg.X, (float)ptRotateOrg.Y );
//                plotter.PlotterG.RotateTransform( fAngle );
//                rcBound.Offset( -ptRotateOrg );
//            }

//            // 调用共用绘制涵数，jingzhou xu
//            DrawCommon( plotter, rcBound.toRectangle() );
//            // ----------------------------------------------------------------
//        }

//        /// <summary>
//        /// 正常绘制，jingzhou xu，2006.05.08
//        /// </summary>
//        /// <param name="plotter">绘制机</param>
//        /// <param name="status">状态</param>
//        public override void Draw( IPlotter plotter, DrawStatus status )
//        {
//            if( status == DrawStatus.Selected )
//            {
//                // 绘制选中轮廓线
//                DrawOutline( plotter, status );
//                return;
//            }

//            Graphics g			= plotter.PlotterG;

//            SmoothingMode sMode	= g.SmoothingMode;
//            g.SmoothingMode		= SmoothingMode.AntiAlias;	

//            GraphicsState gs	= g.Save();

//            // -------------------------------------------------------------------------------------------------
//            // 边界笔宽从毫米转换为世界坐标，进行绘制
//            float oldPenWidth	= m_Style.SurfaceCurveStyle.CurveWidth;
//            m_Style.SurfaceCurveStyle.CurveWidth = (float)CUnitCovert.Convert( plotter.Coordinate.MapUnit, enumLinearMmeasure.Millimeter, m_Style.SurfaceCurveStyle.CurveWidth );
//            plotter.Coordinate.TransformLen( CoordinateSpaceEx.World, CoordinateSpaceEx.Map, m_Style.SurfaceCurveStyle.CurveWidth );

//            // 填充中符号填充中大小从毫米转换为世界坐标，进行绘制
//            CSizeD oldSize = new CSizeD();
//            if( m_Style.SurfaceFillStyle.GetType() == typeof(CSymbolFillStyle) )			// 符号填充
//            {
//                oldSize	= ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size.Clone();	
//                ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size.Width	= CUnitCovert.Convert( plotter.Coordinate.MapUnit, enumLinearMmeasure.Millimeter, ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size.Width );
//                ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size.Height	= CUnitCovert.Convert( plotter.Coordinate.MapUnit, enumLinearMmeasure.Millimeter, ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size.Height );
//                plotter.Coordinate.TransformSize( CoordinateSpaceEx.World, CoordinateSpaceEx.Map, ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size );
//                ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size.Width	= Math.Abs( ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size.Width );
//                ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size.Height = Math.Abs( ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size.Height );
//            }
//            // ---------------------------------------------------------------------------------------------------

//            // 角度不为0，旋转绘制，否则正常绘制
//            if( Angle != 0.0f )
//            {
//                DrawRotate( plotter, Angle );
//            }
//            else
//            {
//                CRectD rcD	= Bounds;

//                // 调用共用绘制涵数，jingzhou xu
//                DrawCommon( plotter, rcD.toRectangleF() );
//            }

//            // ---------------------------------------------------------------------------------------------------
//            // 恢复符号填充为原始单位
//            if( m_Style.SurfaceFillStyle.GetType() == typeof(CSymbolFillStyle) )			// 符号填充
//            {
//                ((CSymbolFillStyle)m_Style.SurfaceFillStyle).PointStyle.Size = oldSize;
//            }
//            // 恢复线宽为原始单位
//            m_Style.SurfaceCurveStyle.CurveWidth = oldPenWidth;
//            // ---------------------------------------------------------------------------------------------------

//            g.Restore( gs );	
//            g.SmoothingMode	= sMode ;

//        }

//        /// <summary>
//        /// 移动，由于Bounds返回的是Clone，故此外用成员m_Bound。jingzhou xu
//        /// </summary>
//        /// <param name="offset">移动量</param>
//        public override void Move( CPointD offset )
//        {
//            m_Bound.Offset( offset );
//        }

//        /// <summary>
//        /// 旋转图元
//        /// </summary>
//        /// <param name="fAngle">旋转角度</param>
//        public override void Rotate( float fAngle )
//        {
//        }

//        #endregion

//        #region 辅助涵数


//        #endregion

//    }
//}
