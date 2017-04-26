// ------------------------------------------
// <copyright file="SubElementSimilarity.cs" company="Pedro Sequeira">
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

            var elem1SubElems = elem1.GetSubElements();
            var elem2SubElems = elem2.GetSubElements();
            var union = new HashSet<IElement>(elem1SubElems);
            union.UnionWith(elem2SubElems);
            var intersection = new HashSet<IElement>(elem1SubElems);
            intersection.IntersectWith(elem2SubElems);
            return (double) intersection.Count / union.Count;
        }

        #endregion
    }
}