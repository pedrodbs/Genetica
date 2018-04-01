// ------------------------------------------
// <copyright file="FitnessSimplifyMutation.cs" company="Pedro Sequeira">
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
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using Genesis.Elements;
using Genesis.Evaluation;

namespace Genesis.Operators.Mutation
{
    /// <summary>
    ///     Represents a <see cref="IMutationOperator{TProgram}" /> for <see cref="MathProgram" />.
    ///     This mutation operator tries to simplify (shorten the expression of) a given program by removing descendant
    ///     programs that do not affect its fitness by some degree.
    /// </summary>
    public class FitnessSimplifyMutation : IMutationOperator<MathProgram>
    {
        #region Fields

        private readonly double _epsilon;
        private readonly IFitnessFunction<MathProgram> _fitnessFunction;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="FitnessSimplifyMutation" /> with the given fitness function.
        /// </summary>
        /// <param name="fitnessFunction">The fitness function used for simplification.</param>
        /// <param name="epsilon">
        ///     The acceptable difference between the fitness of the given program and that of a simplified program for them to be
        ///     considered equivalent.
        /// </param>
        public FitnessSimplifyMutation(IFitnessFunction<MathProgram> fitnessFunction, double epsilon)
        {
            this._epsilon = epsilon;
            this._fitnessFunction = fitnessFunction;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public IEnumerable<MathProgram> GetAllMutations(MathProgram program) =>
            new HashSet<MathProgram> {this.Mutate(program)};

        /// <inheritdoc />
        public MathProgram Mutate(MathProgram program) =>
            (MathProgram) program.Simplify(this._fitnessFunction, this._epsilon);

        #endregion
    }
}