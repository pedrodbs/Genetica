// ------------------------------------------
// <copyright file="TerminalSimilarity.cs" company="Pedro Sequeira">
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
            var union = new HashSet<Terminal>(elem1Terminals.Keys);
            union.UnionWith(elem2Terminals.Keys);
            var unionCount = 0u;
            var intersectionCount = 0u;
            foreach (var terminal in union)
            {
                if (elem1Terminals.ContainsKey(terminal)) unionCount += elem1Terminals[terminal];
                if (elem2Terminals.ContainsKey(terminal)) unionCount += elem2Terminals[terminal];
                if (elem1Terminals.ContainsKey(terminal) && elem2Terminals.ContainsKey(terminal))
                    intersectionCount += elem1Terminals[terminal] + elem2Terminals[terminal];
            }
            return (double) intersectionCount / unionCount;
        }

        #endregion
    }
}