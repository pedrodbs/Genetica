using System;
using Genesis.Elements;
using Genesis.Generation;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
	public class SubtreeMutation : IMutationOperator
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());
		private readonly IElementGenerator _elementGenerator;
		private readonly PrimitiveSet _primitives;

		public uint MaxDepth { get; set; }

		public SubtreeMutation(IElementGenerator elementGenerator, PrimitiveSet primitives, uint maxDepth)
		{
			this._primitives = primitives;
			this._elementGenerator = elementGenerator;
			this.MaxDepth = maxDepth;
		}

		public IElement Mutate(IElement element)
		{
			if (element == null) return null;

			// define the mutation point randomly
			var mutatePoint = (uint)this._random.Next(element.Count);

			// define the new random element and creates replacement
			var newElem = this._elementGenerator.Generate(this._primitives, this.MaxDepth);
			return element.Replace(mutatePoint, newElem);
		}

		public void Dispose()
		{
		}
	}
}
