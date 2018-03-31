// ------------------------------------------
// <copyright file="IProgramGenerator.cs" company="Pedro Sequeira">
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
using Genesis.Elements;

namespace Genesis.Operators.Generation
{
    /// <summary>
    ///     An interface for generation operators for <see cref="ITreeProgram{TOutput}" />, i.e., operators that generate
    ///     new programs by randomly combining elements from a given <see cref="PrimitiveSet{TProgram}" /> .
    /// </summary>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public interface IProgramGenerator<TProgram, TOutput> : IDisposable where TProgram : ITreeProgram<TOutput>
    {
        #region Public Methods

        /// <summary>
        ///     Generates a new program having some given maximum depth by randomly combining elements from the given
        ///     <see cref="PrimitiveSet{TProgram}" />
        /// </summary>
        /// <param name="primitives">The set from which to select primitives.</param>
        /// <param name="maxDepth">The maximum depth of the program generated.</param>
        /// <returns>The new generated program.</returns>
        TProgram Generate(PrimitiveSet<TProgram> primitives, uint maxDepth);

        #endregion
    }
}