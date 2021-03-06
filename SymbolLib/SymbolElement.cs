using System;
using System.Drawing;
using Jurassic.Graph.Base;

namespace Jurassic.Graph.Drawer.Symbol
{
	/// <summary>
	/// 符号及符号图元的基类。作为子符号的符号也被视作符号图元。
    /// 编写：徐景周，2006.4.10
	/// </summary>
	public abstract class CSymbolElement
	{
		/// <summary>
		/// 旋转角度
		/// </summary>
		public float m_fAngle;								 // 旋转角度

		/// <summary>
		/// 旋转原点
		/// </summary>
		public CPointD m_RotateOrigin	= new CPointD(0,0);  // 旋转基点
		
		/// <summary>
		/// 符号旋转基点
		/// </summary>
		public virtual CPointD RotateOrigin
		{
			get
			{
				return m_RotateOrigin;
			}
			set
			{
				m_RotateOrigin = value; 
			}
		}

		/// <summary>
		/// 符号旋转角度
		/// </summary>
		public virtual float Angle
		{
			get 
			{ 
				return m_fAngle; 
			}
			set 
			{
				m_fAngle = value; 
			}
		}

		/// <summary>
		/// CSymbolElement构造涵数
		/// </summary>
		public CSymbolElement()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 从xml元素读取数据
		/// </summary>
		/// <param name="elm">包含数据的xml节点</param>
		/// <returns>是否成功</returns>
		public abstract bool Read( System.Xml.XmlElement elm );

		/// <summary>
		/// 将数据写到xml元素
		/// </summary>
		/// <param name="elm">包含数据的xml节点</param>
		/// <returns>是否成功</returns>
		public abstract bool Write( System.Xml.XmlElement elm );
	}


}
