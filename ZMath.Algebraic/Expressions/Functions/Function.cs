﻿using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Functions
{
	public class Function
	{
		protected ISymbol _root;
		protected VariableContext _ctx;

		public Function(ISymbol root, VariableContext ctx)
		{
			_root = root;
			_ctx = ctx;
		}

		public void Substitute(string variableName, ISymbol substitution)
		{
			_ctx.Define(variableName, substitution);
		}

		public Number Evaluate()
		{
			return _root.MakeSubstitutions(_ctx).GetValue();
		}
	}
}
