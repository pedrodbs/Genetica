// ------------------------------------------
// <copyright file="MyGraphvizEdge.cs" company="Pedro Sequeira">
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
using System.Collections.Generic;
using System.IO;
using QuickGraph.Graphviz.Dot;

namespace Genesis.Graphviz.Patch
{
    /// <summary>
    ///     Override of the original QuickGraph code in <see href="https://quickgraph.codeplex.com/" /> in order to have
    ///     colored vertexes and edges.
    /// </summary>
    public class MyGraphvizEdge
    {
        #region Properties & Indexers

        public string Comment { get; set; } = null;

        public GraphvizEdgeDirection Dir { get; set; } = GraphvizEdgeDirection.Forward;

        public GraphvizFont Font { get; set; } = null;

        public MyGraphvizColor FontColor { get; set; } = MyGraphvizColor.Black;

        public GraphvizEdgeExtremity Head { get; set; } = new GraphvizEdgeExtremity(true);

        public GraphvizArrow HeadArrow { get; set; } = null;

        public string HeadPort { get; set; }

        public bool IsConstrained { get; set; } = true;

        public bool IsDecorated { get; set; } = false;

        public GraphvizEdgeLabel Label { get; set; } = new GraphvizEdgeLabel();

        public GraphvizLayer Layer { get; set; } = null;

        public int Length { get; set; } = 1;

        public int MinLength { get; set; } = 1;

        public MyGraphvizColor StrokeColor { get; set; } = MyGraphvizColor.Black;

        public GraphvizEdgeStyle Style { get; set; } = GraphvizEdgeStyle.Unspecified;

        public GraphvizEdgeExtremity Tail { get; set; } = new GraphvizEdgeExtremity(false);

        public GraphvizArrow TailArrow { get; set; } = null;

        public string TailPort { get; set; }

        public string ToolTip { get; set; } = null;

        public string Url { get; set; } = null;

        public double Weight { get; set; } = 1;

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
            var dic = new Dictionary<string, object>(StringComparer.Ordinal);
            if (this.Comment != null)
            {
                dic["comment"] = this.Comment;
            }
            if (this.Dir != GraphvizEdgeDirection.Forward)
            {
                dic["dir"] = this.Dir.ToString().ToLower();
            }
            if (this.Font != null)
            {
                dic["fontname"] = this.Font.Name;
                dic["fontsize"] = this.Font.SizeInPoints;
            }
            if (!this.FontColor.Equals(MyGraphvizColor.Black))
            {
                dic["fontcolor"] = this.FontColor;
            }
            this.Head.AddParameters(dic);
            if (this.HeadArrow != null)
            {
                dic["arrowhead"] = this.HeadArrow.ToDot();
            }
            if (!this.IsConstrained)
            {
                dic["constraint"] = this.IsConstrained;
            }
            if (this.IsDecorated)
            {
                dic["decorate"] = this.IsDecorated;
            }
            this.Label.AddParameters(dic);
            if (this.Layer != null)
            {
                dic["layer"] = this.Layer.Name;
            }
            if (this.MinLength != 1)
            {
                dic["minlen"] = this.MinLength;
            }
            if (!this.StrokeColor.Equals(MyGraphvizColor.Black))
            {
                dic["color"] = this.StrokeColor;
            }
            if (this.Style != GraphvizEdgeStyle.Unspecified)
            {
                dic["style"] = this.Style.ToString().ToLower();
            }
            this.Tail.AddParameters(dic);
            if (this.TailArrow != null)
            {
                dic["arrowtail"] = this.TailArrow.ToDot();
            }
            if (this.ToolTip != null)
            {
                dic["tooltip"] = this.ToolTip;
            }
            if (this.Url != null)
            {
                dic["URL"] = this.Url;
            }
            if (this.Weight != 1)
            {
                dic["weight"] = this.Weight;
            }
            if (this.HeadPort != null)
                dic["headport"] = this.HeadPort;
            if (this.TailPort != null)
                dic["tailport"] = this.TailPort;
            if (this.Length != 1)
                dic["len"] = this.Length;
            return this.GenerateDot(dic);
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
                if (entry.Value is GraphvizEdgeDirection)
                {
                    writer.Write("{0}={1}", entry.Key, ((GraphvizEdgeDirection) entry.Value).ToString().ToLower());
                    continue;
                }
                if (entry.Value is GraphvizEdgeStyle)
                {
                    writer.Write("{0}={1}", entry.Key, ((GraphvizEdgeStyle) entry.Value).ToString().ToLower());
                    continue;
                }
                if (entry.Value is MyGraphvizColor)
                {
                    var graphvizColor = (MyGraphvizColor) entry.Value;
                    writer.Write("{0}=\"#{1}{2}{3}{4}\"", entry.Key, graphvizColor.R.ToString("x2").ToUpper(),
                        graphvizColor.G.ToString("x2").ToUpper(), graphvizColor.B.ToString("x2").ToUpper(),
                        graphvizColor.A.ToString("x2").ToUpper());
                    continue;
                }
                writer.Write(" {0}={1}", entry.Key, entry.Value.ToString().ToLower());
            }
            return writer.ToString();
        }

        #endregion
    }
}