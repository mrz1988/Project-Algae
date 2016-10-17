using System;
using System.Collections.Generic;

namespace ZMath.Algebraic
{
	public class StringTokenizer
	{
		public static List<SymbolToken> Parse(string expression)
		{
			var pipe1 = new StringToPrimitiveTokenPipe(expression);
			var pipe2 = new MatchAllParenthesesProcessor(pipe1);
			var pipe3 = new NegationProcessor(pipe2);
			var pipe4 = new RedundantParenthesesProcessor(pipe3);
			var pipe5 = new TokenValidater(pipe4);
			return pipe5.PumpAll();
		}

		public static ISymbol BuildTreeFrom(string expression)
		{
			var tokens = Parse(expression);
			var tb = new TreeBuilder(tokens);
			return tb.Parse();
		}
	}
}
