using System;

namespace Jurassic.Graph.Drawer.Symbol
{
	/// <summary>
	/// 符号元素(CSymbolElement)集合
	/// </summary>
	public class CSymElmCollection : System.Collections.CollectionBase
	{
		#region 变量及默认值
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
		public void Remove( CSymbolElement value )  
		{
			List.Remove( value );
		}
		public bool Contains( CSymbolElement value )  
		{
			// If value is not of type CSymbolElement, this will return false.
			return( List.Contains( value ) );
		}
		/// <summary>
		/// 在向 CollectionBase 实例中插入新元素之前执行其他自定义进程。
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		protected override void OnInsert( int index, Object value )  
		{
			//	if ( value.GetType().GetInterface( "Jurassic.GraphPlatform.SymbolLib.CSymbolElement" ) == null )
			//		throw new ArgumentException( "value must be of type CSymbolElement.", "value" );
		}
		protected override void OnRemove( int index, Object value )  
		{
			//	if ( value.GetType().GetInterface( "Jurassic.GraphPlatform.SymbolLib.CSymbolElement" ) == null )
			//		throw new ArgumentException( "value must be of type CSymbolElement.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			//		if ( newValue.GetType().GetInterface( "Jurassic.GraphPlatform.SymbolLib.CSymbolElement" ) == null )
			//		throw new ArgumentException( "newValue must be of type CSymbolElement.", "newValue" );
		}
		protected override void OnValidate( Object value )  
		{
			//		if ( value.GetType().GetInterface( "Jurassic.GraphPlatform.SymbolLib.CSymbolElement" ) == null )
			//			throw new ArgumentException( "value must be of type CSymbolElement." );
		}
		#endregion // Standard Function
	}
}
