// ------------------------------------------
// <copyright file="SubtreeMutation.cs" company="Pedro Sequeira">
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
using Genesis.Generation;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
    /// <summary>
    ///     Represents a mutation operator that replaces one sub-element of a given <see cref="IElement" /> by a new random
    ///     element generated using some <see cref="IElementGenerator" />.
    /// </summary>
    public class SubtreeMutation : IMutationOperator
    {
        #region Fields

        private readonly IElementGenerator _elementGenerator;
        private readonly PrimitiveSet _primitives;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        public SubtreeMutation(IElementGenerator elementGenerator, PrimitiveSet primitives, uint maxDepth)
        {
            this._primitives = primitives;
            this._elementGenerator = elementGenerator;
            this.MaxDepth = maxDepth;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets or sets the maximum depth of new random sub-elements.
        /// </summary>
        public uint MaxDepth { get; set; }

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        /// <summary>
        ///     Gets a list containing all possible elements resulting from applying this mutation operator.
        /// </summary>
        /// <param name="element">The element we want to mutate.</param>
        /// <returns>A list containing all possible elements resulting from applying this mutation operator.</returns>
        /// <remarks>
        ///     Because there may be a huge number of possible mutations resulting from the use of this operator, only the
        ///     primitives (functions + terminals) are considered, i.e., new elements of depth 0 or 1.
        /// </remarks>
        public IEnumerable<IElement> GetAllMutations(IElement element)
        {
            var mutations = new HashSet<IElement>();
            if (element == null) return mutations;

            var allPrimitives = new List<IElement>(this._primitives.Functions);
            allPrimitives.AddRange(this._primitives.Terminals);

            // replaces each sub-element by a primitive element
            for (var i = 0u; i < element.Count; i++)
                foreach (var primitive in allPrimitives)
                    mutations.Add(element.Replace(i, primitive));

            return mutations;
        }

        /// <summary>
        ///     Mutates the given <see cref="IElement" /> by replacing one of its sub-elements by a new random element generated
        ///     using the defined <see cref="IElementGenerator" />.
        /// </summary>
        /// <param name="element">The element we want to mutate.</param>
        /// <returns>
        ///     A new <see cref="IElement" /> by replacing one of its sub-elements by a new random element generated using the
        ///     defined <see cref="IElementGenerator" />.
        /// </returns>
        public IElement Mutate(IElement element)
        {
            if (element == null) return null;

            // define the mutation point randomly
            var mutatePoint = (uint) this._random.Next(element.Count);

            // define the new random element and creates replacement
            var newElem = this._elementGenerator.Generate(this._primitives, this.MaxDepth);
            return element.Replace(mutatePoint, newElem);
        }

        #endregion
    }
}