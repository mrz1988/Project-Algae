using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Operations
{
    public class Division : BinaryOperation
    {
        public Division(ISymbol o1, ISymbol o2) : base (o1, o2) { }

        public override SymbolType Type { get { return SymbolType.Division; } }

        public override ISymbol Copy()
        {
            return new Division(_operand1.Copy(), _operand2.Copy());
        }

        protected override Number Evaluate(int left, int right)
        {
            return Evaluate((double)left, (double)right);
        }

        protected override Number Evaluate(double left, double right)
        {
            return new Number(left / right);
        }
    }
}
