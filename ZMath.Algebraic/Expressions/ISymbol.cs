﻿using ZMath.Algebraic.Constraints;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic
{
    public interface ISymbol
    {
        Number GetValue();
        SymbolType Type { get; }
        ISymbol Copy();
        ISymbol Reduce();
        bool CanEvaluate();
        bool Matches(SymbolConstraint constraint);
        ISymbol MakeSubstitutions(VariableContext ctx);
    }
}
