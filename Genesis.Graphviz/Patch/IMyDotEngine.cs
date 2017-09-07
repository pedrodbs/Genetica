// ------------------------------------------
// <copyright file="IMyDotEngine.cs" company="Pedro Sequeira">
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
    /// <summary>
    ///     Override of the original QuickGraph code in <see href="https://quickgraph.codeplex.com/" /> in order to have
    ///     all the output formats supported by GraphViz.
    /// </summary>
    public interface IMyDotEngine
    {
        #region Public Methods

        string Run(MyGraphvizImageType imageType, string dot, string outputFileName);

        #endregion
    }
}