// ------------------------------------------
// <copyright file="SubElementTree.cs" company="Pedro Sequeira">
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
//    Project: Genesis
//    Last updated: 03/23/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using Genesis.Elements;

namespace Genesis.Trees
{
    /// <summary>
    ///     Modified version of the <see cref="OrderedSymbolTree{TProgram}" /> data structure where nodes are created for each
    ///     sub-program of an added <see cref="ITreeProgram{TOutput}" />.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of output.</typeparam>
    public class SubProgramTree<TProgram, TOutput> : OrderedSymbolTree<TProgram> where TProgram : ITreeProgram<TOutput>
    {
        #region Public Methods

        public override void AddProgram(TProgram program)
        {
            this.rootNode.Value++;
            var visited = new HashSet<SymbolNode>();

            // adds program
            AddElement(program, this.rootNode.Children[0], this.rootNode, visited);

            // adds all sub-programs of the given program
            foreach (var subProgram in program.GetSubPrograms())
                AddElement((TProgram) subProgram, this.rootNode.Children[0], this.rootNode, visited);
        }

        #endregion
    }
}