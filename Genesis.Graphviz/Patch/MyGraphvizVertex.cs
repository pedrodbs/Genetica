// ------------------------------------------
// <copyright file="MyGraphvizVertex.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;
using System.IO;
using QuickGraph.Graphviz.Dot;

namespace Genesis.Graphviz.Patch
{
    /// <summary>
    ///     Override of the original QuickGraph code in <see href="https://quickgraph.codeplex.com/" /> in order to have
    ///     colored vertexes and edges.
    /// </summary>
    public class MyGraphvizVertex
    {
        #region Properties & Indexers

        public string BottomLabel { get; set; } = null;

        public string Comment { get; set; } = null;

        public double Distorsion { get; set; } = 0;

        public MyGraphvizColor FillColor { get; set; } = MyGraphvizColor.White;

        public bool FixedSize { get; set; } = false;

        public GraphvizFont Font { get; set; } = null;

        public MyGraphvizColor FontColor { get; set; } = MyGraphvizColor.Black;

        public string Group { get; set; } = null;

        public string Label { get; set; } = null;

        public GraphvizLayer Layer { get; set; } = null;

        public double Orientation { get; set; } = 0;

        public int Peripheries { get; set; } = -1;

        public GraphvizPoint Position { get; set; }

        public GraphvizRecord Record { get; set; } = new GraphvizRecord();

        public bool Regular { get; set; } = false;

        public GraphvizVertexShape Shape { get; set; } = GraphvizVertexShape.Unspecified;

        public int Sides { get; set; } = 4;

        public GraphvizSizeF Size { get; set; } = new GraphvizSizeF(0f, 0f);

        public double Skew { get; set; } = 0;

        public MyGraphvizColor StrokeColor { get; set; } = MyGraphvizColor.Black;

        public GraphvizVertexStyle Style { get; set; } = GraphvizVertexStyle.Unspecified;

        public string ToolTip { get; set; } = null;

        public string TopLabel { get; set; } = null;

        public string Url { get; set; } = null;

        public double Z { get; set; } = -1;

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.ToDot();
        }

        #endregion

        #region Public Methods

        public string ToDot()
        {
            var pairs = new Dictionary<string, object>();
            if (this.Font != null)
            {
                pairs["fontname"] = this.Font.Name;
                pairs["fontsize"] = this.Font.SizeInPoints;
            }
            if (!this.FontColor.Equals(MyGraphvizColor.Black))
            {
                pairs["fontcolor"] = this.FontColor;
            }
            if (this.Shape != GraphvizVertexShape.Unspecified)
            {
                pairs["shape"] = this.Shape;
            }
            if (this.Style != GraphvizVertexStyle.Unspecified)
            {
                pairs["style"] = this.Style;
            }
            if (this.Shape == GraphvizVertexShape.Record)
            {
                pairs["label"] = this.Record;
            }
            else if (this.Label != null)
            {
                pairs["label"] = this.Label;
            }
            if (this.FixedSize)
            {
                pairs["fixedsize"] = true;
                if (this.Size.Height > 0f)
                {
                    pairs["height"] = this.Size.Height;
                }
                if (this.Size.Width > 0f)
                {
                    pairs["width"] = this.Size.Width;
                }
            }
            if (!this.StrokeColor.Equals(MyGraphvizColor.Black))
            {
                pairs["color"] = this.StrokeColor;
            }
            if (!this.FillColor.Equals(MyGraphvizColor.White))
            {
                pairs["fillcolor"] = this.FillColor;
            }
            if (this.Regular)
            {
                pairs["regular"] = this.Regular;
            }
            if (this.Url != null)
            {
                pairs["URL"] = this.Url;
            }
            if (this.ToolTip != null)
            {
                pairs["tooltip"] = this.ToolTip;
            }
            if (this.Comment != null)
            {
                pairs["comment"] = this.Comment;
            }
            if (this.Group != null)
            {
                pairs["group"] = this.Group;
            }
            if (this.Layer != null)
            {
                pairs["layer"] = this.Layer.Name;
            }
            if (this.Orientation > 0)
            {
                pairs["orientation"] = this.Orientation;
            }
            if (this.Peripheries >= 0)
            {
                pairs["peripheries"] = this.Peripheries;
            }
            if (this.Z > 0)
            {
                pairs["z"] = this.Z;
            }
            if (this.Position != null)
            {
                pairs["pos"] = $"{this.Position.X},{this.Position.Y}!";
            }
            if (this.Style == GraphvizVertexStyle.Diagonals || this.Shape == GraphvizVertexShape.MCircle ||
                this.Shape == GraphvizVertexShape.MDiamond || this.Shape == GraphvizVertexShape.MSquare)
            {
                if (this.TopLabel != null)
                    pairs["toplabel"] = this.TopLabel;
                if (this.BottomLabel != null)
                    pairs["bottomlable"] = this.BottomLabel;
            }
            if (this.Shape == GraphvizVertexShape.Polygon)
            {
                if (this.Sides != 0)
                    pairs["sides"] = this.Sides;
                if (this.Skew != 0)
                    pairs["skew"] = this.Skew;
                if (this.Distorsion != 0)
                    pairs["distorsion"] = this.Distorsion;
            }

            return this.GenerateDot(pairs);
        }

        #endregion

        #region Internal Methods

        internal string GenerateDot(Dictionary<string, object> pairs)
        {
            var flag = false;
            var writer = new StringWriter();
            foreach (var entry in pairs)
            {
                if (flag)
                {
                    writer.Write(", ");
                }
                else
                {
                    flag = true;
                }
                if (entry.Value is string)
                {
                    writer.Write("{0}=\"{1}\"", entry.Key, entry.Value);
                    continue;
                }
                if (entry.Value is GraphvizVertexShape)
                {
                    writer.Write("{0}={1}", entry.Key, ((GraphvizVertexShape) entry.Value).ToString().ToLower());
                    continue;
                }
                if (entry.Value is GraphvizVertexStyle)
                {
                    writer.Write("{0}={1}", entry.Key, ((GraphvizVertexStyle) entry.Value).ToString().ToLower());
                    continue;
                }
                if (entry.Value is MyGraphvizColor)
                {
                    var color = (MyGraphvizColor) entry.Value;
                    writer.Write("{0}=\"#{1}{2}{3}{4}\"", entry.Key, color.R.ToString("x2").ToUpper(),
                        color.G.ToString("x2").ToUpper(), color.B.ToString("x2").ToUpper(),
                        color.A.ToString("x2").ToUpper());
                    continue;
                }
                if (entry.Value is GraphvizRecord)
                {
                    writer.WriteLine("{0}=\"{1}\"", entry.Key, ((GraphvizRecord) entry.Value).ToDot());
                    continue;
                }
                writer.Write(" {0}={1}", entry.Key, entry.Value.ToString().ToLower());
            }
            return writer.ToString();
        }

        #endregion
    }
}