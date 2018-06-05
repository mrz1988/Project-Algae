using System;
using NUnit.Framework;
using ZMath.Algebraic.Transforms;

namespace ZMath.Algebraic.Tests.Transforms
{
    [TestFixture]
    public static class TransformIntegrationTests
    {
        [Test]
        public static void CanReduceAlgebraicExpression()
        {
            var transform = SymbolicTransform.MoveComplexSubtreesRight
                .CombineWith(SymbolicTransform.AlgebraicReduce);
            var expression = StringTokenizer.ToExpression("3*x + 3*x + ((3*x * 1)/(3*x * 1))", VariableContext.Default);
            var expected = transform.Transform(StringTokenizer.ToExpression("6*x + 1", VariableContext.Default));
            var result = transform.Transform(expression);

            Assert.AreEqual(expected, result);
        }
    }
}
