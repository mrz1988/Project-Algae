using System;
using System.Collections.Generic;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Functions
{
    public class SingleVariableFunction : Function
    {
        public string VariableName { get; private set; }
        public SingleVariableFunction(ISymbol root, string variableName)
            : base(root, VariableContext.FromVariableNames(new List<string> { variableName }))
        {
            VariableName = variableName;
        }

        public static SingleVariableFunction FromString(string expression, string variableName)
        {
            var ctx = VariableContext.FromVariableNames(new List<string> { variableName } );
            var root = StringTokenizer.ToExpression(expression, ctx);
            return new SingleVariableFunction(root, variableName);
        }

        public Number Call(double val)
        {
            return Call(new Number(val));
        }

        public Number Call(int val)
        {
            return Call(new Number(val));
        }

        public Number Call(Number val)
        {
            Substitute(val);
            var result = Evaluate();
            if (result.Type != SymbolType.Number)
                throw new EvaluationFailureException("Function did not reduce to a number");

            return (Number)result;
        }

        public void Substitute(Number val)
        {
            Substitute(VariableName, val);
        }
    }
}
