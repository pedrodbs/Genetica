﻿// ------------------------------------------
// <copyright file="SubCombinationSimilarity.cs" company="Pedro Sequeira">
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
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using Genetica.Elements;

namespace Genetica.Similarity
{
    /// <summary>
    ///     Measures the similarity of <see cref="ITreeProgram{TOutput}" /> based on their sub-combinations. Sub-combination
    ///     similarity is given by the division of the number of common sub-combinations in the programs and the total number
    ///     of sub-combinations.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class SubCombinationSimilarity<TProgram, TOutput> : ISimilarityMeasure<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Public Methods

        /// <inheritdoc />
        public double Calculate(TProgram prog1, TProgram prog2)
        {
            if (prog1 == null || prog2 == null) return 0;
            if (prog1.Equals(prog2)) return 1;

            var prog1SubCombs = prog1.GetSubCombinations();
            var prog2SubCombs = prog2.GetSubCombinations();
            var union = new HashSet<ITreeProgram<TOutput>>(prog1SubCombs);
            union.UnionWith(prog2SubCombs);
            var intersection = prog1SubCombs as HashSet<ITreeProgram<TOutput>> ??
                               new HashSet<ITreeProgram<TOutput>>(prog1SubCombs);
            intersection.IntersectWith(prog2SubCombs);
            return (double) intersection.Count / union.Count;
        }

        #endregion
    }
}