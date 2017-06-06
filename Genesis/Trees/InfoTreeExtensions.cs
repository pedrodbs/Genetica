// ------------------------------------------
// <copyright file="InfoTreeExtensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/06/01
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Linq;

namespace Genesis.Trees
{
    public static class InfoTreeExtensions
    {
        #region Public Methods

        /// <summary>
        ///     Gets the degree of fullness of the tree.
        /// </summary>
        /// <returns>The degree of fullness of the tree.</returns>
        public static double GetFullness(this IInformationTree tree)
        {
            return 1d / tree.RootNode.Value *
                   tree.RootNode.Children.Sum(child => GetFullness((IInformationTreeNode) child, 0));
        }

        /// <summary>
        ///     Calculates the value of a node relative to the associated root node.
        /// </summary>
        /// <param name="node">The node whose relative value we want to calculate.</param>
        /// <returns>The value of a node relative to the associated root node.</returns>
        public static double GetRelativeValue(this IInformationTreeNode node)
        {
            return (double) node.Value / node.RootNode.Value;
        }

        #endregion

        #region Private & Protected Methods

        private static double GetFullness(IInformationTreeNode node, uint depth)
        {
            var sum = node.Children?.Sum(child => GetFullness((IInformationTreeNode) child, depth + 1));
            return sum != null ? node.Value / Math.Pow(2, depth) + (double) sum : -1;
        }

        #endregion
    }
}