using System;
using System.Collections.Generic;
using ZUtils.Pipes;

namespace ZMath.Algebraic
{
	public class MatchAllParenthesesProcessor : AsymmetricPipe<SymbolToken, SymbolToken>
	{
		private int _parens = 0;
		public MatchAllParenthesesProcessor(IEnumerable<SymbolToken> input) : base(input) { }

		protected override void Consume(SymbolToken val)
		{
			if (val.Type == SymbolType.OpenBracket)
				_parens++;
			else if (val.Type == SymbolType.CloseBracket)
				_parens--;

			Output(val);
		}

		protected override void Finish()
		{
			for (var i = 0; i < _parens; i++)
			{
				Output(SymbolToken.CloseBracket);
			}
		}
	}
}
