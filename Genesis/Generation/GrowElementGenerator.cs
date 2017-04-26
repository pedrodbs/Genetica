using Genesis.Util;
using Genesis.Elements;
using System.Linq;
using System;
using MathNet.Numerics.Random;

namespace Genesis.Generation
{
	public class GrowElementGenerator : IElementGenerator
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		public void Dispose()
		{
		}

		public IElement Generate(PrimitiveSet primitives, uint maxDepth)
		{
			return this.Generate(primitives, 0, maxDepth);
		}

		private IElement Generate(PrimitiveSet primitives, uint depth, uint maxDepth)
		{
			// check max depth, just return a random terminal element
			if (depth == maxDepth)
				return primitives.Terminals.ToList<IElement>().GetRandomItem(this._random);

			// otherwise, get a random primitive
			var primitivesList = primitives.Functions.ToList<IElement>();
			primitivesList.AddRange(primitives.Terminals);
			var element = primitivesList.GetRandomItem(this._random);

			// recursively generate random children for it
			var numChildren = element.Children.Count;
			var children = new IElement[numChildren];
			for (var i = 0; i < numChildren; i++)
				children[i] = Generate(primitives, depth + 1, maxDepth);

			// generate a new element with the children
			return element.CreateNew(children);
		}
	}
}
