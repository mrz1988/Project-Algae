﻿using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Operations
{
    public class Negation : UnaryOperation
    {
        public Negation(ISymbol n) : base(n) { }

        public override SymbolType Type { get { return SymbolType.Negation; } }

        public override ISymbol Copy()
        {
            return new Negation(Child.Copy());
        }

        protected override Number Evaluate(int val)
        {
            return new Number(-val);
        }

        protected override Number Evaluate(double val)
        {
            return new Number(-val);
        }
    }
}
