// ------------------------------------------
// <copyright file="MyGraphvizAlgorithm.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis.Graphviz
//    Last updated: 2017/09/07
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using QuickGraph;
using QuickGraph.Graphviz.Dot;

namespace Genesis.Graphviz.Patch
{
    /// <summary>
    ///     An algorithm that renders a graph to the GraphViz DOT format.
    ///     Override of the original QuickGraph code in <see href="https://quickgraph.codeplex.com/" /> in order to have
    ///     colored vertexes and edges.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public class MyGraphvizAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Delegates

        public delegate void MyFormatEdgeAction(object sender, MyFormatEdgeEventArgs e);

        public delegate void MyFormatVertexEventHandler(object sender, MyFormatVertexEventArgs e);

        #endregion

        #region Static Fields & Constants

        private static readonly Regex WriteLineReplace = new Regex("\n", RegexOptions.Multiline);

        #endregion

        #region Fields

        private readonly Dictionary<TVertex, int> _vertexIds = new Dictionary<TVertex, int>();

        #endregion

        #region Constructors

        public MyGraphvizAlgorithm(IEdgeListGraph<TVertex, TEdge> g)
            : this(g, MyGraphvizImageType.Svg)
        {
        }

        public MyGraphvizAlgorithm(
            IEdgeListGraph<TVertex, TEdge> g,
            MyGraphvizImageType imageType
        )
        {
            this.VisitedGraph = g;
            this.ImageType = imageType;
            this.GraphFormat = new GraphvizGraph();
            this.CommonVertexFormat = new MyGraphvizVertex();
            this.CommonEdgeFormat = new MyGraphvizEdge();
        }

        #endregion

        #region Properties & Indexers

        public MyGraphvizEdge CommonEdgeFormat { get; }

        public MyGraphvizVertex CommonVertexFormat { get; }

        public GraphvizGraph GraphFormat { get; }

        /// <summary>
        ///     Current image output type
        /// </summary>
        public MyGraphvizImageType ImageType { get; set; }

        /// <summary>
        ///     Dot output stream.
        /// </summary>
        public StringWriter Output { get; private set; }

        public IEdgeListGraph<TVertex, TEdge> VisitedGraph { get; set; }

        #endregion

        #region Public Methods

        public string Escape(string value)
        {
            return WriteLineReplace.Replace(value, "\\n");
        }

        public string Generate()
        {
            this._vertexIds.Clear();
            this.Output = new StringWriter();

            // build vertex id map
            var i = 0;
            foreach (var v in this.VisitedGraph.Vertices)
                this._vertexIds.Add(v, i++);

            this.Output.Write(this.VisitedGraph.IsDirected ? "digraph " : "graph ");
            this.Output.Write(this.GraphFormat.Name);
            this.Output.WriteLine(" {");

            var gf = this.GraphFormat.ToDot();
            if (gf.Length > 0)
                this.Output.WriteLine(gf);
            var vf = this.CommonVertexFormat.ToDot();
            if (vf.Length > 0)
                this.Output.WriteLine($"node [{vf}];");
            var ef = this.CommonEdgeFormat.ToDot();
            if (ef.Length > 0)
                this.Output.WriteLine($"edge [{ef}];");

            // initialize vertex map
            var colors = new Dictionary<TVertex, GraphColor>();
            foreach (var v in this.VisitedGraph.Vertices)
                colors[v] = GraphColor.White;
            var edgeColors = new Dictionary<TEdge, GraphColor>();
            foreach (var e in this.VisitedGraph.Edges)
                edgeColors[e] = GraphColor.White;

            this.WriteVertices(colors, this.VisitedGraph.Vertices);
            this.WriteEdges(edgeColors, this.VisitedGraph.Edges);

            this.Output.WriteLine("}");
            return this.Output.ToString();
        }

        public string Generate(IMyDotEngine dot, string outputFileName)
        {
            this.Generate();
            return dot.Run(this.ImageType, this.Output.ToString(), outputFileName);
        }

        #endregion

        #region Private & Protected Methods

        private void OnFormatEdge(TEdge e)
        {
            if (this.FormatEdge == null) return;
            var ev = new MyGraphvizEdge();
            this.FormatEdge(this, new MyFormatEdgeEventArgs(e, ev));
            this.Output.Write(" {0}", ev.ToDot());
        }

        private void OnFormatVertex(TVertex v)
        {
            this.Output.Write("{0} ", this._vertexIds[v]);
            if (this.FormatVertex != null)
            {
                var gv = new MyGraphvizVertex {Label = v.ToString()};
                this.FormatVertex(this, new MyFormatVertexEventArgs(v, gv));

                var s = gv.ToDot();
                if (s.Length != 0)
                    this.Output.Write("[{0}]", s);
            }
            this.Output.WriteLine(";");
        }

        private void WriteEdges(
            IDictionary<TEdge, GraphColor> edgeColors,
            IEnumerable<TEdge> edges)
        {
            foreach (var e in edges)
            {
                if (edgeColors[e] != GraphColor.White)
                    continue;

                this.Output.Write($"{this._vertexIds[e.Source]} -> {this._vertexIds[e.Target]} [");
                this.OnFormatEdge(e);
                this.Output.WriteLine("];");

                edgeColors[e] = GraphColor.Black;
            }
        }

        private void WriteVertices(
            IDictionary<TVertex, GraphColor> colors,
            IEnumerable<TVertex> vertices)
        {
            foreach (var v in vertices)
                if (colors[v] == GraphColor.White)
                {
                    this.OnFormatVertex(v);
                    colors[v] = GraphColor.Black;
                }
        }

        #endregion

        public event MyFormatEdgeAction FormatEdge;

        public event MyFormatVertexEventHandler FormatVertex;

        #region Nested type: MyFormatEdgeEventArgs

        public sealed class MyFormatEdgeEventArgs : EdgeEventArgs<TVertex, TEdge>
        {
            #region Constructors

            internal MyFormatEdgeEventArgs(TEdge e, MyGraphvizEdge edgeFormatter)
                : base(e)
            {
                this.EdgeFormatter = edgeFormatter;
            }

            #endregion

            #region Properties & Indexers

            /// <summary>
            ///     Edge formatter
            /// </summary>
            public MyGraphvizEdge EdgeFormatter { get; }

            #endregion
        }

        #endregion

        #region Nested type: MyFormatVertexEventArgs

        public sealed class MyFormatVertexEventArgs : VertexEventArgs<TVertex>
        {
            #region Constructors

            internal MyFormatVertexEventArgs(TVertex v, MyGraphvizVertex vertexFormatter) : base(v)
            {
                this.VertexFormatter = vertexFormatter;
            }

            #endregion

            #region Properties & Indexers

            public MyGraphvizVertex VertexFormatter { get; }

            #endregion
        }

        #endregion
    }
}