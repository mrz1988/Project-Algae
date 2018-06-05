using System;
using System.Collections.Generic;

namespace ZMath.Algebraic.Constraints
{
    public class EquatingSymbolicConstraint : BasicSymbolicConstraint
    {
        private Dictionary<string, ISymbol> _equatingTable;
        private readonly SymbolName _name = SymbolName.None;

        public EquatingSymbolicConstraint() : base() { }
        public EquatingSymbolicConstraint(Predicate<ISymbol> nodeEvaluator)
            : base(nodeEvaluator) { }
        public EquatingSymbolicConstraint(Predicate<ISymbol> nodeEvaluator, EquatingSymbolicConstraint[] childConstraints)
            : base(nodeEvaluator, childConstraints)
        {
            // all sub nodes must have the same reference table
            _equatingTable = new Dictionary<string, ISymbol>();
            foreach (var constraint in childConstraints)
                constraint._equatingTable = _equatingTable;
        }
        public EquatingSymbolicConstraint(SymbolName matches) : this(matches, _ => true) { }
        public EquatingSymbolicConstraint(SymbolName matches, Predicate<ISymbol> nodeEvaluator)
            : base(nodeEvaluator)
        {
            _name = matches;
        }

        public override bool BaseNodeIsValid(ISymbol symbol)
        {
            if (!base.BaseNodeIsValid(symbol))
                return false;

            if (_name == SymbolName.None)
                return true;

            if (!_equatingTable.ContainsKey(_name.Name))
            {
                _equatingTable.Add(_name.Name, symbol);
                return true;
            }

            // Have to use .Equals here instead of ==
            // since we're doing an interface comparison
            // and we care about content, not pointer equality
            return _equatingTable[_name.Name].Equals(symbol);
        }

        public override void Reset()
        {
            _equatingTable.Clear();
        }
    }
}
