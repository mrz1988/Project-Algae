using System;
using ZMath.Algebraic.Values;
using ZMath.Algebraic.Constraints;

namespace ZMath.Algebraic.Operations
{
    public abstract class BinaryOperation : ISymbol
    {
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

        public bool Matches(SymbolConstraint constraint)
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

            BinaryOperation op = (BinaryOperation)obj;

            if (op.Type != Type)
                return false;

            return op.LeftEquals(Operand1) && op.RightEquals(Operand2);
        }

        public override int GetHashCode()
        {
            var hash = 27;
            hash = (hash * 17) + Type.GetHashCode();
            hash = (hash * 17) + Operand1.GetHashCode();
            hash = (hash * 17) + Operand2.GetHashCode();

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
