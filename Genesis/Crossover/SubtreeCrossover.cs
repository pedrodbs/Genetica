// ------------------------------------------
// <copyright file="SubtreeCrossover.cs" company="Pedro Sequeira">
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
using Genesis.Elements.Functions;
using MathNet.Numerics.Random;

namespace Genesis.Crossover
{
    /// <summary>
    ///     This crossover operator replaces a random function sub-element of the first parent by a random element of the
    ///     second parent.
    /// </summary>
    public class SubtreeCrossover : ICrossoverOperator
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        /// <summary>
        ///     Creates a new element by replacing a random function sub-element of the first parent by a random element of the
        ///     second parent.
        /// </summary>
        /// <param name="parent1">The first parent element.</param>
        /// <param name="parent2">The second parent element.</param>
        /// <returns>A new element resulting from the crossover between the given parent elements.</returns>
        public IElement Crossover(IElement parent1, IElement parent2)
        {
            // define the first crossover point as a random function of parent 1
            uint crossPoint1;
            IElement elem = null;
            var subElems1 = parent1.GetSubElements();
            do
            {
                crossPoint1 = (uint) this._random.Next(parent1.Count);
                if (crossPoint1 > 0) elem = subElems1[crossPoint1 - 1];
            } while (elem == null || parent1.Children.Count > 0 && !(elem is IFunction));

            // define the second crossover point as a random element of parent 2
            elem = parent2.ElementAt((uint) this._random.Next(parent2.Count));
            return parent1.Replace(crossPoint1, elem);
        }

        public IEnumerable<IElement> GetAllOffspring(IElement parent1, IElement parent2)
        {
            // checks equal parents, returns empty
            if (parent1.Equals(parent2)) return new HashSet<IElement>();

            // gets parent 1's cross-over points (the function sub-elements)
            var crossPoints1 = new List<uint>();
            var subElems1 = parent1.GetSubElements();
            for (var i = 1u; i < parent1.Count; i++)
                if (subElems1[i - 1] is IFunction)
                    crossPoints1.Add(i);

            // creates new elements by replacing function sub-elements of parent 1 by sub-elements of parent 2
            var offspring = new HashSet<IElement>();
            var subElems2 = new List<IElement> {parent2};
            subElems2.AddRange(parent2.GetSubElements());
            foreach (var crossPoint1 in crossPoints1)
                for (var j = 0; j < parent2.Count; j++)
                    offspring.Add(parent1.Replace(crossPoint1, subElems2[j]));

            return offspring;
        }

        public void Dispose()
        {
        }

        #endregion
    }
}