// ------------------------------------------
// <copyright file="MathProgramExtensions.cs" company="Pedro Sequeira">
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
//    Project: Genesis
//    Last updated: 03/26/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements.Terminals;
using Genesis.Evaluation;
using Genesis.Util;
using MathNet.Numerics.Random;

namespace Genesis.Elements
{
    /// <summary>
    ///     Declares a set of extension methods for <see cref="MathProgram" />.
    /// </summary>
    public static class MathProgramExtensions
    {
        #region Static Fields & Constants

        private const string VAR_NAME_STR = "VAR";
        private const double DEFAULT_MARGIN = 1e-6d;
        private const uint DEFAULT_NUM_TRIALS = 1000;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Verifies whether the <see cref="MathProgram" /> contains a constant value, i.e., whether any one of its
        ///     descendant programs are instances have a constant value equal to <paramref name="val" />.
        /// </summary>
        /// <returns>
        ///     <c>true</c>, if program contains a constant value equal to <paramref name="val" />, <c>false</c> otherwise.
        /// </returns>
        /// <param name="program">The program to verify whether it contains a constant.</param>
        /// <param name="val">The value to test for the program.</param>
        public static bool ContainsConstant(this ITreeProgram<double> program, double val)
        {
            return program != null &&
                   (program.EqualsConstant(val) ||
                    program.Input != null && program.Input.Count > 0 &&
                    program.Input.Any(child => child.ContainsConstant(val)));
        }

        /// <summary>
        ///     Verifies whether the <see cref="MathProgram" /> is a constant value, i.e., whether all its descendant leaf
        ///     programs are instances of <see cref="Constant" />, and whether the associated value equals to
        ///     <paramref name="val" />.
        /// </summary>
        /// <returns>
        ///     <c>true</c>, if program is a constant and its value equals <paramref name="val" />, <c>false</c>
        ///     otherwise.
        /// </returns>
        /// <param name="program">The program to verify whether it is a constant.</param>
        /// <param name="val">The value to test for the program.</param>
        public static bool EqualsConstant(this ITreeProgram<double> program, double val) =>
            program.IsConstant() && program.Compute().Equals(val);

        /// <summary>
        ///     Computes a <see cref="Range" /> representing the minimum and maximum values that a given <see cref="MathProgram" />
        ///     can compute, as dictated by its sub-programs.
        /// </summary>
        /// <param name="program">The program whose range we want to compute.</param>
        /// <returns>The range of the given program.</returns>
        public static Range GetRange(this ITreeProgram<double> program)
        {
            // checks for constant value
            if (program.IsConstant())
            {
                var value = program.Compute();
                return new Range(value, value);
            }

            // checks for variable
            if (program is Variable) return ((Variable) program).Range;

            // collects info on ranges of all children 
            var childrenRanges = new List<IEnumerable<double>>();
            foreach (var child in program.Input)
            {
                var childRange = GetRange(child);
                childrenRanges.Add(new[] {childRange.Min, childRange.Max});
            }

            // gets all combinations between children ranges
            var min = double.MaxValue;
            var max = double.MinValue;
            var allRangeCombinations = childrenRanges.GetAllCombinations();
            foreach (var rangeCombination in allRangeCombinations)
            {
                // builds new program by replacing children with constant values (range min or max)
                var children = new ITreeProgram<double>[rangeCombination.Count];
                for (var i = 0; i < rangeCombination.Count; i++)
                    children[i] = new Constant(rangeCombination[i]);
                var newElem = program.CreateNew(children);

                // checks min and max values from new prog value
                var val = newElem.Compute();
                min = Math.Min(min, val);
                max = Math.Max(max, val);
            }

            return new Range(min, max);
        }

        /// <summary>
        ///     Gets the root-mean-square deviation (RMSD) between the values computed for the given programs. The method works by
        ///     substituting the variables in both programs' expressions by random values for a certain number of trials. In each
        ///     trial, the square of the difference between the values computed by both programs is calculated.
        /// </summary>
        /// <param name="program">The first program that we want to test.</param>
        /// <param name="other">The second program that we want to test.</param>
        /// <param name="numTrials">The number of trials used to compute the squared difference.</param>
        /// <returns>The RMSD between the several values computed for the given programs.</returns>
        public static double GetValueRmsd(
            this ITreeProgram<double> program, ITreeProgram<double> other, uint numTrials = DEFAULT_NUM_TRIALS)
        {
            // checks null
            if (program == null || other == null) return double.MaxValue;

            // checks expression or constant equivalence after simplification
            program = program.Simplify();
            other = other.Simplify();
            if (program.Equals(other) || program.IsConstant() && other.IsConstant())
                return Math.Abs(program.Compute() - other.Compute());

            // replaces variables of each expression by custom variables
            var customVariables = new Dictionary<Variable, Variable>();
            program = ReplaceVariables(program, customVariables);
            other = ReplaceVariables(other, customVariables);
            if (customVariables.Count == 0) return double.MaxValue;

            // gets random values for each variable
            var customVariablesValues = customVariables.Values.ToDictionary(
                variable => variable,
                variable => GetTrialRandomNumbers(numTrials, variable.Range));

            // runs a batch of trials
            var squareDiffSum = 0d;
            for (var i = 0; i < numTrials; i++)
            {
                // replaces the value of each variable by a random number
                foreach (var customVariablesValue in customVariablesValues)
                    customVariablesValue.Key.Value = customVariablesValue.Value[i];

                // computes difference of the resulting values of the expressions
                var prog1Value = program.Compute();
                var prog2Value = other.Compute();
                var diff = prog1Value.Equals(prog2Value) ? 0 : prog1Value - prog2Value;
                squareDiffSum += diff * diff;
            }

            // returns RMSD
            return Math.Sqrt(squareDiffSum / numTrials);
        }

        /// <summary>
        ///     Verifies whether the <see cref="MathProgram" /> has a constant value, i.e., whether all its descendant leaf
        ///     programs are instances of <see cref="Constant" />.
        /// </summary>
        /// <returns><c>true</c>, if all leaf programs are constant, <c>false</c> otherwise.</returns>
        /// <param name="program">The program to verify whether it is a constant.</param>
        public static bool IsConstant(this ITreeProgram<double> program)
        {
            return program != null &&
                   (program is Constant ||
                    program.Input != null && program.Input.Count > 0 &&
                    program.Input.All(child => child.IsConstant()));
        }

        /// <summary>
        ///     Checks whether the given <see cref="MathProgram" /> computes a value that is consistently equivalent to that
        ///     computed
        ///     by another program according to some margin. The method works by substituting the variables in both programs'
        ///     expressions by random values for a certain number of trials. In each trial, the difference between the values
        ///     computed by both programs is calculated. If the difference is less than a given margin for all the trials, then the
        ///     programs are considered to be value-equivalent.
        /// </summary>
        /// <param name="program">The first program that we want to test.</param>
        /// <param name="other">The second program that we want to test.</param>
        /// <param name="margin">
        ///     The margin used to compare against the difference of values computed by both expressions in each
        ///     trial.
        /// </param>
        /// <param name="numTrials">The number of trials used to discern whether the given programs are equivalent.</param>
        /// <returns><c>True</c> if the given programs are considered to be value-equivalent, <c>False</c> otherwise.</returns>
        public static bool IsValueEquivalent(
            this ITreeProgram<double> program, ITreeProgram<double> other, double margin = DEFAULT_MARGIN,
            uint numTrials = DEFAULT_NUM_TRIALS)
        {
            // checks null
            if (program == null || other == null) return false;

            // checks equivalence
            if (program.Equals(other)) return true;

            // checks expression or constant equivalence after simplification
            program = program.Simplify();
            other = other.Simplify();
            if (program.Equals(other) ||
                program.IsConstant() && other.IsConstant() && Math.Abs(program.Compute() - other.Compute()) < margin)
                return true;

            // gets RMSE by replacing the values of the variables in some number of trials
            return program.GetValueRmsd(other, numTrials) <= margin;
        }

        /// <summary>
        ///     Simplifies the expression of the given <see cref="MathProgram" /> by returning a sub-combination whose fitness is
        ///     approximately equal to that of the original program by a margin of <paramref name="margin" />, and whose
        ///     expression is naturally simpler, i.e., has fewer sub-programs.
        /// </summary>
        /// <returns>An program corresponding to a simplification of the given program.</returns>
        /// <param name="program">The program we want to simplify.</param>
        /// <param name="fitnessFunction">The fitness function used to determine equivalence.</param>
        /// <param name="margin">
        ///     The acceptable difference between the fitness of the given program and that of a simplified program for them to be
        ///     considered equivalent.
        /// </param>
        public static ITreeProgram<double> Simplify(
            this MathProgram program, IFitnessFunction<MathProgram> fitnessFunction,double margin = DEFAULT_MARGIN)
        {
            // gets original program's fitness and count
            var simplified = program;
            var fitness = fitnessFunction.Evaluate(program);
            var length = program.Length;

            // first replaces all sub-programs by infinity and NaN recursively
            var consts = new HashSet<MathProgram>
                         {
                             new Constant(double.NegativeInfinity),
                             new Constant(double.NaN),
                             new Constant(double.PositiveInfinity)
                         };
            bool simplificationFound;
            do
            {
                simplificationFound = false;
                var simpLength = simplified.Length;
                for (var i = 0u; i < simpLength && !simplificationFound; i++)
                {
                    if (consts.Contains(simplified.ProgramAt(i)))
                        continue;
                    foreach (var constant in consts)
                    {
                        var prog = (MathProgram)simplified.Replace(i, constant);
                        var progFit = fitnessFunction.Evaluate(prog);
                        var progLength = prog.Length;
                        if (Math.Abs(progFit - fitness) < margin && progLength <= length)
                        {
                            simplified = prog;
                            length = progLength;
                            simplificationFound = true;
                            break;
                        }
                    }
                }
            } while (simplificationFound);

            // then gets all sub-combinations of the simplified program
            var alternatives = simplified.GetSubCombinations();
            foreach (var treeProgram in alternatives)
            {
                var subComb = (MathProgram) treeProgram;
                var subFit = fitnessFunction.Evaluate(subComb);
                var subCombLength = subComb.Length;

                //checks their fitness and count
                if (Math.Abs(subFit - fitness) < margin && subCombLength < length)
                {
                    simplified = subComb;
                    length = subCombLength;
                }
            }

            return simplified;
        }

        #endregion

        #region Private & Protected Methods

        private static IList<double> GetTrialRandomNumbers(uint numTrials, Range range)
        {
            var rnd = new WH2006(RandomSeed.Robust());

            // adds equally-separated values
            var trialNumbers = new List<double>();
            var interval = range.Interval / numTrials;
            for (var i = 0; i < numTrials; i++)
                trialNumbers.Add(range.Min + i * interval);
            trialNumbers.Shuffle(rnd);
            return trialNumbers;
        }

        private static ITreeProgram<double> ReplaceVariables(
            this ITreeProgram<double> program, IDictionary<Variable, Variable> customVars)
        {
            // if program is not a variable, tries to replace all children recursively
            var variable = program as Variable;
            if (variable == null)
                return program.CreateNew(program.Input?.Select(child => child.ReplaceVariables(customVars))
                    .ToList());

            // checks if a corresponding variable has not yet been created
            if (!customVars.ContainsKey(variable))
                customVars.Add(variable, new Variable($"{VAR_NAME_STR}{customVars.Count}"));

            // replaces it by the custom variable
            return customVars[variable];
        }

        #endregion
    }
}