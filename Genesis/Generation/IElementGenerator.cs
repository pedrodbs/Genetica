using Genesis.Elements;
using System;

namespace Genesis.Generation
{
	public interface IElementGenerator : IDisposable
	{
		IElement Generate(PrimitiveSet primitives, uint maxDepth);
	}
}
