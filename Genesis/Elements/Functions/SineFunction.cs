// ------------------------------------------
// <copyright file="SineFunction.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/08/12
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
    public class SineFunction : UnaryFunction
    {
        #region Constructors

        public SineFunction(IElement operand) : base(operand)
        {
            this.Expression = $"{this.Label}({this.Operand.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public override string Expression { get; }

        public override string Label => "sin";

        #endregion

        #region Public Methods

        public override IElement CreateNew(IList<IElement> children)
        {
            return children == null || children.Count != 1
                ? null
                : new SineFunction(children[0]);
        }

        public override double GetValue()
        {
            return Math.Sin(this.Operand.GetValue());
        }

        #endregion
    }
}