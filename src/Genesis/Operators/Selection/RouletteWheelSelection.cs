// ------------------------------------------
// <copyright file="RouletteWheelSelection.cs" company="Pedro Sequeira">
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
using Genesis.Evaluation;

namespace Genesis.Operators.Selection
{
    /// <summary>
    ///     Represents a <see cref="ISelectionOperator{TProgram}" /> that performs fitness proportionate
    ///     (roulette wheel) selection. The operator works by selecting <em>n</em> programs, where <em>n</em> is the size of
    ///     the given population. The operator assigns a selection probability to each program (a proportion of the wheel)
    ///     according to their fitness score, as dictated by some <see cref="IFitnessFunction{TProgram}" />. The
    ///     proportion is decided according to some <see cref="PopulationSelector" /> scheme. While programs with a higher
    ///     fitness will be more likely selected, there is still a chance that some weaker programs may survive the selection
    ///     process.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public class RouletteWheelSelection<TProgram> : ISelectionOperator<TProgram> where TProgram : IProgram
    {
        #region Fields

        private readonly IFitnessFunction<TProgram> _fitnessFunction;
        private readonly PopulationSelector _selector;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="RouletteWheelSelection{TProgram}" /> with the given arguments.
        /// </summary>
        /// <param name="selector">The scheme used to attribute proportion of selection.</param>
        /// <param name="fitnessFunction">The function used to evaluate the programs' fitness.</param>
        public RouletteWheelSelection(PopulationSelector selector, IFitnessFunction<TProgram> fitnessFunction)
        {
            this._fitnessFunction = fitnessFunction;
            this._selector = selector;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public IEnumerable<TProgram> Select(IPopulation<TProgram> population)
        {
            // stores programs in a list, gets fitness sum
            var popList = new TProgram[population.Count];
            var totalFitness = 0d;
            var i = 0;
            foreach (var individual in population)
            {
                totalFitness += this._fitnessFunction.Evaluate(individual);
                popList[i++] = individual;
            }

            // gets selection pointers (from 0 to 1) and sorts them
            var pointers = this._selector((uint) population.Count);
            Array.Sort(pointers);

            // selects programs according to pointers
            var pointerIndex = 0;
            var indivIndex = 0;
            var curFitSum = 0d;
            var selectionSize = Math.Min(pointers.Length, population.Count);
            var selection = new TProgram[selectionSize];
            while (pointerIndex < selectionSize)
            {
                if (pointers[pointerIndex] * totalFitness < curFitSum)
                {
                    selection[pointerIndex] = popList[indivIndex - 1];
                    pointerIndex++;
                }
                else
                {
                    curFitSum += this._fitnessFunction.Evaluate(popList[indivIndex++]);
                }
            }

            return selection;
        }

        #endregion
    }
}