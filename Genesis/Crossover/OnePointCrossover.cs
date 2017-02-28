using Genesis.Elements;
using System.Linq;
using Genesis.Util;
using MathNet.Numerics.Random;
using System;

namespace Genesis.Crossover
{
	/// <summary>
	/// Creates offsrping by selecting a random crossover point in the common region betwen the two 
	/// <seealso cref="IElement"/> parents and then replacing a subtree of the first parent by the corresponding 
	/// subtree of the second parent.
	/// </summary>
	/// <remarks>The common region between the parent elements corresponds to the subtrees where the parents have the 
	/// same shape.
	/// </remarks>
	public class OnePointCrossover : ICrossoverOperator
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public IElement Crossover(IElement parent1, IElement parent2)
		{
			// checks equal parents, returns first
			if (parent1.Equals(parent2)) return parent1;

			// gets common region between parents as a correspondence between indexes
			var commonRegionIndexes = parent1.GetCommonRegionIndexes(parent2);

			// chooses random crossover point in parent 1
			var crossoverPoint = commonRegionIndexes.Keys.ToList().GetRandomItem(this._random);

			// gets corresponding element in parent 2
			var element = parent2.ElementAt(commonRegionIndexes[crossoverPoint]);

			// replaces sub-element of parent 1 by the one of parent 2
			return parent1.Replace(crossoverPoint, element);
		}

		public void Dispose()
		{
		}
	}
}
