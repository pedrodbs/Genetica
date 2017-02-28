using System;
using System.Collections.Generic;
using Genesis.Elements;
using Genesis.Evaluation;

namespace Genesis.Selection
{
	public class RouletteWheelSelection : ISelectionOperator
	{
		private readonly IPopulationSelector _selector;
		private readonly IFitnessFunction _fitnessFunction;

		public RouletteWheelSelection(IPopulationSelector selector, IFitnessFunction fitnessFunction)
		{
			this._fitnessFunction = fitnessFunction;
			this._selector = selector;
		}

		public IEnumerable<IElement> Select(IPopulation population)
		{
			// stores elements in a list, gets fitness sum
			var popList = new IElement[population.Count];
			var totalFitness = 0d;
			var i = 0;
			foreach (var individual in population)
			{
				totalFitness += this._fitnessFunction.Evaluate(individual);
				popList[i++] = individual;
			}

			// gets selection pointers (from 0 to 1) and sorts them
			var pointers = this._selector.GetSelectionPointers((uint)population.Count);
			Array.Sort(pointers);

			// selects elements according to pointers
			var pointerIndex = 0;
			var indivIndex = 0;
			var curFitSum = 0d;
			var selectionSize = Math.Min(pointers.Length, population.Count);
			var selection = new IElement[selectionSize];
			while (pointerIndex < selectionSize)
			{
				if ((pointers[pointerIndex] * totalFitness) < curFitSum)
				{
					selection[pointerIndex] = popList[indivIndex - 1];
					pointerIndex++;
				}
				else
				{
					curFitSum += this._fitnessFunction.Evaluate(popList[indivIndex++]);
				}
			}
			return selection;
		}

		public void Dispose()
		{
		}
	}
}
