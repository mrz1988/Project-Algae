using System;
using System.Collections.Generic;

namespace ZMath.Repl
{
    class Repl
    {
        static void Main(string[] args)
        {
            var runner = new ReplManager();
            runner.Start();
        }
    }
}
