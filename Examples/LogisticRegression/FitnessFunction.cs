using System;
using Genesis.Elements;
using Genesis.Evaluation;

namespace LogisticRegression
{
	public class FitnessFunction : IFitnessFunction
	{
		readonly uint numSamples;
		readonly double min;
		readonly double max;
		readonly Func<double, double> function;
		readonly ValuedObject x;

		public FitnessFunction(
			Func<double, double> function, ValuedObject valued, uint numSamples, double min, double max)
		{
			this.x = valued;
			this.function = function;
			this.max = max;
			this.min = min;
			this.numSamples = numSamples;
		}

		public double Evaluate(IElement element)
		{
			var delta = (this.max - this.min) / this.numSamples;
			var error = 0d;
			for (var i = 0; i < numSamples; i++)
			{
				this.x.Value = this.min + (delta * i);
				var y = this.function(this.x.Value);
				var estimate = element.GetValue();
				var diff = y - estimate;
				error += diff * diff;
			}
			return double.IsNaN(error) || double.IsInfinity(error) ? double.MinValue : -Math.Sqrt(error);
		}

		public int Compare(IElement x, IElement y)
		{
			// x is best (higher) than y if its fitness is higher and the expression is shorter (in case of tie)
			var fitnessDiff = this.Evaluate(x).CompareTo(this.Evaluate(y));
			return fitnessDiff.Equals(0) ? -x.Count.CompareTo(y.Count) : fitnessDiff;
		}
	}
}
