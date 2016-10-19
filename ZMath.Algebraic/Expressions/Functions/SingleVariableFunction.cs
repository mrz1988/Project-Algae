using System;
using System.Collections.Generic;

namespace ZMath.Algebraic
{
	public class SingleVariableFunction : Function
	{
		public string VariableName { get; private set; }
		public SingleVariableFunction(ISymbol root, VariableContext ctx) : base(root, ctx) { }
		public SingleVariableFunction(ISymbol root, string variableName)
			: base(root, VariableContext.FromVariableNames(new List<string> { variableName }))
		{
			VariableName = variableName;
		}

		public static SingleVariableFunction FromString(string expression, string variableName)
		{
			var ctx = VariableContext.FromVariableNames(new List<string> { variableName } );
			var root = StringTokenizer.BuildTreeFrom(expression, ctx);
			return new SingleVariableFunction(root, ctx);
		}

		public ISymbol Call(double val)
		{
			return Call(new Number(val));
		}

		public ISymbol Call(int val)
		{
			return Call(new Number(val));
		}

		public ISymbol Call(Number val)
		{
			Substitute(val);
			return Evaluate();
		}

		public void Substitute(Number val)
		{
			Substitute(VariableName, val);
		}
	}
}
