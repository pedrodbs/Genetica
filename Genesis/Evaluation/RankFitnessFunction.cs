using Genesis.Elements;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Genesis.Evaluation
{
	public abstract class RankFitnessFunction : IFitnessFunction
	{
		protected readonly Dictionary<IElement, int> individualRankings = new Dictionary<IElement, int>();

		public double SelectivePressure { get; set; } = 1.5;

		protected RankFitnessFunction(IComparer<IElement> elementComparer, IPopulation population)
		{
			// sort the population according to the original fitness function
			var sortedPopulation = population.ToList();
			sortedPopulation.Sort(elementComparer);

			// store the ranking for each individual
			for (var i = 0; i < sortedPopulation.Count; i++)
				this.individualRankings.Add(sortedPopulation[i], i);
		}

		public abstract double Evaluate(IElement element);

		public int Compare(IElement x, IElement y)
		{
			return this.individualRankings[x].CompareTo(this.individualRankings[y]);
		}
	}
}
