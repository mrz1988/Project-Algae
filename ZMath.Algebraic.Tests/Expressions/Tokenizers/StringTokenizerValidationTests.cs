using NUnit.Framework;

namespace ZMath.Algebraic.Tests
{
    [TestFixture]
    public static class StringTokenizerValidationTests
    {
        [Test]
        public static void CannotHaveConsecutiveBinaryOperators()
        {
            var e = Assert.Throws<InvalidTokenException>(() =>
            {
                StringTokenizer.Parse("2 ++3");
            });

            Assert.AreEqual(3, e.Position);
            Assert.AreEqual(1, e.Length);
        }

        [Test]
        public static void UnaryOperatorMustHaveParenthesisFollowing()
        {
            var e = Assert.Throws<InvalidTokenException>(() =>
            {
                StringTokenizer.Parse("sin2");
            });

            Assert.AreEqual(3, e.Position);
            Assert.AreEqual(1, e.Length);
        }

        [Test]
        public static void NumbersCannotHaveMultipleDecimalPoints()
        {
            var e = Assert.Throws<InvalidTokenException>(() =>
            {
                StringTokenizer.Parse("1.3 + 123.34.56");
            });

            Assert.AreEqual(6, e.Position);
            Assert.AreEqual(9, e.Length);
        }

        [Test]
        public static void EmptyParenthesesThrow()
        {
            var e = Assert.Throws<InvalidParenthesisException>(() =>
            {
                StringTokenizer.Parse("()");
            });

            Assert.AreEqual(1, e.Position);
            Assert.AreEqual(1, e.Length);
        }

        [Test]
        public static void EmptyParenthesesThrowInMiddle()
        {
            var e = Assert.Throws<InvalidParenthesisException>(() =>
            {
                StringTokenizer.Parse("4 + (()6");
            });

            Assert.AreEqual(6, e.Position);
            Assert.AreEqual(1, e.Length);
        }

        [Test]
        public static void MismatchedParenthesesThrow()
        {
            var e = Assert.Throws<InvalidParenthesisException>(() =>
            {
                StringTokenizer.Parse("(3 + 5))");
            });

            Assert.AreEqual(7, e.Position);
            Assert.AreEqual(1, e.Length);
        }
    }
}
