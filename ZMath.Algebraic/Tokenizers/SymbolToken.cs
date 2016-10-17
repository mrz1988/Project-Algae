using System;
using System.Collections.Generic;
using System.Linq;

namespace ZMath.Algebraic
{
	public class SymbolToken
	{
		public SymbolType Type { get; private set; }
		public string Token { get; private set; }

		public int Position { get; private set; } = -1;
		public int Length { get; private set; } = -1;
		public bool HasPositionSet { get { return Position >= 0 && Length >= 0; } }

		public static SymbolToken NegationToken { get { return new SymbolToken(SymbolType.Negation, "-"); } }
		public static SymbolToken OpenBracket { get { return new SymbolToken(SymbolType.OpenBracket, "("); } }
		public static SymbolToken CloseBracket { get { return new SymbolToken(SymbolType.CloseBracket, ")"); } }

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
			{ "!", SymbolType.Factorial },
			{ "(", SymbolType.OpenBracket },
			{ "[", SymbolType.OpenBracket },
			{ "{", SymbolType.OpenBracket },
			{ ")", SymbolType.CloseBracket },
			{ "]", SymbolType.CloseBracket },
			{ "}", SymbolType.CloseBracket },
		};

		public static readonly List<string> ValidSymbols = SymbolicOperators.Keys.ToList();

		public SymbolToken(SymbolType type, string token)
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

		public SymbolToken SetPosition(int position, int length)
		{
			Position = position;
			Length = length;
			return this;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			var t = (SymbolToken)obj;

			return Token == t.Token && Type == t.Type;
		}

		public override int GetHashCode()
		{
			var hash = 27;
			hash = (hash * 13) + Type.GetHashCode();
			hash = (hash * 13) + Token.GetHashCode();

			return hash;
		}

		public static bool operator ==(SymbolToken a, SymbolToken b)
		{
			if (a == null)
				return b == null;

			return a.Equals(b);
		}

		public static bool operator !=(SymbolToken a, SymbolToken b)
		{
			return !(a == b);
		}
	}
}
