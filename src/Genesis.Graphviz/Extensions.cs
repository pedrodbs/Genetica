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
//    Project: Genesis.Graphviz
//    Last updated: 03/30/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Genesis.Elements;
using Genesis.Trees;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace Genesis.Graphviz
{
    public static class Extensions
    {
        #region Static Fields & Constants

        private const GraphvizImageType GRAPHVIZ_IMAGE_TYPE = GraphvizImageType.Pdf;
        private const int FONT_SIZE = 18;
        private const int ARG_FONT_SIZE = 7;
        private const string FONT_NAME = "Candara"; //"Tahoma";
        private const GraphvizVertexShape NODE_VERTEX_SHAPE = GraphvizVertexShape.Ellipse;
        private const GraphvizVertexStyle ROOT_VERTEX_STYLE = GraphvizVertexStyle.Diagonals;
        private const GraphvizVertexShape ARG_VERTEX_SHAPE = GraphvizVertexShape.Circle;
        private const int MAX_STROKE_COLOR = 180; //220;
        private const int MAX_FONT_COLOR = 140;
        private const int GRAPHVIZ_TIMEOUT_MS = 2000;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Saves the given <see cref="IInformationTree{TProgram}" /> to an image file.
        /// </summary>
        /// <typeparam name="TProgram">The type of program.</typeparam>
        /// <param name="tree">The information tree to be saved.</param>
        /// <param name="basePath">The path in which to save the given information tree</param>
        /// <param name="fileName">The name of the image file to be saved (without extension)</param>
        /// <param name="imageType">The type of image file in which to save the tree.</param>
        /// <param name="timeout">The maximum time to wait for Graphviz to create the image file.</param>
        /// <returns>The path to file where the tree image file was saved.</returns>
        public static string ToGraphvizFile<TProgram>(
            this IInformationTree<TProgram> tree, string basePath, string fileName,
            GraphvizImageType imageType = GRAPHVIZ_IMAGE_TYPE, int timeout = GRAPHVIZ_TIMEOUT_MS)
            where TProgram : ITreeProgram
        {
            return tree.RootNode.ToGraphvizFile(
                basePath, fileName, OnFormatInfoVertex, OnFormatInfoEdge, imageType, timeout);
        }

        /// <summary>
        ///     Saves the given <see cref="ITreeNode" /> to an image file.
        /// </summary>
        /// <param name="rootNode">The root node of the tree to be saved.</param>
        /// <param name="basePath">The path in which to save the given tree</param>
        /// <param name="fileName">The name of the image file to be saved (without extension)</param>
        /// <param name="imageType">The type of image file in which to save the tree.</param>
        /// <param name="timeout">The maximum time to wait for Graphviz to create the image file.</param>
        /// <returns>The path to file where the tree image file was saved.</returns>
        public static string ToGraphvizFile(
            this ITreeNode rootNode, string basePath, string fileName,
            GraphvizImageType imageType = GRAPHVIZ_IMAGE_TYPE, int timeout = GRAPHVIZ_TIMEOUT_MS)
        {
            return rootNode.ToGraphvizFile(
                basePath, fileName, OnFormatElementVertex, OnFormatElementEdge, imageType, timeout);
        }

        /// <summary>
        ///     Saves the given <see cref="ITreeNode" /> to an image file.
        /// </summary>
        /// <param name="rootNode">The root node of the tree to be saved.</param>
        /// <param name="basePath">The path in which to save the given tree</param>
        /// <param name="fileName">The name of the image file to be saved (without extension)</param>
        /// <param name="formatVertex">The delegate used to format the vertexes.</param>
        /// <param name="formatEdge">The delegate used to format the edges.</param>
        /// <param name="imageType">The type of image file in which to save the tree.</param>
        /// <param name="timeout">The maximum time to wait for Graphviz to create the image file.</param>
        /// <returns>The path to file where the tree image file was saved.</returns>
        public static string ToGraphvizFile(
            this ITreeNode rootNode, string basePath, string fileName,
            FormatVertexEventHandler<Vertex> formatVertex,
            FormatEdgeAction<Vertex, Edge> formatEdge,
            GraphvizImageType imageType = GRAPHVIZ_IMAGE_TYPE, int timeout = GRAPHVIZ_TIMEOUT_MS)
        {
            var graph = new AdjacencyGraph<Vertex, Edge>();
            GraphAdd(rootNode, graph, new Dictionary<ITreeNode, Vertex>());

            var filePath = Path.Combine(basePath, $"{fileName}.dot");
            if (File.Exists(filePath))
                File.Delete(filePath);

            var viz = new GraphvizAlgorithm<Vertex, Edge>(graph) {ImageType = imageType};
            if (formatVertex != null) viz.FormatVertex += formatVertex;
            if (formatEdge != null) viz.FormatEdge += formatEdge;
            return viz.Generate(new FileDotEngine(timeout), filePath);
        }

        #endregion

        #region Private & Protected Methods

        private static void FormatEdge(double relCount, GraphvizEdge edgeFormatter)
        {
            var edgeColor = (byte) ((1d - relCount) * MAX_STROKE_COLOR);
            edgeFormatter.StrokeGraphvizColor = new GraphvizColor(255, edgeColor, edgeColor, edgeColor);
        }

        private static void FormatVertex(
            GraphvizVertex vertexFormatter, double relCount, string label, bool isRoot, bool isArgument = false)
        {
            var strokeColor = (byte) ((1d - relCount) * MAX_STROKE_COLOR);
            var fontColor = (byte) ((1d - relCount) * MAX_FONT_COLOR);
            vertexFormatter.Shape = isArgument ? ARG_VERTEX_SHAPE : NODE_VERTEX_SHAPE;
            if (isRoot) vertexFormatter.Style = ROOT_VERTEX_STYLE;
            vertexFormatter.Size = new GraphvizSizeF(0.15f, 0.15f);
            vertexFormatter.FixedSize = isArgument;
            vertexFormatter.Font = new GraphvizFont(FONT_NAME, isArgument ? ARG_FONT_SIZE : FONT_SIZE);
            vertexFormatter.FontColor = new GraphvizColor(255, fontColor, fontColor, fontColor);
            vertexFormatter.Label = label;
            vertexFormatter.StrokeColor = new GraphvizColor(255, strokeColor, strokeColor, strokeColor);
        }

        private static Vertex GraphAdd(
            ITreeNode node, AdjacencyGraph<Vertex, Edge> graph, IDictionary<ITreeNode, Vertex> vertices)
        {
            if (node == null) return null;

            //// checks if vertex already exists for this node
            //if (vertices.ContainsKey(node)) return vertices[node];

            // creates new vertex
            var vertex = new Vertex(node);

            //vertices.Add(node, vertex);
            graph.AddVertex(vertex);

            // iterates through children, creating edges between parent and child nodes
            if (node.Children == null) return vertex;
            foreach (var child in node.Children)
            {
                var childVertex = GraphAdd(child, graph, vertices);
                if (childVertex != null)
                    graph.AddEdge(new Edge(vertex, childVertex));
            }

            return vertex;
        }

        private static void OnFormatElementEdge(object sender, FormatEdgeEventArgs<Vertex, Edge> e)
        {
            FormatEdge(1, e.EdgeFormatter);
        }

        private static void OnFormatElementVertex(object sender, FormatVertexEventArgs<Vertex> e)
        {
            var element = e.Vertex.Node as ITreeProgram;
            FormatVertex(e.VertexFormatter, 1, element?.Label ?? e.Vertex.Node.ToString(), false);
        }

        private static void OnFormatInfoEdge(object sender, FormatEdgeEventArgs<Vertex, Edge> e)
        {
            var treeNode = e.Edge.Target.Node as IInformationTreeNode;
            var relCount = (treeNode?.Value ?? 1d) / treeNode?.RootNode?.Value ?? 1d;
            FormatEdge(relCount, e.EdgeFormatter);
        }

        private static void OnFormatInfoVertex(object sender, FormatVertexEventArgs<Vertex> e)
        {
            if (!(e.Vertex.Node is IInformationTreeNode)) return;
            var treeNode = (IInformationTreeNode) e.Vertex.Node;
            var relCount = (double) treeNode.Value / treeNode.RootNode?.Value ?? 1d;
            var isArgument = treeNode is IArgInfoTreeNode;
            FormatVertex(
                e.VertexFormatter, relCount, $"{treeNode}:{treeNode.Value}", treeNode.RootNode == null, isArgument);
        }

        #endregion

        #region Nested type: FileDotEngine

        internal sealed class FileDotEngine : IDotEngine
        {
            #region Fields

            private readonly int _timeout;

            #endregion

            #region Constructors

            public FileDotEngine(int timeout)
            {
                this._timeout = timeout;
            }

            #endregion

            #region Public Methods

            public string Run(GraphvizImageType imageType, string dot, string outputFileName)
            {
                // writes .dot file with the tree and converts it to an image
                File.WriteAllText(outputFileName, dot);
                var args = $"\"{outputFileName}\" -T{imageType.GetOutputFormatStr()} -O";
                var processInfo = new ProcessStartInfo("dot", args)
                                  {
                                      UseShellExecute = false,
                                      RedirectStandardOutput = true
                                  };
                var process = Process.Start(processInfo);
                process?.WaitForExit(this._timeout);
                return outputFileName;
            }

            #endregion
        }

        #endregion
    }
}