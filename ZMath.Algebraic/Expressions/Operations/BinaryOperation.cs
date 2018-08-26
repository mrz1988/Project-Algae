using System;
using System.Collections.Generic;
using ZMath.Algebraic.Values;
using ZMath.Algebraic.Constraints;

namespace ZMath.Algebraic.Operations
{
    public abstract class BinaryOperation : ISymbol
    {
        private int? _hash = null;

        public readonly ISymbol Operand1;
        public readonly ISymbol Operand2;

        public abstract SymbolType Type { get; }

        public BinaryOperation(ISymbol operand1, ISymbol operand2)
        {
            Operand1 = operand1;
            Operand2 = operand2;
        }

        protected abstract Number Evaluate(double left, double right);
        protected abstract Number Evaluate(int left, int right);
        public abstract ISymbol Copy();

        public Number GetValue()
        {
            var left = Operand1.GetValue();
            var right = Operand2.GetValue();

            if (left.IsFloatingPt || right.IsFloatingPt)
            {
                var leftval = left.AsFloatingPt;
                var rightval = right.AsFloatingPt;

                return Evaluate(leftval, rightval);
            }

            return Evaluate(left.AsInt, right.AsInt);
        }

        public bool CanEvaluate()
        {
            return Operand1.CanEvaluate() && Operand2.CanEvaluate();
        }

        public bool Matches(BasicSymbolicConstraint constraint)
        {
            if (!constraint.BaseNodeIsValid(this))
                return false;

            if (constraint.Left != null && !Operand1.Matches(constraint.Left))
                return false;

            if (constraint.Right != null && !Operand2.Matches(constraint.Right))
                return false;

            // If for some reason this was seeking a ternary+ operation let's not match
            return constraint.ChildConstraints.Length <= 2;
        }

        public static ISymbol FromValues(SymbolType type, ISymbol operand1, ISymbol operand2)
        {
            switch (type)
            {
                case SymbolType.Addition:
                    return new Addition(operand1, operand2);
                case SymbolType.Multiplication:
                    return new Multiplication(operand1, operand2);
                case SymbolType.Division:
                    return new Division(operand1, operand2);
                case SymbolType.Exponentiation:
                    return new Exponentiation(operand1, operand2);
                default:
                    throw new ArgumentException("Not a valid binary symbol type", nameof(type));
            }
        }

        public ISymbol ReplaceOperand1(ISymbol newOperand)
        {
            return FromValues(Type, newOperand, Operand2);
        }

        public ISymbol ReplaceOperand2(ISymbol newOperand)
        {
            return FromValues(Type, Operand1, newOperand);
        }

        public ISymbol MakeSubstitutions(VariableContext ctx)
        {
            var o1 = Operand1.MakeSubstitutions(ctx);
            var o2 = Operand2.MakeSubstitutions(ctx);

            return FromValues(Type, o1, o2);
        }

        public ISymbol Reduce()
        {
            if (CanEvaluate())
                return GetValue();

            return FromValues(Type, Operand1.Reduce(), Operand2.Reduce());
        }

        public bool LeftEquals(ISymbol other)
        {
            return Operand1.Equals(other);
        }

        public bool RightEquals(ISymbol other)
        {
            return Operand2.Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || GetType() != obj.GetType())
                return false;

            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            if (_hash != null)
                return _hash.Value;

            var hash = 27;
            hash = (hash * 17) + Type.GetHashCode();
            hash = (hash * 17) + Operand1.GetHashCode();
            hash = (hash * 17) + Operand2.GetHashCode();

            _hash = hash;
            return hash;
        }

        public bool Equals(ISymbol other)
        {
            if (ReferenceEquals(other, null) || GetType() != other.GetType())
                return false;

            return GetHashCode() == other.GetHashCode();
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

            return $"{left} {symbol} {right}";
        }

        public string ToString(VariableContext ctx)
        {
            return ToString();
        }

        public virtual List<SymbolToken> Tokenize()
        {
            var opToken = new SymbolToken(Type, SymbolToken.OperatorStringOf(Type));
            var tokens = new List<SymbolToken>();

            if (Operand1.Type.Order() < Type.Order())
                tokens.Add(SymbolToken.OpenBracket);
            tokens.AddRange(Operand1.Tokenize());
            if (Operand1.Type.Order() < Type.Order())
                tokens.Add(SymbolToken.CloseBracket);

            tokens.Add(opToken);

            if (Operand2.Type.Order() < Type.Order())
                tokens.Add(SymbolToken.OpenBracket);
            tokens.AddRange(Operand2.Tokenize());
            if (Operand2.Type.Order() < Type.Order())
                tokens.Add(SymbolToken.CloseBracket);

            return tokens;
        }

        public static bool operator ==(BinaryOperation a, ISymbol b)
        {
            if (ReferenceEquals(a, null))
                return ReferenceEquals(b, null);
            
            return a.Equals(b);
        }

        public static bool operator !=(BinaryOperation a, ISymbol b)
        {
            return !(a == b);
        }
    }
}
