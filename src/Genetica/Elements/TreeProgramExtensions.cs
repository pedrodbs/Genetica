// ------------------------------------------
// <copyright file="TreeProgramExtensions.cs" company="Pedro Sequeira">
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
//    Project: Genetica
//    Last updated: 04/02/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Genetica.Util;

namespace Genetica.Elements
{
    /// <summary>
    ///     Declares a set of extension methods for <see cref="ITreeProgram{TOutput}" /> objects.
    /// </summary>
    public static class TreeProgramExtensions
    {
        #region Public Methods

        /// <summary>
        ///     Checks whether a given <see cref="ITreeProgram{TOutput}" /> contains the given sub-program.
        ///     A sub-program is an program that is a descendant of a given program.
        /// </summary>
        /// <returns><c>true</c>, if the given program is a descendant of the given program, <c>false</c> otherwise.</returns>
        /// <param name="program">The program for which to look for a descendant equal to the given program.</param>
        /// <param name="other">The program we want to know if it is a sub-program.</param>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static bool ContainsSubElement<TOutput>(
            this ITreeProgram<TOutput> program, ITreeProgram<TOutput> other)

        {
            // checks prog
            if (program == null || other == null ||
                program.Length <= other.Length || program.Input?.Count == 0) return false;

            // search the descendants for the given program
            return program.Input != null &&
                   program.Input.Any(child => other.Equals(child) || child.ContainsSubElement(other));
        }

        /// <summary>
        ///     Gets a <see cref="IDictionary{T,T}" /> representing the program indexes in the common region between
        ///     <paramref name="program" /> and <paramref name="otherProgram" />. The dictionary represents the index
        ///     correspondence between the sub-programs of the given programs.
        /// </summary>
        /// <returns>The index correspondence between the sub-programs of the given programs.</returns>
        /// <param name="program">The first program</param>
        /// <param name="otherProgram">The other program to get the common region.</param>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static IDictionary<uint, uint> GetCommonRegionIndexes<TOutput>(
            this ITreeProgram<TOutput> program, ITreeProgram<TOutput> otherProgram)
        {
            var commonRegionIndexes = new Dictionary<uint, uint>();
            var idx1 = 0u;
            var idx2 = 0u;
            GetCommonRegionIndexes(program, otherProgram, commonRegionIndexes, ref idx1, ref idx2);
            return commonRegionIndexes;
        }

        /// <summary>
        ///     Gets a dictionary containing all the <see cref="ITreeProgram{TOutput}" /> leaves of the given program and their
        ///     count. This corresponds to the leaf nodes of the expression tree of the given program.
        /// </summary>
        /// <param name="program">The program whose leaf sub-programs we want to retrieve.</param>
        /// <returns>A set containing all the <see cref="ITreeProgram{TOutput}" /> sub-programs of the given program.</returns>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static IDictionary<ITreeProgram<TOutput>, uint> GetLeaves<TOutput>(this ITreeProgram<TOutput> program)
        {
            if (program == null) return null;
            var leaves = new Dictionary<ITreeProgram<TOutput>, uint>();
            GetLeaves(program, leaves);
            return leaves;
        }

        /// <summary>
        ///     Gets the maximum breadth of the program, i.e., the number of <see cref="ITreeProgram{TOutput}" /> leaves
        ///     encountered starting from this program.
        /// </summary>
        /// <param name="program">The root program to calculate the breadth.</param>
        /// <returns>The maximum breadth of the program.</returns>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static uint GetMaxBreadth<TOutput>(this ITreeProgram<TOutput> program)
        {
            return program == null
                ? 0
                : program.Input == null || program.Input.Count == 0
                    ? 1
                    : (uint) program.Input.Sum(child => child.GetMaxBreadth());
        }

        /// <summary>
        ///     Gets the maximum depth of the program, i.e., the maximum number of child programs encountered starting from this
        ///     program until reaching a <see cref="T:Terminal" /> program.
        /// </summary>
        /// <param name="program">The root program to calculate the depth.</param>
        /// <returns>The maximum depth of the program.</returns>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static uint GetMaxDepth<TOutput>(this ITreeProgram<TOutput> program)
        {
            return program?.Input == null || program.Input.Count == 0
                ? 0
                : 1 + program.Input.Max(child => child.GetMaxDepth());
        }

        /// <summary>
        ///     Gets a dictionary containing all the <see cref="ITreeProgram{TOutput}" /> primitives in the given program and their
        ///     count.
        /// </summary>
        /// <param name="program">The program whose terminal sub-programs we want to retrieve.</param>
        /// <returns>A set containing all the <see cref="ITreeProgram{TOutput}" /> primitives of the given program.</returns>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static IDictionary<ITreeProgram<TOutput>, uint> GetPrimitives<TOutput>(
            this ITreeProgram<TOutput> program)
        {
            if (program == null) return null;
            var primitives = new Dictionary<ITreeProgram<TOutput>, uint>();
            GetPrimitives(program, primitives);
            return primitives;
        }

        /// <summary>
        ///     Gets all the <see cref="ITreeProgram{TOutput}" /> sub-combinations of the given program. If the program is a leaf,
        ///     there are no combinations, otherwise returns all the possible combinations between the sub-programs of the children
        ///     and also the sub-combinations of each child.
        /// </summary>
        /// <param name="program">The program we want to get the sub-combinations.</param>
        /// <returns>All the sub-combinations of the given program.</returns>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static ISet<ITreeProgram<TOutput>> GetSubCombinations<TOutput>(this ITreeProgram<TOutput> program)
        {
            //remove program itself from sub-combinations
            var subCombs = GetSubCombs(program);
            subCombs.Remove(program);
            return subCombs;
        }

        /// <summary>
        ///     Gets a <see cref="ISet{T}" /> containing all the descendant <see cref="ITreeProgram{TOutput}" /> of the given
        ///     program.
        /// </summary>
        /// <param name="program">The program we want to get the sub-programs.</param>
        /// <returns>A set containing all the sub programs of the given program.</returns>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static ITreeProgram<TOutput>[] GetSubPrograms<TOutput>(this ITreeProgram<TOutput> program)
        {
            if (program == null) return null;
            var subProgs = new ITreeProgram<TOutput>[program.Length - 1];
            var index = 0;
            GetSubPrograms(program, ref index, program.Length - 1, subProgs);
            return subProgs;
        }

        /// <summary>
        ///     Checks whether the given <see cref="ITreeProgram{TOutput}" /> is a leaf node, i.e., it has no child nodes.
        /// </summary>
        /// <param name="program">The program we want to check if it is a leaf.</param>
        /// <returns><c>true</c> if the given program is a leaf, <c>false</c> otherwise.</returns>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static bool IsLeaf<TOutput>(this ITreeProgram<TOutput> program)
            => program.Input == null || program.Input.Count == 0;

        /// <summary>
        ///     Checks whether a given <see cref="ITreeProgram{TOutput}" /> is a sub-program of another
        ///     <see cref="ITreeProgram{TOutput}" />.
        ///     A sub-program is an program that is a descendant of a given program.
        /// </summary>
        /// <param name="program">The program we want to know if it is a sub-program.</param>
        /// <param name="other">The program for which to look for a descendant equal to the given program.</param>
        /// <returns><c>true</c>, if the program is a descendant of the other program, <c>false</c> otherwise.</returns>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static bool IsSubProgramOf<TOutput>(this ITreeProgram<TOutput> program, ITreeProgram<TOutput> other)
        {
            // checks prog
            if (program == null || other == null ||
                program.Length >= other.Length || other.Input?.Count == 0) return false;

            // search the descendants for the given program
            return other.Input != null &&
                   other.Input.Any(child => program.Equals(child) || program.IsSubProgramOf(child));
        }

        /// <summary>
        ///     Get the <see cref="ITreeProgram{TOutput}" /> at the given index, where programs are indexed in a zero-based, depth
        ///     first search manner.
        /// </summary>
        /// <returns>
        ///     The <see cref="ITreeProgram{TOutput}" /> at the given index, or <see langword="null" /> if the given index
        ///     is greater than or equal to the program's length.
        /// </returns>
        /// <param name="program">The root program to search for the child program at the given index.</param>
        /// <param name="index">The index of the program we want to search for.</param>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static ITreeProgram<TOutput> ProgramAt<TOutput>(this ITreeProgram<TOutput> program, uint index)
        {
            return ProgramAt(program, ref index);
        }

        /// <summary>
        ///     Gets a new copy of <paramref name="program" /> where the descendant program at the given index is replaced by
        ///     <paramref name="newSubProgram" />. Elements are indexed in a zero-based, depth first search manner.
        /// </summary>
        /// <param name="program">The root program to copy and search for the sub-program at the given index.</param>
        /// <param name="index">The index of the sub-program we want to replace.</param>
        /// <param name="newSubProgram">The new program to replace the one at the given index.</param>
        /// <returns>A copy of the program with the descendant at the given index replaced by the new program.</returns>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static ITreeProgram<TOutput> Replace<TOutput>(
            this ITreeProgram<TOutput> program, uint index, ITreeProgram<TOutput> newSubProgram)
        {
            return Replace(program, ref index, newSubProgram);
        }

        /// <summary>
        ///     Gets a new copy of <paramref name="program" /> where all sub-programs that are equal to
        ///     <paramref name="oldSubProgram" /> are replaced by <paramref name="newSubProgram" />.
        /// </summary>
        /// <param name="program">The root program to copy and search for the given sub-program .</param>
        /// <param name="oldSubProgram">The sub-program we want to replace.</param>
        /// <param name="newSubProgram">The new sub-program to replace the given one.</param>
        /// <returns>
        ///     A copy of the program with the given descendant replaced by the new program. If the given program is equal to the
        ///     sub-program we want to replace, then the replacement is returned. If the given sub-program is not found, a copy of
        ///     the original program is returned, or <c>null</c> if the program is <c>null</c>.
        /// </returns>
        /// <typeparam name="TOutput">The type of program output.</typeparam>
        public static ITreeProgram<TOutput> Replace<TOutput>(
            this ITreeProgram<TOutput> program, ITreeProgram<TOutput> oldSubProgram,
            ITreeProgram<TOutput> newSubProgram)
        {
            if (program == null || oldSubProgram == null || newSubProgram == null)
                return program;

            // checks if program is equal, return replacement
            if (program.Equals(oldSubProgram)) return newSubProgram;

            // replaces children recursively and creates a new program
            if (program.Input == null || program.Input.Count == 0) return program;
            var children = new ITreeProgram<TOutput>[program.Input.Count];
            for (var i = 0; i < program.Input.Count; i++)
                children[i] = Replace(program.Input[i], oldSubProgram, newSubProgram);
            return program.CreateNew(children);
        }

        #endregion

        #region Private & Protected Methods

        private static void GetCommonRegionIndexes<TOutput>(
            this ITreeProgram<TOutput> program, ITreeProgram<TOutput> otherProgram, IDictionary<uint, uint> indexes,
            ref uint idx1, ref uint idx2)
        {
            // add the corresponding (common region) indexes
            indexes.Add(idx1, idx2);

            // check if children differ (different sub-structure)
            if (program?.Input == null || otherProgram?.Input == null ||
                program.Input.Count != otherProgram.Input.Count)
            {
                // just advance the indexes of both sub-trees
                if (program != null) idx1 += (uint) program.Length - 1;
                if (otherProgram != null) idx2 += (uint) otherProgram.Length - 1;
                return;
            }

            // programs have same number of children, iterate recursively
            for (var i = 0; i < program.Input.Count; i++)
            {
                idx1++;
                idx2++;
                GetCommonRegionIndexes(program.Input[i], otherProgram.Input[i], indexes, ref idx1,
                    ref idx2);
            }
        }

        private static void GetLeaves<TOutput>(
            this ITreeProgram<TOutput> program, IDictionary<ITreeProgram<TOutput>, uint> leaves)
        {
            // checks program is leaf, add to set and update count
            if (program.Input == null || program.Input.Count == 0)
            {
                var leaf = program.CreateNew(new List<ITreeProgram<TOutput>>());
                if (!leaves.ContainsKey(leaf)) leaves.Add(leaf, 0);
                leaves[leaf]++;
                return;
            }

            // searches children 
            if (program.Input != null)
                foreach (var child in program.Input)
                    GetLeaves(child, leaves);
        }

        private static void GetPrimitives<TOutput>(
            this ITreeProgram<TOutput> program, IDictionary<ITreeProgram<TOutput>, uint> primitives)
        {
            // checks count table
            var prog = program.GetPrimitive();
            if (!primitives.ContainsKey(prog)) primitives.Add(prog, 0);
            primitives[prog]++;

            // searches children
            if (program.Input != null)
                foreach (var child in program.Input)
                    GetPrimitives(child, primitives);
        }

        private static ISet<ITreeProgram<TOutput>> GetSubCombs<TOutput>(ITreeProgram<TOutput> program)
        {
            var combs = new HashSet<ITreeProgram<TOutput>>();
            if (program == null) return combs;

            // checks no more children
            if (program.IsLeaf())
            {
                combs.Add(program);
                return combs;
            }

            // gets sub-programs from all children
            var childrenSubCombs = new List<IEnumerable<ITreeProgram<TOutput>>>();
            foreach (var child in program.Input)
            {
                var childSubCombs = GetSubCombs(child);
                childrenSubCombs.Add(childSubCombs);

                // adds the sub-combinations of children to combination list
                foreach (var childSubComb in childSubCombs) combs.Add(childSubComb);
            }

            // creates new programs where each child is replaced by some sub-combination of it
            var allChildrenCombs = childrenSubCombs.GetAllCombinations();
            foreach (var children in allChildrenCombs)
                if (children.Count == program.Input.Count)
                    combs.Add(program.CreateNew(children));

            childrenSubCombs.Clear();

            return combs;
        }

        private static void GetSubPrograms<TOutput>(
            ITreeProgram<TOutput> program, ref int index, int maxIdx, IList<ITreeProgram<TOutput>> subProgs)
        {
            if (index > maxIdx)
                return;

            if (index > 0) subProgs[index - 1] = program;
            if (program.Input == null) return;
            foreach (var child in program.Input)
            {
                index++;
                GetSubPrograms(child, ref index, maxIdx, subProgs);
            }
        }

        private static ITreeProgram<TOutput> ProgramAt<TOutput>(this ITreeProgram<TOutput> program, ref uint index)
        {
            if (index == 0) return program;
            if (program?.Input == null) return default(ITreeProgram<TOutput>);
            if (index >= program.Length)
            {
                index -= (uint) program.Length - 1;
                return default(ITreeProgram<TOutput>);
            }

            foreach (var child in program.Input)
            {
                index--;
                var prog = child.ProgramAt(ref index);
                if (prog != null || index == 0)
                    return prog;
            }

            return default(ITreeProgram<TOutput>);
        }

        private static ITreeProgram<TOutput> Replace<TOutput>(
            this ITreeProgram<TOutput> program, ref uint index, ITreeProgram<TOutput> newSubProgram)
        {
            if (program == null) return default(ITreeProgram<TOutput>);
            if (index == 0) return newSubProgram;
            if (program.Input == null) return program;

            var newChildren = program.Input.ToArray();
            for (var i = 0; i < program.Input.Count; i++)
            {
                index--;
                newChildren[i] = Replace(program.Input[i], ref index, newSubProgram);
                if (index == 0)
                    break;
            }

            return program.CreateNew(newChildren);
        }

        #endregion
    }
}