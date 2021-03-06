﻿using System;
using System.Linq;
using System.Collections.Generic;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic
{
    public class VariableContext
    {
        private Dictionary<string, ISymbol> _initMap;
        private Dictionary<string, ISymbol> _definedVars;

        public static VariableContext ConstantsOnly
        {
            get
            {
                return new VariableContext(new Dictionary<string, ISymbol> {
                    { "pi", Number.Pi },
                    { "e", Number.E }
                });
            }
        }

        public static VariableContext Default
        {
            get
            {
                return new VariableContext(new Dictionary<string, ISymbol> {
                    { "pi", Number.Pi },
                    { "e", Number.E },
                    { "x", new Variable("x") },
                    { "y", new Variable("y") },
                    { "z", new Variable("z") },
                });
            }
        }

        public VariableContext() : this(new Dictionary<string, ISymbol>()) { }
        public VariableContext(Dictionary<string, ISymbol> mapping)
        {
            _initMap = mapping;
            _definedVars = new Dictionary<string, ISymbol>();
        }

        public VariableContext Copy()
        {
            return FromVariableNames(_initMap.Keys.ToArray());
        }

        public static VariableContext FromVariableNames(string varName)
        {
            return FromVariableNames(new string[] {varName});
        }

        public static VariableContext FromVariableNames(string var1, string var2)
        {
            return FromVariableNames(new string[] { var1, var2 });
        }

        public static VariableContext FromVariableNames(IEnumerable<string> variableNames)
        {
            var ctx = ConstantsOnly;
            foreach (var name in variableNames)
            {
                ctx.Register(name, new Variable(name));
            }

            return ctx;
        }

        public void Register(string name, ISymbol symbol)
        {
            if (_initMap.ContainsKey(name))
                throw new ArgumentException("Variable name already taken", nameof(name));
            _initMap[name] = symbol;
        }

        public void Define(string name, ISymbol definition)
        {
            if (!IsRegistered(name))
                throw new InvalidOperationException($"Variable '{name}' was not registered");
            _definedVars[name] = definition;
        }

        /// <summary>
        /// Adds definitions from intersecting variable names, substituting their
        /// values into the context. Similar to running "Define" on the present
        /// context from each defined variable in the other context.
        /// </summary>
        /// <param name="other"></param>
        public void DefineFrom(VariableContext other)
        {
            foreach (var variableName in _initMap.Keys)
            {
                if (other.IsDefined(variableName))
                {
                    Define(variableName, other.Get(variableName));
                }
            }
        }

        public void UndefineAll()
        {
            _definedVars = new Dictionary<string, ISymbol>();
        }

        public void Undefine(string name)
        {
            _definedVars.Remove(name);
        }

        public bool IsRegistered(string name)
        {
            return _initMap.ContainsKey(name);
        }

        public bool IsDefined(string name)
        {
            return _definedVars.ContainsKey(name);
        }

        /// <summary>
        /// Identifies whether all variables in this context are
        /// registered in the other context
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSubsetOf(VariableContext other)
        {
            foreach (var variableName in _initMap.Keys)
            {
                if (!other.IsRegistered(variableName))
                    return false;
            }

            return true;
        }

        public ISymbol InitVar(string name)
        {
            return _initMap[name].Copy();
        }

        public ISymbol Get(string name)
        {
            if (IsDefined(name))
                return _definedVars[name].Copy();
            if (IsRegistered(name))
                return _initMap[name].Copy();

            throw new KeyNotFoundException(string.Format(
                "Variable name did not exist: {0}", name));
        }

        public ISymbol Get(SymbolToken token)
        {
            return Get(token.Token);
        }

        public SymbolToken GetToken(string name)
        {
            if (!IsRegistered(name))
                throw new KeyNotFoundException(string.Format(
                    "Variable name did not exist: {0}", name));

            return new SymbolToken(SymbolType.Variable, name);
        }
    }
}
