using System;
using System.Collections.Generic;

namespace ZMath.Algebraic
{
	public class TreeBuilder
	{
		private List<SymbolToken> _tokens;

		public TreeBuilder(List<SymbolToken> tokens)
		{
			_tokens = TrimParentheses(tokens);
			if (_tokens.Count == 0)
				throw new ArgumentException("missing token", nameof(tokens));
			
			if (_tokens.Count == 1 && _tokens[0].Type != SymbolType.Number)
			{
				throw new ArgumentException(string.Format("hanging token: {0}", tokens[0].Token),
					nameof(tokens));
			}
		}

		public static List<SymbolToken> TrimParentheses(List<SymbolToken> tokens)
		{
			var len = tokens.Count;
			while (tokens[0].Type == SymbolType.OpenBracket &&
			    tokens[len - 1].Type == SymbolType.CloseBracket)
			{
				tokens = tokens.GetRange(1, len - 2);
			}

			return tokens;
		}

		public Number ToNumber(SymbolToken token)
		{
			if (token.Type != SymbolType.Number)
				throw new ArgumentException("Not a number", nameof(token));

			var s = token.Token;
			if (!s.Contains("."))
			{
				var i = int.Parse(s);
				return new Number(i);
			}

			var f = double.Parse(s);
			return new Number(f);
		}

		public UnaryOperation ToUnaryOp(SymbolToken op, List<SymbolToken> parameter)
		{
			if (!op.Type.IsUnaryOperation())
				throw new ArgumentException("Not a unary operation", nameof(op));

			var paramTreeBuilder = new TreeBuilder(parameter);
			var paramTree = paramTreeBuilder.Parse();

			switch (op.Type)
			{
				case SymbolType.Negation:
					return new Negation(paramTree);
				case SymbolType.Sine:
					return new Sine(paramTree);
				case SymbolType.Cosine:
					return new Cosine(paramTree);
				case SymbolType.Tangent:
					return new Tangent(paramTree);
			}

			throw new NotImplementedException(string.Format(
				"Support not written for unary operation: {0}", op.Type));
		}

		public ISymbol ToBinaryOp(List<SymbolToken> leftTokens, SymbolToken op, List<SymbolToken> rightTokens)
		{
			if (!op.Type.IsBinaryOperation())
				throw new ArgumentException("Not a binary operation", nameof(op));
			
			var leftParamTreeBuilder = new TreeBuilder(leftTokens);
			var rightParamTreeBuilder = new TreeBuilder(rightTokens);
			var leftParam = leftParamTreeBuilder.Parse();
			var rightParam = rightParamTreeBuilder.Parse();

			switch (op.Type)
			{
				case SymbolType.Addition:
					return new Addition(leftParam, rightParam);
				case SymbolType.Subtraction:
					var right = new Negation(rightParam);
					return new Addition(leftParam, right);
				case SymbolType.Multiplication:
					return new Multiplication(leftParam, rightParam);
				case SymbolType.Division:
					return new Division(leftParam, rightParam);
				case SymbolType.Exponentiation:
					return new Exponentiation(leftParam, rightParam);
			}

			throw new NotImplementedException(string.Format(
				"Support not written for binary operation: {0}", op.Type));
		}

		public ISymbol ParseAsUnaryOp()
		{
			if (_tokens[1].Type != SymbolType.OpenBracket)
			{
				throw new InvalidOperationException("Malformed expression: expected open bracket");
			}

			var innerSymbols = new List<SymbolToken>();
			var parens = 1;
			for (var i = 2; i < _tokens.Count; i++)
			{
				var token = _tokens[i];
				innerSymbols.Add(token);

				if (token.Type == SymbolType.OpenBracket)
					parens++;
				if (token.Type == SymbolType.CloseBracket)
					parens--;

				if (parens < 0)
					throw new InvalidOperationException("Malformed expression: too many close brackets");
				if (parens == 0)
					break;
			}

			return ToUnaryOp(_tokens[0], innerSymbols);
		}

		public ISymbol ParseAsBinaryOp()
		{
			var parens = 0;
			var lowestIx = -1;
			var lowestOp = int.MaxValue;

			for (var i = 0; i < _tokens.Count; i++)
			{
				var token = _tokens[i];

				if (token.Type == SymbolType.OpenBracket)
					parens++;
				else if (token.Type == SymbolType.CloseBracket)
					parens--;

				if (parens < 0)
					throw new InvalidOperationException("Malformed expression: too many close brackets");
				if (parens > 0)
					continue; // skip inner parens, they're done later
				if (!token.Type.IsBinaryOperation())
					continue; // skip all numbers/unaries, they are special cased elsewhere

				var order = token.Type.Order();
				if (order < lowestOp)
				{
					lowestIx = i;
					lowestOp = order;
				}
			}

			if (lowestIx < 0)
				throw new InvalidOperationException("Malformed expression: missing operator");

			var leftSide = _tokens.GetRange(0, lowestIx);
			var op = _tokens[lowestIx];
			var rightSide = _tokens.GetRange(lowestIx + 1,
				_tokens.Count - lowestIx - 1);
			
			return ToBinaryOp(leftSide, op, rightSide);
		}

		public ISymbol Parse()
		{
			var len = _tokens.Count;

			// Case 1: Just a number remaining
			if (len == 1)
				return ToNumber(_tokens[0]);

			// Case 2: Unary operation
			if (_tokens[0].Type.IsUnaryOperation())
			{
				return ParseAsUnaryOp();
			}

			// Case 3: Binary operation
			if (_tokens[0].Type.IsBinaryOperation())
			{
				return ParseAsBinaryOp();
			}

			throw new NotImplementedException();
		}
	}
}
