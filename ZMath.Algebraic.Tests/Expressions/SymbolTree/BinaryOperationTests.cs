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

		[Fact]
		public static void CanEquateBinaryOperations()
		{
			var op1 = new Addition(One, Two);
			var op2 = new Addition(One, Two);

			Assert.True(op1.Equals(op2));
			Assert.True(op1.Equals(op1));
			Assert.True(op2.Equals(op1));
		}

		[Fact]
		public static void CanEquateBinaryOperationsWithOperator()
		{
			var op1 = new Multiplication(Two, Two);
			var op2 = new Multiplication(Two, Two);

			Assert.True(op1 == op2);
			Assert.True(op1 == op1);
			Assert.True(op2 == op1);
		}

		[Fact]
		public static void BinaryOperationsWithDifferentOperandsAreNotEqual()
		{
			var op1 = new Addition(One, One);
			var op2 = new Addition(One, Two);
			var op3 = new Addition(Two, One);

			Assert.True(op1 != op2);
			Assert.True(op2 != op1);
			Assert.True(op2 != op3);
			Assert.True(op3 != op2);
			Assert.True(op1 != op3);
			Assert.True(op3 != op1);
		}

		[Fact]
		public static void BinaryOperationsWithDifferentOperatorsAreNotEqual()
		{
			var op1 = new Addition(One, One);
			var op2 = new Multiplication(One, One);

			Assert.True(op1 != op2);
		}

		[Fact]
		public static void BinaryOperationsAreNotEqualToTheirResults()
		{
			var op1 = new Addition(One, One);

			Assert.True(op1 != Two);
			Assert.False(op1 == Two);
		}

		[Fact]
		public static void BinaryOperationsAreNotEqualToNull()
		{
			var op1 = new Addition(One, One);

			Assert.True(op1 != null);
			Assert.False(op1 == null);
			Assert.False(null == op1);
		}

		[Fact]
		public static void BinaryOperationsCanCompareToUnaryOperations()
		{
			var op1 = new Addition(One, One);
			var op2 = new Negation(Two);

			Assert.False(op1 == op2);
			Assert.False(op2 == op1);
			Assert.False(op1.Equals(op2));
			Assert.False(op2.Equals(op1));
		}
	}
}
