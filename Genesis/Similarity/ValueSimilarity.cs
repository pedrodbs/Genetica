// ------------------------------------------
// <copyright file="ValueSimilarity.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/03/16
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using Genesis.Elements;
using Genesis.Elements.Terminals;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Measures the similarity of elements in terms of the values that they can compute when we replace their
    ///     <see cref="Constant" /> elements by random variables.
    /// </summary>
    public class ValueSimilarity : ISimilarityMeasure
    {
        #region Public Methods

        public double Calculate(IElement elem1, IElement elem2)
        {
            if (elem1 == null || elem2 == null) return 0;
            if (elem1.Equals(elem2)) return 1;

            var valueRmsd = elem1.GetValueRmsd(elem2);
            return valueRmsd.Equals(double.NaN) ? 0 : 1d - Math.Min(1, valueRmsd);
        }

        #endregion
    }
}