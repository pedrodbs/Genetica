// ------------------------------------------
// <copyright file="IMutationOperator.cs" company="Pedro Sequeira">
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

namespace Genesis.Operators.Mutation
{
    /// <summary>
    ///     An interface for mutation operators for <see cref="IProgram{TInput,TOutput}" />, i.e., operators that take one
    ///     program and create a new one by changing some sub-program in a certain way.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public interface IMutationOperator<TProgram> : IDisposable
        where TProgram : IProgram
    {
        #region Public Methods

        /// <summary>
        ///     Gets a list containing all possible programs resulting from applying this mutation operator.
        /// </summary>
        /// <param name="program">The program we want to mutate.</param>
        /// <returns>A list containing all possible programs resulting from applying this mutation operator.</returns>
        IEnumerable<TProgram> GetAllMutations(TProgram program);

        /// <summary>
        ///     Mutates the given <typeparamref name="TProgram" /> by creating a new one based on some change of one of its
        ///     sub-programs.
        /// </summary>
        /// <param name="program">The program we want to mutate.</param>
        /// <returns>A new <typeparamref name="TProgram" /> based on some change of one of the given program's sub-programs.</returns>
        TProgram Mutate(TProgram program);

        #endregion
    }
}