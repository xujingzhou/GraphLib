using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Jurassic.Graph.Drawer.Symbol
{
	/// <summary>
	/// 符号绘制接口，规范了符号绘制方法调用形式，其实现类可以实现各自的特殊绘制逻辑。绘制符号时，只绘制符号图元，而不再绘制符号图元中包含的帧。
	/// </summary>
	/// <remarks>
	/// 符号绘制，封闭了绘制符号的变化需求，使应用程序中的符号绘制方式易于更改。
	/// 调用CSymbolLib.GetSymbol获得符号对象，再调用本接口的成员函数绘制符号。
	///	在需要更改符号绘制方式时，实例化另一个符号绘制对象，来代替当前使用的符号绘制对象。
	/// </remarks>
    /// 编写：徐景周，2006.4.10
	public interface ISymbolDrawer
	{
		#region 绘制符号

		/// <summary>
		/// 在指定的位置，以指定的角度，宽高绘制符号。
		/// 本方法的目的是设计成一个通用的GDI API	绘图函数。坐标类型和GDI+相同.
		/// </summary>
		/// <param name=" g ">符号绘制画布</param>
		/// <param name=" sym ">要绘制的符号</param>
		/// <param name="x">要绘制符号的水平坐标</param>
		/// <param name="y">要绘制符号的垂直坐标</param>
		/// <param name="fWidth">符号宽度,如果为0，则以指定的高度(fHeight) 和不变形方式（以符号原始的宽高比例）绘制</param>
		/// <param name="fHeight">符号高度,如果为0，则以指定的宽度(fWidth) 和不变形方式（以符号原始的宽高比例）绘制</param>
		/// <param name="fAngle">整体符号的角度</param>
		void DrawSymbol(Graphics g,CSymbol sym,float x,float y,float fWidth,float fHeight,float fAngle);

		/// <summary>
		/// 在指定的位置，以指定的角度，宽高和单一颜色绘制符号。
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
		void DrawSymbol(Graphics g,CSymbol sym,double x,double y,double fWidth,double fHeight,float fAngle,Color Unicolor);

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
		/// <code>
		/// 简单用法：
		/// GraphicsPath path = new GraphicsPath();
		/// path.AddEllipse( new Rectangle( 50, 50, 600, 600 ));
		/// FillSymbol( 5, 200, 200, 45.0f, Color.Empty, path );
		/// </code>
		/// </example>
		/// 编写：徐景周，2005.12.26
		void FillSymbol(Graphics g,CSymbol sym,double fWidth,double fHeight,float fAngle,Color color,GraphicsPath path);

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
		/// 编写：徐景周，2005.12.26 
		void FillSymbol(Graphics g,CSymbol sym,double fWidth,double fHeight,float fAngle,Color color,PointF[] polygon);

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
		/// 编写：徐景周，2005.12.26
		void FillSymbol(Graphics g,CSymbol sym,double fWidth,double fHeight,float fAngle,Color color,RectangleF rect);

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
		/// 编写：徐景周，2005.12.26
		void FillSymbol(Graphics g,CSymbol sym,double fWidth,double fHeight,float fAngle,Color color,PointF ellipseCeneter,float fEllipseA,float fEllipseB);

		#endregion
	}
}
