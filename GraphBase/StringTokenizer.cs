// ============================================================================
// 类名：StringTokenizer
// 说明：字符串解析类
// 附注：可以读取文本文件及行字符串，并进行解析
// 修订：徐景周
// 日期：2006.05.15
// 用法：
//		string input = "hello \"cool programmer\", your number: 3.45!";
//
//		StringTokenizer strTok	= new StringTokenizer( input );
//		// 指定特殊字符
//		strTok.SymbolChars		= new char[]{ ',' };
//		// 忽略所有空格和指定特殊字符
//		strTok.IgnoreWhiteSpace	= true;
//		strTok.IgnoreSymbol		= true;
//
//		Token token;
//		do
//		{
//		   token = strTok.Next();
//		   Console.WriteLine( token.Kind.ToString() + ": " + token.Value );
//        
//		} while( token.Kind != TokenKind.EOF );
//		
//		输出结果：
//		Word: hello
//		QuotedString: "cool programmer"
//		Word: your
//		Word: number
//		Unknown: :
//		Number: 3.45
//		Unknown: !
//
// =============================================================================
using System;
using System.IO;
using System.Text;

namespace Jurassic.Graph.Base
{
	/// <summary>
	/// 字符类型
	/// </summary>
	public enum TokenKind
	{
		/// <summary>
		/// 未知
		/// </summary>
		Unknown,			
		/// <summary>
		/// 单词
		/// </summary>
		Word,			
		/// <summary>
		/// 数字
		/// </summary>
		Number,		
		/// <summary>
		/// 双引号
		/// </summary>
		QuotedString,		
		/// <summary>
		/// 空格或Tab
		/// </summary>
		WhiteSpace,			
		/// <summary>
		/// 特殊符号
		/// </summary>
		Symbol,			
		/// <summary>
		/// 行结束
		/// </summary>
		EOL,				
		/// <summary>
		/// 文件结束
		/// </summary>
		EOF					
	}

	/// <summary>
	/// 字符标记
	/// </summary>
	public class Token
	{
		#region 成员变量

		private int			line;
		private int			column;
		private string		value;
		private TokenKind	kind;

		#endregion

		#region 属性

		/// <summary>
		/// 列
		/// </summary>
		public int Column
		{
			get 
			{ 
				return this.column; 
			}
		}

		/// <summary>
		/// 字符类型
		/// </summary>
		public TokenKind Kind
		{
			get 
			{ 
				return this.kind; 
			}
		}

		/// <summary>
		/// 行
		/// </summary>
		public int Line
		{
			get 
			{ 
				return this.line; 
			}
		}

		/// <summary>
		/// 值
		/// </summary>
		public string Value
		{
			get 
			{ 
				return this.value; 
			}
		}

		#endregion

		#region 涵数

		/// <summary>
		/// 构造
		/// </summary>
		/// <param name="kind"></param>
		/// <param name="value"></param>
		/// <param name="line"></param>
		/// <param name="column"></param>
		public Token(TokenKind kind, string value, int line, int column)
		{
			this.kind	= kind;
			this.value	= value;
			this.line	= line;
			this.column = column;
		}

		#endregion

	}

	/// <summary>
	/// StringTokenizer解析字符串或流到标记。
	/// </summary>
	public class StringTokenizer
	{
		#region 成员变量

		const char EOF = (char)0;

		int		line;
		int		column;
		int		pos;					// 数据(data)中位置

		string	data;

		bool	ignoreWhiteSpace;		// 忽略空格和Tab值
		bool	ignoreSymbol;			// 忽略指定符号(如果没指定，使用默认字符symbolChars)
		char[]	symbolChars;

		int		saveLine;
		int		saveCol;
		int		savePos;

		#endregion

		#region 属性

		/// <summary>
		/// 获取和设置指定特殊符号TokenKind.Symbol
		/// </summary>
		public char[] SymbolChars
		{
			get 
			{ 
				return this.symbolChars; 
			}
			set 
			{ 
				this.symbolChars = value; 
			}
		}

		/// <summary>
		/// 为真，空格和Tab将被忽略，但字符串中EOL和双引号中空格仍将保留。
		/// </summary>
		public bool IgnoreWhiteSpace
		{
			get 
			{ 
				return this.ignoreWhiteSpace; 
			}
			set 
			{ 
				this.ignoreWhiteSpace = value; 
			}
		}

		/// <summary>
		/// 为真，指定特殊符号将被忽略，但字符串中EOL和双引号中特殊字符仍将保留。
		/// </summary>
		public bool IgnoreSymbol
		{
			get 
			{ 
				return this.ignoreSymbol; 
			}
			set 
			{ 
				this.ignoreSymbol = value; 
			}
		}

		#endregion

		#region 涵数

		/// <summary>
		/// 构造
		/// </summary>
		/// <param name="reader"></param>
		public StringTokenizer(TextReader reader)
		{
			if( reader == null )
				throw new ArgumentNullException( "reader" );

			data = reader.ReadToEnd();

			// 初始化
			Reset();
		}

		/// <summary>
		/// 构造
		/// </summary>
		/// <param name="data"></param>
		public StringTokenizer(string data)
		{
			if( data == null )
				throw new ArgumentNullException( "data" );

			this.data = data;

			// 初始化
			Reset();
		}

		/// <summary>
		/// 初始化
		/// </summary>
		private void Reset()
		{
			// 初始不忽略空白字符
			this.ignoreWhiteSpace	= false;
			// 初始不忽略特殊字符
			this.ignoreSymbol		= false;
			// 默认特殊字符
			this.symbolChars		= new char[]{'=', '+', '/', ',', '.', '*', '~', '!', '@', '#', '$', '%', '^', '&', '(', ')', '{', '}', '[', ']', ':', ';', '<', '>', '?', '|', '\\'};

			// 字符所在行、列及位置
			line	= 1;
			column	= 1;
			pos		= 0;
		}

		/// <summary>
		/// 获取当前位置偏移后的字符
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		protected char LA(int count)
		{
			if( pos + count >= data.Length )
				return EOF;
			else
				return data[pos+count];
		}

		/// <summary>
		/// 跳过
		/// </summary>
		/// <returns></returns>
		protected char Consume()
		{
			char ret = data[pos];
			pos++;
			column++;

			return ret;
		}

		/// <summary>
		/// 创建Token
		/// </summary>
		/// <param name="kind"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected Token CreateToken(TokenKind kind, string value)
		{
			return new Token( kind, value, line, column );
		}

		/// <summary>
		/// 创建Token
		/// </summary>
		/// <param name="kind"></param>
		/// <returns></returns>
		protected Token CreateToken(TokenKind kind)
		{
			string tokenData = data.Substring( savePos, pos - savePos );

			return new Token( kind, tokenData, saveLine, saveCol );
		}

		/// <summary>
		/// 下一个Token
		/// </summary>
		/// <returns></returns>
		public Token Next()
		{
			ReadToken:							// 标记位

			char ch = LA(0);
			switch( ch )
			{
				case EOF:
					return CreateToken( TokenKind.EOF, string.Empty );

				case ' ':
				case '\t':
				{
					if( this.ignoreWhiteSpace )
					{
						Consume();				// 跳过字符，并从新开始
						goto ReadToken;
					}
					else
						return ReadWhitespace();
				}

				case '-':						// 负数
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return ReadNumber();

				case '\r':
				{
					StartRead();
					Consume();
					if( LA(0) == '\n' )
						Consume();			// 在DOS/Windows下用 \r\n定义新行

					line++;
					column=1;

					return CreateToken( TokenKind.EOL );
				}

				case '\n':
				{
					StartRead();
					Consume();
					line++;
					column=1;
					
					return CreateToken( TokenKind.EOL );
				}

				case '"':
				{
					return ReadString();
				}

				default:
				{
					if( Char.IsLetter(ch) || ch == '_' )
						return ReadWord();
					else if( IsSymbol(ch) )
					{
						if( this.ignoreSymbol )
						{
							Consume();				// 跳过指定特殊符号，并从新开始
							goto ReadToken;
						}
						else
						{
							StartRead();			// 读取特殊符号
							Consume();
							return CreateToken( TokenKind.Symbol );
						}
					}
					else
					{
						StartRead();
						Consume();
						return CreateToken( TokenKind.Unknown );						
					}
				}

			}
		}

		/// <summary>
		/// 保存读取点位置以便CreateToken能使用它们
		/// </summary>
		private void StartRead()
		{
			saveLine	= line;
			saveCol		= column;
			savePos		= pos;
		}

		/// <summary>
		/// 读所有空白 (不包括新行)
		/// </summary>
		/// <returns></returns>
		protected Token ReadWhitespace()
		{
			StartRead();

			Consume();		// consume the looked-ahead whitespace char

			while( true )
			{
				char ch = LA(0);
				if( ch == '\t' || ch == ' ' )
					Consume();
				else
					break;
			}

			return CreateToken( TokenKind.WhiteSpace );
			
		}

		/// <summary>
		/// 读取数字，数字可以是: DIGIT + ("." DIGIT*) + ("-" DIGIT*)
		/// </summary>
		/// <returns></returns>
		protected Token ReadNumber()
		{
			StartRead();

			bool hadDot		= false;
			bool hadMinus	= false;

			Consume();								// 读第一个字符

			while( true )
			{
				char ch = LA(0);
				if( Char.IsDigit( ch ) )			// 数字
					Consume();
				else if( ch == '.' && !hadDot )		// 小数点
				{
					hadDot = true;
					Consume();
				}
				else if( ch == '-' && !hadMinus )	// 负号
				{
					hadMinus = true;
					Consume();
				}
				else
					break;
			}

			return CreateToken( TokenKind.Number );
		}

		/// <summary>
		/// reads word. Word contains any alpha character or _
		/// </summary>
		protected Token ReadWord()
		{
			StartRead();

			Consume();		// consume first character of the word

			while( true )
			{
				char ch = LA(0);
				if( Char.IsLetter(ch) || ch == '_' )
					Consume();
				else
					break;
			}

			return CreateToken( TokenKind.Word );
		}

		/// <summary>
		/// reads all characters until next " is found.
		/// If "" (2 quotes) are found, then they are consumed as
		/// part of the string
		/// </summary>
		/// <returns></returns>
		protected Token ReadString()
		{
			StartRead();

			Consume(); // read "

			while( true )
			{
				char ch = LA(0);
				if( ch == EOF )
					break;
				else if( ch == '\r' )	// 处理换行
				{
					Consume();
					if( LA(0) == '\n' )	// 针对DOS & windows
						Consume();

					line++;
					column = 1;
				}
				else if( ch == '\n' )	// new line in quoted string
				{
					Consume();

					line++;
					column = 1;
				}
				else if( ch == '"' )
				{
					Consume();
					if( LA(0) != '"' )
						break;			// done reading, and this quotes does not have escape character
					else
						Consume();		// consume second ", because first was just an escape
				}
				else
					Consume();
			}

			return CreateToken( TokenKind.QuotedString );
		}

		/// <summary>
		/// 检查字符c是否在指定特殊字符串中.
		/// </summary>
		protected bool IsSymbol(char c)
		{
			for( int i = 0; i < symbolChars.Length; i++ )
				if( symbolChars[i] == c )
					return true;

			return false;
		}

		#endregion
	}
}
