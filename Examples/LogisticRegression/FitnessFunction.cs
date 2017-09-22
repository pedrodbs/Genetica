// ------------------------------------------
// <copyright file="FitnessFunction.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis.Examples.LogisticRegression
//    Last updated: 2017/09/08
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using Genesis.Elements;
using Genesis.Elements.Terminals;
using Genesis.Evaluation;

namespace Genesis.Examples.LogisticRegression
{
    public class FitnessFunction : IFitnessFunction
    {
        #region Fields

        private readonly Func<double, double> _function;
        private readonly double _max;
        private readonly double _min;
        private readonly uint _numSamples;
        private readonly Variable _x;

        #endregion

        #region Constructors

        public FitnessFunction(
            Func<double, double> function, Variable variable, uint numSamples, double min, double max)
        {
            this._x = variable;
            this._function = function;
            this._max = max;
            this._min = min;
            this._numSamples = numSamples;
        }

        #endregion

        #region Public Methods

        public int Compare(IElement x, IElement y)
        {
            // x is best (higher) than y if its fitness is higher and the expression is shorter (in case of tie)
            var fitnessDiff = this.Evaluate(x).CompareTo(this.Evaluate(y));
            return fitnessDiff.Equals(0) ? -x.Length.CompareTo(y.Length) : fitnessDiff;
        }

        public double Evaluate(IElement element)
        {
            var delta = (this._max - this._min) / this._numSamples;
            var error = 0d;
            for (var i = 0; i < this._numSamples; i++)
            {
                this._x.Value = this._min + delta * i;
                var y = this._function(this._x.Value);
                var estimate = element.GetValue();
                var diff = y - estimate;
                error += diff * diff;
            }
            return double.IsNaN(error) || double.IsInfinity(error) ? double.MinValue : -Math.Sqrt(error);
        }

        #endregion
    }
}