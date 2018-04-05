// ------------------------------------------
// <copyright file="IProgram.cs" company="Pedro Sequeira">
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
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

namespace Genetica.Elements
{
    /// <summary>
    ///     Represents an interface for a genetic program.
    /// </summary>
    public interface IProgram
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets a string representing this program's expression.
        /// </summary>
        string Expression { get; }

        /// <summary>
        ///     Gets the program's length.
        /// </summary>
        ushort Length { get; }

        #endregion
    }

    /// <summary>
    ///     Represents an interface for a genetic program that has some input of type <typeparamref name="TInput" /> and output
    ///     of type <typeparamref name="TOutput" />.
    /// </summary>
    /// <typeparam name="TInput">The type of program input.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public interface IProgram<out TInput, out TOutput> : IProgram
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the input of this program.
        /// </summary>
        TInput Input { get; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Computes the output of the program based on it's <see cref="Input" />.
        /// </summary>
        /// <returns>A <typeparamref name="TOutput" /> value corresponding to this program's computed output.</returns>
        TOutput Compute();

        #endregion
    }
}