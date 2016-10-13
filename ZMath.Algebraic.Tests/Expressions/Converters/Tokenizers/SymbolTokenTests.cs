using System;
using Xunit;

namespace ZMath.Algebraic.Tests
{
	public static class SymbolTokenTests
	{
		[Fact]
		public static void CanBuildAdditionToken()
		{
			Assert.Equal(SymbolType.Addition, SymbolTokens.Addition.Type);
		}

		[Fact]
		public static void CanBuildSubractionToken()
		{
			Assert.Equal(SymbolType.Subtraction, SymbolTokens.Subtraction.Type);
		}

		[Fact]
		public static void CanBuildMultiplicationToken()
		{
			Assert.Equal(SymbolType.Multiplication, SymbolTokens.Multiplication.Type);
		}

		[Fact]
		public static void CanBuildDivisionToken()
		{
			Assert.Equal(SymbolType.Division, SymbolTokens.Division.Type);
		}

		[Fact]
		public static void CanBuildExponentiationToken()
		{
			Assert.Equal(SymbolType.Exponentiation, SymbolTokens.Exponentiation.Type);
		}

		[Fact]
		public static void CanBuildInt()
		{
			var val = 237;
			var result = SymbolTokens.Number(val);
			Assert.Equal(SymbolType.Number, result.Type);
			Assert.Equal(val.ToString(), result.Token);
		}

		[Fact]
		public static void CanBuildDouble()
		{
			var val = Math.PI;
			var result = SymbolTokens.Number(val);
			Assert.Equal(SymbolType.Number, result.Type);
			Assert.Equal(val.ToString(), result.Token);
		}
	}
}
