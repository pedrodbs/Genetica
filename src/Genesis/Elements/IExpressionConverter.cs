// ------------------------------------------
// <copyright file="IExpressionConverter.cs" company="Pedro Sequeira">
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

namespace Genesis.Elements
{
    /// <summary>
    ///     Represents an interface for classes converting from <see cref="IProgram" /> objects to <see cref="string" /> and
    ///     vice-versa.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public interface IExpressionConverter<TProgram> where TProgram : IProgram
    {
        #region Public Methods

        /// <summary>
        ///     Converts the given <see cref="string" /> to a program of type <see cref="TProgram" />.
        /// </summary>
        /// <param name="programStr">The string representing some program. </param>
        /// <returns>The program corresponding to the given string representation.</returns>
        TProgram FromString(string programStr);

        /// <summary>
        ///     Converts the given <see cref="TProgram" /> to a string.
        /// </summary>
        /// <param name="program">The program we want to convert.</param>
        /// <returns>The string representing the given program.</returns>
        string ToString(TProgram program);

        #endregion
    }
}