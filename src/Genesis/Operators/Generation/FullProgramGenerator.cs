// ------------------------------------------
// <copyright file="FullProgramGenerator.cs" company="Pedro Sequeira">
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
//    Last updated: 03/23/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Linq;
using Genesis.Elements;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Operators.Generation
{
    /// <summary>
    ///     Represents a <see cref="IProgramGenerator{TProgram,TOutput}" /> that generates programs of a given depth.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    /// <typeparam name="TOutput">The type of program output.</typeparam>
    public class FullProgramGenerator<TProgram, TOutput> : IProgramGenerator<TProgram, TOutput>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        public TProgram Generate(PrimitiveSet<TProgram> primitives, uint maxDepth)
        {
            return this.Generate(primitives, 0, maxDepth);
        }

        #endregion

        #region Private & Protected Methods

        private TProgram Generate(PrimitiveSet<TProgram> primitives, uint depth, uint maxDepth)
        {
            // check max depth, just return a random terminal program
            if (depth == maxDepth)
                return primitives.Terminals.ToList().GetRandomItem(this._random);

            // otherwise, get a random operator
            var program = primitives.Functions.ToList().GetRandomItem(this._random);

            // recursively generate random children for it
            var numChildren = program.Children.Count;
            var children = new ITreeProgram<TOutput>[numChildren];
            for (var i = 0; i < numChildren; i++)
                children[i] = this.Generate(primitives, depth + 1, maxDepth);

            // generate a new program with the children
            return (TProgram) program.CreateNew(children);
        }

        #endregion
    }
}