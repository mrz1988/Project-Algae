using System;
namespace ZMath.Algebraic
{
	public abstract class BinaryOperation : ISymbol
	{
		protected readonly ISymbol _operand1;
		protected readonly ISymbol _operand2;

		public abstract SymbolType Type { get; }

		public BinaryOperation(ISymbol operand1, ISymbol operand2)
		{
			_operand1 = operand1;
			_operand2 = operand2;
		}

		protected abstract Number Evaluate(double left, double right);
		protected abstract Number Evaluate(int left, int right);
		public abstract ISymbol Copy();

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

		public bool LeftEquals(ISymbol other)
		{
			return _operand1.Equals(other);
		}

		public bool RightEquals(ISymbol other)
		{
			return _operand2.Equals(other);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			BinaryOperation op = (BinaryOperation)obj;

			if (op.Type != Type)
				return false;

			return op.LeftEquals(_operand1) && op.RightEquals(_operand2);
		}

		public override int GetHashCode()
		{
			var hash = 27;
			hash = (hash * 17) + Type.GetHashCode();
			hash = (hash * 17) + _operand1.GetHashCode();
			hash = (hash * 17) + _operand2.GetHashCode();

			return hash;
		}

		public static bool operator ==(BinaryOperation a, ISymbol b)
		{
			if (a == null)
				return b == null;
			
			return a.Equals(b);
		}

		public static bool operator !=(BinaryOperation a, ISymbol b)
		{
			return !(a == b);
		}
	}
}
