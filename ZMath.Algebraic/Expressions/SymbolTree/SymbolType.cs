using System;
using System.Collections.Generic;

namespace ZMath.Algebraic
{
	public enum SymbolType
	{
		Number,
		Variable,
		Addition,
		Subtraction,
		Multiplication,
		Division,
		Exponentiation,
		//Logarithm,
		Negation,
		Factorial,
		Sine,
		Cosine,
		Tangent,
		ArcSine,
		ArcCosine,
		ArcTangent,

		OpenBracket,
		CloseBracket,
	}

	public static class SymbolTypeExtensions
	{
		public static readonly HashSet<SymbolType> Values = new HashSet<SymbolType> {
			SymbolType.Number,
			SymbolType.Variable,
		};

		public static HashSet<SymbolType> UnaryOperations = new HashSet<SymbolType> {
			SymbolType.Factorial,
			SymbolType.Sine,
			SymbolType.Cosine,
			SymbolType.Tangent,
			SymbolType.ArcSine,
			SymbolType.ArcCosine,
			SymbolType.ArcTangent,
			SymbolType.Negation,
		};

		public static HashSet<SymbolType> BinaryOperations = new HashSet<SymbolType> {
			SymbolType.Addition,
			SymbolType.Subtraction,
			SymbolType.Multiplication,
			SymbolType.Division,
			SymbolType.Exponentiation,
			//SymbolType.Logarithm,
		};

		public static bool IsValue(this SymbolType t)
		{
			return Values.Contains(t);
		}

		public static bool IsUnaryOperation(this SymbolType t)
		{
			return UnaryOperations.Contains(t);
		}

		public static bool IsBinaryOperation(this SymbolType t)
		{
			return BinaryOperations.Contains(t);
		}

		public static int Order(this SymbolType t)
		{
			switch (t)
			{
				case SymbolType.OpenBracket:
				case SymbolType.CloseBracket:
					return 20;
				case SymbolType.Number:
				case SymbolType.Variable:
					return 10;
				case SymbolType.Negation:
				case SymbolType.Sine:
				case SymbolType.Cosine:
				case SymbolType.Tangent:
				case SymbolType.ArcSine:
				case SymbolType.ArcCosine:
				case SymbolType.ArcTangent:
				case SymbolType.Factorial:
					return 5;
				case SymbolType.Exponentiation:
					return 4;
				case SymbolType.Multiplication:
				case SymbolType.Division:
					return 3;
				case SymbolType.Addition:
				case SymbolType.Subtraction:
					return 2;
			}

			throw new NotImplementedException();
		}
	}
}
