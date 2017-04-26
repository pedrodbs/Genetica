// ------------------------------------------
// <copyright file="SwapMutation.cs" company="Pedro Sequeira">
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
using Genesis.Elements.Functions;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
    /// <summary>
    ///     Represents a mutation operator that mutates a given <see cref="IElement" /> by swaping (reversing the order of) the
    ///     children of a randomly-selected function sub-element.
    /// </summary>
    public class SwapMutation : IMutationOperator
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

            // reverses the order of all function sub-elements
            var subElems = new List<IElement> {element};
            subElems.AddRange(element.GetSubElements());
            for (var i = 0; i < element.Count; i++)
            {
                var subElem = subElems[i];
                if (!(subElem is IFunction)) continue;
                mutations.Add(element.Replace((uint) i, subElem.CreateNew(subElem.Children.Reverse().ToList())));
            }

            mutations.Remove(element);
            return mutations;
        }

        /// <summary>
        ///     Mutates the given <see cref="IElement" /> by swaping (reversing the order of) the children of a randomly-selected
        ///     function sub-element.
        /// </summary>
        /// <param name="element">The element we want to mutate.</param>
        /// <returns>
        ///     A new <see cref="IElement" />by swaping (reversing the order of) the children of a randomly-selected function
        ///     sub-element.
        /// </returns>
        public IElement Mutate(IElement element)
        {
            if (element == null) return null;

            // define the mutation point randomly
            var mutatePoint = (uint) this._random.Next(element.Count);

            // get sub-element and swap children (inverse order)
            var elem = element.ElementAt(mutatePoint);
            if (elem.Children == null || elem.Children.Count < 2) return elem;
            var children = elem.Children.Reverse().ToList();

            // returns a replaced sub-element
            return elem.Replace(mutatePoint, elem.CreateNew(children));
        }

        #endregion
    }
}