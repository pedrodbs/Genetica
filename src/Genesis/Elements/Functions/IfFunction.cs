// ------------------------------------------
// <copyright file="IfFunction.cs" company="Pedro Sequeira">
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

namespace Genesis.Elements.Functions
{
    /// <summary>
    ///     Represents a <see cref="MathProgram" /> performing a conditional operation according to the value of the first
    ///     sub-program.
    /// </summary>
    public class IfFunction : MathProgram
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="IfFunction" /> with the given sub-programs.
        /// </summary>
        /// <param name="conditionProgram">The sub-program computing the conditional value.</param>
        /// <param name="zeroProgram">The sub-program computing the value if the conditional is 0.</param>
        /// <param name="positiveProgram">The sub-program computing the value if the conditional is &gt;0</param>
        /// <param name="negativeProgram">The sub-program computing the value if the conditional is &lt;0</param>
        public IfFunction(
            ITreeProgram<double> conditionProgram, ITreeProgram<double> zeroProgram,
            ITreeProgram<double> positiveProgram, ITreeProgram<double> negativeProgram)
            : base(new[] {conditionProgram, zeroProgram, positiveProgram, negativeProgram})
        {
            this.Expression = $"({this.ConditionProgram.Expression}?{this.ZeroProgram.Expression}:" +
                              $"{this.PositiveProgram.Expression}:{this.NegativeProgram.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public override string Expression { get; }

        public override string Label => "if";

        /// <summary>
        ///     Gets the sub-program computing the conditional value.
        /// </summary>
        public ITreeProgram<double> ConditionProgram => this.Input[0];

        /// <summary>
        ///     Gets the sub-program computing the value if the conditional is less than 0.
        /// </summary>
        public ITreeProgram<double> NegativeProgram => this.Input[3];

        /// <summary>
        ///     Gets the sub-program computing the value if the conditional is greater than 0
        /// </summary>
        public ITreeProgram<double> PositiveProgram => this.Input[2];

        /// <summary>
        ///     Gets the sub-program computing the value if the conditional is equal to 0.
        /// </summary>
        public ITreeProgram<double> ZeroProgram => this.Input[1];

        #endregion

        #region Public Methods

        public override double Compute()
        {
            var val = this.ConditionProgram.Compute();
            return val.Equals(0)
                ? this.ZeroProgram.Compute()
                : (val > 0
                    ? this.PositiveProgram.Compute()
                    : this.NegativeProgram.Compute());
        }

        public override ITreeProgram<double> CreateNew(IList<ITreeProgram<double>> children) =>
            children == null || children.Count != 4
                ? null
                : new IfFunction(children[0], children[1], children[2], children[3]);

        #endregion
    }
}