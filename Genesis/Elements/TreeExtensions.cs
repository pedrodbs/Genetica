// ------------------------------------------
// <copyright file="TreeExtensions.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;
using System.Linq;
using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;
using Genesis.Util;

namespace Genesis.Elements
{
    public static class TreeExtensions
    {
        #region Public Methods

        /// <summary>
        ///     Checks whether a given <see cref="IElement" /> contains the given sub-element.
        ///     A sub-element is an element that is a descendant of a given element.
        /// </summary>
        /// <returns><c>true</c>, if the given element is a descendant of the given element, <c>false</c> otherwise.</returns>
        /// <param name="element">The element for which to look for a descendant equal to the given element.</param>
        /// <param name="other">The element we want to know if it is a sub-element.</param>
        public static bool ContainsSubElement(this IElement element, IElement other)
        {
            // checks trivials
            if (element == null || other == null ||
                element.Count <= other.Count || element.Children?.Count == 0) return false;

            // search the descendants for the given element
            return element.Children != null &&
                   element.Children.Any(child => other.Equals(child) || child.ContainsSubElement(other));
        }

        /// <summary>
        ///     Deep clones the given element by creating an <see cref="IElement" /> that is an exact copy of the given
        ///     element's structure.
        /// </summary>
        /// <returns>The cloned element.</returns>
        /// <param name="element">The element to be cloned.</param>
        public static IElement DeepClone(this IElement element)
        {
            if (element == null) return null;
            if (element.Children == null) return element.Clone();

            var newChildren = new IElement[element.Children.Count];
            for (var i = 0; i < element.Children.Count; i++)
                newChildren[i] = element.Children[i].DeepClone();
            return element.CreateNew(newChildren);
        }

        /// <summary>
        ///     Get the <see cref="IElement" /> at the given index, where elements are indexed in a zero-based, depth first
        ///     search manner.
        /// </summary>
        /// <returns>
        ///     The <see cref="IElement" /> at the given index, or <see langword="null" /> if the given index is greater
        ///     than or equal to the <see cref="IElement.Count" />.
        /// </returns>
        /// <param name="element">The root element to search for the child element at the given index.</param>
        /// <param name="index">The index of the element we want to search for.</param>
        public static IElement ElementAt(this IElement element, uint index)
        {
            return ElementAt(element, ref index);
        }

        /// <summary>
        ///     Gets a <see cref="IDictionary{T,T}" /> representing the element indexes in the common
        ///     region between <paramref name="element" /> and <paramref name="otherElement" />. The dictionary represents
        ///     the index correspondence between the sub-elements of the given elements.
        /// </summary>
        /// <returns>The index correspondence between the sub-elements of the given elements.</returns>
        /// <param name="element">The first element</param>
        /// <param name="otherElement">The other element to get the common region.</param>
        public static IDictionary<uint, uint> GetCommonRegionIndexes(this IElement element, IElement otherElement)
        {
            var commonRegionIndexes = new Dictionary<uint, uint>();
            var idx1 = 0u;
            var idx2 = 0u;
            GetCommonRegionIndexes(element, otherElement, commonRegionIndexes, ref idx1, ref idx2);
            return commonRegionIndexes;
        }

        /// <summary>
        ///     Gets the maximum breadth of the element, i.e., the maximum number of <see cref="Terminal" /> elements
        ///     encountered starting from this element.
        /// </summary>
        /// <returns>The maximum breadth of the element.</returns>
        /// <param name="element">The root element to calculate the breadth.</param>
        public static uint GetMaxBreadth(this IElement element)
        {
            return element == null
                ? 0
                : element.Children == null || element.Children.Count == 0
                    ? 1
                    : (uint) element.Children.Sum(child => child.GetMaxBreadth());
        }

        /// <summary>
        ///     Gets the maximum depth of the element, i.e., the maximum number of child elements encountered starting from
        ///     this element until reaching a <see cref="T:Terminal" /> element.
        /// </summary>
        /// <returns>The maximum depth of the element.</returns>
        /// <param name="element">The root element to calculate the depth.</param>
        public static uint GetMaxDepth(this IElement element)
        {
            return element?.Children == null || element.Children.Count == 0
                ? 0
                : 1 + element.Children.Max(child => child.GetMaxDepth());
        }

        /// <summary>
        ///     Gets all the <see cref="IElement" /> sub-combinations of the given element. If the element is a
        ///     <see cref="Terminal" />, return a clone of itself, if it is a <see cref="IFunction" />,
        ///     then returns all the possible combinations between the sub-elements of the children and also the
        ///     sub-combinations of each child.
        /// </summary>
        /// <returns>Al the sub-combinations of the given element.</returns>
        /// <param name="element">The element we want to get the sub-combinations.</param>
        public static ISet<IElement> GetSubCombinations(this IElement element)
        {
            var combs = new HashSet<IElement>();
            if (element == null) return combs;

            // checks no more children
            if (element.Children == null || element.Children.Count == 0)
            {
                combs.Add(element);
                return combs;
            }

            // gets sub-elements from all children
            var childrenSubCombs = new List<IEnumerable<IElement>>();
            foreach (var child in element.Children)
            {
                var childSubCombs = child.GetSubCombinations();
                childrenSubCombs.Add(childSubCombs);

                // adds the sub-combinations of children to combination list
                foreach (var childSubComb in childSubCombs) combs.Add(childSubComb);
            }

            // creates new elements where each child is replaced by some sub-combination of it
            var allChildrenCombs = childrenSubCombs.GetAllCombinations();
            foreach (var children in allChildrenCombs)
                if (children.Count == element.Children.Count)
                    combs.Add(element.CreateNew(children));

            childrenSubCombs.Clear();
            return combs;
        }

        /// <summary>
        ///     Gets a <see cref="ISet{T}" /> containing all the descendant <see cref="IElement" /> of the given element.
        /// </summary>
        /// <returns>A set containing all the sub elements of the given element.</returns>
        /// <param name="element">The element we want to get the sub-elements.</param>
        public static IElement[] GetSubElements(this IElement element)
        {
            if (element == null) return null;
            var subElements = new IElement[element.Count-1];
            var index = 0;
            GetSubElements(element, ref index, element.Count-1, subElements);
            return subElements;
        }

        /// <summary>
        ///     Gets a set constaining all the <see cref="Terminal" /> sub-elements of the given element. This corresponds to the
        ///     leaf nodes of the expression tree of the given element.
        /// </summary>
        /// <param name="element">The element whose terminal sub-elements we want to retrieve.</param>
        /// <returns>A set constaining all the <see cref="Terminal" /> sub-elements of the given element.</returns>
        public static ISet<Terminal> GetTerminals(this IElement element)
        {
            if (element == null) return null;
            var terminals = new HashSet<Terminal>();
            GetTerminals(element, terminals);
            return terminals;
        }

        /// <summary>
        ///     Checks whether a given <see cref="IElement" /> is a sub-element of another <see cref="IElement" />.
        ///     A sub-element is an element that is a descendant of a given element.
        /// </summary>
        /// <returns><c>true</c>, if the element is a descendant of the other element, <c>false</c> otherwise.</returns>
        /// <param name="element">The element we want to know if it is a sub-element.</param>
        /// <param name="other">The element for which to look for a descendant equal to the given element.</param>
        public static bool IsSubElementOf(this IElement element, IElement other)
        {
            // checks trivials
            if (element == null || other == null ||
                element.Count >= other.Count || other.Children?.Count == 0) return false;

            // search the descendants for the given element
            return other.Children != null &&
                   other.Children.Any(child => element.Equals(child) || element.IsSubElementOf(child));
        }

        /// <summary>
        ///     Gets a new copy of <paramref name="element" /> where the descendant element at the given index is replaced by
        ///     <paramref name="newSubElement" />. Elements are indexed in a zero-based, depth first search manner.
        /// </summary>
        /// <returns>A copy of the element with the descendant at the given index replaced by the new element.</returns>
        /// <param name="element">The root element to copy and search for the sub-element at the given index.</param>
        /// <param name="index">The index of the sub-element we want to replace.</param>
        /// <param name="newSubElement">The new element to replace the one at the given index.</param>
        public static IElement Replace(this IElement element, uint index, IElement newSubElement)
        {
            return Replace(element, ref index, newSubElement);
        }

        /// <summary>
        ///     Gets a new copy of <paramref name="element" /> where all sub-elements that are equal to
        ///     <paramref name="oldSubElement" /> are replaced by <paramref name="newSubElement" />.
        /// </summary>
        /// <returns>
        ///     A copy of the element with the given descendant replaced by the new element. If the given element is equal to the
        ///     sub-element we want to replace, then the replacement is returned. If the given sub-element is not found, a copy of
        ///     the original element is returned, or <c>null</c> if the element is <c>null</c>.
        /// </returns>
        /// <param name="element">The root element to copy and search for the given sub-element .</param>
        /// <param name="oldSubElement">The sub-element we want to replace.</param>
        /// <param name="newSubElement">The new sub-element to replace the given one.</param>
        public static IElement Replace(this IElement element, IElement oldSubElement, IElement newSubElement)
        {
            if (element == null || oldSubElement == null || newSubElement == null)
                return element?.Clone();

            // checks if element is equal, return replacement
            if (element.Equals(oldSubElement)) return newSubElement;

            // replaces children recursively and creates a new element
            if (element.Children == null || element.Children.Count == 0) return element;
            var children = new IElement[element.Children.Count];
            for (var i = 0; i < element.Children.Count; i++)
                children[i] = element.Children[i].Replace(oldSubElement, newSubElement);
            return element.CreateNew(children);
        }

        #endregion

        #region Private & Protected Methods

        private static void GetSubElements(IElement element, ref int index, int maxIdx, IList<IElement> subElements)
        {
            if (index > maxIdx )
                return;

            if (index > 0) subElements[index - 1] = element;
            if (element.Children == null) return;
            foreach (var child in element.Children)
            {
                index++;
                GetSubElements(child, ref index, maxIdx, subElements);
            }
        }

        #endregion

        #region Private Methods

        private static IElement ElementAt(this IElement element, ref uint index)
        {
            if (index == 0) return element;
            if (element?.Children == null) return null;
            if (index >= element.Count)
            {
                index -= (uint) element.Count - 1;
                return null;
            }

            foreach (var child in element.Children)
            {
                index--;
                var elem = child.ElementAt(ref index);
                if (elem != null || index == 0)
                    return elem;
            }
            return null;
        }

        private static void GetCommonRegionIndexes(
            this IElement element, IElement otherElement, IDictionary<uint, uint> indexes, ref uint idx1, ref uint idx2)
        {
            // add the corresponding (common region) indexes
            indexes.Add(idx1, idx2);

            // check if children differ (different sub-structure)
            if (element?.Children == null || otherElement?.Children == null ||
                element.Children.Count != otherElement.Children.Count)
            {
                // just advance the indexes of both sub-trees
                if (element != null) idx1 += (uint) element.Count - 1;
                if (otherElement != null) idx2 += (uint) otherElement.Count - 1;
                return;
            }

            // elements have same number of children, iterate recursively
            for (var i = 0; i < element.Children.Count; i++)
            {
                idx1++;
                idx2++;
                GetCommonRegionIndexes(element.Children[i], otherElement.Children[i], indexes, ref idx1, ref idx2);
            }
        }

        private static void GetTerminals(this IElement element, ISet<Terminal> terminals)
        {
            // checks element is terminal, add to set
            var terminal = element as Terminal;
            if (terminal != null)
            {
                terminals.Add(terminal);
                return;
            }

            // searches children 
            if (element.Children != null)
                foreach (var child in element.Children)
                    GetTerminals(child, terminals);
        }

        private static IElement Replace(this IElement element, ref uint index, IElement newSubElement)
        {
            if (element == null) return null;
            if (index == 0) return newSubElement;
            if (element.Children == null) return element;

            var newChildren = element.Children.ToArray();
            for (var i = 0; i < element.Children.Count; i++)
            {
                index--;
                newChildren[i] = element.Children[i].Replace(ref index, newSubElement);
                if (index == 0)
                    break;
            }
            return element.CreateNew(newChildren);
        }

        #endregion
    }
}