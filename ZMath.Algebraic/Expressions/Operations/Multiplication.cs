using System;
namespace ZMath.Algebraic
{
	public class Multiplication : BinaryOperation
	{
		public Multiplication(ISymbol o1, ISymbol o2) : base (o1, o2) { }

		public override SymbolType Type { get { return SymbolType.Multiplication; } }

		protected override Number Evaluate(int left, int right)
		{
			return new Number(left * right);
		}

		protected override Number Evaluate(double left, double right)
		{
			return new Number(left * right);
		}
	}
}
