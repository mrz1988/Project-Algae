using System;
namespace ZMath.Algebraic
{
	public class TokenParseException : Exception
	{
		public int Position;
		public int Length;
		public TokenParseException(int position, int length, string msg) : base(msg)
		{
			Position = position;
			Length = length;
		}
	}

	public class UnrecognizedTokenException : TokenParseException
	{
		public UnrecognizedTokenException(int p, int l, string token)
			: base(p, l, FormatMsg(p, token)) { }

		private static string FormatMsg(int position, string token)
		{
			return string.Format("Bad token @[{0}]: {1}", position, token);
		}
	}

	public class InvalidParenthesisException : TokenParseException
	{
		public InvalidParenthesisException(int p)
			: base(p, 1, string.Format("Invalid parenthesis @[{0}]", p)) { }
	}

	public class InvalidTokenException : TokenParseException
	{
		public InvalidTokenException(int p, int l, string msg)
			: base(p, l, FormatMsg(p, l, msg)) { }

		private static string FormatMsg(int pos, int len, string msg)
		{
			return string.Format("{0} @[{1}]", msg, pos);
		}
	}
}
