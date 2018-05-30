using NUnit.Framework;

namespace ZMath.Algebraic.Tests
{
    [TestFixture]
	public class BinaryOperationTests
	{
		private static Number Two { get { return new Number(2); } }
		private static Number One { get { return new Number(1); } }
		private static Number Five { get { return new Number(5); } }
		private static Number Three { get { return new Number(3); } }

		[Test]
		public static void CanAddTwoNumbers()
		{
			var val1 = 10;
			var val2 = 3;
			var sum = val1 + val2;

			var num1 = new Number(val1);
			var num2 = new Number(val2);
			var addition = new Addition(num1, num2);

			Assert.AreEqual(sum, addition.GetValue().AsInt);
		}

		[Test]
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

			Assert.AreEqual(sum, addition.GetValue().AsInt);
		}

		[Test]
		public static void AdditionHasCorrectType()
		{
			var addition = new Addition(One, One);
			Assert.AreEqual(SymbolType.Addition, addition.Type);
		}

		[Test]
		public static void CanMultiplyTwoNumbers()
		{
			var val1 = 10;
			var val2 = 3;
			var product = val1 * val2;

			var num1 = new Number(val1);
			var num2 = new Number(val2);
			var mult = new Multiplication(num1, num2);

			Assert.AreEqual(product, mult.GetValue().AsInt);
		}

		[Test]
		public static void MultiplicationHasCorrectType()
		{
			var multiplication = new Multiplication(One, One);
			Assert.AreEqual(SymbolType.Multiplication, multiplication.Type);
		}

		[Test]
		public static void CanDivideTwoIntegersWithNoRemainder()
		{
			var val1 = 20;
			var val2 = 2;
			var quotient = val1 / val2;

			var num1 = new Number(val1);
			var num2 = new Number(val2);
			var div = new Division(num1, num2);

			Assert.AreEqual(quotient, div.GetValue().AsInt);
		}

		[Test]
		public static void CanDivideTwoIntegersWithRemainder()
		{
			int val1 = 5;
			int val2 = 2;
			var quotient = 2.5;

			var num1 = new Number(val1);
			var num2 = new Number(val2);
			var div = new Division(num1, num2);

			Assert.AreEqual(quotient, div.GetValue().AsFloatingPt);
		}

		[Test]
		public static void DivisionHasCorrectType()
		{
			var div = new Division(One, One);
			Assert.AreEqual(SymbolType.Division, div.Type);
		}

		[Test]
		public static void CanSquareANumber()
		{
			var exp = new Exponentiation(Five, Two);

			Assert.AreEqual(25, exp.GetValue().AsInt);
		}

		[Test]
		public static void ExponentiationHasCorrectType()
		{
			var exp = new Exponentiation(One, One);
			Assert.AreEqual(SymbolType.Exponentiation, exp.Type);
		}

		[Test]
		public static void CanEquateBinaryOperations()
		{
			var op1 = new Addition(One, Two);
			var op2 = new Addition(One, Two);

			Assert.True(op1.Equals(op2));
			Assert.True(op1.Equals(op1));
			Assert.True(op2.Equals(op1));
		}

		[Test]
		public static void CanEquateBinaryOperationsWithOperator()
		{
			var op1 = new Multiplication(Two, Two);
			var op2 = new Multiplication(Two, Two);

			Assert.True(op1 == op2);
#pragma warning disable CS1718 // Comparison made to same variable
			Assert.True(op1 == op1);
#pragma warning restore CS1718 // Comparison made to same variable
			Assert.True(op2 == op1);
		}

		[Test]
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

		[Test]
		public static void BinaryOperationsWithDifferentOperatorsAreNotEqual()
		{
			var op1 = new Addition(One, One);
			var op2 = new Multiplication(One, One);

			Assert.True(op1 != op2);
		}

		[Test]
		public static void BinaryOperationsAreNotEqualToTheirResults()
		{
			var op1 = new Addition(One, One);

			Assert.True(op1 != Two);
			Assert.False(op1 == Two);
		}

		[Test]
		public static void BinaryOperationsAreNotEqualToNull()
		{
			var op1 = new Addition(One, One);

			Assert.True(op1 != null);
			Assert.False(op1 == null);
			Assert.False(null == op1);
		}

		[Test]
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
