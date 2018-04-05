// ------------------------------------------
// <copyright file="Vertex.cs" company="Pedro Sequeira">
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
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using MathNet.Numerics.Random;

namespace Genetica.Graphviz
{
    /// <summary>
    ///     Represents a vertex for <see cref="ITreeNode" /> types to be used in GraphViz.
    /// </summary>
    public class Vertex
    {
        #region Fields

        private readonly int _hashCode;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Vertex" /> with the given node.
        /// </summary>
        /// <param name="node">The node associated with the vertex.</param>
        public Vertex(ITreeNode node)
        {
            // every vertex will have its own hash key 
            // to ensure repetition in the graph display of leaf elements (terminals)
            this._hashCode = this._random.Next();
            this.Node = node;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     The <see cref="ITreeNode" /> associated with this vertex.
        /// </summary>
        public ITreeNode Node { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this._hashCode;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Node.ToString();
        }

        #endregion
    }
}