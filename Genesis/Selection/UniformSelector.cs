using System;
using MathNet.Numerics.Random;

namespace Genesis.Selection
{
	public class UniformSelector : IPopulationSelector
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		/// <summary>
		/// Gets an array of pointers that are equally spread.
		/// </summary>
		/// <returns>The selection pointers.</returns>
		/// <param name="numPointers">The number of pointers.</param>
		public double[] GetSelectionPointers(uint numPointers)
		{
			var delta = 1d / numPointers;
			var initDelta = this._random.NextDouble() * delta;
			var pointers = new double[numPointers];
			for (var i = 0; i < numPointers; i++)
				pointers[i] = initDelta + (i * delta);
			return pointers;
		}
	}
}
