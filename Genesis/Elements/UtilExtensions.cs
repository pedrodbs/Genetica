// ------------------------------------------
// <copyright file="UtilExtensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/08/28
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using Genesis.Elements.Terminals;

namespace Genesis.Elements
{
    public static class UtilExtensions
    {
        #region Public Methods

        /// <summary>
        ///     Counts the number of each kind of primitive within the given element.
        /// </summary>
        /// <param name="element">The element whose primitive elements we want to count.</param>
        /// <returns>A list with all primitive elements and corresponding count.</returns>
        public static IDictionary<IElement, uint> CountPrimitives(this IElement element)
        {
            var primitivesCounts = new Dictionary<IElement, uint>();
            CountPrimitives(element, primitivesCounts);
            return primitivesCounts;
        }

        #endregion

        #region Private & Protected Methods

        private static void CountPrimitives(
            this IElement element, IDictionary<IElement, uint> primitivesCounts)
        {
            // gets generic primitive
            var children = new List<IElement>();
            for (var i = 0; i < (element.Children?.Count ?? 1); i++)
                children.Add(new Constant(0));
            var primitive = element.CreateNew(children);

            // adds count
            if (primitivesCounts.ContainsKey(primitive))
                primitivesCounts[primitive]++;
            else primitivesCounts[primitive] = 1;

            // recurses through children
            if (element.Children != null && element.Children.Count != 0)
                foreach (var child in element.Children)
                    CountPrimitives(child, primitivesCounts);
        }

        #endregion
    }
}