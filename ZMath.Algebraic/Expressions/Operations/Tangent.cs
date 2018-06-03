using System;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Operations
{
    public class Tangent : UnaryOperation
    {
        public Tangent(ISymbol n) : base(n) { }

        public override SymbolType Type { get { return SymbolType.Tangent; } }

        public override ISymbol Copy()
        {
            return new Tangent(Child.Copy());
        }

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
