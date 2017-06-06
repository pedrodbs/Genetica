// ------------------------------------------
// <copyright file="InformationTree.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;

namespace Genesis.Trees
{
    /// <summary>
    ///     Implementation of the information tree (iTree) data structure in [1].
    /// </summary>
    /// <remarks>
    ///     [1] Ekárt, A. and Gustafson, S. (2004, April). A data structure for improved GP analysis via efficient computation
    ///     and visualization of population measures. In European Conference on Genetic Programming (pp. 35-46). Springer
    ///     Berlin Heidelberg.
    /// </remarks>
    public class InformationTree : IInformationTree
    {
        #region Fields

        private TreeNode _rootNode = new TreeNode(null);

        #endregion

        #region Properties & Indexers

        public IInformationTreeNode RootNode => this._rootNode;

        #endregion

        #region Public Methods

        public void AddElement(IElement element)
        {
            AddElement(element, this._rootNode, this._rootNode);
        }

        public void AddElements(IEnumerable<IElement> elements)
        {
            foreach (var element in elements)
                this.AddElement(element);
        }

        public void Clear()
        {
            this._rootNode = new TreeNode(null);
        }

        public uint GetCount()
        {
            return this._rootNode.GetCount();
        }

        public uint GetNodeCount()
        {
            return GetNodeCount(this._rootNode);
        }

        public void Prune(double valueThreshold)
        {
            var rootNode = this._rootNode;
            Prune(rootNode, (uint) (valueThreshold * rootNode.Value));
        }

        #endregion

        #region Private & Protected Methods

        private static void AddElement(IElement element, TreeNode node, TreeNode rootNode)
        {
            if (element == null) return;
            node.Value++;

            if (element.Children == null) return;
            for (var i = 0; i < element.Children.Count; i++)
            {
                if (i >= node.Children.Count)
                    node.Children.Add(new TreeNode(rootNode));
                AddElement(element.Children[i], node.Children[i], rootNode);
            }
        }

        private static uint GetNodeCount(TreeNode node)
        {
            var sum = node.Value + node.Children?.Sum(child => GetNodeCount(child));
            return (uint) (sum ?? 0);
        }

        private static void Prune(TreeNode node, uint frequencyThreshold)
        {
            if (node.Children == null || node.Children.Count == 0) return;
            var children = node.Children.ToList();
            node.Children.Clear();
            foreach (var child in children)
            {
                if (child.Value < frequencyThreshold) continue;
                node.Children.Add(child);
                Prune(child, frequencyThreshold);
            }
        }

        #endregion

        #region Nested type: TreeNode

        public class TreeNode : IInformationTreeNode
        {
            #region Properties & Indexers

            public List<TreeNode> Children { get; }

            public IInformationTreeNode RootNode { get; }

            public uint Value { get; set; }

            IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children;

            #endregion

            #region Constructors

            public TreeNode(TreeNode rootNode)
            {
                this.RootNode = rootNode;
                this.Children = new List<TreeNode>();
            }

            #endregion

            #region Public Methods

            public override string ToString()
            {
                return this.Value.ToString();
            }

            #endregion

            #region Public Methods

            public ushort GetCount() => (ushort) (1 + this.Children.Sum(child => child.GetCount()));

            #endregion
        }

        #endregion
    }
}