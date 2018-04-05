// ------------------------------------------
// <copyright file="UniqueItemsList.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genetica.D3
//    Last updated: 2017/05/31
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;

namespace Genetica.D3
{
    public class UniqueItemsList<T> : IDisposable where T : class
    {
        #region Fields

        private readonly IList<T> _values;

        private IDictionary<T, uint> _indexes;

        #endregion

        #region Properties & Indexers

        public int Count => this._values.Count;

        #endregion

        #region Constructors

        internal UniqueItemsList() : this(-1)
        {
        }

        public UniqueItemsList(int capacity = -1)
        {
            this._indexes = capacity == -1 ? new Dictionary<T, uint>() : new Dictionary<T, uint>(capacity);
            this._values = capacity == -1 ? new List<T>() : new List<T>(capacity);
        }

        #endregion

        #region Public Methods

        public void Clear()
        {
            this._indexes.Clear();
            this._values.Clear();
        }

        public uint GetIndex(T item)
        {
            if (this._indexes.ContainsKey(item)) return this._indexes[item];
            var idx = (uint) this._indexes.Count;
            this._indexes.Add(item, idx);
            this._values.Add(item);
            return idx;
        }

        public T GetItem(uint idx)
        {
            return idx < this._values.Count ? this._values[(int) idx] : null;
        }

        public void Dispose()
        {
            this.Clear();
            this._indexes = null;
        }

        #endregion
    }
}