////////////////////////////////////////////////////////////////////////
// 类名：CPoint3D
// 功能：三维坐标类
// 附注：图元填充类Fill使用
// 修订：徐景周(jingzhou_xu@163.com)
// 组织：
// 日期：2005.7.28
////////////////////////////////////////////////////////////////////////
using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;
using IComparer	= System.Collections.IComparer;

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// Enumeration type that defines which set of data points - X or Y - is used  
	/// <seealso cref="System.Collections.ArrayList.Sort()"/> to perform the sort.
	/// </summary>
	public enum SortTypes
	{
		/// <summary>
		/// Use the Y values to sort the list.
		/// </summary>
		YValues,
		/// <summary>
		/// Use the X values to sort the list.
		/// </summary>
		XValues
	};

	/// <summary>
	/// A simple point represented by an (X,Y) pair of
	/// double values.
	/// </summary>
	[Serializable]
	public class CPoint3D : ISerializable
	{
	#region Member variables
		/// <summary>
		/// Missing values are represented internally using <see cref="System.Double.MaxValue"/>.
		/// </summary>
		public const double Missing = Double.MaxValue;

		/// <summary>
		/// The default format to be used for displaying point values via the
		/// <see cref="ToString()"/> method.
		/// </summary>
		public const string DefaultFormat = "G";

		/// <summary>
		/// This CPoint3D's X coordinate
		/// </summary>
		public double X;

		/// <summary>
		/// This CPoint3D's Y coordinate
		/// </summary>
		public double Y;
		
		/// <summary>
		/// This CPoint3D's Z coordinate.  Also used for the lower value (dependent axis)
		/// for <see cref="HiLowBarItem"/> charts.
		/// </summary>
		public double Z;

		/// <summary>
		/// A tag object for use by the user.  This can be used to store additional
		/// information associated with the <see cref="CPoint3D"/>.  ZedGraph never
		/// modifies this value, but if it is a <see cref="String"/> type, it
		/// may be displayed in a <see cref="System.Windows.Forms.ToolTip"/>
		/// within the <see cref="ZedGraphControl"/> object.
		/// </summary>
		/// <remarks>
		/// Note that, if you are going to Serialize ZedGraph data, then any type
		/// that you store in <see cref="Tag"/> must be a serializable type (or
		/// it will cause an exception).
		/// </remarks>
		public object Tag;

	#endregion

	#region Constructors

		/// <summary>
		/// Default Constructor
		/// </summary>
		public CPoint3D()
		{
			this.X = 0;
			this.Y = 0;
			this.Z = 0;
			this.Tag = null;
		}

		/// <summary>
		/// Creates a point pair with the specified X and Y.
		/// </summary>
		/// <param name="x">This pair's x coordinate.</param>
		/// <param name="y">This pair's y coordinate.</param>
		public CPoint3D( double x, double y )
		{
			this.X = x;
			this.Y = y;
			this.Z = 0;
			this.Tag = null;
		}

		/// <summary>
		/// Creates a point pair with the specified X, Y, and
		/// label (<see cref="Tag"/>).
		/// </summary>
		/// <param name="x">This pair's x coordinate.</param>
		/// <param name="y">This pair's y coordinate.</param>
		/// <param name="label">This pair's string label (<see cref="Tag"/>)</param>
		public CPoint3D( double x, double y, string label )
		{
			this.X = x;
			this.Y = y;
			this.Z = 0;
			this.Tag = label;
		}

		/// <summary>
		/// Creates a point pair with the specified X, Y, and base value.
		/// </summary>
		/// <param name="x">This pair's x coordinate.</param>
		/// <param name="y">This pair's y coordinate.</param>
		/// <param name="z">This pair's z or lower dependent coordinate.</param>
		public CPoint3D( double x, double y, double z )
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.Tag = null;
		}

		/// <summary>
		/// Creates a point pair with the specified X, Y, base value, and
		/// label (<see cref="Tag"/>).
		/// </summary>
		/// <param name="x">This pair's x coordinate.</param>
		/// <param name="y">This pair's y coordinate.</param>
		/// <param name="z">This pair's z or lower dependent coordinate.</param>
		/// <param name="label">This pair's string label (<see cref="Tag"/>)</param>
		public CPoint3D( double x, double y, double z, string label )
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.Tag = label;
		}

		/// <summary>
		/// Creates a point pair from the specified <see cref="PointF"/> struct.
		/// </summary>
		/// <param name="pt">The <see cref="PointF"/> struct from which to get the
		/// new <see cref="CPoint3D"/> values.</param>
		public CPoint3D( PointF pt )
		{
			this.X = pt.X;
			this.Y = pt.Y;
			this.Z = 0;
			this.Tag = null;
		}

		/// <summary>
		/// The CPoint3D copy constructor.
		/// </summary>
		/// <param name="rhs">The basis for the copy.</param>
		public CPoint3D( CPoint3D rhs )
		{
			this.X = rhs.X;
			this.Y = rhs.Y;
			this.Z = rhs.Z;

			if ( rhs.Tag is ICloneable )
				this.Tag = ((ICloneable) rhs.Tag).Clone();
			else
				this.Tag = rhs.Tag;
		}
	#endregion

	#region Serialization
		/// <summary>
		/// Current schema value that defines the version of the serialized file
		/// </summary>
		public const int schema = 1;

		/// <summary>
		/// Constructor for deserializing objects
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
		/// </param>
		/// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
		/// </param>
		protected CPoint3D( SerializationInfo info, StreamingContext context )
		{
			// The schema value is just a file version parameter.  You can use it to make future versions
			// backwards compatible as new member variables are added to classes
			int sch = info.GetInt32( "schema" );

			X = info.GetDouble( "X" );
			Y = info.GetDouble( "Y" );
			Z = info.GetDouble( "Z" );

			Tag = info.GetValue( "Tag", typeof(object) );
		}
		/// <summary>
		/// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
		/// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public virtual void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( "schema", schema );
			info.AddValue( "X", X );
			info.AddValue( "Y", Y );
			info.AddValue( "Z", Z );
			info.AddValue( "Tag", Tag );
		}
	#endregion

	#region Properties
		/// <summary>
		/// Readonly value that determines if either the X or the Y
		/// coordinate in this CPoint3D is a missing value.
		/// </summary>
		/// <returns>true if either value is missing</returns>
		public bool IsMissing
		{
			get { return this.X == CPoint3D.Missing || this.Y == CPoint3D.Missing; }
		}

		/// <summary>
		/// Readonly value that determines if either the X or the Y
		/// coordinate in this CPoint3D is an invalid (not plotable) value.
		/// It is considered invalid if it is missing (equal to System.Double.Max),
		/// Infinity, or NaN.
		/// </summary>
		/// <returns>true if either value is invalid</returns>
		public bool IsInvalid
		{
			get { return	this.X == CPoint3D.Missing ||
							this.Y == CPoint3D.Missing ||
							Double.IsInfinity( this.X ) ||
							Double.IsInfinity( this.Y ) ||
							Double.IsNaN( this.X ) ||
							Double.IsNaN( this.Y );
				}
		}

		/// <summary>
		/// static method to determine if the specified point value is invalid.
		/// </summary>
		/// <remarks>The value is considered invalid if it is <see cref="CPoint3D.Missing"/>,
		/// <see cref="Double.PositiveInfinity"/>, <see cref="Double.NegativeInfinity"/>
		/// or <see cref="Double.NaN"/>.</remarks>
		/// <param name="value">The value to be checked for validity.</param>
		/// <returns>true if the value is invalid, false otherwise</returns>
		public static bool IsValueInvalid( double value )
		{
			return ( value == CPoint3D.Missing ||
					Double.IsInfinity( value ) ||
					Double.IsNaN( value ) );
		}

		/// <summary>
		/// Readonly value that determines if either the X, Y, or Z
		/// coordinate in this CPoint3D is an invalid (not plotable) value.
		/// It is considered invalid if it is missing (equal to System.Double.Max),
		/// Infinity, or NaN.
		/// </summary>
		/// <returns>true if any value is invalid</returns>
		public bool IsInvalid3D
		{
			get { return	this.X == CPoint3D.Missing ||
							this.Y == CPoint3D.Missing ||
							this.Z == CPoint3D.Missing ||
							Double.IsInfinity( this.X ) ||
							Double.IsInfinity( this.Y ) ||
							Double.IsInfinity( this.Z ) ||
							Double.IsNaN( this.X ) ||
							Double.IsNaN( this.Y ) ||
							Double.IsNaN( this.Z );
				}
		}

		/// <summary>
		/// The "low" value for this point (lower dependent-axis value).
		/// This is really just an alias for <see cref="CPoint3D.Z"/>.
		/// </summary>
		/// <value>The lower dependent value for this <see cref="CPoint3D"/>.</value>
		public double LowValue
		{
			get { return this.Z; }
			set { this.Z = value; }
		}

	#endregion

	#region Operator Overloads
		/// <summary>
		/// Implicit conversion from CPoint3D to PointF.  Note that this conversion
		/// can result in data loss, since the data are being cast from a type
		/// double (64 bit) to a float (32 bit).
		/// </summary>
		/// <param name="pair">The CPoint3D struct on which to operate</param>
		/// <returns>A PointF struct equivalent to the CPoint3D</returns>
		public static implicit operator PointF( CPoint3D pair )
		{
			return new PointF( (float) pair.X, (float) pair.Y );
		}

	#endregion

	#region Inner classes
		/// <summary>
		/// Compares points based on their y values.  Is setup to be used in an
		/// ascending order sort.
		/// <seealso cref="System.Collections.ArrayList.Sort()"/>
		/// </summary>
		public class CPoint3DComparerY : IComparer 
		{
		
			/// <summary>
			/// Compares two <see cref="CPoint3D"/>s.
			/// </summary>
			/// <param name="l">Point to the left.</param>
			/// <param name="r">Point to the right.</param>
			/// <returns>-1, 0, or 1 depending on l.Y's relation to r.Y</returns>
			int IComparer.Compare( object l, object r ) 
			{
				if (l == null && r == null) 
				{
					return 0;
				} 
				else if (l == null && r != null) 
				{
					return -1;
				} 
				else if (l != null && r == null) 
				{
					return 1;
				} 

				double lY = ((CPoint3D) l).Y;
				double rY = ((CPoint3D) r).Y;

				if (System.Math.Abs(lY - rY) < .000000001)
					return 0;
				
				return lY < rY ? -1 : 1;
			}
		}
	
		/// <summary>
		/// Compares points based on their x values.  Is setup to be used in an
		/// ascending order sort.
		/// <seealso cref="System.Collections.ArrayList.Sort()"/>
		/// </summary>
		public class CPoint3DComparer : IComparer 
		{
			private SortTypes sortType;
			
			/// <summary>
			/// Constructor for CPoint3DComparer.
			/// </summary>
			/// <param name="type">The axis type on which to sort.</param>
			public CPoint3DComparer( SortTypes type )
			{
				this.sortType = type;
			}
			
			/// <summary>
			/// Compares two <see cref="CPoint3D"/>s.
			/// </summary>
			/// <param name="l">Point to the left.</param>
			/// <param name="r">Point to the right.</param>
			/// <returns>-1, 0, or 1 depending on l.X's relation to r.X</returns>
			int IComparer.Compare( object l, object r ) 
			{				 
				if ( l == null && r == null ) 
					return 0;
				else if ( l == null && r != null ) 
					return -1;
				else if ( l != null && r == null ) 
					return 1;

				double lVal, rVal;
			
				if ( sortType == SortTypes.XValues )
				{
					lVal = ((CPoint3D) l).X;
					rVal = ((CPoint3D) r).X;
				}
				else
				{
					lVal = ((CPoint3D) l).Y;
					rVal = ((CPoint3D) r).Y;
				}
				
				if ( lVal == CPoint3D.Missing || Double.IsInfinity( lVal ) || Double.IsNaN( lVal ) )
					l = null;
				if ( rVal == CPoint3D.Missing || Double.IsInfinity( rVal ) || Double.IsNaN( rVal ) )
					r = null;

				if ( ( l == null && r == null ) || ( System.Math.Abs( lVal - rVal ) < 1e-10 ) )
					return 0;
				else if ( l == null && r != null ) 
					return -1;
				else if ( l != null && r == null ) 
					return 1;
				else
					return lVal < rVal ? -1 : 1;
			}
		}
	
	#endregion

	#region Methods
		/// <summary>
		/// Format this CPoint3D value using the default format.  Example:  "( 12.345, -16.876 )".
		/// The two double values are formatted with the "g" format type.
		/// </summary>
		/// <returns>A string representation of the CPoint3D</returns>
		public override string ToString()
		{
			return this.ToString( CPoint3D.DefaultFormat, false );
		}

		/// <summary>
		/// Format this CPoint3D value using the default format.  Example:  "( 12.345, -16.876 )".
		/// The two double values are formatted with the "g" format type.
		/// </summary>
		/// <param name="isShowZ">true to show the third "Z" or low dependent value coordinate</param>
		/// <returns>A string representation of the CPoint3D</returns>
		public string ToString( bool isShowZ )
		{
			return this.ToString( CPoint3D.DefaultFormat, isShowZ );
		}

		/// <summary>
		/// Format this CPoint3D value using a general format string.
		/// Example:  a format string of "e2" would give "( 1.23e+001, -1.69e+001 )".
		/// </summary>
		/// <param name="format">A format string that will be used to format each of
		/// the two double type values (see <see cref="System.Double.ToString()"/>).</param>
		/// <returns>A string representation of the CPoint3D</returns>
		public string ToString( string format )
		{
			return this.ToString( format, false );
		}

		/// <summary>
		/// Format this CPoint3D value using a general format string.
		/// Example:  a format string of "e2" would give "( 1.23e+001, -1.69e+001 )".
		/// If <see paramref="isShowZ"/>
		/// is true, then the third "Z" coordinate is also shown.
		/// </summary>
		/// <param name="format">A format string that will be used to format each of
		/// the two double type values (see <see cref="System.Double.ToString()"/>).</param>
		/// <returns>A string representation of the CPoint3D</returns>
		/// <param name="isShowZ">true to show the third "Z" or low dependent value coordinate</param>
		public string ToString( string format, bool isShowZ )
		{
			return "( " + this.X.ToString( format ) +
					", " + this.Y.ToString( format ) + 
					( isShowZ ? ( ", " + this.Z.ToString( format ) ) : "" )
					+ " )";
		}
	#endregion
	}
}
