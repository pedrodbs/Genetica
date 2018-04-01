// ------------------------------------------
// <copyright file="SubtreeMutation.cs" company="Pedro Sequeira">
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
using Genesis.Elements;
using Genesis.Operators.Generation;
using MathNet.Numerics.Random;

namespace Genesis.Operators.Mutation
{
    /// <summary>
    ///     Represents a <see cref="IMutationOperator{TProgram}" /> for <see cref="ITreeProgram{TOutput}" />.
    ///     Represents a mutation operator that replaces one sub-program of a given program by a new random program generated
    ///     using some <see cref="IProgramGenerator{TProgram,TOutput}" />.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class SubtreeMutation<TProgram, TOutput> : IMutationOperator<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly PrimitiveSet<TProgram> _primitives;
        private readonly IProgramGenerator<TProgram, TOutput> _programGenerator;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates anew <see cref="SubtreeMutation{TProgram,TOutput}" /> with the given arguments.
        /// </summary>
        /// <param name="programGenerator">The generator for new sub-programs. </param>
        /// <param name="primitives">The primitive set to be used in mutation operations.</param>
        /// <param name="maxDepth">The maximum depth of new random sub-programs.</param>
        public SubtreeMutation(
            IProgramGenerator<TProgram, TOutput> programGenerator, PrimitiveSet<TProgram> primitives,
            uint maxDepth)
        {
            this._primitives = primitives;
            this._programGenerator = programGenerator;
            this.MaxDepth = maxDepth;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets or sets the maximum depth of new random sub-programs.
        /// </summary>
        public uint MaxDepth { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <summary>
        ///     Gets a list containing all possible programs resulting from applying this mutation operator.
        /// </summary>
        /// <param name="program">The program we want to mutate.</param>
        /// <returns>A list containing all possible programs resulting from applying this mutation operator.</returns>
        /// <remarks>
        ///     Because there may be a huge number of possible mutations resulting from the use of this operator, only the
        ///     primitives (functions + terminals) are considered, i.e., new programs of depth 0 or 1.
        /// </remarks>
        public IEnumerable<TProgram> GetAllMutations(TProgram program)
        {
            //todo this is not correct: needs to either  
            // - know all possible generated programs
            // - use functions but then use all combinations of terminals
            var mutations = new HashSet<TProgram>();
            if (program == null) return mutations;

            var allPrimitives = new List<TProgram>(this._primitives.Functions);
            allPrimitives.AddRange(this._primitives.Terminals);

            // replaces each sub-program by a primitive program
            for (var i = 0u; i < program.Length; i++)
                foreach (var primitive in allPrimitives)
                    mutations.Add((TProgram) program.Replace(i, primitive));

            return mutations;
        }

        /// <summary>
        ///     Mutates the given <typeparamref name="TProgram" /> by replacing one of its sub-programs by a new random program
        ///     generated using the defined <see cref="IProgramGenerator{TProgram,TOutput}" />.
        /// </summary>
        /// <param name="program">The program we want to mutate.</param>
        /// <returns>
        ///     A new <typeparamref name="TProgram" /> by replacing one of its sub-programs by a new random program generated using
        ///     the
        ///     defined <see cref="IProgramGenerator{TProgram,TOutput}" />.
        /// </returns>
        public TProgram Mutate(TProgram program)
        {
            if (program == null) return default(TProgram);

            // define the mutation point randomly
            var mutatePoint = (uint) this._random.Next(program.Length);

            // define the new random program and creates replacement
            var newElem = this._programGenerator.Generate(this._primitives, this.MaxDepth);
            return (TProgram) program.Replace(mutatePoint, newElem);
        }

        #endregion
    }
}