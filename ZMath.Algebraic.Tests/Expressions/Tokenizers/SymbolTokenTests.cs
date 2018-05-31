using System;
using NUnit.Framework;

namespace ZMath.Algebraic.Tests
{
    public static class SymbolTokenTests
    {
        [Test]
        public static void CanBuildAdditionToken()
        {
            Assert.AreEqual(SymbolType.Addition, SymbolTokens.Addition.Type);
        }

        [Test]
        public static void CanBuildSubractionToken()
        {
            Assert.AreEqual(SymbolType.Subtraction, SymbolTokens.Subtraction.Type);
        }

        [Test]
        public static void CanBuildMultiplicationToken()
        {
            Assert.AreEqual(SymbolType.Multiplication, SymbolTokens.Multiplication.Type);
        }

        [Test]
        public static void CanBuildDivisionToken()
        {
            Assert.AreEqual(SymbolType.Division, SymbolTokens.Division.Type);
        }

        [Test]
        public static void CanBuildExponentiationToken()
        {
            Assert.AreEqual(SymbolType.Exponentiation, SymbolTokens.Exponentiation.Type);
        }

        [Test]
        public static void CanBuildInt()
        {
            var val = 237;
            var result = SymbolTokens.Number(val);
            Assert.AreEqual(SymbolType.Number, result.Type);
            Assert.AreEqual(val.ToString(), result.Token);
        }

        [Test]
        public static void CanBuildDouble()
        {
            var val = Math.PI;
            var result = SymbolTokens.Number(val);
            Assert.AreEqual(SymbolType.Number, result.Type);
            Assert.AreEqual(val.ToString(), result.Token);
        }
    }
}
