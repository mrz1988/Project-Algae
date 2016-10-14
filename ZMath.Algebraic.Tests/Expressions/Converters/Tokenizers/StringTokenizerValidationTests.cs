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
				StringTokenizer.Parse("sin 2");
			});

			Assert.Equal(e.Position, 3);
			Assert.Equal(e.Length, 1);
		}

		[Fact]
		public static void NumbersCannotHaveMultipleDecimalPoints()
		{
			var e = Assert.Throws<InvalidTokenException>(() =>
			{
				StringTokenizer.Parse("1.3 + 123.34.56");
			});

			Assert.Equal(e.Position, 7);
			Assert.Equal(e.Length, 9);
		}
	}
}
