using System;
using System.Collections.Generic;
using System.Text;
using ZUtils.Pipes;

namespace ZMath.Algebraic
{
	public class InvalidTokenException : Exception
	{
		public InvalidTokenException(string message) : base(message) { }
	}

	public class StringTokenizer
	{
		public static List<SymbolToken> Parse(string expression)
		{
			var pipe1 = new StringToPrimitiveTokenPipe(expression);
			var pipe2 = new NegationProcessor(pipe1);
			var pipe3 = new RedundantParenthesesProcessor(pipe2);
			return pipe3.PumpAll();
		}
	}

	public class StringToTokensProcessor
	{
		private static bool Validate(List<SymbolToken> tokens)
		{
			for (int i = 0; i < tokens.Count; i++)
			{
				var token = tokens[i];

				// Unary operations must have open bracket after
				if (token.Type.IsUnaryOperation())
				{
					if (tokens[i + 1].Type != SymbolType.OpenBracket)
						return false;
				}

				// Two binary operators cannot exist in sequence
				if (token.Type.IsBinaryOperation())
				{
					if (tokens[i + 1].Type.IsBinaryOperation())
						return false;
				}

				// Two values cannot exist in sequence
				if (token.Type.IsValue())
				{
					if (i + 1 >= tokens.Count)
						return true;
					if (tokens[i + 1].Type.IsValue())
						return false;
				}

				// Close parentheses must follow an open parenthesis
				if (token.Type == SymbolType.CloseBracket)
					return false;

				// Each open parenthesis must be closed
				if (token.Type == SymbolType.OpenBracket)
				{
					i++;
					var parentheses = 1;
					var inner = new List<SymbolToken>();

					while (parentheses > 0)
					{
						if (i == tokens.Count)
							return false;
						
						if (tokens[i].Type == SymbolType.OpenBracket)
							parentheses++;
						if (tokens[i].Type == SymbolType.CloseBracket)
							parentheses--;
						if (parentheses > 0)
							inner.Add(tokens[i]);

						i++;
					}

					if (!Validate(inner))
						return false;
				}
			}
			return true;
		}
	}

	public class ValidationProcessor
	{

	}
}
