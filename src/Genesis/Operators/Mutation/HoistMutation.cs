// ------------------------------------------
// <copyright file="HoistMutation.cs" company="Pedro Sequeira">
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
//    Last updated: 03/22/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;
using MathNet.Numerics.Random;

namespace Genesis.Operators.Mutation
{
    /// <summary>
    ///     Represents a <see cref="IMutationOperator{TProgram}" /> for <see cref="ITreeProgram{TOutput}" />.
    ///     This operator selects a random sub-program of a given program.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class HoistMutation<TProgram, TOutput> : IMutationOperator<TProgram> where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        public IEnumerable<TProgram> GetAllMutations(TProgram program)
        {
            if (program == null) return new List<TProgram>();

            // simply return all sub-programs
            var allMutations = new List<TProgram> {program};
            allMutations.AddRange(program.GetSubPrograms().Cast<TProgram>());
            return allMutations;
        }

        /// <summary>
        ///     Mutates the given <see cref="TProgram" /> by selecting one of its sub-programs.
        /// </summary>
        /// <param name="program">The program we want to mutate.</param>
        /// <returns>A sub-program of the given <see cref="TProgram" />.</returns>
        public TProgram Mutate(TProgram program)
        {
            if (program == null) return default(TProgram);

            // define the mutation point randomly
            var mutatePoint = (uint) this._random.Next(program.Length);

            // return the sub-program
            return (TProgram) program.ProgramAt(mutatePoint);
        }

        #endregion
    }
}