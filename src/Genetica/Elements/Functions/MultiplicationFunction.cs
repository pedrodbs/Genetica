// ------------------------------------------
// <copyright file="MultiplicationFunction.cs" company="Pedro Sequeira">
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
//    Last updated: 04/04/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using Genetica.Elements.Terminals;

namespace Genetica.Elements.Functions
{
    /// <summary>
    ///     Represents a <see cref="CommutativeBinaryFunction" /> performing the multiplication of two sub-programs.
    /// </summary>
    public class MultiplicationFunction : CommutativeBinaryFunction
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="MultiplicationFunction" />.
        /// </summary>
        /// <param name="firstProgram">The first sub-program.</param>
        /// <param name="secondProgram">The second sub-program.</param>
        public MultiplicationFunction(ITreeProgram<double> firstProgram, ITreeProgram<double> secondProgram) :
            base(firstProgram, secondProgram)
        {
            this.Expression = $"({this.FirstParameter.Expression}{this.Label}{this.SecondParameter.Expression})";
        }

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public override string Expression { get; }

        /// <inheritdoc />
        public override string Label => "*";

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double Compute() => this.FirstParameter.Compute() * this.SecondParameter.Compute();

        /// <inheritdoc />
        public override ITreeProgram<double> CreateNew(IList<ITreeProgram<double>> children) =>
            children == null || children.Count != 2
                ? null
                : new MultiplicationFunction(children[0], children[1]);

        /// <inheritdoc />
        public override ITreeProgram<double> Simplify()
        {
            // if its a constant value, just return a constant with that value
            if (this.IsConstant())
                return new Constant(this.Compute());

            // otherwise first tries to simplify children
            var input = new ITreeProgram<double>[this.Input.Count];
            for (var i = 0; i < this.Input.Count; i++)
                input[i] = this.Input[i].Simplify();

            //check whether one operand is 1 and return the other
            if (input[0].EqualsConstant(1))
                return input[1];
            if (input[1].EqualsConstant(1))
                return input[0];

            //check whether one of operands is 0 and return 0
            if (input[0].EqualsConstant(0) || input[1].EqualsConstant(0))
                return Constant.Zero;

            ////check whether operands are the same and return square
            //if (input[0].Equals(input[1]))
            //    return new PowerFunction(input[0], new Constant(2));

            // check whether there's a ((x^a)*x) situation, returns (x^(a+1))
            if (input[0] is PowerFunction && input[0].Input[1] is Constant &&
                input[0].Input[0].Equals(input[1]))
                return new PowerFunction(input[1], new Constant(input[0].Input[1].Compute() + 1));
            if (input[1] is PowerFunction && input[1].Input[1] is Constant &&
                input[1].Input[0].Equals(input[0]))
                return new PowerFunction(input[0], new Constant(input[1].Input[1].Compute() + 1));

            return this.CreateNew(input);
        }

        #endregion
    }
}