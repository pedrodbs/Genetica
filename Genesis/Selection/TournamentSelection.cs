using System;
using System.Collections.Generic;
using Genesis.Elements;
using System.Linq;
using MathNet.Numerics.Random;

namespace Genesis.Selection
{
	public class TournamentSelection : ISelectionOperator
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());
		private readonly IComparer<IElement> _elementComparer;

		public uint TournamentSize { get; }

		public TournamentSelection(IComparer<IElement> elementComparer, uint tournamentSize = 1)
		{
			this._elementComparer = elementComparer;
			this.TournamentSize = tournamentSize;
		}

		public IEnumerable<IElement> Select(IPopulation population)
		{
			var popList = population.ToList();
			var tourSize = (uint)Math.Max(Math.Min(this.TournamentSize, population.Count), 1);

			// makes tournaments of selected size
			var selection = new IElement[population.Count];
			for (var i = 0; i < population.Count; i++)
				selection[i] = this.PlayTournament(popList, tourSize);
			return selection;
		}

		private IElement PlayTournament(List<IElement> popList, uint tourSize)
		{
			var indexes = new HashSet<int>();
			var best = popList[0];
			for (var i = 0; i < tourSize; i++)
			{
				//selects individual at random from pop (no repetition)
				int index;
				do
				{
					index = this._random.Next(popList.Count);
				} while (indexes.Contains(index));
				indexes.Add(index);

				// checks max fitenss
				var individual = popList[index];
				if (this._elementComparer.Compare(individual, best) > 0)
					best = individual;
			}
			return best;
		}

		public void Dispose()
		{
		}
	}
}
