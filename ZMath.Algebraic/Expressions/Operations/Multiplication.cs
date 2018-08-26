using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Operations
{
    public class Multiplication : BinaryOperation
    {
        public Multiplication(ISymbol o1, ISymbol o2) : base (o1, o2) { }

        public override SymbolType Type { get { return SymbolType.Multiplication; } }

        public override ISymbol Copy()
        {
            return new Multiplication(Operand1.Copy(), Operand2.Copy());
        }

        protected override Number Evaluate(int left, int right)
        {
            return new Number(left * right);
        }

        protected override Number Evaluate(double left, double right)
        {
            return new Number(left * right);
        }

        public override string ToString()
        {
            var symbol = SymbolToken.OperatorStringOf(Type);
            var left = Operand1.ToString();
            if (Operand1.Type.Order() < Type.Order())
                left = $"({left})";

            var right = Operand2.ToString();
            if (Operand2.Type.Order() < Type.Order())
                right = $"({right})";

            // we reverse here since we want the more complex stuff on the left.
            // our transforms tend to push them to the right, and we can do this
            // since multiplication is commutative.
            return $"{right} {symbol} {left}";
        }
    }
}
