// ------------------------------------------
// <copyright file="StochasticMutation.cs" company="Pedro Sequeira">
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

namespace Genetica.Operators.Mutation
{
    /// <summary>
    ///     Represents a generic <see cref="IMutationOperator{TProgram}" /> that selects from a list of
    ///     mutation operators at random.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public class StochasticMutation<TProgram> : IMutationOperator<TProgram> where TProgram : IProgram
    {
        #region Fields

        private readonly IDictionary<IMutationOperator<TProgram>, double> _possibleMutations;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="StochasticMutation{TProgram}" /> with the given list of operators to
        ///     choose from. All operators will have the same probability of being selected (uniform distribution).
        /// </summary>
        /// <param name="possibleMutations">The list of mutation operators to be used by this operator.  </param>
        public StochasticMutation(ICollection<IMutationOperator<TProgram>> possibleMutations)
        {
            this._possibleMutations = possibleMutations.ToDictionary(x => x, x => 1d / possibleMutations.Count);
        }

        /// <summary>
        ///     Creates a new <see cref="StochasticMutation{TProgram}" /> with the given list of operators to
        ///     choose from. Operators will be selected according to the corresponding probability.
        /// </summary>
        /// <param name="possibleMutations">
        ///     A list of mutation operators to be used by this operator and the corresponding selection probabilities.
        /// </param>
        public StochasticMutation(IDictionary<IMutationOperator<TProgram>, double> possibleMutations)
        {
            this._possibleMutations = possibleMutations;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Dispose()
        {
            this._possibleMutations.Clear();
        }

        /// <inheritdoc />
        public IEnumerable<TProgram> GetAllMutations(TProgram program)
        {
            var mutations = new HashSet<TProgram>();
            foreach (var mutationOperator in this._possibleMutations.Keys)
                mutations.AddRange(mutationOperator.GetAllMutations(program));
            return mutations;
        }

        /// <summary>
        ///     Randomly chooses one of the <see cref="IMutationOperator{TProgram}" />s to perform the mutation.
        /// </summary>
        /// <returns>A new program corresponding to the given program mutated.</returns>
        /// <param name="program">An program to be mutated.</param>
        public TProgram Mutate(TProgram program)
        {
            return this._possibleMutations.GetRandomItem(this._random).Mutate(program);
        }

        #endregion
    }
}