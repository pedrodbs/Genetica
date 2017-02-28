using System.Collections.Generic;
using Genesis.Elements;
using System;

namespace Genesis.Selection
{
	public interface ISelectionOperator : IDisposable
	{
		IEnumerable<IElement> Select(IPopulation population);
	}
}
