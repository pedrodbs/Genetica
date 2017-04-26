// ------------------------------------------
// <copyright file="TreeEditSimilarity.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/04/06
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;
using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;
using Genesis.Util;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Measures the similarity of elements based on the edit or Levenshtein distance, i.e., based on the number of
    ///     edits---transformations, additions and deletions---needed to transform the syntactic tree of one element into the
    ///     one of the other element.
    /// </summary>
    public class TreeEditSimilarity : ISimilarityMeasure
    {
        #region Public Methods

        public double Calculate(IElement elem1, IElement elem2)
        {
            // checks simple cases
            if (elem1 == null || elem2 == null) return 0;
            if (elem1.Equals(elem2)) return 1;

            // replace all common sub-elements by weighted variables
            var ignoreElements = new HashSet<IElement>();
            var minCount = Math.Min(elem1.Count, elem2.Count);
            for (var i = 0; i < minCount; i++)
            {
                var largestCommon = GetLargestCommon(ref elem1, ref elem2, ignoreElements);
                if (largestCommon == null || largestCommon.Count < 2) break;
                var newSubElement = new WeightedVariable(((char) ('a' + i)).ToString(), largestCommon.Count);
                elem1 = elem1.Replace(largestCommon, newSubElement);
                elem2 = elem2.Replace(largestCommon, newSubElement);
                ignoreElements.Add(newSubElement);
            }

            // gets sub-elements
            var subElems1 = new List<IElement> {elem1};
            subElems1.AddRange(elem1.GetSubElements());
            var subElems2 = new List<IElement> {elem2};
            subElems2.AddRange(elem2.GetSubElements());

            // tries to align both trees in all possible ways
            var minRelCost = double.MaxValue;
            for (var i = 0u; i < elem1.Count; i++)
            for (var j = 0u; j < elem2.Count; j++)
            {
                // alignment is determined by the starting indexes of both trees
                var idx1 = i;
                var idx2 = j;

                // adds cost of having to add previous nodes (shifting/alignment cost)
                var cost = i + j;
                var totalCost = cost;

                // gets edit cost between sub-elements
                GetEditCost(subElems1, subElems2, ref idx1, ref idx2, ref cost, ref totalCost);

                // checks cost of having to add remainding nodes
                var remainder = elem1.Count - idx1 + elem2.Count - idx2 - 2;
                cost += remainder;
                totalCost += remainder;
                var relCost = (double) cost / totalCost;

                // updates min relative cost
                if (relCost < minRelCost) minRelCost = relCost;
            }

            return 1d - minRelCost;
        }

        #endregion

        #region Private & Protected Methods

        private static void GetEditCost(
            IList<IElement> subElems1, IList<IElement> subElems2,
            ref uint idx1, ref uint idx2, ref uint curCost, ref uint totalCost)
        {
            totalCost++;
            var subElem1 = idx1 < subElems1.Count ? subElems1[(int) idx1] : null;
            var subElem2 = idx2 < subElems2.Count ? subElems2[(int) idx2] : null;

            // if elements are equal
            if (subElem1 != null && subElem2 != null && subElem1.Equals(subElem2))
            {
                // just advance both indexes and adds total cost (cur cost remains the same)
                var remainder = subElem1.Count - 1u;
                idx1 += remainder;
                idx2 += remainder;
                totalCost += (subElem1 is WeightedVariable ? ((WeightedVariable) subElem1).Weight : subElem1.Count) - 1;
                return;
            }

            // check if one of the sub-elements is null or has no children
            if (subElem1 == null || subElem2 == null ||
                subElem1.Children == null || subElem1.Children.Count == 0 ||
                subElem2.Children == null || subElem2.Children.Count == 0)
            {
                // adds cost of adding / removing the null node
                curCost++;

                // advance the indexes of both sub-trees and adds cost of adding / removing remainder nodes
                if (subElem1 != null)
                {
                    if (subElem1 is WeightedVariable)
                    {
                        // it was a swap: ignores cost of adding/removing nodes the original element's nodes
                        totalCost += ((WeightedVariable) subElem1).Weight - 1;
                    }
                    else if (subElem1.Count > 1)
                    {
                        // normal, uncommon element, count cost of adding/removing its nodes
                        var remainder1 = subElem1.Count - 1u;
                        idx1 += remainder1;
                        curCost += remainder1;
                        totalCost += remainder1;
                    }
                }
                if (subElem2 != null)
                {
                    if (subElem2 is WeightedVariable)
                    {
                        // it was a swap: ignores cost of adding/removing nodes the original element's nodes
                        totalCost += ((WeightedVariable) subElem2).Weight - 1;
                    }
                    else if (subElem2.Count > 1)
                    {
                        // normal, uncommon element, count cost of adding/removing its nodes
                        var remainder2 = subElem2.Count - 1u;
                        idx2 += remainder2;
                        curCost += remainder2;
                        totalCost += remainder2;
                    }
                }
                return;
            }

            // both elements have children, compares type of function
            var sameNode = subElem1 is Terminal
                ? subElem2 is Terminal && subElem1.Equals(subElem2)
                : subElem1.GetType() == subElem2.GetType();

            // adds transformation cost
            if (!sameNode) curCost++;

            var children1 = new List<IElement>(subElem1.Children);
            var children2 = new List<IElement>(subElem2.Children);

            // try to align children if one (or both) function are commutative
            if (subElem1 is CommutativeBinaryFunction)
                CollectionUtil.Align(children2, children1);
            else if (subElem2 is CommutativeBinaryFunction)
                CollectionUtil.Align(children1, children2);

            // iterate recursively through both sub-elements' children in parallel
            var maxNumChildren = Math.Max(children1.Count, children2.Count);
            for (var i = 0; i < maxNumChildren; i++)
            {
                // advances indexes if corresponding child exists
                var prevIdx1 = children1.Count > i ? idx1 + 1 : uint.MaxValue;
                var prevIdx2 = children2.Count > i ? idx2 + 1 : uint.MaxValue;

                GetEditCost(subElems1, subElems2, ref prevIdx1, ref prevIdx2, ref curCost, ref totalCost);

                // updates indexes based on search
                idx1 = prevIdx1 == uint.MaxValue ? idx1 : prevIdx1;
                idx2 = prevIdx2 == uint.MaxValue ? idx2 : prevIdx2;
            }
        }

        private static IElement GetLargestCommon(
            ref IElement elem1, ref IElement elem2, ISet<IElement> ignoreElements = null)
        {
            // gets sub-elements and sort descendingly
            var subElems1 = new SortedSet<IElement>(elem1.GetSubElements(),
                Comparer<IElement>.Create((a, b) => -(a.Count == b.Count ? a.CompareTo(b) : a.Count.CompareTo(b.Count))));
            var subElems2 = new HashSet<IElement>(elem2.GetSubElements());
            return subElems1.FirstOrDefault(
                subElem1 =>
                    (ignoreElements == null || !ignoreElements.Contains(subElem1)) && subElems2.Contains(subElem1));
        }

        #endregion

        #region Nested type: WeightedVariable

        private class WeightedVariable : Variable
        {
            #region Properties & Indexers

            public uint Weight { get; }

            #endregion

            #region Constructors

            public WeightedVariable(string name, uint weight) : base(name, null)
            {
                this.Weight = weight;
            }

            #endregion
        }

        #endregion
    }
}