// ------------------------------------------
// <copyright file="SymbolTreeSimilarity.cs" company="Pedro Sequeira">
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

using Genesis.Elements;
using Genesis.Trees;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Measures the similarity of two <see cref="ITreeProgram" /> based on the similarity of their
    ///     <see cref="SymbolTree{TProgram}" />.
    /// </summary>
    public class SymbolTreeSimilarity : ISimilarityMeasure<ITreeProgram>
    {
        #region Public Methods

        public double Calculate(ITreeProgram prog1, ITreeProgram prog2)
        {
            if (prog1 == null || prog2 == null) return 0;
            if (prog1.Equals(prog2)) return 1;

            var prog1SymbTree = new SymbolTree<ITreeProgram>();
            prog1SymbTree.AddProgram(prog1);
            var prog2SymbTree = new SymbolTree<ITreeProgram>();
            prog2SymbTree.AddProgram(prog2);
            return prog1SymbTree.GetSimilarity(prog2SymbTree);
        }

        #endregion
    }
}