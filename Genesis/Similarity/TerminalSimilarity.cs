// ------------------------------------------
// <copyright file="TerminalSimilarity.cs" company="Pedro Sequeira">
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
using Genesis.Elements.Terminals;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Measures the similarity of elements based on the leaf nodes of their expressions. Terminal similarity is given by
    ///     the division between the number of common terminals and the number of all terminals of the two given elements.
    /// </summary>
    public class TerminalSimilarity : ISimilarityMeasure
    {
        #region Public Methods

        public double Calculate(IElement elem1, IElement elem2)
        {
            if (elem1 == null || elem2 == null) return 0;
            if (elem1.Equals(elem2)) return 1;

            var elem1Terminals = elem1.GetTerminals();
            var elem2Terminals = elem2.GetTerminals();
            var union = new HashSet<Terminal>(elem1Terminals);
            union.UnionWith(elem2Terminals);
            var intersection = elem1Terminals as HashSet<Terminal> ?? new HashSet<Terminal>(elem1Terminals);
            intersection.IntersectWith(elem2Terminals);
            return (double) intersection.Count / union.Count;
        }

        #endregion
    }
}