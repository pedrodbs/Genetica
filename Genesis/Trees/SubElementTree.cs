// ------------------------------------------
// <copyright file="SubElementTree.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/06/05
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
    ///     Modified version of the <see cref="OrderedSymbolTree" /> data structure where nodes are created for each
    ///     sub-element of an added <see cref="IElement" />.
    /// </summary>
    public class SubElementTree : OrderedSymbolTree
    {
        #region Public Methods

        public override void AddElement(IElement element)
        {
            this.rootNode.Value++;
            var visited = new HashSet<SymbolNode>();

            // adds element
            AddElement(element, this.rootNode.Children[0], this.rootNode, visited);

            // adds all sub-elements of the given element
            foreach (var subElement in element.GetSubElements())
                AddElement(subElement, this.rootNode.Children[0], this.rootNode, visited);
        }

        #endregion
    }
}