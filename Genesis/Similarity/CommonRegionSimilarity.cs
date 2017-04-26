// ------------------------------------------
// <copyright file="CommonRegionSimilarity.cs" company="Pedro Sequeira">
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

using System;
using Genesis.Elements;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Measures the similarity of elements based on the common-region of their expressions' trees. Common-region
    ///     similarity is given by the division of the number of sub-elements in the common-region and the max number of
    ///     sub-elements between the two given elements.
    /// </summary>
    public class CommonRegionSimilarity : ISimilarityMeasure
    {
        #region Public Methods

        public double Calculate(IElement elem1, IElement elem2)
        {
            if (elem1 == null || elem2 == null) return 0;
            if (elem1.Equals(elem2)) return 1;

            var commonRegionIdxs = elem1.GetCommonRegionIndexes(elem2);
            return (double) commonRegionIdxs.Count / Math.Max(elem1.Count, elem2.Count);
        }

        #endregion
    }
}