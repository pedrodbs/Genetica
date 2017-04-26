// ------------------------------------------
// <copyright file="PointMutation.cs" company="Pedro Sequeira">
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

namespace Genesis.Mutation
{
    /// <summary>
    ///     Allows the mutation of <see cref="IElement" /> by randomly replacing each sub-element by one element with the same
    ///     arity taken from some <see cref="PrimitiveSet" />.
    /// </summary>
    public class PointMutation : IMutationOperator
    {
        #region Fields

        private readonly Dictionary<int, List<IElement>> _primitives = new Dictionary<int, List<IElement>>();
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="PointMutation" /> with the given mutation probability and primitive set.
        /// </summary>
        /// <param name="primitives">The primitive set to be used in mutation operations.</param>
        /// <param name="mutationProbability">The probability of mutating each sub-element.</param>
        public PointMutation(PrimitiveSet primitives, double mutationProbability = 0.5d)
        {
            // stores primitives as a function of their arity (0, 1, ...)
            var allPrimitives = new List<IElement>(primitives.Functions);
            allPrimitives.AddRange(primitives.Terminals);
            foreach (var primitive in allPrimitives)
            {
                var arity = primitive.Children.Count;
                if (!this._primitives.ContainsKey(arity))
                    this._primitives.Add(arity, new List<IElement>());
                this._primitives[arity].Add(primitive);
            }

            this.MutationProbability = mutationProbability;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets or sets the probability of mutating each sub-element.
        /// </summary>
        public double MutationProbability { get; set; }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            this._primitives.Clear();
        }

        public IEnumerable<IElement> GetAllMutations(IElement element)
        {
            // checks element
            if (element?.Children == null) return new List<IElement>();

            // if terminal, just return list of terminals
            if (element.Children.Count == 0) return this._primitives[0];

            // mutates all children
            var numChildren = element.Children.Count;
            var newChildren = new List<IEnumerable<IElement>>(numChildren);
            for (var i = 0; i < numChildren; i++)
                newChildren.Add(this.GetAllMutations(element.Children[i]));

            // gets all possible combinations of children
            var childrenCombinations = newChildren.GetAllCombinations().ToList();
            var mutations = new HashSet<IElement>();
            foreach (var mutation in this._primitives[element.Children.Count])
            foreach (var childrenCombination in childrenCombinations)
                mutations.Add(mutation.CreateNew(childrenCombination));

            return mutations;
        }

        /// <summary>
        ///     Mutates the given <see cref="IElement" /> by randomly replacing each sub-element by one element with the same
        ///     arity taken from the defined <see cref="PrimitiveSet" />.
        /// </summary>
        /// <param name="element">The element we want to mutate.</param>
        /// <returns>
        ///     A new <see cref="IElement" /> created by randomly replacing each sub-element by one element with the same arity
        ///     taken from the defined <see cref="PrimitiveSet" />.
        /// </returns>
        public IElement Mutate(IElement element)
        {
            if (element == null) return null;
            if (element.Children == null) return element;

            // mutates all children
            var numChildren = element.Children.Count;
            var newChildren = new IElement[numChildren];
            for (var i = 0; i < numChildren; i++)
                newChildren[i] = this.Mutate(element.Children[i]);

            // checks whether to mutate this element (otherwise use same element)
            var primitive = element;
            if (this._random.NextDouble() < this.MutationProbability && this._primitives.ContainsKey(numChildren))
            {
                // mutates by creating a new random element with same arity and same children
                primitive = this._primitives[numChildren].GetRandomItem(this._random);
            }

            // creates new element with new children
            return primitive.CreateNew(newChildren);
        }

        #endregion
    }
}