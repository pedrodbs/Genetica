using System;
using Genesis.Elements;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
	/// <summary>
	/// This mutation operator simplifies (tries to shorten the expression of) a random descendant element of a given 
	/// <see cref="IElement"/>.
	/// </summary>
	public class SimplifyMutation : IMutationOperator
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public void Dispose()
		{
		}

		public IElement Mutate(IElement element)
		{
			// define the mutation point randomly
			var mutatePoint = (uint)this._random.Next(element.Count);

			// replaces with a simplified version of the sub-element
			var simp = element.ElementAt(mutatePoint).Simplify();
			return element.Replace(mutatePoint, simp);
			//return element.Simplify();
		}
	}
}
