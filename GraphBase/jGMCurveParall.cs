using System;
using System.Collections;
using System.Diagnostics;

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// 构造曲线的平行线算法。
	/// 多种平行线生成方法：Intersect、Mix、Cut。
	/// </summary>
	public class jGMCurveParall
	{
		/// <summary>
		/// 平行线算法,对于连接点偏移采用交点
		/// </summary>
		/// <param name="lstPts">原始曲线点数组</param>
		/// <param name="fOffset">平行偏移量</param>
		/// <param name="lstPtsParallel">偏移后的曲线点数组</param>
		/// <returns>处理成功,返回true,否则返回false</returns>
		public  static  bool Parall_Intersect(IList lstPts,float fOffset, IList lstPtsParallel)
		{
			try
			{
				int iNum = lstPts.Count;
				CPointD[] ptOrg = new CPointD[iNum];

				int i = 1;
				int j = 0;
				int nLine = 0;
				CPointD Pt = new CPointD(0.0,0.0);

				for(i =0; i < iNum; i++)
				{
					ptOrg[i] = ( CPointD )lstPts[i];
				}
				CPointD[] Line1 = new CPointD[2];
				CPointD[] Line2 = new CPointD[2];

				bool bClose = false;
				//计算出第一段平行线段。
				double dSinA = 0.0,dCosA = 0.0;
				double dSinB = 0.0,dCosB = 0.0;
				double dSinX = 0.0,dCosX = 0.0;
				i = 1;
				while( (false ==_CalcSinCos(ptOrg[i-1], ptOrg[i], out dSinA,out dCosA)) && (i < iNum) ) i++;
				Line2[0] = _OffsetPnt(ptOrg[i-1], fOffset, dCosA, -dSinA);
				lstPtsParallel.Add( Line2[0] );
				Line2[1] = _OffsetPnt(ptOrg[ i ], fOffset, dCosA, -dSinA);
				lstPtsParallel.Add( Line2[1] );

				int nPoint = i + 1;
				if( ptOrg[0].IsEquals( ptOrg[iNum-1] ) ) 
				{
					bClose = true;
				}
				int nCondition = 0;
				if(bClose == true)
					nCondition = iNum+1 ;
				else
					nCondition = iNum;
				for(i = nPoint; i < nCondition; i++)
				{
					if( i != iNum)
						j = i;
					else
						j = nPoint-1;
					if( !_CalcSinCos(ptOrg[j-1], ptOrg[j], out dSinB,out  dCosB) ) continue;
					Line2[0] = _OffsetPnt(ptOrg[j-1], fOffset, dCosB, -dSinB);
					Line2[1] = _OffsetPnt(ptOrg[ j ], fOffset, dCosB, -dSinB);
					//计算两个线段法线键底的夹角 dJudge。
					dSinX = dCosA * dSinB - dSinA * dCosB;
					dCosX = dSinA * dSinB + dCosA * dCosB;
					double dJudge = 0.0;
					if(  jGMA.DoubleEquals(dCosX,0) == true ) dJudge = Math.PI / 2.0;
					else dJudge = Math.Atan(Math.Abs(dSinX/dCosX));
					if( dCosX < 0.0 ) dJudge = Math.PI - dJudge;
					if( dSinX < 0.0 ) dJudge *= -1.0;

					nLine = lstPtsParallel.Count - 1;
					Line1[0] =(CPointD) lstPtsParallel[nLine-1];
					Line1[1] = (CPointD) lstPtsParallel[nLine];
					if( Math.Abs( dJudge )*jGMC.R2D < 5.0 )	
					{// 原始曲线中两条线段近似平行线(两线间的夹角小于5度)。
						//--------------------------------------------------------------
						//张伟强 2005-9-7修改原来的点设置算法
						Pt.X =  (float)((Line1[1].X + Line2[0].X) / 2.0 );
						Pt.Y =  (float)((Line1[1].Y + Line2[0].Y) / 2.0 );
						//Pt.X =  (float)(Line1[1].X + (Line2[1].X - Line2[0].X) / 2.0  );
						//Pt.Y =  (float)(Line1[1].Y + (Line2[1].Y - Line2[0].Y) / 2.0  );
						//-------------------------------------------------------
						lstPtsParallel.RemoveAt(lstPtsParallel.Count - 1);
						lstPtsParallel.Add(Pt);
						lstPtsParallel.Add( Line2[1] );
						
					}
					else
					{//	原始曲线中两条线段是弯曲折线。
						double dT1 =0.0;
						double dT2 = 0.0;
						Pt = jGMA.CP_Sect2Sect(Line1[0], Line1[1],Line2[0], Line2[1],ref dT1,ref dT2 );
						while( (dT1 < 0.0) && (nLine > 1) )
						{// 消除内侧平行线出现的三角效应。
							if(nLine < lstPtsParallel.Count)
							{
								lstPtsParallel.RemoveAt( nLine );	
								nLine--;
							}
							Line1[0] = (CPointD)lstPtsParallel[nLine-1];
							Line1[1] = (CPointD)lstPtsParallel[ nLine ];
							Pt = jGMA.CP_Sect2Sect(Line1[0], Line1[1],Line2[0], Line2[1],ref dT1,ref dT2 );
						}
						if( (0.0 <= dT1) && (dT1 <= 1.0 ) )
						{//	两个生成线段相交，则生成节点在原始线段夹角内。
							if( i == iNum )
							{// 封闭曲线的处理。
								lstPtsParallel.RemoveAt(0);
								lstPtsParallel.Insert(0,Pt);
								lstPtsParallel.RemoveAt(lstPtsParallel.Count - 1);
								lstPtsParallel.Add(Pt);
							}
							else
							{
								lstPtsParallel.RemoveAt(lstPtsParallel.Count - 1);
								lstPtsParallel.Add(Pt);
								lstPtsParallel.Add( Line2[1] );
							}
						}
						else
						{// 两个生成线段的延长线相交，则生成节点在原始线段夹角外。
							//	根据生成节点情况及节点处理方式进行修正。
							// 插入交点
							
							lstPtsParallel.Add( Pt );
							lstPtsParallel.Add( Line2[0] );	
							lstPtsParallel.Add( Line2[1] );
							if( i == iNum )
							{// 封闭曲线的处理。
								lstPtsParallel.RemoveAt( lstPtsParallel.Count-1 );
								
							}
						}
					}
					dSinA = dSinB;		dCosA = dCosB;
				}
			}
			catch(Exception ex)
			{
				Debug.Write(ex.Message + ex.TargetSite);
				
			}
			return true;
		}

		/// <summary>
		/// 平行线算法,对于连接点偏移采用两个方向偏移,中间按照偏移距离插值
		/// </summary>
		/// <param name="lstPts">原始曲线点数组</param>
		/// <param name="fOffset">平行偏移量</param>
		/// <param name="lstPtsParallel">偏移后的曲线点数组</param>
		/// <returns>处理成功,返回true,否则返回false</returns>
		public  static  bool Parall_Mix(IList lstPts,float fOffset, IList lstPtsParallel)
		{
			try
			{
				int iNum = lstPts.Count;
				CPointD[] ptOrg = new CPointD[iNum];
				int i = 1;
				int j = 0;
				int nLine = 0;
				CPointD Pt = new CPointD(0,0);

				for(i =0; i < iNum; i++)
				{
					ptOrg[i] = ( CPointD )lstPts[i];
				}
				CPointD[] Line1 = new CPointD[2];
				CPointD[] Line2 = new CPointD[2];

				bool bClose = false;
				//计算出第一段平行线段。
				double dSinA = 0,dCosA = 0;
				double dSinB = 0,dCosB = 0;
				double dSinX = 0,dCosX = 0;
				i = 1;
				while( (false == _CalcSinCos(ptOrg[i-1], ptOrg[i], out dSinA,out dCosA)) && (i < iNum) ) i++;
				Line2[0] = _OffsetPnt(ptOrg[i-1], fOffset, dCosA, -dSinA);
				lstPtsParallel.Add( Line2[0] );
				Line2[1] = _OffsetPnt(ptOrg[ i ], fOffset, dCosA, -dSinA);
				lstPtsParallel.Add( Line2[1] );
				int nPoint = i + 1;
				if( ptOrg[0].IsEquals( ptOrg[iNum-1] ) ) 
				{
					bClose = true;
				}
				int nCondition = 0;
				if(bClose == true)
					nCondition = iNum+1 ;
				else
					nCondition = iNum;
				for(i = nPoint; i < nCondition; i++)
				{
					if( i != iNum)
						j = i;
					else
						j = nPoint-1;
					if( !_CalcSinCos(ptOrg[j-1], ptOrg[j], out dSinB,out  dCosB) ) continue;
					Line2[0] = _OffsetPnt(ptOrg[j-1], fOffset, dCosB, -dSinB);
					Line2[1] = _OffsetPnt(ptOrg[ j ], fOffset, dCosB, -dSinB);
					//计算两个线段法线键底的夹角 dJudge。
					dSinX = dCosA * dSinB - dSinA * dCosB;
					dCosX = dSinA * dSinB + dCosA * dCosB;
					double dJudge = 0.0;
					if( jGMA.DoubleEquals(dCosX,0 ) == true ) dJudge = Math.PI / 2.0;
					else dJudge = Math.Atan(Math.Abs(dSinX/dCosX));
					if( dCosX < 0.0 ) dJudge = Math.PI - dJudge;
					if( dSinX < 0.0 ) dJudge *= -1.0;

					nLine = lstPtsParallel.Count - 1;
					Line1[0] =(CPointD) lstPtsParallel[nLine-1];
					Line1[1] = (CPointD) lstPtsParallel[nLine];
				{//	原始曲线中两条线段是弯曲折线。
					double dT1 =0.0;
					double dT2 = 0.0;
					Pt = jGMA.CP_Sect2Sect(Line1[0], Line1[1],Line2[0], Line2[1],ref dT1,ref dT2 );
					while( (dT1 < 0.0) && (nLine > 1) )
					{// 消除内侧平行线出现的三角效应。
						if(nLine < lstPtsParallel.Count)
						{
							lstPtsParallel.RemoveAt( nLine );	
							nLine--;
						}
						Line1[0] = (CPointD)lstPtsParallel[nLine-1];
						Line1[1] = (CPointD)lstPtsParallel[ nLine ];
						Pt = jGMA.CP_Sect2Sect(Line1[0], Line1[1],Line2[0], Line2[1],ref dT1,ref dT2 );
					}
					if( (0.0 <= dT1) && (dT1 <= 1.0 ) )
					{//	两个生成线段相交，则生成节点在原始线段夹角内。
						if( i == iNum )
						{// 封闭曲线的处理。
							lstPtsParallel.RemoveAt(0);
							lstPtsParallel.Insert(0,Pt);
							lstPtsParallel.RemoveAt(lstPtsParallel.Count - 1);
							lstPtsParallel.Add(Pt);
						}
						else
						{
							lstPtsParallel.RemoveAt(lstPtsParallel.Count - 1);
							lstPtsParallel.Add(Pt);
							lstPtsParallel.Add( Line2[1] );
						}
					}
					else
					{// 两个生成线段的延长线相交，则生成节点在原始线段夹角外。
						//	根据生成节点情况及节点处理方式进行修正。
						// 附加点处理。
						int nPt = (int )(6.0*( Math.Abs(dJudge) )/ Math.PI ) + 1;	//附加点数目。
						for(int k = 1; k < nPt; k++)
						{
							Pt = new CPointD(0,0);
							float  dCosY = (float) Math.Cos( k * dJudge / nPt );			
							float dSinY = (float ) Math.Sin( k * dJudge / nPt );		
							dCosX = -dSinA * dCosY - dCosA * dSinY;	
							dSinX =  dCosA * dCosY - dSinA * dSinY;	
							Pt.X = ptOrg[j-1].X + fOffset * dCosX;
							Pt.Y = ptOrg[j-1].Y + fOffset * dSinX;
							lstPtsParallel.Add( Pt );	
						}
						lstPtsParallel.Add( Line2[0] );	
						lstPtsParallel.Add( Line2[1] );
						if( i == iNum )
						{// 封闭曲线的处理。
							lstPtsParallel.RemoveAt( lstPtsParallel.Count-1 );
						}
					}
				}
					dSinA = dSinB;		dCosA = dCosB;
				}
			}
			catch(Exception ex)
			{
				Debug.Write(ex.Message + ex.TargetSite);
				
			}
			return true;
		}

		/// <summary>
		/// 平行线算法,对于连接点偏移采用两个方向偏移,直接连接
		/// </summary>
		/// <param name="lstPts">原始曲线点数组</param>
		/// <param name="fOffset">平行偏移量</param>
		/// <param name="lstPtsParallel">偏移后的曲线点数组</param>
		/// <returns>处理成功,返回true,否则返回false</returns>
		public  static  bool Parall_Cut(IList lstPts,float fOffset,IList lstPtsParallel)
		{
			try
			{
				int iNum = lstPts.Count;
				CPointD[] ptOrg = new CPointD[iNum];
				int i = 1;
				int j = 0;
				int nLine = 0;
				CPointD Pt = new CPointD(0,0);

				for(i =0; i < iNum; i++)
				{
					ptOrg[i] = ( CPointD )lstPts[i];
				}
				CPointD[] Line1 = new CPointD[2];
				CPointD[] Line2 = new CPointD[2];

				bool bClose = false;
				//计算出第一段平行线段。
				double dSinA = 0,dCosA = 0;
				double dSinB = 0,dCosB = 0;
				double dSinX = 0,dCosX = 0;
				i = 1;
				while( (false == _CalcSinCos(ptOrg[i-1], ptOrg[i], out dSinA,out dCosA)) && (i < iNum) ) i++;
				Line2[0] = _OffsetPnt(ptOrg[i-1], fOffset, dCosA, -dSinA);
				lstPtsParallel.Add( Line2[0] );
				Line2[1] = _OffsetPnt(ptOrg[ i ], fOffset, dCosA, -dSinA);
				lstPtsParallel.Add( Line2[1] );
				int nPoint = i + 1;
				if( ptOrg[0].IsEquals( ptOrg[iNum-1] ) ) 
				{
					bClose = true;
				}
				int nCondition = 0;
				if(bClose == true)
					nCondition = iNum+1 ;
				else
					nCondition = iNum;
				for(i = nPoint; i < nCondition; i++)
				{
					if( i != iNum)
						j = i;
					else
						j = nPoint-1;
					if( !_CalcSinCos(ptOrg[j-1], ptOrg[j], out dSinB,out  dCosB) ) continue;
					Line2[0] = _OffsetPnt(ptOrg[j-1], fOffset, dCosB, -dSinB);
					Line2[1] = _OffsetPnt(ptOrg[ j ], fOffset, dCosB, -dSinB);
					//计算两个线段法线键底的夹角 dJudge。
					dSinX = dCosA * dSinB - dSinA * dCosB;
					dCosX = dSinA * dSinB + dCosA * dCosB;
					double dJudge = 0.0;
					if( jGMA.DoubleEquals(dCosX,0 ) == true ) dJudge = Math.PI / 2.0;
					else dJudge = Math.Atan(Math.Abs(dSinX/dCosX));
					if( dCosX < 0.0 ) dJudge = Math.PI - dJudge;
					if( dSinX < 0.0 ) dJudge *= -1.0;

					nLine = lstPtsParallel.Count - 1;
					Line1[0] =(CPointD) lstPtsParallel[nLine-1];
					Line1[1] = (CPointD) lstPtsParallel[nLine];
				{//	原始曲线中两条线段是弯曲折线。
					double dT1 =0.0;
					double dT2 = 0.0;
					Pt = jGMA.CP_Sect2Sect(Line1[0], Line1[1],Line2[0], Line2[1],ref dT1,ref dT2 );
					while( (dT1 < 0.0) && (nLine > 1) )
					{// 消除内侧平行线出现的三角效应。
						if(nLine < lstPtsParallel.Count)
						{
							lstPtsParallel.RemoveAt( nLine );	
							nLine--;
						}
						Line1[0] = (CPointD)lstPtsParallel[nLine-1];
						Line1[1] = (CPointD)lstPtsParallel[ nLine ];
						Pt = jGMA.CP_Sect2Sect(Line1[0], Line1[1],Line2[0], Line2[1],ref dT1,ref dT2 );
					}
					if( (0.0 <= dT1) && (dT1 <= 1.0 ) )
					{//	两个生成线段相交，则生成节点在原始线段夹角内。
						if( i == iNum )
						{// 封闭曲线的处理。
							lstPtsParallel.RemoveAt(0);
							lstPtsParallel.Insert(0,Pt);
							lstPtsParallel.RemoveAt(lstPtsParallel.Count - 1);
							lstPtsParallel.Add(Pt);
						}
						else
						{
							lstPtsParallel.RemoveAt(lstPtsParallel.Count - 1);
							lstPtsParallel.Add(Pt);
							lstPtsParallel.Add( Line2[1] );
						}
					}
					else
					{// 两个生成线段的延长线相交，则生成节点在原始线段夹角外。
						//	根据生成节点情况及节点处理方式进行修正。
						// 不处理节点，直接联接两个生成线段。
						lstPtsParallel.Add( Line2[0] );	
						lstPtsParallel.Add( Line2[1] );
						if( i == iNum )
						{// 封闭曲线的处理。
							lstPtsParallel.RemoveAt( lstPtsParallel.Count-1 );
						}
					}
				}
					dSinA = dSinB;		dCosA = dCosB;
				}
			}
			catch(Exception ex)
			{
				Debug.Write(ex.Message + ex.TargetSite);
				
			}
			return true;
		}
		
		/// <summary>
		/// 计算偏移
		/// </summary>
		/// <param name="orgPoint">原始点</param>
		/// <param name="OffsetAngle">偏移角度</param>
		/// <param name="OffsetH">切线防线的偏移量</param>
		/// <param name="OffsetV">法向方向的偏移量</param>
		/// <returns></returns>
		public static CPointD _OffsetPntEx(CPointD orgPoint,double OffsetAngle,double OffsetH,double OffsetV)
		{
			double X0 = orgPoint.X;
			double Y0 = orgPoint.Y;
			double fAngle = OffsetAngle / 180.0 * Math.PI;//将偏移方向角度从度转换为弧度

			//计算沿切线偏移后的坐标
			X0 += OffsetH * Math.Cos(OffsetAngle / 180.0 * Math.PI);
			Y0 += OffsetH * Math.Sin(OffsetAngle / 180.0 * Math.PI);


			//计算法向方向偏移后的坐标
			X0 += OffsetV * Math.Sin(fAngle / 180 * Math.PI);
			Y0 += OffsetV * Math.Cos(fAngle / 180 * Math.PI);

			return new CPointD(X0,Y0);
		}

		#region 内部使用的方法
		/// <summary>
		/// 计算矢量线段的正弦余弦值
		/// </summary>
		/// <param name="pt1">矢量线段的起点坐标</param>
		/// <param name="pt2">矢量线段的终点坐标</param>
		/// <param name="SinX">矢量线段的正弦值</param>
		/// <param name="CosX">矢量线段的余弦值</param>
		/// <returns>正常时返回true，出错时返回false</returns>
		private static bool _CalcSinCos( CPointD pt1, CPointD pt2, out double SinX, out double CosX)
		{
			SinX = CosX = 0.0;
			double	len = jGMA.Distance(pt1, pt2);
			if ( jGMA.DoubleEquals(len,0) == true)
			{//距离为0,即两个点重合,进行处理
				SinX = CosX = 0.0;
				return false;
			}
			SinX = (pt2.Y - pt1.Y) / len;
			CosX = (pt2.X - pt1.X) / len;
			return true;
		}
		/// <summary>
		/// 指定的基准点按给定的方向偏移len的新点的坐标
		/// </summary>
		/// <param name="pt">原始点坐标</param>
		/// <param name="len">偏移距离</param>
		/// <param name="SinX">偏移方向的正弦值</param>
		/// <param name="CosX">偏移方向的余弦值</param>
		/// <returns>偏移后的新坐标</returns>
		/// 备注:算法:设直线的基准点坐标为p(x0, y0)，直线的倾角为
		///		  alpha，插入点与基准点的距离为len，则：
		///			x = x0 + cos(alpha) * len;
		///			y = y0 + sin(alpha) * len;
		/// 
		private static CPointD _OffsetPnt(CPointD pt, double len, double SinX, double CosX)
		{
			CPointD	tmpPt = new CPointD(0.0,0.0);
	
			double dX = len * CosX;
			double dY = len * SinX;
			tmpPt.X = pt.X + dX;
			tmpPt.Y = pt.Y + dY;
			return tmpPt;
		}

		#endregion 

	}
}
