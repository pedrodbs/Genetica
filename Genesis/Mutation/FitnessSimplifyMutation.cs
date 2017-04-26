// ------------------------------------------
// <copyright file="FitnessSimplifyMutation.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;
using Genesis.Elements;
using Genesis.Evaluation;

namespace Genesis.Mutation
{
    /// <summary>
    ///     This mutation operator tries to simplify (shorten the expression of) a given <see cref="IElement" /> by
    ///     removing descendant elements that do not affect its fitness by some degree.
    /// </summary>
    public class FitnessSimplifyMutation : IMutationOperator
    {
        #region Fields

        private readonly double _epsilon;
        private readonly IFitnessFunction _fitnessFunction;

        #endregion

        #region Constructors

        public FitnessSimplifyMutation(IFitnessFunction fitnessFunction, double epsilon)
        {
            this._epsilon = epsilon;
            this._fitnessFunction = fitnessFunction;
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        public IEnumerable<IElement> GetAllMutations(IElement element)
        {
            return new HashSet<IElement> {this.Mutate(element)};
        }

        public IElement Mutate(IElement element)
        {
            return element.Simplify(this._fitnessFunction, this._epsilon);
        }

        #endregion
    }
}