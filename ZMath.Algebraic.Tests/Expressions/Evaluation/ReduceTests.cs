using NUnit.Framework;
using ZMath.Algebraic.Operations;

namespace ZMath.Algebraic.Tests
{
    [TestFixture]
    public static class ReduceTests
    {
        [Test]
        public static void CanReduceWithoutVariables()
        {
            // = 5 * 0 + 5 * 5 / 5
            // = 0 + 25 / 5
            // = 0 + 5
            // = 5
            var op = new Addition(
                new Multiplication(
                    Numbers.Five,
                    Numbers.Zero
                ),
                new Multiplication(
                    Numbers.Five,
                    new Division(
                        Numbers.Five,
                        Numbers.Five
                    )
                )
            );
            var expected = Numbers.Five;

            Assert.AreEqual(expected, op.Reduce());
        }

        [Test]
        public static void CanReduceWithVariables()
        {
            // = (2 + 3) * x + 5 - 3
            // = 5 * x + 2
            // Not sure if this is represented the way it would
            // normally be parsed because I'm lazy but it should
            // reduce the same.
            var op = new Addition(
                new Multiplication(
                    new Addition(
                        Numbers.Two,
                        Numbers.Three
                    ),
                    Numbers.X
                ),
                new Addition(
                    Numbers.Five,
                    new Negation(Numbers.Three)
                )
            );

            var expected = new Addition(
                new Multiplication(
                    Numbers.Five,
                    Numbers.X
                ),
                Numbers.Two
            );

            Assert.AreEqual(expected, op.Reduce());
        }
    }
}
