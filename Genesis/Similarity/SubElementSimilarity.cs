// ------------------------------------------
// <copyright file="SubElementSimilarity.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/06/05
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using Genesis.Elements;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Measures the similarity of elements based on their sub-combinations. Sub-element similarity is given by the
    ///     division of the number of common sub-elements of the elements and the total number of sub-elements.
    /// </summary>
    public class SubElementSimilarity : ISimilarityMeasure
    {
        #region Public Methods

        public double Calculate(IElement elem1, IElement elem2)
        {
            if (elem1 == null || elem2 == null) return 0;
            if (elem1.Equals(elem2)) return 1;

            var elem1SubElems = GetSubElements(elem1);
            var elem2SubElems = GetSubElements(elem2);
            var union = new HashSet<IElement>(elem1SubElems.Keys);
            union.UnionWith(elem2SubElems.Keys);
            var unionCount = 0u;
            var intersectionCount = 0u;
            foreach (var subElem in union)
            {
                if (elem1SubElems.ContainsKey(subElem)) unionCount += elem1SubElems[subElem];
                if (elem2SubElems.ContainsKey(subElem)) unionCount += elem2SubElems[subElem];
                if (elem1SubElems.ContainsKey(subElem) && elem2SubElems.ContainsKey(subElem))
                    intersectionCount += elem1SubElems[subElem] + elem2SubElems[subElem];
            }
            return (double) intersectionCount / unionCount;
        }

        #endregion

        #region Private & Protected Methods

        private static IDictionary<IElement, uint> GetSubElements(IElement element)
        {
            // organizes sub-elements in dictionary-count form
            var subElems = element.GetSubElements();
            var subElemsCounts = new Dictionary<IElement, uint>();
            foreach (var subElem in subElems)
            {
                if (!subElemsCounts.ContainsKey(subElem)) subElemsCounts.Add(subElem, 0);
                subElemsCounts[subElem]++;
            }
            return subElemsCounts;
        }

        #endregion
    }
}