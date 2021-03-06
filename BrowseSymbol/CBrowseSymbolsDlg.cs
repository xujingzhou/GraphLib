using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Web.UI;			// 使用Pair
using Jurassic.Graph.Base;
using Jurassic.Graph.Drawer.Symbol;

namespace BrowseSymbol
{
	/// <summary>
	/// 符号库符号选取对话框,jingzhou xu
	/// </summary>
	public class CBrowseSymbolsDlg : System.Windows.Forms.Form
	{
		#region 窗体组件成员

		private System.Windows.Forms.ImageList  imageList;

		private System.ComponentModel.IContainer components;
		#endregion
		#region 类成员变量

		XmlDocument             m_doc;				// xml文档对象
		XmlValidatingReader     m_reader;		
		
		public	 Pair			m_pairCurSymID;		// 当前符号的索引及帧号(ID, FrameID)
		private  string	        m_sLibFileName;
		private  Hashtable		m_table;

		private  bool			m_bFromFile;

		ISymbolDrawer m_Drawer	= new CSymbolDrawer();
		CSymbolLib m_Lib		= new CSymbolLib();

		private System.Windows.Forms.Panel panel_SymbolRegion;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox m_textBoxName;
		private System.Windows.Forms.TextBox m_textBoxID;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button m_cancel;
		private System.Windows.Forms.Button m_ok;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel m_SymView;
		private System.Windows.Forms.ListBox m_SymlistBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox m_cbFrameID;
		private System.Windows.Forms.TreeView m_SymTree;
		

		#endregion
		#region 构造函数
		public CBrowseSymbolsDlg()
		{
			// Windows 窗体设计器支持所必需的
			InitializeComponent();

			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			m_pairCurSymID		= new Pair( (int)-1, (int)-1 );
			m_sLibFileName		= "";
			m_bFromFile			= true;
			this.AcceptButton	= this.m_ok;		// 默认回车
			this.CancelButton	= this.m_cancel;	// 默认ESC

		}

		public CBrowseSymbolsDlg(object Symlib, bool bFromfile)
		{
			// Windows 窗体设计器支持所必需的
			InitializeComponent();

			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			m_pairCurSymID		= new Pair( (int)-1, (int)-1 );
			m_sLibFileName		= "";
			m_bFromFile			= bFromfile;
			this.AcceptButton	= this.m_ok;		// 默认回车
			this.CancelButton	= this.m_cancel;	// 默认ESC

			if(m_bFromFile == true)
			{
				m_sLibFileName = (string)Symlib;
			}
			else
			{
				m_table = (Hashtable)Symlib;
			}

		}
		#endregion		
		#region 清理所有正在使用的资源。
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion
		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CBrowseSymbolsDlg));
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.panel_SymbolRegion = new System.Windows.Forms.Panel();
			this.m_cbFrameID = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.m_textBoxName = new System.Windows.Forms.TextBox();
			this.m_textBoxID = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.m_cancel = new System.Windows.Forms.Button();
			this.m_ok = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.m_SymView = new System.Windows.Forms.Panel();
			this.m_SymlistBox = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.m_SymTree = new System.Windows.Forms.TreeView();
			this.panel_SymbolRegion.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panel_SymbolRegion
			// 
			this.panel_SymbolRegion.Controls.Add(this.m_cbFrameID);
			this.panel_SymbolRegion.Controls.Add(this.label6);
			this.panel_SymbolRegion.Controls.Add(this.m_textBoxName);
			this.panel_SymbolRegion.Controls.Add(this.m_textBoxID);
			this.panel_SymbolRegion.Controls.Add(this.label5);
			this.panel_SymbolRegion.Controls.Add(this.label4);
			this.panel_SymbolRegion.Controls.Add(this.m_cancel);
			this.panel_SymbolRegion.Controls.Add(this.m_ok);
			this.panel_SymbolRegion.Controls.Add(this.label3);
			this.panel_SymbolRegion.Controls.Add(this.m_SymView);
			this.panel_SymbolRegion.Controls.Add(this.m_SymlistBox);
			this.panel_SymbolRegion.Controls.Add(this.label2);
			this.panel_SymbolRegion.Controls.Add(this.label1);
			this.panel_SymbolRegion.Controls.Add(this.m_SymTree);
			this.panel_SymbolRegion.Location = new System.Drawing.Point(0, 8);
			this.panel_SymbolRegion.Name = "panel_SymbolRegion";
			this.panel_SymbolRegion.Size = new System.Drawing.Size(512, 344);
			this.panel_SymbolRegion.TabIndex = 15;
			// 
			// m_cbFrameID
			// 
			this.m_cbFrameID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbFrameID.Items.AddRange(new object[] {
															 "0"});
			this.m_cbFrameID.Location = new System.Drawing.Point(424, 220);
			this.m_cbFrameID.Name = "m_cbFrameID";
			this.m_cbFrameID.Size = new System.Drawing.Size(80, 20);
			this.m_cbFrameID.TabIndex = 27;
			this.m_cbFrameID.SelectedIndexChanged += new System.EventHandler(this.m_cbFrameID_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(424, 204);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 16);
			this.label6.TabIndex = 26;
			this.label6.Text = "帧索引：";
			// 
			// m_textBoxName
			// 
			this.m_textBoxName.Location = new System.Drawing.Point(339, 276);
			this.m_textBoxName.Name = "m_textBoxName";
			this.m_textBoxName.ReadOnly = true;
			this.m_textBoxName.Size = new System.Drawing.Size(160, 21);
			this.m_textBoxName.TabIndex = 25;
			this.m_textBoxName.Text = "xxx";
			// 
			// m_textBoxID
			// 
			this.m_textBoxID.Location = new System.Drawing.Point(339, 220);
			this.m_textBoxID.Name = "m_textBoxID";
			this.m_textBoxID.ReadOnly = true;
			this.m_textBoxID.Size = new System.Drawing.Size(77, 21);
			this.m_textBoxID.TabIndex = 23;
			this.m_textBoxID.Text = "-1";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(339, 260);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 16);
			this.label5.TabIndex = 24;
			this.label5.Text = "符号名称：";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(339, 204);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.TabIndex = 22;
			this.label4.Text = "符号索引：";
			// 
			// m_cancel
			// 
			this.m_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_cancel.Location = new System.Drawing.Point(427, 308);
			this.m_cancel.Name = "m_cancel";
			this.m_cancel.TabIndex = 21;
			this.m_cancel.Text = "取消";
			this.m_cancel.Click += new System.EventHandler(this.m_cancel_Click);
			// 
			// m_ok
			// 
			this.m_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ok.Location = new System.Drawing.Point(339, 308);
			this.m_ok.Name = "m_ok";
			this.m_ok.TabIndex = 20;
			this.m_ok.Text = "确定";
			this.m_ok.Click += new System.EventHandler(this.m_ok_Click);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("宋体", 11F);
			this.label3.Location = new System.Drawing.Point(339, 4);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 19;
			this.label3.Text = "预览：";
			// 
			// m_SymView
			// 
			this.m_SymView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_SymView.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.m_SymView.Cursor = System.Windows.Forms.Cursors.Default;
			this.m_SymView.Location = new System.Drawing.Point(339, 28);
			this.m_SymView.Name = "m_SymView";
			this.m_SymView.Size = new System.Drawing.Size(160, 160);
			this.m_SymView.TabIndex = 18;
			this.m_SymView.Paint += new System.Windows.Forms.PaintEventHandler(this.m_SymView_Paint);
			// 
			// m_SymlistBox
			// 
			this.m_SymlistBox.ItemHeight = 12;
			this.m_SymlistBox.Location = new System.Drawing.Point(179, 28);
			this.m_SymlistBox.Name = "m_SymlistBox";
			this.m_SymlistBox.Size = new System.Drawing.Size(144, 304);
			this.m_SymlistBox.TabIndex = 17;
			this.m_SymlistBox.SelectedIndexChanged += new System.EventHandler(this.m_SymlistBox_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("宋体", 11F);
			this.label2.Location = new System.Drawing.Point(179, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 16;
			this.label2.Text = "列表：";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("宋体", 11F);
			this.label1.Location = new System.Drawing.Point(11, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 15;
			this.label1.Text = "目录：";
			// 
			// m_SymTree
			// 
			this.m_SymTree.HideSelection = false;
			this.m_SymTree.ImageList = this.imageList;
			this.m_SymTree.Location = new System.Drawing.Point(11, 28);
			this.m_SymTree.Name = "m_SymTree";
			this.m_SymTree.SelectedImageIndex = 1;
			this.m_SymTree.Size = new System.Drawing.Size(160, 304);
			this.m_SymTree.TabIndex = 14;
			this.m_SymTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_SymTree_AfterSelect);
			this.m_SymTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.m_SymTree_BeforeSelect);
			// 
			// CBrowseSymbolsDlg
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(512, 349);
			this.Controls.Add(this.panel_SymbolRegion);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CBrowseSymbolsDlg";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "符号选取";
			this.Load += new System.EventHandler(this.CBrowseSymbolsDlg_Load);
//			this.ChangeUICues += new System.Windows.Forms.UICuesEventHandler(this.CBrowseSymbolsDlg_ChangeUICues);
			this.panel_SymbolRegion.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		#region 加载、初始化
		private void CBrowseSymbolsDlg_Load(object sender, System.EventArgs e)
		{
			
			InitGraphics();

			// 打开符号库文件
			m_Lib.OpenLibFile(m_sLibFileName);

			if(this.m_bFromFile ==  true)
			{
				FillTreeFromFile(m_sLibFileName);
			}
			else
			{
				FillTreeFromCatch(m_table);
			}

			// 帧索引初始选中
			m_cbFrameID.SelectedIndex = 0;
		}	
		
		void InitGraphics()
		{
			
		}

		#endregion

		#region  构造树，选取符号

		/// <summary>
		/// 读取符号库符号时错误信息显示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private void ValidationEventHandle (object sender, ValidationEventArgs args)
		{
			Console.WriteLine("\r\n\tValidation error: " + args.Message );
			System.Windows.Forms.MessageBox.Show(args.Message);
		}

		/// <summary>
		/// 构造树从符号库文件
		/// </summary>
		/// <param name="sLibFileName">文件名称</param>
		void FillTreeFromFile(string sLibFileName)
		{
			if(sLibFileName == "") 
				return;

			this.m_SymTree.ImageList		= this.imageList;
			m_doc							= new XmlDocument();
			XmlTextReader tr				= new XmlTextReader(sLibFileName);
			m_reader						= new XmlValidatingReader(tr);
			m_reader.ValidationEventHandler += new ValidationEventHandler (this.ValidationEventHandle);
			m_doc.Load(m_reader);
			XmlElement eml					= m_doc.DocumentElement;			
			foreach(XmlElement node in eml.ChildNodes)
			{
				TreeNode subnode				= null;
				TreeNodeCollection subnodelist	= this.m_SymTree.Nodes;
			//	string index = node.GetAttribute("id");
				string path						= node.GetAttribute("fullname");
				string[ ] patharray				= path.Split('/');
				if(patharray.Length > 0)
				{					
					for(int j=0; j<patharray.Length-1; j++)
					{
						subnode		= findnode(patharray[ j ],subnodelist,subnode);					
						subnodelist = subnode.Nodes;					
					}
				}				
			}
			tr.Close();		
		}

		/// <summary>
		/// 寻找树节点
		/// </summary>
		private TreeNode findnode(string nodetext, TreeNodeCollection  nodelist, TreeNode pNode)
		{
			TreeNode curnode	= null;
			TreeNode pnode		= pNode;
			int count			= nodelist.Count;
			for(int i=0; i<count; i++)
			{
				pnode = nodelist[ i ].Parent;
				if(nodetext == nodelist[ i ].Text)
				{
					curnode = nodelist[ i ];
					break;
				}
			}

			if(curnode == null)
			{
				curnode = new TreeNode(nodetext);
				if(pnode==null)
					this.m_SymTree.Nodes.Add(curnode);
				else
					pNode.Nodes.Add(curnode);
			}			
			return curnode;			
		}

		/// <summary>
		/// 构造树从缓存
		/// </summary>
		/// <param name="table">缓存</param>
		void FillTreeFromCatch(Hashtable table)
		{
			CSymbol sym = new CSymbol();
			foreach(object item in table.Values)
			{
				sym					= (CSymbol)item;
				string path			= sym.SymbolFullName;
				string[ ] patharray = path.Split('/');
				if(patharray.Length > 0)
				{		
					TreeNode subnode				= null;
					TreeNodeCollection subnodelist	= this.m_SymTree.Nodes;

					for(int j=0; j<patharray.Length-1; j++)
					{
						subnode		= findnode(patharray[ j ],subnodelist,subnode);					
						subnodelist = subnode.Nodes;					
					}
				}		
			}
			
		}

		/// <summary>
		/// 根据符号ID和帧号从符号库文件读取指定符号帧
		/// </summary>
		/// <param name="nID">符号ID</param>
		/// <param name="nFrame">符号帧号</param>
		/// <returns>符号对象</returns>
		CSymbol GetSymFromFile( int nID, int nFrame )
		{
			CSymbol sym	= m_Lib.GetSymbol( nID, nFrame );

			return sym;
		}

		/// <summary>
		/// 根据符号ID从符号库文件读取符号
		/// </summary>
		/// <param name="nID">符号ID</param>
		/// <returns>符号对象</returns>
		CSymbol GetSymFromFile(int nID)
		{
			CSymbol sym	= m_Lib.GetSymbol( nID );

			return sym;
		}

		/// <summary>
		/// 根据符号ID从符号库缓存读取符号
		/// </summary>
		/// <param name="nID">符号ID</param>
		/// <returns>符号对象</returns>
		CSymbol GetSymFromCatch(int nID)
		{
			CSymbol sym	= m_Lib.GetSymbol( nID );

			return sym;
		}
		
		#endregion

		#region 符号绘制、刷新
	
		/// <summary>
		/// 绘制预览符号
		/// </summary>
		private void m_SymView_Paint( object sender, System.Windows.Forms.PaintEventArgs e )
		{
			Graphics g = e.Graphics;	

			if( (int)this.m_pairCurSymID.First == -1 ) 
				return;

			CSymbol sym = GetSymFromFile( (int)this.m_pairCurSymID.First, (int)this.m_pairCurSymID.Second );
			if( sym == null ) 
				return;
			
			m_Drawer.DrawSymbol( g, sym, 80, 80, 150, 150, 0.0f );

			base.OnPaint( e );
		}

		/// <summary>
		/// 刷新显示
		/// </summary>
		void InvalidatePreviewSymbol()
		{		
			Invalidate( true );	
		}


		#endregion

		#region 事件响应

		private void m_SymTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeNode node = this.m_SymTree.SelectedNode;	
			if(this.m_bFromFile)
			{
				if(this.m_sLibFileName == "")
					return;

				m_doc							= new XmlDocument();
				XmlTextReader tr				= new XmlTextReader(this.m_sLibFileName);
				m_reader						= new XmlValidatingReader(tr);
				m_reader.ValidationEventHandler += new ValidationEventHandler (this.ValidationEventHandle);
				m_doc.Load(m_reader);		
				string path						= "";
				path							= node.Text;
				for(;1>0;)
				{
					node		= node.Parent;
					if(node == null)
					{
						break;
					}
					string str	= node.Text;
					path		= str + "/" + path;
				}

				//this.m_curpath = path;
				this.m_SymlistBox.Items.Clear();
				XmlElement elmt = m_doc.DocumentElement;
				foreach(XmlElement subnode in elmt.ChildNodes)
				{
					string fullname = subnode.GetAttribute("fullname");
					string index	= subnode.GetAttribute("id");

					if(fullname.LastIndexOf(path) > -1)
					{
						string[] patharray	= fullname.Split('/');
						Itemdata itemdata	= new Itemdata(index,patharray[patharray.Length-1]  );
						int itemindex		= this.m_SymlistBox.Items.Add(itemdata);		
					}
				}
				tr.Close();
			}
			else
			{
				string path = "";
				path		= node.Text;
				for(; 1>0; )
				{
					node		= node.Parent;
					if(node == null)
					{
						break;
					}
					string str	= node.Text;
					path		=str + "/" + path;
				}
				this.m_SymlistBox.Items.Clear();				
				foreach(int id in this.m_table.Keys)
				{
					CSymbol subnode = (CSymbol)m_table[id];
					string fullname = subnode.SymbolFullName;
					string index ;
					if(fullname.LastIndexOf(path) > -1)
					{
						string[] patharray	= fullname.Split('/');
						string name			= patharray[patharray.Length-1] ;
						index				= id.ToString(); 
						Itemdata itemdata	= new Itemdata(index,name );
						int itemindex		= this.m_SymlistBox.Items.Add(itemdata);		
					}
				}
				
			}

			// 更新符号帧下拉列表(默认帧为0)，避免列表帧内容与选中树不同步问题
			m_cbFrameID.Items.Clear();
			m_cbFrameID.Items.Add( "0" );
			m_cbFrameID.SelectedIndex = 0;

		}

		private void m_SymlistBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int count = this.m_SymlistBox.Items.Count;
			for(int i=0; i< count; i++)
			{
				if(this.m_SymlistBox.GetSelected(i))
				{
					Itemdata data				= (Itemdata)this.m_SymlistBox.Items[i];
					this.m_textBoxID.Text		= data.itemindex;
					this.m_textBoxName.Text		= data.itemname;
					this.m_pairCurSymID.First	= int.Parse(data.itemindex);

					// 更新符号帧下拉列表(默认帧为0)
					CSymbol sym	= m_Lib.GetSymbol( (int)this.m_pairCurSymID.First );
					for( int nFrame = 1; nFrame <= sym.Frames.Count; ++nFrame )
					{
						m_cbFrameID.Items.Add( nFrame.ToString() );
					}
					m_cbFrameID.SelectedIndex = 0;

					InvalidatePreviewSymbol();			
					break;					
				}
			}	
		}

		// 符号帧选择改变时
		private void m_cbFrameID_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.m_pairCurSymID.Second = (int)m_cbFrameID.SelectedIndex; 
			InvalidatePreviewSymbol();
		}

		private void m_SymTree_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			e.Node.SelectedImageIndex = 1;
		}

		private void m_ok_Click(object sender, System.EventArgs e)
		{
			this.m_pairCurSymID.First  = int.Parse(this.m_textBoxID.Text);
			this.m_pairCurSymID.Second = (int)m_cbFrameID.SelectedIndex ; 
			
		}

		private void m_cancel_Click(object sender, System.EventArgs e)
		{
			this.m_pairCurSymID.First  = (int)-1;
			this.m_pairCurSymID.Second = (int)-1;
		}

		private void m_textBoxName_TextChanged(object sender, System.EventArgs e)
		{
		
		}
		private void m_textBoxID_TextChanged(object sender, System.EventArgs e)
		{
		
		}
		#endregion	
		
	}

	#region 列表项数据

	/// <summary>
	/// 列表项中存储的数据结构
	/// </summary>
	public class Itemdata
	{
		private string index ; ///符号索引
		private string name ; ///符号名称
		public  Itemdata(string index, string name)
		{
			this.name = name;
			this.index = index;
		}
		public  string itemname
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}
		public string itemindex
		{
			get
			{
				return index;
			}
			set
			{
				index =value;
			}
		}
		public override string ToString()
		{
			return this.name;
		}
	}

	#endregion
}
