using System.Collections.Generic;
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

        public override string ToString()
        {
            // In an ideal world we would convert this into subtraction when
            // applicable. This is the lazy way without doing re-tokenization.
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

        public override List<SymbolToken> Tokenize()
        {
            // TODO: It's probably better to have a "prettify" tree transform
            // that always runs before tokenizing an expression rather than
            // manually manipulating stuff here...
            var leftNegated = Operand1.Type == SymbolType.Negation;

            var tokens = new List<SymbolToken>();

            if (leftNegated)
            {
                tokens.AddRange(Operand2.Tokenize());
                tokens.Add(new SymbolToken(SymbolType.Subtraction, "-"));
                if (Operand1.Type.IsValue())
                {
                    tokens.AddRange(Operand1.Tokenize());
                }
                else
                {
                    tokens.Add(SymbolToken.OpenBracket);
                    tokens.AddRange(Operand1.Tokenize());
                    tokens.Add(SymbolToken.CloseBracket);
                }

                return tokens;
            }

            // Reverses addition (commutative -- See TODO above)
            tokens.AddRange(Operand2.Tokenize());
            tokens.Add(new SymbolToken(Type, SymbolToken.OperatorStringOf(Type)));
            tokens.AddRange(Operand1.Tokenize());
            return tokens;
        }
    }
}
