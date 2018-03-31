// ------------------------------------------
// <copyright file="MathPrimitiveSets.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;
using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;

namespace Genesis.Elements
{
    /// <summary>
    ///     Contains different <see cref="PrimitiveSet{TProgram}" /> for <see cref="MathProgram" />.
    /// </summary>
    public static class MathPrimitiveSets
    {
        #region Static Fields & Constants

        /// <summary>
        ///     Gets the default <see cref="PrimitiveSet{TProgram}" /> for <see cref="MathProgram" /> containing
        ///     <see cref="Constant.Zero" /> and <see cref="Constant.One" /> as the terminals and all available mathematical
        ///     functions.
        /// </summary>
        public static readonly PrimitiveSet<MathProgram> Default = new PrimitiveSet<MathProgram>(
            new HashSet<MathProgram> {Constant.Zero, Constant.One},
            new HashSet<MathProgram>
            {
                new AdditionFunction(Constant.Zero, Constant.Zero),
                new CosineFunction(Constant.Zero),
                new DivisionFunction(Constant.Zero, Constant.Zero),
                new IfFunction(Constant.Zero, Constant.Zero, Constant.Zero, Constant.Zero),
                new LogarithmFunction(Constant.Zero, Constant.Zero),
                new MaxFunction(Constant.Zero, Constant.Zero),
                new MinFunction(Constant.Zero, Constant.Zero),
                new MultiplicationFunction(Constant.Zero, Constant.Zero),
                new PowerFunction(Constant.Zero, Constant.Zero),
                new SineFunction(Constant.Zero),
                new SubtractionFunction(Constant.Zero, Constant.Zero)
            });

        #endregion
    }
}