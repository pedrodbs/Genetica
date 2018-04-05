// ------------------------------------------
// <copyright file="OnePointCrossover.cs" company="Pedro Sequeira">
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
//    Project: Genetica
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Genetica.Util;
using Genetica.Elements;
using MathNet.Numerics.Random;

namespace Genetica.Operators.Crossover
{
    /// <summary>
    ///     Represents a <see cref="ICrossoverOperator{TProgram}" /> for <see cref="ITreeProgram{TOutput}" />.
    ///     Creates offspring by selecting a random crossover point in the common region between the two
    ///     <see cref="ITreeProgram{TOutput}" /> parents and then replacing a subtree of the first parent by the corresponding
    ///     subtree of the second parent.
    /// </summary>
    /// <remarks>
    ///     The common region between the parent programs corresponds to the subtrees where the parents have the same shape.
    /// </remarks>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class OnePointCrossover<TProgram, TOutput> : ICrossoverOperator<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public TProgram Crossover(TProgram parent1, TProgram parent2)
        {
            if (parent1 == null || parent2 == null) return default(TProgram);

            // checks equal parents, returns first
            if (parent1.Equals(parent2)) return parent1;

            // gets common region between parents as a correspondence between indexes
            var commonRegionIndexes = parent1.GetCommonRegionIndexes(parent2);

            // chooses random crossover point in parent 1
            var crossoverPoint = commonRegionIndexes.Keys.ToList().GetRandomItem(this._random);

            // gets corresponding program in parent 2
            var program = parent2.ProgramAt(commonRegionIndexes[crossoverPoint]);

            // replaces sub-program of parent 1 by the one of parent 2
            return (TProgram) parent1.Replace(crossoverPoint, program);
        }

        /// <inheritdoc />
        public IEnumerable<TProgram> GetAllOffspring(TProgram parent1, TProgram parent2)
        {
            if (parent1 == null || parent2 == null) return new List<TProgram>();

            // checks equal parents, returns first
            if (parent1.Equals(parent2)) return new HashSet<TProgram> {parent1};

            // gets common region between parents as a correspondence between indexes
            var commonRegion = parent1.GetCommonRegionIndexes(parent2);

            // for each crossover point in parent 1 replace by the corresponding sub-program of parent 2
            var subProgs2 = new List<ITreeProgram<TOutput>> {parent2};
            subProgs2.AddRange(parent2.GetSubPrograms());
            var offspring = new HashSet<TProgram>(
                commonRegion.Select(
                    indexes => (TProgram) parent1.Replace(indexes.Key, subProgs2[(int) indexes.Value])));

            return offspring;
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        #endregion
    }
}