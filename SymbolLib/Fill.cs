////////////////////////////////////////////////////////////////////////
// 类名：Fill
// 功能：图元填充类，可进行单色，二色渐变，三色渐变，位图等多种填充。
// 附注：对矩形、椭圆、扇形等进行各种填充效果。
// 修订：徐景周
// 组织：引自(http://www.codeproject.com/csharp/zedgraph.asp)
// 日期：2005.7.28
////////////////////////////////////////////////////////////////////////
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Diagnostics;

using Jurassic.Graph.Base;

namespace Jurassic.Graph.Drawer.Symbol
{
	
	/// <exclude/>
	/// <summary>
	/// 已有路径渐变填充样式，jingzhou xu,2005.8.1
	/// </summary>
	public enum PathGradientBrushStyles
	{
//		/// <summary> 无路径渐变画刷 </summary>
//		None,

		/// <summary> 六边形路径渐变画刷 </summary>
		HexagonPathGradientBrush,

		/// <summary> 双三角路径渐变画刷 </summary>
		TwoTrianglePathGradientBrush,

		/// <summary> 菱形路径渐变画刷 </summary>
		DiamondPathGradientBrush,

		/// <summary> 四边形路径渐变画刷 </summary>
		SquarePathGradientBrush,

		/// <summary> 五角星路径渐变画刷 </summary>
		StarPathGradientBrush,

		/// <summary> 球形路径渐变画刷 </summary>
		BallPathGradientBrush,

	};

	/// <exclude/>
	/// <summary>
	/// 画刷填充样式，jingzhou xu
	/// </summary>
	public enum BrushFillStyles
	{
//		/// <summary> 不填充 </summary>
//		None,

		/// <summary> 单色画刷填充 </summary>
		SolidBursh,

		/// <summary> 阴影画刷填充 </summary>
		HatchBrush,

		/// <summary> 线性渐变画刷填充 </summary>
		LinearGradientBrush,

		/// <summary> 路径渐变画刷填充 </summary>
		PathGradientBrush,

		/// <summary> 纹理画刷填充 </summary>
		TextureBrush

	};

	/// <exclude/>
	/// <summary>
	/// Enumeration type for the various types of fills that can be used with 
	/// charts.
	/// </summary>
	public enum FillTypes
	{
		/// <summary> No fill </summary>
		None,
		/// <summary> A solid fill using <see cref="System.Drawing.SolidBrush"/> </summary>
		Solid,
		/// <summary> A custom fill using either <see cref="LinearGradientBrush"/> or
		/// <see cref="TextureBrush"/></summary>
		Brush,
		/// <summary>
		/// Fill with a single solid color based on the X value of the data.</summary>
		/// <remarks>The X value is
		/// used to determine the color value based on a gradient brush, and using a data range
		/// of <see cref="Fill.RangeMin"/> and <see cref="Fill.RangeMax"/>.  You can create a multicolor
		/// range by initializing the <see cref="Fill"/> class with your own custom
		/// <see cref="Brush"/> object based on a <see cref="ColorBlend"/>.  In cases where a
		/// data value makes no sense a default value of 50% of the range is assumed.  The default range is 0 to 1.
		/// </remarks>
		GradientByX,
		/// <summary>
		/// Fill with a single solid color based on the Z value of the data.</summary>
		/// <remarks>The Z value is
		/// used to determine the color value based on a gradient brush, and using a data range
		/// of <see cref="Fill.RangeMin"/> and <see cref="Fill.RangeMax"/>.  You can create a multicolor
		/// range by initializing the <see cref="Fill"/> class with your own custom
		/// <see cref="Brush"/> object based on a <see cref="ColorBlend"/>.  In cases where a
		/// data value makes no sense a default value of 50% of the range is assumed.  The default range is 0 to 1.
		/// </remarks>
		GradientByY,
		/// <summary>
		/// Fill with a single solid color based on the Z value of the data.</summary>
		/// <remarks>The Z value is
		/// used to determine the color value based on a gradient brush, and using a data range
		/// of <see cref="Fill.RangeMin"/> and <see cref="Fill.RangeMax"/>.  You can create a multicolor
		/// range by initializing the <see cref="Fill"/> class with your own custom
		/// <see cref="Brush"/> object based on a <see cref="ColorBlend"/>.  In cases where a
		/// data value makes no sense a default value of 50% of the range is assumed.  The default range is 0 to 1.
		/// </remarks>
		GradientByZ
	};

	/// <exclude/>
	/// <summary>
	/// Enumeration type for the different horizontal text alignment options
	/// </summary>
	public enum AlignHs
	{
		/// <summary>
		/// Position the text so that its left edge is aligned with the
		/// specified X,Y location.  Used by the
		/// </summary>
		Left,
		/// <summary>
		/// Position the text so that its center is aligned (horizontally) with the
		/// specified X,Y location.  Used by the
		/// </summary>
		Center,
		/// <summary>
		/// Position the text so that its right edge is aligned with the
		/// specified X,Y location.  Used by the
		/// </summary>
		Right
	};
	
	/// <exclude/>
	/// <summary>
	/// Enumeration type for the different proximal alignment options
	/// </summary>
	public enum AlignPs
	{
		/// <summary>
		/// Position the text so that its "inside" edge (the edge that is
		/// nearest to the alignment reference point or object) is aligned.
		/// </summary>
		Inside,
		/// <summary>
		/// Position the text so that its center is aligned with the
		/// reference object or point.
		/// </summary>
		Center,
		/// <summary>
		/// Position the text so that its right edge (the edge that is
		/// farthest from the alignment reference point or object) is aligned.
		/// </summary>
		Outside
	};
	
	/// <exclude/>
	/// <summary>
	/// Enumeration type for the different vertical text alignment options
	/// </summary>
	/// specified X,Y location.  Used by the
	public enum AlignVs
	{
		/// <summary>
		/// Position the text so that its top edge is aligned with the
		/// specified X,Y location.  Used by the
		/// </summary>
		Top,
		/// <summary>
		/// Position the text so that its center is aligned (vertically) with the
		/// specified X,Y location.  Used by the
		/// </summary>
		Center,
		/// <summary>
		/// Position the text so that its bottom edge is aligned with the
		/// specified X,Y location.  Used by the
		/// </summary>
		Bottom
	};

	/// <exclude/>
	/// <summary>
	/// 功能：图元填充类，可进行单色，二色渐变，三色渐变，位图等多种填充。
	/// 附注：对矩形、椭圆、扇形等进行各种填充效果。
	/// </summary>
	/// <example>
	///	<code>
	/// 用法：
	///		public Fill  m_fill				= new Fill();		// 图元填充(先默认构造)
	///			
	///		m_fill.StartColor	= Color.Transparent;
	///		m_fill.MidColor		= Color.Transparent;
	///		m_fill.EndColor		= Color.Transparent;
	///		// 构造三色渐变(三种颜色相同为单色)
	///		m_fill.ConstructLinearGradientBrush( m_fill.StartColor, m_fill.MidColor, m_fill.EndColor, 0.0f);
	///
	///		CRectD rcD			= m_Bound;
	///		RectangleF rectF	= new RectangleF((float)rcD.left, (float)rcD.top, (float)rcD.Width(), (float)rcD.Height());
	///		
	///		Brush brush			= m_fill.MakeBrush(rectF);
	///		g.FillPie( brush, rectF.Left, rectF.Top, rectF.Width, rectF.Height, sector.m_startAngle, sector.m_sweepAngle );
	///		brush.Dispose () ;
	///	</code>
	/// </example>
	[Serializable]
	public class Fill : ISerializable
	{
	#region Fields

	// ---------------------------------------------------------------------------------------------
		// 填充画刷样式(默认线性渐变画刷),jingzhou xu
		private BrushFillStyles brushStyle;	//				= BrushFillStyle.LinearGradientBrush;

		// 阴影画刷风格,jingzhou xu
		private HatchStyle hatchBrushStyle; //				= HatchStyle.HorizontalBrick;

		// 已有路径渐变画刷风格(默认六边形画刷),jingzhou xu
		private PathGradientBrushStyles pathBrushStyle; //	= PathGradientBrushStyle.HexagonPathGradientBrush;

		private Color startColor;			//				= Color.Red;	// 填充渐变起始色,jingzhou xu
		private Color midColor;				//				= Color.Green;	// 填充渐变中间色,jingzhou xu
		private Color endColor;				//				= Color.Blue;	// 填充渐变终止色,jingzhou xu

		private Color hatchForegroundColor; //				= Color.White;	// 阴影画刷前景色，jingzhou xu
		private Color hatchBackgroundColor; //				= Color.Black;	// 阴影画刷背景色，jingzhou xu

		private Image textureBrushImage;									// 纹理画刷图案，jingzhou xu

		PathGradientBrush pathBrushOne, pathBrushTwo;						// 路径渐变画刷，jingzhou xu

	// ----------------------------------------------------------------------------------------------

		/// <summary>
		/// Private field that stores the fill color.  Use the public
		/// </summary>
		private Color		color;
		/// <summary>
		/// Private field that stores the custom fill brush. 
		/// </summary>
		private Brush		brush;
		/// <summary>
		/// Private field that determines the type of color fill. 
		/// <see cref="Brush"/>.
		/// </summary>
		private FillTypes	type;
		/// <summary>
		/// Private field that determines if the brush will be scaled to the bounding box
		/// of the filled object.  If this value is false, then the brush will only be aligned
		/// with the filled object based on the <see cref="AlignH"/> and <see cref="AlignV"/>
		/// properties.
		/// </summary>
		private bool		isScaled;
		/// <summary>
		/// Private field that determines how the brush will be aligned with the filled object
		/// in the horizontal direction. 
		/// </summary>
		/// <seealso cref="AlignH"/>
		/// <seealso cref="AlignV"/>
		private AlignHs		alignH;
		/// <summary>
		/// Private field that determines how the brush will be aligned with the filled object
		/// in the vertical direction. 
		/// properties.
		/// </summary>
		/// <seealso cref="AlignH"/>
		/// <seealso cref="AlignV"/>
		private AlignVs		alignV;

		private double	rangeMin;
		private double	rangeMax;
		private Bitmap	gradientBM;

		/// <summary>
		/// 纹理画刷图案
		/// </summary>
		private Image	image;

		/// <summary>
		/// Private field that saves the image wrapmode passed to the constructor.
		/// This is used strictly for serialization.
		/// </summary>
		private WrapMode wrapMode;

		/// <summary>
		/// Private field that saves the list of colors used to create the
		/// <see cref="LinearGradientBrush"/> in the constructor.  This is used strictly
		/// for serialization.
		/// </summary>
		private Color[] colorList;

		/// <summary>
		/// Private field that saves the list of positions used to create the
		/// <see cref="LinearGradientBrush"/> in the constructor.  This is used strictly
		/// for serialization.
		/// </summary>
		private float[] positionList;

		/// <summary>
		/// Private field the saves the angle of the fill.  This is used strictly for serialization.
		/// </summary>
		private float angle;


	#endregion

	#region Defaults
		/// <summary>
		/// A simple struct that defines the
		/// default property values for the <see cref="Fill"/> class.
		/// </summary>
		public struct Default
		{
			// Default Fill properties
			/// <summary>
			/// The default scaling mode for <see cref="Brush"/> fills.
			/// This is the default value for the <see cref="Fill.IsScaled"/> property.
			/// </summary>
			public static bool IsScaled = true;
			/// <summary>
			/// The default horizontal alignment for <see cref="Brush"/> fills.
			/// This is the default value for the <see cref="Fill.AlignH"/> property.
			/// </summary>
			public static AlignHs AlignH = AlignHs.Center;
			/// <summary>
			/// The default vertical alignment for <see cref="Brush"/> fills.
			/// This is the default value for the <see cref="Fill.AlignV"/> property.
			/// </summary>
			public static AlignVs AlignV = AlignVs.Center;
		}
	#endregion
	
	#region Constructors
		/// <summary>
		/// Generic initializer to default values
		/// </summary>
		private void Init()
		{
			color					= Color.White;
			brush					= null;
			type					= FillTypes.None;
			this.isScaled			= Default.IsScaled;
			this.alignH				= Default.AlignH;
			this.alignV				= Default.AlignV;
			this.rangeMin			= 0.0;
			this.rangeMax			= 1.0;
			gradientBM				= null;

			colorList				= null;
			positionList			= null;
			angle					= 0;
			image					= null;
			wrapMode				= WrapMode.Tile;

			// ----------------------------------
			// 纹理画刷图案,jingzhou xu
			textureBrushImage		= null;

			// 画刷风格,jingzhou xu
			brushStyle				= BrushFillStyles.LinearGradientBrush;

			// 阴影画刷风格,jingzhou xu
			hatchBrushStyle			= HatchStyle.HorizontalBrick;

			// 已有路径渐变画刷风格(默认六边形画刷),jingzhou xu
			pathBrushStyle			= PathGradientBrushStyles.HexagonPathGradientBrush;

			startColor				= Color.RoyalBlue;	// 填充渐变起始色,jingzhou xu
			midColor				= Color.White;		// 填充渐变中间色,jingzhou xu
			endColor				= Color.RoyalBlue;	// 填充渐变终止色,jingzhou xu

			hatchForegroundColor	= Color.White;		// 阴影画刷前景色，jingzhou xu
			hatchBackgroundColor	= Color.Black;		// 阴影画刷背景色，jingzhou xu

			// 构造路径渐变画刷，jingzhou xu
			pathBrushOne			= null;
			pathBrushTwo			= null;
			ConstructPathGradientBrush();
			// ----------------------------------

		}

		/// <summary>
		/// 填充构造，创建路径渐变画刷，jingzhou xu
		/// </summary>
		public Fill(Point[] apt, WrapMode wm)
		{
			Init();

			// 创建指定区域平铺模式的渐变色画刷
			this.brush		= new PathGradientBrush( apt, wm );
			this.type		= FillTypes.Brush;

			this.brushStyle = BrushFillStyles.PathGradientBrush;
		}

		/// <summary>
		/// 填充构造，创建路径渐变画刷，jingzhou xu
		/// </summary>
		public Fill(PointF[] aptf, WrapMode wm)
		{
			Init();

			// 创建指定区域平铺模式的渐变色画刷
			this.brush		= new PathGradientBrush( aptf, wm );
			this.type		= FillTypes.Brush;

			this.brushStyle = BrushFillStyles.PathGradientBrush;
		}

		/// <summary>
		/// 填充构造，创建阴影画刷，jingzhou xu
		/// </summary>
		public Fill(HatchStyle hatchSytle, Color clrForeground, Color clrBackground)
		{
			
			Init();

			// 构造阴影画刷
			ConstructHatchBrush(hatchSytle, clrForeground, clrBackground);
		}

		/// <summary>
		/// The default constructor.  Initialized to no fill.
		/// </summary>
		public Fill()
		{
			Init();
		}
		
		/// <summary>
		/// Constructor that specifies the color, brush, and type for this fill.
		/// </summary>
		/// <param name="color">The color of the fill for solid fills</param>
		/// <param name="brush">A custom brush for fills.  Can be a <see cref="SolidBrush"/>,
		/// <see cref="LinearGradientBrush"/>, or <see cref="TextureBrush"/>.</param>
		/// <param name="type">for this fill.</param>
		public Fill( Color color, Brush brush, FillTypes type )
		{
			Init();
			this.color	= color;
			this.brush	= brush;
			this.type	= type;
		}
		
		/// <summary>
		/// Constructor that creates a solid color-fill, 
		/// </summary>
		/// <param name="color">The color of the solid fill</param>
		public Fill( Color color )
		{
			Init();

			// 构造单色填充画刷，jingzhou xu,2005.8.3
			ConstructSolidBrush(color);
			
		}
		
		/// <summary>
		/// Constructor that creates a linear gradient color-fill
		/// </summary>
		/// <param name="color1">The first color for the gradient fill</param>
		/// <param name="color2">The second color for the gradient fill</param>
		/// <param name="angle">The angle (degrees) of the gradient fill</param>
		public Fill( Color color1, Color color2, float angle )
		{
			Init();
			this.color			= color2;

			ColorBlend blend	= new ColorBlend( 2 );
			blend.Colors[0]		= color1;
			blend.Colors[1]		= color2;
			blend.Positions[0]	= 0.0f;
			blend.Positions[1]	= 1.0f;
			this.type			= FillTypes.Brush;

			this.CreateBrushFromBlend( blend, angle );

			// 赋值，为属性页更新显示，jingzhou xu
			this.brushStyle		= BrushFillStyles.LinearGradientBrush;
		}
		
		/// <summary>
		/// Constructor that creates a linear gradient color-fill
		/// </summary>
		/// <param name="color1">The first color for the gradient fill</param>
		/// <param name="color2">The second color for the gradient fill</param>
		public Fill( Color color1, Color color2 ) : this( color1, color2, 0.0F )
		{
		}
		
		/// <summary>
		/// Constructor that creates a linear gradient color-fill, This gradient fill
		/// consists of three colors.
		/// </summary>
		/// <param name="color1">The first color for the gradient fill</param>
		/// <param name="color2">The second color for the gradient fill</param>
		/// <param name="color3">The third color for the gradient fill</param>
		public Fill( Color color1, Color color2, Color color3 ) :
			this( color1, color2, color3, 0.0f )
		{
			
		}

		/// <summary>
		/// Constructor that creates a linear gradient color-fill, This gradient fill
		/// consists of three colors
		/// </summary>
		/// <param name="color1">The first color for the gradient fill</param>
		/// <param name="color2">The second color for the gradient fill</param>
		/// <param name="color3">The third color for the gradient fill</param>
		/// <param name="angle">The angle (degrees) of the gradient fill</param>
		public Fill( Color color1, Color color2, Color color3, float angle )
		{
			Init();

			// 构造线性渐变画刷，jingzhou xu,2005.8.3
			ConstructLinearGradientBrush(color1, color2, color3, angle);
			
		}
		
		/// <summary>
		/// Constructor that creates a linear gradient multi-color-fill,The gradient
		/// angle is defaulted to zero.
		/// </summary>
		/// <param name="blend">The <see cref="ColorBlend"/> object that defines the colors
		/// and positions along the gradient.</param>
		public Fill( ColorBlend blend ) :
			this( blend, 0.0F )
		{
		}

		/// <summary>
		/// Constructor that creates a linear gradient multi-color-fill, drawn at the
		/// specified angle (degrees).
		/// </summary>
		/// <param name="blend">The  object that defines the colors
		/// and positions along the gradient.</param>
		/// <param name="angle">The angle (degrees) of the gradient fill</param>
		public Fill( ColorBlend blend, float angle )
		{
			Init();
			this.type = FillTypes.Brush;
			this.CreateBrushFromBlend( blend, angle );
		}

		/// <summary>
		/// Constructor that creates a linear gradient multi-color-fill
		/// </summary>
		/// <param name="colors">The array of <see cref="Color"/> objects that defines the colors
		/// along the gradient.</param>
		public Fill( Color[] colors ) :
			this( colors, 0.0F )
		{
		}

		/// <summary>
		/// Constructor that creates a linear gradient multi-color-fill
		/// </summary>
		/// <param name="colors">The array of <see cref="Color"/> objects that defines the colors
		/// along the gradient.</param>
		/// <param name="angle">The angle (degrees) of the gradient fill</param>
		public Fill( Color[] colors, float angle )
		{
			Init();
			color				= colors[ colors.Length - 1 ];

			ColorBlend blend	= new ColorBlend();
			blend.Colors		= colors;
			blend.Positions		= new float[colors.Length];
			blend.Positions[0]	= 0.0F;
			for ( int i=1; i<colors.Length; i++ )
				blend.Positions[i] = (float) i / (float)( colors.Length - 1 );
			this.type			= FillTypes.Brush;

			this.CreateBrushFromBlend( blend, angle );
		}

		/// <summary>
		/// Constructor that creates a linear gradient multi-color-fill
		/// </summary>
		/// <param name="colors">The array of <see cref="Color"/> objects that defines the colors
		/// along the gradient.</param>
		/// <param name="positions">The array of floating point values that defines the color
		/// positions along the gradient.  Values should range from 0 to 1.</param>
		public Fill( Color[] colors, float[] positions ) :
			this( colors, positions, 0.0F )
		{
		}

		/// <summary>
		/// Constructor that creates a linear gradient multi-color-fill
		/// </summary>
		/// <param name="colors">The array of <see cref="Color"/> objects that defines the colors
		/// along the gradient.</param>
		/// <param name="positions">The array of floating point values that defines the color
		/// positions along the gradient.  Values should range from 0 to 1.</param>
		/// <param name="angle">The angle (degrees) of the gradient fill</param>
		public Fill( Color[] colors, float[] positions, float angle )
		{
			Init();
			color				= colors[ colors.Length - 1 ];

			ColorBlend blend	= new ColorBlend();
			blend.Colors		= colors;
			blend.Positions		= positions;
			this.type			= FillTypes.Brush;

			this.CreateBrushFromBlend( blend, angle );
		}

		/// <summary>
		/// Constructor that creates a texture fill, using the specified image.
		/// </summary>
		/// <param name="image">The <see cref="Image"/> to use for filling</param>
		/// <param name="wrapMode">The <see cref="WrapMode"/> class that controls the image wrapping properties</param>
		public Fill( Image image, WrapMode wrapMode )
		{
			Init();

			// 构造纹理填充画刷，jingzhou xu,2005.8.5
			ConstructTextureBrush( image, wrapMode );
				
		}
		
		/// <summary>
		/// Constructor that creates a <see cref="Brush"/> fill, using a user-supplied, custom
		/// <see cref="Brush"/>.  The brush will be scaled to fit the destination screen object
		/// unless you manually change <see cref="IsScaled"/> to false;
		/// </summary>
		/// <param name="brush">The <see cref="Brush"/> to use for fancy fills.  Typically, this would
		/// be a <see cref="LinearGradientBrush"/> or a <see cref="TextureBrush"/> class</param>
		public Fill( Brush brush ) : this( brush, Default.IsScaled )
		{
		}
		
		/// <summary>
		/// Constructor that creates a <see cref="Brush"/> fill, using a user-supplied, custom
		/// <see cref="Brush"/>.  The brush will be scaled to fit the destination screen object
		/// according to the <see paramref="isScaled"/> parameter.
		/// </summary>
		/// <param name="brush">The <see cref="Brush"/> to use for fancy fills.  Typically, this would
		/// be a <see cref="LinearGradientBrush"/> or a <see cref="TextureBrush"/> class</param>
		/// <param name="isScaled">Determines if the brush will be scaled to fit the bounding box
		/// of the destination object.  true to scale it, false to leave it unscaled</param>
		public Fill( Brush brush, bool isScaled )
		{
			Init();
			this.isScaled	= isScaled;
			this.color		= Color.White;
			this.brush		= (Brush) brush.Clone();
			this.type		= FillTypes.Brush;
		}
		
		/// <summary>
		/// Constructor that creates a <see cref="Brush"/> fill, using a user-supplied, custom
		/// <see cref="Brush"/>.  This constructor will make the brush unscaled (see <see cref="IsScaled"/>),
		/// but it provides <see paramref="alignH"/> and <see paramref="alignV"/> parameters to control
		/// alignment of the brush with respect to the filled object.
		/// </summary>
		/// <param name="brush">The <see cref="Brush"/> to use for fancy fills.  Typically, this would
		/// be a <see cref="LinearGradientBrush"/> or a <see cref="TextureBrush"/> class</param>
		/// <param name="alignH">Controls the horizontal alignment of the brush within the filled object
		/// (see <see cref="AlignH"/></param>
		/// <param name="alignV">Controls the vertical alignment of the brush within the filled object
		/// (see <see cref="AlignV"/></param>
		public Fill( Brush brush, AlignHs alignH, AlignVs alignV )
		{
			Init();
			this.alignH		= alignH;
			this.alignV		= alignV;
			this.isScaled	= false;
			this.color		= Color.White;
			this.brush		= (Brush) brush.Clone();
			this.type		= FillTypes.Brush;
		}
		
		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The Fill object from which to copy</param>
		public Fill( Fill rhs )
		{
			color		= rhs.color;
			if ( rhs.brush != null )
				brush	= (Brush) rhs.brush.Clone();
			else
				brush	= null;
			type		= rhs.type;
			alignH		= rhs.AlignH;
			alignV		= rhs.AlignV;
            isScaled	= rhs.IsScaled;
			rangeMin	= rhs.RangeMin;
			rangeMax	= rhs.RangeMax;
			gradientBM	= null;

			if ( rhs.colorList != null )
				colorList = (Color[]) rhs.colorList.Clone();
			else
				colorList = null;

			if ( rhs.positionList != null )
			{
				positionList = (float[]) rhs.positionList.Clone();
			}
			else
				positionList = null;

			if ( rhs.image != null )
				image = (Image) rhs.image.Clone();
			else
				image = null;

			angle		= rhs.angle;
			wrapMode	= rhs.wrapMode;
		}
		
		/// <summary>
		/// Deep-copy clone routine
		/// </summary>
		/// <returns>A new, independent copy of the Fill class</returns>
		public object Clone()
		{ 
			return new Fill( this ); 
		}

		private void CreateBrushFromBlend( ColorBlend blend, float angle )
		{
			this.angle		= angle;

			colorList		= (Color[]) blend.Colors.Clone();
			positionList	= (float[]) blend.Positions.Clone();

			this.brush		= new LinearGradientBrush( new Rectangle( 0, 0, 100, 100 ),
				Color.Red, Color.White, angle );
			((LinearGradientBrush)this.brush).InterpolationColors = blend;
		}
	#endregion

	#region Serialization
		/// <summary>
		/// Current schema value that defines the version of the serialized file
		/// </summary>
		public const int schema = 1;

		/// <summary>
		/// Constructor for deserializing objects
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
		/// </param>
		/// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
		/// </param>
		protected Fill( SerializationInfo info, StreamingContext context )
		{
			Init();

			// The schema value is just a file version parameter.  You can use it to make future versions
			// backwards compatible as new member variables are added to classes
			int sch		= info.GetInt32( "schema" );

			color		= (Color) info.GetValue( "color", typeof(Color) );
			//brush = (Brush) info.GetValue( "brush", typeof(Brush) );
			//brushHolder = (BrushHolder) info.GetValue( "brushHolder", typeof(BrushHolder) );
			type		= (FillTypes) info.GetValue( "type", typeof(FillTypes) );
			isScaled	= info.GetBoolean( "isScaled" );
			alignH		= (AlignHs) info.GetValue( "alignH", typeof(AlignHs) );
			alignV		= (AlignVs) info.GetValue( "alignV", typeof(AlignVs) );
			rangeMin	= info.GetDouble( "rangeMin" );
			rangeMax	= info.GetDouble( "rangeMax" );

			//BrushHolder brushHolder = (BrushHolder) info.GetValue( "brushHolder", typeof( BrushHolder ) );
			//brush = brush;

			colorList	= (Color[]) info.GetValue( "colorList", typeof(Color[]) );
			positionList= (float[]) info.GetValue( "positionList", typeof(float[]) );
			angle		= info.GetSingle( "angle" );
			image		= (Image) info.GetValue( "image", typeof(Image) );
			wrapMode	= (WrapMode) info.GetValue( "wrapMode", typeof(WrapMode) );

			if ( colorList != null && positionList != null )
			{
				ColorBlend blend = new ColorBlend();
				blend.Colors = colorList;
				blend.Positions = positionList;
				CreateBrushFromBlend( blend, angle );
			}
			else if ( image != null )
			{
				this.brush = new TextureBrush( image, wrapMode );
			}
		}
		/// <summary>
		/// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
		/// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public virtual void GetObjectData( SerializationInfo info, StreamingContext context )
		{

			info.AddValue( "schema", schema );
			info.AddValue( "color", color );
			//info.AddValue( "brush", brush );
			//info.AddValue( "brushHolder", brushHolder );
			info.AddValue( "type", type );
			info.AddValue( "isScaled", isScaled );
			info.AddValue( "alignH", alignH );
			info.AddValue( "alignV", alignV );
			info.AddValue( "rangeMin", rangeMin );
			info.AddValue( "rangeMax", rangeMax );

			//BrushHolder brushHolder = new BrushHolder();
			//brush = brush;
			//info.AddValue( "brushHolder", brushHolder );

			info.AddValue( "colorList", colorList );
			info.AddValue( "positionList", positionList );
			info.AddValue( "angle", angle );
			info.AddValue( "image", image );
			info.AddValue( "wrapMode", wrapMode );

		}
	#endregion

	#region Properties

		/// <summary>
		/// 纹理画刷填充图案, jingzhou xu
		/// </summary>
		public Image TextureBrushImage
		{
			get
			{
				return textureBrushImage;
			}
			set
			{
				textureBrushImage = value;
			}
		}

		/// <summary>
		/// 填充画刷样式, jingzhou xu
		/// </summary>
		public BrushFillStyles BrushStyle
		{
			get
			{
				return brushStyle;
			}
			set
			{
				brushStyle = value;
			}
		}

		/// <summary>
		/// 阴影画刷风格(共53种), jingzhou xu
		/// </summary>
		public HatchStyle HatchBrushStyle
		{
			get
			{
				return hatchBrushStyle;
			}
			set
			{
				hatchBrushStyle = value;
			}
		}

		/// <summary>
		/// 已有的路径渐变画刷风格, jingzhou xu
		/// </summary>
		public PathGradientBrushStyles PathBrushStyle
		{
			get
			{
				return pathBrushStyle;
			}
			set
			{
				pathBrushStyle = value;
			}
		}

		/// <summary>
		/// 填充渐变起始颜色, jingzhou xu
		/// </summary>
		public Color StartColor
		{
			get
			{
				return startColor;
			}
			set
			{
				startColor = value;
			}
		}

		/// <summary>
		/// 填充渐变中间颜色, jingzhou xu
		/// </summary>
		public Color MidColor
		{
			get
			{
				return midColor;
			}
			set
			{
				midColor = value;
			}
		}

		/// <summary>
		/// 填充渐变终止颜色, jingzhou xu
		/// </summary>
		public Color EndColor
		{
			get
			{
				return endColor;
			}
			set
			{
				endColor = value;
			}
		}

		/// <summary>
		/// 阴影画刷前景色, jingzhou xu
		/// </summary>
		public Color HatchForegroundColor
		{
			get
			{
				return hatchForegroundColor;
			}
			set
			{
				hatchForegroundColor = value;
			}
		}

		/// <summary>
		/// 阴影画刷背景色, jingzhou xu
		/// </summary>
		public Color HatchBackgroundColor
		{
			get
			{
				return hatchBackgroundColor;
			}
			set
			{
				hatchBackgroundColor = value;
			}
		}
		
		/// <summary>
		/// 路径渐变画刷1, jingzhou xu
		/// </summary>
		public PathGradientBrush PathBrushOne
		{
			get
			{
				return pathBrushOne;
			}
			set
			{
				pathBrushOne = value;
			}
		}

		/// <summary>
		/// 路径渐变画刷2, jingzhou xu
		/// </summary>
		public PathGradientBrush PathBrushTwo
		{
			get
			{
				return pathBrushTwo;
			}
			set
			{
				pathBrushTwo = value;
			}
		}

		/// <summary>
		/// The fill color.  This property is used as a single color to make a solid fill
		/// or it can be used in combination with <see cref="System.Drawing.Color.White"/> to make a
		/// <see cref="LinearGradientBrush"/>
		/// </summary>
		/// <seealso cref="Type"/>
		public Color Color
		{
			get { return color; }
			set { color = value; }
		}

		/// <summary>
		/// The custom fill brush. 
		/// </summary>
		public Brush Brush
		{
			get { return brush; }
			set { brush = value; }
		}

		/// <summary>
		/// Determines the type of fill, which can be either solid
		/// color
		/// </summary>
		public FillTypes Type
		{
			get { return type; }
			set { type = value; }
		}

		/// <summary>
		/// This property determines the type of color fill. 
		/// </summary>
		public bool IsVisible
		{
			get { return type != FillTypes.None; }
			set { type = value ? FillTypes.Brush : FillTypes.None; }
		}

		/// <summary>
		/// Determines if the brush will be scaled to the bounding box
		/// of the filled object.  If this value is false, then the brush will only be aligned
		/// with the filled object based on the <see cref="AlignH"/> and <see cref="AlignV"/>
		/// properties.
		/// </summary>
		public bool IsScaled
		{
			get { return isScaled; }
			set { isScaled = value; }
		}
		
		/// <summary>
		/// Determines how the brush will be aligned with the filled object
		/// in the horizontal direction. 
		/// </summary>
		/// <seealso cref="AlignV"/>
		public AlignHs AlignH
		{
			get { return alignH; }
			set { alignH = value; }
		}
		
		/// <summary>
		/// Determines how the brush will be aligned with the filled object
		/// in the vertical direction. This field only applies if <see cref="IsScaled"/> is false.
		/// </summary>
		/// <seealso cref="AlignH"/>
		public AlignVs AlignV
		{
			get { return alignV; }
			set { alignV = value; }
		}

		/// <summary>
		/// Returns a boolean value indicating whether or not this fill is a "Gradient-By-Value"
		/// type. 
		/// </summary>
		/// <value>true if this is a Gradient-by-value type, false otherwise</value>
		public bool IsGradientValueType
		{
			get { return type == FillTypes.GradientByX || type == FillTypes.GradientByY ||
					type == FillTypes.GradientByZ; }
		}

		/// <summary>
		/// The minimum user-scale value for the gradient-by-value determination.  This defines
		/// the user-scale value for the start of the gradient.
		/// </summary>
		/// <value>A double value, in user scale unit</value>
		public double RangeMin
		{
			get { return rangeMin; }
			set { rangeMin = value; }
		}
		/// <summary>
		/// The maximum user-scale value for the gradient-by-value determination.  This defines
		/// the user-scale value for the end of the gradient.
		/// </summary>
		/// <value>A double value, in user scale unit</value>
		public double RangeMax
		{
			get { return rangeMax; }
			set { rangeMax = value; }
		}

	#endregion

	#region Methods

		/// <summary>
		/// 根据FillTypes类型创建一种相应画刷
		/// </summary>
		/// <param name="rect">填充画刷的范围</param>
		/// <returns>新创建的画刷</returns>
		public Brush MakeBrush( RectangleF rect )
		{
			// 获取一种新画刷
			if ( this.IsVisible && ( !this.color.IsEmpty || this.brush != null ) )
			{
					// 创建成比例的各种画刷
				if ( this.type == FillTypes.Brush )
				{
					if ( rect.Height < 1.0F )
						rect.Height = 1.0F;
					if ( rect.Width < 1.0F )
						rect.Width = 1.0F;
					
					return ScaleBrush( rect, this.brush, this.isScaled );
				}
					// 创建一种随机渐变色的单色画刷
				else if ( IsGradientValueType )
				{
					return new SolidBrush( GetGradientColor() );
				}
					// 创建单色的实体画刷
				else
					return new SolidBrush( this.color );
			}

			// 默认返回一白色单色画刷
			return new SolidBrush( Color.White );
		}

		/// <summary>
		/// 根据三维坐标随机选取一种颜色
		/// </summary>
		/// <returns>随机选取的一种颜色</returns>
		private Color GetGradientColor( )
		{
			double val, valueFraction;

			if ( this.type == FillTypes.GradientByZ )
				val = 0.5;									// 默认Z轴方向值
			else if ( this.type == FillTypes.GradientByY )
				val = 0.5;									// 默认Y轴方向值
			else
				val = 0.5;									// 默认X轴方向值

			if ( Double.IsInfinity( val ) || double.IsNaN( val ) || val == Double.MaxValue ||
					this.rangeMax - this.rangeMin < 1e-20 )
				valueFraction = 0.5;
			else
				valueFraction = ( val - this.rangeMin ) / ( rangeMax - rangeMin );

			if ( valueFraction < 0.0 )
				valueFraction = 0.0;
			else if ( valueFraction > 1.0 )
				valueFraction = 1.0;

			if ( gradientBM == null )
			{
				RectangleF rect = new RectangleF( 0, 0, 100, 1 );
				gradientBM		= new Bitmap( 100, 1 );
				Graphics gBM	= Graphics.FromImage( gradientBM );

				Brush tmpBrush	= ScaleBrush( rect, this.brush, true );
				gBM.FillRectangle( tmpBrush, rect );
			}

			return gradientBM.GetPixel( (int) (99.9 * valueFraction), 0 );
		}

		/// <summary>
		/// 创建五种画刷中的一种，其中线性渐变和纹理画刷可按比例矩形大小选择是否缩放创建。
		/// </summary>
		/// <param name="rect">矩形范围</param>
		/// <param name="brush">指定画刷</param>
		/// <param name="isScaled">是否按指定矩形缩放</param>
		/// <returns>返回五种画刷中的一种</returns>
		private Brush ScaleBrush( RectangleF rect, Brush brush, bool isScaled )
		{
			if ( brush != null )
			{
				if( brush is PathGradientBrush)				// 路径渐变画刷,jingzhou xu
				{
					return (Brush) brush.Clone();
				}
				else if( brush is HatchBrush)				// 阴影画刷,jingzhou xu
				{
					return (Brush) brush.Clone();
				}
				else if ( brush is SolidBrush )				// 单色画刷
				{
					return (Brush) brush.Clone();
				}
				else if ( brush is LinearGradientBrush )	// 线性渐变画刷，可缩放
				{
					LinearGradientBrush linBrush = (LinearGradientBrush) brush.Clone();
					
					if ( isScaled )
					{
						linBrush.ScaleTransform( rect.Width / linBrush.Rectangle.Width,
							rect.Height / linBrush.Rectangle.Height, MatrixOrder.Append );
						linBrush.TranslateTransform( rect.Left - linBrush.Rectangle.Left,
							rect.Top - linBrush.Rectangle.Top, MatrixOrder.Append );
					}
					else
					{
						float	dx = 0,
								dy = 0;
						switch ( this.alignH )
						{
						case AlignHs.Left:
							dx = rect.Left - linBrush.Rectangle.Left;
							break;
						case AlignHs.Center:
							dx = ( rect.Left + rect.Width / 2.0F ) - linBrush.Rectangle.Left;
							break;
						case AlignHs.Right:
							dx = ( rect.Left + rect.Width ) - linBrush.Rectangle.Left;
							break;
						}
						
						switch ( this.alignV )
						{
						case AlignVs.Top:
							dy = rect.Top - linBrush.Rectangle.Top;
							break;
						case AlignVs.Center:
							dy = ( rect.Top + rect.Height / 2.0F ) - linBrush.Rectangle.Top;
							break;
						case AlignVs.Bottom:
							dy = ( rect.Top + rect.Height) - linBrush.Rectangle.Top;
							break;
						}

						linBrush.TranslateTransform( dx, dy, MatrixOrder.Append );
					}
					
					return linBrush;
					
				} 
				else if ( brush is TextureBrush )			// 纹理画刷，可缩放
				{
					TextureBrush texBrush = (TextureBrush) brush.Clone();
					
					if ( isScaled )
					{
						texBrush.ScaleTransform( rect.Width / texBrush.Image.Width,
							rect.Height / texBrush.Image.Height, MatrixOrder.Append );
						texBrush.TranslateTransform( rect.Left, rect.Top, MatrixOrder.Append );
					}
					else
					{
						float	dx = 0,
								dy = 0;
						switch ( this.alignH )
						{
						case AlignHs.Left:
							dx = rect.Left;
							break;
						case AlignHs.Center:
							dx = ( rect.Left + rect.Width / 2.0F );
							break;
						case AlignHs.Right:
							dx = ( rect.Left + rect.Width );
							break;
						}
						
						switch ( this.alignV )
						{
						case AlignVs.Top:
							dy = rect.Top;
							break;
						case AlignVs.Center:
							dy = ( rect.Top + rect.Height / 2.0F );
							break;
						case AlignVs.Bottom:
							dy = ( rect.Top + rect.Height);
							break;
						}

						texBrush.TranslateTransform( dx, dy, MatrixOrder.Append );
					}
					
					return texBrush;
				}
				else // 其它画刷类型
				{
					return (Brush) brush.Clone();
				}
			}
			else
				// 如果没有提供任何画刷，创建一个渐变为白色的画刷
				return new LinearGradientBrush( rect, Color.White, this.color, 0F );
		}

		/// <summary>
		/// Fill the background of the <see cref="RectangleF"/> area, using the
		/// fill type from this <see cref="Fill"/>.
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="rect">The <see cref="RectangleF"/> struct specifying the area
		/// to be filled</param>
		public void Draw( Graphics g, RectangleF rect )
		{
			if ( this.IsVisible )
			{
				Brush brush = this.MakeBrush( rect );
				g.FillRectangle( brush, rect );
				brush.Dispose();
			}
		}
	#endregion
	
	#region 辅助涵数

		// --------------------------------------------------------------------------------------------------
	
		/// <summary>
		/// 构造纹理填充画刷，jingzhou xu,2005.8.5
		/// </summary>
		public void ConstructTextureBrush(Image image, WrapMode wrapMode)
		{
			// 构造纹理填充画刷，jingzhou xu,2005.8.5
			this.color				= Color.White;
			
			if( image == null)
			{
				// 图案为空不创建纹理画刷，jingzhou xu
				brush				= null;
			}
			else
			{
				// 图案不为空创建纹理画刷，jingzhou xu
				brush				= new TextureBrush( image, wrapMode );
//				((TextureBrush)brush).RotateTransform( 45.0f );
			}
			this.type				= FillTypes.Brush;
			this.image				= image;
			this.wrapMode			= wrapMode;
			this.isScaled			= false;		// 不进行自动缩放处理，jingzhou xu

			// 赋值，为属性页更新显示，jingzhou xu
			this.brushStyle			= BrushFillStyles.TextureBrush;
			this.TextureBrushImage	= this.image;
		}

		/// <summary>
		/// 构造线性渐变填充画刷，jingzhou xu,2005.8.3
		/// </summary>
		public void ConstructLinearGradientBrush(Color color1, Color color2, Color color3, float angle)
		{
			this.color			= color3;

			ColorBlend blend	= new ColorBlend( 3 );
			blend.Colors[0]		= color1;
			blend.Colors[1]		= color2;
			blend.Colors[2]		= color3;
			blend.Positions[0]	= 0.0f;
			blend.Positions[1]	= 0.5f;
			blend.Positions[2]	= 1.0f;
			this.type			= FillTypes.Brush;
			this.isScaled		= true;				// 进行自动缩放处理，jingzhou xu
			
			this.CreateBrushFromBlend( blend, angle );

			// 赋值，为属性页更新显示，jingzhou xu
			this.startColor		= color1;
			this.midColor		= color2;
			this.endColor		= color3;
			this.brushStyle		= BrushFillStyles.LinearGradientBrush;
		}

		/// <summary>
		/// 构造单色填充画刷，jingzhou xu,2005.8.3
		/// </summary>
		public void ConstructSolidBrush(Color color)
		{
			this.color		= color;
			this.type		= FillTypes.Solid;

			// 赋值，为属性页更新显示，jingzhou xu
			this.brushStyle = BrushFillStyles.SolidBursh;
			this.startColor = color;
		}

		/// <summary>
		/// 构造阴影填充画刷，jingzhou xu,2005.8.3
		/// </summary>
		public void ConstructHatchBrush(HatchStyle hatchSytle, Color clrForeground, Color clrBackground)
		{

			// 创建指定绘制风格的背景色，前景色的阴影画刷
			this.brush					= new HatchBrush( hatchSytle, clrForeground, clrBackground );
			this.type					= FillTypes.Brush;

			this.brushStyle				= BrushFillStyles.HatchBrush;
			this.hatchBrushStyle		= hatchSytle;
			this.hatchBackgroundColor	= clrBackground;
			this.hatchForegroundColor	= clrForeground;

		}

		/// <summary>
		/// 构造已有路径渐变填充画刷，jingzhou xu,2005.8.2
		/// </summary>
		public void ConstructPathGradientBrush()
		{
			switch(PathBrushStyle)
			{
					// 已有六边形填充
				case PathGradientBrushStyles.HexagonPathGradientBrush:
				{
					GetHexagonPathGradientBrush(out this.pathBrushOne, out this.pathBrushTwo);

					break;
				}

					// 已有双三角形填充
				case PathGradientBrushStyles.TwoTrianglePathGradientBrush:
				{
					GetTwoTrianglePathGradientBrush(out this.pathBrushOne, out this.pathBrushTwo);

					break;
				}

					// 已有菱形填充
				case PathGradientBrushStyles.DiamondPathGradientBrush:
				{
					GetDiamondPathGradientBrush(out this.pathBrushOne);

					break;
				}

					// 已有四边形填充
				case PathGradientBrushStyles.SquarePathGradientBrush:
				{
					GetSquarePathGradientBrush(out this.pathBrushOne);

					break;
				}

					// 已有五角星填充
				case PathGradientBrushStyles.StarPathGradientBrush:
				{
					GetStarPathGradientBrush(out this.pathBrushOne);

					break;
				}

					// 已有球形填充
				case PathGradientBrushStyles.BallPathGradientBrush:
				{
					GetBallPathGradientBrush(out this.pathBrushOne);

					break;
				}

					// 其它情况下：用六边形填充
				default:
				{
					GetHexagonPathGradientBrush(out this.pathBrushOne, out this.pathBrushTwo);

					break;
				}

			}

		}

		/// <summary>
		///  获取六边形平铺路径渐变两画刷，jingzhou xu,2005.8.1
		/// </summary>
		/// <param name="brush1">指定画刷一</param>
		/// <param name="brush2">指定画刷二</param>
		public void GetHexagonPathGradientBrush(out PathGradientBrush brush1, out PathGradientBrush brush2)
		{
			const float fSide	= 30;
			float fHalf			= fSide * (float)Math.Sin(Math.PI/3);
 
			// 创建第一奇数列路径渐变画刷
			PointF[] aptf		= {	
									  new PointF(fSide,			0),
									  new PointF(fSide*1.5f,	0),
									  new PointF(fSide,			0),
									  new PointF(fSide/2,		-fHalf),
									  new PointF(-fSide/2,		-fHalf),
									  new PointF(-fSide,		0),
									  new PointF(-fSide*1.5f,	0),
									  new PointF(-fSide,		0),
									  new PointF(-fSide/2,		fHalf),
									  new PointF(fSide/2,		fHalf) 
								  };
			brush1				= new PathGradientBrush(aptf, WrapMode.Tile);

			// 创建第二偶数列路径渐变画刷
			for(int i = 0; i < aptf.Length; i++)
			{
				aptf[i].X		+= fSide * 1.5f;
				aptf[i].Y		+= fHalf;
			}
			brush2				= new PathGradientBrush(aptf, WrapMode.Tile);

		}

		/// <summary>
		/// 获取双三角平铺路径渐变两画刷，jingzhou xu,2005.8.1
		/// </summary>
		/// <param name="brush1">指定画刷一</param>
		/// <param name="brush2">指定画刷二</param>
		public void GetTwoTrianglePathGradientBrush(out PathGradientBrush brush1, out PathGradientBrush brush2)
		{
			const float fSide	= 30;
 
			// 创建第一上三角路径渐变画刷
			PointF[] aptf		=	{	
										new PointF(0,			0),
										new PointF(fSide,		0),
										new PointF(0,			fSide),
									};
			brush1				= new PathGradientBrush(aptf, WrapMode.TileFlipXY);

			// 创建第二下三角路径渐变画刷
			aptf				= new PointF[] {	
												   new PointF(fSide,		0),
												   new PointF(fSide,		fSide),
												   new PointF(0,			fSide),
												};
			brush2				= new PathGradientBrush(aptf, WrapMode.TileFlipXY);

		}

		/// <summary>
		/// 获取菱形平铺路径渐变画刷，jingzhou xu,2005.8.1
		/// </summary>
		/// <param name="brush">指定画刷</param>
		public void GetDiamondPathGradientBrush(out PathGradientBrush brush)
		{
			const float fSide	= 30;
 
			// 创建菱形路径渐变画刷
			PointF[] aptf		=	{	
										new PointF(0,			0),
										new PointF(fSide,		0),
										new PointF(0,			fSide),
									};
			brush				= new PathGradientBrush(aptf, WrapMode.TileFlipXY);
		}

		/// <summary>
		///  获取四边形平铺路径渐变画刷，jingzhou xu,2005.8.1
		/// </summary>
		/// <param name="brush">指定画刷</param>
		public void GetSquarePathGradientBrush(out PathGradientBrush brush)
		{
			const float fSide	= 30;
 
			// 创建四边形路径渐变画刷
			PointF[] aptf		=	{	
										new PointF(0,			0),
										new PointF(fSide,		0),
										new PointF(fSide,		fSide),
										new PointF(0,			fSide),
									};
			brush				= new PathGradientBrush(aptf, WrapMode.Tile);

		}

		/// <summary>
		///  获取五角星平铺路径渐变画刷，jingzhou xu,2005.8.1
		/// </summary>
		/// <param name="brush">指定画刷</param>
		public void GetStarPathGradientBrush(out PathGradientBrush brush)
		{
			const float fSide		= 30;

			Point[] apt				= new Point[5];

			for(int i = 0; i < apt.Length; i++)
			{
				double dAngle		= (i*0.8 - 0.5) * Math.PI;
				apt[i]				= new Point( 
												(int)(fSide * (0.50 + 0.48 * Math.Cos(dAngle))),
												(int)(fSide * (0.50 + 0.48 * Math.Sin(dAngle)))
												);
			}

			brush					= new PathGradientBrush(apt, WrapMode.Tile);
			brush.CenterColor		= Color.White;
			brush.SurroundColors	= new Color[1]{ Color.Black };

		}

		/// <summary>
		///  获取球形平铺路径渐变画刷，jingzhou xu,2005.8.8
		/// </summary>
		/// <param name="brush">指定画刷</param>
		public void GetBallPathGradientBrush(out PathGradientBrush brush)
		{
			const float fSide		= 30;

			GraphicsPath path		= new GraphicsPath();
			path.AddEllipse(0, 0, fSide, fSide);

			brush					= new PathGradientBrush(path);
			brush.WrapMode			= WrapMode.Tile;
			brush.CenterPoint		= new PointF(fSide/3, fSide/3); 
			brush.CenterColor		= Color.White;
			brush.SurroundColors	= new Color[]{ Color.Black };

		}

		// --------------------------------------------------------------------------------------------------
	#endregion
	}
}
