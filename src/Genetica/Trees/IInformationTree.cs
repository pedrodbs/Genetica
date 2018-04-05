// ------------------------------------------
// <copyright file="IInformationTree.cs" company="Pedro Sequeira">
// 
//     Copyright (c) 2018 Pedro Sequeira
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </copyright>
// <summary>
//    Project: Genetica
//    Last updated: 03/23/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using Genetica.Elements;

namespace Genetica.Trees
{
    /// <summary>
    ///     Represents an interface for trees representing information about a collection of <see cref="ITreeProgram" />s.
    ///     Usually an <see cref="IInformationTree{TProgram}" /> represents the "average" semantic and/or syntactical structure
    ///     of the set of <see cref="ITreeNode" /> considered.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public interface IInformationTree<in TProgram> where TProgram : ITreeProgram
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the root node of this information tree.
        /// </summary>
        IInformationTreeNode RootNode { get; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Adds an program to this information tree.
        /// </summary>
        /// <param name="program">The program to be added to the tree.</param>
        void AddProgram(TProgram program);

        /// <summary>
        ///     Adds a collection of programs to this information tree.
        /// </summary>
        /// <param name="programs">The programs to be added to the tree.</param>
        void AddPrograms(IEnumerable<TProgram> programs);

        /// <summary>
        ///     Clears the tree by removing all information collected from the added programs.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Gets the number of node positions sampled in the tree search space, i.e., the structure-unique node count.
        /// </summary>
        /// <returns>The structure-unique node count.</returns>
        uint GetCount();

        /// <summary>
        ///     Gets the total number of terminal nodes (genetic nodes) of the programs added to the tree.
        /// </summary>
        /// <returns>The node count.</returns>
        uint GetNodeCount();

        /// <summary>
        ///     Prunes the tree by removing all nodes stored in this tree whose value falls below the given threshold.
        /// </summary>
        /// <param name="valueThreshold">The threshold value used to prune the nodes. </param>
        void Prune(double valueThreshold);

        #endregion
    }
}