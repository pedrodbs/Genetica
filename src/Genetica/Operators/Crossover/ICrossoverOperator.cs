// ------------------------------------------
// <copyright file="ICrossoverOperator.cs" company="Pedro Sequeira">
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
//    Last updated: 03/22/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Genetica.Elements;

namespace Genetica.Operators.Crossover
{
    /// <summary>
    ///     An interface for crossover operators for <see cref="IProgram{TInput,TOutput}" />, i.e., operators that take two
    ///     parent programs and produce a new program that results in some combination of the parent's sub-programs.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public interface ICrossoverOperator<TProgram> : IDisposable
        where TProgram : IProgram
    {
        #region Public Methods

        /// <summary>
        ///     Creates a new program resulting from the crossover between the given parent programs.
        /// </summary>
        /// <param name="parent1">The first parent program.</param>
        /// <param name="parent2">The second parent program.</param>
        /// <returns>A new program resulting from the crossover between the given parent programs.</returns>
        TProgram Crossover(TProgram parent1, TProgram parent2);

        /// <summary>
        ///     Gets a list containing all possible offspring programs resulting from applying this crossover operator.
        /// </summary>
        /// <param name="parent1">The first parent program.</param>
        /// <param name="parent2">The second parent program.</param>
        /// <returns>A list containing all possible offspring programs resulting from applying this crossover operator.</returns>
        IEnumerable<TProgram> GetAllOffspring(TProgram parent1, TProgram parent2);

        #endregion
    }
}