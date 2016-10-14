using System;
using System.Collections;
using System.Collections.Generic;

namespace ZUtils.Pipes
{
	public abstract class AsymmetricPipe<I, O> : IEnumerable<O>
	{
		private readonly IEnumerator<I> _input;
		private Queue<O> _output;

		public AsymmetricPipe(IEnumerable<I> input)
		{
			_input = input.GetEnumerator();
		}

		public abstract void Consume(I val);

		private bool Pull()
		{
			while (_output.Count == 0)
			{
				if (!_input.MoveNext())
					return false;
				Consume(_input.Current)
			}
			return true;
		}

		public IEnumerator<O> GetEnumerator()
		{
			while (_output.Count > 0 || Pull())
			{
				yield return _output.Dequeue();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
