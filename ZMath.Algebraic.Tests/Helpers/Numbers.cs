using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMath.Algebraic.Values;

namespace ZMath.Algebraic.Tests
{
    public static class Numbers
    {
        public static Number NegativeOne = new Number(-1);
        public static Number Zero = new Number(0);
        public static Number One = new Number(1);
        public static Number Two = new Number(2);
        public static Number Three = new Number(3);
        public static Number Five = new Number(5);
        public static Variable X = new Variable("x");
        public static Variable Y = new Variable("y");
        public static Variable Z = new Variable("z");
        public static Number Pi = Number.Pi;
    }
}
