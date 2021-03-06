using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.IO;			// 支持Stream流
using System.Drawing;

using Jurassic.Graph.Base;

namespace Jurassic.Graph.Drawer.Symbol
{
	#region 库信息

	/// <summary>
	/// 符号库信息结构。 
	/// </summary>
	public struct SymbolLibInfo
	{
		/// <summary>
		/// 符号库名称
		/// </summary>
		public string sLibName;   
		/// <summary>
		/// 符号库应用
		/// </summary>
		public string sApplication;  
		/// <summary>
		/// 符号库作者
		/// </summary>
		public string sAuthor; 

		/// <summary>
		/// SymbolLibInfo构造涵数
		/// </summary>
		/// <param name="name">库名称</param>
		/// <param name="application">符号库应用</param>
		/// <param name="author">符号库作者</param>
		public SymbolLibInfo(string name,string application,string author)
		{
			sLibName		= name;
			sApplication	= application;
			sAuthor			= author;
		}
	}

	#endregion

	/// <summary>
	/// 符号库类，用于对符号库进行各种操作。
	/// </summary>
	/// <example>
	/// <c>符号库使用示例</c>
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
	///	// 绘制符号
	///	sym = m_Lib.GetSymbol( 1, 0 );														// 默认符号(用 m_Lib.GetSymbol( 1 );可替换)
	///	m_Drawer.DrawSymbol( e.Graphics, sym, 200, 300, 250, 0, 45.0f, Color.Violet );		// 单色(高为0，按指定宽绘制)
	///	sym = m_Lib.GetSymbol( 1, 2 );														// 指定索引符号1中的第2帧
	///	m_Drawer.DrawSymbol( e.Graphics, sym, 600, 1000, 500, 250, 45.0f );		
	///	
	/// </code>
	/// <c>线型库中使用符号库示例</c>
	/// <code>
	/// // 创建绘制对象
	/// CCurveStyleDrawer drawer = new CCurveStyleDrawer();
	/// drawer.SymbolLib.OpenLibFile("Symbol.xml");
	///
	/// // 打开线型库
	/// CCurveStyleLib curveStyleLib = new CCurveStyleLib();
	/// curveStyleLib.OpenLibFile("curveStyle.xml");
	///
	/// // 数据
	/// CPointDCollections lstPnts = new CPointDCollections();
	/// lstPnts.Add(new CPointD(100,700));
	/// lstPnts.Add(new CPointD(400,300));
	/// lstPnts.Add(new CPointD(900,1200));
	///
	/// // 用ID=1的线型绘制曲线
	/// drawer.DrawCurveStyle(g, curveStyleLib.GetCurveStyle(1), lstPnts, 1, 1);
	/// </code>
	/// </example>
	//	编写：徐景周，2006.1.10
	public class CSymbolLib
	{
		#region 成员变量

		/// <summary>
		/// XML文档对象
		/// </summary>
		XmlDocument  m_xmlDoc			= new XmlDocument();

		/// <summary>
		/// 符号对象散列表
		/// </summary>
		private Hashtable m_hash		= new Hashtable();

		#endregion

		#region 构造涵数

		/// <summary>
		/// CSymbolLib构造涵数，无参
		/// </summary>
		public CSymbolLib()
		{
		
		}

		#endregion

		#region 打开库
		/// <summary>
		/// 打开库文件
		/// </summary>
		/// <param name="sFileName">根据文件名称打开指定的符号库</param>
		/// <returns>是否成功</returns>
		public bool OpenLibFile( string sFileName ) 
		{
			try
			{
				// 清空库
				ClearSymbolLib();

				XmlTextReader tr				= new XmlTextReader( sFileName );
				XmlValidatingReader reader		= new XmlValidatingReader( tr );
				reader.ValidationEventHandler	+= new ValidationEventHandler( this.ValidationEventHandle );
				m_xmlDoc.Load( reader );

				tr.Close();	
				reader.Close();
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 根据流对象打开指定的符号库
		/// </summary>
		/// <param name="stream">包含要读取的 XML 数据的流</param>
		/// <returns>是否成功</returns>
		public bool OpenLibFile( Stream stream )
		{
			// 清空库
			ClearSymbolLib();

			m_xmlDoc = new XmlDocument();

			try
			{
				XmlTextReader tr				= new XmlTextReader( stream );				
				XmlValidatingReader reader		= new XmlValidatingReader( tr );		// xml带框架验证的读取器
				reader.ValidationEventHandler	+= new ValidationEventHandler( this.ValidationEventHandle );
				m_xmlDoc.Load( reader );

				tr.Close();	
				reader.Close();
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		///根据xml文档对象打开指定的符号库
		/// </summary>
		/// <param name="xmlDoc"></param>
		/// <returns>是否成功</returns>
		public bool OpenLibFile( XmlDocument xmlDoc )
		{
			if( xmlDoc == null )
				return false;

			// 清空库
			ClearSymbolLib();

			m_xmlDoc = xmlDoc;

			return true;
		}

		#endregion

		#region 保存库

		/// <summary>
		/// 保存符号库，有问题(会抛出异常)
		/// </summary>
		/// <returns>是否成功</returns>
		public bool SaveLib()
		{
			try
			{
				( (XmlDataDocument)m_xmlDoc ).DataSet.AcceptChanges();
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 将符号库另存为
		/// </summary>
		/// <param name="sFileName">另存的符号库文件名</param>
		/// <returns>是否成功</returns>
		public bool SaveLib( string sFileName )
		{
			if( sFileName == "" )
				return false;

			m_xmlDoc.Save( sFileName );

			return true;
		}

		#endregion

		#region 缓存操作

		/// <summary>
		/// 加载符号库片断到缓存中，用于从图件文档内嵌的符号集中加载
		/// </summary>
		/// <param name="node">要加载的库节点</param>
		/// <returns>是否成功</returns>
		public bool LoadToBuffer( XmlNode node )
		{
			if( !node.HasChildNodes )
				return false;

			// 符号库片断加载
			foreach( XmlElement nodeChild in node.ChildNodes )
			{
				CSymbol symbol	= new CSymbol();
				int   nIndex	= int.Parse( nodeChild.GetAttribute( "id" ) );
				symbol.Read( nodeChild );

				m_hash[nIndex] = symbol;
			}

			return true;
		}

		/// <summary>
		/// 加载符号库片断到缓存中，用于从文件流内嵌的符号对象集中加载
		/// </summary>
		/// <param name="stream">包含符号对象数据的二进制输入流</param>
		/// <returns>是否成功</returns>
		public bool LoadToBuffer( Stream stream )
		{
			try
			{
				XmlDocument xmlDoc				= new XmlDocument();

				XmlTextReader tr				= new XmlTextReader( stream );
				XmlValidatingReader reader		= new XmlValidatingReader( tr );		// xml带框架验证的读取器
				reader.ValidationEventHandler	+= new ValidationEventHandler( this.ValidationEventHandle );
				xmlDoc.Load( reader );
				LoadToBuffer( xmlDoc.DocumentElement );

				tr.Close();	
				reader.Close();
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 将指定符号库加载到缓冲区中
		/// </summary>
		/// <param name="sFileName">库文件名</param>
		/// <returns>是否成功</returns>
		public bool LoadToBuffer( string sFileName )
		{
			try
			{
				OpenLibFile( sFileName );
				LoadToBuffer( (XmlNode)m_xmlDoc.DocumentElement );

			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 将缓存的符号对象保存到node节点下。用于将文档使用过的符号对象嵌入到文档中保存.
		/// </summary>
		/// <param name="node">保存符号对象数据的Xml节点</param>
		/// <returns>是否成功</returns>
		public bool SaveBuffer( XmlNode node )
		{
			if( m_hash.Count == 0 || node == null )
				return false;

			foreach(  int iNum in  m_hash.Keys )
			{
				CSymbol symbol = (CSymbol)m_hash[iNum];

				XmlElement sub = ( (XmlElement)node ).OwnerDocument.CreateElement( "Symbol", node.NamespaceURI );
				sub.SetAttribute( "id", "", iNum.ToString() );
				symbol.Write( sub );

				node.AppendChild( (XmlNode)sub );
			}

			return true;
		}

		/// <summary>
		/// 将缓存的符号对象保存到Stream流中。用于将使用过的符号对象嵌入到流中，只对新建的空流保存后的文件才有效。
		/// </summary>
		/// <example>
		/// <code>
		///	public bool Test_SaveBuffer_Stream()
		///	{
		///		// 读取到文件流中，只对新建的空流保存后的文件才有效。
		///		FileStream fs	= new FileStream( m_SaveFile, FileMode.Create, FileAccess.Write );
		///
		///		bool bResult	= false;
		///		bResult			= m_Lib.SaveBuffer( fs );
		///		Debug.Assert( bResult );
		///	
		///		// 关闭流文件
		///		fs.Close();
		///	
		///		return bResult;
		///	}
		/// </code>
		/// </example>
		/// <param name="stream">保存符号对象数据的流</param>
		/// <returns>是否成功</returns>
		public bool SaveBuffer( Stream stream )
		{
			XmlDocument xmlDoc = new XmlDocument();

			string rootname	 = "SymLib";
			XmlElement	root = xmlDoc.CreateElement( rootname, "http://www.jurassic.com.cn/SymML.xsd" );
			root.SetAttribute( "xmlns", "http://www.jurassic.com.cn/SymML.xsd" );
			root.SetAttribute( "xmlns:xsi","http://www.w3.org/2001/XMLSchema-instance" );
			root.SetAttribute( "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance","http://www.jurassic.com.cn/SymML.xsd 符号库.xsd" );			
			root.SetAttribute( "name", "" );	
			root.SetAttribute( "application", "" );		
			root.SetAttribute( "author", "" );
			// 符号库文件头部插入相关信息
			xmlDoc.AppendChild( root );
			XmlDeclaration xmldecl;
			xmldecl = xmlDoc.CreateXmlDeclaration( "1.0","UTF-8", "yes" );
			xmlDoc.InsertBefore( xmldecl, root );

			if( !SaveBuffer( (XmlNode)root ) )
				return false;

			xmlDoc.AppendChild( root );

			xmlDoc.Save( stream );

			return true;
		}

		/// <summary>
		/// 将缓存的符号保存到文件.
		/// </summary>
		/// <param name="sFileName">保存符号库文件名</param>
		/// <returns>是否成功</returns> 
		public bool SaveBuffer( string sFileName )
		{
			if( !SaveBuffer( (XmlNode)m_xmlDoc.DocumentElement ) )
				return false;

			SaveLib( sFileName );

			return true;
		}

		#endregion

		#region 清空库

		/// <summary>
		/// 清空已有的符号库内容
		/// </summary>
		public void ClearSymbolLib()
		{
			if( m_xmlDoc != null )
				m_xmlDoc.RemoveAll();

			if( m_hash != null )
				m_hash.Clear();

		}

		#endregion

		#region 库信息

		/// <summary>
		/// 创建新的符号库
		/// </summary>
		/// <param name="sLibFileName">库文件名</param>
		/// <param name="libInfo">库说明信息</param>
		/// <returns>是否成功</returns>
		public bool CreateNewLib( string sLibFileName, SymbolLibInfo  libInfo )
		{
			if( sLibFileName.Trim().Length == 0 )
				return false;
	
			// 创建根元素符号库
			if( m_xmlDoc != null )
				m_xmlDoc.RemoveAll();
			else
				m_xmlDoc = new XmlDocument();

			string rootname	 = "SymLib";
			XmlElement	root = m_xmlDoc.CreateElement( rootname, "http://www.jurassic.com.cn/SymML.xsd" );
			root.SetAttribute( "xmlns", "http://www.jurassic.com.cn/SymML.xsd" );
			root.SetAttribute( "xmlns:xsi","http://www.w3.org/2001/XMLSchema-instance" );
			root.SetAttribute( "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance","http://www.jurassic.com.cn/SymML.xsd 符号库.xsd" );			
			root.SetAttribute( "name", libInfo.sLibName );	
			root.SetAttribute( "application", libInfo.sApplication );		
			root.SetAttribute( "author", libInfo.sAuthor );

			// 符号库文件头部插入相关信息
			m_xmlDoc.AppendChild( root );
			XmlDeclaration xmldecl;
			xmldecl = m_xmlDoc.CreateXmlDeclaration( "1.0","UTF-8", "yes" );
			m_xmlDoc.InsertBefore( xmldecl, root );

			m_xmlDoc.Save( sLibFileName );

			return true;
		}

		/// <summary>
		/// 编辑符号库信息，保存结果，调用SaveLib
		/// </summary>
		/// <param name="libInfo">库说明信息</param>
		public void EditLibInfo( SymbolLibInfo  libInfo )
		{
			XmlNode oldNode = m_xmlDoc.DocumentElement;
			XmlElement root = (XmlElement)m_xmlDoc.DocumentElement;
			root.SetAttribute( "name", libInfo.sLibName );	
			root.SetAttribute( "application", libInfo.sApplication );		
			root.SetAttribute( "author", libInfo.sAuthor );

			m_xmlDoc.ReplaceChild( (XmlNode)root, oldNode );
		}

		/// <summary>
		/// 获取符号库说明信息
		/// </summary>
		/// <returns>符号库说明信息</returns>
		public SymbolLibInfo  GetLibInfo()
		{
			SymbolLibInfo libInfo;
			XmlNode oldNode			= m_xmlDoc.DocumentElement;
			XmlElement root			= (XmlElement)m_xmlDoc.DocumentElement;
			libInfo.sLibName		= root.GetAttribute( "name" );	
			libInfo.sApplication	= root.GetAttribute( "application" );		
			libInfo.sAuthor			= root.GetAttribute( "author" );

			return libInfo;
		}

		#endregion

		#region 库符号操作

		/// <summary>
		/// 将符号对象保存到符号库中，保存调用SaveLib。
		/// </summary>
		/// <param name="nID">符号对象ID</param>
		/// <param name="sym">符号对象</param>
		/// <returns>是否成功</returns>
		public bool PutSymbolToLib( int nID, CSymbol sym )
		{
			if( sym == null ) 
				return false;

			XmlElement	root	= m_xmlDoc.DocumentElement;

			XmlElement	sub		= m_xmlDoc.CreateElement( "Symbol", root.NamespaceURI );
			sub.SetAttribute( "id", "", nID.ToString() );
			sym.Write( sub );

			// 库中寻找
			XmlNode oldSub		= null;
			foreach( XmlElement node in root.ChildNodes )
			{
				int   nIndex	= int.Parse( node.GetAttribute( "id" ) );
				if( nIndex >= nID )
				{
					oldSub = (XmlNode)node;
					break;
				}
			}
			
			if( oldSub != null )
			{
				// 找到，前端插入
				root.InsertBefore( (XmlNode)sub, oldSub );
			}
			else
			{
				// 否则后端添加
				root.AppendChild( sub );
			}

			m_xmlDoc.ReplaceChild( (XmlNode)root, (XmlNode)m_xmlDoc.DocumentElement );

			return true;
		}

		/// <summary>
		/// 将符号中的nID符号对象（索引为nID）用指定的sym符号对象替换，保存调用SaveLib。
		/// </summary>
		/// <param name="nID">符号对象ID</param>
		/// <param name="sym">符号对象</param>
		/// <returns>是否成功</returns>
		public bool ReplaceSymbol( int nID, CSymbol sym )
		{
			if( sym == null )
				return false;

			XmlElement root = m_xmlDoc.DocumentElement;
			XmlElement sub	= m_xmlDoc.CreateElement( "Symbol", root.NamespaceURI );
			sub.SetAttribute( "id", "", nID.ToString() );
			sub.SetAttribute( "fullname", "", sym.SymbolFullName );
			sym.Write( sub );
	
			// 符号库中寻找
			XmlNode oldSub	= null;
			foreach( XmlElement node in root.ChildNodes )
			{
				int nIndex = int.Parse( node.GetAttribute( "id" ) );
				if( nIndex == nID )
				{
					oldSub = (XmlNode)node;
					break;
				}
			}

			if( oldSub != null )
			{
				// 找到，替换
				root.ReplaceChild( (XmlNode)sub, oldSub );
			}
			else// 没找到
				return false;

			// 符号库中替换
			m_xmlDoc.ReplaceChild( (XmlNode)root, (XmlNode)m_xmlDoc.DocumentElement );

			return true;
		}

		/// <summary>
		/// 将符号库中的索引为nID符号对象从库中删除，保存调用SaveLib。
		/// </summary>
		/// <param name="nID">符号索引号</param>
		/// <returns>是否成功</returns>
		public bool DelSymbolFromLib( int nID )
		{
			// 符号库中寻找
			XmlElement root = m_xmlDoc.DocumentElement;
			XmlNode oldSub	= null;
			foreach( XmlElement node in root.ChildNodes )
			{
				int nIndex	= int.Parse( node.GetAttribute( "id" ) );
				if( nIndex == nID )
				{
					oldSub	= (XmlNode)node;
					break;
				}
			}

			if( oldSub != null )
			{
				// 找到从符号库中删除
				root.RemoveChild( oldSub );
				m_xmlDoc.ReplaceChild( (XmlNode)root, (XmlNode)m_xmlDoc.DocumentElement );

				return true;
			}	
			
			return false;
		}

		#endregion

		#region 库索引

		/// <summary>
		/// 获得符号库索引信息，索引信息的数据文档结构符合索引格式规范。用于构造符号列表树。
		/// 获取的策略：首先从buffer中获得符号信息，再从符号库文件中获得符号信息，如果在buffer中有的符号，
		/// 就忽略符号库文件中的符号。
		/// </summary>
		/// <returns>xml文档</returns>
		public XmlDocument GetSymbolIndex()
		{
			XmlDocument xmlSymbolIndexInfo = new XmlDocument();

			// 创建根元素符号库
			string rootname		= "SymbolIndexs";
			XmlElement	root	= xmlSymbolIndexInfo.CreateElement( rootname );
			xmlSymbolIndexInfo.AppendChild(root);
			XmlDeclaration xmldecl;
			xmldecl				= xmlSymbolIndexInfo.CreateXmlDeclaration( "1.0", "UTF-8", "yes" );
			xmlSymbolIndexInfo.InsertBefore( xmldecl, root );

			XmlElement	node	= null;
			for( int iNum = 0; iNum < m_hash.Count; ++iNum )
			{
				CSymbol symbol			= (CSymbol)m_hash[iNum];
				string path				= symbol.SymbolFullName;
				if( path == "" )
					continue;
				// 根据"/"分隔符提取相应字符串到数组中
				string[ ] pathArray		= path.Split( '/' );

				XmlElement  parentNode	= root;
				for( int j = 0; j < pathArray.Length; ++j )
				{
					if( parentNode.GetElementsByTagName( pathArray[j] ).Count > 0 )
					{
						parentNode = (XmlElement)( parentNode.GetElementsByTagName( pathArray[j] )[0] );
						continue;
					}

					node = xmlSymbolIndexInfo.CreateElement( pathArray[j] );
					if( j != ( pathArray.Length - 1 ) )
					{
						( (XmlNode)parentNode ).AppendChild( node );

						parentNode = node;
					}
					else
					{
						node.SetAttribute( "index", "", iNum.ToString() );
						( (XmlNode)parentNode ).AppendChild( node );

						parentNode = null;
					}
				}
		
			}

			return xmlSymbolIndexInfo;
		}

		#endregion

		#region 获取库符号

		/// <summary>
		/// 根据符号ID和帧序得到符号对象，不实现缓存功能，用于选取符号。
		/// </summary>
		/// <param name="nID">符号索引号</param>
		/// <param name="nFrame">帧序号,从1开始,因为第0帧就是符号本身。</param>
		/// <returns>指定的符号</returns>
		public CSymbol GetSymbolNoBuffer( int nID, int nFrame )
		{
			CSymbol symbol = CreateSymbolFromXmlDoc( m_xmlDoc, nID );

			if( nFrame > 0 )
				return symbol.Frames[nFrame - 1];
			else
				return symbol;
		}

		/// <summary>
		/// 根据符号ID得到默认的符号对象，不实现缓存功能，用于选取符号。
		/// </summary>
		/// <param name="nID">符号索引号</param>
		/// <returns>指定的符号</returns>
		public CSymbol GetSymbolNoBuffer( int nID )
		{
			CSymbol symbol = CreateSymbolFromXmlDoc( m_xmlDoc, nID );

			if( symbol == null )
				return null;
			else
				return symbol;
		}

		/// <summary>
		/// 从符号库中取出指定索引号和帧号的符号，如果要求的符号在缓存(m_hash)中，
		/// 则返回缓存(m_hash)的符号对象，否则从符号文件中生成符号对象，同时放入缓存(m_hash)中。
		/// </summary>
		/// <param name="nID">符号索引号</param>
		/// <param name="nFrame">帧序号,从1开始,因为第0帧就是符号本身</param>
		/// <returns>指定的符号</returns>
		public CSymbol GetSymbol( int nID, int nFrame )
		{
			CSymbol symbol = GetSymbol( nID );

			if( symbol == null )
				return null;

			if( nFrame > 0 )
				return symbol.Frames[nFrame - 1];
			else
				return symbol;
		}

		/// <summary>
		/// 从符号库中取出指定索引的默认符号。
		/// </summary>
		/// <param name="nID">符号索引号</param>
		/// <returns>指定的符号</returns>
		public CSymbol GetSymbol( int nID )
		{

			if( m_hash[nID] == null )
			{
				CSymbol symbol = CreateSymbolFromXmlDoc( m_xmlDoc, nID );
				if( symbol == null )
					return null;

				m_hash[nID]	= symbol;

			}

			return (CSymbol)m_hash[nID];
		}

		/// <summary>
		/// 从符号库文件创建符号对象 
		/// </summary>
		/// <param name="doc">xml文档</param>
		/// <param name="nID">符号索引号</param>
		/// <returns>指定的符号</returns>
		private CSymbol CreateSymbolFromXmlDoc( XmlDocument doc, int nID )
		{
			CSymbol symbol = null;		
			
			// 创建符号库中指定索引号的符号
			XmlElement eml = doc.DocumentElement;
			foreach( XmlElement node in eml.ChildNodes )
			{
				string indexstr = node.GetAttribute( "id" );
				if( int.Parse( indexstr ) == nID )
				{			
					symbol = new CSymbol();		
					symbol.Read( node );	
			
					break;			
				}			
			}

			return symbol;
		}

		#endregion

		#region 库中缓存大小

		/// <summary>
		/// 返回符号库中缓冲区大小
		/// </summary>
		/// <returns>缓存大小</returns>
		public int GetSymbolBufferSize()
		{
			return  m_hash.Count;
		}

		#endregion

		#region 库校验

		/// <summary>
		/// 读取符号库符号时错误信息显示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private void ValidationEventHandle( object sender, ValidationEventArgs args )
		{
			Console.WriteLine( "\r\n\tValidation error: " + args.Message );
		}

		#endregion

		/// <summary>
		/// 创建符号绘制
		/// </summary>
		/// <param name="sName"></param>
		/// <returns></returns>
		public ISymbolDrawer CreateDrawer(string sName)
		{
			if( sName=="DefaultSymDrawer" )
				return new CSymbolDrawer();

			return new CSymbolDrawer();
		}
	}
}
