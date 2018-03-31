// ------------------------------------------
// <copyright file="SubtreeCrossover.cs" company="Pedro Sequeira">
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
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Operators.Crossover
{
    /// <summary>
    ///     Represents a <see cref="ICrossoverOperator{TProgram}" /> for <see cref="ITreeProgram{TOutput}" />.
    ///     This crossover operator replaces a random function sub-program of the first parent by a random program of the
    ///     second parent. If the first parent is a leaf, then the second parent is returned.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class SubtreeCrossover<TProgram, TOutput> : ICrossoverOperator<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        /// <summary>
        ///     Creates a new program by replacing a random function sub-program of the first parent by a random program of the
        ///     second parent. If the first parent is a leaf, then the second parent is returned.
        /// </summary>
        /// <param name="parent1">The first parent program.</param>
        /// <param name="parent2">The second parent program.</param>
        /// <returns>A new program resulting from the crossover between the given parent programs.</returns>
        public TProgram Crossover(TProgram parent1, TProgram parent2)
        {
            if (parent1 == null || parent2 == null) return default(TProgram);

            // checks equal parents, returns one of them
            if (parent1.Equals(parent2)) return parent1;

            // define the first crossover point as a random function of parent 1
            var subProgs1 = parent1.GetSubPrograms();
            var crossPoints1 = new List<uint>();
            for (var i = 1u; i < parent1.Length; i++)
                if (!subProgs1[i - 1].IsLeaf())
                    crossPoints1.Add(i);

            // if parent 1 does not have crossover points, return parent 2
            if (crossPoints1.Count == 0) return parent2;

            // define the second crossover point as a random program of parent 2
            var crossPoint1 = crossPoints1.GetRandomItem(this._random);
            var prog = (TProgram) parent2.ProgramAt((uint) this._random.Next(parent2.Length));
            return (TProgram) parent1.Replace(crossPoint1, prog);
        }

        public IEnumerable<TProgram> GetAllOffspring(TProgram parent1, TProgram parent2)
        {
            if (parent1 == null || parent2 == null) return new List<TProgram>();

            // checks equal parents, returns one of them
            if (parent1.Equals(parent2)) return new HashSet<TProgram> {parent1};

            // gets parent 1's cross-over points (the function sub-programs)
            var crossPoints1 = new List<uint>();
            var subProgs1 = parent1.GetSubPrograms();
            for (var i = 1u; i < parent1.Length; i++)
                if (!subProgs1[i - 1].IsLeaf())
                    crossPoints1.Add(i);

            // if parent 1 does not have crossover points, return parent 2
            var offspring = new HashSet<TProgram> {parent2};
            if (crossPoints1.Count == 0) return offspring;

            // creates new programs by replacing function sub-programs of parent 1 by sub-programs of parent 2
            var subProgs2 = new List<ITreeProgram<TOutput>> {parent2};
            subProgs2.AddRange(parent2.GetSubPrograms());
            foreach (var crossPoint1 in crossPoints1)
                for (var j = 0; j < parent2.Length; j++)
                    offspring.Add((TProgram) parent1.Replace(crossPoint1, subProgs2[j]));

            return offspring;
        }

        public void Dispose()
        {
        }

        #endregion
    }
}