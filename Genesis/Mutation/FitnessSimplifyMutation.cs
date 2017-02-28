using System;
using Genesis.Elements;
using Genesis.Evaluation;

namespace Genesis.Mutation
{
	/// <summary>
	/// This mutation operator tries to simplify (shorten the expression of) a given <see cref="IElement"/> by
	/// removing descendant elements that do not affect its fitness by some degree.
	/// </summary>
	public class FitnessSimplifyMutation : IMutationOperator
	{
		private readonly IFitnessFunction _fitnessFunction;
		private readonly double _epsilon;

		public FitnessSimplifyMutation(IFitnessFunction fitnessFunction, double epsilon)
		{
			this._epsilon = epsilon;
			this._fitnessFunction = fitnessFunction;
		}

		public IElement Mutate(IElement element)
		{
			return element.Simplify(this._fitnessFunction, this._epsilon);
		}

		public void Dispose()
		{
		}
	}
}
