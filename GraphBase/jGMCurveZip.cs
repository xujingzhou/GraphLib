using System;
using System.Collections;

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// 曲线抽稀算法。
	/// </summary>
	public class jGMCurveZip
	{
		/// <summary>
		/// 抽稀折线的点数
		/// </summary>
		/// <param name="lstPts">内容被改写，折点的有序数组，输入待抽稀的折线，输出抽稀后的折线。</param>
		/// <param name="E">
		/// 输入，误差,当距离小于E时可忽略不计。
		/// 对于1比1,000,000地图, E = 200 压缩成1比2,000,000, 以此类推
		/// </param>
		/// <returns>抽稀后折线的点数</returns>
		/// <remarks>
		/// 如果IList的大小是不可变的，则抽稀后的折线lstPts中前 返回点数 的节点是抽稀后的节点，后面的节点都没有用了
		/// ( 0,1,2,...抽稀后折线的点数-1)为抽稀后的节点，（抽稀后折线的点数,...最后一个点）是无效的。
		/// 如果IList的大小是可变的，则抽稀后的折线点就是lstPts中全部的点。
		/// </remarks>
		public static int ZipCurve( IList lstPts, double E )
		{
			return ZipCurve( lstPts, E, 0, lstPts.Count-1 );
		}
		/// <summary>
		/// 抽稀折线局部区间的点数
		/// </summary>
		/// <param name="lstPts">内容被改写，折点的有序数组，输入待抽稀的折线，输出抽稀后的折线。</param>
		/// <param name="E">
		/// 输入，误差,当距离小于E时可忽略不计。
		/// 对于1比1,000,000地图, E = 200 压缩成1比2,000,000, 以此类推
		/// </param>
		/// <param name="nStart">曲线抽稀时的起始位置</param>
		/// <param name="nEnd">曲线抽稀时的结束位置</param>
		/// <returns>（nStart、nEnd）区间内，抽稀后折线的点数</returns>
		/// <remarks>
		/// 如果IList的大小是不可变的，则抽稀后的折线lstPts中（nStart+返回点数，nEnd）之间的节点是无用的
		/// ( 0,1,2,...,nStart,nStart+1,...,nStart+返回点数-1 )有效；( nStart+返回点数,...,nEnd )无效；（nEnd+1,...最后一个点）有效。
		/// 如果IList的大小是可变的，则抽稀后的折线点就是lstPts中全部的点。
		/// </remarks>
		public static int ZipCurve( IList lstPts, double E, int nStart, int nEnd )
		{
			if ( E <= 0 )		// ahr	2006-03-02
				return -1;

//			int nSize = lstPts.Count;
			int nSize = nEnd-nStart+1;
			if (nSize<3) return -1;

			double H;
			int M;
			int M1 = nStart;	//0;
			int M2 = nEnd;		//nSize-1;
			int TTT=M1;
			int T = 1;
			int[] L = new int[nSize];
			L[0]=M1; L[1]=M2;
			while (true)
			{
				_ZipMD( lstPts, M1, M2, out H, out M );
				if (H>E)	//	显著、分段，设左边的段为处理段
				{
					T++;
					L[T] = M;
					M1 = M1;
					M2 = M;
				}
				else	//	左边的段起伏不显著，将段右边的点（显著点）依次放在左边，设右边的段为处理段
				{
					TTT++;
					((CPointD)lstPts[TTT]).X = ((CPointD)lstPts[M2]).X;
					((CPointD)lstPts[TTT]).Y = ((CPointD)lstPts[M2]).Y;
					T--;
					if (T<1) break;
					M1=M2;
					M2=L[T];
				}
			}
			TTT++;
			if ( TTT > nSize )
				TTT = nSize;
			else
			{	//	如果lstPts是可以改变大小，缩减lstPts
				if ( false == lstPts.IsFixedSize )
				{
					int nDelAt = nStart + TTT;
					for ( int n = nDelAt; n<=nEnd; n++ )
						lstPts.RemoveAt( nDelAt );
				}
			}
			return TTT;
		}

		#region 内部使用的过程
		/// <summary>
		/// 计算曲线局部段之间到局部段两端点构成的直线的最大玄背高度
		/// 调用者：ZipLine
		/// </summary>
		/// <param name="lstPts">点数组，输入</param>
		/// <param name="m1">线段起始点序号，输入</param>
		/// <param name="m2">线段终止点序号，输入</param>
		/// <param name="h">最大玄高，输出</param>
		/// <param name="m">具有最大玄高的节点序号，输出</param>
		/// <remarks>
		/// 如果m1和m2点重合，玄背高度为节点到m1点的距离。
		/// </remarks>
		private static void	_ZipMD( IList lstPts,  int m1, int m2, out double h, out int m )
		{
			//			int nSize = lstPts.Count;
			double A,B,C,D,DD;
			jGMA.LineFomuleFrom2Pnt( (CPointD)lstPts[m1], (CPointD)lstPts[m2], out A, out B, out C );
			D = Math.Sqrt( A*A + B*B );
			h = -1.0; m = m1; 
			if ( jGMA.DoubleEquals( D, 0.0 ) )	
			{	//	两点重合
				for ( int i=m1; i<=m2; i++ )
				{
					DD = jGMA.Distance2( (CPointD)lstPts[m1], (CPointD)lstPts[i] );
					if (DD>h)
					{
						h = DD;
						m = i;
					}
				}
				h = Math.Sqrt( h );
			}
			else
			{
				A /= D; B /= D; C /= D;
				for ( int i=m1; i<=m2; i++ )
				{
					DD = Math.Abs( A*((CPointD)lstPts[i]).X + B*((CPointD)lstPts[i]).Y + C );
					if (DD>h)
					{
						h = DD;
						m = i;
					}
				}
			}
		}
	#endregion
	}
}
