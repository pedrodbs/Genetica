// ------------------------------------------
// <copyright file="ITreeExpressionConverter.cs" company="Pedro Sequeira">
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
//    Last updated: 04/02/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

namespace Genesis.Elements
{
    /// <summary>
    ///     Represents an interface for classes converting from <see cref="ITreeProgram" /> objects to <see cref="string" />
    ///     and vice-versa both in normal and prefix notation. In normal-form notation, nodes are written in the form
    ///     node-type(arg1 arg2 ...) or (arg1 node-type arg2). In prefix notation, nodes are written in the form (node-type
    ///     arg1 arg2 ...).
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public interface ITreeExpressionConverter<TProgram> : IExpressionConverter<TProgram>
        where TProgram : ITreeProgram
    {
        #region Public Methods

        /// <summary>
        ///     Converts the given <see cref="string" /> expression written in normal notation, i.e., where functions are written
        ///     in the form func(arg1 arg2 ...) or (arg1 func arg2) to a <typeparamref name="TProgram" />.
        /// </summary>
        /// <param name="expression">The expression we want to convert to an program.</param>
        /// <returns>A <typeparamref name="TProgram" /> converted from the given expression.</returns>
        TProgram FromNormalNotation(string expression);

        /// <summary>
        ///     Converts the given <see cref="string" /> expression written in prefix notation, i.e., where functions are written
        ///     in the form (func arg1 arg2 ...) to a <typeparamref name="TProgram" />.
        /// </summary>
        /// <param name="expression">The expression we want to convert to an program.</param>
        /// <returns>A <typeparamref name="TProgram" /> converted from the given expression.</returns>
        TProgram FromPrefixNotation(string expression);

        /// <summary>
        ///     Converts the given <typeparamref name="TProgram" /> to a <see cref="string" /> expression in normal notation, i.e.,
        ///     where functions are written in the form func(arg1 arg2 ...) or (arg1 func arg2).
        /// </summary>
        /// <param name="program">The program we want to convert to an expression.</param>
        /// <returns>A <see cref="string" /> representing the given program in prefix notation.</returns>
        string ToNormalNotation(TProgram program);

        /// <summary>
        ///     Converts the given <typeparamref name="TProgram" /> to a <see cref="string" /> expression in prefix notation, i.e.,
        ///     where functions are written in the form (func arg1 arg2 ...).
        /// </summary>
        /// <param name="program">The program we want to convert to an expression.</param>
        /// <param name="includeParentheses">
        ///     Whether to write opening '(' and closing ')' parentheses when writing the expression of functions.
        /// </param>
        /// <returns>A <see cref="string" /> representing the given program in prefix notation.</returns>
        string ToPrefixNotation(TProgram program, bool includeParentheses = true);

        #endregion
    }
}