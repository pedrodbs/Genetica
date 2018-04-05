// ------------------------------------------
// <copyright file="MathProgram.cs" company="Pedro Sequeira">
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

using System;
using System.Collections.Generic;
using System.Linq;
using Genetica.Elements.Terminals;

namespace Genetica.Elements
{
    /// <summary>
    ///     Represents a base class for mathematical program programs. A <see cref="MathProgram" /> represents a mathematical
    ///     expression whose output is a double-precision value. Math programs are useful to perform symbolic regression.
    /// </summary>
    public abstract class MathProgram : ITreeProgram<double>
    {
        #region Fields

        private readonly int _hashCode;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="MathProgram" /> with the given children / input nodes.
        /// </summary>
        /// <param name="children">The input nodes to this math program.</param>
        protected MathProgram(ITreeProgram<double>[] children)
        {
            this.Input = children;

            // cache hash-code and length
            this._hashCode = this.ProduceHashCode();
            this.Length = (ushort) (1 + this.Input.Sum(child => child.Length));
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets this program's expression in normal form by combining the <see cref="Label" />s of all descendant
        ///     programs encountered starting from this program (the root).
        /// </summary>
        public abstract string Expression { get; }

        /// <summary>
        ///     Gets the program's length, i.e., the total number of <see cref="MathProgram" /> descendant programs encountered
        ///     starting from this program.
        /// </summary>
        public ushort Length { get; }

        /// <summary>
        ///     Gets the children of this program, i.e., the direct <see cref="MathProgram" /> descendants of this program.
        /// </summary>
        public IReadOnlyList<ITreeProgram<double>> Input { get; }

        IReadOnlyList<ITreeNode> ITreeNode.Children => this.Input;

        /// <inheritdoc />
        public abstract string Label { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return !(obj is null) && (ReferenceEquals(this, obj) ||
                                      obj.GetType() == this.GetType() && this.Equals((MathProgram) obj));
        }

        /// <inheritdoc />
        public override int GetHashCode() => this._hashCode;

        /// <inheritdoc />
        public override string ToString() => this.Expression;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Checks if two <see cref="MathProgram" /> are equal.
        /// </summary>
        /// <param name="left">The first program.</param>
        /// <param name="right">The second program.</param>
        /// <returns><c>true</c> if the programs are equal <c>false</c> otherwise.</returns>
        public static bool operator ==(MathProgram left, MathProgram right)
        {
            return ReferenceEquals(left, right) || !(left is null) && left.Equals(right);
        }

        /// <summary>
        ///     Checks if two <see cref="MathProgram" /> are not equal.
        /// </summary>
        /// <param name="left">The first program.</param>
        /// <param name="right">The second program.</param>
        /// <returns><c>true</c> if the programs are not equal <c>false</c> otherwise.</returns>
        public static bool operator !=(MathProgram left, MathProgram right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Checks whether this program is equal to another.
        /// </summary>
        /// <param name="other">The other program to check for equality.</param>
        /// <returns>
        ///     <c>true</c> if the objects are the same or have the same <see cref="Label" /> and their <see cref="Input" />
        ///     sequence is the same.
        /// </returns>
        public bool Equals(MathProgram other)
        {
            return !(other is null) &&
                   (ReferenceEquals(this, other) ||
                    this.GetType() == other.GetType() &&
                    this._hashCode == other._hashCode &&
                    string.Equals(this.Label, other.Label) &&
                    this.Input.SequenceEqual(other.Input));
        }

        /// <summary>
        ///     Compares this program with another program using a <c>string.CompareOrdinal</c> comparison over their
        ///     <see cref="Expression" />.
        /// </summary>
        /// <param name="other">The other program to compare to.</param>
        /// <returns>
        ///     A value indicating if this program is less, equal or more than the other program, according to their
        ///     <see cref="Expression" /> comparison.
        /// </returns>
        public int CompareTo(ITreeProgram<double> other) => string.CompareOrdinal(this.Expression, other.Expression);

        /// <summary>
        ///     Computes the output of the program based on this program's mathematical expression and its <see cref="Input" />.
        /// </summary>
        /// <returns>A value corresponding to the execution of this program's mathematical expression and its <see cref="Input" />.</returns>
        public abstract double Compute();

        /// <inheritdoc />
        public virtual ITreeProgram<double> Simplify()
        {
            // if this is a constant value, just return a constant with that value
            if (this.IsConstant())
                return new Constant(this.Compute());

            // otherwise tries to simplify children
            var input = new ITreeProgram<double>[this.Input.Count];
            for (var i = 0; i < this.Input.Count; i++)
                input[i] = this.Input[i].Simplify();

            // creates new program with possibly simplified input
            return this.CreateNew(input);
        }

        /// <summary>
        ///     Creates a new <see cref="MathProgram" /> which is a copy of this program.
        /// </summary>
        /// <param name="children">The children of the new program.</param>
        /// <returns>A new <see cref="MathProgram" /> of the same kind of this program with the given child programs.</returns>
        public abstract ITreeProgram<double> CreateNew(IList<ITreeProgram<double>> children);

        /// <summary>
        ///     Gets a program node representing the primitive of the corresponding <see cref="MathProgram" /> derived type. For
        ///     functions this corresponds to a <see cref="MathProgram" /> whose inputs are <see cref="Constant.Zero" />, for
        ///     terminals it returns the terminal itself.
        /// </summary>
        /// <returns>The default program node of the corresponding <see cref="MathProgram" /> derived type.</returns>
        public ITreeProgram<double> GetPrimitive()
        {
            if (this.Input == null || this.Input.Count == 0) return this;
            var children = new ITreeProgram<double>[this.Input.Count];
            for (var i = 0; i < this.Input.Count; i++)
                children[i] = Constant.Zero;
            return this.CreateNew(children);
        }

        #endregion

        #region Private & Protected Methods

        private int ProduceHashCode()
        {
            unchecked
            {
                const int hashingBase = (int) 2166136261;
                const int hashingMultiplier = 16777619;

                var hashCode = hashingBase;
                foreach (var child in this.Input)
                    hashCode = (hashCode * hashingMultiplier) ^ child.GetHashCode();
                return (hashCode * hashingMultiplier) ^ this.Label?.GetHashCode() ?? 0;
            }
        }

        #endregion
    }
}