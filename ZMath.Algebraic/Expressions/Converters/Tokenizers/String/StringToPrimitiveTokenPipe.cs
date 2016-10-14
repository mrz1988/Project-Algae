using System;
using System.Text;
using System.Collections.Generic;
using ZUtils.Pipes;

namespace ZMath.Algebraic
{
	public class StringToPrimitiveTokenPipe : AsymmetricPipe<char, SymbolToken>
	{
		private StringBuilder _chars;

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

		public StringToPrimitiveTokenPipe(string input) : this(input.ToCharArray()) { }
		public StringToPrimitiveTokenPipe(IEnumerable<char> input) : base(input)
		{
			_chars = new StringBuilder();
		}

		protected override void Consume(char val)
		{
			if (char.IsWhiteSpace(val))
				return;

			if (char.IsDigit(val) || val == '.')
			{
				ParseNumChar(val);
				return;
			}

			if (SymbolicChars.Contains(val))
			{
				ParseSymbol(val);
				return;
			}

			ParseWordChar(val);
		}

		protected override void Finish()
		{
			var finalPart = _chars.ToString();
			if (finalPart.Length > 0)
			{
				_chars = new StringBuilder();
				ParseString(finalPart);
			}
		}

		private void ParseString(string s)
		{
			SymbolToken token;
			var parsed = SymbolToken.TryParse(s, out token);
			if (parsed)
				Output(token);
			else
				throw new InvalidTokenException(s);
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

		private void ParseSymbol(char symbol)
		{
			if (_buildingNum || _buildingWord)
				CompleteToken();

			// Assume all symbols are single-char
			ParseString(symbol.ToString());
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
	}
}
