// ------------------------------------------
// <copyright file="ShrinkMutation.cs" company="Pedro Sequeira">
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
using Genesis.Elements.Terminals;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
    /// <summary>
    ///     This mutation operator removes a random descendant node of a given <see cref="IElement" /> and replaces it with
    ///     a random <see cref="Terminal" /> element from a given <see cref="PrimitiveSet" />.
    /// </summary>
    public class ShrinkMutation : IMutationOperator
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());
        private readonly IList<Terminal> _terminals;

        #endregion

        #region Constructors

        public ShrinkMutation(PrimitiveSet primitives)
        {
            this._terminals = new List<Terminal>(primitives.Terminals);
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        public IEnumerable<IElement> GetAllMutations(IElement element)
        {
            var mutations = new HashSet<IElement>();
            if (element == null) return mutations;

            // replaces each sub-element by a terminal element
            for (var i = 0u; i < element.Length; i++)
                foreach (var terminal in this._terminals)
                    mutations.Add(element.Replace(i, terminal));

            return mutations;
        }

        /// <summary>
        ///     Mutates the given <see cref="IElement" /> by removing one of its sub-elements at random and replacing it with
        ///     a random <see cref="Terminal" /> element from a given <see cref="PrimitiveSet" />..
        /// </summary>
        /// <param name="element">The element we want to mutate.</param>
        /// <returns>A new <see cref="IElement" /> based on some change of one of the given element's sub-elements.</returns>
        public IElement Mutate(IElement element)
        {
            // define the mutation point randomly
            var mutatePoint = (uint) this._random.Next(element.Length);

            // replaces with a new random terminal element
            return element.Replace(mutatePoint, this._terminals.GetRandomItem(this._random));
        }

        #endregion
    }
}