// ============================================================================
// 类名：CMatrix3
// 说明：CMatrix3矩阵类，实现+、-、*、/、平移、缩放、旋转等基本操作.
// 附注：
// 修订：徐景周
// 日期：2005.9.28
// =============================================================================

using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// CMatrix3矩阵类，实现+、-、*、/、平移、缩放、旋转等基本操作。
	/// </summary>
	/// <example>
	///	<code>
	///	对于简单3X3矩阵用法如下：
	///		CMatrix3 tempMtx = new CMatrix3();
	///		tempMtx.Translate( 10, 10 );
	///	//	tempMtx.Scale( 1, 2 );
	///	//	tempMtx.Rotate( 45.0f );
	///	//	tempMtx.Shear( 2, 3 );
	///		Console.WriteLine(tempMtx);
	///		
	///	对于其它NXN矩阵用法如下：
	///		int rows, columns; 
	///		rows =  columns = 4;
	///		CMatrix3 tempMtx = new CMatrix3(rows, columns);
	///		tempMtx.Rotate( 45.0f )
	///		Console.WriteLine(tempMtx);
	///		
	///	矩阵内部元素改变如下：
	///		for(int i = 0; i 〈 tempMtx.Rows; i++)
	///		{
	///			for(int j = 0; j 〈 tempMtx.Columns; j++)
	///			{
	///				Console.Write("Matrix[{0}][{1}] : ", i, j);
	///				double input = double.Parse(Console.ReadLine());
	///				tempMtx[i, j] = input;
	///			}
	///		}
	///		
	///	矩阵简单运算如下：
	///		CMatrix3 mtx1 = new CMatrix3();
	///		CMatrix3 mtx2 = new CMatrix3();
	///		CMatrix3 resultMtx;
	///		
	///		try
	///		{
	///			// 加
	///			resultMtx = mtx1 + mtx2;
	///			Console.WriteLine(resultMtx);
	///			// 减
	///			resultMtx = mtx1 - mtx2;
	///			Console.WriteLine(resultMtx);
	///			// 乘
	///			resultMtx = mtx1 * mtx2;
	///			Console.WriteLine(resultMtx);
	///			// 常量乘
	///			double constVal = double.Parse(Console.ReadLine());
	///			resultMtx = constVal * mtx1;
	///			Console.WriteLine(resultMtx);
	///			// 除
	///			double constVal = double.Parse(Console.ReadLine());
	///			resultMtx = mtx1 / constVal;
	///			Console.WriteLine(resultMtx);
	///			// 幂
	///			int power = int.Parse(Console.ReadLine());
	///			resultMtx = mtx ^ power;
	///			Console.WriteLine(resultMtx);
	///			// 转置矩阵(transpose)
	///			resultMtx = ~mtx1
	///			Console.WriteLine(resultMtx);
	///			// 反转矩阵(inverse)
	///			resultMtx = !mtx1;
	///			Console.WriteLine(resultMtx);
	///			
	///		}
	///		catch( CMatrixException ex )
	///		{
	///			Console.WriteLine();
	///			Console.WriteLine(ex.Message);
	///			Console.WriteLine();
	///		}
	///	</code>
	/// </example>
	public class CMatrix3
	{
		#region 成员

		private int			m_rows;					// 行数
		private int			m_columns;				// 列数

		private double[,]	m_matrix;				// 矩阵二维数组

		#endregion

		#region 属性
		
		/// <summary>
		///  矩阵元素值
		/// </summary>
		public double this[int row, int column]
		{
			get
			{
				return m_matrix[row,column];
			}
			set
			{
				m_matrix[row,column] = value;
			}
		}

		/// <summary>
		///  矩阵行数
		/// </summary>
		public int Rows
		{
			get
			{
				return m_rows;
			}
		}

		/// <summary>
		///  矩阵列数
		/// </summary>
		public int Columns
		{
			get
			{
				return m_columns;
			}
		}

		#endregion

		#region 重载涵数

		/// <summary>
		/// 转换为字符串
		/// </summary>
		public override string ToString()
		{
			int maxSpace = 0;
			
			// 寻找矩阵中最长成员的长度
			for(int i = 0; i < m_rows; i++)
			{
				for(int j = 0; j < m_columns; j++)
				{
					int currentLen = this[i,j].ToString().Length;

					if(maxSpace < currentLen)
					{
						maxSpace = currentLen;
					}
				}
			}

			// 最大空白为成员最长长度加1
			maxSpace++;

			// 为StringBuilder计算近似值
			StringBuilder sb = new StringBuilder(maxSpace + (m_rows * m_columns));

			for(int i = 0; i < m_rows; i++)
			{
				for(int j = 0; j < m_columns; j++)
				{
					string currentEle = this[i,j].ToString();

					sb.Append(' ', maxSpace - currentEle.Length);
					sb.Append(currentEle);
				}

				sb.Append("\n");
			}

			return sb.ToString();
		}

		/// <summary>
		/// 获取散列码
		/// </summary>
		public override int GetHashCode()
		{
			double result = 0;

			for(int i = 0; i < m_rows; i++)
			{
				for(int j = 0; j < m_columns; j++)
				{
					result += this[i,j];
				}
			}

			return (int)result;
		}

		/// <summary>
		/// 两矩阵是否相等
		/// </summary>
		public override bool Equals(Object obj)
		{
			CMatrix3 mtx = (CMatrix3)obj;

			if(this.Rows != mtx.Rows || this.Columns != mtx.Columns)
				return false;

			for(int i = 0; i < this.Rows; i++)
			{
				for(int j = 0; j < this.Columns; j++)
				{
					if(this[i,j] != mtx[i,j])
						return false;
				}
			}

			return true;
		}

		#endregion

		#region 重载操作符
 
		/// <summary>
		/// 相等
		/// </summary>
		public static bool operator == (CMatrix3 lmtx, CMatrix3 rmtx)
		{
			return Equals(lmtx, rmtx);
		}

		/// <summary>
		/// 不等
		/// </summary>
		public static bool operator != (CMatrix3 lmtx, CMatrix3 rmtx)
		{
			return !(lmtx == rmtx);
		}

		/// <summary>
		/// 两矩阵相乘
		/// </summary>
		public static CMatrix3 operator * (CMatrix3 lmtx, CMatrix3 rmtx)
		{

			if(lmtx.Columns != rmtx.Rows)
				throw new CMatrixException("Attempt to multiply matrices with unmatching row and column indexes");
			//return null;

			CMatrix3 result = new CMatrix3(lmtx.Rows,rmtx.Columns);

			for(int i = 0; i < lmtx.Rows; i++)
			{
				for(int j = 0; j < rmtx.Columns; j++)
				{
					for(int k = 0; k < rmtx.Rows; k++)
					{
						result[i,j] += lmtx[i,k] * rmtx[k,j];
					}
				}
			}

			return result;
		}

		/// <summary>
		/// 矩阵和常量相乘
		/// </summary>
		public static CMatrix3 operator * (CMatrix3 mtx, double val)
		{

			CMatrix3 result = new CMatrix3(mtx.Rows,mtx.Columns);

			for(int i = 0; i < mtx.Rows; i++)
			{
				for(int j = 0; j < mtx.Columns; j++)
				{
					result[i,j] = mtx[i,j] * val;
				}
			}

			return result;
		}

		/// <summary>
		/// 常量和矩阵相乘
		/// </summary>
		public static CMatrix3 operator * (double val, CMatrix3 mtx)
		{
			return mtx * val;
		}

		/// <summary>
		/// 矩阵除常量
		/// </summary>
		public static CMatrix3 operator / (CMatrix3 mtx, double val)
		{

			if(val == 0)
				throw new CMatrixException("Attempt to devide the matrix by zero");
			//return null;

			CMatrix3 result = new CMatrix3(mtx.Rows,mtx.Columns);

			for(int i = 0; i < mtx.Rows; i++)
			{
				for(int j = 0; j < mtx.Columns; j++)
				{
					result[i,j] = mtx[i,j] / val;
				}
			}

			return result;
		}

		/// <summary>
		/// 矩阵的N次幂，其中N为val
		/// </summary>
		public static CMatrix3 operator ^ (CMatrix3 mtx, double val)
		{
			
			if(mtx.Rows != mtx.Columns)
				throw new CMatrixException("Attempt to find the power of a non square matrix");
			//return null;

			CMatrix3 result = mtx;

			for(int i = 0; i < val; i++)
			{
				result = result * mtx;
			}

			return result;
		}

		/// <summary>
		/// 两矩阵相加
		/// </summary>
		public static CMatrix3 operator + (CMatrix3 lmtx, CMatrix3 rmtx)
		{

			if(lmtx.Rows != rmtx.Rows || lmtx.Columns != rmtx.Columns)
				throw new CMatrixException("Attempt to add matrixes of different sizes");
			//return null;

			CMatrix3 result = new CMatrix3(lmtx.Rows,lmtx.Columns);

			for(int i = 0; i < lmtx.Rows; i++)
			{
				for(int j = 0; j < lmtx.Columns; j++)
				{
					result[i,j] = lmtx[i,j] + rmtx[i,j];
				}
			}

			return result;
		}

		/// <summary>
		///  两矩阵相减
		/// </summary>
		public static CMatrix3 operator - (CMatrix3 lmtx, CMatrix3 rmtx)
		{

			if(lmtx.Rows != rmtx.Rows || lmtx.Columns != rmtx.Columns)
				throw new CMatrixException("Attempt to subtract matrixes of different sizes");
			//return null;

			CMatrix3 result = new CMatrix3(lmtx.Rows,lmtx.Columns);

			for(int i = 0; i < lmtx.Rows; i++)
			{
				for(int j = 0; j < lmtx.Columns; j++)
				{
					result[i,j] = lmtx[i,j] - rmtx[i,j];
				}
			}

			return result;
		}

		/// <summary>
		///  转置矩阵(transpose)
		/// </summary>
		public static CMatrix3 operator ~ (CMatrix3 mtx)
		{

			CMatrix3 result = new CMatrix3(mtx.Columns,mtx.Rows);

			for(int i = 0; i < mtx.Rows; i++)
			{
				for(int j = 0; j < mtx.Columns; j++)
				{
					result[j,i] = mtx[i,j];
				}
			}

			return result;
		}

		/// <summary>
		///  反转矩阵(inverse)
		/// </summary>
		public static CMatrix3 operator ! (CMatrix3 mtx)
		{
			
			if(mtx.Determinant() == 0)
				throw new CMatrixException("Attempt to invert a singular matrix");
			//return null;

			// 2x2矩阵的反转
			if(mtx.Rows == 2 && mtx.Columns == 2)
			{
				CMatrix3 tempMtx = new CMatrix3(2,2);

				tempMtx[0,0] = mtx[1,1];
				tempMtx[0,1] = -mtx[0,1];
				tempMtx[1,0] = -mtx[1,0];
				tempMtx[1,1] = mtx[0,0];

				return tempMtx / mtx.Determinant();
			}

			return mtx.Adjoint()/mtx.Determinant();

		}

		#endregion

		#region 成员涵数

		/// <summary>
		///  默认构造涵数
		/// </summary>
		public CMatrix3()
		{
			// 创建3*3单位矩阵(对角线元素为1，其余为0)
			SetToIdentity();
		}

		/// <summary>
		///  构造函数(传入矩阵行，列数为参数)
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		public CMatrix3(int rows, int columns)
		{

			m_rows		= rows;
			m_columns	= columns;

			m_matrix	= new double[rows,columns];

		}

		/// <summary>
		/// 从Matrix对象创建。
		/// </summary>
		/// <param name="matrix"></param>
		public CMatrix3(Matrix matrix)
		{
			SetToIdentity();
			m_matrix[0,0] = matrix.Elements[0];
			m_matrix[0,1] = matrix.Elements[1];
			m_matrix[1,0] = matrix.Elements[2];
			m_matrix[1,1] = matrix.Elements[3];
			m_matrix[2,0] = matrix.Elements[4];
			m_matrix[2,1] = matrix.Elements[5];
		}

		/// <summary>
		///  初始创建3*3单位矩阵(Identity)
		/// </summary>
		public void SetToIdentity()
		{
			m_rows		= 3;
			m_columns	= 3;

			m_matrix	= new double[3,3];

			for (int  i = 0; i < 3; i++)
			{
				for (int j=0; j<3; j++)
				{
					if(i == j)
					{
						this[i,j] = 1.0f;
					}
					else
					{
						this[i,j] = 0.0f;
					}
				}
			}

		}

		/// <summary>
		/// 一维向量与矩阵相乘，即每一列元素乘相应一维向量值相加之和(3*3矩阵)
		/// [ x y 1] * | Sx Ry 0 | 
		///			   | Rx Sy 0 | = [ Sx*x + Rx*y + Dx, Ry*x + Sy*y + Dy ]
		///			   | Dx Dy 1 |
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public double[] VectorMultiply(double[] vector) 
		{ 
			double[] result = new double[3];
			for (int  i = 0; i < 3; i++)
			{
				for (int  j = 0; j < 3; j++)
				{
					result[i] += vector[j] * this[j,i];
				}
			}

			return result;
		} 

		/// <summary>
		///  切变处理(3*3矩阵)
		/// </summary>
		/// <param name="rx"></param>
		/// <param name="ry"></param>
		public void SetShear(double rx, double ry)
		{
			SetToIdentity();

			this[1,0] = rx;			
			this[0,1] = ry;			
		}

		/// <summary>
		///  沿X轴旋转处理(3*3矩阵)
		/// </summary>
		/// <param name="angle"></param>
		public void SetRotate(double angle)
		{
			SetToIdentity();

			this[0,0] = (double)Math.Cos(	2* Math.PI* angle/360.0f );			
			this[0,1] = (double)Math.Sin(	2* Math.PI* angle/360.0f );			
			this[1,0] = -(double)Math.Sin(	2* Math.PI* angle/360.0f );			
			this[1,1] = (double)Math.Cos(	2* Math.PI* angle/360.0f );			
		}

		/// <summary>
		///  缩放处理(3*3矩阵)
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="sy"></param>
		public void SetScale(double sx, double sy)
		{
			SetToIdentity();

			this[0,0] = sx;
			this[1,1] = sy;
		}

		/// <summary>
		///  平移处理(3*3矩阵)
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		public void SetTranslate(double dx, double dy)
		{
			SetToIdentity();

			this[2,0] = dx;
			this[2,1] = dy;
		}

		/// <summary>
		/// 变换点坐标，公式如下：
		///					| A11 A12 0 |
		///	[ x, y, 1 ] *	| A21 A22 0 | = [ x*A11 + y*A21 + A31, x*A12 + y*A22 + A32, 1 ]
		///					| A31 A32 1 |	
		/// </summary>
		/// <param name="pts">变换点 </param>
		public void TransformPoints(CPointD[] pts)
		{
			this[0,2] = 0;
			this[1,2] = 0;
			this[2,2] = 1;

			for( int nPos = 0; nPos < pts.Length; ++nPos )
			{
				double dbX = pts[nPos].X;
				double dbY = pts[nPos].Y;
				pts[nPos].X = dbX * (double)(this[0,0]) + dbY *  (double)(this[1,0]) +  (double)(this[2,0]);
				pts[nPos].Y = dbX * (double)( this[0,1] )+ dbY * (double)( this[1,1] )+ (double)( this[2,1]);
			}
		}

		/// <summary>
		/// 变换点坐标，公式如下：
		///					| A11 A12 0 |
		///	[ x, y, 1 ] *	| A21 A22 0 | = [ x*A11 + y*A21 + A31, x*A12 + y*A22 + A32, 1 ]
		///					| A31 A32 1 |	
		/// </summary>
		/// <param name="pt">变换点</param>
		public void TransformPoint(CPointD pt)
		{
			this[0,2] = 0;
			this[1,2] = 0;
			this[2,2] = 1;

			//李捷修改  
	//		pt.X = pt.X * (double)(this[0,0]) + pt.Y *  (double)(this[1,0]) +  (double)(this[2,0]);
	//		pt.Y = pt.X * (double)( this[0,1] )+ pt.Y * (double)( this[1,1] )+ (double)( this[2,1]);
			
			double x  = pt.X * (double)(this[0,0]) + pt.Y *  (double)(this[1,0]) +  (double)(this[2,0]);
			pt.Y = pt.X * (double)( this[0,1] )+ pt.Y * (double)( this[1,1] )+ (double)( this[2,1]);
			pt.X = x;
		}

		/// <summary>
		///  determinent
		/// </summary>
		/// <returns></returns>
		private double Determinant()
		{

			double determinent = 0;

			if(this.Rows != this.Columns)
				throw new CMatrixException("Attempt to find the determinent of a non square matrix");
				//return 0;

			// get the determinent of a 2x2 matrix
			if(this.Rows == 2 && this.Columns == 2)
			{
				determinent = (this[0,0] * this[1,1]) - (this[0,1] * this[1,0]);   
				return determinent;
			}

			CMatrix3 tempMtx = new CMatrix3(this.Rows - 1, this.Columns - 1);
 
			// find the determinent with respect to the first row
			for(int j = 0; j < this.Columns; j++)
			{
				
				tempMtx = this.Minor(0, j);

				// recursively add the determinents
				determinent += (int)Math.Pow(-1, j) * this[0,j] * tempMtx.Determinant();
				
			}

			return determinent;
		}

		/// <summary>
		///  伴随矩阵
		/// </summary>
		/// <returns></returns>
		private CMatrix3 Adjoint()
		{

			if(this.Rows < 2 || this.Columns < 2)
				throw new CMatrixException("Adjoint matrix not available");

			CMatrix3 tempMtx = new CMatrix3(this.Rows-1 , this.Columns-1);
			CMatrix3 adjMtx = new CMatrix3 (this.Columns , this.Rows);

			for(int i = 0; i < this.Rows; i++)
			{
				for(int j = 0; j < this.Columns;j++)
				{

					tempMtx = this.Minor(i, j);

					// put the determinent of the minor in the transposed position
					adjMtx[j,i] = (int)Math.Pow(-1,i+j) * tempMtx.Determinant();
				}
			}

			return adjMtx;
		}

		/// <summary>
		///  returns a minor of a matrix with respect to an element
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private CMatrix3 Minor(int row, int column)
		{

			if(this.Rows < 2 || this.Columns < 2)
				throw new CMatrixException("Minor not available");

			int i, j = 0;

			CMatrix3 minorMtx = new CMatrix3(this.Rows - 1, this.Columns - 1);
 
			// find the minor with respect to the first element
			for(int k = 0; k < minorMtx.Rows; k++)
			{

				if(k >= row)
					i = k + 1;
				else
					i = k;

				for(int l = 0; l < minorMtx.Columns; l++)
				{
					if(l >= column)
						j = l + 1;
					else
						j = l;

					minorMtx[k,l] = this[i,j];
				}

			}

			return minorMtx;
		}

		/// <summary>
		///  矩阵是否为单位矩阵
		/// </summary>
		/// <returns></returns>
		public bool IsIdentity()
		{

			if(Rows != Columns)
				return false;

			for(int i = 0; i < Rows; i++)
			{
				for(int j = 0; j < Columns; j++)
				{
					if(i == j)
					{
						if(this[i,j] != 1.0f) 
							return false;
					}
					else
					{
						if(this[i,j] != 0.0f) 
							return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		///  矩阵是否是可逆转的
		/// </summary>
		/// <returns></returns>
		public bool IsInvertible()
		{

			if(this.Determinant() == 0)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		///  复位3*3单位矩阵(Identity)
		/// </summary>
		public void Reset()
		{
			if(m_rows != m_columns)
				throw new CMatrixException("Attempt to make non square matrix identity");

			for(int j = 0; j < 3; j++)
			{
				for(int i = 0; i < 3; i++)
				{
					if(i == j)
					{
						this[i,j] = 1.0f;
					}
					else
					{
						this[i,j] = 0.0f;
					}
				}
			}
		}

		/// <summary>
		///  3*3矩阵元素清0
		/// </summary>
		public void Clear()
		{
			for(int j = 0; j < 3; j++)
			{
				for(int i = 0; i < 3; i++)
				{
					this[i,j] = 0.0f;
				}
			}
		}

		// ====================================================================================
		// ahr	2006-03-23 增加逆矩阵，和Matrix转换
		// ====================================================================================
		/// <summary>
		/// 如果此 CMatrix3 对象是可逆转的，则逆转该对象。
		/// </summary>
		public void Invert()
		{
			CMatrix3 adjoint = Adjoint();
			double determinant = Determinant();
			if ( 0 == determinant )		// 判断是否可逆
				return;
			for(int j = 0; j < 3; j++)
			{
				for(int i = 0; i < 3; i++)
				{
					this[i,j] =adjoint[i,j]/determinant;
				}
			}

		}

		/// <summary>
		/// 返回一个GDI+的Matrix对象。
		/// </summary>
		/// <returns>GDI+的Matrix对象</returns>
		public System.Drawing.Drawing2D.Matrix ToMatrix()
		{
			return new System.Drawing.Drawing2D.Matrix(
				(float)this[0,0], (float)this[0,1],
				(float)this[1,0], (float)this[1,1],
				(float)this[2,0], (float)this[2,1]);
		}

		/// <summary>
		/// 将指定 System.Drawing.Drawing2D.Matrix 转换为 CMatrix3
		/// </summary>
		/// <param name="matrix">要转换的 System.Drawing.Drawing2D.Matrix 。</param>
		/// <returns></returns>
		public static implicit operator CMatrix3(System.Drawing.Drawing2D.Matrix matrix)
		{
			CMatrix3 matrix3 = new CMatrix3();
			for(int j = 0; j < 3; j++)
			{
				for(int i = 0; i < 2; i++)
				{
					matrix3[j,i] =  matrix.Elements[j*2+i];
				}
			}
			return matrix3;
		}

		/// <summary>
		/// 从一个矩阵拷贝数据到自身
		/// </summary>
		/// <param name="matrix">拷贝的源矩阵</param>
		public void CopyFrom(CMatrix3 matrix)
		{
			for(int row = 0; row < 3; row ++)
			{
				for(int col = 0; col < 3; col ++)
				{
					this[row,col] = matrix[row,col];
				}
			}
		}

		/// <summary>
		/// 自身与一个矩阵相乘
		/// </summary>
		/// <param name="matrix">要乘的矩阵</param>
		/// <param name="order"></param>
		public void Multiply(CMatrix3 matrix, MatrixOrder order)
		{
			CMatrix3 multiplyResult = new CMatrix3(3,3);
			if ( MatrixOrder.Append == order )
			{
				for(int i = 0; i < 3; i++)
				{
					for(int j = 0; j < 3; j++)
					{
						for(int k = 0; k < 3; k++)
						{
							multiplyResult[i,j] += this[i,k] * matrix[k,j];
						}
					}
				}
			}
			else
			{
				for(int i = 0; i < 3; i++)
				{
					for(int j = 0; j < 3; j++)
					{
						for(int k = 0; k < 3; k++)
						{
							multiplyResult[i,j] += matrix[i,k] * this[k,j];
						}
					}
				}
			}
			CopyFrom(multiplyResult);		
		}

		/// <summary>
		/// 矩阵平移
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <param name="order"></param>
		public void Translate(double dx, double dy, MatrixOrder order)
		{
			CMatrix3 mtxT = new CMatrix3();
			mtxT.SetTranslate(dx, dy);
			Multiply(mtxT, order);
		}
		/// <summary>
		/// 矩阵旋转
		/// </summary>
		/// <param name="fAngle"></param>
		/// <param name="order"></param>
		public void Rotate(double fAngle, MatrixOrder order)
		{
			CMatrix3 mtxT = new CMatrix3();
			mtxT.SetRotate(fAngle);
			Multiply(mtxT, order);
		}

		/// <summary>
		/// 按指定的顺序将沿指定点的顺时针旋转应用到该 CMatrix3 对象。
		/// </summary>
		/// <param name="angle">旋转角度。</param>
		/// <param name="point">表示旋转中心的 CPointD 对象。</param>
		/// <param name="order">一个 MatrixOrder 枚举，它指定应用旋转的顺序（追加或预先计算）。 </param>
		public void RotateAt(double angle, CPointD point, MatrixOrder order)
		{
			CMatrix3 mtxT = new CMatrix3();
			mtxT.SetTranslate(point.X, point.Y);
			mtxT.Rotate(angle, MatrixOrder.Prepend);
			mtxT.Translate(-point.X, -point.Y, MatrixOrder.Prepend);
			Multiply(mtxT, order);
		}

		/// <summary>
		/// 矩阵缩放
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="sy"></param>
		/// <param name="order"></param>
		public void Scale(double sx, double sy, MatrixOrder order)
		{
			CMatrix3 mtxT = new CMatrix3();
			mtxT.SetScale(sx, sy);
			Multiply(mtxT, order);
		}

		#endregion

	}

	#region 矩阵异常类

	/// <exclude/>
	/// <summary>
	///  矩阵异常类
	/// </summary>
	public class CMatrixException : Exception
	{
		/// <summary>
		/// 矩阵异常构造涵数，无参
		/// </summary>
		public CMatrixException() : base()
		{
		}
	
		/// <summary>
		/// 矩阵异常构造涵数，单参
		/// </summary>
		/// <param name="Message">解释异常原因的错误信息</param>
		public CMatrixException(string Message) : base(Message)
		{
		}
		
		/// <summary>
		/// 矩阵异常构造涵数，双参
		/// </summary>
		/// <param name="Message">解释异常原因的错误信息</param>
		/// <param name="InnerException">导致当前异常的异常。如果 innerException 参数不是空引用（在 Visual Basic 中为 Nothing），则在处理内部异常的 catch 块中引发当前异常。</param>
		public CMatrixException(string Message, Exception InnerException) : base(Message, InnerException)
		{
		}
	}

	#endregion
}
