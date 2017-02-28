using System;
using Genesis.Elements;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
	public class HoistMutation : IMutationOperator
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public void Dispose()
		{
		}

		public IElement Mutate(IElement element)
		{
			if (element == null) return null;

			// define the mutation point randomly
			var mutatePoint = (uint)this._random.Next(element.Count);

			// return the sub-element
			return element.ElementAt(mutatePoint);
		}
	}
}
