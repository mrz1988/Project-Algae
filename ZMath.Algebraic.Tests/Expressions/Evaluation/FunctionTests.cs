using System;
using Xunit;

namespace ZMath.Algebraic.Tests
{
	public static class FunctionTests
	{
		[Fact]
		public static void CanSubstituteAndEvaluateFunction()
		{
			var ctx = VariableContext.FromVariableNames("x");
			var tree = StringTokenizer.BuildTreeFrom(" 2 * x", ctx);
			var function = new SingleVariableFunction(tree, "x");

			Assert.Equal(4, function.Call(2).AsInt);
			Assert.Equal(10, function.Call(5).AsInt);
		}
	}
}
