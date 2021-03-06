using System.Collections;

namespace Jurassic.Graph.Base
{
	/// <exclude/>
	/// <summary>
	/// 表示分割曲线的单个元素体（单个元素体可能有多个元素组成）
	/// 用分割体（符号、文本）分割曲线，用于绘制等值线等专业曲线
	/// </summary>
	public sealed class CSplitItem
	{
		/// <summary>
		/// 元素体中心离曲线起点的距离（沿着曲线测量）
		/// </summary>
		public float fStartPos;	

		/// <summary>
		/// 元素体长度
		/// </summary>
		public float fLen;			

		/// <summary>
		/// CSplitItem构造涵数
		/// </summary>
		/// <param name="startPos">起始距离</param>
		/// <param name="len">元素体长度</param>
		public CSplitItem( float startPos, float len )
		{
			fStartPos	= startPos;
			fLen		= len;
		}

		/// <summary>
		/// CSplitItem构造涵数，无参
		/// </summary>
		public CSplitItem()
		{
			fStartPos	= 0;
			fLen		= 0;
		}
	}
}
