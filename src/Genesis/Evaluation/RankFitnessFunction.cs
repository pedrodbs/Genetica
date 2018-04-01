// ------------------------------------------
// <copyright file="RankFitnessFunction.cs" company="Pedro Sequeira">
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
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;

namespace Genesis.Evaluation
{
    /// <summary>
    ///     Represents an <see cref="IFitnessFunction{TProgram}" /> that computes a fitness based on the relative ranking of
    ///     elements as given by some <see cref="IComparer{T}" />.
    /// </summary>
    /// <remarks>
    ///     <see href="http://www.pohlheim.com/Papers/mpga_gal95/gal2_3.html" />
    /// </remarks>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public abstract class RankFitnessFunction<TProgram> : IFitnessFunction<TProgram> where TProgram : IProgram
    {
        #region Fields

        protected readonly Dictionary<TProgram, int> individualRankings = new Dictionary<TProgram, int>();

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="RankFitnessFunction{TProgram}" /> with the given parameters.
        /// </summary>
        /// <param name="programComparer">The object used to compare programs and determine their ranking.</param>
        /// <param name="population">The list of programs whose fitness can be computed by this function.</param>
        protected RankFitnessFunction(IComparer<TProgram> programComparer, IPopulation<TProgram> population)
        {
            // sort the population according to the original fitness function
            var sortedPopulation = population.ToList();
            sortedPopulation.Sort(programComparer);

            // store the ranking for each individual
            for (var i = 0; i < sortedPopulation.Count; i++)
                this.individualRankings.Add(sortedPopulation[i], i);
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets or sets the "probability of the best individual being selected compared to the average probability of
        ///     selection of all individuals". The value should be in [1.0, 2.0].
        /// </summary>
        /// <remarks>
        ///     <see href="http://www.pohlheim.com/Papers/mpga_gal95/gal2_3.html" />
        /// </remarks>
        public double SelectivePressure { get; set; } = 1.5d;

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public int Compare(TProgram x, TProgram y) => this.individualRankings[x].CompareTo(this.individualRankings[y]);

        /// <inheritdoc />
        public abstract double Evaluate(TProgram program);

        #endregion
    }
}