using System;
namespace ZMath.Algebraic.Tests
{
	public static class SymbolTokens
	{
		public static SymbolToken Addition { get { return BuildToken("+"); } }
		public static SymbolToken Subtraction { get { return BuildToken("-"); } }
		public static SymbolToken Multiplication { get { return BuildToken("*"); } }
		public static SymbolToken Division { get { return BuildToken("/"); } }
		public static SymbolToken Exponentiation { get { return BuildToken("^"); } }
		public static SymbolToken LeftParen { get { return BuildToken("("); } }
		public static SymbolToken RightParen { get { return BuildToken(")"); } }
		public static SymbolToken Sine { get { return BuildToken("sin"); } }
		public static SymbolToken Cosine { get { return BuildToken("cos"); } }
		public static SymbolToken Tangent { get { return BuildToken("tan"); } }

		public static SymbolToken Number(int n)
		{
			return BuildToken(n.ToString());
		}

		public static SymbolToken Number(double n)
		{
			return BuildToken(n.ToString());
		}

		public static SymbolToken BuildToken(string c)
		{
			SymbolToken token;
			SymbolToken.TryParse(c, out token);
			return token;
		}
	}
}
