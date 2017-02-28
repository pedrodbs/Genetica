using System;
using Genesis.Elements;
using System.Collections.Generic;
using Genesis.Elements.Terminals;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
	/// <summary>
	/// This mutation operator removes a random descendant node of a given <see cref="IElement"/> and replaces it with
	/// a random <see cref="Terminal"/> element from a given <see cref="PrimitiveSet"/>.
	/// </summary>
	public class ShrinkMutation : IMutationOperator
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());
		private readonly IList<Terminal> _terminals;

		public ShrinkMutation(PrimitiveSet primitives)
		{
			this._terminals = new List<Terminal>(primitives.Terminals);
		}

		public void Dispose()
		{
		}

		public IElement Mutate(IElement element)
		{
			// define the mutation point randomly
			var mutatePoint = (uint)this._random.Next(element.Count);

			// replaces with a new random terminal element
			return element.Replace(mutatePoint, this._terminals.GetRandomItem(this._random));
		}
	}
}
