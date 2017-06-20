// ------------------------------------------
// <copyright file="Extensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis.Graphviz
//    Last updated: 2017/06/05
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Genesis.Elements;
using Genesis.Graphviz.Patch;
using Genesis.Trees;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace Genesis.Graphviz
{
    public static class Extensions
    {
        #region Static Fields & Constants

        private const GraphvizImageType GRAPHVIZ_IMAGE_TYPE = GraphvizImageType.Svgz;
        private const int FONT_SIZE = 18;
        private const int ARG_FONT_SIZE = 7;
        private const string FONT_NAME = "Candara"; //"Tahoma";
        private const GraphvizVertexShape NODE_VERTEX_SHAPE = GraphvizVertexShape.Ellipse;
        private const GraphvizVertexStyle ROOT_VERTEX_STYLE = GraphvizVertexStyle.Diagonals;
        private const GraphvizVertexShape ARG_VERTEX_SHAPE = GraphvizVertexShape.Circle;
        private const int MAX_STROKE_COLOR = 180; //220;
        private const int MAX_FONT_COLOR = 140;

        #endregion

        #region Public Methods

        public static string ToGraphvizFile(
            this IInformationTree tree, string basePath, string fileName,
            GraphvizImageType imageType = GRAPHVIZ_IMAGE_TYPE)
        {
            return tree.RootNode.ToGraphvizFile(basePath, fileName, OnFormatInfoVertex, OnFormatInfoEdge, imageType);
        }

        public static string ToGraphvizFile(
            this IElement element, string basePath, string fileName,
            GraphvizImageType imageType = GRAPHVIZ_IMAGE_TYPE)
        {
            return element.ToGraphvizFile(basePath, fileName, OnFormatElementVertex, OnFormatElementEdge, imageType);
        }

        public static string ToGraphvizFile(
            this ITreeNode rootNode, string basePath, string fileName,
            MyGraphvizAlgorithm<Vertex, Edge>.MyFormatVertexEventHandler formatVertex,
            MyGraphvizAlgorithm<Vertex, Edge>.MyFormatEdgeAction formatEdge,
            GraphvizImageType imageType = GRAPHVIZ_IMAGE_TYPE)
        {
            var graph = new AdjacencyGraph<Vertex, Edge>();
            GraphAdd(rootNode, graph, new Dictionary<ITreeNode, Vertex>());

            var filePath = Path.Combine(basePath, $"{fileName}.dot");
            if (File.Exists(filePath))
                File.Delete(filePath);

            var viz = new MyGraphvizAlgorithm<Vertex, Edge>(graph) {ImageType = imageType};
            if (formatVertex != null) viz.FormatVertex += formatVertex;
            if (formatEdge != null) viz.FormatEdge += formatEdge;
            return viz.Generate(new FileDotEngine(), filePath);
        }

        #endregion

        #region Private & Protected Methods

        private static void FormatEdge(double relCount, MyGraphvizEdge edgeFormatter)
        {
            var edgeColor = (byte) ((1d - relCount) * MAX_STROKE_COLOR);
            edgeFormatter.StrokeColor = new MyGraphvizColor(255, edgeColor, edgeColor, edgeColor);
        }

        private static void FormatVertex(
            MyGraphvizVertex vertexFormatter, double relCount, string label, bool isRoot, bool isArgument = false)
        {
            var strokeColor = (byte) ((1d - relCount) * MAX_STROKE_COLOR);
            var fontColor = (byte) ((1d - relCount) * MAX_FONT_COLOR);
            vertexFormatter.Shape = isArgument ? ARG_VERTEX_SHAPE : NODE_VERTEX_SHAPE;
            if (isRoot) vertexFormatter.Style = ROOT_VERTEX_STYLE;
            vertexFormatter.Size = new GraphvizSizeF(0.15f, 0.15f);
            vertexFormatter.FixedSize = isArgument;
            vertexFormatter.Font = new GraphvizFont(FONT_NAME, isArgument ? ARG_FONT_SIZE : FONT_SIZE);
            vertexFormatter.FontColor = new MyGraphvizColor(255, fontColor, fontColor, fontColor);
            vertexFormatter.Label = label;
            vertexFormatter.StrokeColor = new MyGraphvizColor(255, strokeColor, strokeColor, strokeColor);
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

        private static void OnFormatElementEdge(object sender, MyGraphvizAlgorithm<Vertex, Edge>.MyFormatEdgeEventArgs e)
        {
            FormatEdge(1, e.EdgeFormatter);
        }

        private static void OnFormatElementVertex(
            object sender, MyGraphvizAlgorithm<Vertex, Edge>.MyFormatVertexEventArgs e)
        {
            var element = e.Vertex.Node as IElement;
            FormatVertex(e.VertexFormatter, 1, element?.Label ?? e.Vertex.Node.ToString(), false);
        }

        private static void OnFormatInfoEdge(object sender, MyGraphvizAlgorithm<Vertex, Edge>.MyFormatEdgeEventArgs e)
        {
            var treeNode = e.Edge.Target.Node as IInformationTreeNode;
            var relCount = (treeNode?.Value ?? 1d) / treeNode?.RootNode?.Value ?? 1d;
            FormatEdge(relCount, e.EdgeFormatter);
        }

        private static void OnFormatInfoVertex(
            object sender, MyGraphvizAlgorithm<Vertex, Edge>.MyFormatVertexEventArgs e)
        {
            if (!(e.Vertex.Node is IInformationTreeNode)) return;
            var treeNode = (IInformationTreeNode) e.Vertex.Node;
            var relCount = (double) treeNode.Value / treeNode.RootNode?.Value ?? 1d;
            var isArgument = treeNode is OrderedSymbolTree.ArgumentNode;
            FormatVertex(e.VertexFormatter, relCount,
                isArgument || treeNode is InformationTree.TreeNode
                    ? treeNode.ToString()
                    : $"{treeNode}:{treeNode.Value}",
                treeNode.RootNode == null, isArgument);
        }

        #endregion

        #region Nested type: FileDotEngine

        internal sealed class FileDotEngine : IDotEngine
        {
            #region Public Methods

            public string Run(GraphvizImageType imageType, string dot, string outputFileName)
            {
                // writes .dot file with the tree and converts it to an image
                File.WriteAllText(outputFileName, dot);
                var args = $"\"{outputFileName}\" -T{imageType.ToString().ToLower()} -O";
                var processInfo = new ProcessStartInfo("dot", args)
                                  {
                                      UseShellExecute = false,
                                      RedirectStandardOutput = true
                                  };
                Process.Start(processInfo);
                return outputFileName;
            }

            #endregion
        }

        #endregion
    }
}