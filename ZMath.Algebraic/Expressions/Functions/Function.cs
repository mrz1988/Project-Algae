using ZMath.Algebraic.Values;

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

        public void SubstituteFrom(VariableContext context)
        {
            _ctx.DefineFrom(context);
        }

        public void ClearSubstitutions()
        {
            _ctx.UndefineAll();
        }

        public Number Evaluate()
        {
            return _root.MakeSubstitutions(_ctx).GetValue();
        }

        public ISymbol ToExpression()
        {
            return _root.MakeSubstitutions(_ctx);
        }

        public bool ContainsVariables(VariableContext variables)
        {
            return variables.IsSubsetOf(_ctx);
        }
    }
}
