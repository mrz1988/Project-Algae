using System;
using System.Collections.Generic;
using System.Linq;

namespace ZMath.Algebraic
{
	public class SymbolToken
	{
		public SymbolType Type { get; private set; }
		public string Token { get; private set; }

		public static SymbolToken NegationToken { get { return new SymbolToken(SymbolType.Negation, "-"); } }

		public static readonly Dictionary<string, SymbolType> TextOperators = new Dictionary<string, SymbolType> {
			{ "sin", SymbolType.Sine },
			{ "cos", SymbolType.Cosine },
			{ "tan", SymbolType.Tangent },
			{ "arcsin", SymbolType.ArcSine },
			{ "arccos", SymbolType.ArcCosine },
			{ "arctan", SymbolType.ArcTangent },
			//{ "log", SymbolType.Logarithm },
		};

		public static readonly Dictionary<string, SymbolType> SymbolicOperators = new Dictionary<string, SymbolType> {
			{ "+", SymbolType.Addition },
			{ "-", SymbolType.Subtraction },
			{ "*", SymbolType.Multiplication },
			{ "/", SymbolType.Division },
			{ "^", SymbolType.Exponentiation },
			{ "!", SymbolType.Factorial }
		};

		public static readonly List<string> ValidSymbols = SymbolicOperators.Keys.ToList();

		private SymbolToken(SymbolType type, string token)
		{
			Type = type;
			Token = token;
		}

		public static bool TryParse(string val, out SymbolToken token)
		{
			if (SymbolicOperators.ContainsKey(val))
			{
				var type = SymbolicOperators[val];
				token = new SymbolToken(type, val);
				return true;
			}
			if (TextOperators.ContainsKey(val))
			{
				var type = TextOperators[val];
				token = new SymbolToken(type, val);
				return true;
			}

			int integer;
			if (int.TryParse(val, out integer))
			{
				var type = SymbolType.Number;
				token = new SymbolToken(type, val);
				return true;
			}

			double floating;
			if (double.TryParse(val, out floating))
			{
				var type = SymbolType.Number;
				token = new SymbolToken(type, val);
				return true;
			}

			token = null;
			return false;
		}
	}
}
