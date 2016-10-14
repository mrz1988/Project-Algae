﻿using System;
using System.Collections.Generic;
using ZUtils.Pipes;

namespace ZMath.Algebraic
{
	public class RedundantParenthesesProcessor : AsymmetricPipe<SymbolToken, SymbolToken>
	{
		private bool _holdInput = false;
		private List<SymbolToken> _heldInput;
		private int _parentheses = 0;
		private SymbolToken _lastOutput;

		public RedundantParenthesesProcessor(IEnumerable<SymbolToken> input) : base(input)
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

			if (_parentheses == 0)
			{
				// Remove all surrounding parentheses
				while (_heldInput[0].Type == SymbolType.OpenBracket &&
				    _heldInput[_heldInput.Count - 1].Type == SymbolType.CloseBracket)
				{
					_heldInput = _heldInput.GetRange(1, _heldInput.Count - 2);
				}

				var pp = new RedundantParenthesesProcessor(_heldInput);
				var output = pp.PumpAll();

				// Decide if parentheses should be added back in
				if (output.Count == 1 && !_lastOutput.Type.IsUnaryOperation())
				{
					Output(output);
				}
				else
				{
					Output(SymbolToken.OpenBracket);
					Output(output);
					Output(SymbolToken.CloseBracket);
				}

				_heldInput = new List<SymbolToken>();
				_holdInput = false;
			}
		}

		protected override void Consume(SymbolToken val)
		{
			if (_holdInput)
			{
				Hold(val);
				return;
			}

			if (val.Type == SymbolType.OpenBracket)
			{
				_holdInput = true;
				_parentheses = 0;
				Hold(val);
			}
			else
			{
				Output(val);
			}
		}

		protected override void Finish()
		{
			if (_heldInput.Count == 0)
				return;

			if (_heldInput.Count == 1)
			{
				Output(_heldInput[0]);
			}
			else
			{
				Output(SymbolToken.OpenBracket);
				var pp = new RedundantParenthesesProcessor(_heldInput);
				Output(pp.PumpAll());
				for (int i = 0; i < _parentheses; i++)
				{
					Output(SymbolToken.CloseBracket);
				}
			}

			_parentheses = 0;
			_heldInput = new List<SymbolToken>();
			_lastOutput = SymbolToken.OpenBracket;
			_holdInput = false;
		}
	}
}
