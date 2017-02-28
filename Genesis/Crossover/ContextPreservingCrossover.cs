using Genesis.Elements;
using System;
using MathNet.Numerics.Random;

namespace Genesis.Crossover
{
	/// <summary>
	/// Creates offsrping by choosing a random sub-element of the first parent and replacing with a sub-element of the 
	/// second parent that has the same index.
	/// </summary>
	public class ContextPreservingCrossover : ICrossoverOperator
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public IElement Crossover(IElement parent1, IElement parent2)
		{
			// checks equal parents, returns first
			if (parent1.Equals(parent2)) return parent1;

			// checks shorter tree
			var maxIndex = (int)Math.Min(parent1.Count, parent2.Count);

			// gets random crossover point
			var crossoverPoint = (uint)this._random.Next(maxIndex);

			// gets corresponding element in parent 2
			var element = parent2.ElementAt(crossoverPoint);

			// replaces sub-element of parent 1 by the one of parent 2
			return parent1.Replace(crossoverPoint, element);
		}

		public void Dispose()
		{
		}
	}
}
