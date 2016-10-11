using System;
namespace ZMath.Algebraic
{
	public class Addition : BinaryOperation
	{
		public Addition(Number o1, Number o2) : base(o1, o2) { }

		protected override Number Evaluate(int left, int right)
		{
			return new Number(left + right);
		}

		protected override Number Evaluate(double left, double right)
		{
			return new Number(left + right);
		}
	}
}
