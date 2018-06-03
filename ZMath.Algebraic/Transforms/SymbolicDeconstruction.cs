using System;
using System.Collections.Generic;
using ZMath.Algebraic.Operations;

namespace ZMath.Algebraic.Transforms
{
    public class SymbolicDeconstruction
    {
        private readonly SymbolMap _map;

        public SymbolicDeconstruction(SymbolMap map)
        {
            _map = map;

        }

        public VariableContext Deconstruct(ISymbol expression)
        {
            return Deconstruct(expression, _map, new VariableContext());
        }

        private static VariableContext Deconstruct(
            ISymbol expression, SymbolMap map, VariableContext output)
        {
            if (map == null)
                return output;

            if (expression.Type.IsBinaryOperation())
            {
                var binaryExp = expression as BinaryOperation;
                return DeconstructBinary(binaryExp, map, output);
            }

            if (expression.Type.IsUnaryOperation())
            {
                var unaryExp = expression as UnaryOperation;
                return DeconstructUnary(unaryExp, map, output);
            }

            if (expression.Type.IsValue())
            {
                if (map.HasChildren)
                {
                    throw new InvalidOperationException(
                        "symbol map attempted to traverse expression tree too deep");
                }
                if (map.Value.HasName)
                    output.Define(map.Value.Name, expression);

                return output;
            }

            throw new NotImplementedException(
                "Attempted to traverse unsupported expression type: " +
                expression.Type.ToString());
        }

        private static VariableContext DeconstructBinary(
            BinaryOperation op, SymbolMap map, VariableContext output)
        {
            var leftMapChild = (SymbolMap)map.ChildOrDefault(0);
            var rightMapChild = (SymbolMap)map.ChildOrDefault(1);

            output = Deconstruct(op.Operand1, leftMapChild, output);
            output = Deconstruct(op.Operand2, rightMapChild, output);

            if (leftMapChild == null && rightMapChild == null && map.Value.HasName)
                output.Define(map.Value.Name, op);

            return output;
        }

        private static VariableContext DeconstructUnary(
            UnaryOperation op, SymbolMap map, VariableContext output)
        {
            var mapChild = (SymbolMap)map.ChildOrDefault(0);

            if (mapChild != null)
                output = Deconstruct(op.Child, mapChild, output);

            if (mapChild == null && map.Value.HasName)
                output.Define(map.Value.Name, op);

            return output;
        }
    }
}
