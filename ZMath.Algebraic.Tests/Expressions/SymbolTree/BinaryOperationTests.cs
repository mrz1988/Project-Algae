using System;
using Xunit;

namespace ZMath.Algebraic.Tests
{
	public class BinaryOperationTests
	{
		private static Number Two { get { return new Number(2); } }
		private static Number One { get { return new Number(1); } }
		private static Number Five { get { return new Number(5); } }
		private static Number Three { get { return new Number(3); } }

		[Fact]
		public static void CanAddTwoNumbers()
		{
			var val1 = 10;
			var val2 = 3;
			var sum = val1 + val2;

			var num1 = new Number(val1);
			var num2 = new Number(val2);
			var addition = new Addition(num1, num2);

			Assert.Equal(sum, addition.GetValue().AsInt);
		}

		[Fact]
		public static void CanAddThreeNumbers()
		{
			var val1 = 1;
			var val2 = 2;
			var val3 = 3;
			var sum = val1 + val2 + val3;

			var num1 = new Number(val1);
			var num2 = new Number(val2);
			var num3 = new Number(val3);

			var nestedAddition = new Addition(num2, num3);
			var addition = new Addition(num1, nestedAddition);

			Assert.Equal(sum, addition.GetValue().AsInt);
		}

		[Fact]
		public static void AdditionHasCorrectType()
		{
			var addition = new Addition(One, One);
			Assert.Equal(SymbolType.Addition, addition.Type);
		}

		[Fact]
		public static void CanMultiplyTwoNumbers()
		{
			var val1 = 10;
			var val2 = 3;
			var product = val1 * val2;

			var num1 = new Number(val1);
			var num2 = new Number(val2);
			var mult = new Multiplication(num1, num2);

			Assert.Equal(product, mult.GetValue().AsInt);
		}

		[Fact]
		public static void MultiplicationHasCorrectType()
		{
			var multiplication = new Multiplication(One, One);
			Assert.Equal(SymbolType.Multiplication, multiplication.Type);
		}

		[Fact]
		public static void CanDivideTwoIntegersWithNoRemainder()
		{
			var val1 = 20;
			var val2 = 2;
			var quotient = val1 / val2;

			var num1 = new Number(val1);
			var num2 = new Number(val2);
			var div = new Division(num1, num2);

			Assert.Equal(quotient, div.GetValue().AsInt);
		}

		[Fact]
		public static void CanDivideTwoIntegersWithRemainder()
		{
			int val1 = 5;
			int val2 = 2;
			var quotient = 2.5;

			var num1 = new Number(val1);
			var num2 = new Number(val2);
			var div = new Division(num1, num2);

			Assert.Equal(quotient, div.GetValue().AsFloatingPt);
		}

		[Fact]
		public static void DivisionHasCorrectType()
		{
			var div = new Division(One, One);
			Assert.Equal(SymbolType.Division, div.Type);
		}

		[Fact]
		public static void CanSquareANumber()
		{
			var exp = new Exponentiation(Five, Two);

			Assert.Equal(25, exp.GetValue().AsInt);
		}

		[Fact]
		public static void ExponentiationHasCorrectType()
		{
			var exp = new Exponentiation(One, One);
			Assert.Equal(SymbolType.Exponentiation, exp.Type);
		}
	}
}
