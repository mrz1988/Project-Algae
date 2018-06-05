using System;

namespace ZMath.Algebraic.Constraints
{
    public class BasicSymbolicConstraint
    {
        protected readonly Predicate<ISymbol> NodeEvaluator;
        public readonly BasicSymbolicConstraint[] ChildConstraints;

        public BasicSymbolicConstraint Left => ChildConstraints.Length > 0 ? ChildConstraints[0] : null;
        public BasicSymbolicConstraint Right => ChildConstraints.Length > 1 ? ChildConstraints[1] : null;

        public BasicSymbolicConstraint() : this(_ => { return true; }) { }
        public BasicSymbolicConstraint(Predicate<ISymbol> nodeEvaluator)
            : this(nodeEvaluator, new BasicSymbolicConstraint[] { }) { }
        public BasicSymbolicConstraint(Predicate<ISymbol> nodeEvaluator, BasicSymbolicConstraint[] childConstraints)
        {
            NodeEvaluator = nodeEvaluator;
            ChildConstraints = childConstraints;
        }

        public static BasicSymbolicConstraint IsOperation(SymbolType operationType)
        {
            return new BasicSymbolicConstraint(s => { return s.Type == operationType; });
        }

        public static BasicSymbolicConstraint IsOperation(SymbolType operationType, BasicSymbolicConstraint[] childConstraints)
        {
            return new BasicSymbolicConstraint(
                s => { return s.Type == operationType; }, childConstraints);
        }

        public virtual bool BaseNodeIsValid(ISymbol symbol)
        {
            return NodeEvaluator(symbol);
        }

        public virtual void Reset() { }
    }
}
