using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ZUtils.Pipes;

namespace ZMath.Algebraic
{
	public class StringToPrimitiveTokenPipe : AsymmetricPipe<char, SymbolToken>
	{
		private StringBuilder _chars;
		private int _charsParsed = 0;
		private int _recentWhitespace = 0;

		private bool _buildingNum = false;
		private bool _buildingWord = false;
		private bool _previouslyConsumedSymbol = false;

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
			{
				_charsParsed++;
				if (!_previouslyConsumedSymbol)
					_recentWhitespace++;
				return;
			}

			if (char.IsDigit(val) || val == '.')
				ParseNumChar(val);
			else if (SymbolicChars.Contains(val))
				ParseSymbol(val);
			else
				ParseWordChar(val);
		}

		protected override void Finish()
		{
			if (_buildingNum || _buildingWord)
				CompleteToken();

			_charsParsed = 0;
			_recentWhitespace = 0;
		}

		private void ParseString(string s)
		{
			SymbolToken token;
			var parsed = SymbolToken.TryParse(s, out token);
			var pos = _charsParsed - _recentWhitespace;
			var len = s.Length;
			_charsParsed += len;
			_recentWhitespace = 0;
			if (parsed)
				Output(token.SetPosition(pos, len));
			else
			{
				throw new UnrecognizedTokenException(pos, len, s);
			}
		}

		private void ParseNumChar(char digit)
		{
			if (_buildingWord)
				CompleteToken();

			_buildingNum = true;
			_chars.Append(digit);
			_previouslyConsumedSymbol = false;
		}

		private void ParseWordChar(char letter)
		{
			if (_buildingNum)
				CompleteToken();

			_buildingWord = true;
			_chars.Append(letter);
			_previouslyConsumedSymbol = false;
		}

		private void ParseSymbol(char symbol)
		{
			if (_buildingNum || _buildingWord)
				CompleteToken();

			// Assume all symbols are single-char
			ParseString(symbol.ToString());
			_previouslyConsumedSymbol = true;
		}

		private void CompleteToken()
		{
			var tokenString = _chars.ToString();

			if (_buildingNum)
			{
				var result = tokenString.Replace(".", "");
				if (tokenString.Length - result.Length > 1)
				{
					var pos = _charsParsed - _recentWhitespace;
					var len = tokenString.Length;
					throw new InvalidTokenException(pos, len, "Multiple decimal points");
				}
			}

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
