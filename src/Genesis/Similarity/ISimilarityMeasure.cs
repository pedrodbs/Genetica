// ------------------------------------------
// <copyright file="ISimilarityMeasure.cs" company="Pedro Sequeira">
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

using Genesis.Elements;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Represents an interface for similarity measures between two programs, e.g., based on their tree structure.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public interface ISimilarityMeasure<in TProgram> where TProgram : IProgram
    {
        #region Public Methods

        /// <summary>
        ///     Calculates the similarity (or inverse distance) between two <typeparamref name="TProgram" />.
        /// </summary>
        /// <param name="prog1">The first program of the comparison.</param>
        /// <param name="prog2">The second program of the comparison.</param>
        /// <returns>
        ///     A number between <c>0</c> and <c>1</c> representing the calculated similarity between the given programs. A
        ///     value near 1 indicates a high similarity (or low distance) between the two programs, while a value near 0
        ///     represents a low similarity (or high distance) between the programs. If the programs are the same, it returns 1, if
        ///     any of the programs is <c>null</c>, it returns 0.
        /// </returns>
        double Calculate(TProgram prog1, TProgram prog2);

        #endregion
    }
}