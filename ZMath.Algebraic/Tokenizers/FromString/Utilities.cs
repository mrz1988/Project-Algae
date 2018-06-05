using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMath.Algebraic
{
    public static class Utilities
    {
        public static bool HasRedundantParentheses(this IList<SymbolToken> tokens)
        {
            if (tokens[0].Type != SymbolType.OpenBracket ||
                    tokens[tokens.Count - 1].Type != SymbolType.CloseBracket)
                return false;

            var parenCount = 0;
            for (int i = 1; i < tokens.Count - 1; i++)
            {
                var val = tokens[i];
                if (val.Type == SymbolType.OpenBracket)
                    parenCount++;
                if (val.Type == SymbolType.CloseBracket)
                    parenCount--;

                if (parenCount < 0)
                    return false;
            }

            return true;
        }
    }
}
