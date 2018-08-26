using System;
using System.Collections.Generic;
using System.Text;
using ZMath.Algebraic;

namespace ZMath.Repl
{
    public class ReplManager
    {
        private AlgebraProcessor _proc;
        private HashSet<string> _quitCommands = new HashSet<string>
        {
            "q",
            "quit",
            "exit"
        };
        public ReplManager()
        {
            _proc = new AlgebraProcessor();
        }

        public void Start()
        {
            while (ProcessInput(Console.ReadLine())) { }
        }

        private bool ProcessInput(string input)
        {
            if (_quitCommands.Contains(input))
                return false;

            Console.WriteLine(_proc.Reduce(input));
            return true;
        }
    }
}
