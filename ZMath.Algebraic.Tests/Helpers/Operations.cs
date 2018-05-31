using ZMath.Algebraic.Operations;

namespace ZMath.Algebraic.Tests
{
    public static class Operations
    {
        public static Addition BasicAddition = new Addition(Numbers.One, Numbers.Two);
        public static Addition AdditionWithSameOperand = new Addition(Numbers.One, Numbers.One);
        public static Addition AdditionWithVariable = new Addition(Numbers.Zero, Numbers.X);
        public static Addition AdditionWithSameVariable = new Addition(Numbers.X, Numbers.X);
        public static Multiplication BasicMultiplication = new Multiplication(Numbers.One, Numbers.Two);
        public static Negation BasicNegation = new Negation(Numbers.One);
        public static Sine BasicSine = new Sine(Numbers.Pi);
    }
}
