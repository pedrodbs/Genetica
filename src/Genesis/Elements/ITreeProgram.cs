// ------------------------------------------
// <copyright file="ITreeProgram.cs" company="Pedro Sequeira">
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

namespace Genesis.Elements
{
    /// <summary>
    ///     Represents an interface for <see cref="IProgram" /> objects that are represented by a hierarchical tree.
    /// </summary>
    public interface ITreeProgram : IProgram, ITreeNode
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the program's label, i.e., its representation independently of its descendant nodes.
        /// </summary>
        string Label { get; }

        #endregion
    }

    /// <summary>
    ///     Represents a <see cref="IProgram{TInput,TOutput}" /> that is represented as a hierarchical syntactic tree, in which
    ///     the descendants (inputs) can be any kind of <see cref="ITreeProgram{TOutput}" />.
    /// </summary>
    /// <typeparam name="TOutput">The type of output.</typeparam>
    public interface ITreeProgram<TOutput> :
        IProgram<IReadOnlyList<ITreeProgram<TOutput>>, TOutput>, ITreeProgram, IComparable<ITreeProgram<TOutput>>
    {
        #region Public Methods

        /// <summary>
        ///     Creates a new <see cref="ITreeProgram{TOutput}" /> of the same kind of this program with the given child programs.
        /// </summary>
        /// <param name="children">The children of the new program.</param>
        /// <returns>A new <see cref="ITreeProgram{TOutput}" /> of the same kind of this program with the given child programs.</returns>
        ITreeProgram<TOutput> CreateNew(IList<ITreeProgram<TOutput>> children);

        /// <summary>
        ///     Gets a program node representing the primitive of the corresponding <see cref="ITreeProgram{TOutput}" />
        ///     derived type.
        /// </summary>
        /// <returns>
        ///     The program node representing the primitive of the corresponding <see cref="ITreeProgram{TOutput}" />
        ///     derived type.
        /// </returns>
        ITreeProgram<TOutput> GetPrimitive();

        #endregion
    }
}