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
			List<SymbolToken> tokens;
			if (!TryParse(expression, out tokens))
				throw new ArgumentException("Could not parse", nameof(expression));

			return tokens;
		}

		public static bool TryParse(string expression, out List<SymbolToken> tokens)
		{
			var tb = new StringToTokensProcessor();
			foreach (char c in expression)
			{
				try
				{
					tb.ConsumeChar(c);
				}
				catch (InvalidTokenException)
				{
					tokens = null;
					return false;
				}
			}

			try
			{
				tokens = tb.Finish();
				return true;
			}
			catch (InvalidTokenException)
			{
				tokens = null;
				return false;
			}
		}
	}

	public class StringToTokensProcessor
	{
		private StringBuilder _chars;
		private List<SymbolToken> _tokens;

		private bool _buildingNum = false;
		private bool _buildingWord = false;

		private static readonly List<char> SymbolicChars = new List<char> {
			'+',
			'-',
			'*',
			'/',
			'^',
			'(',
			')'
		};

		public StringToTokensProcessor()
		{
			_chars = new StringBuilder();
			_tokens = new List<SymbolToken>();
		}

		public void ConsumeChar(char character)
		{
			if (char.IsWhiteSpace(character))
				return;

			if (char.IsDigit(character) || character == '.')
			{
				ParseNumChar(character);
				return;
			}

			if (SymbolicChars.Contains(character))
			{
				ParseSymbol(character);
				return;
			}

			ParseWordChar(character);
		}

		public static List<SymbolToken> PostProcess(List<SymbolToken> firstPass)
		{
			// TODO
			// fix logarithms/binary parenthesized ops

			var secondPass = ProcessNegations(firstPass);
			if (!Validate(secondPass))
			{
				throw new ArgumentException("Invalid");
			}

			return secondPass;
		}

		//TODO: This can use a refactor, it's pretty dense.
		// The idea is this:
		// Anywhere that a "minus sign (-)" exists, we need to
		// determine if it meens "negative" or "subtract".
		// If it is a negative, we have to insert parentheses
		// and a negation sign so it looks something like this:
		// -(innerStuff)
		// This uses a bunch of tricks to figure out where to place
		// the negation vs subraction signs, as well as any new necessary
		// parentheses.
		// It makes it much easier to parse into a tree later if we can
		// standardize how unary/binary operations are tokenized.
		// Unary:
		// unaryOp ( param )
		// Binary:
		// left binaryOp right
		// We require the above format for everything except negation and logarithms
		// at the moment, but that's likely to break eventually...
		private static List<SymbolToken> ProcessNegations(List<SymbolToken> tokens)
		{
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

		public List<SymbolToken> Finish()
		{
			var finalPart = _chars.ToString();
			if (finalPart.Length > 0)
			{
				_chars = new StringBuilder();
				ParseString(finalPart);
			}

			return PostProcess(_tokens);
		}

		private void ParseNumChar(char digit)
		{
			if (_buildingWord)
				CompleteToken();
			
			_buildingNum = true;
			_chars.Append(digit);
		}

		private void ParseWordChar(char letter)
		{
			if (_buildingNum)
				CompleteToken();

			_buildingWord = true;
			_chars.Append(letter);
		}

		private void CompleteToken()
		{
			var tokenString = _chars.ToString();

			try
			{
				ParseString(tokenString);
			}
			finally
			{
				_chars = new StringBuilder();
				_buildingNum = false;
				_buildingWord = false;
			}
		}

		private void ParseString(string s)
		{
			SymbolToken token;
			var parsed = SymbolToken.TryParse(s, out token);
			if (parsed)
				_tokens.Add(token);
			else
				throw new InvalidTokenException(s);
		}

		private void ParseSymbol(char symbol)
		{
			if (_buildingNum || _buildingWord)
				CompleteToken();

			// Assume all symbols are single-char
			ParseString(symbol.ToString());
		}
	}

	public class ValidationProcessor
	{

	}
}
