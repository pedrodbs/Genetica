// ------------------------------------------
// <copyright file="MyGraphvizColor.cs" company="Pedro Sequeira">
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

namespace Genesis.Graphviz.Patch
{
    /// <summary>
    ///     Override of the original QuickGraph code in <see href="https://quickgraph.codeplex.com/" /> in order to have
    ///     colored vertexes and edges.
    /// </summary>
    public struct MyGraphvizColor : IEquatable<MyGraphvizColor>
    {
        #region Properties & Indexers

        public byte A { get; }

        public byte B { get; }

        public static MyGraphvizColor Black => new MyGraphvizColor(0xFF, 0, 0, 0);

        public byte G { get; }

        public static MyGraphvizColor LightYellow => new MyGraphvizColor(0xFF, 0xFF, 0xFF, 0xE0);

        public byte R { get; }

        public static MyGraphvizColor White => new MyGraphvizColor(0xFF, 0xFF, 0xFF, 0xFF);

        #endregion

        #region Constructors

        public MyGraphvizColor(byte a, byte r, byte g, byte b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        #endregion

        #region Public Methods

        public override int GetHashCode()
        {
            return (this.A << 24) | (this.R << 16) | (this.G << 8) | this.B;
        }

        #endregion

        #region Public Methods

        public bool Equals(MyGraphvizColor other)
        {
            return this.A == other.A && this.R == other.R && this.G == other.G && this.B == other.B;
        }

        #endregion
    }
}