// ------------------------------------------
// <copyright file="OrderedSymbolTree.cs" company="Pedro Sequeira">
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
using System.Globalization;
using System.Linq;
using Genesis.Elements;

namespace Genesis.Trees
{
    /// <summary>
    ///     Modified version of the symbol-tree data structure in [1] where parent nodes are created for each sub-element
    ///     corresponding to the position in which they appear in their parent's children list. In other words, this tree
    ///     allows the separation of function element's children according to the argument position in which they appear.
    /// </summary>
    /// <remarks>
    ///     [1] Foster, M. A. (2005). The program structure of genetic programming trees (Master’s thesis, School of Computer
    ///     Science and Information Technology, RMIT University Melbourne Australia).
    /// </remarks>
    public class OrderedSymbolTree : IInformationTree
    {
        #region Static Fields & Constants

        private const string ROOT_NODE_LABEL = "Root";

        #endregion

        #region Fields

        protected SymbolNode rootNode = new SymbolNode(ROOT_NODE_LABEL, 1, null);

        #endregion

        #region Properties & Indexers

        public IInformationTreeNode RootNode => this.rootNode;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Gets the ratio between the number of tree nodes and the total number of nodes inserted into the tree.
        ///     The rationale is to discover correlations between the size of the Symbol–Tree and the number of generation nodes.
        /// </summary>
        /// <returns>The node ratio.</returns>
        public double GetNodeRatio()
        {
            return (double) (this.GetCount() * this.rootNode.Value) / this.GetNodeCount();
        }

        /// <summary>
        ///     Gets the similarity between this and another symbol-tree. It counts the number of genetic nodes that have similar
        ///     sub–trees.
        /// </summary>
        /// <returns>The similarity between this and the given symbol-tree.</returns>
        /// <param name="other">The other tree we want to calculate the similarity with.</param>
        public double GetSimilarity(OrderedSymbolTree other)
        {
            var commonCount = GetCommonCount(this.rootNode, other.rootNode) -
                              Math.Min(this.rootNode.Value, other.rootNode.Value);
            return (double) commonCount / Math.Max(this.GetNodeCount(), other.GetNodeCount());
        }

        public virtual void AddElement(IElement element)
        {
            this.rootNode.Value++;
            AddElement(element, this.rootNode.Children[0], this.rootNode, new HashSet<SymbolNode>());
        }

        public void AddElements(IEnumerable<IElement> elements)
        {
            foreach (var element in elements)
                this.AddElement(element);
        }

        public void Clear()
        {
            this.rootNode = new SymbolNode(ROOT_NODE_LABEL, 1, null);
        }

        /// <summary>
        ///     Gets the number of nodes in the symbol-tree.
        /// </summary>
        /// <returns>The tree node count.</returns>
        public uint GetCount()
        {
            return (uint) (this.rootNode.GetCount() - 1);
        }

        /// <summary>
        ///     Gets the total number of program nodes (genetic nodes) used to build the tree [1].
        /// </summary>
        /// <returns>The total node count.</returns>
        public uint GetNodeCount()
        {
            // discounts the (artificial) root node count
            return GetNodeCount(this.rootNode) - this.rootNode.Value;
        }

        public void Prune(double frequencyThreshold)
        {
            Prune(this.rootNode, (uint) (frequencyThreshold * this.rootNode.Value));
        }

        #endregion

        #region Private & Protected Methods

        protected static void AddElement(
            IElement element, ArgumentNode node, SymbolNode rootNode, HashSet<SymbolNode> visited)
        {
            if (element == null) return;

            // creates child symbol node if needed and updates count
            var numChildren = element.Children?.Count ?? 0;
            if (!node.Children.ContainsKey(element.Label))
                node.Children.Add(element.Label, new SymbolNode(element.Label, numChildren, rootNode));
            var symbolNode = node.Children[element.Label];
            if (!visited.Contains(symbolNode))
            {
                symbolNode.Value++;
                visited.Add(symbolNode);
            }

            // recurses through children
            if (element.Children == null || element.Children.Count == 0) return;
            for (var i = 0; i < element.Children.Count; i++)
                AddElement(element.Children[i], symbolNode.Children[i], rootNode, visited);
        }

        private static uint GetCommonCount(SymbolNode node1, SymbolNode node2)
        {
            var commonCount = Math.Min(node1.Value, node2.Value);
            for (var i = 0; i < node1.Children.Length; i++)
            {
                var child1 = node1.Children[i];
                var child2 = node2.Children[i];
                foreach (var symbChild1 in child1.Children)
                    if (child2.Children.ContainsKey(symbChild1.Key))
                        commonCount += GetCommonCount(symbChild1.Value, child2.Children[symbChild1.Key]);
            }
            return commonCount;
        }

        private static uint GetNodeCount(SymbolNode node)
        {
            return (uint) (node.Value + node.Children.Sum(
                               child => child.Children.Values.Sum(
                                   symbChild => GetNodeCount(symbChild))));
        }

        private static void Prune(SymbolNode node, uint frequencyThreshold)
        {
            if (node.Children.Length == 0) return;

            foreach (var child in node.Children)
            {
                var children = child.Children.ToList();
                child.Children.Clear();
                foreach (var symbChild in children)
                {
                    if (symbChild.Value.Value < frequencyThreshold) continue;
                    child.Children.Add(symbChild);
                    Prune(symbChild.Value, frequencyThreshold);
                }
            }
        }

        #endregion

        #region Nested type: ArgumentNode

        public class ArgumentNode : IInformationTreeNode
        {
            #region Fields

            private readonly int _index;

            #endregion

            #region Properties & Indexers

            public IDictionary<string, SymbolNode> Children { get; }

            public SymbolNode Parent { get; }

            public IInformationTreeNode RootNode => this.Parent.RootNode;

            public uint Value { get { return this.Parent.Value; } set { } }

            IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children.Values.ToList();

            #endregion

            #region Constructors

            public ArgumentNode(int index, SymbolNode parent)
            {
                this._index = index;
                this.Children = new Dictionary<string, SymbolNode>();
                this.Parent = parent;
            }

            #endregion

            #region Public Methods

            public override string ToString()
            {
                return this._index.ToString(CultureInfo.InvariantCulture);
            }

            #endregion

            #region Public Methods

            public ushort GetCount() => (ushort) (1 + this.Children.Values.Sum(child => child.GetCount()));

            #endregion
        }

        #endregion

        #region Nested type: SymbolNode

        public class SymbolNode : IInformationTreeNode
        {
            #region Fields

            private readonly string _symbol;

            #endregion

            #region Properties & Indexers

            public ArgumentNode[] Children { get; }

            public IInformationTreeNode RootNode { get; }

            public uint Value { get; set; }

            IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children;

            #endregion

            #region Constructors

            public SymbolNode(string symbol, int count, SymbolNode rootNode)
            {
                this.Children = new ArgumentNode[count];
                for (var i = 0; i < count; i++)
                    this.Children[i] = new ArgumentNode(i, this);
                this.RootNode = rootNode;
                this._symbol = symbol;
            }

            #endregion

            #region Public Methods

            public override string ToString()
            {
                return this._symbol;
            }

            #endregion

            #region Public Methods

            public ushort GetCount() => (ushort) (1 + this.Children.Sum(child => child.GetCount()));

            #endregion
        }

        #endregion
    }
}