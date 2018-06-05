using System;

namespace ZMath.Algebraic
{
    //TODO: add this to variable contexts
    public class SymbolName
    {
        public static SymbolName None = new SymbolName();

        public readonly string Name;
        public bool HasName => !string.IsNullOrEmpty(Name);

        public SymbolName() : this(string.Empty) { }
        public SymbolName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }
    }
}
