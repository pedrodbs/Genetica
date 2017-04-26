// ------------------------------------------
// <copyright file="UniformCrossover.cs" company="Pedro Sequeira">
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
using System.Linq;
using Genesis.Elements;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Crossover
{
    /// <summary>
    ///     Creates offspring by visiting the points in the common region between the parents and flipping a coin at each
    ///     point to decide whether the corresponding offspring sub-element should be picked from the first or the second
    ///     parent.
    /// </summary>
    /// <remarks>
    ///     The common region between the parent elements corresponds to the subtrees where the parents have the same shape.
    /// </remarks>
    public class UniformCrossover : ICrossoverOperator
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        /// <summary>
        ///     Iterates the common (structural) region between the two parents, picking a sub-element from one of the parents at
        ///     random.
        /// </summary>
        /// <param name="parent1">The first parent element.</param>
        /// <param name="parent2">The second parent element.</param>
        /// <returns>A new element resulting from the uniform crossover between the given parent elements.</returns>
        public IElement Crossover(IElement parent1, IElement parent2)
        {
            // checks equal parents, returns first, otherwise perform crossover
            return parent1.Equals(parent2) ? parent1 : this.GetCrossover(parent1, parent2);
        }

        public IEnumerable<IElement> GetAllOffspring(IElement parent1, IElement parent2)
        {
            // checks equal parents, returns empty
            if (parent1.Equals(parent2)) return new HashSet<IElement>();

            // gets offspring
            var offspring = GetSubOffspring(parent1, parent2);

            // removes parents
            offspring.Remove(parent1);
            offspring.Remove(parent2);

            return offspring;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion

        #region Private & Protected Methods

        private static ISet<IElement> GetSubOffspring(IElement subElement1, IElement subElement2)
        {
            var subElements = new HashSet<IElement> {subElement1, subElement2};

            // check if children differ (different sub-structure) or no children -> nothing to do in this branch
            if (subElement1.Children == null || subElement2.Children == null ||
                subElement1.Children.Count != subElement2.Children.Count ||
                subElement1.Children.Count == 0)
                return subElements;

            // elements have same number of children, iterate recursively to get possible children in each sub-branch
            var numChildren = subElement1.Children.Count;
            var subChildren = new List<IEnumerable<IElement>>(numChildren);
            for (var i = 0; i < numChildren; i++)
                subChildren.Add(GetSubOffspring(subElement1.Children[i], subElement2.Children[i]).ToList());

            // gets all combinations of possible sub-branches
            var childrenCombinations = subChildren.GetAllCombinations().ToList();
            var subOffspring = new HashSet<IElement>();

            // if sub-elements are the same function, we only need one of them
            if (subElement1.GetType() == subElement2.GetType())
                subElements.Remove(subElement2);

            // for each sub-element, creates new sub-elements for each combination
            foreach (var subElement in subElements)
            foreach (var childrenCombination in childrenCombinations)
                subOffspring.Add(subElement.CreateNew(childrenCombination));

            return subOffspring;
        }

        private IElement GetCrossover(IElement parent1, IElement parent2)
        {
            // create new sub-element with corresponding sub-element taken from parent 1 or 2 (50% chance)
            var element = this._random.Next(2) == 0 ? parent1 : parent2;

            // check if children differ (different sub-structure) -> nothing to do in this branch, return
            if (parent1.Children == null || parent2.Children == null ||
                parent1.Children.Count != parent2.Children.Count)
                return element;

            // elements have same number of children, iterate recursively
            var numChildren = parent1.Children.Count;
            var children = new List<IElement>(numChildren);
            for (var i = 0; i < numChildren; i++)
                children.Add(GetCrossover(parent1.Children[i], parent2.Children[i]));

            // creates and returns new element
            return element.CreateNew(children);
        }

        #endregion
    }
}