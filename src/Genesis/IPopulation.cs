// ------------------------------------------
// <copyright file="IPopulation.cs" company="Pedro Sequeira">
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
using Genesis.Elements;

namespace Genesis
{
    /// <summary>
    ///     Represents a set of <see cref="IProgram" /> to be evolved through GP according to the defined evolutionary
    ///     operators.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public interface IPopulation<TProgram> : ISet<TProgram>, IDisposable where TProgram : IProgram
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the <see cref="TProgram" /> in the population attaining the maximal fitness across all programs.
        /// </summary>
        TProgram BestProgram { get; }

        /// <summary>
        ///     Gets or sets the percentage of a population used for the crossover operation during GP.
        /// </summary>
        double CrossoverPercent { get; set; }

        /// <summary>
        ///     Gets or sets the percentage of a population selected to be copied to the next generation during GP.
        /// </summary>
        double ElitismPercent { get; set; }

        /// <summary>
        ///     Gets or sets the percentage of a population used for the mutation operation during GP.
        /// </summary>
        double MutationPercent { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Initializes this population with the given seed programs.
        /// </summary>
        /// <param name="seeds">The seed programs used to initialize this population.</param>
        void Init(ISet<TProgram> seeds);

        /// <summary>
        ///     Performs one step of GP across the whole population, i.e., it generates a new generation by applying the
        ///     evolutionary operators.
        /// </summary>
        void Step();

        #endregion
    }
}