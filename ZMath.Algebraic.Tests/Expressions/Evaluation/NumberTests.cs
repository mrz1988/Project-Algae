using NUnit.Framework;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Tests
{
    [TestFixture]
	public static class NumberTests
	{
		[Test]
		public static void CanStoreFetchInt()
		{
			var n = new Number(5);
			Assert.AreEqual(5, n.AsInt);
		}

		[Test]
		public static void CanStoreFetchDouble()
		{
			double val = 10.5;
			var n = new Number(val);
			Assert.AreEqual(val, n.AsFloatingPt);
		}

		[Test]
		public static void CanStoreIntFetchDouble()
		{
			int val = 10;
			double valAsFloat = (double)val;
			var n = new Number(val);
			Assert.AreEqual(valAsFloat, n.AsFloatingPt);
		}

		[Test]
		public static void CanStoreDoubleFetchInt()
		{
			double val = 10.5;
			int valAsInt = (int)val;
			var n = new Number(val);
			Assert.AreEqual(valAsInt, n.AsInt);
		}

		[Test]
		public static void IntsAreEquatable()
		{
			var n1 = new Number(3);
			var n2 = new Number(0);
			var n3a = new Number(-3);
			var n3b = new Number(-3);

			Assert.True(n1.Equals(n1));
			Assert.True(n2.Equals(n2));
			Assert.True(n3a.Equals(n3b));
			Assert.True(n3b.Equals(n3a));
		}

		[Test]
		public static void IntsAreEquatableByOperator()
		{
			var n1 = new Number(3);
			var n2 = new Number(0);
			var n3a = new Number(-3);
			var n3b = new Number(-3);

#pragma warning disable CS1718 // Comparison made to same variable
			Assert.True(n1 == n1);
			Assert.True(n2 == n2);
#pragma warning restore CS1718 // Comparison made to same variable
			Assert.True(n3a == n3b);
			Assert.True(n3b == n3a);
		}

		[Test]
		public static void ExactDoublesAreEquatable()
		{
			var n1 = Number.Pi;
			var n2 = Number.E;
			var n3a = new Number(0.1);
			var n3b = new Number(0.1);

			Assert.True(n1.Equals(n1));
			Assert.True(n2.Equals(n2));
			Assert.True(n3a.Equals(n3b));
			Assert.True(n3b.Equals(n3a));
		}

		[Test]
		public static void ExactDoublesAreEquatableByOperator()
		{
			var n1 = Number.Pi;
			var n2 = Number.E;
			var n3a = new Number(0.1);
			var n3b = new Number(0.1);

#pragma warning disable CS1718 // Comparison made to same variable
			Assert.True(n1 == n1);
			Assert.True(n2 == n2);
#pragma warning restore CS1718 // Comparison made to same variable
			Assert.True(n3a == n3b);
			Assert.True(n3b == n3a);
		}

		[Test]
		public static void CloseDoublesAreEquatable()
		{
			var n1 = new Number(10.1);
			var n2 = new Number(10.1000000001);

			Assert.True(n1.Equals(n2));
			Assert.True(n2.Equals(n1));
		}

		[Test]
		public static void CloseDoublesAreEquatableByOperator()
		{
			var n1 = new Number(10.1);
			var n2 = new Number(10.1000000001);

			Assert.True(n1 == n2);
			Assert.True(n2 == n1);
		}

		[Test]
		public static void IntsCanBeNotEqual()
		{
			var n1 = new Number(1);
			var n2 = new Number(-1);

			Assert.False(n1.Equals(n2));
			Assert.False(n2.Equals(n1));
			Assert.True(n1 != n2);
			Assert.True(n2 != n1);
		}

		[Test]
		public static void DoublesCanBeNotEqual()
		{
			var n1 = new Number(0.1);
			var n2 = new Number(0.2);

			Assert.False(n2.Equals(n1));
			Assert.True(n1 != n2);
			Assert.True(n2 != n1);
		}

		[Test]
		public static void SemiCloseDoublesCanBeNotEqual()
		{
			var n1 = new Number(0.1000000);
			var n2 = new Number(0.1000001);

			Assert.False(n2.Equals(n1));
			Assert.True(n1 != n2);
			Assert.True(n2 != n1);
		}

		[Test]
		public static void IntsCannotEqualNull()
		{
			var n = new Number(0);
			Assert.False(n.Equals(null));
			Assert.False(n == null);
			Assert.True(n != null);
		}

		[Test]
		public static void DoublesCannotEqualNull()
		{
			var n = Number.Pi;
			Assert.False(n.Equals(null));
			Assert.False(n == null);
			Assert.True(n != null);
			Assert.False(null == n);
			Assert.True(null != n);
		}

		[Test]
		public static void DoublesCanEqualInts()
		{
			var n1 = new Number(1);
			var n2 = new Number(1.0);

			Assert.True(n1.Equals(n2));
			Assert.True(n1 == n2);
		}


		[Test]
		public static void IntsCanBeGreaterThanOthers()
		{
			var n1 = new Number(0);
			var n2 = new Number(-1);

			Assert.True(n1 > n2);
			Assert.True(n2 < n1);
		}

		[Test]
		public static void DoublesCanBeGreaterThanOthers()
		{
			var n1 = new Number(1.0);
			var n2 = new Number(1.1);

			Assert.True(n2 > n1);
			Assert.True(n1 < n2);
		}

		[Test]
		public static void DoublesCanCompareToInts()
		{
			var n1 = new Number(1);
			var n2 = new Number(1.01);

			Assert.True(n2 > n1);
			Assert.True(n1 < n2);
		}

		[Test]
		public static void AllIntsAreGreaterThanNull()
		{
			var n = new Number(int.MinValue);

			Assert.True(n > null);
			Assert.False(n < null);
			Assert.False(null > n);
			Assert.True(null < n);
		}

		[Test]
		public static void AllDoublesAreGreaterThanNull()
		{
			var n = new Number(double.MinValue);

			Assert.True(n > null);
			Assert.False(n < null);
			Assert.False(null > n);
			Assert.True(null < n);
		}
	}
}
