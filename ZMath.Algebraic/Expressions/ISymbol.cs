using System;
namespace ZMath.Algebraic
{
	public interface ISymbol
	{
		Number GetValue();
		SymbolType Type { get; }
		ISymbol Copy();
		bool CanEvaluate();
	}
}
