// ------------------------------------------
// <copyright file="SimplifyMutation.cs" company="Pedro Sequeira">
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

using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;
using MathNet.Numerics.Random;

namespace Genesis.Operators.Mutation
{
    /// <summary>
    ///     Represents a <see cref="IMutationOperator{TProgram}" /> for <see cref="MathProgram" />.
    ///     This mutation operator simplifies (tries to shorten the expression of) a random descendant program of a given
    ///     program.
    /// </summary>
    public class SimplifyMutation : IMutationOperator<MathProgram>
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public IEnumerable<MathProgram> GetAllMutations(MathProgram program)
        {
            var mutations = new HashSet<MathProgram>();
            if (program == null) return mutations;

            // replaces each sub-program by its simplification
            var subProgs = new List<MathProgram> {program};
            subProgs.AddRange(program.GetSubPrograms().Cast<MathProgram>());
            for (var i = 0; i < program.Length; i++)
                mutations.Add((MathProgram) program.Replace((uint) i, subProgs[i].Simplify()));

            return mutations;
        }

        /// <summary>
        ///     Mutates the given <see cref="MathProgram" /> by simplifying a random sub-program of the given
        ///     <see cref="MathProgram" />.
        /// </summary>
        /// <param name="program">The program we want to mutate.</param>
        /// <returns>A new <see cref="MathProgram" /> resulting of the simplification of one of the sub-programs.</returns>
        public MathProgram Mutate(MathProgram program)
        {
            if (program == null) return null;

            // define the mutation point randomly
            var mutatePoint = (uint) this._random.Next(program.Length);

            // replaces with a simplified version of the sub-program
            var simp = ((MathProgram) program.ProgramAt(mutatePoint)).Simplify();
            return (MathProgram) program.Replace(mutatePoint, simp);
        }

        #endregion
    }
}