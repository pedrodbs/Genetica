// ------------------------------------------
// <copyright file="IInfoTreeNode.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/06/01
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

namespace Genesis.Trees
{
    /// <summary>
    ///     Specialization of <see cref="ITreeNode" /> for information trees.
    /// </summary>
    public interface IInformationTreeNode : ITreeNode
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the root node of tree in which this node is in.
        /// </summary>
        IInformationTreeNode RootNode { get; }

        /// <summary>
        ///     Gets or sets the value associated with the node.
        /// </summary>
        uint Value { get; set; }

        #endregion
    }
}