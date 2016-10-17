using System;
using Xunit;

namespace ZMath.Algebraic.Tests
{
	public static class TreeBuilderTests
	{
		[Fact]
		public static void CanBuildAdditionTree()
		{
			var expected = new Addition(
				new Number(3),
				new Number(3)
			);

			var result = StringTokenizer.BuildTreeFrom("3 + 3");

			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanBuildNegationTree()
		{
			var expected = new Negation(new Number(3));

			var result = StringTokenizer.BuildTreeFrom("-3");

			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanBuildSubtractionTree()
		{
			var expected = new Addition(
				new Number(3),
				new Negation(
					new Number(3)
				)
			);

			var result = StringTokenizer.BuildTreeFrom("3 - 3");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanBuildNestedNegationTree()
		{
			var expected = new Addition(
				new Number(3),
				new Negation(
					new Negation(
						new Number(3)
					)
				)
			);

			var result = StringTokenizer.BuildTreeFrom("3 -- 3");
			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanBuildSimpleOrderOfOperations()
		{
			var expected = new Addition(
				new Addition(
					new Number(3),
					new Multiplication(
						new Number(2),
						new Number(5)
					)
				),
				new Multiplication(
					new Number(1),
					new Addition(
						new Number(10),
						new Number(3)
					)
				)
			);
			var result = StringTokenizer.BuildTreeFrom("3 + 2 * 5 + 1 * (10 + 3)");

			Assert.Equal(expected, result);
		}

		[Fact]
		public static void CanSolveSimpleOrderOfOperations()
		{
			var tree1 = StringTokenizer.BuildTreeFrom("6+7*8");
			var tree2 = StringTokenizer.BuildTreeFrom("16 / 8 - 2");
			var tree3 = StringTokenizer.BuildTreeFrom("(25-11) * 3");

			var result1 = tree1.GetValue().AsInt;
			var result2 = tree2.GetValue().AsInt;
			var result3 = tree3.GetValue().AsInt;

			Assert.Equal(62, result1);
			Assert.Equal(0, result2);
			Assert.Equal(42, result3);
		}

		[Fact]
		public static void CanSolveMediumOrderOfOperations()
		{
			var tree1 = StringTokenizer.BuildTreeFrom("3 + 6 * (5 + 4) / 3 - 7");
			var tree2 = StringTokenizer.BuildTreeFrom("9 - 5 / (8 - 3) * 2 + 6");
			var tree3 = StringTokenizer.BuildTreeFrom("150 / (6 + 3 * 8) - 5");

			var result1 = tree1.GetValue().AsInt;
			var result2 = tree2.GetValue().AsInt;
			var result3 = tree3.GetValue().AsInt;

			Assert.Equal(14, result1);
			Assert.Equal(13, result2);
			Assert.Equal(0, result3);
		}
	}
}
