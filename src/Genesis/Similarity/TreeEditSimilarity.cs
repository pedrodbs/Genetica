// ------------------------------------------
// <copyright file="TreeEditSimilarity.cs" company="Pedro Sequeira">
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
//    Last updated: 04/04/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;
using Genesis.Util;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Measures the similarity of two <see cref="ITreeProgram{TOutput}" /> based on the edit or Levenshtein distance,
    ///     i.e., based on the number of edits---transformations, additions and deletions---needed to transform the syntactic
    ///     tree of one program into the one of the other program.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class TreeEditSimilarity<TProgram, TOutput> : ISimilarityMeasure<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Public Methods

        /// <inheritdoc />
        public double Calculate(TProgram prog1, TProgram prog2) => Calculate(prog1, prog2 as ITreeProgram<TOutput>);

        #endregion

        #region Private & Protected Methods

        private static double Calculate(ITreeProgram<TOutput> prog1, ITreeProgram<TOutput> prog2)
        {
            // checks simple cases
            if (prog1 == null || prog2 == null) return 0;
            if (prog1.Equals(prog2)) return 1;

            // replace all common sub-programs by weighted variables
            var ignorePrograms = new HashSet<ITreeProgram<TOutput>>();
            var minCount = Math.Min(prog1.Length, prog2.Length);
            for (var i = 0; i < minCount; i++)
            {
                var largestCommon = GetLargestCommon(ref prog1, ref prog2, ignorePrograms);
                if (largestCommon == null || largestCommon.Length < 2) break;
                var newSubProgram = new WeightedVariable(largestCommon.Expression, largestCommon.Length);
                prog1 = prog1.Replace(largestCommon, newSubProgram);
                prog2 = prog2.Replace(largestCommon, newSubProgram);
                ignorePrograms.Add(newSubProgram);
            }

            // gets sub-programs
            var subProgs1 = new List<ITreeProgram<TOutput>> {prog1};
            subProgs1.AddRange(prog1.GetSubPrograms());
            var subProgs2 = new List<ITreeProgram<TOutput>> {prog2};
            subProgs2.AddRange(prog2.GetSubPrograms());

            // tries to align both trees in all possible ways
            var minRelCost = double.MaxValue;
            for (var i = 0u; i < prog1.Length; i++)
            for (var j = 0u; j < prog2.Length; j++)
            {
                // alignment is determined by the starting indexes of both trees
                var idx1 = i;
                var idx2 = j;

                // adds cost of having to add previous nodes (shifting/alignment cost)
                var cost = i + j;
                var totalCost = cost;

                // gets edit cost between sub-programs
                GetEditCost(subProgs1, subProgs2, ref idx1, ref idx2, ref cost, ref totalCost);

                // checks cost of having to add remaining nodes
                var remainder = prog1.Length - idx1 + prog2.Length - idx2 - 2;
                cost += remainder;
                totalCost += remainder;
                var relCost = (double) cost / totalCost;

                // updates min relative cost
                if (relCost < minRelCost) minRelCost = relCost;
            }

            return 1d - minRelCost;
        }

        private static void GetEditCost(
            IList<ITreeProgram<TOutput>> subProgs1, IList<ITreeProgram<TOutput>> subProgs2,
            ref uint idx1, ref uint idx2, ref uint curCost, ref uint totalCost)
        {
            totalCost++;
            var subProg1 = idx1 < subProgs1.Count ? subProgs1[(int) idx1] : null;
            var subProg2 = idx2 < subProgs2.Count ? subProgs2[(int) idx2] : null;

            // if programs are equal
            if (subProg1 != null && subProg2 != null && subProg1.Equals(subProg2))
            {
                // just advance both indexes and adds total cost (cur cost remains the same)
                var remainder = subProg1.Length - 1u;
                idx1 += remainder;
                idx2 += remainder;
                totalCost += (subProg1 is WeightedVariable variable ? variable.Weight : subProg1.Length) - 1;
                return;
            }

            // check if one of the sub-programs is null or has no children
            if (subProg1 == null || subProg2 == null ||
                subProg1.Input == null || subProg1.Input.Count == 0 ||
                subProg2.Input == null || subProg2.Input.Count == 0)
            {
                // adds cost of adding / removing the null node
                curCost++;

                // advance the indexes of both sub-trees and adds cost of adding / removing remainder nodes
                if (subProg1 != null)
                {
                    if (subProg1 is WeightedVariable variable)
                    {
                        // it was a swap: ignores cost of adding/removing nodes the original program's nodes
                        totalCost += variable.Weight - 1;
                    }
                    else if (subProg1.Length > 1)
                    {
                        // normal, uncommon program, count cost of adding/removing its nodes
                        var remainder1 = subProg1.Length - 1u;
                        idx1 += remainder1;
                        curCost += remainder1;
                        totalCost += remainder1;
                    }
                }

                if (subProg2 != null)
                {
                    if (subProg2 is WeightedVariable variable)
                    {
                        // it was a swap: ignores cost of adding/removing nodes the original program's nodes
                        totalCost += variable.Weight - 1;
                    }
                    else if (subProg2.Length > 1)
                    {
                        // normal, uncommon program, count cost of adding/removing its nodes
                        var remainder2 = subProg2.Length - 1u;
                        idx2 += remainder2;
                        curCost += remainder2;
                        totalCost += remainder2;
                    }
                }

                return;
            }

            // both programs have children, compares type of function
            var sameNode = subProg1.IsLeaf()
                ? subProg2.IsLeaf() && subProg1.Equals(subProg2)
                : subProg1.GetType() == subProg2.GetType();

            // adds transformation cost
            if (!sameNode) curCost++;

            var children1 = new List<ITreeProgram<TOutput>>(subProg1.Input);
            var children2 = new List<ITreeProgram<TOutput>>(subProg2.Input);

            // try to align children if one (or both) function are commutative
            if (subProg1 is ICommutativeTreeProgram<TOutput>)
                CollectionUtil.Align(children2, children1);
            else if (subProg2 is ICommutativeTreeProgram<TOutput>)
                CollectionUtil.Align(children1, children2);

            // iterate recursively through both sub-programs' children in parallel
            var maxNumChildren = Math.Max(children1.Count, children2.Count);
            for (var i = 0; i < maxNumChildren; i++)
            {
                // advances indexes if corresponding child exists
                var prevIdx1 = children1.Count > i ? idx1 + 1 : uint.MaxValue;
                var prevIdx2 = children2.Count > i ? idx2 + 1 : uint.MaxValue;

                GetEditCost(subProgs1, subProgs2, ref prevIdx1, ref prevIdx2, ref curCost, ref totalCost);

                // updates indexes based on search
                idx1 = prevIdx1 == uint.MaxValue ? idx1 : prevIdx1;
                idx2 = prevIdx2 == uint.MaxValue ? idx2 : prevIdx2;
            }
        }

        private static ITreeProgram<TOutput> GetLargestCommon(
            ref ITreeProgram<TOutput> prog1, ref ITreeProgram<TOutput> prog2,
            ISet<ITreeProgram<TOutput>> ignorePrograms = null)
        {
            // gets sub-programs and sort descendingly
            var subProgs1 = new SortedSet<ITreeProgram<TOutput>>(prog1.GetSubPrograms(),
                Comparer<ITreeProgram<TOutput>>.Create((a, b) => -(a.Length == b.Length
                                                           ? a.CompareTo(b)
                                                           : a.Length.CompareTo(b.Length))));
            var subProgs2 = new HashSet<ITreeProgram<TOutput>>(prog2.GetSubPrograms());
            return subProgs1.FirstOrDefault(
                subProg1 =>
                    (ignorePrograms == null || !ignorePrograms.Contains(subProg1)) && subProgs2.Contains(subProg1));
        }

        #endregion

        #region Nested type: WeightedVariable

        private class WeightedVariable : ITreeProgram<TOutput>
        {
            #region Constructors

            public WeightedVariable(string label, uint weight)
            {
                this.Weight = weight;
                this.Label = label;
            }

            #endregion

            #region Properties & Indexers

            public uint Weight { get; }

            public string Expression => this.Label;

            public ushort Length => 1;

            public IReadOnlyList<ITreeProgram<TOutput>> Input { get; } = new ITreeProgram<TOutput>[0];

            IReadOnlyList<ITreeNode> ITreeNode.Children => this.Input;

            public string Label { get; }

            #endregion

            #region Public Methods

            public override bool Equals(object obj) =>
                !(obj is null) && (ReferenceEquals(this, obj) ||
                                   obj is ITreeProgram<TOutput> term && this.Label.Equals(term.Label));

            /// <inheritdoc />
            public override int GetHashCode() => this.Expression.GetHashCode();

            public override string ToString() => this.Label;

            #endregion

            #region Public Methods

            public int CompareTo(ITreeProgram<TOutput> other) =>
                string.CompareOrdinal(this.Expression, other.Expression);

            public TOutput Compute() => default(TOutput);

            public ITreeProgram<TOutput> CreateNew(IList<ITreeProgram<TOutput>> children) => this;

            public ITreeProgram<TOutput> GetPrimitive() => this;

            public ITreeProgram<TOutput> Simplify() => this;

            #endregion
        }

        #endregion
    }
}