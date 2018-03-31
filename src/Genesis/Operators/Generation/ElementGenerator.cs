using System;
using Genesis.Elements;
using System.Collections.Generic;

namespace Genesis.Generation
{
	public abstract class ElementGenerator : IElementGenerator
	{
		private static readonly Random Random = new Random();

		public abstract IElement Generate(PrimitiveSet primitives, uint maxDepth);

		protected static IElement GetRandomElement(IList<IElement> elements)
		{
			return elements[Random.Next(elements.Count)];
		}
	}
}
