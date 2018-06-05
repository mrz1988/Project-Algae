using System;
using ZMath.Algebraic.Constraints;

namespace ZMath.Algebraic.Values
{
    public class Variable : ISymbol
    {
        private int? _hash = null;

        public SymbolType Type { get { return SymbolType.Variable; } }
        public string Name { get; private set; }

        public Variable(string name)
        {
            Name = name;
        }

        public ISymbol MakeSubstitutions(VariableContext ctx)
        {
            return ctx.Get(Name);
        }

        public Number GetValue()
        {
            throw new InvalidOperationException("Variables have no value");
        }

        public bool CanEvaluate()
        {
            return false;
        }

        public bool Matches(BasicSymbolicConstraint constraint)
        {
            if (!constraint.BaseNodeIsValid(this))
                return false;

            // No children, there better be no child constraints :D
            return constraint.ChildConstraints.Length == 0;
        }

        public ISymbol Reduce()
        {
            return this;
        }

        public ISymbol Copy()
        {
            return new Variable(Name);
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(ISymbol other)
        {
            if (other == null || GetType() != other.GetType())
                return false;

            return GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            if (_hash != null)
                return _hash.Value;

            _hash = Name.GetHashCode();
            return _hash.Value;
        }

        public static bool operator ==(Variable a, Variable b)
        {
            if (ReferenceEquals(a, null))
                return ReferenceEquals(b, null);

            return a.Equals(b);
        }

        public static bool operator !=(Variable a, Variable b)
        {
            return !(a == b);
        }
    }
}
