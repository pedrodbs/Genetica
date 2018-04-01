// ------------------------------------------
// <copyright file="StochasticSelection.cs" company="Pedro Sequeira">
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

namespace Genesis.Operators.Selection
{
    /// <summary>
    ///     Represents a generic <see cref="ISelectionOperator{TProgram}" /> that selects from a list of selection operators at
    ///     random.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public class StochasticSelection<TProgram> : ISelectionOperator<TProgram> where TProgram : IProgram
    {
        #region Fields

        private readonly IDictionary<ISelectionOperator<TProgram>, double> _possibleSelections;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="StochasticSelection{TProgram}" /> with the given list of operators to choose from. All
        ///     operators will have the same probability of being selected (uniform distribution).
        /// </summary>
        /// <param name="possibleSelections">The list of selection operators to be used by this operator.  </param>
        public StochasticSelection(ICollection<ISelectionOperator<TProgram>> possibleSelections)
        {
            this._possibleSelections = possibleSelections.ToDictionary(x => x, x => 1d / possibleSelections.Count);
        }

        /// <summary>
        ///     Creates a new <see cref="StochasticSelection{TProgram}" /> with the given list of operators to choose from.
        ///     Operators will be selected according to the corresponding probability.
        /// </summary>
        /// <param name="possibleSelections">
        ///     A list of selection operators to be used by this operator and the corresponding selection probabilities.
        /// </param>
        public StochasticSelection(
            IDictionary<ISelectionOperator<TProgram>, double> possibleSelections)
        {
            this._possibleSelections = possibleSelections;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Dispose()
        {
            this._possibleSelections.Clear();
        }

        /// <inheritdoc />
        public IEnumerable<TProgram> Select(IPopulation<TProgram> population) =>
            this._possibleSelections.GetRandomItem(this._random).Select(population);

        #endregion
    }
}