using System;
namespace ZMath.Algebraic
{
	public abstract class BinaryOperation : ISymbol
	{
		protected ISymbol _operand1;
		protected ISymbol _operand2;

		public abstract SymbolType Type { get; }

		public BinaryOperation(ISymbol operand1, ISymbol operand2)
		{
			_operand1 = operand1;
			_operand2 = operand2;
		}

		protected abstract Number Evaluate(double left, double right);
		protected abstract Number Evaluate(int left, int right);

		public Number GetValue()
		{
			var left = _operand1.GetValue();
			var right = _operand2.GetValue();

			if (left.IsFloatingPt || right.IsFloatingPt)
			{
				var leftval = left.AsFloatingPt;
				var rightval = right.AsFloatingPt;

				return Evaluate(leftval, rightval);
			}

			return Evaluate(left.AsInt, right.AsInt);
		}
	}
}
