// ------------------------------------------
// <copyright file="StochasticProgramGenerator.cs" company="Pedro Sequeira">
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

using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Operators.Generation
{
    /// <summary>
    ///     Represents a generic <see cref="IProgramGenerator{TProgram,TOutput}" /> that selects from a list of generator
    ///     operators at random.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public sealed class StochasticProgramGenerator<TProgram, TOutput> : IProgramGenerator<TProgram, TOutput>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly IDictionary<IProgramGenerator<TProgram, TOutput>, double> _possibleGenerators;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="StochasticProgramGenerator{TProgram,TOutput}" /> with the given list of operators to
        ///     choose from. All operators will have the same probability of being selected (uniform distribution).
        /// </summary>
        /// <param name="possibleGenerators">The list of generator operators to be used by this operator.  </param>
        public StochasticProgramGenerator(IList<IProgramGenerator<TProgram, TOutput>> possibleGenerators)
        {
            this._possibleGenerators = possibleGenerators.ToDictionary(x => x, x => 1d / possibleGenerators.Count);
        }

        /// <summary>
        ///     Creates a new <see cref="StochasticProgramGenerator{TProgram,TOutput}" /> with the given list of operators to
        ///     choose from. Operators will be selected according to the corresponding probability.
        /// </summary>
        /// <param name="possibleGenerators">
        ///     A list of generator operators to be used by this operator and the corresponding selection probabilities.
        /// </param>
        public StochasticProgramGenerator(IDictionary<IProgramGenerator<TProgram, TOutput>, double> possibleGenerators)
        {
            this._possibleGenerators = possibleGenerators;
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            this._possibleGenerators.Clear();
        }

        public TProgram Generate(PrimitiveSet<TProgram> primitives, uint maxDepth) =>
            this._possibleGenerators.GetRandomItem(this._random).Generate(primitives, maxDepth);

        #endregion
    }
}