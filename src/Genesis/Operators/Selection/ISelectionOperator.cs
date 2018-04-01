// ------------------------------------------
// <copyright file="ISelectionOperator.cs" company="Pedro Sequeira">
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

namespace Genesis.Operators.Selection
{
    /// <summary>
    ///     An interface for selection operators for <see cref="IProgram{TInput,TOutput}" />, i.e., operators that select a
    ///     certain number of programs from a given <see cref="IPopulation{TProgram}" />.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public interface ISelectionOperator<TProgram> : IDisposable
        where TProgram : IProgram
    {
        #region Public Methods

        /// <summary>
        ///     Performs the selection operation over the given <see cref="IPopulation{TProgram}" />.
        /// </summary>
        /// <param name="population">The population over which to perform selection.</param>
        /// <returns>A list of <typeparamref name="TProgram" /> resulting from the selection operation.</returns>
        IEnumerable<TProgram> Select(IPopulation<TProgram> population);

        #endregion
    }
}