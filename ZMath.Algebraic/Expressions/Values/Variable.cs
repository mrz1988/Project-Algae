using System;
using ZMath.Algebraic.Constraints;

namespace ZMath.Algebraic.Values
{
    public class Variable : ISymbol
    {
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

        public bool Matches(SymbolConstraint constraint)
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Variable v = (Variable)obj;

            return v.Type == Type && v.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(Variable a, Variable b)
        {
            if (a == null)
                return b == null;

            return a.Equals(b);
        }

        public static bool operator !=(Variable a, Variable b)
        {
            return !(a == b);
        }
    }
}
