using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Operations
{
	public class Multiplication : BinaryOperation
	{
		public Multiplication(ISymbol o1, ISymbol o2) : base (o1, o2) { }

		public override SymbolType Type { get { return SymbolType.Multiplication; } }

		public override ISymbol Copy()
		{
			return new Multiplication(_operand1.Copy(), _operand2.Copy());
		}

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
