// ------------------------------------------
// <copyright file="ISimilarityMeasure.cs" company="Pedro Sequeira">
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

using Genesis.Elements;

namespace Genesis.Similarity
{
    /// <summary>
    ///     An interface for similarity measures between two elements, e.g., based on their tree structure.
    /// </summary>
    public interface ISimilarityMeasure
    {
        #region Public Methods

        /// <summary>
        ///     Calculates the similarity (or inverse distance) between two <see cref="IElement" />.
        /// </summary>
        /// <param name="elem1">The first element of the comparison.</param>
        /// <param name="elem2">The second element of the comparison.</param>
        /// <returns>
        ///     A number between <c>0</c> and <c>1</c> representing the calculated similarity between the given elements. A
        ///     value near 1 indicates a high similarity (or low distance) between the two elements, while a value near 0
        ///     represents a low similarity (or high distance) between the elements. If the elements are the same, it returns 1, if
        ///     any of the elements is <c>null</c>, it returns 0.
        /// </returns>
        double Calculate(IElement elem1, IElement elem2);

        #endregion
    }
}