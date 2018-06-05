using System;
using ZMath.Algebraic.Functions;
using ZMath.Algebraic.Constraints;
using ZMath.Algebraic.Operations;

namespace ZMath.Algebraic.Transforms
{
    public partial class SymbolicTransform
    {
        // Steps:
        // 1. Take in an existing expression alongside a function definition that
        //    holds a symbolic "final state" of any transform, as well as a symbol
        //    matcher and deconstructor
        // 2. Find a matching node to some SymbolConstraint
        // 3. Create a new Function consisting of the expression with the matching node
        //    replaced by a variable (some guid)
        //    (It seems like this can be done by copying the expression to a new expression
        //    one node at a time until a "match" is hit, at which point the matching is
        //    halted and the transform is run on the matching subtree)
        // 4. Decompose the subtree at the current location using SymbolicDeconstruction
        // 5. Use the transform function to inject deconstructed variable context into
        //    a new subtree
        // 6. Inject the new subtree into the matching spot of the original expression

        private BasicSymbolicConstraint _constraint;
        private SymbolicDeconstructor _deconstructor;
        private Function _output;

        private SymbolicTransform(
            BasicSymbolicConstraint constraint,
            SymbolicDeconstructor deconstructor,
            Function outputFunction)
        {
            _constraint = constraint;
            _deconstructor = deconstructor;
            _output = outputFunction;
        }

        public static SymbolicTransform New(
            BasicSymbolicConstraint constraint,
            SymbolMap inputMap,
            Function outputFunction)
        {
            var inputContext = inputMap.GenerateContext();
            if (!outputFunction.ContainsVariables(inputContext))
                throw new InvalidTransformException(
                    $"'{nameof(inputMap)}' contains variable names that do not exist in '{nameof(outputFunction)}'");

            var deconstructor = new SymbolicDeconstructor(inputMap);

            return new SymbolicTransform(constraint, deconstructor, outputFunction);
        }

        public ISymbol Transform(ISymbol expression)
        {
            // unfortunately this reset is necessary to make sure
            // our constraint doesn't have left over stuff from
            // its previous calculation. Alternatively we could
            // copy it fresh each time, but this saves some
            // CPU power. This really only applies to
            // equatable constraints, which have to be stateful...
            _constraint.Reset();
            return TransformGeneric(expression).Item2;
        }

        private Tuple<bool, ISymbol> TransformGeneric(ISymbol op)
        {
            if (op.Type.IsBinaryOperation())
                return TransformBinary(op as BinaryOperation);

            if (op.Type.IsUnaryOperation())
                return TransformUnary(op as UnaryOperation);

            if (op.Type.IsValue())
                return TransformValue(op);

            throw new NotImplementedException($"Unsupported expression node type: {op.Type}");
        }

        private Tuple<bool, ISymbol> TransformValue(ISymbol value)
        {
            if (!value.Matches(_constraint))
            {
                return new Tuple<bool, ISymbol>(false, value);
            }

            var result = RunTransformOn(value);
            return new Tuple<bool, ISymbol>(true, result);
        }

        private Tuple<bool, ISymbol> TransformBinary(BinaryOperation op)
        {
            BinaryOperation output = op;
            var transformOp1 = TransformGeneric(output.Operand1);
            if (transformOp1.Item1)
            {
                output = op.ReplaceOperand1(transformOp1.Item2) as BinaryOperation;
            }

            var transformOp2 = TransformGeneric(output.Operand2);
            if (transformOp2.Item1)
            {
                output = op.ReplaceOperand2(transformOp2.Item2) as BinaryOperation;
            }

            var transformOccurred = transformOp1.Item1 || transformOp2.Item1;
            if (!output.Matches(_constraint))
            {
                return new Tuple<bool, ISymbol>(transformOccurred, output);
            }

            var transformed = RunTransformOn(output);
            return new Tuple<bool, ISymbol>(true, transformed);
        }

        private Tuple<bool, ISymbol> TransformUnary(UnaryOperation op)
        {
            var transformOp1 = TransformGeneric(op.Child);
            if (transformOp1.Item1)
            {
                var output = op.ReplaceChild(transformOp1.Item2) as UnaryOperation;
                return new Tuple<bool, ISymbol>(true, output);
            }

            if (!op.Matches(_constraint))
            {
                return new Tuple<bool, ISymbol>(false, op);
            }

            var transformed = RunTransformOn(op);
            return new Tuple<bool, ISymbol>(true, transformed);
        }

        private ISymbol RunTransformOn(ISymbol expression)
        {
            var context = _deconstructor.Deconstruct(expression);
            _output.SubstituteFrom(context);
            var result = _output.ToExpression();
            _output.ClearSubstitutions();

            return result;
        }
    }
}
