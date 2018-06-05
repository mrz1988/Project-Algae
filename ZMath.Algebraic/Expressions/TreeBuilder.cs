using System;
using System.Collections.Generic;
using ZMath.Algebraic.Operations;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic
{
    public class TreeBuilder
    {
        private List<SymbolToken> _tokens;
        private VariableContext _ctx;

        public TreeBuilder(List<SymbolToken> tokens, VariableContext ctx)
        {
            _tokens = TrimParentheses(tokens);
            if (_tokens.Count == 0)
                throw new ArgumentException("missing token", nameof(tokens));
            
            if (_tokens.Count == 1 && !_tokens[0].Type.IsValue())
            {
                throw new ArgumentException(string.Format("hanging token: {0}", tokens[0].Token),
                    nameof(tokens));
            }

            _ctx = ctx;
        }

        public static List<SymbolToken> TrimParentheses(List<SymbolToken> tokens)
        {
            var len = tokens.Count;
            while (tokens.HasRedundantParentheses())
            {
                tokens = tokens.GetRange(1, len - 2);
            }

            return tokens;
        }

        public ISymbol ToValue(SymbolToken token)
        {
            if (token.Type != SymbolType.Number && token.Type != SymbolType.Variable)
                throw new ArgumentException("Not a number", nameof(token));

            if (token.Type == SymbolType.Variable)
                return _ctx.Get(token);

            var s = token.Token;
            if (!s.Contains("."))
            {
                var i = int.Parse(s);
                return new Number(i);
            }

            var f = double.Parse(s);
            return new Number(f);
        }

        public UnaryOperation ToUnaryOp(SymbolToken op, List<SymbolToken> parameter)
        {
            if (!op.Type.IsUnaryOperation())
                throw new ArgumentException("Not a unary operation", nameof(op));

            var paramTreeBuilder = new TreeBuilder(parameter, _ctx);
            var paramTree = paramTreeBuilder.Parse();

            switch (op.Type)
            {
                case SymbolType.Negation:
                    return new Negation(paramTree);
                case SymbolType.Sine:
                    return new Sine(paramTree);
                case SymbolType.Cosine:
                    return new Cosine(paramTree);
                case SymbolType.Tangent:
                    return new Tangent(paramTree);
            }

            throw new NotImplementedException(string.Format(
                "Support not written for unary operation: {0}", op.Type));
        }

        public ISymbol ToBinaryOp(List<SymbolToken> leftTokens, SymbolToken op, List<SymbolToken> rightTokens)
        {
            if (!op.Type.IsBinaryOperation())
                throw new ArgumentException("Not a binary operation", nameof(op));
            
            var leftParamTreeBuilder = new TreeBuilder(leftTokens, _ctx);
            var rightParamTreeBuilder = new TreeBuilder(rightTokens, _ctx);
            var leftParam = leftParamTreeBuilder.Parse();
            var rightParam = rightParamTreeBuilder.Parse();

            switch (op.Type)
            {
                case SymbolType.Addition:
                    return new Addition(leftParam, rightParam);
                case SymbolType.Subtraction:
                    var right = new Negation(rightParam);
                    return new Addition(leftParam, right);
                case SymbolType.Multiplication:
                    return new Multiplication(leftParam, rightParam);
                case SymbolType.Division:
                    return new Division(leftParam, rightParam);
                case SymbolType.Exponentiation:
                    return new Exponentiation(leftParam, rightParam);
            }

            throw new NotImplementedException(string.Format(
                "Support not written for binary operation: {0}", op.Type));
        }

        public ISymbol ParseAsOperation()
        {
            var parens = 0;
            var lowestIx = -1;
            var lowestOp = int.MaxValue;

            for (var i = 0; i < _tokens.Count; i++)
            {
                var token = _tokens[i];

                if (token.Type == SymbolType.OpenBracket)
                    parens++;
                else if (token.Type == SymbolType.CloseBracket)
                    parens--;

                if (parens < 0)
                    throw new InvalidOperationException("Malformed expression: too many close brackets");
                if (parens > 0)
                    continue; // skip inner parens, they're done later

                var order = token.Type.Order();

                // Must be <= to adhere to left-to-right (right has to be considered lower than left)
                if (order <= lowestOp)
                {
                    lowestIx = i;
                    lowestOp = order;
                }
            }

            if (lowestIx < 0)
                throw new InvalidOperationException("Malformed expression: missing operator");

            var op = _tokens[lowestIx];
            if (op.Type.IsUnaryOperation())
            {
                // If you see the below exception, an assumption was wrong.
                // It's assumed that all binary operations outside of parentheses
                // will be consumed first, leaving unary operations for last.
                // This would make the unary operation the first symbol.
                // Sorry, this means you probably have bugs to fix :(
                if (lowestIx != 0)
                    throw new InvalidOperationException("Unary operation seems to violate unary-first rule");

                var param = _tokens.GetRange(1, _tokens.Count - 1);
                return ToUnaryOp(op, param);
            }
            var leftSide = _tokens.GetRange(0, lowestIx);
            var rightSide = _tokens.GetRange(lowestIx + 1, _tokens.Count - lowestIx - 1);

            return ToBinaryOp(leftSide, op, rightSide);
        }

        public ISymbol Parse()
        {
            var len = _tokens.Count;

            // Case 1: Just a number or variable remaining
            if (len == 1)
                return ToValue(_tokens[0]);

            return ParseAsOperation();
        }
    }
}
