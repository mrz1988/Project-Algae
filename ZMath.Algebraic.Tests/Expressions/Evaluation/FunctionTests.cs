using NUnit.Framework;

namespace ZMath.Algebraic.Tests
{
    [TestFixture]
	public static class FunctionTests
	{
		[Test]
		public static void CanSubstituteAndEvaluateFunction()
		{
			var ctx = VariableContext.FromVariableNames("x");
			var tree = StringTokenizer.BuildTreeFrom(" 2 * x", ctx);
			var function = new SingleVariableFunction(tree, "x");

			Assert.AreEqual(4, function.Call(2).AsInt);
			Assert.AreEqual(10, function.Call(5).AsInt);
		}
	}
}
