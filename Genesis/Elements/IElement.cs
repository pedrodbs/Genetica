// ------------------------------------------
// <copyright file="IElement.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/01/19
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;

namespace Genesis.Elements
{
    public interface IElement : IComparable<IElement>
    {
        #region Properties & Indexers

        IReadOnlyList<IElement> Children { get; }

        /// <summary>
        ///     Gets the element count, i.e., the total number of <see cref="IElement" /> descendant elements encountered
        ///     starting from this element.
        /// </summary>
        ushort Count { get; }

        string Expression { get; }

        string Label { get; }

        #endregion

        #region Public methods

        IElement Clone();

        IElement CreateNew(IList<IElement> children);

        double GetValue();

        #endregion
    }
}