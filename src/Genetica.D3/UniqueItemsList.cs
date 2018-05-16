// ------------------------------------------
// <copyright file="UniqueItemsList.cs" company="Pedro Sequeira">
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
//    Project: Genetica.D3
//    Last updated: 05/16/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;

namespace Genetica.D3
{
    /// <summary>
    ///     Represents an ordered set of items (no repetitions).
    /// </summary>
    /// <typeparam name="T">The type of items to be stored in the list.</typeparam>
    public class UniqueItemsList<T> : IDisposable where T : class
    {
        #region Fields

        private readonly IList<T> _values;

        private IDictionary<T, uint> _indexes;

        #endregion

        #region Constructors

        internal UniqueItemsList() : this(-1)
        {
        }

        /// <summary>
        ///     Creates a new <see cref="UniqueItemsList{T}" /> with the given capacity.
        /// </summary>
        /// <param name="capacity">The capacity used to initialize the list, default is -1 (capacity not defined).</param>
        public UniqueItemsList(int capacity = -1)
        {
            this._indexes = capacity == -1 ? new Dictionary<T, uint>() : new Dictionary<T, uint>(capacity);
            this._values = capacity == -1 ? new List<T>() : new List<T>(capacity);
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the number of items stored in this list.
        /// </summary>
        public int Count => this._values.Count;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Clears this list by removing all items.
        /// </summary>
        public void Clear()
        {
            this._indexes.Clear();
            this._values.Clear();
        }

        /// <summary>
        ///     Gets the index of a given item in this list. If the item is not present, it first adds the item to the list.
        /// </summary>
        /// <param name="item">The item whose index we want to retrieve and/or add.</param>
        /// <returns>The index of the given item in the list.</returns>
        public uint GetIndex(T item)
        {
            if (this._indexes.ContainsKey(item)) return this._indexes[item];
            var idx = (uint) this._indexes.Count;
            this._indexes.Add(item, idx);
            this._values.Add(item);
            return idx;
        }

        /// <summary>
        ///     Gets the item at the given index, or null if the index is out of bounds.
        /// </summary>
        /// <param name="idx">The index of the item that we want to retrieve.</param>
        /// <returns>The item at the given index, or null if the index is out of bounds.</returns>
        public T GetItem(uint idx) => idx < this._values.Count ? this._values[(int) idx] : null;

        /// <inheritdoc />
        public void Dispose()
        {
            this.Clear();
            this._indexes = null;
        }

        #endregion
    }
}