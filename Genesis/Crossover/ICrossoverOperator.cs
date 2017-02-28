using Genesis.Elements;
using System;

namespace Genesis.Crossover
{
	public interface ICrossoverOperator : IDisposable
	{
		IElement Crossover(IElement parent1, IElement parent2);
	}
}
