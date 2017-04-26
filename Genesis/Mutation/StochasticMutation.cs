// ------------------------------------------
// <copyright file="StochasticMutation.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/03/08
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Mutation
{
    public class StochasticMutation : IMutationOperator
    {
        #region Fields

        private readonly IDictionary<IMutationOperator, double> _possibleMutations;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        public StochasticMutation(ICollection<IMutationOperator> possibleSelections)
        {
            this._possibleMutations = possibleSelections.ToDictionary(x => x, x => 1d / possibleSelections.Count);
        }

        public StochasticMutation(IDictionary<IMutationOperator, double> possibleMutations)
        {
            this._possibleMutations = possibleMutations;
        }

        #endregion

        #region Public Methods

        public IEnumerable<IElement> GetAllMutations(IElement element)
        {
            var mutations = new HashSet<IElement>();
            foreach (var mutationOperator in this._possibleMutations.Keys)
                mutations.AddRange(mutationOperator.GetAllMutations(element));
            return mutations;
        }

        /// <summary>
        ///     Randomly chooses one of the <see cref="IMutationOperator" />s to perform the mutation.
        /// </summary>
        /// <returns>A new element corresponding to the given element mutated.</returns>
        /// <param name="element">An element to be mutated.</param>
        public IElement Mutate(IElement element)
        {
            return this._possibleMutations.GetRandomItem(this._random).Mutate(element);
        }

        #endregion

        #region IDisposable Support

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;
            if (disposing) this._possibleMutations.Clear();
            this._disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}