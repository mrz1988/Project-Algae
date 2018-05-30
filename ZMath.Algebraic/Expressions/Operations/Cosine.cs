using System;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Operations
{
	public class Cosine : UnaryOperation
	{
		public Cosine(ISymbol n) : base(n) { }

		public override SymbolType Type { get { return SymbolType.Cosine; } }

		public override ISymbol Copy()
		{
			return new Cosine(_child.Copy());
		}

		protected override Number Evaluate(int val)
		{
			return new Number(Math.Cos(val));
		}

		protected override Number Evaluate(double val)
		{
			return new Number(Math.Cos(val));
		}
	}
}
