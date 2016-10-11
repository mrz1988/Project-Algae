using System;
namespace ZMath.Algebraic
{
	public class Number : ISymbol
	{
		private int _intVal;
		private double _floatVal;
		private NumberType _type;

		public SymbolType Type { get { return SymbolType.Number; } }

		public static Number E
		{
			get
			{
				return new Number(Math.E);
			}
		}

		public static Number Pi
		{
			get
			{
				return new Number(Math.PI);
			}
		}

		public int AsInt
		{
			get
			{
				if (_type == NumberType.Integer)
					return _intVal;
				return (int)_floatVal;
			}
		}

		public double AsFloatingPt
		{
			get
			{
				if (_type == NumberType.Integer)
					return _intVal;
				return _floatVal;
			}
		}

		public bool IsFloatingPt
		{
			get
			{
				return _type == NumberType.Float;
			}
		}

		public Number(double num)
		{
			_type = NumberType.Float;
			_floatVal = num;
		}

		public Number(int num)
		{
			_type = NumberType.Integer;
			_intVal = num;
		}

		public Number GetValue()
		{
			return this;
		}
	}
}
