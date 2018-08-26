using System;
using System.Collections.Generic;
using ZMath.Algebraic.Values;
using ZMath.Algebraic.Constraints;

namespace ZMath.Algebraic.Operations
{
    public abstract class UnaryOperation : ISymbol
    {
        private int? _hash = null;

        public readonly ISymbol Child;
        public abstract SymbolType Type { get; }

        public UnaryOperation(ISymbol child)
        {
            Child = child;
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

        public ISymbol ReplaceChild(ISymbol newChild)
        {
            return FromValues(Type, newChild);
        }

        public ISymbol MakeSubstitutions(VariableContext ctx)
        {
            var child = Child.MakeSubstitutions(ctx);
            return FromValues(Type, child);
        }

        public Number GetValue()
        {
            var val = Child.GetValue();

            if (val.IsFloatingPt)
                return Evaluate(val.AsFloatingPt);

            return Evaluate(val.AsInt);
        }

        public bool CanEvaluate()
        {
            return Child.CanEvaluate();
        }

        public ISymbol Reduce()
        {
            if (CanEvaluate())
                return GetValue();

            return FromValues(Type, Child.Reduce());
        }

        public bool Matches(BasicSymbolicConstraint constraint)
        {
            if (!constraint.BaseNodeIsValid(this))
                return false;

            if (constraint.Left != null && !Child.Matches(constraint.Left))
                return false;

            // If for some reason this was seeking a binary+ operation let's not match
            return constraint.ChildConstraints.Length <= 1;
        }

        public bool ChildEquals(ISymbol other)
        {
            return Child.Equals(other);
        }

        public bool Equals(ISymbol other)
        {
            if (ReferenceEquals(other, null) || GetType() != other.GetType())
                return false;

            return GetHashCode() == other.GetHashCode();
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
            hash = (hash * 17) + Child.GetHashCode();
            _hash = hash;

            return hash;
        }

        public override string ToString()
        {
            var symbol = SymbolToken.OperatorStringOf(Type);
            return $"{symbol}({Child.ToString()})";
        }

        public string ToString(VariableContext ctx)
        {
            return ToString();
        }

        public virtual List<SymbolToken> Tokenize()
        {
            var opToken = new SymbolToken(Type, SymbolToken.OperatorStringOf(Type));
            var tokens = new List<SymbolToken>();
            tokens.Add(opToken);
            tokens.Add(SymbolToken.OpenBracket);
            tokens.AddRange(Child.Tokenize());
            tokens.Add(SymbolToken.CloseBracket);

            return tokens;
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
