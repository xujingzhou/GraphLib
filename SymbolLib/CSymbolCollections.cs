using System;
using System.Collections;

namespace Jurassic.Graph.Drawer.Symbol
{
	/// <summary>
	/// 符号(CSymbol)集合，用于表示符号帧。
	/// </summary>
	//	编写：徐景周，2006.1.10
	public class CSymbolCollections : CollectionBase
	{
		#region 变量及默认值

		/// <summary>
		/// 符号(CSymbol)集合的索引器
		/// </summary>
		public CSymbol this[int index]  
		{
			get  
			{
				return( (CSymbol) List[index] );
			}

			set  
			{
				List[index] = value;
			}
		}

		#endregion // 变量及默认值

		#region 构造函数

		/// <summary>
		/// 默认构造
		/// </summary>
		public CSymbolCollections() : base()
		{
		}

		#endregion //构造函数
		
		#region 继承IList函数

		/// <summary>
		/// 实现IList.Add方法，将CSymbol添加到 CollectionBase 的结尾处。
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add( CSymbol value )  
		{
			return ( List.Add( value ) );
		}

		/// <summary>
		/// 实现IList.IndexOf 方法，搜索指定的 CSymbol，并返回整个 CollectionBase 中第一个匹配项的从零开始的索引。
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf( CSymbol value )  
		{
			return ( List.IndexOf( value ) );
		}

		/// <summary>
		/// 实现IList.Insert 方法，将元素插入 CollectionBase 的指定索引处。
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert( int index, CSymbol value )  
		{
			List.Insert( index, value );
		}

		/// <summary>
		/// 实现IList.Remove 方法，将元素value删除。
		/// </summary>
		/// <param name="value"></param>
		public void Remove( CSymbol value )  
		{
			List.Remove( value );
		}

		/// <summary>
		/// 实现IList.Remove 方法，将指定index索引元素删除。
		/// </summary>
		/// <param name="index">索引号</param>
		/// <seealso cref="IList.Remove"/>
		public void Remove( int index )
		{
			if ( index >= 0 && index < List.Count )
				List.RemoveAt( index );
		}

		/// <summary>
		/// 实现IList.Contains 方法，是否List中包含value元素。
		/// </summary>
		/// <param name="value"></param>
		public bool Contains( CSymbol value )  
		{
			return ( List.Contains( value ) );
		}
		
		#endregion 
	}
}
