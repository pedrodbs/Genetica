using Genesis.Elements;
using System;
using MathNet.Numerics.RootFinding;
using MathNet.Numerics;
using System.Collections.Generic;

namespace Genesis.Evaluation
{
	/// <summary>
	/// Calculates a transformed fitness based on a non-linear rank.
	/// <see href="http://www.pohlheim.com/Papers/mpga_gal95/gal2_3.html"/>
	/// <see href="http://www.geatbx.com/docu/algindex-02.html#P249_16387"/> 
	/// </summary>
	public class NonLinearRankFitnessFunction : LinearRankFitnessFunction
	{
		private readonly double _root;
		private readonly double _rootFactor;
		private readonly bool _rootFound;

		public NonLinearRankFitnessFunction(
			IComparer<IElement> elementComparer, IPopulation population, double selectivePressure)
			: base(elementComparer, population)
		{
			var n = population.Count;
			this.SelectivePressure = Math.Max(1d, Math.Min(n - 2d, selectivePressure));

			// finds the root of the polynomial
			try
			{
				this._root = Brent.FindRootExpand(this.Polynomial, -n, n, 1E-08, 1000);
				this._rootFound = true;
			}
			catch (NonConvergenceException)
			{
			}

			// stores the exponential sum
			this._rootFactor = 0d;
			var pow = 1d;
			for (var i = 0; i < n; i++)
			{
				this._rootFactor += pow;
				pow *= this._root;
			}
			this._rootFactor = n / this._rootFactor;
		}

		private double Polynomial(double x)
		{
			// returns (sp-1) x^(n-1) + sp x^(n-2) + ... + sp x + sp
			var n = this.individualRankings.Count;
			var sp = this.SelectivePressure;
			var val = 0d;
			var pow = 1d;
			for (var i = 0; i <= n - 2; i++)
			{
				val += sp * pow;
				pow *= x;
			}
			val += ((sp - 1) * pow * x);
			return val;
		}

		public override double Evaluate(IElement element)
		{
			return !this._rootFound || !this.individualRankings.ContainsKey(element)
				? base.Evaluate(element)
				: this._rootFactor * Math.Pow(this._root, this.individualRankings[element]);
		}
	}
}
