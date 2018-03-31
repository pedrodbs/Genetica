// ------------------------------------------
// <copyright file="SwapMutation.cs" company="Pedro Sequeira">
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
//    Last updated: 03/28/2018
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
    ///     Represents a <see cref="IMutationOperator{TProgram}" /> for <see cref="ITreeProgram{TOutput}" />.
    ///     This operator mutates a given program by swapping (reversing the order of) the children of a randomly-selected
    ///     function sub-program.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class SwapMutation<TProgram, TOutput> : IMutationOperator<TProgram> where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        public IEnumerable<TProgram> GetAllMutations(TProgram program)
        {
            if (program == null) return new List<TProgram>();

            var mutations = new HashSet<TProgram> { program };

            // reverses the order of all function sub-programs
            var subProgs = new List<ITreeProgram<TOutput>> {program};
            subProgs.AddRange(program.GetSubPrograms());
            for (var i = 0; i < program.Length; i++)
            {
                var subProg = subProgs[i];
                if (subProg.IsLeaf()) continue;
                mutations.Add((TProgram) program.Replace((uint) i,
                    subProg.CreateNew(subProg.Input.Reverse().ToList())));
            }

            return mutations;
        }

        /// <summary>
        ///     Mutates the given <see cref="TProgram" /> by swapping (reversing the order of) the children of a
        ///     randomly-selected function sub-program.
        /// </summary>
        /// <param name="program">The program we want to mutate.</param>
        /// <returns>
        ///     A new <see cref="TProgram" />by swapping (reversing the order of) the children of a randomly-selected function
        ///     sub-program.
        /// </returns>
        public TProgram Mutate(TProgram program)
        {
            if (program == null) return default(TProgram);

            // get sub-program at a random mutation point
            var mutatePoint = (uint) this._random.Next(program.Length);
            var prog = program.ProgramAt(mutatePoint);

            // if program does not have children to swap return program itself
            if (prog.Input == null || prog.Input.Count < 2) return program;

            // swap children (inverse order) and returns a replaced sub-program
            var children = prog.Input.Reverse().ToList();
            return (TProgram) program.Replace(mutatePoint, prog.CreateNew(children));
        }

        #endregion
    }
}