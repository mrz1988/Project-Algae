using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Constraints
{
    public static class SymbolicConstraints
    {
        public static BasicSymbolicConstraint Any = new BasicSymbolicConstraint();
        public static BasicSymbolicConstraint None = new BasicSymbolicConstraint(
            s => { return false; });

        public static BasicSymbolicConstraint IsOne = new BasicSymbolicConstraint(s =>
            {
                if (s.Type != SymbolType.Number)
                    return false;
                return (s as Number) == new Number(1);
            }
        );

        public static BasicSymbolicConstraint IsZero = new BasicSymbolicConstraint(s =>
            {
                if (s.Type != SymbolType.Number)
                    return false;
                return (s as Number) == new Number(0);
            }
        );

        public static BasicSymbolicConstraint ReducibleConstant = new BasicSymbolicConstraint(
            s => { return s.CanEvaluate(); },
            new BasicSymbolicConstraint[] { });

        public static BasicSymbolicConstraint PureConstant =
            BasicSymbolicConstraint.IsOperation(SymbolType.Number);

        public static BasicSymbolicConstraint SingleVariable =
            BasicSymbolicConstraint.IsOperation(SymbolType.Variable);
    }
}
