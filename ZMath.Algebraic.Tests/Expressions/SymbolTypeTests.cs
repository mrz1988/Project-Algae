using System;
using NUnit.Framework;

namespace ZMath.Algebraic.Tests
{
    [TestFixture]
	public static class SymbolTypeTests
	{
		[Test]
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

		[Test]
		public static void AllSymbolTypesHaveOrder()
		{
			foreach (SymbolType type in Enum.GetValues(typeof(SymbolType)))
			{
				type.Order();
			}
		}

		[Test]
		public static void AdditionAndSubtractionHaveSameOrder()
		{
			Assert.AreEqual(SymbolType.Addition.Order(), SymbolType.Subtraction.Order());
		}

		[Test]
		public static void DivisionAndMultiplicationHaveSameOrder()
		{
			Assert.AreEqual(SymbolType.Multiplication.Order(), SymbolType.Division.Order());
		}

		[Test]
		public static void MultiplicationTrumpsAddition()
		{
			Assert.True(SymbolType.Multiplication.Order() > SymbolType.Addition.Order());
		}

		[Test]
		public static void ExponentiationTrumpsMultiplication()
		{
			Assert.True(SymbolType.Exponentiation.Order() > SymbolType.Multiplication.Order());
		}

		[Test]
		public static void AllUnaryOperationsHaveSameOrder()
		{
			var order = SymbolType.Negation.Order();

			foreach (SymbolType type in Enum.GetValues(typeof(SymbolType)))
			{
				if (type.IsUnaryOperation())
				{
					Assert.AreEqual(order, type.Order());
				}
			}
		}

		[Test]
		public static void UnaryOperationsHavePrecedence()
		{
			Assert.True(SymbolType.Negation.Order() > SymbolType.Exponentiation.Order());
		}
	}
}
