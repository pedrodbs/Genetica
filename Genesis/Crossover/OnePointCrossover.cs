// ------------------------------------------
// <copyright file="OnePointCrossover.cs" company="Pedro Sequeira">
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
using System.Linq;
using Genesis.Elements;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Crossover
{
    /// <summary>
    ///     Creates offspring by selecting a random crossover point in the common region between the two
    ///     <seealso cref="IElement" /> parents and then replacing a subtree of the first parent by the corresponding subtree
    ///     of the second parent.
    /// </summary>
    /// <remarks>
    ///     The common region between the parent elements corresponds to the subtrees where the parents have the same shape.
    /// </remarks>
    public class OnePointCrossover : ICrossoverOperator
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        public IElement Crossover(IElement parent1, IElement parent2)
        {
            // checks equal parents, returns first
            if (parent1.Equals(parent2)) return parent1;

            // gets common region between parents as a correspondence between indexes
            var commonRegionIndexes = parent1.GetCommonRegionIndexes(parent2);

            // chooses random crossover point in parent 1
            var crossoverPoint = commonRegionIndexes.Keys.ToList().GetRandomItem(this._random);

            // gets corresponding element in parent 2
            var element = parent2.ElementAt(commonRegionIndexes[crossoverPoint]);

            // replaces sub-element of parent 1 by the one of parent 2
            return parent1.Replace(crossoverPoint, element);
        }

        public IEnumerable<IElement> GetAllOffspring(IElement parent1, IElement parent2)
        {
            // checks equal parents, returns empty
            var offspring = new HashSet<IElement>();
            if (parent1.Equals(parent2)) return offspring;

            // gets common region between parents as a correspondence between indexes
            var commonRegion = parent1.GetCommonRegionIndexes(parent2);

            // for each crossover point in parent 1 replace by the corresponding sub-element of parent 2
            var subElems2 = parent2.GetSubElements();
            offspring.AddRange(
                commonRegion.Select(
                    indexes => indexes.Key == 0 ? parent2 : parent1.Replace(indexes.Key, subElems2[indexes.Value - 1])));

            // removes parents
            offspring.Remove(parent1);
            offspring.Remove(parent2);

            return offspring;
        }

        public void Dispose()
        {
        }

        #endregion
    }
}