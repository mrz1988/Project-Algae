using System;
namespace ZMath.Algebraic
{
	public class ParsedSymbolToken : SymbolToken
	{
		public int Position;
		public int Length;

		public ParsedSymbolToken(SymbolType type, string token, int pos, int length) : base(type, token)
		{
			Position = pos;
			Length = length;
		}
	}
}
