using System;
using Xunit;

namespace ZMath.Algebraic.Tests
{
	public static class UnaryOperationTests
	{
		private static Number One { get { return new Number(1); } }

		[Fact]
		public static void CanNegateInteger()
		{
			var num = 1;
			var negated = -1;

			var negation = new Negation(new Number(num));
			Assert.Equal(negated, negation.GetValue().AsInt);
		}

		[Fact]
		public static void CanNegateDouble()
		{
			var num = 3.56;
			var negated = -3.56;

			var negation = new Negation(new Number(num));
			Assert.Equal(negated, negation.GetValue().AsFloatingPt);
		}

		[Fact]
		public static void NegationHasCorrectType()
		{
			var negation = new Negation(One);
			Assert.Equal(SymbolType.Negation, negation.Type);
		}

		[Fact]
		public static void CanSine()
		{
			var num1 = 1;
			var num2 = Math.PI;

			var result1 = Math.Sin(num1);
			var result2 = Math.Sin(num2);

			var sin1 = new Sine(new Number(num1));
			var sin2 = new Sine(Number.Pi);

			Assert.Equal(result1, sin1.GetValue().AsFloatingPt);
			Assert.Equal(result2, sin2.GetValue().AsFloatingPt);
		}

		[Fact]
		public static void SineHasCorrectType()
		{
			var sine = new Sine(One);
			Assert.Equal(SymbolType.Sine, sine.Type);
		}

		[Fact]
		public static void CanCosine()
		{
			var num1 = 1;
			var num2 = Math.PI;

			var result1 = Math.Cos(num1);
			var result2 = Math.Cos(num2);

			var cos1 = new Cosine(new Number(num1));
			var cos2 = new Cosine(Number.Pi);

			Assert.Equal(result1, cos1.GetValue().AsFloatingPt);
			Assert.Equal(result2, cos2.GetValue().AsFloatingPt);
		}

		[Fact]
		public static void CosineHasCorrectType()
		{
			var cosine = new Cosine(One);
			Assert.Equal(SymbolType.Cosine, cosine.Type);
		}

		[Fact]
		public static void CanTangent()
		{
			var num1 = 1;
			var num2 = Math.PI / 2;

			var result1 = Math.Tan(num1);
			var result2 = Math.Tan(num2);

			var tan1 = new Tangent(new Number(num1));
			var tan2 = new Tangent(new Number(num2));

			Assert.Equal(result1, tan1.GetValue().AsFloatingPt);
			Assert.Equal(result2, tan2.GetValue().AsFloatingPt);
		}

		[Fact]
		public static void TangentHasCorrectType()
		{
			var tangent = new Tangent(One);
			Assert.Equal(SymbolType.Tangent, tangent.Type);
		}
	}
}
