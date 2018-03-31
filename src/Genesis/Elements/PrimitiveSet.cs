// ------------------------------------------
// <copyright file="PrimitiveSet.cs" company="Pedro Sequeira">
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Util;

namespace Genesis.Elements
{
    /// <summary>
    ///     Represents a collection of <see cref="TProgram" /> primitives including a set of functions and terminals.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public class PrimitiveSet<TProgram> : IDisposable where TProgram : ITreeProgram
    {
        #region Fields

        private readonly HashSet<TProgram> _functions;
        private readonly HashSet<TProgram> _terminals;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="PrimitiveSet{TProgram" /> with the give terminals and functions.
        /// </summary>
        /// <param name="terminals">The programs representing the terminal primitives.</param>
        /// <param name="functions">The programs representing the function primitives.</param>
        public PrimitiveSet(IEnumerable<TProgram> terminals, IEnumerable<TProgram> functions)
        {
            this._terminals = new HashSet<TProgram>(terminals);
            this._functions = new HashSet<TProgram>(functions);
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the programs representing the function primitives.
        /// </summary>
        public IReadOnlyCollection<TProgram> Functions => this._functions.ToList();

        /// <summary>
        ///     Gets the programs representing the terminal primitives.
        /// </summary>
        public IReadOnlyCollection<TProgram> Terminals => this._terminals.ToList();

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var sb = new StringBuilder("Functions: ");
            foreach (var function in this._functions)
                sb.Append($"{function.Label},");
            if (this._functions.Count > 0) sb.Remove(sb.Length - 1, 1);
            sb.Append("\nTerminals: ");
            foreach (var terminal in this._terminals)
                sb.Append($"{terminal.Label},");
            if (this._terminals.Count > 0) sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Adds the functions and terminals in the given <see cref="PrimitiveSet{TProgram}" /> to this set.
        /// </summary>
        /// <param name="primitiveSet">The set containing the functions and terminals to be added to this set.</param>
        public void Add(PrimitiveSet<TProgram> primitiveSet)
        {
            this._terminals.AddRange(primitiveSet.Terminals);
            this._functions.AddRange(primitiveSet.Functions);
        }

        public void Dispose()
        {
            this._functions.Clear();
            this._terminals.Clear();
        }

        #endregion
    }
}