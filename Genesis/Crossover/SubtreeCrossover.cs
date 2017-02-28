using Genesis.Elements;
using System;
using Genesis.Elements.Functions;
using MathNet.Numerics.Random;

namespace Genesis.Crossover
{
	public class SubtreeCrossover : ICrossoverOperator
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public IElement Crossover(IElement parent1, IElement parent2)
		{
			// define the first crossover point as a random function of parent 1
			uint crossPoint1;
			IElement elem;
			do
			{
				crossPoint1 = (uint)this._random.Next(parent1.Count);
				elem = parent1.ElementAt(crossPoint1);
			} while ((elem == null) || ((parent1.Children.Count > 0) && !(elem is IFunction)));

			// define the second crossover point as a random element of parent 2
			elem = parent2.ElementAt((uint)this._random.Next(parent2.Count));

			return parent1.Replace(crossPoint1, elem);
		}

		public void Dispose() { }
	}
}
