using NUnit.Framework;
using ZMath.Algebraic.Transforms;
using ZMath.Algebraic.Constraints;
using ZMath.Algebraic.Operations;
using ZMath.Algebraic.Functions;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Tests.Transforms
{
    [TestFixture]
    public static class BasicTransformTests
    {
        private static SymbolicTransform GenerateAdditionForceValuesLeftTransform()
        {
            // constraint where value is on the right, and
            // some non-left subtree is on the left
            var constraint = new BasicSymbolicConstraint(
                s => s.Type == SymbolType.Addition,
                new BasicSymbolicConstraint[] {
                    new BasicSymbolicConstraint(
                        s => !s.Type.IsValue()
                    ),
                    new BasicSymbolicConstraint(
                        s => s.Type.IsValue()
                    )
                }
            );

            var leftName = "left";
            var rightName = "right";
            var inputMap = new SymbolMap(new SymbolMap[]
            {
                new SymbolMap(new SymbolName(leftName)),
                new SymbolMap(new SymbolName(rightName))
            });
            var context = inputMap.GenerateContext();

            var outputExpression = new Addition(
                new Variable(rightName),
                new Variable(leftName)
            );
            var outputFunction = new Function(outputExpression, context);

            return SymbolicTransform.New(constraint, inputMap, outputFunction);
        }

        [Test]
        public static void CanMoveValuesToLeftAdditionSubtrees()
        {
            var transform = GenerateAdditionForceValuesLeftTransform();
            var input = new Addition(
                Operations.AdditionWithVariable,
                Numbers.Five
            );
            var expected = new Addition(
                Numbers.Five,
                Operations.AdditionWithVariable
            );

            var result = transform.Transform(input);
            Assert.AreEqual(expected, result);
            Assert.AreNotEqual(input, result);
        }

        private static SymbolicTransform GenerateOneTimesXToXTransform()
        {
            var constraint = new BasicSymbolicConstraint(
                s => s.Type == SymbolType.Multiplication,
                new BasicSymbolicConstraint[] {
                    new BasicSymbolicConstraint(
                        s => s.Type == SymbolType.Number && (Number)s == Numbers.One),
                    new BasicSymbolicConstraint()
                }
            );
            var varName ="value";
            var inputMap = new SymbolMap(
                new SymbolMap[]
                {
                    new SymbolMap(), // placeholder for left part
                    new SymbolMap(new SymbolName(varName))
                }
            );
            var context = inputMap.GenerateContext();

            var outputExpression = new Variable(varName);
            var outputFunction = new Function(outputExpression, context);

            return SymbolicTransform.New(constraint, inputMap, outputFunction);
        }

        [Test]
        public static void CanReduceOneTimesXToJustX()
        {
            var transform = GenerateOneTimesXToXTransform();

            var input = new Multiplication(
                Numbers.One,
                Operations.AdditionWithVariable
            );
            var expected = Operations.AdditionWithVariable;
            var result = transform.Transform(input);

            Assert.AreEqual(expected, result);
            Assert.AreNotEqual(input, result);
        }
    }
}
