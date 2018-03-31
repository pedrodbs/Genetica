// ------------------------------------------
// <copyright file="ShrinkMutation.cs" company="Pedro Sequeira">
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

namespace Genesis.Operators.Mutation
{
    /// <summary>
    ///     Represents a <see cref="IMutationOperator{TProgram}" /> for <see cref="ITreeProgram{TOutput}" />.
    ///     This mutation operator removes a random descendant node of a given program and replaces it with a random terminal
    ///     program from a given <see cref="PrimitiveSet{TProgram}" />.
    /// </summary>
    public class ShrinkMutation<TProgram, TOutput> : IMutationOperator<TProgram> where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());
        private readonly IList<TProgram> _terminals;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="ShrinkMutation{TProgram,TOutput}" /> with the given primitives.
        /// </summary>
        /// <param name="primitives">The primitive set to be used in mutation operations.</param>
        public ShrinkMutation(PrimitiveSet<TProgram> primitives)
        {
            this._terminals = primitives.Terminals.ToList();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        public IEnumerable<TProgram> GetAllMutations(TProgram program)
        {
            var mutations = new HashSet<TProgram>();
            if (program == null) return mutations;

            // replaces each sub-program by a terminal program
            for (var i = 0u; i < program.Length; i++)
                foreach (var terminal in this._terminals)
                    mutations.Add((TProgram) program.Replace(i, terminal));

            return mutations;
        }

        /// <summary>
        ///     Mutates the given <see cref="TProgram" /> by removing one of its sub-programs at random and replacing it with
        ///     a random terminal program from a given <see cref="PrimitiveSet{TProgram}" />.
        /// </summary>
        /// <param name="program">The program we want to mutate.</param>
        /// <returns>A new <see cref="TProgram" /> based on some change of one of the given program's sub-programs.</returns>
        public TProgram Mutate(TProgram program)
        {
            if (program == null) return default(TProgram);

            // define the mutation point randomly
            var mutatePoint = (uint) this._random.Next(program.Length);

            // replaces with a new random terminal program
            return (TProgram) program.Replace(mutatePoint, this._terminals.GetRandomItem(this._random));
        }

        #endregion
    }
}