// ------------------------------------------
// <copyright file="ICrossoverOperator.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/03/08
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Genesis.Elements;

namespace Genesis.Crossover
{
    /// <summary>
    ///     An interface for crossover operators, i.e., operators that take two parent elements and produce a new element that
    ///     results in some combination of the parent's sub-elements.
    /// </summary>
    public interface ICrossoverOperator : IDisposable
    {
        #region Public Methods

        /// <summary>
        ///     Creates a new element resulting from the crossover between the given parent elements.
        /// </summary>
        /// <param name="parent1">The first parent element.</param>
        /// <param name="parent2">The second parent element.</param>
        /// <returns>A new element resulting from the crossover between the given parent elements.</returns>
        IElement Crossover(IElement parent1, IElement parent2);

        /// <summary>
        ///     Gets a list containing all possible offspring elements resulting from applying this crossover operator.
        /// </summary>
        /// <param name="parent1">The first parent element.</param>
        /// <param name="parent2">The second parent element.</param>
        /// <returns>A list containing all possible offspring elements resulting from applying this crossover operator.</returns>
        IEnumerable<IElement> GetAllOffspring(IElement parent1, IElement parent2);

        #endregion
    }
}