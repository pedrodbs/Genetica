// ------------------------------------------
// <copyright file="ContextPreservingCrossover.cs" company="Pedro Sequeira">
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
using MathNet.Numerics.Random;

namespace Genesis.Crossover
{
    /// <summary>
    ///     Creates offspring by choosing a random sub-element of the first parent and replacing with a sub-element of the
    ///     second parent that has the same index.
    /// </summary>
    public class ContextPreservingCrossover : ICrossoverOperator
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        /// <summary>
        ///     Creates a new element by choosing a random sub-element of the first parent and replacing with a sub-element of the
        ///     second parent that has the same index.
        /// </summary>
        /// <param name="parent1">The first parent element.</param>
        /// <param name="parent2">The second parent element.</param>
        /// <returns>A new element resulting from the crossover between the given parent elements.</returns>
        public IElement Crossover(IElement parent1, IElement parent2)
        {
            // checks equal parents, returns first
            if (parent1.Equals(parent2)) return parent1;

            // checks shorter tree
            var maxIndex = (int) Math.Min(parent1.Length, parent2.Length);

            // gets random crossover point
            var crossoverPoint = (uint) this._random.Next(maxIndex);

            // gets corresponding element in parent 2
            var element = parent2.ElementAt(crossoverPoint);

            // replaces sub-element of parent 1 by the one of parent 2
            return parent1.Replace(crossoverPoint, element);
        }

        public IEnumerable<IElement> GetAllOffspring(IElement parent1, IElement parent2)
        {
            // checks equal parents, returns empty
            if (parent1.Equals(parent2)) return new HashSet<IElement>();

            // checks shorter tree
            var maxIndex = (int) Math.Min(parent1.Length, parent2.Length);

            // replaces each sub-element of parent 1 by the index-corresponding one of parent 2
            var offspring = new HashSet<IElement>();
            var subElems2 = parent2.GetSubElements();
            for (var i = 1u; i < maxIndex; i++)
                offspring.Add(parent1.Replace(i, subElems2[i - 1]));

            return offspring;
        }

        public void Dispose()
        {
        }

        #endregion
    }
}