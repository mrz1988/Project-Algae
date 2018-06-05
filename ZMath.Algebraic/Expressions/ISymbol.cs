using System;
using ZMath.Algebraic.Constraints;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic
{
    public interface ISymbol : IEquatable<ISymbol>
    {
        Number GetValue();
        SymbolType Type { get; }
        ISymbol Copy();
        ISymbol Reduce();
        bool CanEvaluate();
        bool Matches(BasicSymbolicConstraint constraint);
        ISymbol MakeSubstitutions(VariableContext ctx);
    }
}
