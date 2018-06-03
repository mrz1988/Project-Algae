using System;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Operations
{
    public class Exponentiation : BinaryOperation
    {
        public Exponentiation(ISymbol o1, ISymbol o2) : base (o1, o2) { }

        public override SymbolType Type { get { return SymbolType.Exponentiation; } }

        public override ISymbol Copy()
        {
            return new Exponentiation(Operand1.Copy(), Operand2.Copy());
        }

        protected override Number Evaluate(int left, int right)
        {
            return new Number(Math.Pow(left, right));
        }

        protected override Number Evaluate(double left, double right)
        {
            return new Number(Math.Pow(left, right));
        }
    }
}
