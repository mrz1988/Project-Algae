using System;
namespace ZMath.Algebraic
{
	public class Negation : UnaryOperation
	{
		public Negation(ISymbol n) : base(n) { }

		public override SymbolType Type { get { return SymbolType.Negation; } }

		protected override Number Evaluate(int val)
		{
			return new Number(-val);
		}

		protected override Number Evaluate(double val)
		{
			return new Number(-val);
		}
	}
}
