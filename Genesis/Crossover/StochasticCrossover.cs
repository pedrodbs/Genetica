using Genesis.Elements;
using System.Collections.Generic;
using Genesis.Util;
using System.Linq;
using System;
using MathNet.Numerics.Random;

namespace Genesis.Crossover
{
	public class StochasticCrossover : ICrossoverOperator
	{
		private readonly IDictionary<ICrossoverOperator, double> _possibleCrossovers;
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public StochasticCrossover(IList<ICrossoverOperator> possibleCrossovers)
		{
			this._possibleCrossovers = possibleCrossovers.ToDictionary(x => x, x => 1d / possibleCrossovers.Count);
		}

		public StochasticCrossover(IDictionary<ICrossoverOperator, double> possibleCrossovers)
		{
			this._possibleCrossovers = possibleCrossovers;
		}

		public IElement Crossover(IElement parent1, IElement parent2)
		{
			return this._possibleCrossovers.GetRandomItem(this._random).Crossover(parent1, parent2);
		}

		#region IDisposable Support

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed) return;
			if (disposing) this._possibleCrossovers.Clear();
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
