// ------------------------------------------
// <copyright file="SubProgramSimilarity.cs" company="Pedro Sequeira">
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
    ///     Measures the similarity of <see cref="ITreeProgram{TOutput}" /> based on their sub-combinations. Sub-program
    ///     similarity is given by the division of the number of common sub-programs of the programs and the total number of
    ///     sub-programs.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class SubProgramSimilarity<TProgram, TOutput> : ISimilarityMeasure<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Public Methods

        /// <inheritdoc />
        public double Calculate(TProgram prog1, TProgram prog2)
        {
            if (prog1 == null || prog2 == null) return 0;
            if (prog1.Equals(prog2)) return 1;

            var prog1SubProgs = GetSubPrograms(prog1);
            var prog2SubProgs = GetSubPrograms(prog2);
            var union = new HashSet<ITreeProgram<TOutput>>(prog1SubProgs.Keys);
            union.UnionWith(prog2SubProgs.Keys);
            var unionCount = 0u;
            var intersectionCount = 0u;
            foreach (var subProg in union)
            {
                if (prog1SubProgs.ContainsKey(subProg)) unionCount += prog1SubProgs[subProg];
                if (prog2SubProgs.ContainsKey(subProg)) unionCount += prog2SubProgs[subProg];
                if (prog1SubProgs.ContainsKey(subProg) && prog2SubProgs.ContainsKey(subProg))
                    intersectionCount += prog1SubProgs[subProg] + prog2SubProgs[subProg];
            }
            return (double) intersectionCount / unionCount;
        }

        #endregion

        #region Private & Protected Methods

        private static IDictionary<ITreeProgram<TOutput>, uint> GetSubPrograms(ITreeProgram<TOutput> program)
        {
            // organizes sub-programs in dictionary-count form
            var subProgs = program.GetSubPrograms();
            var subProgsCounts = new Dictionary<ITreeProgram<TOutput>, uint>();
            foreach (var subProg in subProgs)
            {
                if (!subProgsCounts.ContainsKey(subProg)) subProgsCounts.Add(subProg, 0);
                subProgsCounts[subProg]++;
            }
            return subProgsCounts;
        }

        #endregion
    }
}