using System;
using System.Collections.Generic;

namespace ZMath.Algebraic
{
	public class VariableContext
	{
		private Dictionary<string, ISymbol> _initMap;
		private Dictionary<string, Number> _definedVars;

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
			_definedVars = new Dictionary<string, Number>();
		}

		public void Register(string name, ISymbol symbol)
		{
			if (_initMap.ContainsKey(name))
				throw new ArgumentException("Variable name already taken", nameof(name));
			_initMap[name] = symbol;
		}

		public void Define(string name, Number definition)
		{
			_definedVars[name] = definition;
		}

		public void UndefineAll()
		{
			_definedVars = new Dictionary<string, Number>();
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
