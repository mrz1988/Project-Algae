using System;
namespace ZMath.Algebraic
{
	public class Function
	{
		private ISymbol _root;
		private VariableContext _ctx;

		public Function(ISymbol root, VariableContext ctx)
		{
			_root = root;
			_ctx = ctx;
		}

		public void Substitute(string variableName, Number substitution)
		{
			_ctx.Define(variableName, substitution);
		}

		public Number Evaluate()
		{
			return _root.MakeSubstitutions(_ctx).GetValue();
		}
	}
}
