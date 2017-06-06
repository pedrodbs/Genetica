// ------------------------------------------
// <copyright file="HoistMutation.cs" company="Pedro Sequeira">
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

using System;
using System.Collections.Generic;
using Genesis.Elements;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
    /// <summary>
    ///     Represents a mutation operator that selects a random sub-element of a given <see cref="IElement" />.
    /// </summary>
    public class HoistMutation : IMutationOperator
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        public IEnumerable<IElement> GetAllMutations(IElement element)
        {
            // simply return all sub-elements
            var allMutations = new List<IElement> {element};
            allMutations.AddRange(element.GetSubElements());
            return allMutations;
        }

        /// <summary>
        ///     Mutates the given <see cref="IElement" /> by selecting one of its sub-elements.
        /// </summary>
        /// <param name="element">The element we want to mutate.</param>
        /// <returns>A sub-element of the given <see cref="IElement" />.</returns>
        public IElement Mutate(IElement element)
        {
            if (element == null) return null;

            // define the mutation point randomly
            var mutatePoint = (uint) this._random.Next(element.Length);

            // return the sub-element
            return element.ElementAt(mutatePoint);
        }

        #endregion
    }
}