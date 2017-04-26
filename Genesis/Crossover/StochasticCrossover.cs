// ------------------------------------------
// <copyright file="StochasticCrossover.cs" company="Pedro Sequeira">
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

namespace Genesis.Crossover
{
    public class StochasticCrossover : ICrossoverOperator
    {
        #region Fields

        private readonly IDictionary<ICrossoverOperator, double> _possibleCrossovers;
        private readonly Random _random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Constructors

        public StochasticCrossover(IList<ICrossoverOperator> possibleCrossovers)
        {
            this._possibleCrossovers = possibleCrossovers.ToDictionary(x => x, x => 1d / possibleCrossovers.Count);
        }

        public StochasticCrossover(IDictionary<ICrossoverOperator, double> possibleCrossovers)
        {
            this._possibleCrossovers = possibleCrossovers;
        }

        #endregion

        #region Public Methods

        public IElement Crossover(IElement parent1, IElement parent2)
        {
            return this._possibleCrossovers.GetRandomItem(this._random).Crossover(parent1, parent2);
        }

        public IEnumerable<IElement> GetAllOffspring(IElement parent1, IElement parent2)
        {
            var offspring = new HashSet<IElement>();
            foreach (var crossover in this._possibleCrossovers.Keys)
                offspring.AddRange(crossover.GetAllOffspring(parent1, parent2));
            return offspring;
        }

        #endregion

        #region IDisposable Support

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;
            if (disposing) this._possibleCrossovers.Clear();
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