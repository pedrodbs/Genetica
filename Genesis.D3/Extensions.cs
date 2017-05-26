// ------------------------------------------
// <copyright file="Extensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis.D3
//    Last updated: 2017/05/18
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.IO;
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
        private const string NODE_CHILDREN_PROP_NAME = "c";

        #endregion

        #region Public Methods

        public static void ToD3JsonFile(
            this InformationTree tree, string filePath, Formatting formatting = Formatting.None)
        {
            tree.RootNode.ToD3JsonFile(filePath, formatting);
        }

        public static void ToD3JsonFile(
            this IElement element, string filePath, Formatting formatting = Formatting.None)
        {
            ToD3JsonFile((ITreeNode) element, filePath, formatting);
        }

        public static void ToD3JsonFile(
            this SymbolTree tree, string filePath, Formatting formatting = Formatting.None)
        {
            tree.RootNode.ToD3JsonFile(filePath, formatting);
        }

        public static void ToD3JsonFile(
            this OrderedSymbolTree tree, string filePath, Formatting formatting = Formatting.None)
        {
            tree.RootNode.ToD3JsonFile(filePath, formatting);
        }

        public static void ToD3JsonFile(
            this ITreeNode rootNode, string filePath, Formatting formatting = Formatting.None)
        {
            using (var sw = new StreamWriter(filePath))
            {
                var writer = new JsonTextWriter(sw) {Formatting = formatting};
                WriteJson(rootNode, writer);
            }
        }

        #endregion

        #region Private & Protected Methods

        private static string GetLabel(ITreeNode node)
        {
            return node is IElement ? ((IElement) node).Label : node.ToString();
        }

        private static double GetValue(ITreeNode node)
        {
            if (node is SymbolTree.TreeNode)
            {
                var treeNode = (SymbolTree.TreeNode) node;
                return (double) treeNode.Value / treeNode.RootNode?.Value ?? 1d;
            }
            if (node is InformationTree.TreeNode)
            {
                var treeNode = (InformationTree.TreeNode) node;
                return (double) treeNode.Value / treeNode.RootNode?.Value ?? 1d;
            }
            if (node is OrderedSymbolTree.SymbolNode)
            {
                var treeNode = (OrderedSymbolTree.SymbolNode) node;
                return (double) treeNode.Value / treeNode.RootNode?.Value ?? 1d;
            }
            if (node is OrderedSymbolTree.ArgumentNode)
            {
                var treeNode = (OrderedSymbolTree.ArgumentNode) node;
                return (double) treeNode.Parent.Value / treeNode.Parent.RootNode?.Value ?? 1d;
            }

            return 1;
        }

        private static void WriteJson(ITreeNode node, JsonTextWriter writer)

        {
            writer.WriteStartObject();

            // writes name or node id
            writer.WritePropertyName(NODE_NAME_PROP_NAME);
            writer.WriteValue(GetLabel(node));

            // writes distance/dissimilarity
            writer.WritePropertyName(NODE_VALUE_PROP_NAME);
            writer.WriteValue(Math.Round(GetValue(node), 2));

            // writes node's children
            writer.WritePropertyName(NODE_CHILDREN_PROP_NAME);
            writer.WriteStartArray();
            foreach (var child in node.Children)
                WriteJson(child, writer);
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        #endregion
    }
}