using System;
namespace ZMath.Algebraic
{
	public class SymbolTree
	{
		private ISymbol _root;

		public SymbolTree(ISymbol root)
		{
			_root = root;
		}

		public Number Evaluate()
		{
			return _root.GetValue();
		}
	}
}
