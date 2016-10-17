using System;
namespace ZMath.Algebraic
{
	public class Variable : ISymbol
	{
		public SymbolType Type { get { return SymbolType.Variable; } }
		public string Name { get; private set; }

		public Variable(string name)
		{
			Name = name;
		}

		public Number GetValue()
		{
			throw new InvalidOperationException("Variables have no value");
		}

		public bool CanEvaluate()
		{
			return false;
		}

		public ISymbol Copy()
		{
			return new Variable(Name);
		}
	}
}
