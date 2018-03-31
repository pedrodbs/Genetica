// ------------------------------------------
// <copyright file="NonLinearRankFitnessFunction.cs" company="Pedro Sequeira">
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
//    Project: Genesis
//    Last updated: 03/28/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Genesis.Elements;
using MathNet.Numerics;
using MathNet.Numerics.RootFinding;

namespace Genesis.Evaluation
{
    /// <summary>
    ///     Calculates a transformed fitness based on a polynomial ranking. It implements the function in [1].
    /// </summary>
    /// <remarks>
    ///     [1] - Pohlheim, H. (1995). The multipopulation genetic algorithm: Local selection and migration. Technical report,
    ///     Technical University Ilmenau.
    ///     <see href="http://www.pohlheim.com/Papers/mpga_gal95/gal2_3.html" />
    ///     <see href="http://www.geatbx.com/docu/algindex-02.html#P249_16387" />
    /// </remarks>
    public class NonLinearRankFitnessFunction<TProgram> : LinearRankFitnessFunction<TProgram> where TProgram : IProgram
    {
        #region Fields

        private readonly double _root;
        private readonly double _rootFactor;
        private readonly bool _rootFound;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="NonLinearRankFitnessFunction{TProgram}" /> with the given parameters.
        /// </summary>
        /// <param name="programComparer">The object used to compare programs and determine their ranking.</param>
        /// <param name="population">The list of programs whose fitness can be computed by this function.</param>
        /// <param name="selectivePressure">
        ///     The probability of the best individual being selected compared to the average probability of selection of all
        ///     individuals.
        /// </param>
        public NonLinearRankFitnessFunction(
            IComparer<TProgram> programComparer, IPopulation<TProgram> population, double selectivePressure)
            : base(programComparer, population)
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

        #endregion

        #region Public Methods

        public override double Evaluate(TProgram program)
        {
            return !this._rootFound || !this.individualRankings.ContainsKey(program)
                ? base.Evaluate(program)
                : this._rootFactor * Math.Pow(this._root, this.individualRankings[program]);
        }

        #endregion

        #region Private & Protected Methods

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

            val += (sp - 1) * pow * x;
            return val;
        }

        #endregion
    }
}