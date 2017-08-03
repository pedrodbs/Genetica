// ------------------------------------------
// <copyright file="ValuedObject.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis.Examples.LogisticRegression
//    Last updated: 2017/07/27
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using Genesis.Elements.Terminals;

namespace Genesis.Examples.LogisticRegression
{
    public class ValuedObject : IValued
    {
        #region Properties & Indexers

        public double Value { get; set; }

        #endregion
    }
}