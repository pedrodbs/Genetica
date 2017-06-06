// ------------------------------------------
// <copyright file="SimplifyMutation.cs" company="Pedro Sequeira">
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
    ///     This mutation operator simplifies (tries to shorten the expression of) a random descendant element of a given
    ///     <see cref="IElement" />.
    /// </summary>
    public class SimplifyMutation : IMutationOperator
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
            var mutations = new HashSet<IElement>();
            if (element == null) return mutations;

            // replaces each sub-element by its simplification
            var subElems = new List<IElement> {element};
            subElems.AddRange(element.GetSubElements());
            for (var i = 0; i < element.Length; i++)
                mutations.Add(element.Replace((uint) i, subElems[i].Simplify()));

            mutations.Remove(element);
            return mutations;
        }

        /// <summary>
        ///     Mutates the given <see cref="IElement" /> by simplifying a random sub-element of the given <see cref="IElement" />.
        /// </summary>
        /// <param name="element">The element we want to mutate.</param>
        /// <returns>A new <see cref="IElement" /> resulting of the simplification of one of the sub-elements.</returns>
        public IElement Mutate(IElement element)
        {
            // define the mutation point randomly
            var mutatePoint = (uint) this._random.Next(element.Length);

            // replaces with a simplified version of the sub-element
            var simp = element.ElementAt(mutatePoint).Simplify();
            return element.Replace(mutatePoint, simp);
        }

        #endregion
    }
}