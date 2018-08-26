using System;
using System.Collections.Generic;
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
        string ToString();
        string ToString(VariableContext ctx);
        List<SymbolToken> Tokenize();
    }
}
