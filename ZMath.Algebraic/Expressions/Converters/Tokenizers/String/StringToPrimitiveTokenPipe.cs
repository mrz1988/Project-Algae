﻿using System;
using System.Text;
using System.Collections.Generic;
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
				_recentWhitespace++;
				return;
			}

			if (char.IsDigit(val) || val == '.')
				ParseNumChar(val);
			else if (SymbolicChars.Contains(val))
				ParseSymbol(val);
			else
				ParseWordChar(val);
			
			_charsParsed++;
			_recentWhitespace = 0;
		}

		protected override void Finish()
		{
			var finalPart = _chars.ToString();
			if (finalPart.Length > 0)
			{
				_chars = new StringBuilder();
				ParseString(finalPart);
			}
			_charsParsed = 0;
			_buildingNum = false;
			_buildingWord = false;
		}

		private void ParseString(string s)
		{
			SymbolToken token;
			var parsed = SymbolToken.TryParse(s, out token);
			var pos = _charsParsed - s.Length - _recentWhitespace;
			var len = s.Length;
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
