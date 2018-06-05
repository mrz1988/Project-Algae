using System.Collections.Generic;
using ZMath.Algebraic.Constraints;
using ZMath.Algebraic.Values;
using ZMath.Algebraic.Operations;
using ZMath.Algebraic.Functions;

namespace ZMath.Algebraic.Transforms
{
    // TODO: Everything in this class should be generatable from XML or a DSL
    // this is a ridiculous amount of code that exists for no reason but I'm too lazy
    // to do this properly for the first pass
    public partial class SymbolicTransform
    {
        private static class SymbolNames
        {
            public static SymbolName Left = new SymbolName("left");
            public static SymbolName Right = new SymbolName("right");

            public static SymbolName One = new SymbolName("one");
            public static SymbolName Two = new SymbolName("two");
            public static SymbolName Three = new SymbolName("three");
            public static SymbolName Four = new SymbolName("four");
            public static SymbolName Five = new SymbolName("five");
        }

        private static class Numbers
        {
            public static Number One = new Number(1);
            public static Number Zero = new Number(0);
            public static Number NegativeOne = new Number(-1);
        }

        #region Move Complex Subtrees Right
        private static BasicSymbolicConstraint AdditionWithVariableOnLeftValueOnRight =
            new BasicSymbolicConstraint(
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

        private static BasicSymbolicConstraint MultiplicationWithVariableOnLeftValueOnRight =
            new BasicSymbolicConstraint(
                s => s.Type == SymbolType.Multiplication,
                new BasicSymbolicConstraint[] {
                    new BasicSymbolicConstraint(
                        s => !s.Type.IsValue()
                    ),
                    new BasicSymbolicConstraint(
                        s => s.Type.IsValue()
                    )
                }
            );

        private static SymbolMap MapToLeftRightChildren = new SymbolMap(new SymbolMap[] {
            new SymbolMap(SymbolNames.Left),
            new SymbolMap(SymbolNames.Right)
        });

        private static Function FlipAdditionSubtreeOutputMap = new Function(
            new Addition(
                new Variable(SymbolNames.Right.Name),
                new Variable(SymbolNames.Left.Name)
            ),
            MapToLeftRightChildren.GenerateContext()
        );

        private static Function FlipMultiplicationSubtreeOutputMap = new Function(
            new Multiplication(
                new Variable(SymbolNames.Right.Name),
                new Variable(SymbolNames.Left.Name)
            ),
            MapToLeftRightChildren.GenerateContext()
        );

        public static SymbolicTransform MoveComplexAdditionSubtreesRight = New(
            AdditionWithVariableOnLeftValueOnRight,
            MapToLeftRightChildren,
            FlipAdditionSubtreeOutputMap
        );

        public static SymbolicTransform MoveComplexMultiplicationSubtreesRight = New(
            MultiplicationWithVariableOnLeftValueOnRight,
            MapToLeftRightChildren,
            FlipMultiplicationSubtreeOutputMap
        );

        public static TransformSet MoveComplexSubtreesRight = new TransformSet(
            new List<SymbolicTransform>()
            {
                MoveComplexAdditionSubtreesRight,
                MoveComplexMultiplicationSubtreesRight
            }
        );
        #endregion

        //TODO: Negation massaging (move negations up the chain to not interfere)

        #region BasicOperationSimplification

        //TODO: Add negation matching as well (negation subtrees can be reduced)

        private static BasicSymbolicConstraint OneTimesAnything =
            new BasicSymbolicConstraint(
                s => s.Type == SymbolType.Multiplication,
                new BasicSymbolicConstraint[] {
                    new BasicSymbolicConstraint(
                        s => s.Type == SymbolType.Number && (Number)s == Numbers.One
                    ),
                    new BasicSymbolicConstraint()
                }
            );

        private static BasicSymbolicConstraint ZeroTimesAnything =
            new BasicSymbolicConstraint(
                s => s.Type == SymbolType.Multiplication,
                new BasicSymbolicConstraint[] {
                    new BasicSymbolicConstraint(
                        s => s.Type == SymbolType.Number && (Number)s == Numbers.Zero
                    ),
                    new BasicSymbolicConstraint()
                }
            );

        private static BasicSymbolicConstraint NegativeOneTimesAnything =
            new BasicSymbolicConstraint(
                s => s.Type == SymbolType.Multiplication,
                new BasicSymbolicConstraint[] {
                    new BasicSymbolicConstraint(
                        s => s.Type == SymbolType.Number && (Number)s == Numbers.NegativeOne
                    ),
                    new BasicSymbolicConstraint()
                }
            );

        private static BasicSymbolicConstraint AnythingDividedByOne =
            new BasicSymbolicConstraint(
                s => s.Type == SymbolType.Division,
                new BasicSymbolicConstraint[] {
                    new BasicSymbolicConstraint(),
                    new BasicSymbolicConstraint(
                        s => s.Type == SymbolType.Number && (Number)s == Numbers.One
                    )
                }
            );

        // FIXME: This should use a true negation maybe, or might not be needed
        private static BasicSymbolicConstraint AnythingDividedByNegativeOne =
            new BasicSymbolicConstraint(
                s => s.Type == SymbolType.Division,
                new BasicSymbolicConstraint[] {
                    new BasicSymbolicConstraint(),
                    new BasicSymbolicConstraint(
                        s => s.Type == SymbolType.Number && (Number)s == Numbers.NegativeOne
                    )
                }
            );

        private static EquatingSymbolicConstraint AnythingPlusItself =
            new EquatingSymbolicConstraint(
                s => s.Type == SymbolType.Addition,
                new EquatingSymbolicConstraint[]
                {
                    new EquatingSymbolicConstraint(SymbolNames.One),
                    new EquatingSymbolicConstraint(SymbolNames.One)
                }
            );

        private static EquatingSymbolicConstraint AnythingTimesItself =
            new EquatingSymbolicConstraint(
                s => s.Type == SymbolType.Multiplication,
                new EquatingSymbolicConstraint[]
                {
                    new EquatingSymbolicConstraint(SymbolNames.One),
                    new EquatingSymbolicConstraint(SymbolNames.One)
                }
            );

        private static EquatingSymbolicConstraint AnythingDividedByItself =
            new EquatingSymbolicConstraint(
                s => s.Type == SymbolType.Division,
                new EquatingSymbolicConstraint[]
                {
                    new EquatingSymbolicConstraint(SymbolNames.One),
                    new EquatingSymbolicConstraint(SymbolNames.One)
                }
            );

        private static BasicSymbolicConstraint ComplexMultiplicationHasIsolatableConstants =
            new BasicSymbolicConstraint(
                s => s.Type == SymbolType.Multiplication,
                new BasicSymbolicConstraint[]
                {
                    BasicSymbolicConstraint.IsOperation(SymbolType.Number),
                    new BasicSymbolicConstraint(
                        s => s.Type == SymbolType.Multiplication,
                        new BasicSymbolicConstraint[]
                        {
                            new BasicSymbolicConstraint(s => s.Type == SymbolType.Number),
                            new BasicSymbolicConstraint(s => !s.CanEvaluate())
                        }
                    )
                }
            );

        private static BasicSymbolicConstraint ComplexAdditionHasIsolatableConstants =
            new BasicSymbolicConstraint(
                s => s.Type == SymbolType.Addition,
                new BasicSymbolicConstraint[]
                {
                    BasicSymbolicConstraint.IsOperation(SymbolType.Number),
                    new BasicSymbolicConstraint(
                        s => s.Type == SymbolType.Addition,
                        new BasicSymbolicConstraint[]
                        {
                            new BasicSymbolicConstraint(s => s.Type == SymbolType.Number),
                            new BasicSymbolicConstraint(s => !s.CanEvaluate())
                        }
                    )
                }
            );

        private static SymbolMap MapLeftChild = new SymbolMap(new SymbolMap[]
        {
            new SymbolMap(SymbolNames.Left),
            new SymbolMap()
        });

        private static SymbolMap MapRightChild = new SymbolMap(new SymbolMap[]
        {
            new SymbolMap(),
            new SymbolMap(SymbolNames.Right)
        });

        private static SymbolMap MapOneLeafLeftTwoLeavesRight = new SymbolMap(new SymbolMap[]
        {
            new SymbolMap(SymbolNames.One),
            new SymbolMap(new SymbolMap[]
            {
                new SymbolMap(SymbolNames.Two),
                new SymbolMap(SymbolNames.Three)
            })
        });

        private static Function KeepLeftChildOnly = new Function(
            new Variable(SymbolNames.Left.Name),
            MapLeftChild.GenerateContext()
        );

        private static Function KeepRightChildOnly = new Function(
            new Variable(SymbolNames.Right.Name),
            MapRightChild.GenerateContext()
        );

        private static Function One = new Function(
            Numbers.One,
            VariableContext.ConstantsOnly
        );

        private static Function Zero = new Function(
            Numbers.Zero,
            VariableContext.ConstantsOnly
        );

        private static Function NegateLeftChild = new Function(
            new Negation(
                new Variable(SymbolNames.Left.Name)
            ),
            MapLeftChild.GenerateContext()
        );

        private static Function NegateRightChild = new Function(
            new Negation(
                new Variable(SymbolNames.Right.Name)
            ),
            MapRightChild.GenerateContext()
        );

        private static Function TwoTimesRightChild = new Function(
            new Multiplication(
                new Number(2),
                new Variable(SymbolNames.Right.Name)
            ),
            MapRightChild.GenerateContext()
        );

        private static Function RightChildSquared = new Function(
            new Exponentiation(
                new Variable(SymbolNames.Right.Name),
                new Number(2)
            ),
            MapRightChild.GenerateContext()
        );

        private static Function MultiplicationWithOneTwoLeftThreeRight = new Function(
            new Multiplication(
                new Multiplication(
                    new Variable(SymbolNames.One.Name),
                    new Variable(SymbolNames.Two.Name)
                ),
                new Variable(SymbolNames.Three.Name)
            ),
            MapOneLeafLeftTwoLeavesRight.GenerateContext()
        );

        private static Function AdditionWithOneTwoLeftThreeRight = new Function(
            new Addition(
                new Addition(
                    new Variable(SymbolNames.One.Name),
                    new Variable(SymbolNames.Two.Name)
                ),
                new Variable(SymbolNames.Three.Name)
            ),
            MapOneLeafLeftTwoLeavesRight.GenerateContext()
        );

        public static SymbolicTransform OneTimesAnythingIsItself = New(
            OneTimesAnything,
            MapRightChild,
            KeepRightChildOnly
        );

        public static SymbolicTransform ZeroTimesAnythingIsZero = New(
            ZeroTimesAnything,
            new SymbolMap(),
            Zero
        );

        public static SymbolicTransform NegativeOneTimesAnythingIsNegated = New(
            NegativeOneTimesAnything,
            MapRightChild,
            NegateRightChild
        );

        public static SymbolicTransform AnythingDividedByOneIsItself = New(
            AnythingDividedByOne,
            MapLeftChild,
            NegateLeftChild
        );

        public static SymbolicTransform AnythingDividedByItselfIsOne = New(
            AnythingDividedByItself,
            new SymbolMap(),
            One
        );

        public static SymbolicTransform AnythingPlusItselfIsItselfTimesTwo = New(
            AnythingPlusItself,
            MapRightChild,
            TwoTimesRightChild
        );

        public static SymbolicTransform AnythingTimesItselfIsItselfSquared = New(
            AnythingTimesItself,
            MapRightChild,
            RightChildSquared
        );

        public static SymbolicTransform MultipliedConstantsPropagateUpwards = New(
            ComplexMultiplicationHasIsolatableConstants,
            MapOneLeafLeftTwoLeavesRight,
            MultiplicationWithOneTwoLeftThreeRight
        );

        public static SymbolicTransform AddedConstantsPropagateUpwards = New(
            ComplexAdditionHasIsolatableConstants,
            MapOneLeafLeftTwoLeavesRight,
            AdditionWithOneTwoLeftThreeRight
        );

        public static TransformSet AlgebraicReduce = new TransformSet(
            new List<SymbolicTransform>()
            {
                OneTimesAnythingIsItself,
                ZeroTimesAnythingIsZero,
                NegativeOneTimesAnythingIsNegated,
                AnythingDividedByOneIsItself,
                AnythingDividedByItselfIsOne,
                AnythingPlusItselfIsItselfTimesTwo,
                AnythingTimesItselfIsItselfSquared,
                MultipliedConstantsPropagateUpwards,
                AddedConstantsPropagateUpwards,
            });
        #endregion
    }
}
