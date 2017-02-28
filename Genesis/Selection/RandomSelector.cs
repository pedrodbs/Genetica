using System;
using MathNet.Numerics.Random;

namespace Genesis.Selection
{
	public class RandomSelector : IPopulationSelector
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		/// <summary>
		/// Gets an array of pointers that are unevenly (randomly) spread.
		/// </summary>
		/// <returns>The pointers.</returns>
		/// <param name="numPointers">The number of pointers.</param>
		public double[] GetSelectionPointers(uint numPointers)
		{
			var pointers = new double[numPointers];
			for (var i = 0; i < numPointers; i++)
				pointers[i] = this._random.NextDouble();
			return pointers;
		}
	}
}
