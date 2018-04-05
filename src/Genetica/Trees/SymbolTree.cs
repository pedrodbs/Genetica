// ------------------------------------------
// <copyright file="SymbolTree.cs" company="Pedro Sequeira">
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
//    Project: Genetica
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Genetica.Elements;

namespace Genetica.Trees
{
    /// <summary>
    ///     Implementation of the symbol-tree data structure in [1].
    /// </summary>
    /// <remarks>
    ///     [1] Foster, M. A. (2005). The program structure of genetic programming trees (Master’s thesis, School of Computer
    ///     Science and Information Technology, RMIT University Melbourne Australia).
    /// </remarks>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public class SymbolTree<TProgram> : IInformationTree<TProgram> where TProgram : ITreeProgram
    {
        #region Static Fields & Constants

        private const string ROOT_NODE_LABEL = "Root";

        #endregion

        #region Fields

        private readonly SymbolTreeNode _rootNode = new SymbolTreeNode(ROOT_NODE_LABEL, null);

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public IInformationTreeNode RootNode => this._rootNode;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Gets the ratio between the number of tree nodes and the total number of nodes inserted into the tree.
        ///     The rationale is to discover correlations between the size of the Symbol–Tree and the number of generation nodes.
        /// </summary>
        /// <returns>The node ratio.</returns>
        public double GetNodeRatio() => (double) (this.GetCount() * this._rootNode.Value) / this.GetNodeCount();

        /// <summary>
        ///     Gets the similarity between this and another symbol-tree. It counts the number of genetic nodes that have similar
        ///     sub–trees.
        /// </summary>
        /// <returns>The similarity between this and the given symbol-tree.</returns>
        /// <param name="other">The other tree we want to calculate the similarity with.</param>
        public double GetSimilarity(SymbolTree<TProgram> other)
        {
            var commonCount = GetCommonCount(this._rootNode, other._rootNode) -
                              Math.Min(this._rootNode.Value, other._rootNode.Value);
            return (double) commonCount / Math.Max(this.GetNodeCount(), other.GetNodeCount());
        }

        /// <inheritdoc />
        public void AddProgram(TProgram program)
        {
            this._rootNode.Value++;
            AddElement(program, this._rootNode, this._rootNode, new HashSet<SymbolTreeNode>());
        }

        /// <inheritdoc />
        public void AddPrograms(IEnumerable<TProgram> programs)
        {
            foreach (var program in programs)
                this.AddProgram(program);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this._rootNode.Children.Clear();
        }

        /// <inheritdoc />
        public uint GetCount() => (uint) (this._rootNode.GetCount() - 1);

        /// <inheritdoc />
        public uint GetNodeCount()
        {
            // discounts the (artificial) root node count
            return GetNodeCount(this._rootNode) - this._rootNode.Value;
        }

        /// <inheritdoc />
        public void Prune(double frequencyThreshold)
        {
            Prune(this._rootNode, (uint) (frequencyThreshold * this._rootNode.Value));
        }

        #endregion

        #region Private & Protected Methods

        private static void AddElement(
            TProgram program, SymbolTreeNode parent, SymbolTreeNode rootNode, HashSet<SymbolTreeNode> visited)
        {
            if (program == null) return;

            if (!parent.Children.ContainsKey(program.Label))
                parent.Children.Add(program.Label, new SymbolTreeNode(program.Label, rootNode));
            var node = parent.Children[program.Label];
            if (!visited.Contains(node))
            {
                node.Value++;
                visited.Add(node);
            }

            if (program.Children == null || program.Children.Count == 0) return;
            foreach (var child in program.Children)
                AddElement((TProgram) child, node, rootNode, visited);
        }

        private static uint GetCommonCount(SymbolTreeNode node1, SymbolTreeNode node2)
        {
            var commonCount = Math.Min(node1.Value, node2.Value);
            foreach (var child1 in node1.Children)
                if (node2.Children.ContainsKey(child1.Key))
                    commonCount += GetCommonCount(child1.Value, node2.Children[child1.Key]);
            return commonCount;
        }

        private static uint GetNodeCount(SymbolTreeNode node)
        {
            return (uint) (node.Value + node.Children.Values.Sum(child => GetNodeCount(child)));
        }

        private static void Prune(SymbolTreeNode node, uint frequencyThreshold)
        {
            if (node.Children == null || node.Children.Count == 0) return;
            var children = node.Children.ToList();
            node.Children.Clear();
            foreach (var child in children)
            {
                if (child.Value.Value < frequencyThreshold) continue;
                node.Children.Add(child);
                Prune(child.Value, frequencyThreshold);
            }
        }

        #endregion

        #region Nested type: SymbolTreeNode

        private class SymbolTreeNode : IInformationTreeNode
        {
            #region Fields

            private readonly string _symbol;

            #endregion

            #region Constructors

            public SymbolTreeNode(string symbol, SymbolTreeNode rootNode)
            {
                this._symbol = symbol;
                this.Children = new Dictionary<string, SymbolTreeNode>();
                this.RootNode = rootNode;
            }

            #endregion

            #region Properties & Indexers

            /// <summary>
            ///     Gets the list of child <see cref="SymbolTreeNode" /> associated with this node indexed by their label.
            /// </summary>
            public IDictionary<string, SymbolTreeNode> Children { get; }

            public IInformationTreeNode RootNode { get; }

            public uint Value { get; set; }

            IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children.Values.ToList().AsReadOnly();

            #endregion

            #region Public Methods

            public override string ToString() => this._symbol;

            #endregion

            #region Public Methods

            /// <summary>
            ///     Gets the number of nodes in the sub-tree created by this node.
            /// </summary>
            /// <returns>The number of nodes in the sub-tree created by this node.</returns>
            public ushort GetCount() => (ushort) (1 + this.Children.Values.Sum(child => child.GetCount()));

            #endregion
        }

        #endregion
    }
}