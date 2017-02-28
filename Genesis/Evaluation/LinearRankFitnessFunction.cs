using System.Collections.Generic;
using Genesis.Elements;

namespace Genesis.Evaluation
{
	public class LinearRankFitnessFunction : RankFitnessFunction
	{
		public LinearRankFitnessFunction(IComparer<IElement> elementComparer, IPopulation population)
			: base(elementComparer, population)
		{
		}

		public override double Evaluate(IElement element)
		{
			if (!this.individualRankings.ContainsKey(element)) return -1d;

			// gets a changed fitness according to the ranking
			return 2d - this.SelectivePressure +
						   (2d * (this.SelectivePressure - 1d) *
							((this.individualRankings[element] - 1d) / (this.individualRankings.Count - 1d)));
		}
	}
}
