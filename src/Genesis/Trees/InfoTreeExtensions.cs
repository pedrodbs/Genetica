// ------------------------------------------
// <copyright file="InfoTreeExtensions.cs" company="Pedro Sequeira">
// 
//     Copyright (c) 2018 Pedro Sequeira
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 03/23/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Linq;
using Genesis.Elements;

namespace Genesis.Trees
{
    /// <summary>
    ///     Contains a set of extension methods for all kinds of <see cref="IInformationTree{TProgram}" />.
    /// </summary>
    public static class InfoTreeExtensions
    {
        #region Public Methods

        /// <summary>
        ///     Gets the degree of fullness of the tree.
        /// </summary>
        /// <returns>The degree of fullness of the tree.</returns>
        public static double GetFullness<TProgram>(this IInformationTree<TProgram> tree) where TProgram : ITreeProgram
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