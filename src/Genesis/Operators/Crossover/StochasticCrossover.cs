// ------------------------------------------
// <copyright file="StochasticCrossover.cs" company="Pedro Sequeira">
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

using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Operators.Crossover
{
    /// <summary>
    ///     Represents a generic <see cref="ICrossoverOperator{TProgram}" /> that selects from a list of
    ///     crossover operators at random.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public class StochasticCrossover<TProgram> : ICrossoverOperator<TProgram> where TProgram : IProgram
    {
        #region Fields

        private readonly IDictionary<ICrossoverOperator<TProgram>, double> _possibleCrossovers;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="StochasticCrossover{TProgram}" /> with the given list of operators to
        ///     choose from. All operators will have the same probability of being selected (uniform distribution).
        /// </summary>
        /// <param name="possibleCrossovers">The list of crossover operators to be used by this operator.  </param>
        public StochasticCrossover(IList<ICrossoverOperator<TProgram>> possibleCrossovers)
        {
            this._possibleCrossovers = possibleCrossovers.ToDictionary(x => x, x => 1d / possibleCrossovers.Count);
        }

        /// <summary>
        ///     Creates a new <see cref="StochasticCrossover{TProgram}" /> with the given list of operators to
        ///     choose from. Operators will be selected according to the corresponding probability.
        /// </summary>
        /// <param name="possibleCrossovers">
        ///     A list of crossover operators to be used by this operator and the corresponding selection probabilities.
        /// </param>
        public StochasticCrossover(IDictionary<ICrossoverOperator<TProgram>, double> possibleCrossovers)
        {
            this._possibleCrossovers = possibleCrossovers;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public TProgram Crossover(TProgram parent1, TProgram parent2) =>
            this._possibleCrossovers.GetRandomItem(this._random).Crossover(parent1, parent2);

        /// <inheritdoc />
        public IEnumerable<TProgram> GetAllOffspring(TProgram parent1, TProgram parent2)
        {
            var offspring = new HashSet<TProgram>();
            foreach (var crossover in this._possibleCrossovers.Keys)
                offspring.AddRange(crossover.GetAllOffspring(parent1, parent2));
            return offspring;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._possibleCrossovers.Clear();
        }

        #endregion
    }
}