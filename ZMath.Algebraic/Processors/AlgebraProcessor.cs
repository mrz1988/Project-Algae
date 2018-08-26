using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMath.Algebraic.Values;
using ZMath.Algebraic.Functions;
using ZMath.Algebraic.Transforms;

namespace ZMath.Algebraic
{
    public class AlgebraProcessor
    {
        // This whole class should probably just get moved to the CLI proj or figure out its exact role. I'm not
        // sure what this is supposed to do differently than "Take String -> Return Thing". Perhaps it should
        // still exist here but be a lot more string-focused, then rely on a REPL to implement individual
        // commands that map onto this. That way this can also have some other client that doesn't use a CLI
        // that can run on a similar set of commands that are executed from a different spot?
        private VariableContext _variableContext;
        private Dictionary<string, Function> _functions;
        private TransformSet _algebraReduce = SymbolicTransform.MoveComplexSubtreesRight.CombineWith(SymbolicTransform.AlgebraicReduce);

        public AlgebraProcessor(VariableContext context = null)
        {
            _variableContext = context ?? VariableContext.Default;
        }

        public ISymbol Reduce(ISymbol expression)
        {
            return _algebraReduce.Transform(expression);
        }

        public string Reduce(string expression)
        {
            var exp = StringTokenizer.ToExpression(expression, _variableContext);
            var reduced = Reduce(exp);
            return StringTokenizer.FromExpression(reduced);
        }

        public void AddVariable(string name)
        {
            _variableContext.Register(name, new Variable(name));
        }

        public void AddFunction(string name, ISymbol expression, IEnumerable<string> variables)
        {
        }
    }
}
