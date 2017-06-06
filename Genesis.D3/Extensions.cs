// ------------------------------------------
// <copyright file="Extensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis.D3
//    Last updated: 2017/06/05
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Genesis.Elements;
using Genesis.Trees;
using Newtonsoft.Json;

namespace Genesis.D3
{
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

        public static void ToD3JsonFile(
            this IInformationTree tree, string filePath, Formatting formatting = Formatting.None)
        {
            tree.RootNode.ToD3JsonFile(filePath, formatting);
        }

        public static void ToD3JsonFile(
            this IElement element, string filePath, Formatting formatting = Formatting.None)
        {
            ToD3JsonFile((ITreeNode) element, filePath, formatting);
        }

        public static void ToD3JsonFile(
            this ITreeNode rootNode, string filePath, Formatting formatting = Formatting.None)
        {
            using (var fs = File.OpenWrite(filePath))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                var writer = new JsonTextWriter(sw) {Formatting = formatting};
                WriteGraphJson(rootNode, writer);
            }
        }

        public static void ToD3TreeJsonFile(
            this ITreeNode rootNode, string filePath, Formatting formatting = Formatting.None)
        {
            using (var fs = File.OpenWrite(filePath))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
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

        private static uint GetCount(ITreeNode node)
        {
            return node is IElement
                ? ((IElement) node).Length
                : ((node as IInformationTreeNode)?.Value ?? 1);
        }

        private static string GetLabel(ITreeNode node)
        {
            return node is IElement ? ((IElement) node).Label : node.ToString();
        }

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
            if (node is OrderedSymbolTree.ArgumentNode)
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