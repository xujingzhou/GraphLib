using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Jurassic.Graph.Base;
using Jurassic.Graph.Drawer.Symbol;
using BrowseSymbol;

namespace TestSymbol
{
	/// <summary>
	/// 符号库测试
    /// 编写：徐景周，2006.4.10
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		ISymbolDrawer	m_Drawer		= new CSymbolDrawer();
		CSymbolLib		m_Lib			= new CSymbolLib();

		CSymbol			m_Symbol		= null;
		string			m_CurFilename	= null;

		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem_Open;
		private System.Windows.Forms.MenuItem menuItem_Save;
		private System.Windows.Forms.MenuItem menuItem_SaveAs;
		private System.Windows.Forms.MenuItem menuItem_Append;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem_TestGeo;
		private System.Windows.Forms.MenuItem menuItem_TestSymbolLib;

		// 窗体淡出效果定时器,jingzhou xu
		protected internal System.Timers.Timer tmrFade;

		public Form1()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			// 打开符号库文件
			m_Lib.OpenLibFile("Symbol.xml");


/*			// 窗体淡出效果,jingzhou xu
			this.Opacity	= 0;
			this.Show();
			tmrFade.Enabled = true;
*/
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.tmrFade = new System.Timers.Timer();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem_Open = new System.Windows.Forms.MenuItem();
			this.menuItem_Save = new System.Windows.Forms.MenuItem();
			this.menuItem_SaveAs = new System.Windows.Forms.MenuItem();
			this.menuItem_Append = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem_TestGeo = new System.Windows.Forms.MenuItem();
			this.menuItem_TestSymbolLib = new System.Windows.Forms.MenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.tmrFade)).BeginInit();
			// 
			// tmrFade
			// 
			this.tmrFade.Interval = 20;
			this.tmrFade.SynchronizingObject = this;
			this.tmrFade.Elapsed += new System.Timers.ElapsedEventHandler(this.tmrFade_Elapsed);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem3});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem_Open,
																					  this.menuItem_Save,
																					  this.menuItem_SaveAs,
																					  this.menuItem_Append});
			this.menuItem1.Text = "文件";
			// 
			// menuItem_Open
			// 
			this.menuItem_Open.Index = 0;
			this.menuItem_Open.Text = "打开...";
			this.menuItem_Open.Click += new System.EventHandler(this.menuItem_Open_Click);
			// 
			// menuItem_Save
			// 
			this.menuItem_Save.Index = 1;
			this.menuItem_Save.Text = "保存...";
			this.menuItem_Save.Click += new System.EventHandler(this.menuItem_Save_Click);
			// 
			// menuItem_SaveAs
			// 
			this.menuItem_SaveAs.Index = 2;
			this.menuItem_SaveAs.Text = "另存为...";
			this.menuItem_SaveAs.Click += new System.EventHandler(this.menuItem_SaveAs_Click);
			// 
			// menuItem_Append
			// 
			this.menuItem_Append.Index = 3;
			this.menuItem_Append.Text = "追加到库...";
			this.menuItem_Append.Click += new System.EventHandler(this.menuItem_Append_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem_TestGeo,
																					  this.menuItem_TestSymbolLib});
			this.menuItem3.Text = "测试";
			// 
			// menuItem_TestGeo
			// 
			this.menuItem_TestGeo.Index = 0;
			this.menuItem_TestGeo.Text = "测试基础几何算法";
			this.menuItem_TestGeo.Click += new System.EventHandler(this.menuItem_TestGeoAlgorithm_Click);
			// 
			// menuItem_TestSymbolLib
			// 
			this.menuItem_TestSymbolLib.Index = 1;
			this.menuItem_TestSymbolLib.Text = "测试符号库操作(关联IE比较)";
			this.menuItem_TestSymbolLib.Click += new System.EventHandler(this.menuItem_TestSymbolLib_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(792, 485);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "符号库测试";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			((System.ComponentModel.ISupportInitialize)(this.tmrFade)).EndInit();

		}
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		// 窗体淡出效果定时哭函数,jingzhou xu
		private void tmrFade_Elapsed( object sender, System.Timers.ElapsedEventArgs e )
		{
/*			this.Opacity += 0.05;
			if( this.Opacity >= .95 )
			{
				this.Opacity	= 1;
				tmrFade.Enabled = false;
			}
*/		}

		/// <summary>
		/// 符号绘制、填充在此处完成,jingzhou xu,2005.12.28
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Paint( object sender, System.Windows.Forms.PaintEventArgs e )
		{

			// 坐标单位
			e.Graphics.PageUnit = GraphicsUnit.Millimeter;
			e.Graphics.PageScale = 0.1f;

			// --------------------------------------------------------------------------
			// 打开单个符号并绘制
			if( m_Symbol != null )
				m_Drawer.DrawSymbol( e.Graphics, m_Symbol, 700, 600, 250, 250, 0.0f );
			// --------------------------------------------------------------------------

			CSymbol sym			= m_Lib.GetSymbol( 0 );
			// 填充符号(椭圆方式，单色)
			m_Drawer.FillSymbol( e.Graphics, sym, 100, 100, 45.0f, Color.Green, new PointF(150, 1000), 300, 300 );

			// 填充符号(多边形方式)
			const float fSide	= 500;
			PointF[] apt		= new PointF[5];
			for(int i = 0; i < apt.Length; i++)
			{
				double dAngle	= (i*0.8 - 0.5) * Math.PI;
				apt[i]			= new PointF( 
					(int)( fSide * ( 0.5 + 0.48 * Math.Cos(dAngle) ) + 400 ),
					(int)( fSide * ( 0.5 + 0.48 * Math.Sin(dAngle) ) )
					);
			}
			m_Drawer.FillSymbol( e.Graphics, sym, 50, 50, 0f, Color.Empty, apt );


			// 绘制符号
			sym = m_Lib.GetSymbol( 1, 0 );
			m_Drawer.DrawSymbol( e.Graphics, sym, 200, 300, 250, 0, 45.0f, Color.Violet );		// 单色(高为0，按指定宽绘制)
			sym = m_Lib.GetSymbol( 1, 2 );														// 指定帧
			m_Drawer.DrawSymbol( e.Graphics, sym, 600, 1000, 500, 250, 45.0f );		

			sym = m_Lib.GetSymbol( 2, 0 );
			m_Drawer.DrawSymbol( e.Graphics, sym, 1000, 300, 0, 250, 45.0f );					// 宽为0，按指定高绘制
			sym = m_Lib.GetSymbol( 2, 2 );														// 指定帧
			m_Drawer.DrawSymbol( e.Graphics, sym, 1000, 1000, 500, 250, 45.0f, Color.Violet );	// 单色
	
			sym = m_Lib.GetSymbol( 3 );
			m_Drawer.DrawSymbol( e.Graphics, sym, 1400, 300, 300, 300, 45.0f );
			m_Drawer.DrawSymbol( e.Graphics, sym, 1400, 1000, 500, 250, 45.0f );

			sym = m_Lib.GetSymbol( 4 );
			m_Drawer.DrawSymbol( e.Graphics, sym, 1900, 300, 0, 300, 45.0f );					// 宽为0，按指定高绘制
			m_Drawer.DrawSymbol( e.Graphics, sym, 1900, 1000, 500, 250, 45.0f );

		}

		/// <summary>
		/// 打开符号库中符号
		/// </summary>
		private void menuItem_Open_Click(object sender, System.EventArgs e)
		{
			if( !openFileDialog_FileName() )
				return;

			// 打开符号库文件，并选取指定索引符号
			m_Lib.OpenLibFile( m_CurFilename );
			CPickSymbol form	= new CPickSymbol();
			int nSymID, nSymFrame;
			form.PickSymbolFromFile( m_CurFilename, out nSymID, out nSymFrame );
			if( nSymID == -1 )
				return;
			
			if( nSymFrame > 0 )
				m_Symbol = m_Lib.GetSymbol( nSymID, nSymFrame );
			else
				m_Symbol = m_Lib.GetSymbol( nSymID );

			Invalidate();
		}

		/// <summary>
		/// 从文件打开对话框中获取符号库名
		/// </summary>
		private bool openFileDialog_FileName()
		{
			openFileDialog1.Filter = "(*.xml)|*.xml";
			if( openFileDialog1.ShowDialog() == DialogResult.Cancel )
				return false;

			m_CurFilename = openFileDialog1.FileName;

			return true;
		}

		/// <summary>
		/// 从文件保存对话框中获取符号库名
		/// </summary>
		private bool saveFileDialog_FileName()
		{
			saveFileDialog1.Filter = "(*.xml)|*.xml";
			if( saveFileDialog1.ShowDialog() == DialogResult.Cancel )
				return false;

			m_CurFilename = saveFileDialog1.FileName;

			return true;
		}

		/// <summary>
		/// 保存符号库文件
		/// </summary>
		private bool SaveSymbolLib( string sFileName )
		{
			SymbolLibInfo libInfo;
			libInfo.sLibName		= "";	
			libInfo.sApplication	= "";
			libInfo.sAuthor			= "";
			m_Lib.CreateNewLib( sFileName, libInfo );
			m_Lib.SaveBuffer( sFileName );

			return true;
		}

		/// <summary>
		/// 保存符号库
		/// </summary>
		private void menuItem_Save_Click(object sender, System.EventArgs e)
		{
			if( m_CurFilename == null )
			{
				if( !saveFileDialog_FileName() )
					return;
			}
			
			SaveSymbolLib( m_CurFilename );
		}

		/// <summary>
		/// 另存为符号库
		/// </summary>
		private void menuItem_SaveAs_Click(object sender, System.EventArgs e)
		{
			if( !saveFileDialog_FileName() )
				return;

			SaveSymbolLib( m_CurFilename );
		}

		/// <summary>
		/// 将当前打开的符号追加到指定符号库中
		/// </summary>
		private void menuItem_Append_Click(object sender, System.EventArgs e)
		{
			// 当前符号存在
			if( m_Symbol != null )
			{
				if( !openFileDialog_FileName() )
					return;
			
				// 打开要追加到的符号库文件
				m_Lib.LoadToBuffer( m_CurFilename );

				// 在尾部添加当前符号
				m_Lib.PutSymbolToLib( m_Lib.GetSymbolBufferSize()+1, m_Symbol );
				m_Lib.SaveLib( m_CurFilename );
			}
		}

		/// <summary>
		/// 测试基础几何算法
		/// </summary>
		private void menuItem_TestGeoAlgorithm_Click(object sender, System.EventArgs e)
		{
			TestGeoAlgorithm test = new TestGeoAlgorithm();
			test.TestAll();
		}

		/// <summary>
		/// 测试符号库
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem_TestSymbolLib_Click(object sender, System.EventArgs e)
		{
			TestSymbolLib test = new TestSymbolLib();
			test.Test_All();
		}

	}
}
