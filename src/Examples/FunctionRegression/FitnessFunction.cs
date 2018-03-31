// ------------------------------------------
// <copyright file="FitnessFunction.cs" company="Pedro Sequeira">
// 
//     Copyright (c) 2018 Pedro Sequeira
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </copyright>
// <summary>
//    Project: FunctionRegression
//    Last updated: 03/26/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using Genesis.Elements;
using Genesis.Elements.Terminals;
using Genesis.Evaluation;

namespace FunctionRegression
{
    public class FitnessFunction : IFitnessFunction<MathProgram>
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

        public int Compare(MathProgram x, MathProgram y)
        {
            // x is best (higher) than y if its fitness is higher and the expression is shorter (in case of tie)
            var fitnessDiff = this.Evaluate(x).CompareTo(this.Evaluate(y));
            return fitnessDiff.Equals(0) ? -x.Length.CompareTo(y.Length) : fitnessDiff;
        }

        public double Evaluate(MathProgram program)
        {
            var delta = (this._max - this._min) / this._numSamples;
            var error = 0d;
            for (var i = 0; i < this._numSamples; i++)
            {
                this._x.Value = this._min + delta * i;
                var y = this._function(this._x.Value);
                var estimate = program.Compute();
                var diff = y - estimate;
                error += diff * diff;
            }
            return double.IsNaN(error) || double.IsInfinity(error) ? double.MinValue : -Math.Sqrt(error);
        }

        #endregion
    }
}