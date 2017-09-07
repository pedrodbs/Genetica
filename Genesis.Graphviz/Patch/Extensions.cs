// ------------------------------------------
// <copyright file="Extensions.cs" company="Pedro Sequeira">
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

namespace Genesis.Graphviz.Patch
{
    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        ///     Gets the output format string of the given <see cref="MyGraphvizImageType" /> to be used for GraphViz command-line
        ///     conversion.
        /// </summary>
        /// <param name="imageType">The image type we want to get the output format.</param>
        /// <returns>
        ///     The output format string of the given <see cref="MyGraphvizImageType" /> to be used for GraphViz command-line
        ///     conversion.
        /// </returns>
        public static string GetOutputFormatStr(this MyGraphvizImageType imageType)
        {
            switch (imageType)
            {
                case MyGraphvizImageType.Fig:
                    return "fig";
                case MyGraphvizImageType.Gd:
                    return "gd";
                case MyGraphvizImageType.Gd2:
                    return "gd2";
                case MyGraphvizImageType.Gif:
                    return "gif";
                case MyGraphvizImageType.Hpgl:
                    return "hpgl";
                case MyGraphvizImageType.Imap:
                    return "imap";
                case MyGraphvizImageType.Cmap:
                    return "cmap";
                case MyGraphvizImageType.Jpeg:
                    return "jpeg";
                case MyGraphvizImageType.Mif:
                    return "mif";
                case MyGraphvizImageType.Mp:
                    return "mp";
                case MyGraphvizImageType.Pcl:
                    return "pcl";
                case MyGraphvizImageType.Pic:
                    return "pic";
                case MyGraphvizImageType.PlainText:
                    return "plaintext";
                case MyGraphvizImageType.Png:
                    return "png";
                case MyGraphvizImageType.Ps:
                    return "ps";
                case MyGraphvizImageType.Ps2:
                    return "ps2";
                case MyGraphvizImageType.Svgz:
                    return "svgz";
                case MyGraphvizImageType.Vrml:
                    return "vrml";
                case MyGraphvizImageType.Vtx:
                    return "vtx";
                case MyGraphvizImageType.Wbmp:
                    return "wbmp";
                case MyGraphvizImageType.Bmp:
                    return "bmp";
                case MyGraphvizImageType.Canon:
                    return "fig";
                case MyGraphvizImageType.Gv:
                    return "gv";
                case MyGraphvizImageType.Xdot:
                    return "xdot";
                case MyGraphvizImageType.Xdot12:
                    return "xdot1.2";
                case MyGraphvizImageType.Xdot14:
                    return "dot1.4";
                case MyGraphvizImageType.Cgimage:
                    return "cgimage";
                case MyGraphvizImageType.Eps:
                    return "eps";
                case MyGraphvizImageType.Exr:
                    return "exr";
                case MyGraphvizImageType.Gtk:
                    return "gtk";
                case MyGraphvizImageType.Ico:
                    return "ico";
                case MyGraphvizImageType.Cmapx:
                    return "cmapx";
                case MyGraphvizImageType.ImapNp:
                    return "imap_np";
                case MyGraphvizImageType.CmapxNp:
                    return "cmapx_np";
                case MyGraphvizImageType.Ismap:
                    return "ismap";
                case MyGraphvizImageType.Jp2:
                    return "jp2";
                case MyGraphvizImageType.Jpg:
                    return "jpg";
                case MyGraphvizImageType.Jpe:
                    return "jpe";
                case MyGraphvizImageType.Json:
                    return "json";
                case MyGraphvizImageType.Json0:
                    return "json0";
                case MyGraphvizImageType.DotJson:
                    return "dot_json";
                case MyGraphvizImageType.XdotJson:
                    return "xdot_json";
                case MyGraphvizImageType.Pict:
                    return "pict";
                case MyGraphvizImageType.Pct:
                    return "pct";
                case MyGraphvizImageType.Pdf:
                    return "pdf";
                case MyGraphvizImageType.Plain:
                    return "plain";
                case MyGraphvizImageType.PlainExt:
                    return "plain-ext";
                case MyGraphvizImageType.Pov:
                    return "pov";
                case MyGraphvizImageType.Psd:
                    return "psd";
                case MyGraphvizImageType.Sgi:
                    return "sgi";
                case MyGraphvizImageType.Tga:
                    return "tga";
                case MyGraphvizImageType.Tif:
                    return "tif";
                case MyGraphvizImageType.Tiff:
                    return "tiff";
                case MyGraphvizImageType.Tk:
                    return "tk";
                case MyGraphvizImageType.Vml:
                    return "vml";
                case MyGraphvizImageType.Vmlz:
                    return "vmlz";
                case MyGraphvizImageType.Webp:
                    return "webp";
                case MyGraphvizImageType.Xlib:
                    return "xlib";
                case MyGraphvizImageType.X11:
                    return "x11";
                case MyGraphvizImageType.Svg:
                default:
                    return "svg";
            }
        }

        #endregion
    }
}