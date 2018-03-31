// ------------------------------------------
// <copyright file="SimplificationExtensions.cs" company="Pedro Sequeira">
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
//    Last updated: 03/27/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;

namespace Genesis.Elements
{
    /// <summary>
    ///     Contains several methods to simplify expressions of <see cref="MathProgram" />.
    /// </summary>
    public static class SimplificationExtensions
    {
        #region Public Methods

        public static IProgram Simplify(this IProgram program)
        {
            //todo remove this
            return Simplify(program as ITreeProgram<double>);
        }

        /// <summary>
        ///     Simplifies the expression of the given program by returning a new <see cref="MathProgram" /> which value will
        ///     always be equal to the original program, i.e. it removes redundant operations, e.g. subtrees with functions
        ///     that will always result in the value of one of its operands.
        /// </summary>
        /// <returns>An program corresponding to a simplification of the given program.</returns>
        /// <param name="program">The program we want to simplify.</param>
        public static ITreeProgram<double> Simplify(this ITreeProgram<double> program)
        {
            //todo maybe put simplification as interface member of IElement
            // each class would then implement their simplification
            //todo add simplification for x*x*x*x*x = x^5

            if (program == null) return null;

            // if its a terminal, just return the program, no more simplifications possible
            if (program.Input == null || program.Input.Count == 0)
                return program;

            // if its a constant value, just return a constant with that value
            if (program.IsConstant())
                return new Constant(program.Compute());

            // otherwise first tries to simplify children
            var input = new ITreeProgram<double>[program.Input.Count];
            for (var i = 0; i < program.Input.Count; i++)
                input[i] = program.Input[i].Simplify();

            // if its an addition
            if (program is AdditionFunction)

                //check whether one operand is 0 and return the other
                if (input[0].EqualsConstant(0))
                    return input[1];
                else if (input[1].EqualsConstant(0))
                    return input[0];

                // check whether operands are the same and return 2x
                else if (input[0].Equals(input[1]))
                    return new MultiplicationFunction(input[0], new Constant(2));

                // check whether there's a ((a*x)+x) situation, returns ((a+1)*x)
                else if (input[0] is MultiplicationFunction)
                {
                    if (input[0].Input[0] is Constant && input[0].Input[1].Equals(input[1]))
                        return new MultiplicationFunction(new Constant(input[0].Input[0].Compute() + 1), input[1]);
                    if (input[0].Input[1] is Constant && input[0].Input[0].Equals(input[1]))
                        return new MultiplicationFunction(new Constant(input[0].Input[1].Compute() + 1), input[1]);
                }
                else if (input[1] is MultiplicationFunction)
                {
                    if (input[1].Input[0] is Constant && input[1].Input[1].Equals(input[0]))
                        return new MultiplicationFunction(new Constant(input[1].Input[0].Compute() + 1), input[0]);
                    if (input[1].Input[1] is Constant && input[1].Input[0].Equals(input[0]))
                        return new MultiplicationFunction(new Constant(input[1].Input[1].Compute() + 1), input[0]);
                }

            // if its a subtraction
            if (program is SubtractionFunction)

                // if the operands are equal, return 0
                if (input[0].Equals(input[1]))
                    return Constant.Zero;

                // check whether second operand is 0 and return the first
                else if (input[1].EqualsConstant(0))
                    return input[0];

            // if its a multiplication
            if (program is MultiplicationFunction)

                //check whether one operand is 1 and return the other
                if (input[0].EqualsConstant(1))
                    return input[1];
                else if (input[1].EqualsConstant(1))
                    return input[0];

                //check whether one of operands is 0 and return 0
                else if (input[0].EqualsConstant(0) || input[1].EqualsConstant(0))
                    return Constant.Zero;

                //check whether operands are the same and return square
                else if (input[0].Equals(input[1]))
                    return new PowerFunction(input[0], new Constant(2));

                // check whether there's a ((x^a)*x) situation, returns (x^(a+1))
                else if (input[0] is PowerFunction && input[0].Input[1] is Constant &&
                         input[0].Input[0].Equals(input[1]))
                    return new PowerFunction(input[1], new Constant(input[0].Input[1].Compute() + 1));
                else if (input[1] is PowerFunction && input[1].Input[1] is Constant &&
                         input[1].Input[0].Equals(input[0]))
                    return new PowerFunction(input[0], new Constant(input[1].Input[1].Compute() + 1));

            // if its a division or power, check whether second operand is 1 and return the first
            if ((program is DivisionFunction || program is PowerFunction) && input[1].EqualsConstant(1))
                return input[0];

            // if its a division
            if (program is DivisionFunction)

                //if operands are equal, return 1
                if (input[0].Equals(input[1]))
                    return Constant.One;

                //if first operand is 0 return 0
                else if (input[0].EqualsConstant(0))
                    return Constant.Zero;

            // if its a min or max, check whether operands are equal and return the first
            if ((program is MaxFunction || program is MinFunction) && input[0].Equals(input[1]))
                return input[0];

            // if its a max check whether one of operands is minimum and return the other
            if (program is MaxFunction && (input[0].EqualsConstant(double.MinValue) ||
                                           input[0].EqualsConstant(double.NegativeInfinity)))
                return input[1];
            if (program is MaxFunction && (input[1].EqualsConstant(double.MinValue) ||
                                           input[1].EqualsConstant(double.NegativeInfinity)))
                return input[0];

            // if its a min check whether one of operands is maximum and return the other
            if (program is MinFunction && (input[0].EqualsConstant(double.MaxValue) ||
                                           input[0].EqualsConstant(double.PositiveInfinity)))
                return input[1];
            if (program is MinFunction && (input[1].EqualsConstant(double.MaxValue) ||
                                           input[1].EqualsConstant(double.PositiveInfinity)))
                return input[0];

            // if its an if clause
            if (program is IfFunction)
            {
                //  check whether the first child is a constant and returns one of the other children accordingly
                var child1 = input[0];
                if (child1.IsConstant())
                {
                    var val = child1.Compute();
                    return val.Equals(0) ? input[1] : (val > 0 ? input[2] : input[3]);
                }

                // check whether first child is a variable, check its range
                if (child1 is Variable)
                {
                    var range = ((Variable) child1).Range;
                    if (range.Min.Equals(0) && range.Max.Equals(0)) return input[1];
                    if (range.Min > 0) return input[2];
                    if (range.Max < 0) return input[3];
                }

                // check whether result children are equal, in which case replace by one of them
                if (input[1].Equals(input[2]) && input[1].Equals(input[3]))
                    return input[1];
            }

            return program.CreateNew(input);
        }

        #endregion
    }
}