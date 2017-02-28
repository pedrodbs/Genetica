using Genesis.Elements;
using System;
using MathNet.Numerics.Random;

namespace Genesis.Crossover
{
	/// <summary>
	/// Creates offspring by visiting the points in the common region between the parents and flipping a coin at each 
	/// point to decide whether the corresponding offspring sub-element should be picked from the first or the second
	/// parent.
	/// </summary>
	/// <remarks>The common region between the parent elements corresponds to the subtrees where the parents have the 
	/// same shape.
	/// </remarks>
	public class UniformCrossover : ICrossoverOperator
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public IElement Crossover(IElement parent1, IElement parent2)
		{
			// checks equal parents, returns first
			if (parent1.Equals(parent2)) return parent1;

			// gets common region between parents as a correspondence between indexes
			var commonRegion = parent1.GetCommonRegionIndexes(parent2);

			// starts with parent 1
			var element = parent1;

			// for each common point, replace with the corresponding sub-element of parent 2 with 50% chance
			foreach (var indexes in commonRegion)
				if (this._random.Next(2) == 0)
					element = parent1.Replace(indexes.Key, parent2.ElementAt(indexes.Value));

			return element;
		}

		public void Dispose()
		{
		}
	}
}
