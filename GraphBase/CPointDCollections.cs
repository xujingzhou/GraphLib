using System;
using System.Runtime.InteropServices;
using System.Collections;

namespace Jurassic.Graph.Base
{
	/// <exclude/>
	/// <summary>
	/// CPointD点的集合。
    /// 编写：徐景周，2006.4.10
	/// </summary>
	[Serializable]
	[ComVisible(true)]
	public class CPointDCollections : CollectionBase
	{
		#region 构造函数

		/// <summary>
		/// 构造函数
		/// </summary>
		public CPointDCollections( ) : base()
		{
		}

		#endregion 

		#region Standard Function

		/// <summary>
		/// 矢量索引器 
		/// </summary>
		public CPointD this[int index]  
		{
			get  
			{
				return( List[index]  as CPointD );
			}
			set  
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// 向集合中添加点
		/// </summary>
		/// <param name="pnt">点</param>
		/// <returns>加入点位置</returns>
		public int Add( CPointD pnt )  
		{
			return( List.Add( pnt ) );
		}

		/// <summary>
		/// 集合中指定点的索引
		/// </summary>
		/// <param name="pnt">点</param>
		/// <returns>指定点索引</returns>
		public int IndexOf( CPointD pnt )  
		{
			return( List.IndexOf( pnt ) );
		}

		/// <summary>
		/// 向集合中指定索引位置中插入点
		/// </summary>
		/// <param name="index">索引号</param>
		/// <param name="pnt">点</param>
		public void Insert( int index, CPointD pnt )  
		{
			List.Insert( index, pnt );
		}

		/// <summary>
		/// 删除集合中的点
		/// </summary>
		/// <param name="pnt">点</param>
		public void Remove( CPointD pnt )  
		{
			List.Remove( pnt );
		}

		/// <summary>
		/// 集合中是否包含指定点
		/// </summary>
		/// <param name="pnt">点</param>
		/// <returns>是否包含指定点</returns>
		public bool Contains( CPointD pnt )  
		{
			return( List.Contains( pnt ) );
		}

		#endregion // Standard Function

	}


}
