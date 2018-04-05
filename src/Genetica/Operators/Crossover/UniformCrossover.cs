// ------------------------------------------
// <copyright file="UniformCrossover.cs" company="Pedro Sequeira">
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
    ///     Creates offspring by visiting the points in the common region between the parents and flipping a coin at each
    ///     point to decide whether the corresponding offspring sub-program should be picked from the first or the second
    ///     parent.
    /// </summary>
    /// <remarks>
    ///     The common region between the parent programs corresponds to the subtrees where the parents have the same shape.
    /// </remarks>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class UniformCrossover<TProgram, TOutput> : ICrossoverOperator<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        /// <summary>
        ///     Iterates the common (structural) region between the two parents, picking a sub-program from one of the parents at
        ///     random.
        /// </summary>
        /// <param name="parent1">The first parent program.</param>
        /// <param name="parent2">The second parent program.</param>
        /// <returns>A new program resulting from the uniform crossover between the given parent programs.</returns>
        public TProgram Crossover(TProgram parent1, TProgram parent2)
        {
            if (parent1 == null || parent2 == null) return default(TProgram);

            // checks equal parents, returns first, otherwise perform crossover
            return parent1.Equals(parent2) ? parent1 : (TProgram) this.GetCrossover(parent1, parent2);
        }

        /// <inheritdoc />
        public IEnumerable<TProgram> GetAllOffspring(TProgram parent1, TProgram parent2)
        {
            if (parent1 == null || parent2 == null) return new List<TProgram>();

            // checks equal parents, returns parent 1, otherwise gets offspring
            return parent1.Equals(parent2)
                ? new HashSet<TProgram> {parent1}
                : GetSubOffspring(parent1, parent2).Cast<TProgram>();
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        #endregion

        #region Private & Protected Methods

        private static ISet<ITreeProgram<TOutput>> GetSubOffspring(
            ITreeProgram<TOutput> subProg1, ITreeProgram<TOutput> subProg2)
        {
            var offspring = new HashSet<ITreeProgram<TOutput>> {subProg1, subProg2};

            // check if children differ (different sub-structure) or no children -> nothing to do in this branch
            if (subProg1.Input == null || subProg2.Input == null ||
                subProg1.Input.Count != subProg2.Input.Count || subProg1.Input.Count == 0)
                return offspring;

            // programs have same number of children, iterate recursively to get possible children in each sub-branch
            var numChildren = subProg1.Input.Count;
            var subChildren = new List<IEnumerable<ITreeProgram<TOutput>>>(numChildren);
            for (var i = 0; i < numChildren; i++)
                subChildren.Add(GetSubOffspring(subProg1.Input[i], subProg2.Input[i]).ToList());

            // gets all combinations of possible sub-branches
            var childrenCombinations = subChildren.GetAllCombinations().ToList();
            var subOffspring = new HashSet<ITreeProgram<TOutput>>();

            // if sub-programs are the same function, we only need one of them
            if (subProg1.GetType() == subProg2.GetType())
                offspring.Remove(subProg2);

            // for each sub-program, creates new sub-programs for each combination
            foreach (var subProg in offspring)
            foreach (var childCombination in childrenCombinations)
                subOffspring.Add(subProg.CreateNew(childCombination));

            return subOffspring;
        }

        private ITreeProgram<TOutput> GetCrossover(ITreeProgram<TOutput> subProg1, ITreeProgram<TOutput> subProg2)
        {
            // create new program with corresponding sub-program taken from parent 1 or 2 (50% chance)
            var program = this._random.Next(2) == 0 ? subProg1 : subProg2;

            // check if children differ (different sub-structure) -> nothing to do in this branch, return
            if (subProg1.Input == null || subProg2.Input == null ||
                subProg1.Input.Count != subProg2.Input.Count || subProg1.Input.Count == 0)
                return program;

            // programs have same number of children, iterate recursively
            var numChildren = subProg1.Input.Count;
            var children = new List<ITreeProgram<TOutput>>(numChildren);
            for (var i = 0; i < numChildren; i++)
                children.Add(this.GetCrossover(subProg1.Input[i], subProg2.Input[i]));

            // creates and returns new program
            return program.CreateNew(children);
        }

        #endregion
    }
}