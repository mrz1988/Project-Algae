using System;
namespace ZMath.Algebraic
{
	public class Sine : UnaryOperation
	{
		public Sine(Number n) : base(n) { }

		protected override Number Evaluate(int val)
		{
			return new Number(Math.Sin(val));
		}

		protected override Number Evaluate(double val)
		{
			return new Number(Math.Sin(val));
		}
	}
}
