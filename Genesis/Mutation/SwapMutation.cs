using System;
using Genesis.Elements;
using System.Linq;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
	public class SwapMutation : IMutationOperator
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

			// get sub-element and swap children (inverse order)
			var elem = element.ElementAt(mutatePoint);
			if ((elem.Children == null) || (elem.Children.Count < 2)) return elem;
			var children = elem.Children.Reverse().ToList();

			// returns a replaced sub-element
			return elem.Replace(mutatePoint, elem.CreateNew(children));
		}
	}
}
