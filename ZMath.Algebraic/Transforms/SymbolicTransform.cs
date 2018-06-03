using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMath.Algebraic.Transforms
{
    public class SymbolicTransform
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
    }
}
