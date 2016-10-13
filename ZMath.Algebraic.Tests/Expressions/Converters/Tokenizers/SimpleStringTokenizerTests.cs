using System;
using System.Collections.Generic;
using Xunit;

namespace ZMath.Algebraic.Tests
{
	public static class SimpleStringTokenizerTests
	{
		[Fact]
		public static void CanTokenizeSimpleAddition()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Number(23),
				SymbolTokens.Addition,
				SymbolTokens.Number(41)
			};

			var result = StringTokenizer.Parse("23+41");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanTokenizeSimpleSubtraction()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Number(23),
				SymbolTokens.Subtraction,
				SymbolTokens.Number(41)
			};

			var result = StringTokenizer.Parse("23-41");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanTokenizeSimpleMultiplication()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Number(23),
				SymbolTokens.Multiplication,
				SymbolTokens.Number(41)
			};

			var result = StringTokenizer.Parse("23*41");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanTokenizeSimpleDivision()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Number(23),
				SymbolTokens.Division,
				SymbolTokens.Number(41)
			};

			var result = StringTokenizer.Parse("23/41");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanTokenizeSimpleExponentiation()
		{
			var expectedResult = new List<SymbolToken> {
				SymbolTokens.Number(23),
				SymbolTokens.Exponentiation,
				SymbolTokens.Number(41)
			};

			var result = StringTokenizer.Parse("23^41");
			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public static void CanTokenizeSimpleNegation()
		{
			var expected = new List<SymbolToken> {
				SymbolToken.NegationToken,
				SymbolTokens.LeftParen,
				SymbolTokens.Number(123),
				SymbolTokens.RightParen
			};

			var result1 = StringTokenizer.Parse("-123");
			var result2 = StringTokenizer.Parse("-(123)");
			Assert.Equal(expected, result1);
			Assert.Equal(expected, result2);
		}

		[Fact]
		public static void CanTokenizeSimpleSine()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Sine,
				SymbolTokens.LeftParen,
				SymbolTokens.Number(123),
				SymbolTokens.RightParen
			};

			var result = StringTokenizer.Parse("sin(123)");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanTokenizeSimpleCosine()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Cosine,
				SymbolTokens.LeftParen,
				SymbolTokens.Number(123),
				SymbolTokens.RightParen
			};

			var result = StringTokenizer.Parse("cos(123)");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanTokenizeSimpleTangent()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Tangent,
				SymbolTokens.LeftParen,
				SymbolTokens.Number(123),
				SymbolTokens.RightParen
			};

			var result = StringTokenizer.Parse("tan(123)");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanTokenizeSimpleAdditionWithWhitespace()
		{
			var expected = new List<SymbolToken> {
				SymbolTokens.Number(23),
				SymbolTokens.Addition,
				SymbolTokens.Number(41)
			};

			var result = StringTokenizer.Parse("2 3  +41 ");
			Assert.Equal(expected, result);
		}
	}
}
