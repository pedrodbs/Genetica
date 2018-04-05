// ------------------------------------------
// <copyright file="Edge.cs" company="Pedro Sequeira">
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
//    Project: Genetica.Graphviz
//    Last updated: 03/23/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using QuickGraph;

namespace Genetica.Graphviz
{
    /// <summary>
    ///     Represents a vertex between two <see cref="Vertex" /> to be used in GraphViz.
    /// </summary>
    public class Edge : IEdge<Vertex>
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Vertex" /> between the two given vertexes.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The destination vertex.</param>
        public Edge(Vertex source, Vertex target)
        {
            this.Source = source;
            this.Target = target;
        }

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public Vertex Source { get; }

        /// <inheritdoc />
        public Vertex Target { get; }

        #endregion
    }
}