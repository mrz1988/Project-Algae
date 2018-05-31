using System;
using NUnit.Framework;
using ZMath.Algebraic.Constraints;

namespace ZMath.Algebraic.Tests.Expressions.Constraints
{
    [TestFixture]
    public static class SymbolConstraintTests
    {
        [Test]
        public static void CanMatchConstant()
        {
            var constraint = SymbolConstraints.PureConstant;
            var passingTest = Numbers.One;
            var failingTest = Operations.BasicAddition;

            Assert.True(passingTest.Matches(constraint));
            Assert.False(failingTest.Matches(constraint));
        }

        [Test]
        public static void CanMatchReducibleConstants()
        {
            var constraint = SymbolConstraints.ReducibleConstant;
            var passing1 = Numbers.One;
            var passing2 = Operations.BasicAddition;
            var passing3 = Operations.BasicNegation;
            var failing = Operations.AdditionWithVariable;

            Assert.True(passing1.Matches(constraint));
            Assert.True(passing2.Matches(constraint));
            Assert.True(passing3.Matches(constraint));
            Assert.False(failing.Matches(constraint));
        }

        [Test]
        public static void ValuesWithoutTwoChildrenFailOnTwoChildConstraints()
        {
            var constraint = new SymbolConstraint(s => { return true; }, new SymbolConstraint[] {
                SymbolConstraints.Any, SymbolConstraints.Any });
            var fails1 = Numbers.One;
            var fails2 = Operations.BasicNegation;
            var passes = Operations.BasicAddition;

            Assert.True(passes.Matches(constraint));
            Assert.False(fails1.Matches(constraint));
            Assert.False(fails2.Matches(constraint));
        }
    }
}
