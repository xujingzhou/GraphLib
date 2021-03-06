using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Jurassic.Graph.Base;

namespace Jurassic.Graph.Drawer.Symbol
{
	/// <summary>
	/// 符号绘制、填充类，从ISymbolDrawer接口派生。
	/// </summary>
	/// <example>
	/// <c>CSymbolDrawer使用示例</c>
	/// <code>
	/// // 坐标单位
	///	e.Graphics.PageUnit	= GraphicsUnit.Millimeter;
	///	e.Graphics.PageScale = 0.1f;
	///
	///	ISymbolDrawer	m_Drawer = new CSymbolDrawer();
	///	CSymbolLib		m_Lib = new CSymbolLib();
	///	
	///	// 打开符号库文件
	///	m_Lib.OpenLibFile("Symbol.xml");
	///	
	///	CSymbol sym	= m_Lib.GetSymbol( 0 );
	///	// 填充符号(椭圆方式，单色)
	///	m_Drawer.FillSymbol( e.Graphics, sym, 100, 100, 45.0f, Color.Green, new PointF(150, 1000), 300, 300 );
	///
	///	// 填充符号(多边形方式)
	///	const float fSide	= 500;
	///	PointF[] apt		= new PointF[5];
	///	for(int i = 0; i〈 apt.Length; i++)
	///	{
	///		double dAngle	= (i*0.8 - 0.5) * Math.PI;
	///		apt[i]			= new PointF( 
	///		(int)( fSide * ( 0.5 + 0.48 * Math.Cos(dAngle) ) + 400 ),
	///		(int)( fSide * ( 0.5 + 0.48 * Math.Sin(dAngle) ) )
	///		);
	///	}
	///	m_Drawer.FillSymbol( e.Graphics, sym, 50, 50, 0f, Color.Empty, apt );
	///
	///	// 绘制符号
	///	sym = m_Lib.GetSymbol( 1, 0 );														// 默认符号(用 m_Lib.GetSymbol( 1 );可替换)
	///	m_Drawer.DrawSymbol( e.Graphics, sym, 200, 300, 250, 0, 45.0f, Color.Violet );		// 单色(高为0，按指定宽绘制)
	///	sym = m_Lib.GetSymbol( 1, 2 );														// 指定索引符号1中的第2帧
	///	m_Drawer.DrawSymbol( e.Graphics, sym, 600, 1000, 500, 250, 45.0f );		
	///
	///	sym = m_Lib.GetSymbol( 2, 0 );
	///	m_Drawer.DrawSymbol( e.Graphics, sym, 1000, 300, 0, 250, 45.0f );					// 宽为0，按指定高绘制
	///	sym = m_Lib.GetSymbol( 2, 2 );														// 指定索引符号2中的第2帧
	///	m_Drawer.DrawSymbol( e.Graphics, sym, 1000, 1000, 500, 250, 45.0f, Color.Violet );	// 单色
	///	
	///	sym = m_Lib.GetSymbol( 3 );
	///	m_Drawer.DrawSymbol( e.Graphics, sym, 1400, 300, 300, 300, 45.0f );
	///	m_Drawer.DrawSymbol( e.Graphics, sym, 1400, 1000, 500, 250, 45.0f );
	///
	///	sym = m_Lib.GetSymbol( 4 );
	///	m_Drawer.DrawSymbol( e.Graphics, sym, 1900, 300, 0, 300, 45.0f );					// 宽为0，按指定高绘制
	///	m_Drawer.DrawSymbol( e.Graphics, sym, 1900, 1000, 500, 250, 45.0f );
	/// </code>
	/// </example>
	//	编写：徐景周，2006.1.10
	public class CSymbolDrawer : Jurassic.Graph.Drawer.Symbol.ISymbolDrawer
	{
		#region 成员变量

		private static Matrix _tmpMatrix	= new Matrix();		// 在DrawSymbol临时使用，目的是为了避免每次绘制时都new
		private static PointF _tmpPnt		= new PointF(0,0);	// 在DrawSymbol临时使用，目的是为了避免每次绘制时都new

		#endregion

		#region 构造涵数

		/// <summary>
		/// CSymbolDrawer构造涵数，无参
		/// </summary>
		public CSymbolDrawer()
		{
			// 
			// TODO: 在此处添加构造函数逻辑
			//
		}

		#endregion

		#region 绘制符号

		/// <summary>
		/// 实现 ISymbolDrawer接口的成员方法。在指定的位置，以指定的角度，宽高绘制符号。
		/// 本方法的目的是设计成一个通用的GDI API	绘图函数。坐标类型和GDI+相同.
		/// </summary>
		/// <param name="g">符号绘制画布</param>
		/// <param name="sym">要绘制的符号</param>
		/// <param name="x">要绘制符号的水平坐标</param>
		/// <param name="y">要绘制符号的垂直坐标</param>
		/// <param name="fWidth">符号宽度,如果为0，则以指定的高度(fHeight) 和不变形方式（以符号原始的宽高比例）绘制</param>
		/// <param name="fHeight">符号高度,如果为0，则以指定的宽度(fWidth) 和不变形方式（以符号原始的宽高比例）绘制</param>
		/// <param name="fAngle">整体符号的角度</param>
		public void DrawSymbol( Graphics g, CSymbol sym, float x, float y, float fWidth, float fHeight, float fAngle )
		{
			GraphicsState gstate	= g.Save();

			g.TranslateTransform( x, y );
	
			//判断绘制时是否需要反转x,y轴 lijie
			PointF[]  ptsTest = {new PointF(0,0),new PointF(1,1)};
			g.TransformPoints(CoordinateSpace.Device,CoordinateSpace.World,ptsTest);
			bool bXReVert = ptsTest[1].X-ptsTest[0].X<0?true:false;
			bool bYReVert = ptsTest[1].Y-ptsTest[0].Y<0?true:false;
			if(bYReVert)
			{
				g.ScaleTransform(1,-1);
				fAngle = -fAngle;
			}
			// 符号缩放比例
			if( fWidth == 0 )				// 绘制宽为0,指定的绘制高度缩放( (float)fHeight*(sym.Width/sym.Height), (float)fHeight );
			{
				sym.ScaleX			= fHeight/sym.Height;
				sym.ScaleY			= sym.ScaleX;
					
			}
			else if( fHeight == 0 )			// 绘制高为0,指定的绘制宽度缩放( ( float)fWidth, (float)fWidth/(sym.Width/sym.Height) );
			{
				sym.ScaleX			= fWidth/sym.Width;
				sym.ScaleY			= sym.ScaleX;
			}
			else							// 指定绘制宽/高缩放
			{
				sym.ScaleX			= fWidth/sym.Width;
				sym.ScaleY			= fHeight/sym.Height;
			}
			// 符号旋转角度
			sym.Angle				= fAngle;
			
			DrawSymbol( g, sym, fWidth, fHeight, Color.Empty );

			g.Restore( gstate );
		}

		/// <summary>
		/// 实现 ISymbolDrawer接口的成员方法。在指定的位置，以指定的角度，宽高和单一颜色绘制符号。
		/// 本方法的目的是设计成一个通用的GDI API	绘图函数。坐标类型和GDI+相同.
		/// </summary>
		/// <param name="g">符号绘制画布</param>
		/// <param name="sym">要绘制的符号</param>
		/// <param name="x">要绘制符号的水平坐标</param>
		/// <param name="y">要绘制符号的垂直坐标</param>
		/// <param name="fWidth">符号宽度,如果为0，则以指定的高度(fHeight) 和不变形方式（以符号原始的宽高比例）绘制</param>
		/// <param name="fHeight">符号高度,如果为0，则以指定的宽度(fWidth) 和不变形方式（以符号原始的宽高比例）绘制</param>
		/// <param name="fAngle">整体符号的角度</param>
		/// <param name="Unicolor">符号的单一颜色</param>
		public void DrawSymbol( Graphics g, CSymbol sym, double x, double y, double fWidth, double fHeight, float fAngle, Color Unicolor )
		{
			GraphicsState gstate	= g.Save();

			g.TranslateTransform( (float)x, (float)y );

			// 符号缩放比例
			if( fWidth == 0 )				// 绘制宽为0,指定的绘制高度缩放( (float)fHeight*(sym.Width/sym.Height), (float)fHeight );
			{
				sym.ScaleX			= fHeight/sym.Height;
				sym.ScaleY			= fHeight/sym.Height;
					
			}
			else if( fHeight == 0 )			// 绘制高为0,指定的绘制宽度缩放( ( float)fWidth, (float)fWidth/(sym.Width/sym.Height) );
			{
				sym.ScaleX			= fWidth/sym.Width;
				sym.ScaleY			= fWidth/sym.Width;
			}
			else							// 指定绘制宽/高缩放
			{
				sym.ScaleX			= fWidth/sym.Width;
				sym.ScaleY			= fHeight/sym.Height;
			}
			// 符号旋转角度
			sym.Angle				= fAngle;
			
			DrawSymbol( g, sym, (float)fWidth, (float)fHeight, Unicolor );

			g.Restore( gstate );
		}

		/// <summary>
		/// 实现 ISymbolDrawer接口的成员方法。在指定的位置，以指定的角度，宽高和单一颜色绘制符号。
		/// 本方法的目的是设计成一个通用的GDI API	绘图函数。坐标类型和GDI+相同.
		/// </summary>
		/// <param name="g">符号绘制画布</param>
		/// <param name="sym">要绘制的符号</param>
		/// <param name="fWidth">符号宽度,如果为0，则以指定的高度(fHeight) 和不变形方式（以符号原始的宽高比例）绘制</param>
		/// <param name="fHeight">符号高度,如果为0，则以指定的宽度(fWidth) 和不变形方式（以符号原始的宽高比例）绘制</param>
		/// <param name="Unicolor">符号的单一颜色</param>
		private void DrawSymbol( Graphics g, CSymbol sym, float fWidth, float fHeight, Color Unicolor )
		{
			GraphicsState gstate = g.Save();

			// 平移到符号中心
			_tmpMatrix.Reset();
			_tmpMatrix.Rotate( sym.Angle );
			_tmpPnt.X		= (float)sym.CenterPoint.X;
			_tmpPnt.Y		= (float)sym.CenterPoint.Y;
			PointF[] pts	= {_tmpPnt};
			_tmpMatrix.TransformPoints( pts );
			g.TranslateTransform( (float)pts[0].X, (float)pts[0].Y );

			foreach( CSymbolElement item in sym.Children ) // 画子符号
			{
				if( item is CSymbol )
					DrawSymbol( g, (CSymbol)item, fWidth, fHeight, Unicolor );
			}

			// 所有子图元都是以符号中心来进行旋转
			if( sym.Rotate )
				g.RotateTransform( sym.Angle );

			// 所有子图元都是以符号中心来进行缩放
			if( sym.ScaleStyle == ScaleStyles.Zoom )
			{
				g.ScaleTransform( (float)sym.ScaleX, (float)sym.ScaleY );
			}
			else if( sym.ScaleStyle == ScaleStyles.OriginalAspect )
			{
				double fMin = Math.Min( sym.ScaleX, sym.ScaleY );
				g.ScaleTransform( (float)fMin,(float)fMin );	
			}			

			CRectD rct		= new CRectD();
			rct.Left 		= rct.Width	 = 0;
			rct.Top			= rct.Height = 0;
			rct.Inflate(  sym.Width/2, sym.Height/2 );
			RectangleF rctF = rct.toRectangleF();
			// 画(子)符号边框
			Pen pen			= new Pen( Color.Green, (fWidth == 0) ? 2/fHeight : 2/fWidth );
			g.DrawRectangle( pen, rctF.Left, rctF.Top, rctF.Width, rctF.Height );
			
			// 画非子符号
			foreach( CSymbolElement item in sym.Children )
			{
				if( item is CSymText )
					DrawText( g, (CSymText)item, Unicolor );
				else if(item is CSymCurve)
					DrawCurve( g, (CSymCurve)item, Unicolor );
					// -----------------------------
					// 椭圆、扇形
				else if( item is CSymSector )
					DrawSector( g, (CSymSector)item, Unicolor );
				// -----------------------------
			}

			g.Restore( gstate );
		}

		#endregion

		#region 填充符号

		/// <summary>
		/// 符号充填
		/// </summary>
		/// <param name=" g ">符号绘制画布</param>
		/// <param name=" sym ">符号索引</param>
		/// <param name="fWidth">符号宽度</param>
		/// <param name="fHeight">符号高度</param>
		/// <param name="fAngle">整体符号的角度</param>
		/// <param name="color">符号的单一颜色</param>
		/// <param name=" path ">充填路径</param>
		/// <remarks>
		/// 实现技术：
		///	①　创建符号大小的内存位图作为画布，在其上进行绘制操作；
		///	②　完成后利用它创建纹理画刷，进行平铺，用画刷的旋转功能进行整体角度旋转；
		///	③　最后在指定的路径中利用纹理画刷进行填充。
		/// </remarks>
		/// <example>
		///	<code>	
		/// FillSymbol简单用法：
		/// GraphicsPath path = new GraphicsPath();
		/// path.AddEllipse( new Rectangle( 50, 50, 600, 600 ));
		/// FillSymbol( 5, 200, 200, 45.0f, Color.Empty, path );
		///	</code>
		/// </example>
		//	编写： jingzhou xu, 2005.12.27
		public void FillSymbol( Graphics g, CSymbol sym, double fWidth, double fHeight, float fAngle, Color color, GraphicsPath path )
		{
			// 原理：创建符号大小的内存位图作为画布，在其上进行绘制操作，
			// 完成后利用它创建纹理画刷，进行平铺，全体角度旋转等操作，
			// 最后在指定的路径中利用纹理画刷进行填充。jingzhou xu,2005.10.11
			Graphics oldGraphics	= g;
			Bitmap symbolBitmap		= new Bitmap( (int)fWidth, (int)fHeight );
			g						= Graphics.FromImage( symbolBitmap );

			g.SmoothingMode			= SmoothingMode.AntiAlias;
			// 在位图上绘制符号
			DrawSymbol( g, sym, fWidth/2, fHeight/2, fWidth, fHeight, 0.0f, color );
			//			symbolBitmap.Save( "e:\\1.bmp" );
			
			// 恢复原画布，并在原画布上绘制
			g						= oldGraphics;

			// 创建纹理画刷
			TextureBrush brush		= new TextureBrush( symbolBitmap, WrapMode.Tile );
			// 整体旋转指定角度
			brush.RotateTransform( fAngle );

			// 填充指定路径
			g.FillPath( brush, path );
			// 绘制路径
			g.DrawPath( SystemPens.Highlight, path );

			brush.Dispose ();
		}

		/// <summary>
		/// 用符号充填多边形
		/// </summary>
		/// <param name=" g ">符号绘制画布</param>
		/// <param name=" sym ">符号索引</param>
		/// <param name="fWidth">符号宽度</param>
		/// <param name="fHeight">符号高度</param>
		/// <param name="fAngle">整体符号的角度</param>
		/// <param name="color">符号的单一颜色</param>
		/// <param name=" polygon ">多边形点数组</param>
		/// <remarks>
		/// 实现技术：
		///	①　创建符号大小的内存位图作为画布，在其上进行绘制操作；
		///	②　完成后利用它创建纹理画刷，进行平铺，用画刷的旋转功能进行整体角度旋转；
		///	③　最后在指定的路径中利用纹理画刷进行填充。
		/// </remarks>
		/// <example>
		/// <c>FillSymbol使用示例</c>
		/// <code>
		/// // 坐标单位
		///	e.Graphics.PageUnit	= GraphicsUnit.Millimeter;
		///	e.Graphics.PageScale = 0.1f;
		///
		///	ISymbolDrawer	m_Drawer = new CSymbolDrawer();
		///	CSymbolLib		m_Lib = new CSymbolLib();
		///	
		///	// 打开符号库文件
		///	m_Lib.OpenLibFile("Symbol.xml");
		///	
		///	CSymbol sym	= m_Lib.GetSymbol( 0 );
		///	// 填充符号(多边形方式)
		///	const float fSide	= 500;
		///	PointF[] apt		= new PointF[5];
		///	for(int i = 0; i〈 apt.Length; i++)
		///	{
		///		double dAngle	= (i*0.8 - 0.5) * Math.PI;
		///		apt[i]			= new PointF( 
		///		(int)( fSide * ( 0.5 + 0.48 * Math.Cos(dAngle) ) + 400 ),
		///		(int)( fSide * ( 0.5 + 0.48 * Math.Sin(dAngle) ) )
		///		);
		///	}
		///	m_Drawer.FillSymbol( e.Graphics, sym, 50, 50, 0f, Color.Empty, apt );
		/// </code>
		/// </example>
		//	编写： jingzhou xu, 2005.12.27
		public void FillSymbol( Graphics g, CSymbol sym, double fWidth ,double fHeight, float fAngle, Color color, PointF[] polygon )
		{
			GraphicsPath path	= new GraphicsPath();
			path.AddPolygon( polygon );
			// 封闭路径
			path.CloseFigure();

			// 路径填充模式
			path.FillMode		= FillMode.Winding;
			// 进行指定路径符号填充
			FillSymbol( g, sym, fWidth, fHeight, fAngle, color, path );
		}

		/// <summary>
		/// 用符号充填矩形
		/// </summary>
		/// <param name=" g ">符号绘制画布</param>
		/// <param name=" sym ">符号索引</param>
		/// <param name="fWidth">符号宽度</param>
		/// <param name="fHeight">符号高度</param>
		/// <param name="fAngle">整体符号的角度</param>
		/// <param name="color">符号的单一颜色</param>
		/// <param name=" rect ">矩形大小</param>
		/// <remarks>
		/// 实现技术：
		///	①　创建符号大小的内存位图作为画布，在其上进行绘制操作；
		///	②　完成后利用它创建纹理画刷，进行平铺，用画刷的旋转功能进行整体角度旋转；
		///	③　最后在指定的路径中利用纹理画刷进行填充。
		/// </remarks>
		/// <example>
		/// <c>FillSymbol使用示例</c>
		/// <code>
		/// // 坐标单位
		///	e.Graphics.PageUnit	= GraphicsUnit.Millimeter;
		///	e.Graphics.PageScale = 0.1f;
		///
		///	ISymbolDrawer	m_Drawer = new CSymbolDrawer();
		///	CSymbolLib		m_Lib = new CSymbolLib();
		///	
		///	// 打开符号库文件
		///	m_Lib.OpenLibFile("Symbol.xml");
		///	
		///	CSymbol sym	= m_Lib.GetSymbol( 0 );
		///	// 填充符号(椭圆方式，单色)
		///	m_Drawer.FillSymbol( e.Graphics, sym, 100, 100, 45.0f, Color.Green, new Rectangle( 50, 50, 600, 600 ) );
		/// </code>
		/// </example>
		//	编写： jingzhou xu, 2005.12.27
		public void FillSymbol( Graphics g, CSymbol sym, double fWidth, double fHeight, float fAngle, Color color, RectangleF rect )
		{
			GraphicsPath path = new GraphicsPath();
			path.AddRectangle( rect );

			// 进行指定路径符号填充
			FillSymbol( g, sym, fWidth, fHeight, fAngle, color, path );
		}

		/// <summary>
		/// 用符号充填椭圆
		/// </summary>
		/// <param name=" g ">符号绘制画布</param>
		/// <param name=" sym ">符号索引</param>
		/// <param name="fWidth">符号宽度</param>
		/// <param name="fHeight">符号高度</param>
		/// <param name="fAngle">整体符号的角度</param>
		/// <param name="color">符号的单一颜色</param>
		/// <param name=" ellipseCeneter ">椭圆中心点</param>
		/// <param name=" fEllipseA ">椭圆a轴长度（X轴）</param>
		/// <param name=" fEllipseB ">椭圆b轴长度（Y轴）</param>
		/// <remarks>
		/// 实现技术：
		///	①　创建符号大小的内存位图作为画布，在其上进行绘制操作；
		///	②　完成后利用它创建纹理画刷，进行平铺，用画刷的旋转功能进行整体角度旋转；
		///	③　最后在指定的路径中利用纹理画刷进行填充。
		/// </remarks>
		/// <example>
		/// <c>FillSymbol使用示例</c>
		/// <code>
		/// // 坐标单位
		///	e.Graphics.PageUnit	= GraphicsUnit.Millimeter;
		///	e.Graphics.PageScale = 0.1f;
		///
		///	ISymbolDrawer	m_Drawer = new CSymbolDrawer();
		///	CSymbolLib		m_Lib = new CSymbolLib();
		///	
		///	// 打开符号库文件
		///	m_Lib.OpenLibFile("Symbol.xml");
		///	
		///	CSymbol sym	= m_Lib.GetSymbol( 0 );
		///	// 填充符号(椭圆方式，单色)
		///	m_Drawer.FillSymbol( e.Graphics, sym, 100, 100, 45.0f, Color.Green, new PointF(150, 1000), 300, 300 );
		/// </code>
		/// </example>
		//	编写： jingzhou xu, 2005.12.27
		public void FillSymbol( Graphics g, CSymbol sym, double fWidth, double fHeight, float fAngle, Color color, PointF ellipseCeneter, float fEllipseA, float fEllipseB )
		{
			GraphicsPath path	= new GraphicsPath();
			RectangleF rectf	= new RectangleF( ellipseCeneter.X - fEllipseA/2, ellipseCeneter.Y - fEllipseB/2, fEllipseA, fEllipseB );
			path.AddEllipse( rectf );

			// 进行指定路径符号填充
			FillSymbol( g, sym, fWidth, fHeight, fAngle, color, path );
		}

		#endregion

		#region 绘制曲线

		/// <summary>
		/// 绘制曲线符号
		/// </summary>
		/// <param name=" g ">符号绘制画布</param>
		/// <param name=" curve ">要绘制的曲线符号</param>
		/// <param name="Unicolor">符号的单一颜色</param>
		public virtual void DrawCurve( Graphics g, CSymCurve curve, Color Unicolor )
		{
			g.SmoothingMode = SmoothingMode.AntiAlias;	

			// 如果统一单色填充，则边界也用此颜色
			Pen pen;
			Brush brush;
			if( !Unicolor.IsEmpty )
			{
				pen		= new Pen( Unicolor, (float)curve.m_fBorderWidth );			// 单色绘制
				brush	= new SolidBrush( Unicolor );
			}
			else
			{
				pen		= new Pen( curve.m_clrBorderColor, (float)curve.m_fBorderWidth );
				brush	= new SolidBrush( Color.Black );
			}

			// 线形
			pen.DashStyle	= curve.m_nBorderStyle;

			switch( curve.m_nType )
			{
				case CurveType.Border:
					g.DrawLines( pen, (PointF[])curve.m_pts.ToArray(typeof(PointF)) );
					break;
				case CurveType.Fill:
					g.DrawLines( pen, (PointF[])curve.m_pts.ToArray(typeof(PointF)) );
					g.FillPolygon( brush, (PointF[])curve.m_pts.ToArray(typeof(PointF)), FillMode.Alternate );
					break;
				case CurveType.Both:
					g.DrawLines( pen, (PointF[])curve.m_pts.ToArray(typeof(PointF)) );
					g.FillPolygon( brush, (PointF[])curve.m_pts.ToArray(typeof(PointF)), FillMode.Alternate );
					break;
			}
		}

		#endregion

		#region 绘制文字

		/// <summary>
		/// 绘制文字符号
		/// </summary>
		/// <param name=" g ">符号绘制画布</param>
		/// <param name=" text ">要绘制的文字符号</param>
		/// <param name="Unicolor">符号的单一颜色</param>
		public virtual void DrawText( Graphics g, CSymText text, Color Unicolor )
		{
			GraphicsState gs	= g.Save();
			g.SmoothingMode		= SmoothingMode.AntiAlias;  

			// 平移、旋转
			CPointD ptOrg		= text.RotateOrigin;
			g.TranslateTransform( (float)ptOrg.X, (float)ptOrg.Y );		
			g.RotateTransform( text.Angle );

			// 文字左上角
			CPointD ptLT		= new CPointD();
			ptLT.X				= text.m_ptX - text.RotateOrigin.X;
			ptLT.Y				= text.m_ptY-text.RotateOrigin.Y;

			Font font			= new Font( text.m_FontName, (float)text.m_FontSize, text.m_FontStyle, GraphicsUnit.World );	
		
			// 如果统一单色填充，则边界也用此颜色
			Brush brush;
			if( !Unicolor.IsEmpty )
				brush = new SolidBrush( Unicolor );			
			else
				brush = new SolidBrush( text.m_FontColor );	

		　	g.DrawString( text.m_strText, font, brush, new PointF((float)ptLT.X, (float)ptLT.Y) );

			g.Restore( gs );
		}

		#endregion

		#region 绘制椭圆/扇形

		/// <summary>
		/// 绘制椭圆或扇形符号
		/// </summary>
		/// <param name=" g ">符号绘制画布</param>
		/// <param name=" sector ">要绘制的椭圆或扇形符号</param>
		/// <param name="Unicolor">符号的单一颜色</param>
		//	编写： jingzhou xu, 2005.12.27
		public virtual void DrawSector( Graphics g, CSymSector sector, Color Unicolor )
		{
			if( sector.m_rcBound.IsEmpty )
				return;

			// ---------------------------------------------------------------------------------------------------------------
			SmoothingMode sMode		= g.SmoothingMode ;
			g.SmoothingMode			= SmoothingMode.AntiAlias ;	

			CRectD rcD				= sector.m_rcBound;
			GraphicsState state		= g.Save();

			RectangleF rectF		= new RectangleF( (float)rcD.Left, (float)rcD.Top, (float)rcD.Width, (float)rcD.Height );
		
			// 平移、旋转
			g.TranslateTransform( (rectF.Left + rectF.Right)/2, (rectF.Top + rectF.Bottom)/2 );
			g.RotateTransform( sector.Angle );

			RectangleF tmpRelative	= new RectangleF( -rectF.Width/2, -rectF.Height/2, rectF.Width, rectF.Height );
			// 先填充
			if( sector.IsFill )
			{

				Fill fill = new Fill();		// 先默认构造填充画刷
				// 如果统一单色填充，则重构画刷，jingzhou xu,2005.12.28
				if( !Unicolor.IsEmpty )
				{
					fill.ConstructSolidBrush( Unicolor );
				}
				else
				{
					fill.ConstructSolidBrush( sector.FillColor );
				}

				Brush brush	= fill.MakeBrush( tmpRelative );

				g.FillPie( brush, tmpRelative.Left, tmpRelative.Top, tmpRelative.Width, tmpRelative.Height, sector.StartAngle, sector.SweepAngle );

				brush.Dispose() ;
			}

			// 后绘制边界线
			if( sector.IsBorderVisible )
			{

				// 如果统一单色填充，则边界也用此颜色，jingzhou xu,2005.12.28
				Pen pen;
				if( !Unicolor.IsEmpty )
					pen		= new Pen( Unicolor, (float)sector.PenWidth );
				else
					pen		= new Pen( sector.EdgeColor, (float)sector.PenWidth );

				// 边界线线形，jingzhou xu,2005.12.28
				pen.DashStyle	= sector.PenStyle;

				if( !sector.IsDrawArc )			// 扇形
				{
					
					g.DrawPie( pen, tmpRelative, sector.StartAngle, sector.SweepAngle );
					
				}
				else							// 弧形
				{
					g.DrawArc( pen, tmpRelative, sector.StartAngle, sector.SweepAngle );

				}

				pen.Dispose();

			}

			g.Restore( state );
			g.SmoothingMode	= sMode ;	
			// ---------------------------------------------------------------------------------------------------------------
	
		}

		#endregion
	}
}
