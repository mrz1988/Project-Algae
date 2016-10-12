using System;
using NUnit.Framework;
using ZMath.Algebraic;

namespace ZMath.Algebraic.Tests
{
	[TestFixture]
	public static class SymbolTypeTests
	{
		public static void AllSymbolTypesAreCategorized()
		{
			foreach (var type in Enum.GetValues(typeof(SymbolType)))
			{

			}
		}
	}
}
