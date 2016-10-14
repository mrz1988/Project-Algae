using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ZUtils.Pipes;

namespace ZMath.Algebraic
{
	public class NegationProcessor : AsymmetricPipe<SymbolToken, SymbolToken>
	{
		private SymbolToken _lastOutput;
		private bool _holdInput;
		private List<SymbolToken> _heldInput;
		private int _parentheses = 0;

		public NegationProcessor(IEnumerable<SymbolToken> input) : base(input)
		{
			_heldInput = new List<SymbolToken>();
			_lastOutput = SymbolToken.OpenBracket;
		}

		protected override void Output(SymbolToken item)
		{
			base.Output(item);
			_lastOutput = item;
		}

		private void Hold(SymbolToken val)
		{
			_heldInput.Add(val);
			if (val.Type == SymbolType.OpenBracket)
				_parentheses++;
			if (val.Type == SymbolType.CloseBracket)
				_parentheses--;

			// Unary operations are the only case where an open parenthesis
			// that applies to the negation may come as the second token.
			// Unparsed negations also count, handled below as a special case
			// (since they do not require parentheses)
			if (_heldInput.Count == 1 && val.Type.IsUnaryOperation())
				return;
			
			// be careful here: new, unparsed subtractions can be continued
			// negation tokens. Be sure to handle those!
			if (_parentheses == 0 && val.Type != SymbolType.Subtraction)
			{
				var np = new NegationProcessor(_heldInput);
				Output(np.PumpAll());
				Output(SymbolToken.CloseBracket);
				_heldInput = new List<SymbolToken>();
				_holdInput = false;
			}
		}

		protected override void Consume(SymbolToken val)
		{
			Contract.Requires(val != null);
			if (_holdInput)
			{
				Hold(val);
				return;
			}

			if (_lastOutput.Type == SymbolType.Negation)
			{
				_holdInput = true;
				Output(SymbolToken.OpenBracket);
				Hold(val);
				return;
			}

			if (val.Type != SymbolType.Subtraction)
			{
				Output(val);
				return;
			}

			// There's some bug in (mono?) where
			// it seems like the || statements don't short
			// circuit, causing weird issues when _lastOutput == null.
			// This works around that for now...
			var isNegationToken = _lastOutput.Type == SymbolType.OpenBracket ||
				_lastOutput.Type.IsBinaryOperation();
			
			if (isNegationToken)
			{
				Output(SymbolToken.NegationToken);
				return;
			}

			Output(val);
		}

		protected override void Finish()
		{
			if (_heldInput.Count == 0)
				return;

			var np = new NegationProcessor(_heldInput);
			Output(np.PumpAll());
			Output(SymbolToken.CloseBracket);
			_parentheses = 0;
			_heldInput = new List<SymbolToken>();
			_lastOutput = SymbolToken.OpenBracket;
			_holdInput = false;
		}
	}
}
