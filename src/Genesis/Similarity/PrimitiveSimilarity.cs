// ------------------------------------------
// <copyright file="PrimitiveSimilarity.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;
using Genesis.Elements;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Measures the similarity of <see cref="ITreeProgram{TOutput}" /> based on the primitives in their expressions.
    ///     Primitive similarity is given by the division between the number of common primitives and the number of all
    ///     primitives of the two given programs.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class PrimitiveSimilarity<TProgram, TOutput> : ISimilarityMeasure<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Public Methods

        /// <inheritdoc />
        public double Calculate(TProgram prog1, TProgram prog2)
        {
            if (prog1 == null || prog2 == null) return 0;
            if (prog1.Equals(prog2)) return 1;

            var prog1Primitives = prog1.GetPrimitives();
            var prog2Primitives = prog2.GetPrimitives();
            var union = new HashSet<ITreeProgram<TOutput>>(prog1Primitives.Keys);
            union.UnionWith(prog2Primitives.Keys);
            var unionCount = 0u;
            var intersectionCount = 0u;
            foreach (var prog in union)
            {
                if (prog1Primitives.ContainsKey(prog)) unionCount += prog1Primitives[prog];
                if (prog2Primitives.ContainsKey(prog)) unionCount += prog2Primitives[prog];
                if (prog1Primitives.ContainsKey(prog) && prog2Primitives.ContainsKey(prog))
                    intersectionCount += prog1Primitives[prog] + prog2Primitives[prog];
            }

            return (double) intersectionCount / unionCount;
        }

        #endregion
    }
}