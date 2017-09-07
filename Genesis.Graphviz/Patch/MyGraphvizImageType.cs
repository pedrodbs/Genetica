// ------------------------------------------
// <copyright file="MyGraphvizImageType.cs" company="Pedro Sequeira">
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

using System.ComponentModel;

namespace Genesis.Graphviz.Patch
{
    /// <summary>
    ///     Override of the original QuickGraph code in <see href="https://quickgraph.codeplex.com/" /> in order to have
    ///     all the output formats supported by GraphViz as in <see href="http://www.graphviz.org/doc/info/output.html" />.
    /// </summary>
    public enum MyGraphvizImageType
    {
        [Description("Figure format")] Fig = 0,
        [Description("Gd format")] Gd = 1,
        [Description("Gd2 format")] Gd2 = 2,
        [Description("GIF format")] Gif = 3,
        [Description("HP-GL/2 format")] Hpgl = 4,
        [Description("Server-side imagemaps")] Imap = 5,
        [Description("Client-side imagemaps Format (deprecated)")] Cmap = 6,
        [Description("JPEG format")] Jpeg = 7,
        [Description("FrameMaker MIF format")] Mif = 8,
        [Description("MetaPost")] Mp = 9,
        [Description("PCL format")] Pcl = 10,
        [Description("Kernighan's PIC Graphics Language Format")] Pic = 11,
        [Description("Plain Text format")] PlainText = 12,
        [Description("Portable Network Graphics format")] Png = 13,
        [Description("Postscript")] Ps = 14,
        [Description("PostScript for PDF")] Ps2 = 15,
        [Description("Scalable Vector Graphics")] Svg = 0x10,
        [Description("Scalable Vector Graphics, gzipped")] Svgz = 0x11,
        [Description("VRML")] Vrml = 0x12,
        [Description("Visual Thought format")] Vtx = 0x13,
        [Description("Wireless BitMap format")] Wbmp = 20,
        [Description("Windows Bitmap Format")] Bmp = 21,
        [Description("DOT Format")] Canon = 22,
        [Description("DOT Format")] Gv = 23,
        [Description("DOT Format")] Xdot = 24,
        [Description("DOT Format")] Xdot12 = 25,
        [Description("DOT Format")] Xdot14 = 26,
        [Description("CGImage bitmap Format")] Cgimage = 27,
        [Description("Encapsulated PostScript Format")] Eps = 28,
        [Description("OpenEXR Format")] Exr = 29,
        [Description("GTK Canvas Format")] Gtk = 30,
        [Description("Icon Image File Format")] Ico = 31,
        [Description("Client-side imagemaps Format")] Cmapx = 32,
        [Description("Server-side imagemaps Format")] ImapNp = 33,
        [Description("Client-side imagemaps Format")] CmapxNp = 34,
        [Description("Server-side imagemaps Format (deprecated)")] Ismap = 35,
        [Description("JPEG 2000 Format")] Jp2 = 36,
        [Description("JPEG Format")] Jpg = 37,
        [Description("JPEG Format")] Jpe = 38,
        [Description("Dot Graph Represented in JSON Format")] Json = 39,
        [Description("Dot Graph Represented in JSON Format")] Json0 = 40,
        [Description("Dot Graph Represented in JSON Format")] DotJson = 41,
        [Description("Dot Graph Represented in JSON Format")] XdotJson = 42,
        [Description("PICT Format")] Pict = 43,
        [Description("PICT Format")] Pct = 44,
        [Description("Portable Document Format (PDF) Format")] Pdf = 45,
        [Description("Plain-text Format")] Plain = 46,
        [Description("Plain-text Format")] PlainExt = 47,
        [Description("POV-Ray Markup Language (prototype) Format")] Pov = 48,
        [Description("PSD Format")] Psd = 49,
        [Description("SGI Format")] Sgi = 50,
        [Description("Truevision TGA Format")] Tga = 51,
        [Description("Tag Image File Format (TIFF) Format")] Tif = 52,
        [Description("Tag Image File Format (TIFF) Format")] Tiff = 53,
        [Description("TK Graphics Format")] Tk = 54,
        [Description("Vector Markup Language (VML) Format")] Vml = 55,
        [Description("Vector Markup Language (VML) Format")] Vmlz = 56,
        [Description("Image format for the Web Format")] Webp = 57,
        [Description("Xlib Canvas Format")] Xlib = 58,
        [Description("Xlib Canvas Format")] X11 = 59
    }
}