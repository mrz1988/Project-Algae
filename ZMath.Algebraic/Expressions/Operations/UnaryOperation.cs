using System;
namespace ZMath.Algebraic
{
	public abstract class UnaryOperation : ISymbol
	{
		protected readonly ISymbol _child;
		public abstract SymbolType Type { get; }

		public UnaryOperation(ISymbol child)
		{
			_child = child;
		}

		protected abstract Number Evaluate(int val);
		protected abstract Number Evaluate(double val);
		public abstract ISymbol Copy();

		public Number GetValue()
		{
			var val = _child.GetValue();

			if (val.IsFloatingPt)
				return Evaluate(val.AsFloatingPt);

			return Evaluate(val.AsInt);
		}

		public bool CanEvaluate()
		{
			return _child.CanEvaluate();
		}

		public bool ChildEquals(ISymbol other)
		{
			return _child.Equals(other);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			UnaryOperation op = (UnaryOperation)obj;

			if (op.Type != Type)
				return false;

			return op.ChildEquals(_child);
		}

		public override int GetHashCode()
		{
			var hash = 27;
			hash = (hash * 17) + Type.GetHashCode();
			hash = (hash * 17) + _child.GetHashCode();

			return hash;
		}

		public static bool operator ==(UnaryOperation a, ISymbol b)
		{
			if (a == null)
				return b == null;
			
			return a.Equals(b);
		}

		public static bool operator !=(UnaryOperation a, ISymbol b)
		{
			return !(a == b);
		}
	}
}
