// ------------------------------------------
// <copyright file="CommutativeBinaryFunction.cs" company="Pedro Sequeira">
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
//    Last updated: 03/26/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

namespace Genesis.Elements.Functions
{
    /// <summary>
    ///     Represents a <see cref="BinaryFunction" /> in which the order of the operands is not important.
    ///     Therefore they are sorted to avoid creating repeated functions.
    /// </summary>
    public abstract class CommutativeBinaryFunction : BinaryFunction, ICommutativeTreeProgram<double>
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="CommutativeBinaryFunction" /> with the given programs as parameters.
        /// </summary>
        /// <param name="firstProgram">The first parameter of the program.</param>
        /// <param name="secondProgram">The second parameter of the program.</param>
        protected CommutativeBinaryFunction(ITreeProgram<double> firstProgram, ITreeProgram<double> secondProgram) :
            base(firstProgram.CompareTo(secondProgram) >= 0 ? firstProgram : secondProgram,
                firstProgram.CompareTo(secondProgram) >= 0 ? secondProgram : firstProgram)
        {
        }

        #endregion
    }
}