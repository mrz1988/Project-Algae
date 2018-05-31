using System;
using NUnit.Framework;
using ZMath.Algebraic.Constraints;
using ZMath.Algebraic.Operations;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Tests.Expressions.Constraints
{
    [TestFixture]
    public static class SymbolConstraintTests
    {
        [Test]
        public static void CanMatchOne()
        {
            var constraint = SymbolConstraints.IsOne;
            var intOne = Numbers.One;
            var floatOne = new Number(1.0);
            Assert.True(intOne.Matches(constraint));
            Assert.True(floatOne.Matches(constraint));
            Assert.False(Numbers.Zero.Matches(constraint));
        }

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

        [Test]
        public static void ValuesWithChildrenDontFailWithNoChildConstraints()
        {
            var constraint = SymbolConstraints.Any;
            Assert.True(Operations.BasicAddition.Matches(constraint));
        }

        [Test]
        public static void CanFailOnChildConstraints()
        {
            var constraint = new SymbolConstraint(
                s => { return true; },
                new SymbolConstraint[]
                {
                    SymbolConstraints.Any,
                    SymbolConstraints.ReducibleConstant
                }
            );

            var passing1 = new Addition(Numbers.X, Numbers.One);
            var passing2 = Operations.BasicAddition;
            var failing1 = new Addition(Numbers.One, Numbers.X);
            var failing2 = Numbers.One;
            var failing3 = Operations.BasicSine;

            Assert.True(passing1.Matches(constraint));
            Assert.True(passing2.Matches(constraint));
            Assert.False(failing1.Matches(constraint));
            Assert.False(failing2.Matches(constraint));
            Assert.False(failing3.Matches(constraint));
        }
    }
}
