using System;
using System.Collections.Generic;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Functions
{
	public class MultiVariableFunction : Function
	{
		public string[] VariableNames { get; private set; }
		public int VariableCount { get { return VariableNames.Length; } }
		public MultiVariableFunction(ISymbol root, params string[] variableNames)
			: base(root, VariableContext.FromVariableNames(variableNames))
		{
			VariableNames = variableNames;
		}

		public static MultiVariableFunction FromString(string expression, params string[] variableNames)
		{
			var ctx = VariableContext.FromVariableNames(variableNames);
			var root = StringTokenizer.BuildTreeFrom(expression, ctx);
			return new MultiVariableFunction(root, variableNames);
		}

		public Number Call(params double[] vals)
		{
			var nums = new List<Number>();
			foreach (var val in vals)
			{
				nums.Add(new Number(val));
			}

			return Call(nums.ToArray());
		}

		public Number Call(params int[] vals)
		{
			var nums = new List<Number>();
			foreach (var val in vals)
			{
				nums.Add(new Number(val));
			}

			return Call(nums.ToArray());
		}

		public Number Call(params ISymbol[] vals)
		{
			if (vals.Length != VariableCount)
				throw new ArgumentException(string.Format(
					"Must pass {0} variables into this function", VariableCount));
			for (var i = 0; i < vals.Length; i++)
			{
				Substitute(VariableNames[i], vals[i]);
			}

			var result = Evaluate();
			if (result.Type != SymbolType.Number)
				throw new EvaluationFailureException("Function did not reduce to a number");

			return (Number)result;
		}
	}
}
