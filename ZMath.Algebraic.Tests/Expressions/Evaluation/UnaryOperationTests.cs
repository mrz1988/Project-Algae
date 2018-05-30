using System;
using NUnit.Framework;

namespace ZMath.Algebraic.Tests
{
    [TestFixture]
	public static class UnaryOperationTests
	{
		private static Number One { get { return new Number(1); } }
		private static Number Two { get { return new Number(2); } }

		[Test]
		public static void CanNegateInteger()
		{
			var num = 1;
			var negated = -1;

			var negation = new Negation(new Number(num));
			Assert.AreEqual(negated, negation.GetValue().AsInt);
		}

		[Test]
		public static void CanNegateDouble()
		{
			var num = 3.56;
			var negated = -3.56;

			var negation = new Negation(new Number(num));
			Assert.AreEqual(negated, negation.GetValue().AsFloatingPt);
		}

		[Test]
		public static void NegationHasCorrectType()
		{
			var negation = new Negation(One);
			Assert.AreEqual(SymbolType.Negation, negation.Type);
		}

		[Test]
		public static void CanSine()
		{
			var num1 = 1;
			var num2 = Math.PI;

			var result1 = Math.Sin(num1);
			var result2 = Math.Sin(num2);

			var sin1 = new Sine(new Number(num1));
			var sin2 = new Sine(Number.Pi);

			Assert.AreEqual(result1, sin1.GetValue().AsFloatingPt);
			Assert.AreEqual(result2, sin2.GetValue().AsFloatingPt);
		}

		[Test]
		public static void SineHasCorrectType()
		{
			var sine = new Sine(One);
			Assert.AreEqual(SymbolType.Sine, sine.Type);
		}

		[Test]
		public static void CanCosine()
		{
			var num1 = 1;
			var num2 = Math.PI;

			var result1 = Math.Cos(num1);
			var result2 = Math.Cos(num2);

			var cos1 = new Cosine(new Number(num1));
			var cos2 = new Cosine(Number.Pi);

			Assert.AreEqual(result1, cos1.GetValue().AsFloatingPt);
			Assert.AreEqual(result2, cos2.GetValue().AsFloatingPt);
		}

		[Test]
		public static void CosineHasCorrectType()
		{
			var cosine = new Cosine(One);
			Assert.AreEqual(SymbolType.Cosine, cosine.Type);
		}

		[Test]
		public static void CanTangent()
		{
			var num1 = 1;
			var num2 = Math.PI / 2;

			var result1 = Math.Tan(num1);
			var result2 = Math.Tan(num2);

			var tan1 = new Tangent(new Number(num1));
			var tan2 = new Tangent(new Number(num2));

			Assert.AreEqual(result1, tan1.GetValue().AsFloatingPt);
			Assert.AreEqual(result2, tan2.GetValue().AsFloatingPt);
		}

		[Test]
		public static void TangentHasCorrectType()
		{
			var tangent = new Tangent(One);
			Assert.AreEqual(SymbolType.Tangent, tangent.Type);
		}

		[Test]
		public static void UnaryOperationsAreEquatable()
		{
			var op1 = new Negation(One);
			var op2 = new Negation(One);

			Assert.True(op1.Equals(op2));
			Assert.True(op1 == op2);
		}

		[Test]
		public static void UnaryOperationsWithDifferentOperandsAreNotEqual()
		{
			var op1 = new Negation(One);
			var op2 = new Negation(Two);

			Assert.True(op1 != op2);
			Assert.True(op2 != op1);
		}

		[Test]
		public static void UnaryOperationsWithDifferentOperatorsAreNotEqual()
		{
			var op1 = new Negation(One);
			var op2 = new Sine(One);

			Assert.True(op1 != op2);
			Assert.True(op2 != op1);
		}

		[Test]
		public static void UnaryOperationsAreNotEqualToNull()
		{
			var op = new Negation(One);

			Assert.False(op.Equals(null));
			Assert.False(op == null);
			Assert.False(null == op);
		}
	}
}
