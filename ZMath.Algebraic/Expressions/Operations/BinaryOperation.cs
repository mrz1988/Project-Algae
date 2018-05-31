using System;
using ZMath.Algebraic.Values;
using ZMath.Algebraic.Constraints;

namespace ZMath.Algebraic.Operations
{
    public abstract class BinaryOperation : ISymbol
    {
        protected readonly ISymbol _operand1;
        protected readonly ISymbol _operand2;

        public abstract SymbolType Type { get; }

        public BinaryOperation(ISymbol operand1, ISymbol operand2)
        {
            _operand1 = operand1;
            _operand2 = operand2;
        }

        protected abstract Number Evaluate(double left, double right);
        protected abstract Number Evaluate(int left, int right);
        public abstract ISymbol Copy();

        public Number GetValue()
        {
            var left = _operand1.GetValue();
            var right = _operand2.GetValue();

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
            return _operand1.CanEvaluate() && _operand2.CanEvaluate();
        }

        public bool Matches(SymbolConstraint constraint)
        {
            if (!constraint.BaseNodeIsValid(this))
                return false;

            if (constraint.Left != null && !_operand1.Matches(constraint.Left))
                return false;

            if (constraint.Right != null && !_operand2.Matches(constraint.Right))
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

        public ISymbol MakeSubstitutions(VariableContext ctx)
        {
            var o1 = _operand1.MakeSubstitutions(ctx);
            var o2 = _operand2.MakeSubstitutions(ctx);

            return FromValues(Type, o1, o2);
        }

        public bool LeftEquals(ISymbol other)
        {
            return _operand1.Equals(other);
        }

        public bool RightEquals(ISymbol other)
        {
            return _operand2.Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || GetType() != obj.GetType())
                return false;

            BinaryOperation op = (BinaryOperation)obj;

            if (op.Type != Type)
                return false;

            return op.LeftEquals(_operand1) && op.RightEquals(_operand2);
        }

        public override int GetHashCode()
        {
            var hash = 27;
            hash = (hash * 17) + Type.GetHashCode();
            hash = (hash * 17) + _operand1.GetHashCode();
            hash = (hash * 17) + _operand2.GetHashCode();

            return hash;
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
