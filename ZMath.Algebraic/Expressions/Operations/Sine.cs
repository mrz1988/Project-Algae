using System;
namespace ZMath.Algebraic
{
	public class Sine : UnaryOperation
	{
		public Sine(ISymbol n) : base(n) { }

		public override SymbolType Type { get { return SymbolType.Sine; } }

		public override ISymbol Copy()
		{
			return new Sine(_child.Copy());
		}

		protected override Number Evaluate(int val)
		{
			return new Number(Math.Sin(val));
		}

		protected override Number Evaluate(double val)
		{
			return new Number(Math.Sin(val));
		}
	}
}
