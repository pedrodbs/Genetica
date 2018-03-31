// ------------------------------------------
// <copyright file="EvolutionaryDistanceCalculator.cs" company="Pedro Sequeira">
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
using Genesis.Operators.Crossover;
using Genesis.Operators.Mutation;
using Genesis.Similarity;

namespace Genesis.Elements
{
    /// <summary>
    ///     Represents a class for calculating the evolutionary distance between two given <see cref="ITreeProgram{TOutput}" />
    ///     s, i.e., the number of evolutionary operations needed to transform one program into the other. This corresponds to
    ///     a rather exhaustive search over some given <see cref="IMutationOperator{TProgram}" /> and
    ///     <see cref="ICrossoverOperator{TProgram}" />. The search is guided by some given
    ///     <see cref="ISimilarityMeasure{TProgram}" /> that serves as an heuristic for the transformation process.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class EvolutionaryDistanceCalculator<TProgram, TOutput> where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly IEnumerable<ICrossoverOperator<TProgram>> _crossovers;
        private readonly IEnumerable<IMutationOperator<TProgram>> _mutations;
        private readonly PrimitiveSet<TProgram> _primitiveSet;
        private readonly ISimilarityMeasure<TProgram> _similarityMeasure;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="EvolutionaryDistanceCalculator{TProgram,TOutput}" /> with the given arguments.
        /// </summary>
        /// <param name="similarityMeasure">The similarity measure to guide the transformation search process.</param>
        /// <param name="primitiveSet">The primitives used for the crossover operator.</param>
        /// <param name="crossovers">The crossover operators to generate programs during the search.</param>
        /// <param name="mutations">The mutation operators to generate programs during the search.</param>
        public EvolutionaryDistanceCalculator(
            ISimilarityMeasure<TProgram> similarityMeasure,
            PrimitiveSet<TProgram> primitiveSet,
            IEnumerable<ICrossoverOperator<TProgram>> crossovers,
            IEnumerable<IMutationOperator<TProgram>> mutations)
        {
            this._similarityMeasure = similarityMeasure;
            this._primitiveSet = primitiveSet;
            this._crossovers = crossovers;
            this._mutations = mutations;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Calculates the evolutionary distance between the two given programs. The procedure stops whenever the similarity
        ///     between candidate programs cannot be improved by using any of the crossover and mutation operators available.
        /// </summary>
        /// <param name="prog1">The source program to be transformed into the second program.</param>
        /// <param name="prog2">The target program that we want to achieve.</param>
        /// <returns>A list containing pairs of candidate program - type of operator used.</returns>
        public IList<KeyValuePair<TProgram, Type>> GetDistance(TProgram prog1, TProgram prog2)
        {
            var progs = new List<KeyValuePair<TProgram, Type>>();
            var prog = prog1;
            KeyValuePair<TProgram, Type> nextElem;
            while (!(nextElem = this.GetNextBest(prog, prog2)).Key.Equals(prog))
            {
                prog = nextElem.Key;
                progs.Add(nextElem);
            }
            return progs;
        }

        #endregion

        #region Private & Protected Methods

        private KeyValuePair<TProgram, Type> GetNextBest(TProgram prog1, TProgram prog2)
        {
            var maxMeasure = this._similarityMeasure.Calculate(prog1, prog2);
            var maxElem = new KeyValuePair<TProgram, Type>(prog1, null);
            foreach (var function in this._primitiveSet.Functions)
            foreach (var crossover in this._crossovers)
            {
                var allOffspring = crossover.GetAllOffspring(function, prog1);
                foreach (var program in allOffspring)
                {
                    var progMeasure = this._similarityMeasure.Calculate(program, prog2);
                    if (progMeasure <= maxMeasure) continue;
                    maxMeasure = progMeasure;
                    maxElem = new KeyValuePair<TProgram, Type>(program, crossover.GetType());
                }

                var children = new List<ITreeProgram<TOutput>>(function.Input) {[0] = prog1};
                var prog = (TProgram) function.CreateNew(children);
                var funcMeasure = this._similarityMeasure.Calculate(prog, prog2);
                if (funcMeasure <= maxMeasure) continue;
                maxMeasure = funcMeasure;
                maxElem = new KeyValuePair<TProgram, Type>(prog, crossover.GetType());
            }

            foreach (var mutation in this._mutations)
            {
                var allMutations = mutation.GetAllMutations(prog1);
                foreach (var prog in allMutations)
                {
                    var progMeasure = this._similarityMeasure.Calculate(prog, prog2);
                    if (progMeasure <= maxMeasure) continue;
                    maxMeasure = progMeasure;
                    maxElem = new KeyValuePair<TProgram, Type>(prog, mutation.GetType());
                }
            }
            return maxElem;
        }

        #endregion
    }
}