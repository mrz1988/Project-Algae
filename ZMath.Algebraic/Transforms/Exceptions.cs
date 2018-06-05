using System;

namespace ZMath.Algebraic.Transforms
{
    public class InvalidTransformException : Exception
    {
        public InvalidTransformException(string msg) : base(msg) { }
    }
}
