using System;
using ZMath.Algebraic.Values;
using ZMath.Algebraic.Constraints;

namespace ZMath.Algebraic.Operations
{
    public abstract class UnaryOperation : ISymbol
    {
        protected readonly ISymbol _child;
        public abstract SymbolType Type { get; }

        public UnaryOperation(ISymbol child)
        {
            _child = child;
        }

        protected abstract Number Evaluate(int val);
        protected abstract Number Evaluate(double val);
        public abstract ISymbol Copy();

        public static ISymbol FromValues(SymbolType type, ISymbol child)
        {
            switch (type)
            {
                case SymbolType.Sine:
                    return new Sine(child);
                case SymbolType.Cosine:
                    return new Cosine(child);
                case SymbolType.Tangent:
                    return new Tangent(child);
                case SymbolType.Negation:
                    return new Negation(child);
                default:
                    throw new ArgumentException("Not a valid unary operation", nameof(type));
            }
        }

        public ISymbol MakeSubstitutions(VariableContext ctx)
        {
            var child = _child.MakeSubstitutions(ctx);
            return FromValues(Type, child);
        }

        public Number GetValue()
        {
            var val = _child.GetValue();

            if (val.IsFloatingPt)
                return Evaluate(val.AsFloatingPt);

            return Evaluate(val.AsInt);
        }

        public bool CanEvaluate()
        {
            return _child.CanEvaluate();
        }

        public ISymbol Reduce()
        {
            if (CanEvaluate())
                return GetValue();

            return FromValues(Type, _child.Reduce());
        }

        public bool Matches(SymbolConstraint constraint)
        {
            if (!constraint.BaseNodeIsValid(this))
                return false;

            if (constraint.Left != null && !_child.Matches(constraint.Left))
                return false;

            // If for some reason this was seeking a binary+ operation let's not match
            return constraint.ChildConstraints.Length <= 1;
        }

        public bool ChildEquals(ISymbol other)
        {
            return _child.Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || GetType() != obj.GetType())
                return false;

            UnaryOperation op = (UnaryOperation)obj;

            if (op.Type != Type)
                return false;

            return op.ChildEquals(_child);
        }

        public override int GetHashCode()
        {
            var hash = 27;
            hash = (hash * 17) + Type.GetHashCode();
            hash = (hash * 17) + _child.GetHashCode();

            return hash;
        }

        public static bool operator ==(UnaryOperation a, ISymbol b)
        {
            if (ReferenceEquals(a, null))
                return ReferenceEquals(b, null);
            
            return a.Equals(b);
        }

        public static bool operator !=(UnaryOperation a, ISymbol b)
        {
            return !(a == b);
        }
    }
}
