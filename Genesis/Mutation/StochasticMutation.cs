using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
	public class StochasticMutation : IMutationOperator
	{
		private readonly IDictionary<IMutationOperator, double> _possibleMutations;
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public StochasticMutation(ICollection<IMutationOperator> possibleSelections)
		{
			this._possibleMutations = possibleSelections.ToDictionary(x => x, x => 1d / possibleSelections.Count);
		}

		public StochasticMutation(IDictionary<IMutationOperator, double> possibleMutations)
		{
			this._possibleMutations = possibleMutations;
		}

		/// <summary>
		/// Randomly chooses one of the <see cref="IMutationOperator"/>s to perform the mutation.
		/// </summary>
		/// <returns>A new element corresponding to the given element mutated.</returns>
		/// <param name="element">An element to be mutated.</param>
		public IElement Mutate(IElement element)
		{
			return this._possibleMutations.GetRandomItem(this._random).Mutate(element);
		}

		#region IDisposable Support

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed) return;
			if (disposing) this._possibleMutations.Clear();
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
