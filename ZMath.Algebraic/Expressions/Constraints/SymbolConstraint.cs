using System;

namespace ZMath.Algebraic.Constraints
{
    public class SymbolConstraint
    {
        public readonly Predicate<ISymbol> BaseNodeIsValid;
        public readonly SymbolConstraint[] ChildConstraints;
        public readonly bool EnforceChildCount;

        public SymbolConstraint Left => ChildConstraints.Length > 0 ? ChildConstraints[0] : null;
        public SymbolConstraint Right => ChildConstraints.Length > 1 ? ChildConstraints[1] : null;

        public SymbolConstraint() : this(_ => { return true; }) { }
        public SymbolConstraint(Predicate<ISymbol> nodeEvaluator)
            : this(nodeEvaluator, new SymbolConstraint[] { }) { }
        public SymbolConstraint(Predicate<ISymbol> nodeEvaluator, SymbolConstraint[] childConstraints)
        {
            BaseNodeIsValid = nodeEvaluator;
            ChildConstraints = childConstraints;
        }

        public static SymbolConstraint IsOperation(SymbolType operationType)
        {
            return new SymbolConstraint(s => { return s.Type == operationType; });
        }

        public static SymbolConstraint IsOperation(SymbolType operationType, SymbolConstraint[] childConstraints)
        {
            return new SymbolConstraint(
                s => { return s.Type == operationType; }, childConstraints);
        }
    }
}
