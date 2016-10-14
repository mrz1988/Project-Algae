using System;
using System.Collections.Generic;
using ZUtils.Pipes;

namespace ZMath.Algebraic
{
	public class NegationProcessor : AsymmetricPipe<SymbolToken, SymbolToken>
	{
		private SymbolToken _lastOutput;
		private bool _holdInput;
		private List<SymbolToken> _heldInput;
		private int _parentheses = 0;

		public NegationProcessor(IEnumerable<SymbolToken> input) : base(input) { }

		protected override void Output(SymbolToken val)
		{
			base.Output(val);
			_lastOutput = val;
		}

		private void Hold(SymbolToken val)
		{
			_heldInput.Add(val);
			if (val.Type == SymbolType.OpenBracket)
				_parentheses++;
			if (val.Type == SymbolType.CloseBracket)
				_parentheses--;

			//TODO: Handle this better with position reporting!
			if (_parentheses < 0)
				throw new SymbolSyntaxException("Extra close parenthesis");

			// be careful here: new, unparsed subtractions can be continued
			// negation tokens. Be sure to handle those!
			if (_parentheses == 0 && val.Type != SymbolType.Subtraction)
			{
				var np = new NegationProcessor(_heldInput);
				Output(np.PumpAll());
				_heldInput = new List<SymbolToken>();
			}
		}

		public override void Consume(SymbolToken val)
		{
			if (_holdInput)
			{
				Hold(val);
				return;
			}

			if (_lastOutput.Type == SymbolType.Negation)
			{
				_holdInput = true;
				Hold(val);
				return;
				if (val.Type.IsValue())
				{
					Output(SymbolToken.OpenBracket);
					Output(val);
					Output(SymbolToken.CloseBracket);
				}

				if (val.Type.IsUnaryOperation())
				{
					// Start new parentheses context (add open bracket)
					// consume until close bracket is found into a list
					// process the list with a new negation processor
					// output all results
					// output a close bracket
				}

				if (val.Type == SymbolType.Subtraction)
				{
					// special case, double negative.
					// Add extra open parenthesis, and negation token
					// consume 
				}
			}

			if (val.Type != SymbolType.Subtraction)
			{
				Output(val);
				return;
			}

			if (_lastOutput.Type == SymbolType.OpenBracket || _lastOutput.Type.IsBinaryOperation())
			{
				Output(SymbolToken.NegationToken);
				return;
			}
				
			var revisedTokens = new List<SymbolToken>();
			for (int i = 0; i < tokens.Count; i++)
			{
				var token = tokens[i];
				if (token.Type != SymbolType.Subtraction)
				{
					revisedTokens.Add(token);
					continue;
				}

				var prevType = i > 0 ? tokens[i - 1].Type : SymbolType.OpenBracket;
				if (prevType == SymbolType.OpenBracket || prevType.IsBinaryOperation())
				{
					revisedTokens.Add(SymbolToken.NegationToken);

					var nextToken = tokens[i + 1];
					if (nextToken.Type.IsValue())
					{
						revisedTokens.Add(SymbolToken.OpenBracket);
						revisedTokens.Add(nextToken);
						revisedTokens.Add(SymbolToken.CloseBracket);
						i++;
						continue;
					}

					var closesNeeded = 0;
					if (nextToken.Type.IsUnaryOperation())
					{
						revisedTokens.Add(SymbolToken.OpenBracket);
						revisedTokens.Add(nextToken);
						i++;
						closesNeeded++;
					}
					else if (nextToken.Type == SymbolType.Subtraction)
					{
						revisedTokens.Add(SymbolToken.OpenBracket);
						revisedTokens.Add(SymbolToken.NegationToken);
						i++;
						closesNeeded++;
					}

					// we should be on an open bracket now, or there's
					// a syntax error.
					nextToken = tokens[i + 1];
					if (nextToken.Type == SymbolType.Subtraction)
					{

					}
					else if (nextToken.Type != SymbolType.OpenBracket)
						throw new SymbolSyntaxException("Missing open parenthesis near negation");

					if (!needsClose)
						continue; //other parentheses will close themselves

					// Add in the open bracket we're skipping
					revisedTokens.Add(SymbolToken.OpenBracket);
					// Skip to next valid token
					i += 2;
					var parentheses = 1;
					var inner = new List<SymbolToken>();
					while (parentheses > 0)
					{
						if (i == tokens.Count)
							throw new IndexOutOfRangeException("Missing close parenthesis");
						var cur = tokens[i];
						if (cur.Type == SymbolType.CloseBracket)
							parentheses--;
						else if (cur.Type == SymbolType.OpenBracket)
							parentheses++;
						inner.Add(cur);
						i++;
					}

					revisedTokens.AddRange(ProcessNegations(inner));
					revisedTokens.Add(SymbolToken.CloseBracket);
				}
				else
				{
					// use as traditional subtraction operator
					revisedTokens.Add(token);
				}
			}

			return revisedTokens;
		}
	}
}
