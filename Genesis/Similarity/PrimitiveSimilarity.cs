// ------------------------------------------
// <copyright file="PrimitiveSimilarity.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/06/06
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
    ///     Measures the similarity of elements based on the primitives in their expressions. Primitive similarity is given by
    ///     the division between the number of common primitives and the number of all primitives of the two given elements.
    /// </summary>
    public class PrimitiveSimilarity : ISimilarityMeasure
    {
        #region Public Methods

        public double Calculate(IElement elem1, IElement elem2)
        {
            if (elem1 == null || elem2 == null) return 0;
            if (elem1.Equals(elem2)) return 1;

            var elem1Primitives = elem1.GetPrimitives();
            var elem2Primitives = elem2.GetPrimitives();
            var union = new HashSet<IElement>(elem1Primitives.Keys);
            union.UnionWith(elem2Primitives.Keys);
            var unionCount = 0u;
            var intersectionCount = 0u;
            foreach (var elem in union)
            {
                if (elem1Primitives.ContainsKey(elem)) unionCount += elem1Primitives[elem];
                if (elem2Primitives.ContainsKey(elem)) unionCount += elem2Primitives[elem];
                if (elem1Primitives.ContainsKey(elem) && elem2Primitives.ContainsKey(elem))
                    intersectionCount += elem1Primitives[elem] + elem2Primitives[elem];
            }
            return (double) intersectionCount / unionCount;
        }

        #endregion
    }
}