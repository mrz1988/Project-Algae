using System;
using Xunit;

namespace ZMath.Algebraic.Tests
{
	public static class StringTokenizerValidationTests
	{
		[Fact]
		public static void CannotHaveConsecutiveBinaryOperators()
		{
			var e = Assert.Throws<InvalidTokenException>(() =>
			{
				StringTokenizer.Parse("2 ++3");
			});

			Assert.Equal(3, e.Position);
			Assert.Equal(1, e.Length);
		}

		[Fact]
		public static void UnaryOperatorMustHaveParenthesisFollowing()
		{
			var e = Assert.Throws<InvalidTokenException>(() =>
			{
				StringTokenizer.Parse("sin2");
			});

			Assert.Equal(3, e.Position);
			Assert.Equal(1, e.Length);
		}

		[Fact]
		public static void NumbersCannotHaveMultipleDecimalPoints()
		{
			var e = Assert.Throws<InvalidTokenException>(() =>
			{
				StringTokenizer.Parse("1.3 + 123.34.56");
			});

			Assert.Equal(6, e.Position);
			Assert.Equal(9, e.Length);
		}

		[Fact]
		public static void EmptyParenthesesThrow()
		{
			var e = Assert.Throws<InvalidParenthesisException>(() =>
			{
				StringTokenizer.Parse("()");
			});

			Assert.Equal(1, e.Position);
			Assert.Equal(1, e.Length);
		}

		[Fact]
		public static void EmptyParenthesesThrowInMiddle()
		{
			var e = Assert.Throws<InvalidParenthesisException>(() =>
			{
				StringTokenizer.Parse("4 + (()6");
			});

			Assert.Equal(6, e.Position);
			Assert.Equal(1, e.Length);
		}

		[Fact]
		public static void MismatchedParenthesesThrow()
		{
			var e = Assert.Throws<InvalidParenthesisException>(() =>
			{
				StringTokenizer.Parse("(3 + 5))");
			});

			Assert.Equal(7, e.Position);
			Assert.Equal(1, e.Length);
		}
	}
}
