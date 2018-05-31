using System.Collections.Generic;
using NUnit.Framework;
using ZMath.Algebraic.Operations;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Tests
{
    [TestFixture]
    public static class TreeBuilderTests
    {
        [Test]
        public static void CanBuildAdditionTree()
        {
            var expected = new Addition(
                new Number(3),
                new Number(3)
            );

            var result = StringTokenizer.BuildTreeFrom("3 + 3");

            Assert.AreEqual(expected, result);
        }

        [Test]
        public static void CanBuildNegationTree()
        {
            var expected = new Negation(new Number(3));

            var result = StringTokenizer.BuildTreeFrom("-3");

            Assert.AreEqual(expected, result);
        }

        [Test]
        public static void CanBuildSubtractionTree()
        {
            var expected = new Addition(
                new Number(3),
                new Negation(
                    new Number(3)
                )
            );

            var result = StringTokenizer.BuildTreeFrom("3 - 3");
            Assert.AreEqual(expected, result);
        }

        [Test]
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
            Assert.AreEqual(expected, result);
        }

        [Test]
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

            Assert.AreEqual(expected, result);
        }

        [Test]
        public static void CanSolveSimpleOrderOfOperations()
        {
            var tree1 = StringTokenizer.BuildTreeFrom("6+7*8");
            var tree2 = StringTokenizer.BuildTreeFrom("16 / 8 - 2");
            var tree3 = StringTokenizer.BuildTreeFrom("(25-11) * 3");

            var result1 = tree1.GetValue().AsInt;
            var result2 = tree2.GetValue().AsInt;
            var result3 = tree3.GetValue().AsInt;

            Assert.AreEqual(62, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(42, result3);
        }

        [Test]
        public static void CanSolveMediumOrderOfOperations()
        {
            var tree1 = StringTokenizer.BuildTreeFrom("3 + 6 * (5 + 4) / 3 - 7");
            var tree2 = StringTokenizer.BuildTreeFrom("9 - 5 / (8 - 3) * 2 + 6");
            var tree3 = StringTokenizer.BuildTreeFrom("150 / (6 + 3 * 8) - 5");

            var result1 = tree1.GetValue().AsInt;
            var result2 = tree2.GetValue().AsInt;
            var result3 = tree3.GetValue().AsInt;

            Assert.AreEqual(14, result1);
            Assert.AreEqual(13, result2);
            Assert.AreEqual(0, result3);
        }

        [Test]
        public static void CanBuildTreeWithVariable()
        {
            var expected = new Addition(
                new Variable("x"),
                new Multiplication(
                    new Number(2),
                    new Variable("x")
                )
            );

            var ctx = new VariableContext(new Dictionary<string, ISymbol> {
                { "x", new Variable("x") }
            });
            var result = StringTokenizer.BuildTreeFrom("x + 2*x", ctx);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public static void FailsToBuildTreeWithUnrecognizedVariable()
        {
            Assert.Throws<UnrecognizedTokenException>(() => {
                StringTokenizer.BuildTreeFrom("x + 2");
            });
        }

        [Test]
        public static void CanBuildTreeWithConstants()
        {
            var expected = new Number(1);

            var tree = StringTokenizer.BuildTreeFrom("cos(2*pi)");

            Assert.True(tree.CanEvaluate());
            Assert.AreEqual(expected, tree.GetValue());
        }
    }
}
