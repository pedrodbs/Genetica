// ------------------------------------------
// <copyright file="PrimitiveSet.cs" company="Pedro Sequeira">
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
using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;
using Genesis.Util;

namespace Genesis.Elements
{
    public class PrimitiveSet : IDisposable
    {
        #region Static Fields & Constants

        public static readonly Constant Zero = new Constant(0);
        public static readonly Constant One = new Constant(1);

        public static readonly PrimitiveSet Default = new PrimitiveSet(
            new HashSet<Terminal> {Zero, One},
            new HashSet<IFunction>
            {
                new AdditionFunction(Zero, Zero),
                new CosineFunction(Zero),
                new DivisionFunction(Zero, Zero),
                new IfFunction(Zero, Zero, Zero, Zero),
                new LogarithmFunction(Zero, Zero),
                new MaxFunction(Zero, Zero),
                new MinFunction(Zero, Zero),
                new MultiplicationFunction(Zero, Zero),
                new PowerFunction(Zero, Zero),
                new SineFunction(Zero),
                new SubtractionFunction(Zero, Zero)
            });

        #endregion

        #region Fields

        private readonly HashSet<IFunction> _functions;
        private readonly HashSet<Terminal> _terminals;
        private bool _disposed;

        #endregion

        #region Constructors

        public PrimitiveSet(IEnumerable<Terminal> terminals, IEnumerable<IFunction> operators)
        {
            this._terminals = new HashSet<Terminal>(terminals);
            this._functions = new HashSet<IFunction>(operators);
        }

        #endregion

        #region Properties & Indexers

        public IReadOnlyCollection<IFunction> Functions => this._functions.ToList();

        public IReadOnlyCollection<Terminal> Terminals => this._terminals.ToList();

        #endregion

        #region Public Methods

        public void Add(PrimitiveSet primitiveSet)
        {
            this._terminals.AddRange(primitiveSet.Terminals);
            this._functions.AddRange(primitiveSet.Functions);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private & Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;
            if (disposing)
            {
                this._functions.Clear();
                this._terminals.Clear();
            }
            this._disposed = true;
        }

        #endregion
    }
}