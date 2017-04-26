// ------------------------------------------
// <copyright file="Edge.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis.QuickGraph
//    Last updated: 2017/03/29
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using QuickGraph;

namespace Genesis.Graphviz
{
    public class Edge : IEdge<Vertex>
    {
        #region Properties & Indexers

        public Vertex Source { get; }

        public Vertex Target { get; }

        #endregion

        #region Constructors

        public Edge(Vertex source, Vertex target)
        {
            this.Source = source;
            this.Target = target;
        }

        #endregion
    }
}