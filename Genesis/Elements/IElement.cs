// ------------------------------------------
// <copyright file="IElement.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/09/11
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;

namespace Genesis.Elements
{
    /// <summary>
    ///     An <see cref="IElement" /> represents a hierarchical or tree-based program, in which the descendants can themselves
    ///     be any kind of <see cref="IElement" />. Elements are characterized by some <see cref="Label" /> and can be
    ///     represented in string form by their <see cref="Expression" />.
    /// </summary>
    public interface IElement : IComparable<IElement>, ITreeNode
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the children of this element, i.e., the direct <see cref="IElement" /> descendants of this element.
        /// </summary>
        new IReadOnlyList<IElement> Children { get; }

        /// <summary>
        ///     Gets the element's program expression in normal form by combining the <see cref="Label" />s of all descendent
        ///     elements encountered starting from this element.
        /// </summary>
        string Expression { get; }

        /// <summary>
        ///     Gets the element's label, i.e., its representation independently of its descendant nodes.
        /// </summary>
        string Label { get; }

        /// <summary>
        ///     Gets the element's length, i.e., the total number of <see cref="IElement" /> descendant elements encountered
        ///     starting from this element.
        /// </summary>
        ushort Length { get; }

        #endregion

        #region Public methods

        /// <summary>
        ///     Creates a new <see cref="IElement" /> of the same kind of this element with the given child elements.
        /// </summary>
        /// <param name="children">The children of the new element.</param>
        /// <returns>A new <see cref="IElement" /> of the same kind of this element with the given child elements.</returns>
        IElement CreateNew(IList<IElement> children);

        /// <summary>
        ///     Gets the value resulting from the execution of this program.
        /// </summary>
        /// <returns>The value resulting from the execution of this program.</returns>
        double GetValue();

        #endregion
    }
}