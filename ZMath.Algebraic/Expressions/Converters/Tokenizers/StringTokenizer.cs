using System;
using System.Collections.Generic;

namespace ZMath.Algebraic
{
	public class StringTokenizer
	{
		public static List<SymbolToken> Parse(string expression)
		{
			var pipe1 = new StringToPrimitiveTokenPipe(expression);
			var pipe2 = new NegationProcessor(pipe1);
			var pipe3 = new RedundantParenthesesProcessor(pipe2);
			var pipe4 = new TokenValidater(pipe3);
			return pipe4.PumpAll();
		}
	}
}
