using System;
using System.Linq;
using System.Collections.Generic;

namespace ZMath.Algebraic.Transforms
{
    public class TransformSet
    {
        private readonly IEnumerable<SymbolicTransform> _transforms;
        public TransformSet(IEnumerable<SymbolicTransform> transforms)
        {
            _transforms = transforms;
        }

        public TransformSet CombineWith(TransformSet other)
        {
            var aggregate = _transforms.ToList();
            aggregate.AddRange(other._transforms.ToList());
            return new TransformSet(aggregate);
        }

        public ISymbol Transform(ISymbol expression)
        {
            expression = expression.Reduce();
            HashSet<ISymbol> existingPoints = new HashSet<ISymbol>();

            while (!existingPoints.Contains(expression))
            {
                existingPoints.Add(expression);
                foreach (var transform in _transforms)
                {
                    expression = transform.Transform(expression);
                    expression = expression.Reduce();
                }
            }

            return expression;
        }

        public static TransformSet Reduce = new TransformSet(new List<SymbolicTransform>()
        {

        });
    }
}
