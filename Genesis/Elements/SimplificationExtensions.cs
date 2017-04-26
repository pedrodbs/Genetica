// ------------------------------------------
// <copyright file="SimplificationExtensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/03/16
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
using Genesis.Evaluation;
using Genesis.Util;

namespace Genesis.Elements
{
    public static class SimplificationExtensions
    {
        #region Static Fields & Constants

        private const string VAR_NAME_STR = "VAR";
        private const double MIN_VAL = -1000;
        private const double RANGE = -2 * MIN_VAL;
        private const double DEFAULT_MARGIN = 1e-6d;
        private const uint DEFAULT_NUM_TRIALS = 1000;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Verifies whether the <see cref="IElement" /> contains a constant value, i.e., whether any one of its
        ///     descendant elements are instances have a constant value equal to <paramref name="val" />.
        /// </summary>
        /// <returns>
        ///     <c>true</c>, if element contains a constant value equal to <paramref name="val" />, <c>false</c>
        ///     otherwise.
        /// </returns>
        /// <param name="element">The element to verify whether it contains a constant.</param>
        /// <param name="val">The value to test for the element.</param>
        public static bool ContainsConstant(this IElement element, double val)
        {
            return element != null &&
                   (element.EqualsConstant(val) ||
                    element.Children != null && element.Children.Count > 0 &&
                    element.Children.Any(child => child.ContainsConstant(val)));
        }

        /// <summary>
        ///     Verifies whether the <see cref="IElement" /> is a constant value, i.e., whether all its descendant leaf
        ///     elements are instances of <see cref="Constant" />, and whether the associated value equals to
        ///     <paramref name="val" />.
        /// </summary>
        /// <returns>
        ///     <c>true</c>, if element is a constant and its value equals <paramref name="val" />, <c>false</c>
        ///     otherwise.
        /// </returns>
        /// <param name="element">The element to verify whether it is a constant.</param>
        /// <param name="val">The value to test for the element.</param>
        public static bool EqualsConstant(this IElement element, double val)
        {
            return element.IsConstant() && element.GetValue().Equals(val);
        }

        /// <summary>
        ///     Gets the root-mean-square deviation (RMSD) between the values computed for the given elements. The method works by
        ///     substituting the variables in both elements' expressions by random values for a certain number of trials. In each
        ///     trial, the square of the difference between the values computed by both elements is calculated.
        /// </summary>
        /// <param name="element">The first element that we want to test.</param>
        /// <param name="other">The second element that we want to test.</param>
        /// <param name="numTrials">The number of trials used to compute the squared difference.</param>
        /// <returns>The RMSD between the several values computed for the given elements.</returns>
        public static double GetValueRmsd(
            this IElement element, IElement other, uint numTrials = DEFAULT_NUM_TRIALS)
        {
            // checks null
            if (element == null || other == null) return double.MaxValue;

            // checks expression or constant equivalence after simplification
            element = element.Simplify();
            other = other.Simplify();
            if (element.Equals(other) || element.IsConstant() && other.IsConstant())
                return Math.Abs(element.GetValue() - other.GetValue());

            // replaces variables of each expression by custom variables
            var customVariables = new Dictionary<IElement, CustomVariable>();
            element = ReplaceVariables(element, customVariables);
            other = ReplaceVariables(other, customVariables);
            if (customVariables.Count == 0) return double.MaxValue;

            // gets random values for each variable
            var customVariablesValues = customVariables.Values.ToDictionary(
                variable => variable, variable => GetTrialRandomNumbers(numTrials));

            // runs a batch of trials
            var squareDiffSum = 0d;
            for (var i = 0; i < numTrials; i++)
            {
                // replaces the value of each variable by a random number
                foreach (var customVariablesValue in customVariablesValues)
                    customVariablesValue.Key.Value = customVariablesValue.Value[i];

                // computes difference of the resulting values of the expressions
                var elem1Value = element.GetValue();
                var elem2Value = other.GetValue();
                var diff = elem1Value.Equals(elem2Value) ? 0 : elem1Value - elem2Value;
                squareDiffSum += diff * diff;
            }

            // returns rmsd
            return Math.Sqrt(squareDiffSum);
        }

        /// <summary>
        ///     Verifies whether the <see cref="IElement" /> has a constant value, i.e., whether all its descendant leaf
        ///     elements are instances of <see cref="Constant" />.
        /// </summary>
        /// <returns><c>true</c>, if all leaf elements are constant, <c>false</c> otherwise.</returns>
        /// <param name="element">The element to verify whether it is a constant.</param>
        public static bool IsConstant(this IElement element)
        {
            return element != null &&
                   (element is Constant ||
                    element.Children != null && element.Children.Count > 0 &&
                    element.Children.All(child => child.IsConstant()));
        }

        /// <summary>
        ///     Checks whether the given <see cref="IElement" /> computes a value that is consistently equivalent to that computed
        ///     by another element according to some margin. The method works by substituting the variables in both elements'
        ///     expressions by random values for a certain number of trials. In each trial, the difference between the values
        ///     computed by both elements is calculated. If the difference is less than a given margin for all the trials, then the
        ///     elements are considered to be value-equivalent.
        /// </summary>
        /// <param name="element">The first element that we want to test.</param>
        /// <param name="other">The second element that we want to test.</param>
        /// <param name="margin">
        ///     The margin used to compare against the difference of values computed by both expressions in each
        ///     trial.
        /// </param>
        /// <param name="numTrials">The number of trials used to discern whether the given elements are equivalent.</param>
        /// <returns><c>True</c> if the given elements are considered to be value-equivalent, <c>False</c> otherwise.</returns>
        public static bool IsValueEquivalent(
            this IElement element, IElement other, double margin = DEFAULT_MARGIN, uint numTrials = DEFAULT_NUM_TRIALS)
        {
            // checks null
            if (element == null || other == null) return false;

            // checks expression or constant equivalence after simplification
            element = element.Simplify();
            other = other.Simplify();
            if (element.Equals(other) ||
                element.IsConstant() && other.IsConstant() && Math.Abs(element.GetValue() - other.GetValue()) < margin)
                return true;

            // replaces variables of each expression by custom variables
            var customVariables = new Dictionary<IElement, CustomVariable>();
            element = ReplaceVariables(element, customVariables);
            other = ReplaceVariables(other, customVariables);
            if (customVariables.Count == 0) return false;

            // gets random values for each variable
            var customVariablesValues = customVariables.Values.ToDictionary(
                variable => variable, variable => GetTrialRandomNumbers(numTrials));

            // runs a batch of trials
            for (var i = 0; i < numTrials; i++)
            {
                // replaces the value of each variable by a random number
                foreach (var customVariablesValue in customVariablesValues)
                    customVariablesValue.Key.Value = customVariablesValue.Value[i];

                // compares the resulting values of the expressions
                if (Math.Abs(element.GetValue() - other.GetValue()) >= margin)
                    return false;
            }

            // all trials are over, elements are value equivalent
            return true;
        }

        /// <summary>
        ///     Simplifies the expression of the given element by returning a new <see cref="IElement" /> which value will
        ///     always be equal to the original element, i.e. it removes reduntant operations, e.g. subtrees with functions
        ///     that will always result in the value of one of its operands.
        /// </summary>
        /// <returns>An element corresponding to a simplification of the given element.</returns>
        /// <param name="element">The element we want to simplify.</param>
        public static IElement Simplify(this IElement element)
        {
            if (element == null) return null;

            // if its a terminal, just return the element, no more simplifications possible
            if (element.Children == null || element.Children.Count == 0)
                return element;

            // if its a constant value, just return a constant with that value
            if (element.IsConstant())
                return new Constant(element.GetValue());

            // otherwise first tries to simplify children
            var children = new IElement[element.Children.Count];
            for (var i = 0; i < element.Children.Count; i++)
                children[i] = element.Children[i].Simplify();

            // if its an addition, check whether one operand is 0 and return the other
            if (element is AdditionFunction)
                if (children[0].EqualsConstant(0))
                    return children[1];
                else if (children[1].EqualsConstant(0))
                    return children[0];

            // if its an subtraction
            if (element is SubtractionFunction)

                // if the operands are equal, return 0
                if (children[0].Equals(children[1]))
                    return new Constant(0);

                // check whether second operand is 0 and return the first
                else if (children[1].EqualsConstant(0))
                    return children[0];

            // if its a multiplication, check whether one operand is 1 and return the other
            if (element is MultiplicationFunction)
                if (children[0].EqualsConstant(1))
                    return children[1];
                else if (children[1].EqualsConstant(1))
                    return children[0];

            // if its a multiplication, check whether one of operands is 0 and return 0
            if (element is MultiplicationFunction &&
                (children[0].EqualsConstant(0) || children[1].EqualsConstant(0)))
                return new Constant(0);

            // if its a division or power, check whether second operand is 1 and return the first
            if ((element is DivisionFunction || element is PowerFunction) && children[1].EqualsConstant(1))
                return children[0];

            // if its a division
            if (element is DivisionFunction)

                //if operands are equal, return 1
                if (children[0].Equals(children[1]))
                    return new Constant(1);

                //if first operand is 0 return 0
                else if (children[0].EqualsConstant(0))
                    return new Constant(0);

            // if its a min or max, check whether operands are equal and return the first
            if ((element is MaxFunction || element is MinFunction) && children[0].Equals(children[1]))
                return children[0];

            // if its an if clause, check whether the first child is a constant and returns one of the other children
            // accordingly
            if (element is IfFunction && children[0].IsConstant())
            {
                var val = children[0].GetValue();
                return val.Equals(0) ? children[1] : (val > 0 ? children[2] : children[3]);
            }

            // if its an if clause, check whether result children are equal, in which case replace by one of them
            if (element is IfFunction && children[1].Equals(children[2]) && children[1].Equals(children[3]))
                return children[1];

            return element.CreateNew(children);
        }

        /// <summary>
        ///     Simplifies the expression of the given <see cref="IElement" /> by returning a sub-combination whose fitness is
        ///     approximately equal to that of the original element by a margin of <paramref name="margin" />, and whose
        ///     expression is naturally simpler, i.e., has fewer sub-elements.
        /// </summary>
        /// <returns>An element corresponding to a simplification of the given element.</returns>
        /// <param name="element">The element we want to simplify.</param>
        /// <param name="fitnessFunction">The fitness function used to determine equivalence.</param>
        /// <param name="margin">
        ///     The acceptable difference between the fitness of the given element and that of a simplified element for them to be
        ///     considered equivalent.
        /// </param>
        public static IElement Simplify(
            this IElement element, IFitnessFunction fitnessFunction, double margin = DEFAULT_MARGIN)
        {
            // gets original element fitness and count
            var fitness = fitnessFunction.Evaluate(element);

            // gets all subcombinations of the element
            var simplified = element;
            var count = element.Count;
            var subCombs = element.GetSubCombinations();
            foreach (var subComb in subCombs)
            {
                var subFit = fitnessFunction.Evaluate(subComb);
                var subCount = subComb.Count;

                //checks their fitness and count
                if (Math.Abs(subFit - fitness) < margin && subCount < count)
                {
                    simplified = subComb;
                    count = subCount;
                }
            }

            subCombs.Clear();
            return simplified;
        }

        #endregion

        #region Private & Protected Methods

        private static IList<double> GetTrialRandomNumbers(uint numTrials)
        {
            var rnd = new Random();

            // adds fixed numbers
            var trialNumbers = new List<double>((int) numTrials) {0, -1, 1, 0.1, -0.1};

            // adds rest with random numbers
            for (var i = trialNumbers.Count; i < numTrials; i++)
                trialNumbers.Add(MIN_VAL + RANGE * rnd.NextDouble());

            // shuffles and returns
            trialNumbers.Shuffle();
            return trialNumbers;
        }

        #region Private Methods

        private static IElement ReplaceVariables(this IElement element, IDictionary<IElement, CustomVariable> customVars)
        {
            // if element is not a variable, tries to replace all children recursively
            if (!(element is Variable))
                return element.CreateNew(element.Children?.Select(child => child.ReplaceVariables(customVars)).ToList());

            // checks if a corresponding variable was already created
            if (!customVars.ContainsKey(element))
                customVars.Add(element, new CustomVariable($"{VAR_NAME_STR}{customVars.Count}"));

            // replaces it by the custom variable
            return customVars[element];
        }

        #endregion

        #endregion

        #region Nested type: CustomVariable

        private class CustomVariable : Variable
        {
            #region Properties & Indexers

            public double Value { private get; set; }

            #endregion

            #region Constructors

            public CustomVariable(string name) : base(name, null)
            {
            }

            #endregion

            #region Public Methods

            public override IElement Clone()
            {
                // in this case we want the same variable reference when cloned
                return this;
            }

            public override double GetValue() => Value;

            #endregion
        }

        #endregion
    }
}