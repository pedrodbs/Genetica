// ------------------------------------------
// <copyright file="IInformationTree.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;
using Genesis.Elements;

namespace Genesis.Trees
{
    /// <summary>
    ///     Represents an interface for trees representing information about a set of <see cref="IElement" />s.
    ///     Usually an <see cref="IInformationTree" /> represents the "average" semantic and/or syntactical structure of the
    ///     set of <see cref="IElement" /> considered.
    /// </summary>
    public interface IInformationTree
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the root node of this information tree.
        /// </summary>
        IInformationTreeNode RootNode { get; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Adds an element to this information tree.
        /// </summary>
        /// <param name="element">The element to be added to the tree.</param>
        void AddElement(IElement element);

        /// <summary>
        ///     Adds a collection of elements to this information tree.
        /// </summary>
        /// <param name="elements">The elements to be added to the tree.</param>
        void AddElements(IEnumerable<IElement> elements);

        /// <summary>
        ///     Clears the tree by removing all information collected from the added elements.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Gets the number of node positions sampled in the tree search space, i.e., the structure-unique node count.
        /// </summary>
        /// <returns>The structure-unique node count.</returns>
        uint GetCount();

        /// <summary>
        ///     Gets the total number of terminal nodes (genetic nodes) of the elements added to the tree.
        /// </summary>
        /// <returns>The node count.</returns>
        uint GetNodeCount();

        /// <summary>
        ///     Prunes the tree by removing all <see cref="ITreeNode" /> stored in this tree whose value falls below the given
        ///     threshold.
        /// </summary>
        /// <param name="valueThreshold">The threshold value used to prune the <see cref="ITreeNode" />s. </param>
        void Prune(double valueThreshold);

        #endregion
    }
}