using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Constraints
{
    public static class SymbolConstraints
    {
        public static SymbolConstraint Any = new SymbolConstraint();
        public static SymbolConstraint None = new SymbolConstraint(
            s => { return false; });

        public static SymbolConstraint IsOne = new SymbolConstraint(s =>
            {
                if (s.Type != SymbolType.Number)
                    return false;
                return (s as Number) == new Number(1);
            }
        );

        public static SymbolConstraint IsZero = new SymbolConstraint(s =>
            {
                if (s.Type != SymbolType.Number)
                    return false;
                return (s as Number) == new Number(0);
            }
        );

        public static SymbolConstraint ReducibleConstant = new SymbolConstraint(
            s => { return s.CanEvaluate(); },
            new SymbolConstraint[] { });

        public static SymbolConstraint PureConstant =
            SymbolConstraint.IsOperation(SymbolType.Number);

        public static SymbolConstraint SingleVariable =
            SymbolConstraint.IsOperation(SymbolType.Variable);
    }
}
