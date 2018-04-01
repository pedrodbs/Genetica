// ------------------------------------------
// <copyright file="InformationTree.cs" company="Pedro Sequeira">
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
//    Last updated: 03/31/2018
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
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public class InformationTree<TProgram> : IInformationTree<TProgram> where TProgram : ITreeProgram
    {
        #region Fields

        private readonly InfoTreeNode _rootNode = new InfoTreeNode(null);

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public IInformationTreeNode RootNode => this._rootNode;

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void AddProgram(TProgram program)
        {
            AddProgram(program, this._rootNode, this._rootNode);
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
        public uint GetCount()
        {
            return this._rootNode.GetCount();
        }

        /// <inheritdoc />
        public uint GetNodeCount()
        {
            return GetNodeCount(this._rootNode);
        }

        /// <inheritdoc />
        public void Prune(double valueThreshold)
        {
            var rootNode = this._rootNode;
            Prune(rootNode, (uint) (valueThreshold * rootNode.Value));
        }

        #endregion

        #region Private & Protected Methods

        private static void AddProgram(ITreeNode program, InfoTreeNode node, InfoTreeNode rootNode)
        {
            if (program == null) return;
            node.Value++;

            if (program.Children == null) return;
            for (var i = 0; i < program.Children.Count; i++)
            {
                if (i >= node.Children.Count)
                    node.Children.Add(new InfoTreeNode(rootNode));
                AddProgram(program.Children[i], node.Children[i], rootNode);
            }
        }

        private static uint GetNodeCount(InfoTreeNode node)
        {
            var sum = node.Value + node.Children?.Sum(child => GetNodeCount(child));
            return (uint) (sum ?? 0);
        }

        private static void Prune(InfoTreeNode node, uint frequencyThreshold)
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

        #region Nested type: InfoTreeNode

        /// <summary>
        ///     Represents a tree node for <see cref="InformationTree{TProgram}" /> objects.
        /// </summary>
        private class InfoTreeNode : IInformationTreeNode
        {
            #region Constructors

            /// <summary>
            ///     Creates a new <see cref="InfoTreeNode" /> with the given root node.
            /// </summary>
            /// <param name="rootNode">The root node of the tree that this node belongs to.</param>
            public InfoTreeNode(InfoTreeNode rootNode)
            {
                this.RootNode = rootNode;
                this.Children = new List<InfoTreeNode>();
            }

            #endregion

            #region Properties & Indexers

            /// <summary>
            ///     Gets the list of child <see cref="InfoTreeNode" /> associated with this node.
            /// </summary>
            public List<InfoTreeNode> Children { get; }

            public IInformationTreeNode RootNode { get; }

            public uint Value { get; set; }

            IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children;

            #endregion

            #region Public Methods

            public override string ToString() => this.Value.ToString();

            #endregion

            #region Public Methods

            /// <summary>
            ///     Gets the number of nodes in the sub-tree created by this node.
            /// </summary>
            /// <returns>The number of nodes in the sub-tree created by this node.</returns>
            public ushort GetCount() => (ushort) (1 + this.Children.Sum(child => child.GetCount()));

            #endregion
        }

        #endregion
    }
}