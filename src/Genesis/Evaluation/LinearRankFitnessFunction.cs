// ------------------------------------------
// <copyright file="LinearRankFitnessFunction.cs" company="Pedro Sequeira">
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
//    Last updated: 03/23/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using Genesis.Elements;

namespace Genesis.Evaluation
{
    /// <summary>
    ///     Represents a <see cref="RankFitnessFunction{TProgram}" /> that computes the fitness of a program directly according
    ///     to its rank.
    /// </summary>
    /// <remarks><see cref="http://www.pohlheim.com/Papers/mpga_gal95/gal2_3.html"/></remarks>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public class LinearRankFitnessFunction<TProgram> : RankFitnessFunction<TProgram> where TProgram : IProgram
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="LinearRankFitnessFunction{TProgram}" /> with the given parameters.
        /// </summary>
        /// <param name="programComparer">The object used to compare programs and determine their ranking.</param>
        /// <param name="population">The list of programs whose fitness can be computed by this function.</param>
        public LinearRankFitnessFunction(IComparer<TProgram> programComparer, IPopulation<TProgram> population)
            : base(programComparer, population)
        {
        }

        #endregion

        #region Public Methods

        public override double Evaluate(TProgram program)
        {
            if (!this.individualRankings.ContainsKey(program)) return -1d;

            // gets a changed fitness according to the ranking
            return 2d - this.SelectivePressure +
                   2d * (this.SelectivePressure - 1d) *
                   ((this.individualRankings[program] - 1d) / (this.individualRankings.Count - 1d));
        }

        #endregion
    }
}