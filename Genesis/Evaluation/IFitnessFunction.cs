using Genesis.Elements;
using System.Collections.Generic;

namespace Genesis.Evaluation
{
	public interface IFitnessFunction : IComparer<IElement>
	{
		double Evaluate(IElement element);
	}
}
