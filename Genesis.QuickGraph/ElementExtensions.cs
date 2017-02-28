using System.Diagnostics;
using System.IO;
using Genesis.Elements;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace Genesis.QuickGraph
{
	public static class ElementExtensions
	{
		private const int FONT_SIZE = 11;
		private const string FONT_NAME = "Tahoma";

		public static string ToGraphvizFile(
			this IElement element, GraphvizImageType imageType,
			string directory, string fileName)
		{
			var graph = new AdjacencyGraph<Vertex, Edge>();
			GraphAdd(element, graph);

			var filePath = Path.Combine(directory, $"{fileName}.dot");
			if (File.Exists(filePath))
				File.Delete(filePath);

			var viz = new GraphvizAlgorithm<Vertex, Edge>(graph) { ImageType = imageType };
			viz.FormatVertex += FormatVertex;
			return viz.Generate(new FileDotEngine(), filePath);
		}

		private static void FormatVertex(object sender, FormatVertexEventArgs<Vertex> e)
		{
			e.VertexFormatter.Font = new GraphvizFont(FONT_NAME, FONT_SIZE);
			e.VertexFormatter.Label = e.Vertex.ToString();
		}

		private static Vertex GraphAdd(
			IElement element, AdjacencyGraph<Vertex, Edge> graph)
		{
			if (element == null) return null;
			var vertex = new Vertex(element);
			graph.AddVertex(vertex);

			if (element.Children == null) return vertex;
			foreach (var child in element.Children)
			{
				var childVertex = GraphAdd(child, graph);
				if (childVertex != null)
					graph.AddEdge(new Edge(vertex, childVertex));
			}
			return vertex;
		}

		internal sealed class FileDotEngine : IDotEngine
		{
			public string Run(GraphvizImageType imageType, string dot, string outputFileName)
			{
				// writes .dot file with the tree and converts it to an image
				File.WriteAllText(outputFileName, dot);
				var args = $"\"{outputFileName}\" -T{imageType.ToString().ToLower()} -O";
				var processInfo = new ProcessStartInfo("dot", args) { UseShellExecute = false, RedirectStandardOutput = true};
				Process.Start(processInfo);
				return outputFileName;
			}
		}
	}
}
