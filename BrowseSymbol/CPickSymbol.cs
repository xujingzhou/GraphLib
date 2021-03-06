using System.Collections;
using System.Windows.Forms;

namespace BrowseSymbol
{
	/// <summary>
	/// 符号库符号选取类
    /// 徐景周，2006.3.28
	/// </summary>
	public class CPickSymbol
	{
		
		public CPickSymbol()
		{
			
		}
		
		// 从库中选取符号
		public void PickSymbolFromFile( string sLibFileName, out int nSymID, out int nSymFrame )
		{
			nSymID = nSymFrame = -1;

			CBrowseSymbolsDlg browes = new CBrowseSymbolsDlg( sLibFileName, true );
			if( browes.ShowDialog() == DialogResult.Cancel ) 
				return;

			nSymID		= (int)browes.m_pairCurSymID.First;
			nSymFrame	= (int)browes.m_pairCurSymID.Second;
		}

		// 从缓存中选取符号
		public void PickSymbolFromTable( Hashtable table, out int nSymID, out int nSymFrame )
		{
			nSymID = nSymFrame = -1;

			CBrowseSymbolsDlg browes = new CBrowseSymbolsDlg( table, false );
			if( browes.ShowDialog() == DialogResult.Cancel ) 
				return;

			nSymID		= (int)browes.m_pairCurSymID.First;
			nSymFrame	= (int)browes.m_pairCurSymID.Second;
		}
	}
}
