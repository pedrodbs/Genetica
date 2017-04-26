// ------------------------------------------
// <copyright file="ITreeNode.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/03/29
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;

namespace Genesis
{
    public interface ITreeNode
    {
        #region Properties & Indexers

        IReadOnlyList<ITreeNode> Children { get; }

        #endregion
    }
}