using System;

namespace ZMath.Algebraic
{
    public class EvaluationFailureException : Exception
    {
        public EvaluationFailureException(string msg) : base(msg) { }
    }
}
