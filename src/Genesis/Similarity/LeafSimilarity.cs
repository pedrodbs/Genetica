// ------------------------------------------
// <copyright file="LeafSimilarity.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;
using Genesis.Elements;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Represents a <see cref="ISimilarityMeasure{TProgram}" /> for <see cref="ITreeProgram{TOutput}" />.
    ///     It measures the similarity of programs based on the leaf nodes of their expressions. Leaf similarity is given by
    ///     the division between the number of common terminals and the number of all terminals of the two given programs.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class LeafSimilarity<TProgram, TOutput> : ISimilarityMeasure<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Public Methods

        public double Calculate(TProgram prog1, TProgram prog2)
        {
            if (prog1 == null || prog2 == null) return 0;
            if (prog1.Equals(prog2)) return 1;

            var prog1Terminals = prog1.GetLeaves();
            var prog2Terminals = prog2.GetLeaves();
            var union = new HashSet<ITreeProgram<TOutput>>(prog1Terminals.Keys);
            union.UnionWith(prog2Terminals.Keys);
            var unionCount = 0u;
            var intersectionCount = 0u;
            foreach (var terminal in union)
            {
                if (prog1Terminals.ContainsKey(terminal)) unionCount += prog1Terminals[terminal];
                if (prog2Terminals.ContainsKey(terminal)) unionCount += prog2Terminals[terminal];
                if (prog1Terminals.ContainsKey(terminal) && prog2Terminals.ContainsKey(terminal))
                    intersectionCount += prog1Terminals[terminal] + prog2Terminals[terminal];
            }
            return (double) intersectionCount / unionCount;
        }

        #endregion
    }
}