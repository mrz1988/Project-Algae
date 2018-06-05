using System;
using System.Collections.Generic;

namespace ZMath.Algebraic
{
    public class StringTokenizer
    {
        public static List<SymbolToken> Parse(string expression)
        {
            return Parse(expression, VariableContext.ConstantsOnly);
        }

        public static List<SymbolToken> Parse(string expression, VariableContext context)
        {
            var pipe1 = new StringToPrimitiveTokenPipe(expression, context);
            var pipe2 = new MatchAllParenthesesProcessor(pipe1);
            var pipe3 = new NegationProcessor(pipe2);
            var pipe4 = new RedundantParenthesesProcessor(pipe3);
            var pipe5 = new TokenValidater(pipe4);
            return pipe5.PumpAll();
        }

        public static ISymbol ToExpression(string expression)
        {
            return ToExpression(expression, VariableContext.ConstantsOnly);
        }

        public static ISymbol ToExpression(string expression, VariableContext context)
        {
            var tokens = Parse(expression, context);
            var tb = new TreeBuilder(tokens, context);
            return tb.Parse();
        }
    }
}
