// ------------------------------------------
// <copyright file="TournamentSelection.cs" company="Pedro Sequeira">
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
using MathNet.Numerics.Random;

namespace Genesis.Operators.Selection
{
    /// <summary>
    ///     Represents a <see cref="ISelectionOperator{TProgram}" /> that performs tournament selection. The
    ///     operator works by performing <em>n</em> tournaments, where <em>n</em> is the size of the given population. For each
    ///     tournament, the operator selects <em>m</em> programs from the population at random, and the program attaining
    ///     maximal score wins the tournament and gets picked to the selection group.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public class TournamentSelection<TProgram> : ISelectionOperator<TProgram> where TProgram : IProgram
    {
        #region Fields

        private readonly IComparer<TProgram> _programComparer;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="TournamentSelection{TProgram}" /> with the given elements.
        /// </summary>
        /// <param name="programComparer">The comparer used to check the program attaining maximal score.</param>
        /// <param name="tournamentSize">The size of each tournament to be performed.</param>
        public TournamentSelection(IComparer<TProgram> programComparer, uint tournamentSize = 1)
        {
            this._programComparer = programComparer;
            this.TournamentSize = tournamentSize;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the size (in number of programs selected from the population) of each tournament to be performed.
        /// </summary>
        public uint TournamentSize { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public IEnumerable<TProgram> Select(IPopulation<TProgram> population)
        {
            var popList = population.ToList();
            var tourSize = (uint) Math.Max(Math.Min(this.TournamentSize, population.Count), 1);

            // makes tournaments of selected size
            var selection = new TProgram[population.Count];
            for (var i = 0; i < population.Count; i++)
                selection[i] = this.PlayTournament(popList, tourSize);
            return selection;
        }

        #endregion

        #region Private & Protected Methods

        private TProgram PlayTournament(List<TProgram> popList, uint tourSize)
        {
            var indexes = new HashSet<int>();
            var best = popList[0];
            for (var i = 0; i < tourSize; i++)
            {
                // selects individual at random from pop (no repetition)
                int index;
                do
                {
                    index = this._random.Next(popList.Count);
                } while (indexes.Contains(index));

                indexes.Add(index);

                // checks max fitness
                var individual = popList[index];
                if (this._programComparer.Compare(individual, best) > 0)
                    best = individual;
            }

            return best;
        }

        #endregion
    }
}