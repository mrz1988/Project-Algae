using System;
namespace ZMath.Algebraic
{
	namespace ZMath.Algebraic
	{
		public class Cosine : UnaryOperation
		{
			public Cosine(Number n) : base(n) { }

			protected override Number Evaluate(int val)
			{
				return new Number(Math.Cos(val));
			}

			protected override Number Evaluate(double val)
			{
				return new Number(Math.Cos(val));
			}
		}
	}
}
