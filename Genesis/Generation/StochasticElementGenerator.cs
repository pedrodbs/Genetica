using Genesis.Util;
using System.Collections.Generic;
using Genesis.Elements;
using System.Linq;
using MathNet.Numerics.Random;
using System;

namespace Genesis.Generation
{
	public sealed class StochasticElementGenerator : IElementGenerator
	{
		private readonly IDictionary<IElementGenerator, double> _possibleGenerators;
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public StochasticElementGenerator(IList<IElementGenerator> possibleGenerators)
		{
			this._possibleGenerators = possibleGenerators.ToDictionary(x => x, x => 1d / possibleGenerators.Count);
		}

		public StochasticElementGenerator(IDictionary<IElementGenerator, double> possibleGenerators)
		{
			this._possibleGenerators = possibleGenerators;
		}

		public IElement Generate(PrimitiveSet primitives, uint maxDepth)
		{
			return this._possibleGenerators.GetRandomItem(this._random).Generate(primitives, maxDepth);
		}

		#region IDisposable Support

		private bool _disposed;

		private void Dispose(bool disposing)
		{
			if (this._disposed) return;
			if (disposing) this._possibleGenerators.Clear();
			this._disposed = true;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
