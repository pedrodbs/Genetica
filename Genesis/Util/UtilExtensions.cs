// ------------------------------------------
// <copyright file="UtilExtensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/03/03
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Random;

namespace Genesis.Util
{
    public static class UtilExtensions
    {
        #region Static Fields & Constants

        private static readonly Random Random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the given values to the given set.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collections.</typeparam>
        /// <param name="set">The set we want to add the values to.</param>
        /// <param name="values">The values to be added to the set.</param>
        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> values)
        {
            foreach (var val in values) set.Add(val);
        }

        /// <summary>
        ///     Gets an IList representing all combinations of <typeparamref name="T" /> items from the provided lists.
        ///     Combinations are created in the following form: [item from the 1st list, item from the 2nd list , ..., item
        ///     from the nth list].
        /// </summary>
        /// <returns>A list containing all combinations of items.</returns>
        /// <param name="list">The list containing the lists of items to combine.</param>
        /// <typeparam name="T">The type of elements in the lists.</typeparam>
        public static IEnumerable<IList<T>> GetAllCombinations<T>(this IList<IEnumerable<T>> list)
        {
            var combinations = new List<IList<T>> {new T[list.Count]};
            for (var i = 0; i < list.Count; i++)
            {
                var newCombs = new List<IList<T>>();
                foreach (var item in list[i])
                foreach (var combination in combinations)
                {
                    var newComb = combination.ToArray();
                    newComb[i] = item;
                    newCombs.Add(newComb);
                }
                combinations = newCombs;
            }
            return combinations;
        }

        /// <summary>
        ///     Gets a random item from the list.
        /// </summary>
        /// <returns>The random item.</returns>
        /// <param name="list">The list we want to get a random item from.</param>
        /// <param name="random">The random number generator to be used in the item selection.</param>
        /// <typeparam name="T">The type of items stored in the list.</typeparam>
        public static T GetRandomItem<T>(this IList<T> list, Random random)
        {
            return list[random.Next(list.Count)];
        }

        public static T GetRandomItem<T>(this IList<T> list)
        {
            return GetRandomItem(list, Random);
        }

        /// <summary>
        ///     Gets an item from the given dictionary keys, randomly sampled according to the probabilities defined by the
        ///     corresponding values in the dictionary.
        /// </summary>
        /// <returns>The random item.</returns>
        /// <param name="dict">
        ///     The dictionary we want to get a random item from. Assumes <paramref name="dict" />.Values.
        ///     Sum() == 1.
        /// </param>
        /// <param name="random">The random source used in this operation.</param>
        /// <typeparam name="T">The type of items stored in the dictionary keys.</typeparam>
        public static T GetRandomItem<T>(this IDictionary<T, double> dict, Random random)
        {
            var rnd = random.NextDouble();
            var sum = 0d;
            foreach (var item in dict.Keys)
            {
                sum += dict[item];
                if (sum > rnd) return item;
            }
            return dict.Count > 0 ? dict.Keys.First() : default(T);
        }

        public static T GetRandomItem<T>(this IDictionary<T, double> dict)
        {
            return GetRandomItem(dict, Random);
        }

        #endregion
    }
}