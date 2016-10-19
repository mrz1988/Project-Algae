using System;
using System.Collections.Generic;
namespace ZMath.Algebraic
{
	public class MultiVariableFunction : Function
	{
		public string[] VariableNames { get; private set; }
		public int VariableCount { get { return VariableNames.Length; } }
		public MultiVariableFunction(ISymbol root, VariableContext ctx) : base(root, ctx) { }
		public MultiVariableFunction(ISymbol root, params string[] variableNames)
			: base(root, VariableContext.FromVariableNames(variableNames))
		{
			VariableNames = variableNames;
		}

		public static SingleVariableFunction FromString(string expression, params string[] variableNames)
		{
			var ctx = VariableContext.FromVariableNames(variableNames);
			var root = StringTokenizer.BuildTreeFrom(expression, ctx);
			return new SingleVariableFunction(root, ctx);
		}

		public ISymbol Call(params double[] vals)
		{
			var nums = new List<Number>();
			foreach (var val in vals)
			{
				nums.Add(new Number(val));
			}

			return Call(nums.ToArray());
		}

		public ISymbol Call(params int[] vals)
		{
			var nums = new List<Number>();
			foreach (var val in vals)
			{
				nums.Add(new Number(val));
			}

			return Call(nums.ToArray());
		}

		public ISymbol Call(params Number[] vals)
		{
			if (vals.Length != VariableCount)
				throw new ArgumentException(string.Format(
					"Must pass {0} variables into this function", VariableCount));
			for (var i = 0; i < vals.Length; i++)
			{

			}
		}
	}
}
