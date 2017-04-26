// ------------------------------------------
// <copyright file="IMutationOperator.cs" company="Pedro Sequeira">
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

namespace Genesis.Mutation
{
    /// <summary>
    ///     An interface for mutation operators, i.e., operators that take one element and create a new one by changing some
    ///     sub-element in a certain way.
    /// </summary>
    public interface IMutationOperator : IDisposable
    {
        #region Public Methods

        /// <summary>
        ///     Gets a list containing all possible elements resulting from applying this mutation operator.
        /// </summary>
        /// <param name="element">The element we want to mutate.</param>
        /// <returns>A list containing all possible elements resulting from applying this mutation operator.</returns>
        IEnumerable<IElement> GetAllMutations(IElement element);

        /// <summary>
        ///     Mutates the given <see cref="IElement" /> by creating a new one based on some change of one of its sub-elements.
        /// </summary>
        /// <param name="element">The element we want to mutate.</param>
        /// <returns>A new <see cref="IElement" /> based on some change of one of the given element's sub-elements.</returns>
        IElement Mutate(IElement element);

        #endregion
    }
}