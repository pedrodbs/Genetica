// ------------------------------------------
// <copyright file="UtilExtensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/09/06
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Genesis.Elements.Terminals;
using Genesis.Util;

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

        public static Range GetRange(this IElement element)
        {
            // checks for constant value
            if (element.IsConstant())
            {
                var value = element.GetValue();
                return new Range(value, value);
            }

            // checks for variable
            if (element is Variable) return ((Variable) element).Range;

            // collects info on ranges of all children 
            var childrenRanges = new List<IEnumerable<double>>();
            foreach (var child in element.Children)
            {
                var childRange = GetRange(child);
                childrenRanges.Add(new[] {childRange.Min, childRange.Max});
            }

            // gets all combinations between children ranges
            var min = double.MaxValue;
            var max = double.MinValue;
            var allRangeCombinations = childrenRanges.GetAllCombinations();
            foreach (var rangeCombination in allRangeCombinations)
            {
                // builds new element by replacing children with constant values (range min or max)
                var children = new IElement[rangeCombination.Count];
                for (var i = 0; i < rangeCombination.Count; i++)
                    children[i] = new Constant(rangeCombination[i]);
                var newElem = element.CreateNew(children);

                // checks min and max values from new elem value
                var val = newElem.GetValue();
                min = Math.Min(min, val);
                max = Math.Max(max, val);
            }

            return new Range(min, max);
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