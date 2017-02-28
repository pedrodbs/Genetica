using System;
using Genesis.Elements;
using System.Collections.Generic;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
	public class PointMutation : IMutationOperator, IDisposable
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());
		private readonly Dictionary<int, List<IElement>> _primitives = new Dictionary<int, List<IElement>>();

		public double MutationProbability { get; set; }

		public PointMutation(PrimitiveSet primitives, double mutationProbability = 0.5d)
		{
			// stores primitives as a function of their arity (0, 1, ...)
			var allPrimitives = new List<IElement>(primitives.Functions);
			allPrimitives.AddRange(primitives.Terminals);
			foreach (var primitive in allPrimitives)
			{
				var arity = primitive.Children.Count;
				if (!this._primitives.ContainsKey(arity))
					this._primitives.Add(arity, new List<IElement>());
				this._primitives[arity].Add(primitive);
			}

			this.MutationProbability = mutationProbability;
		}

		public void Dispose()
		{
			this._primitives.Clear();
		}

		public IElement Mutate(IElement element)
		{
			if (element == null) return null;
			if (element.Children == null) return element;

			// mutates all children
			var numChildren = element.Children.Count;
			var newChildren = new IElement[numChildren];
			for (var i = 0; i < numChildren; i++)
				newChildren[i] = this.Mutate(element.Children[i]);

			// checks whether to mutate this element (otherwise use same element)
			var primitive = element;
			if ((this._random.NextDouble() < this.MutationProbability) && this._primitives.ContainsKey(numChildren))
			{
				// mutates by creating a new random element with same parity and same children
				primitive = this._primitives[numChildren].GetRandomItem(this._random);
			}

			// creates new element with new children
			return primitive.CreateNew(newChildren);
		}
	}
}
