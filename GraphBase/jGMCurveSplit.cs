using System;
using System.Collections;
using System.Diagnostics;

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// 分割曲线算法。
	/// </summary>
	public class jGMCurveSplit
	{
		/// <summary>
		/// 多个分割体分割曲线
		/// </summary>
		/// <param name="lstPts">分割的曲线点数组列表</param>
		/// <param name="SplitItems">分割体数组</param>
		/// <param name="lstCurves">分割后的曲线数组列表</param>
		/// <returns>分割成功,返回true,否则返回false</returns>
		public  static  bool Split(IList lstPts,IList SplitItems, IList lstCurves)
		{
			//		lstCurves = new ArrayList();
			//		lstCurves.Clear();
			if(SplitItems.Count <= 0)
			{
				CPointDCollections lsttmpPts=  new CPointDCollections();
				for(int i = 0; i < lstPts.Count; i++)
				{
					lsttmpPts.Add(((CPointD)lstPts[i]));
				}
				lstCurves.Add(lsttmpPts);
				return true;

			}
			IList tmpPts = lstPts;

			CPointDCollections lstFrontPts= null;
			CPointDCollections lstRearPts = null;
			for(int i = 0;	i < SplitItems.Count; i++)
			{
				lstFrontPts= new CPointDCollections();
				lstRearPts = new CPointDCollections();
				CSplitItem si = ( CSplitItem)SplitItems[i];
				Split(tmpPts,si.fStartPos,si.fLen, lstFrontPts,lstRearPts);
				
				lstCurves.Add(lstFrontPts);
				tmpPts = lstRearPts;
		
			}
			lstCurves.Add(lstRearPts);
			return true;
		}

		/// <summary>
		/// 单个分割体多层分割曲线
		/// </summary>
		/// <param name="lstPts">分割的曲线点数组列表</param>
		/// <param name="splitPos">分割体位置</param>
		/// <param name="splitlength">分割体宽度</param>
		/// <param name="lstFrontPts">分割后,分割体前的曲线点数组</param>
		/// <param name="lstRearPts">分割后,分割体后的曲线点数组</param>
		/// <returns>分割成功,返回true,否则返回false</returns>
		public  static  bool Split(IList lstPts,double splitPos,double splitlength, CPointDCollections lstFrontPts, CPointDCollections lstRearPts)
		{
			if(lstPts.Count < 2)
				return true;
			lstFrontPts.Clear();
			lstRearPts.Clear();
		
			double fLeftBreakPos = splitPos ;
			double fRightBreakPos = splitPos + splitlength;
			int i = 0;
			double dblLength = 0;
			double dblCurLength =0;
			// 分割体左边以前的曲线点加入到曲线数组列表lstFrontPts
			while((dblLength < fLeftBreakPos )&&
				(i < (lstPts.Count - 1)))
			{
				dblCurLength = jGMA.Distance(((CPointD)lstPts[i]),((CPointD)lstPts[i + 1]));
				dblLength += dblCurLength;
				lstFrontPts.Add(((CPointD)lstPts[i]));
				i++;
				
			}
			double X0 = 0;
			double Y0 = 0;
			double k =0;
			//if( i == (lstPts.Count - 1))//所有点均在分割符前面
			//	return true;
			if(i > 0)
			{//将分割题左边和曲线的交点加入到前面列表
				k = (fLeftBreakPos-(dblLength - dblCurLength))/dblCurLength;//插入break item点的在p1 p2线段上的比例
				//插入点的坐标
				CPointD p1 = lstPts[i - 1] as CPointD;
				CPointD p2 = lstPts[i] as CPointD;
				X0 = p1.X +k*(p2.X-p1.X);
				Y0 = p1.Y + k*(p2.Y-p1.Y);
				lstFrontPts.Add(new CPointD(X0,Y0));
			}
			//过滤分割体包含的点
			while(( dblLength<fRightBreakPos) &&
				(i < (lstPts.Count - 1)))
			{
				dblCurLength = jGMA.Distance(lstPts[i] as CPointD, lstPts[i + 1] as CPointD);
				dblLength += dblCurLength;
				i++;
			}
			//计算分割体右边界和曲线的交点同时将交点和分割体右边的点加入到lstRearPts
			if( i == 0)
			{//全部点均在分割体的右侧
				while( i < lstPts.Count)
				{
					lstRearPts.Add(lstPts[i] as CPointD);
					i++;
				}
				return  true;
			}
			if( i < lstPts.Count)
			{ //穿过符号右边界
				CPointD p1 = lstPts[i - 1] as CPointD;
				CPointD p2 = lstPts[i] as CPointD;
				k = (fRightBreakPos-(dblLength - dblCurLength))/dblCurLength;//插入点(break item右边缘)在p1 p2线段上的比例
				//插入点的坐标
				X0 = p1.X +k*(p2.X-p1.X);
				Y0 = p1.Y + k*(p2.Y-p1.Y);
				lstRearPts.Add(new CPointD(X0,Y0));
				while( i < lstPts.Count)
				{
					lstRearPts.Add(lstPts[i] as CPointD);
					i++;
				}
			}
			return true;
		}
	}
}
