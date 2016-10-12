using System;
namespace ZMath.Algebraic
{
	public abstract class UnaryOperation : ISymbol
	{
		protected ISymbol _child;
		public abstract SymbolType Type { get; }

		public UnaryOperation(ISymbol child)
		{
			_child = child;
		}

		protected abstract Number Evaluate(int val);
		protected abstract Number Evaluate(double val);

		public Number GetValue()
		{
			var val = _child.GetValue();

			if (val.IsFloatingPt)
				return Evaluate(val.AsFloatingPt);

			return Evaluate(val.AsInt);
		}
	}
}
