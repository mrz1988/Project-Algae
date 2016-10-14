using System;
using System.Collections.Generic;
using Xunit;

namespace ZMath.Algebraic.Tests
{
	public static class ComplexStringTokenizerTests
	{
		[Fact]
		public static void CanTokenizeMultipleAdditions()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Number(23),
				SymbolTokens.Addition,
				SymbolTokens.Number(41),
				SymbolTokens.Addition,
				SymbolToken.NegationToken,
				SymbolToken.OpenBracket,
				SymbolTokens.Number(100.5),
				SymbolToken.CloseBracket,
			};

			var result = StringTokenizer.Parse("23 +41 +-100.5");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanTokenizeWithNegatedUnary()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Number(20),
				SymbolTokens.Multiplication,
				SymbolToken.NegationToken,
				SymbolToken.OpenBracket,
				SymbolTokens.Sine,
				SymbolToken.OpenBracket,
				SymbolTokens.Number(123.45),
				SymbolToken.CloseBracket,
				SymbolToken.CloseBracket
			};

			var result = StringTokenizer.Parse("20 * -sin(123.45)");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanTokenizeNegatedParenthesizedExpression()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Number(20),
				SymbolTokens.Subtraction,
				SymbolToken.NegationToken,
				SymbolToken.OpenBracket,
				SymbolTokens.Number(3),
				SymbolTokens.Addition,
				SymbolTokens.Number(3),
				SymbolTokens.Multiplication,
				SymbolTokens.Number(3),
				SymbolToken.CloseBracket
			};

			var result = StringTokenizer.Parse("20 --(3 + 3 * 3)");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanTokenizeMultipleNegationSymbols()
		{
			var expected = new List<SymbolToken> {
				SymbolToken.NegationToken,
				SymbolToken.OpenBracket,
				SymbolToken.NegationToken,
				SymbolToken.OpenBracket,
				SymbolToken.NegationToken,
				SymbolToken.OpenBracket,
				SymbolTokens.Number(3),
				SymbolToken.CloseBracket,
				SymbolToken.CloseBracket,
				SymbolToken.CloseBracket
			};

			var result1 = StringTokenizer.Parse("---3");
			var result2 = StringTokenizer.Parse("-(--3)");

			Assert.Equal(expected, result1);
			Assert.Equal(expected, result2);
		}
	}
}
