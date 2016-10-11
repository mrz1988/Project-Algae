using System;
namespace ZMath.Algebraic
{
	public class Tangent : UnaryOperation
	{
		public Tangent(Number n) : base(n) { }

		protected override Number Evaluate(int val)
		{
			return new Number(Math.Tan(val));
		}

		protected override Number Evaluate(double val)
		{
			return new Number(Math.Tan(val));
		}
	}
}
