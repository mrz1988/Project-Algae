using System;
namespace ZMath.Algebraic
{
	public class StringParseException : Exception
	{
		public int Position;
		public int Length;
		public StringParseException(int position, int length, string msg) : base(msg)
		{
			Position = position;
			Length = length;
		}
	}

	public class UnrecognizedTokenException : StringParseException
	{
		public UnrecognizedTokenException(int p, int l, string token)
			: base(p, l, FormatMsg(p, token)) { }

		private static string FormatMsg(int position, string token)
		{
			return string.Format("Bad token @[{0}]: {1}", position, token);
		}
	}

	public class InvalidParenthesisException : StringParseException
	{
		public InvalidParenthesisException(int p)
			: base(p, 1, string.Format("Invalid parenthesis @[{0}]", p)) { }
	}
}
