// ------------------------------------------
// <copyright file="SymbolTree.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/06/05
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;

namespace Genesis.Trees
{
    /// <summary>
    ///     Implementation of the symbol-tree data structure in [1].
    /// </summary>
    /// <remarks>
    ///     [1] Foster, M. A. (2005). The program structure of genetic programming trees (Master’s thesis, School of Computer
    ///     Science and Information Technology, RMIT University Melbourne Australia).
    /// </remarks>
    public class SymbolTree : IInformationTree
    {
        #region Static Fields & Constants

        private const string ROOT_NODE_LABEL = "Root";

        #endregion

        #region Fields

        private TreeNode _rootNode = new TreeNode(ROOT_NODE_LABEL, null);

        #endregion

        #region Properties & Indexers

        public IInformationTreeNode RootNode => this._rootNode;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Gets the ratio between the number of tree nodes and the total number of nodes inserted into the tree.
        ///     The rationale is to discover correlations between the size of the Symbol–Tree and the number of generation nodes.
        /// </summary>
        /// <returns>The node ratio.</returns>
        public double GetNodeRatio()
        {
            return (double) (this.GetCount() * this._rootNode.Value) / this.GetNodeCount();
        }

        /// <summary>
        ///     Gets the similarity between this and another symbol-tree. It counts the number of genetic nodes that have similar
        ///     sub–trees.
        /// </summary>
        /// <returns>The similarity between this and the given symbol-tree.</returns>
        /// <param name="other">The other tree we want to calculate the similarity with.</param>
        public double GetSimilarity(SymbolTree other)
        {
            var commonCount = GetCommonCount(this._rootNode, other._rootNode) -
                              Math.Min(this._rootNode.Value, other._rootNode.Value);
            return (double) commonCount / Math.Max(this.GetNodeCount(), other.GetNodeCount());
        }

        public void AddElement(IElement element)
        {
            this._rootNode.Value++;
            AddElement(element, this._rootNode, this._rootNode, new HashSet<TreeNode>());
        }

        public void AddElements(IEnumerable<IElement> elements)
        {
            foreach (var element in elements)
                this.AddElement(element);
        }

        public void Clear()
        {
            this._rootNode = new TreeNode(ROOT_NODE_LABEL, null);
        }

        public uint GetCount()
        {
            return (uint) (this._rootNode.GetCount() - 1);
        }

        public uint GetNodeCount()
        {
            // discounts the (artificial) root node count
            return GetNodeCount(this._rootNode) - this._rootNode.Value;
        }

        public void Prune(double frequencyThreshold)
        {
            Prune(this._rootNode, (uint) (frequencyThreshold * this._rootNode.Value));
        }

        #endregion

        #region Private & Protected Methods

        private static void AddElement(
            IElement element, TreeNode parent, TreeNode rootNode, HashSet<TreeNode> visited)
        {
            if (element == null) return;

            if (!parent.Children.ContainsKey(element.Label))
                parent.Children.Add(element.Label, new TreeNode(element.Label, rootNode));
            var node = parent.Children[element.Label];
            if (!visited.Contains(node))
            {
                node.Value++;
                visited.Add(node);
            }

            if (element.Children == null || element.Children.Count == 0) return;
            foreach (var child in element.Children)
                AddElement(child, node, rootNode, visited);
        }

        private static uint GetCommonCount(TreeNode node1, TreeNode node2)
        {
            var commonCount = Math.Min(node1.Value, node2.Value);
            foreach (var child1 in node1.Children)
                if (node2.Children.ContainsKey(child1.Key))
                    commonCount += GetCommonCount(child1.Value, node2.Children[child1.Key]);
            return commonCount;
        }

        private static uint GetNodeCount(TreeNode node)
        {
            return (uint) (node.Value + node.Children.Values.Sum(child => GetNodeCount(child)));
        }

        private static void Prune(TreeNode node, uint frequencyThreshold)
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

        #region Nested type: TreeNode

        private class TreeNode : IInformationTreeNode
        {
            #region Fields

            private readonly string _symbol;

            #endregion

            #region Properties & Indexers

            public IDictionary<string, TreeNode> Children { get; }

            public IInformationTreeNode RootNode { get; }

            public uint Value { get; set; }

            IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children.Values.ToList().AsReadOnly();

            #endregion

            #region Constructors

            public TreeNode(string symbol, TreeNode rootNode)
            {
                this._symbol = symbol;
                this.Children = new Dictionary<string, TreeNode>();
                this.RootNode = rootNode;
            }

            #endregion

            #region Public Methods

            public override string ToString()
            {
                return this._symbol;
            }

            #endregion

            #region Public Methods

            public ushort GetCount() => (ushort) (1 + this.Children.Values.Sum(child => child.GetCount()));

            #endregion
        }

        #endregion
    }
}