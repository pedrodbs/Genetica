// ------------------------------------------
// <copyright file="Extensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis.Graphviz
//    Last updated: 2017/03/29
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Diagnostics;
using System.IO;
using Genesis.Elements;
using Genesis.Graphviz.Patch;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace Genesis.Graphviz
{
    public static class Extensions
    {
        #region Static Fields & Constants

        private const int FONT_SIZE = 18;
        private const string FONT_NAME = "Candara"; //"Tahoma";
        private const GraphvizVertexShape NODE_VERTEX_SHAPE = GraphvizVertexShape.Ellipse;
        private const GraphvizVertexStyle ROOT_VERTEX_STYLE = GraphvizVertexStyle.Diagonals;
        private const int MAX_STROKE_COLOR = 220;
        private const int MAX_FONT_COLOR = 180;

        #endregion

        #region Public Methods

        public static string ToGraphvizFile(
            this InformationTree tree, string basePath, string fileName,
            GraphvizImageType imageType = GraphvizImageType.Svg)
        {
            return tree.RootNode.ToGraphvizFile(basePath, fileName, OnFormatInfoVertex, OnFormatInfoEdge, imageType);
        }

        public static string ToGraphvizFile(
            this IElement element, string basePath, string fileName,
            GraphvizImageType imageType = GraphvizImageType.Svg)
        {
            return element.ToGraphvizFile(basePath, fileName, OnFormatElementVertex, OnFormatElementEdge, imageType);
        }

        public static string ToGraphvizFile(
            this SymbolTree tree, string basePath, string fileName,
            GraphvizImageType imageType = GraphvizImageType.Svg)
        {
            return tree.RootNode.ToGraphvizFile(basePath, fileName, OnFormatSymbolVertex, OnFormatSymbolEdge, imageType);
        }

        public static string ToGraphvizFile(
            this ITreeNode rootNode, string basePath, string fileName,
            MyGraphvizAlgorithm<Vertex, Edge>.MyFormatVertexEventHandler formatVertex,
            MyGraphvizAlgorithm<Vertex, Edge>.MyFormatEdgeAction formatEdge,
            GraphvizImageType imageType = GraphvizImageType.Svg)
        {
            var graph = new AdjacencyGraph<Vertex, Edge>();
            GraphAdd(rootNode, graph);

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

        private static void FormatVertex(MyGraphvizVertex vertexFormatter, double relCount, string label, bool isRoot)
        {
            var strokeColor = (byte) ((1d - relCount) * MAX_STROKE_COLOR);
            var fontColor = (byte) ((1d - relCount) * MAX_FONT_COLOR);
            vertexFormatter.Shape = NODE_VERTEX_SHAPE;
            if (isRoot) vertexFormatter.Style = ROOT_VERTEX_STYLE;
            vertexFormatter.Font = new GraphvizFont(FONT_NAME, FONT_SIZE);
            vertexFormatter.FontColor = new MyGraphvizColor(255, fontColor, fontColor, fontColor);
            vertexFormatter.Label = label;
            vertexFormatter.StrokeColor = new MyGraphvizColor(255, strokeColor, strokeColor, strokeColor);
        }

        private static Vertex GraphAdd(ITreeNode node, AdjacencyGraph<Vertex, Edge> graph)
        {
            if (node == null) return null;
            var vertex = new Vertex(node);
            graph.AddVertex(vertex);

            if (node.Children == null) return vertex;
            foreach (var child in node.Children)
            {
                var childVertex = GraphAdd(child, graph);
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
            var treeNode = e.Edge.Target.Node as InformationTree.TreeNode;
            var relCount = (treeNode?.Value ?? 1d) / treeNode?.RootNode?.Value ?? 1d;
            FormatEdge(relCount, e.EdgeFormatter);
        }

        private static void OnFormatInfoVertex(
            object sender, MyGraphvizAlgorithm<Vertex, Edge>.MyFormatVertexEventArgs e)
        {
            var treeNode = e.Vertex.Node as InformationTree.TreeNode;
            var relCount = (treeNode?.Value ?? 1d) / treeNode?.RootNode?.Value ?? 1d;
            FormatVertex(e.VertexFormatter, relCount, e.Vertex.Node.ToString(), treeNode?.RootNode == null);
        }

        private static void OnFormatSymbolEdge(object sender, MyGraphvizAlgorithm<Vertex, Edge>.MyFormatEdgeEventArgs e)
        {
            var treeNode = e.Edge.Target.Node as SymbolTree.TreeNode;
            var relCount = (treeNode?.Value ?? 1d) / treeNode?.RootNode?.Value ?? 1d;
            FormatEdge(relCount, e.EdgeFormatter);
        }

        private static void OnFormatSymbolVertex(
            object sender, MyGraphvizAlgorithm<Vertex, Edge>.MyFormatVertexEventArgs e)
        {
            var treeNode = e.Vertex.Node as SymbolTree.TreeNode;
            var relCount = (treeNode?.Value ?? 1d) / treeNode?.RootNode?.Value ?? 1d;
            FormatVertex(e.VertexFormatter, relCount, e.Vertex.Node.ToString(), treeNode?.RootNode == null);
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