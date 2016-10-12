using System;
using Xunit;

namespace ZMath.Algebraic.Tests
{
	public static class SymbolTypeTests
	{
		[Fact]
		public static void AllSymbolTypesAreCategorized()
		{
			foreach (SymbolType type in Enum.GetValues(typeof(SymbolType)))
			{
				var isCategorized = type.IsValue() ||
					type.IsUnaryOperation() ||
					type.IsBinaryOperation() ||
					type.IsDirectiveSymbol();
				Assert.True(isCategorized);
			}
		}

		[Fact]
		public static void AllSymbolTypesHaveOrder()
		{
			foreach (SymbolType type in Enum.GetValues(typeof(SymbolType)))
			{
				type.Order();
			}
		}

		[Fact]
		public static void AdditionAndSubtractionHaveSameOrder()
		{
			Assert.Equal(SymbolType.Addition.Order(), SymbolType.Subtraction.Order());
		}

		[Fact]
		public static void DivisionAndMultiplicationHaveSameOrder()
		{
			Assert.Equal(SymbolType.Multiplication.Order(), SymbolType.Division.Order());
		}

		[Fact]
		public static void MultiplicationTrumpsAddition()
		{
			Assert.True(SymbolType.Multiplication.Order() > SymbolType.Addition.Order());
		}

		[Fact]
		public static void ExponentiationTrumpsMultiplication()
		{
			Assert.True(SymbolType.Exponentiation.Order() > SymbolType.Multiplication.Order());
		}

		[Fact]
		public static void AllUnaryOperationsHaveSameOrder()
		{
			var order = SymbolType.Negation.Order();

			foreach (SymbolType type in Enum.GetValues(typeof(SymbolType)))
			{
				if (type.IsUnaryOperation())
				{
					Assert.Equal(order, type.Order());
				}
			}
		}

		[Fact]
		public static void UnaryOperationsHavePrecedence()
		{
			Assert.True(SymbolType.Negation.Order() > SymbolType.Exponentiation.Order());
		}
	}
}
