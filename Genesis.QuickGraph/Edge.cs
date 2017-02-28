using QuickGraph;

namespace Genesis.QuickGraph
{
	public class Edge : IEdge<Vertex>
	{
		public Edge(Vertex source, Vertex target)
		{
			this.Source = source;
			this.Target = target;
		}

		public Vertex Source { get; }

		public Vertex Target { get; }
	}
}
