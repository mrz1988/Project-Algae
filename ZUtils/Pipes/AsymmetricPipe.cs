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
			_output = new Queue<O>();
		}

		public virtual List<O> PumpAll()
		{
			var output = new List<O>();
			foreach (var item in this)
			{
				output.Add(item);
			}

			return output;
		}

		protected virtual void Output(IEnumerable<O> items)
		{
			foreach (var i in items)
				Output(i);
		}

		protected virtual void Output(O item)
		{
			_output.Enqueue(item);
		}

		protected abstract void Consume(I val);
		protected abstract void Finish();

		private bool Pull()
		{
			while (_output.Count == 0)
			{
				if (!_input.MoveNext())
					return false;
				Consume(_input.Current);
			}
			return true;
		}

		public IEnumerator<O> GetEnumerator()
		{
			while (_output.Count > 0 || Pull())
			{
				yield return _output.Dequeue();
			}

			Finish();

			while (_output.Count > 0)
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
