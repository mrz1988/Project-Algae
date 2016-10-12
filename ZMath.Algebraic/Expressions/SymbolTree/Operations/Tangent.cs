using System;
namespace ZMath.Algebraic
{
	public class Tangent : UnaryOperation
	{
		public Tangent(ISymbol n) : base(n) { }

		public override SymbolType Type { get { return SymbolType.Tangent; } }

		protected override Number Evaluate(int val)
		{
			return new Number(Math.Tan(val));
		}

		protected override Number Evaluate(double val)
		{
			return new Number(Math.Tan(val));
		}
	}
}
