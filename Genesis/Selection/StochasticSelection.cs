using System.Collections.Generic;
using Genesis.Elements;
using Genesis.Util;
using System.Linq;
using System;
using MathNet.Numerics.Random;

namespace Genesis.Selection
{
	public class StochasticSelection : ISelectionOperator
	{
		private readonly IDictionary<ISelectionOperator, double> _possibleSelections;
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public StochasticSelection(IList<ISelectionOperator> possibleSelections)
		{
			this._possibleSelections = possibleSelections.ToDictionary(x => x, x => 1d / possibleSelections.Count);
		}

		public StochasticSelection(IDictionary<ISelectionOperator, double> possibleSelections)
		{
			this._possibleSelections = possibleSelections;
		}

		public IEnumerable<IElement> Select(IPopulation population)
		{
			return this._possibleSelections.GetRandomItem(this._random).Select(population);
		}

		#region IDisposable Support

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed) return;
			if (disposing) this._possibleSelections.Clear();
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
