using System;
namespace ZMath.Algebraic
{
	public class Number : ISymbol, IComparable
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

		public ISymbol Copy()
		{
			if (IsFloatingPt)
				return new Number(AsFloatingPt);
			return new Number(AsInt);
		}

		public Number GetValue()
		{
			return this;
		}

		public bool CanEvaluate()
		{
			return true;
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			
			if (GetType() != obj.GetType())
				throw new ArgumentException("Cannot compare, types differ");

			var n = (Number)obj;
			return AsFloatingPt.CompareTo(n.AsFloatingPt);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			Number n = (Number)obj;

			if (IsFloatingPt || n.IsFloatingPt)
			{
				// Tolerance for floating point equality is defined as
				// 0.000001% of x in x.Equals(y)
				var epsilon = 0.00000001 * AsFloatingPt;
				var difference = Math.Abs(n.AsFloatingPt - AsFloatingPt);
				return difference < epsilon;
			}

			return n.AsInt == AsInt;
		}

		public override int GetHashCode()
		{
			return AsFloatingPt.GetHashCode();
		}

		public static bool operator ==(Number a, Number b)
		{
			if (a == null)
				return b == null;
			
			return a.Equals(b);
		}

		public static bool operator !=(Number a, Number b)
		{
			return !(a == b);
		}

		public static bool operator >(Number a, Number b)
		{
			if (a == null)
				return false;
			
			return a.CompareTo(b) > 0;
		}

		public static bool operator <(Number a, Number b)
		{
			if (a == null && b == null)
				return false;
			
			if (a == null)
				return true;
			
			return a.CompareTo(b) < 0;
		}
	}
}
