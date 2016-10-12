using System;
namespace ZMath.Algebraic
{
	public class Cosine : UnaryOperation
	{
		public Cosine(ISymbol n) : base(n) { }

		public override SymbolType Type { get { return SymbolType.Cosine; } }

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
