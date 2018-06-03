using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Operations
{
    public class Addition : BinaryOperation
    {
        public Addition(ISymbol o1, ISymbol o2) : base(o1, o2) { }

        public override SymbolType Type { get { return SymbolType.Addition; } }

        public override ISymbol Copy()
        {
            return new Addition(Operand1.Copy(), Operand2.Copy());
        }

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
