// ------------------------------------------
// <copyright file="Vertex.cs" company="Pedro Sequeira">
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

using System;
using MathNet.Numerics.Random;

namespace Genesis.Graphviz
{
    public class Vertex
    {
        #region Fields

        private readonly int _hashCode;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Properties & Indexers

        public ITreeNode Node { get; }

        #endregion

        #region Constructors

        public Vertex(ITreeNode node)
        {
            // every vertex will have its own hash key 
            // to ensure repetition in the graph display of leaf elements (terminals)
            this._hashCode = this._random.Next();
            this.Node = node;
        }

        #endregion

        #region Public Methods

        public override int GetHashCode()
        {
            return this._hashCode;
        }

        public override string ToString()
        {
            return this.Node.ToString();
        }

        #endregion
    }
}