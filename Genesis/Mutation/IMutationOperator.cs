using Genesis.Elements;
using System;

namespace Genesis.Mutation
{
	public interface IMutationOperator : IDisposable
	{
		IElement Mutate(IElement element);
	}
}
