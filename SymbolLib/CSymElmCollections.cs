using System;

namespace Jurassic.Graph.Drawer.Symbol
{
	/// <summary>
	/// 符号、子符号等元素(CSymbolElement)集合。
    /// 编写：徐景周，2006.4.10
	/// </summary>
	public class CSymElmCollections : System.Collections.CollectionBase
	{
		#region 变量及默认值

		/// <summary>
		/// CSymbolElement索引器
		/// </summary>
		public CSymbolElement this[ int index ]  
		{
			get  
			{
				return( (CSymbolElement) List[index] );
			}
			set  
			{
				List[index] = value;
			}
		}

		#endregion // 变量及默认值

		#region 构造函数
		#endregion //构造函数
	
		#region Standard Function

		/// <summary>
		/// 实现IList.Add方法，将CSymbolElement添加到 CollectionBase 的结尾处。
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add( CSymbolElement value )  
		{
			return( List.Add( value ) );
		}

		/// <summary>
		/// 实现IList.IndexOf 方法，搜索指定的 CSymbolElement，并返回整个 CollectionBase 中第一个匹配项的从零开始的索引。
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf( CSymbolElement value )  
		{
			return( List.IndexOf( value ) );
		}

		/// <summary>
		/// 实现IList.Insert 方法，将元素插入 CollectionBase 的指定索引处。
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert( int index, CSymbolElement value )  
		{
			List.Insert( index, value );
		}

		/// <summary>
		/// 实现IList.Remove 方法，将指定元素删除。
		/// </summary>
		/// <param name="value">值</param>
		public void Remove( CSymbolElement value )  
		{
			List.Remove( value );
		}

		/// <summary>
		/// 实现IList.Contains 方法，是否List中包含value元素。
		/// </summary>
		/// <param name="value"></param>
		public bool Contains( CSymbolElement value )  
		{
			// If value is not of type CSymbolElement, this will return false.
			return( List.Contains( value ) );
		}

		#endregion // Standard Function
	}
}
