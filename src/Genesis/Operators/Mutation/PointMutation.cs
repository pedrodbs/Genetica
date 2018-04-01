// ------------------------------------------
// <copyright file="PointMutation.cs" company="Pedro Sequeira">
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

namespace Genesis.Operators.Mutation
{
    /// <summary>
    ///     Represents a <see cref="IMutationOperator{TProgram}" /> for <see cref="ITreeProgram{TOutput}" />.
    ///     Allows the mutation of programs by randomly replacing each sub-program by one program with the same
    ///     arity taken from some <see cref="PrimitiveSet{TProgram}" />.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class PointMutation<TProgram, TOutput> : IMutationOperator<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly Dictionary<int, List<TProgram>> _primitives = new Dictionary<int, List<TProgram>>();
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="PointMutation{TProgram,TOutput}" /> with the given mutation probability and primitive set.
        /// </summary>
        /// <param name="primitives">The primitive set to be used in mutation operations.</param>
        /// <param name="mutationProbability">The probability of mutating each sub-program.</param>
        public PointMutation(PrimitiveSet<TProgram> primitives, double mutationProbability = 0.5d)
        {
            // stores primitives as a function of their arity (0, 1, ...)
            var allPrimitives = new List<TProgram>(primitives.Functions);
            allPrimitives.AddRange(primitives.Terminals);
            foreach (var primitive in allPrimitives)
            {
                var arity = primitive.Children.Count;
                if (!this._primitives.ContainsKey(arity))
                    this._primitives.Add(arity, new List<TProgram>());
                this._primitives[arity].Add(primitive);
            }

            this.MutationProbability = mutationProbability;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets or sets the probability of mutating each sub-program.
        /// </summary>
        public double MutationProbability { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Dispose()
        {
            this._primitives.Clear();
        }

        /// <inheritdoc />
        public IEnumerable<TProgram> GetAllMutations(TProgram program)
        {
            // if terminal, just return list of terminals plus the program itself
            var mutations = new HashSet<TProgram>();
            if (program == null) return mutations;
            if (program.IsLeaf())
            {
                mutations.Add(program);
                if (this._primitives.ContainsKey(0))
                    mutations.AddRange(this._primitives[0]);
                return mutations;
            }

            // mutates all children
            var numChildren = program.Children.Count;
            var newChildren = new List<IEnumerable<TProgram>>(numChildren);
            for (var i = 0; i < numChildren; i++)
                newChildren.Add(this.GetAllMutations((TProgram) program.Input[i]));

            // gets all possible combinations of children
            var childrenCombinations = newChildren.GetAllCombinations().Cast<IList<ITreeProgram<TOutput>>>().ToList();

            foreach (var mutation in this._primitives[program.Children.Count])
            foreach (var childrenCombination in childrenCombinations)
                mutations.Add((TProgram) mutation.CreateNew(childrenCombination));

            return mutations;
        }

        /// <summary>
        ///     Mutates the given <typeparamref name="TProgram" /> by randomly replacing each sub-program by one program with the
        ///     same arity taken from the defined <see cref="PrimitiveSet{TProgram}" />.
        /// </summary>
        /// <param name="program">The program we want to mutate.</param>
        /// <returns>
        ///     A new <typeparamref name="TProgram" /> created by randomly replacing each sub-program by one program with the same
        ///     arity taken from the defined <see cref="PrimitiveSet{TProgram}" />.
        /// </returns>
        public TProgram Mutate(TProgram program)
        {
            if (program == null) return default(TProgram);

            // mutates all children
            var numChildren = program.Input.Count;
            var newChildren = new ITreeProgram<TOutput>[numChildren];
            for (var i = 0; i < numChildren; i++)
                newChildren[i] = this.Mutate((TProgram) program.Input[i]);

            // checks whether to mutate this program (otherwise use same program)
            var primitive = program;
            if (this._random.NextDouble() < this.MutationProbability && this._primitives.ContainsKey(numChildren))
            {
                // mutates by creating a new random program with same arity and same children
                primitive = this._primitives[numChildren].GetRandomItem(this._random);
            }

            // creates new program with new children
            return (TProgram) primitive.CreateNew(newChildren);
        }

        #endregion
    }
}