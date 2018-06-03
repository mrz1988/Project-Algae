using System.Collections.Generic;
using System.Linq;
using ZUtils.Collections;

namespace ZMath.Algebraic.Transforms
{
    public class SymbolMap : ImmutableTree<SymbolName>
    {
        // enforce leaf-only setup
        public SymbolMap(SymbolName name) : base(name, new SymbolMap[] { }) { }
        public SymbolMap(SymbolMap[] children) : base(SymbolName.None, children) { }

        public VariableContext GenerateContext()
        {
            return VariableContext.FromVariableNames(AllNames());
        }

        public List<string> AllNames()
        {
            var names = new List<string>();
            foreach (var child in Children)
            {
                var castChild = child as SymbolMap;
                names.AddRange(castChild.AllNames());
            }

            if (!string.IsNullOrEmpty(Value.Name))
                names.Add(Value.Name);

            return names;
        }
    }
}
