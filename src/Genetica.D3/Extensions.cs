// ------------------------------------------
// <copyright file="Extensions.cs" company="Pedro Sequeira">
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
//    Project: Genetica.D3
//    Last updated: 03/23/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Genetica.Elements;
using Genetica.Trees;
using Newtonsoft.Json;

namespace Genetica.D3
{
    /// <summary>
    ///     Contains several extension methods to allow printing <see cref="ITreeProgram{TOutput}" /> and
    ///     <see cref="IInformationTree{TProgram}" /> to d3.js files.
    /// </summary>
    public static class Extensions
    {
        #region Static Fields & Constants

        private const string NODE_NAME_PROP_NAME = "n";
        private const string NODE_VALUE_PROP_NAME = "v";
        private const string NODE_COUNT_PROP_NAME = "c";
        private const string NODE_IS_ARG_PROP_NAME = "a";
        private const string NODE_CHILDREN_PROP_NAME = "c";
        private const string GRAPH_NODES_PROP_NAME = "nodes";
        private const string GRAPH_LINKS_PROP_NAME = "links";
        private const string LINK_SOURCE_PROP_NAME = "s";
        private const string LINK_TARGET_PROP_NAME = "t";

        #endregion

        #region Public Methods

        /// <summary>
        ///     Saves the given <see cref="IInformationTree{TProgram}" /> to a d3.js tree file.
        /// </summary>
        /// <typeparam name="TProgram">The type of program.</typeparam>
        /// <param name="tree">The information tree to be saved.</param>
        /// <param name="filePath">The path of the file in which to save the given information tree.</param>
        /// <param name="formatting">The formatting to be used to write to the json file.</param>
        public static void ToD3JsonFile<TProgram>(
            this IInformationTree<TProgram> tree, string filePath, Formatting formatting = Formatting.None)
            where TProgram : ITreeProgram
        {
            ToD3JsonFile(tree.RootNode, filePath, formatting);
        }

        /// <summary>
        ///     Saves the tree of the given <see cref="ITreeNode" /> to a d3.js tree file.
        /// </summary>
        /// <param name="rootNode">The root node of the tree to be saved.</param>
        /// <param name="filePath">The path of the file in which to save the given tree.</param>
        /// <param name="formatting">The formatting to be used to write to the json file.</param>
        public static void ToD3JsonFile(
            this ITreeNode rootNode, string filePath, Formatting formatting = Formatting.None)
        {
            using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                var writer = new JsonTextWriter(sw) {Formatting = formatting};
                WriteGraphJson(rootNode, writer);
            }
        }

        /// <summary>
        ///     Saves the tree of the given <see cref="ITreeNode" /> to a d3.js tree file using a small tree layout.
        /// </summary>
        /// <param name="rootNode">The root node of the tree to be saved.</param>
        /// <param name="filePath">The path of the file in which to save the given tree.</param>
        /// <param name="formatting">The formatting to be used to write to the json file.</param>
        public static void ToD3TreeJsonFile(
            this ITreeNode rootNode, string filePath, Formatting formatting = Formatting.None)
        {
            using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                var writer = new JsonTextWriter(sw) {Formatting = formatting};
                WriteTreeJson(rootNode, writer);
            }
        }

        #endregion

        #region Private & Protected Methods

        private static void CollectData(
            ITreeNode node, UniqueItemsList<ITreeNode> nodes, ICollection<KeyValuePair<uint, uint>> links)
        {
            if (node == null) return;
            var nodeIdx = nodes.GetIndex(node);

            if (node.Children == null) return;
            foreach (var child in node.Children)
            {
                CollectData(child, nodes, links);
                links.Add(new KeyValuePair<uint, uint>(nodeIdx, nodes.GetIndex(child)));
            }
        }

        private static uint GetCount(ITreeNode node) =>
            node is ITreeProgram program ? program.Length : ((node as IInformationTreeNode)?.Value ?? 1);

        private static string GetLabel(ITreeNode node) =>
            node is ITreeProgram program ? program.Label : node.ToString();

        private static double GetValue(ITreeNode node)
        {
            if (!(node is IInformationTreeNode)) return 1;

            var treeNode = (IInformationTreeNode) node;
            return (double) treeNode.Value / treeNode.RootNode?.Value ?? 1d;
        }

        private static void WriteGraphJson(ITreeNode rootNode, JsonTextWriter writer)
        {
            writer.WriteStartObject();

            // collects node and link data
            var nodes = new UniqueItemsList<ITreeNode>();
            var links = new List<KeyValuePair<uint, uint>>();
            CollectData(rootNode, nodes, links);

            // writes nodes (name and value)
            writer.WritePropertyName(GRAPH_NODES_PROP_NAME);
            writer.WriteStartArray();
            for (var i = 0u; i < nodes.Count; i++)
            {
                var node = nodes.GetItem(i);
                writer.WriteStartObject();
                WriteNodeProperties(writer, node);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            // writes links (source and target)
            writer.WritePropertyName(GRAPH_LINKS_PROP_NAME);
            writer.WriteStartArray();
            foreach (var link in links)
            {
                writer.WriteStartObject();

                writer.WritePropertyName(LINK_SOURCE_PROP_NAME);
                writer.WriteValue(link.Key);

                writer.WritePropertyName(LINK_TARGET_PROP_NAME);
                writer.WriteValue(link.Value);

                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        private static void WriteNodeProperties(JsonTextWriter writer, ITreeNode node)
        {
            // writes name or node id
            writer.WritePropertyName(NODE_NAME_PROP_NAME);
            writer.WriteValue(GetLabel(node));

            // writes node relative value
            writer.WritePropertyName(NODE_VALUE_PROP_NAME);
            writer.WriteValue(Math.Round(GetValue(node), 2));

            // writes node count
            writer.WritePropertyName(NODE_COUNT_PROP_NAME);
            writer.WriteValue(GetCount(node));

            // checks argument node
            if (node is IArgInfoTreeNode)
            {
                writer.WritePropertyName(NODE_IS_ARG_PROP_NAME);
                writer.WriteValue("1");
            }
        }

        private static void WriteTreeJson(ITreeNode node, JsonTextWriter writer)
        {
            writer.WriteStartObject();

            // writes node properties
            WriteNodeProperties(writer, node);

            // writes node's children
            writer.WritePropertyName(NODE_CHILDREN_PROP_NAME);
            writer.WriteStartArray();
            foreach (var child in node.Children)
                WriteTreeJson(child, writer);
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        #endregion
    }
}