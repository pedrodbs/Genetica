// ------------------------------------------
// <copyright file="SubCombinationSimilarity.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/03/06
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
    ///     Measures the similarity of elements based on their sub-combinations. Sub-combination similarity is given by the
    ///     division of the number of common sub-combinations in the elements and the total number of sub-combinations.
    /// </summary>
    public class SubCombinationSimilarity : ISimilarityMeasure
    {
        #region Public Methods

        public double Calculate(IElement elem1, IElement elem2)
        {
            if (elem1 == null || elem2 == null) return 0;
            if (elem1.Equals(elem2)) return 1;

            var elem1SubCombs = elem1.GetSubCombinations();
            var elem2SubCombs = elem2.GetSubCombinations();
            var union = new HashSet<IElement>(elem1SubCombs);
            union.UnionWith(elem2SubCombs);
            var intersection = elem1SubCombs as HashSet<IElement> ?? new HashSet<IElement>(elem1SubCombs);
            intersection.IntersectWith(elem2SubCombs);
            return (double) intersection.Count / union.Count;
        }

        #endregion
    }
}