using System.Collections.Generic;
using ZUtils.Pipes;

namespace ZMath.Algebraic
{
    // This class doesn't really need to be a pipe, but it fit well
    // with the other pipes :)
    public class TokenValidater : AsymmetricPipe<SymbolToken, SymbolToken>
    {
        private List<SymbolToken> _heldInput;
        private bool _holdInput;
        private SymbolToken _lastOutput;
        private int _parentheses = 0;

        public TokenValidater(IEnumerable<SymbolToken> items) : base(items)
        {
            _heldInput = new List<SymbolToken>();
            _holdInput = false;
            _lastOutput = SymbolToken.OpenBracket;
        }

        protected override void Output(SymbolToken item)
        {
            base.Output(item);
            _lastOutput = item;
        }

        private void Hold(SymbolToken val)
        {
            _heldInput.Add(val);
            if (val.Type == SymbolType.OpenBracket)
                _parentheses++;
            if (val.Type == SymbolType.CloseBracket)
                _parentheses--;

            if (_parentheses == 0)
            {
                // remove parentheses
                var tv = new TokenValidater(_heldInput.GetRange(1, _heldInput.Count - 2));

                // add back in original parentheses
                Output(_heldInput[0]);
                Output(tv.PumpAll());
                Output(_heldInput[_heldInput.Count - 1]);

                _heldInput = new List<SymbolToken>();
                _holdInput = false;
            }
        }

        protected override void Consume(SymbolToken val)
        {
            if (_holdInput)
            {
                Hold(val);
                return;
            }

            if (_lastOutput.Type.IsUnaryOperation())
            {
                if (val.Type != SymbolType.OpenBracket)
                {
                    throw new InvalidTokenException(val.Position, val.Length,
                        "Expected open parenthesis following unary operation");
                }
            }
            else if (_lastOutput.Type.IsBinaryOperation())
            {
                if (val.Type.IsBinaryOperation())
                {
                    throw new InvalidTokenException(val.Position, val.Length,
                        "Cannot have consecutive binary operators");
                }
            }
            else if (_lastOutput.Type.IsValue())
            {
                if (val.Type.IsValue())
                {
                    throw new InvalidTokenException(val.Position, val.Length,
                        "Cannot have consecutive values");
                }
            }

            if (val.Type == SymbolType.CloseBracket)
            {
                throw new InvalidParenthesisException(val.Position);
            }

            if (val.Type == SymbolType.OpenBracket)
            {
                _holdInput = true;
                Hold(val);
                return;
            }

            Output(val);
        }

        protected override void Finish()
        {
            if (_heldInput.Count == 0)
                return;

            _holdInput = false;
            _heldInput = new List<SymbolToken>();
            _parentheses = 0;
            _lastOutput = SymbolToken.OpenBracket;
        }
    }
}
