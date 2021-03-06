using System;
using System.Collections;

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// 曲线平滑插枝算法。
	/// 包括：2次B样条、贝塞尔、抛物线加权、样条插值。
	/// </summary>
	public class jGMCurveSmooth
	{
		/// <summary>
		/// 2次B样条曲线光滑插值算法
		/// </summary>
		/// <param name="lstPts">曲线原始控制点数组列表</param>
		/// <param name="fStep">光滑插值步长</param>
		/// <param name="lstPtsSmooth">插值后的点数组列表</param>
		/// <returns>插值成功返回true,否则返回false</returns>
		public static bool Smooth_TwoBSpline( IList lstPts, double fStep, IList lstPtsSmooth )
		{
			int num_point = lstPts.Count;
			
			int i  =0;
			ArrayList tmpPts = new ArrayList();
			tmpPts.Clear();
			for(i = 0; i < num_point; i++)
			{
				CPointD dp = (CPointD)lstPts[i];
				tmpPts.Add(dp);
			}
			i = 0;
			while (i < num_point - 1)				// 去掉重复点
			{
				if ( ((CPointD)tmpPts[i]).IsEquals ( (CPointD)tmpPts[i + 1]) )  
				{
					tmpPts.RemoveAt( i );
					num_point--;
				}
				else i++;
			}
			if (num_point <= 2) return false;		// 曲线不多于二个点
			
			double x0,y0,x1,y1,x2,y2;
			for(int j=0; j<=(num_point-3);  j ++)
			{
				// 给前三个点赋值
				x0=((CPointD)tmpPts[j]).X;
				y0=((CPointD)tmpPts[j]).Y;
				x1=((CPointD)tmpPts[j + 1]).X;
				y1=((CPointD)tmpPts[j + 1]).Y;
				x2=((CPointD)tmpPts[j + 2]).X;
				y2=((CPointD)tmpPts[j + 2]).Y; 
				if( 0 == j )
				{
					x0=2*x0-x1;
					y0=2*y0-y1;
				}
				if(j==num_point-3)
				{ 
					x2=2*x2-x1;
					y2=2*y2-y1;
				}
				double xs,ys,a1,a2,b1,b2,t,tt,dt;
				xs=(x0+x1)/2.0f;
				ys=(y0+y1)/2.0f;
				a1=x1-x0;
				a2=(x0-2*x1+x2)/2.0f;
				b1=y1-y0;
				b2=(y0-2*y1+y2)/2.0f;
				
				CPointD pf = new CPointD(xs , ys);
				lstPtsSmooth.Add(pf);
				double fLength=0.0;
				fLength += jGMA.Distance((CPointD)tmpPts[j],(CPointD)tmpPts[j + 1]);
				fLength += jGMA.Distance((CPointD)tmpPts[j + 1],(CPointD)tmpPts[j + 2]);
				int nCount = ((int)(fLength / fStep));
				dt=1.0f/nCount;

				for(i=1;i<=nCount;i++)
				{
					CPointD pMid = new CPointD(0,0);
					t=i*dt;
					tt=t*t;
					pMid.X=xs+a1*t+a2*tt;
					pMid.Y=ys+b1*t+b2*tt;
					lstPtsSmooth.Add(pMid);
				}// for
			} // for
			return true;
		}

		/// <summary>
		/// Bezier曲线光滑插值算法
		/// </summary>
		/// <param name="lstPts">曲线原始控制点数组列表</param>
		/// <param name="fStep">光滑插值步长</param>
		/// <param name="lstPtsSmooth">插值后的点数组列表</param>
		/// <returns>插值成功返回true,否则返回false</returns>
		public  static  bool Smooth_Bezier( IList lstPts, float fStep, IList lstPtsSmooth )
		{

			int num_point = lstPts.Count;
			//剔除重复点
			int i  =0;
			ArrayList tmpPts = new ArrayList();
			tmpPts.Clear();
			for(i = 0; i < num_point; i++)
			{
				CPointD dp = (CPointD)lstPts[i];
				tmpPts.Add(dp);
			}
			i = 0;
			while (i < num_point - 1)				// 去掉重复点或相距太近的点
			{
				if ( ((CPointD)tmpPts[i]).IsEquals( (CPointD)tmpPts[i + 1] ) )  
				{
			
					tmpPts.RemoveAt( i );
					num_point--;
				}
				else i++;
			}
			if (num_point <= 2) return false;		// 曲线不多于二个点

			CPointD pt = new CPointD(((CPointD)tmpPts[0]).X,((CPointD)tmpPts[0]).Y);
			lstPtsSmooth.Add(pt);
			//计算总长度
			double  fLength=0.0;
			for(i = 0; i < (num_point -1); i++)
				fLength +=jGMA.Distance(((CPointD)tmpPts[i+1]),((CPointD)tmpPts[i]));

			//计算总插值点数
			long  lCount=0;
			lCount =  ((long)(fLength/fStep + 1));
			for (i=1;i<lCount;i++)
			{
				pt= _Bezier( tmpPts, num_point-1, i*1.0f/lCount	);
				lstPtsSmooth.Add(pt);
			}

			pt.X = ((CPointD)tmpPts[num_point-1]).X;
			pt.Y = ((CPointD)tmpPts[num_point-1]).Y;
	
			lstPtsSmooth.Add(pt);
			return true;
		}

		/// <summary>
		/// 抛物线加权曲线光滑算法
		/// </summary>
		/// <param name="lstPts">曲线原始控制点数组列表</param>
		/// <param name="fStep">光滑插值步长</param>
		/// <param name="lstPtsSmooth">插值后的点数组列表</param>
		/// <returns>插值成功返回true,否则返回false</returns>
		public  static  bool Smooth_Parabola(IList lstPts,float fStep,IList lstPtsSmooth)
		{
			int			i, j, k, num_point = lstPts.Count;
			double		xs, ys, xe, ye, s1, s2, s3, x1, x2, x3, y1, y2, y3;
			double		xs2, xs3, ys2, ys3, ws2, ws3, ds, dt;
			double		aa1 = 0.0, bb1= 0.0, cc1= 0.0, dd1= 0.0, ee1= 0.0, ff1= 0.0, 
				aa2= 0.0, bb2= 0.0, cc2= 0.0, dd2= 0.0, ee2= 0.0, ff2= 0.0;

			ArrayList tmpPts = new ArrayList();
			tmpPts.Clear();
			for(i = 0; i < num_point; i++)
			{
				CPointD dp = (CPointD)lstPts[i];
				tmpPts.Add(dp);
			}
			i = 0;
			while (i < num_point - 1)				// 去掉重复点
			{
				if ( ((CPointD)tmpPts[i]).IsEquals((CPointD)tmpPts[i + 1]) )
				{
			
					tmpPts.RemoveAt( i );
					num_point--;
				}
				else i++;
			}
			if (num_point <= 2) return false;		// 曲线不多于二个点

			if ( ((CPointD)tmpPts[0]).IsEquals((CPointD)tmpPts[num_point -1]) )		// 闭曲线端点进行扩展
			{
				xs = ((CPointD)tmpPts[num_point - 2]).X;
				ys = ((CPointD)tmpPts[num_point - 2]).Y;
				xe = ((CPointD)tmpPts[1]).X; 
				ye = ((CPointD)tmpPts[1]).Y; 
			}
			else				// 开曲线端点进行扩展
			{
				xs = ((CPointD)tmpPts[0]).X - (((CPointD)tmpPts[1]).X - ((CPointD)tmpPts[0]).X);
				ys = ((CPointD)tmpPts[0]).Y - (((CPointD)tmpPts[1]).Y - ((CPointD)tmpPts[0]).Y);
				xe = ((CPointD)tmpPts[num_point-1]).X -(((CPointD)tmpPts[num_point-2]).X - ((CPointD)tmpPts[num_point-1]).X) ;
				ye = ((CPointD)tmpPts[num_point-1]).Y - (((CPointD)tmpPts[num_point-2]).Y - ((CPointD)tmpPts[num_point-1]).Y);
			}
			s1 = 0.0;			// s1, s2, s3为曲线三段的累计弧长
			s2 = s1 + jGMA.Distance(xs, ys, ((CPointD)tmpPts[0]).X, ((CPointD)tmpPts[0]).Y);
			if (jGMA.DoubleEquals(s2,0) == true) s2 = 0;
			lstPtsSmooth.Add( (CPointD)tmpPts[0] );
			for (j = 0; j < num_point; j++)				// 对数据点逐个进行处理
			{
				x1 = (j <= 0) ? xs : ((CPointD)tmpPts[j-1]).X;		// 从曲线上依次取三个点
				x2 = ((CPointD)tmpPts[j]).X;
				x3 = (j >= num_point-1) ? xe : ((CPointD)tmpPts[j+1]).X;
				y1 = (j <= 0) ? ys : ((CPointD)tmpPts[j-1]).Y;
				y2 = ((CPointD)tmpPts[j]).Y;
				y3 = (j >= num_point-1) ? ye : ((CPointD)tmpPts[j+1]).Y;
				ds = jGMA.Distance(x2, y2, x3, y3);			// 求弧长增量
				s3 = s2 + ds;							// 计算累计弧长
				dt = (s3-s2)*(s3-s1)*(s2-s1);			// 求二次曲线系数矩阵行列式的值

				if (dt <= 1e-5)							// 无弧长增量
					continue;

				aa2 = (x1*s2*s3*(s3-s2) - x2*s3*s1*(s3-s1) + x3*s1*s2*(s2-s1)) / dt;
				bb2 = (x1*(s2-s3)*(s2+s3) + x2*(s3-s1)*(s3+s1) + x3*(s1-s2)*(s1+s2)) /dt;
				cc2 = (x1*(s3-s2) + x2*(s1-s3) + x3*(s2-s1)) / dt;
				dd2 = (y1*s2*s3*(s3-s2) - y2*s3*s1*(s3-s1) + y3*s1*s2*(s2-s1)) / dt;
				ee2 = (y1*(s2-s3)*(s2+s3) + y2*(s3-s1)*(s3+s1) + y3*(s1-s2)*(s1+s2)) /dt;
				ff2 = (y1*(s3-s2) + y2*(s1-s3) + y3*(s2-s1)) / dt;
				if (j <= 0)			// 是起点则不绘图
				{
					aa1 = aa2;	bb1 = bb2;	cc1 = cc2;
					dd1 = dd2;	ee1 = ee2;	ff1 = ff2;
				}
				else				// 是后继点则进行绘图输出
				{
					dt = fStep / (s2-s1);
					k = (int)(1.0f  / dt + 0.5f);
					
					for (i = 1, ds = 0.0; i < k; i++) 
					{
						CPointD ptTmp = new CPointD();
						s1 += fStep;
						ds += dt;
						ws2 = (1 - ds) * (1 - ds) * (1 + 2*ds);
						ws3 = ds*ds * (3 - 2*ds);
						xs2 = aa1 + (bb1 + cc1*s1)*s1;
						xs3 = aa2 + (bb2 + cc2*s1)*s1;
						ys2 = dd1 + (ee1 + ff1*s1)*s1;
						ys3 = dd2 + (ee2 + ff2*s1)*s1;
						ptTmp.X = ws2*xs2 + ws3*xs3 ;
						ptTmp.Y = ws2*ys2 + ws3*ys3;
						lstPtsSmooth.Add(ptTmp);
					}
					lstPtsSmooth.Add((CPointD)tmpPts[j]);
					// 保留二次曲线左边的抛物线参数
					aa1 = aa2;	bb1 = bb2;	cc1 = cc2;
					dd1 = dd2;	ee1 = ee2;	ff1 = ff2;
				}
				s1 = s2;	s2 = s3;             // 修改曲线的弧长值
			}
			return true;
		}

		/// <summary>
		/// 样条插值曲线光滑算法
		/// </summary>
		/// <param name="lstPts">曲线原始控制点数组列表</param>
		/// <param name="fStep">光滑插值步长</param>
		/// <param name="lstPtsSmooth">插值后的点数组列表</param>
		/// <returns>插值成功返回true,否则返回false</returns>
		public  static  bool Smooth_Spline(IList lstPts,float fStep,IList lstPtsSmooth)
		{
			int num_point=lstPts.Count;								//数据点的个数
			//剔除重复点
			int i  =0;
			ArrayList tmpPts = new ArrayList();
			tmpPts.Clear();
			for(i = 0; i < num_point; i++)
			{
				CPointD dp = (CPointD)lstPts[i];
				tmpPts.Add(dp);
			}
			i = 0;
			while (i < num_point - 1)				// 去掉重复点或相距太近的点
			{
				if ( ((CPointD)tmpPts[i]).IsEquals((CPointD)tmpPts[i + 1]) )
				{
			
					tmpPts.RemoveAt( i );
					num_point--;
				}
				else i++;
			}
			if (num_point <= 2) return false;		// 曲线不多于二个点




			/////////////////////////////////////////////////////////////////////////////
			double [] xp = new double[num_point+1];
			double [] yp = new double[num_point+1];
			//先纪录三对角线矩阵的右端项，后用于记录三对角线矩阵的未知项
			double [] hp = new double[num_point+1];
			//相邻两点之间的距离
			//累加弦长
			double [] s = new double[num_point+1];
			//纪录三对角线矩阵的左端项
			double [] aa = new double[num_point+1];
			double [] bb = new double[num_point+1];
			double [] cc = new double[num_point+1];

			double sx1,sy1;								//第一点的导数  三角形横与斜的比值(cos)
			double sxn,syn;								//最后一点的导数
			int nc1=0,									//等值线的点数-1
				nc2=0;									//等值线的点数-2
			double	delx1=0,							//等值点2与等值点1的横坐标之差
				dely1=0,								//等值点2与等值点1的纵坐标之差
				dels1=0;								//等值点2与等值点1的距离
			double delx2,dely2,dels2,dx2,dy2;
			double delxn,delyn,delsn,dx1=0,dy1=0;
			int k1,ki,	ic=0;;
			double ss,zs1,zs2,zs3;                                                                      
			/////////////////////////////////////////////////////////////////////////////
			nc1=num_point-1;							//等值线的点数-1
			nc2=num_point-2;							//等值线的点数-2
			delx1=((CPointD)tmpPts[1]).X-((CPointD)tmpPts[0]).X;	//1,2两点的x差值
			dely1=((CPointD)tmpPts[1]).Y-((CPointD)tmpPts[0]).Y;   //1,2两点的y差值
			dels1=	Math.Sqrt(delx1*delx1+dely1*dely1);	
			sx1=delx1/dels1;							
			sy1=dely1/dels1;
			CPointD	ptHead = new CPointD(((CPointD)tmpPts[0]).X,((CPointD)tmpPts[0]).Y);
			CPointD	ptTail = new CPointD(((CPointD)tmpPts[num_point - 1]).X,
				((CPointD)tmpPts[num_point - 1]).Y);

			if( ptHead.IsEquals(ptTail) )		//判断是不是闭合曲线
			{
				sxn=sx1;								//是闭合曲线，首尾点导数相同
				syn=sy1;
			}
			else										//不是闭合曲线，执行以下操作，其实是另外求了一下终点导数
			{
				delxn= (double)(((CPointD)tmpPts[nc1]).X-((CPointD)tmpPts[nc2]).X);
				delyn= (double)(((CPointD)tmpPts[nc1]).Y-((CPointD)tmpPts[nc2]).Y);
				delsn=Math.Sqrt(delxn*delxn+delyn*delyn);
				sxn=delxn/delsn;
				syn=delyn/delsn;
			}

			//三对角线矩阵右端项的赋值过程------------------------->
			dx1=delx1/dels1;
			dy1=dely1/dels1;
			xp[1]=dx1-sx1;
			yp[1]=dy1-sy1;
			hp[1]=dels1;
			s[1]=0;
			s[2]=dels1;
			for(ki=2; ki<=nc1; ki++)					//三对角线矩阵右端项的赋值过程
			{

				delx2 = ((CPointD)tmpPts[ki]).X - ((CPointD)tmpPts[ki-1]).X;
				dely2 = ((CPointD)tmpPts[ki]).Y - ((CPointD)tmpPts[ki-1]).Y;
				dels2= Math.Sqrt(delx2*delx2+dely2*dely2);

				dx2=delx2/dels2;
				dy2=dely2/dels2;

				xp[ki]=dx2-dx1;
				yp[ki]=dy2-dy1;
				hp[ki]=dels2;
				dx1=dx2;
				dy1=dy2;
				ic=ki+1;
				s[ic]=s[ki]+dels2;
			}
			//三对角线矩阵右端项的赋值过程------------------------->

			//三对角线矩阵左边系数项的赋值过程----------------------------------->
			xp[num_point]=sxn-dx1;							//左边系数最后一项
			yp[num_point]=syn-dy1;							//左边系数最后一项
			//张力系数采用计算获得缺省最佳值
			float 	m_fltTensile=(float)(0.1f * nc1 / s[num_point]);				//规范化张力系数
			double dels=0,d1=0,d2=0;
			dels=m_fltTensile*hp[1];
			d1=m_fltTensile*(((Math.Exp(dels)+Math.Exp(-dels))/2)/Math.Sinh(dels))-1/hp[1]; 
			bb[1]=d1;
			cc[1]=1/hp[1]-m_fltTensile/Math.Sinh(dels); 
			aa[1]=cc[1];
			for( ki=2;ki<=nc1;ki++)						//三对角线矩阵左边系数项的赋值过程
			{
				dels=m_fltTensile*hp[ki];
				dely1=((CPointD)tmpPts[1]).Y-((CPointD)tmpPts[0]).Y;
				cc[ki]=1/hp[ki]-m_fltTensile/Math.Sinh(dels);
				aa[ki]=cc[ki];
				d2=m_fltTensile*(((Math.Exp(dels)+Math.Exp(-dels))/2)/Math.Sinh(dels))-1/hp[ki];
				bb[ki]=d1+d2;
				d1=d2;
			}
			//三对角线矩阵左边系数项的赋值过程----------------------------------->
			bb[num_point]=d1;
			xp[1]=xp[1]/bb[1];
			yp[1]=yp[1]/bb[1];
			double w=0;
			w=bb[1];
			int kb =1;
			for(kb=1;kb<=nc1;kb++)
			{
				bb[kb]=cc[kb]/w;
				w=bb[kb+1]-aa[kb]*bb[kb];
				k1=kb+1;
				xp[k1]=(xp[k1]-aa[kb]*xp[kb])/w;
				yp[k1]=(yp[k1]-aa[kb]*yp[kb])/w;
			}
			for(ki=1;ki<=nc1;ki++)
			{
				kb=num_point-ki;
				xp[kb]=xp[kb]-bb[kb]*xp[kb+1];
				yp[kb]=yp[kb]-bb[kb]*yp[kb+1];
			}
			CPointD pf = new CPointD(((CPointD)tmpPts[0]).X,((CPointD)tmpPts[0]).Y);
			lstPtsSmooth.Add(pf);
			for(ki=1;ki<=nc1;ki++)
			{
				
				kb=(int)hp[ki];							//计算二点间插补点数
				for(float ja=0;ja<=hp[ki];ja=ja+fStep)		
				{
					CPointD pMid = new CPointD(0,0);
					ss=s[ki]+ja;
					zs1=m_fltTensile*(s[ki+1]-ss);
					zs2=m_fltTensile*(ss-s[ki]);
					zs3=m_fltTensile*hp[ki];
					pMid.X=(float)((xp[ki]*Math.Sinh(zs1)+xp[ki+1]*Math.Sinh(zs2))/Math.Sinh(zs3)+(((CPointD)tmpPts[ki-1]).X-xp[ki])*(s[ki+1]-ss)/hp[ki]+(((CPointD)tmpPts[ki]).X-xp[ki+1])*(ss-s[ki])/hp[ki]);
					pMid.Y=(float)((yp[ki]*Math.Sinh(zs1)+yp[ki+1]*Math.Sinh(zs2))/Math.Sinh(zs3)+(((CPointD)tmpPts[ki-1]).Y-yp[ki])*(s[ki+1]-ss)/hp[ki]+(((CPointD)tmpPts[ki]).Y-yp[ki+1])*(ss-s[ki])/hp[ki]);

					lstPtsSmooth.Add(pMid);
				}
				lstPtsSmooth.Add((CPointD)tmpPts[ki]);
			}
			return true;
		}


		#region 内部使用的过程
		private static CPointD _Bezier( IList ptList,int n,double mu)
		{
			int k,kn,nn,nkn;
			double blend,muk,munk;
			CPointD b = new CPointD();
	
			muk = 1;
			munk = Math.Pow((1-mu),n);
	
			for (k=0;k<=n;k++) 
			{
				nn = n;
				kn = k;
				nkn = n - k;
				blend = muk * munk;
				muk *= mu;
				munk /= (1-mu);
				while (nn >= 1) 
				{
					blend *= nn;
					nn--;
					if (kn > 1) 
					{
						blend /= (double)kn;
						kn--;
					}
					if (nkn > 1) 
					{
						blend /= (double)nkn;
						nkn--;
					}
				}
				b.X += ( ((CPointD)ptList[k]).X * blend );
				b.Y += ( ((CPointD)ptList[k]).Y * blend );
			}
			return(b);
		}

		#endregion 
	}
}
