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

		public ISymbol Copy()
		{
			return new Variable(Name);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
