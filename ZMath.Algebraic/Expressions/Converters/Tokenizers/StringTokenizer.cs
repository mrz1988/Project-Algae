using System;
using System.Collections.Generic;
using System.Text;

namespace ZMath.Algebraic
{
	public class InvalidTokenException : Exception
	{
		public InvalidTokenException(string message) : base(message) { }
	}

	public class StringTokenizer
	{
		public static bool TryParse(string expression, out List<SymbolToken> tokens)
		{
			var tb = new StringTokenBuilder();
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

	public class StringTokenBuilder
	{
		private StringBuilder _chars;
		private List<SymbolToken> _tokens;

		private bool _buildingNum = false;
		private bool _buildingWord = false;

		public StringTokenBuilder()
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
		}

		public List<SymbolToken> Finish()
		{
			var finalPart = _chars.ToString();
			if (finalPart.Length > 0)
			{
				_chars = new StringBuilder();
				ParseString(finalPart);
			}

			return _tokens;
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
}
