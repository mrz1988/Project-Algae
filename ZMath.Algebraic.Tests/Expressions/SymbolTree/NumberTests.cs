using System;
using Xunit;

namespace ZMath.Algebraic.Tests
{
	public static class NumberTests
	{
		[Fact]
		public static void CanStoreFetchInt()
		{
			var n = new Number(5);
			Assert.Equal(5, n.AsInt);
		}

		[Fact]
		public static void CanStoreFetchDouble()
		{
			double val = 10.5;
			var n = new Number(val);
			Assert.Equal(val, n.AsFloatingPt);
		}

		[Fact]
		public static void CanStoreIntFetchDouble()
		{
			int val = 10;
			double valAsFloat = (double)val;
			var n = new Number(val);
			Assert.Equal(valAsFloat, n.AsFloatingPt);
		}

		[Fact]
		public static void CanStoreDoubleFetchInt()
		{
			double val = 10.5;
			int valAsInt = (int)val;
			var n = new Number(val);
			Assert.Equal(valAsInt, n.AsInt);
		}
	}
}
