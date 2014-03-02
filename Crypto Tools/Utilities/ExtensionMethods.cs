using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CryptoTools.Utilities
{
    public static class ExtensionMethods
    {
        public const String TheEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                              + "@"
                                              + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

        /// <summary>
        /// Checks wether the string is a valid email string.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this String email)
        {
            return Regex.IsMatch(email, TheEmailPattern);
        }

        /// <summary>
        /// Just convienence method for String.IsNullOrEmpty
        /// 
        /// </summary>
        /// <param name="value">The string to test. </param>
        /// <returns>
        /// true if the <paramref name="value"/> parameter is null or an empty string (""); otherwise, false.
        /// </returns>
        public static bool IsNullOrEmpty(this String value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static void AddRange<T>(this ObservableCollection<T> coll, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                coll.Add(item);
            }
        }

        #region Bit Operations on IEnumearable<int>

        /// <summary>
        /// Computes Or of the sequence of int values that are obtained by invoking a transform function on each element of the input sequence.
        /// Looked into mono sources for the implementation of Sum, modified it to Or
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Or of the sequence of int values</returns>
        public static int Or(this IEnumerable<int> source)
        {
            int total = 0;

            foreach (int element in source)
                total = checked(total | element);
            return total;
        }

        /// <summary>
        /// Computes And of the sequence of int values that are obtained by invoking a transform function on each element of the input sequence.
        /// Looked into mono sources for the implementation of Sum, modified it to And
        /// </summary>
        /// <param name="source"></param>
        /// <returns>And of the sequence of int values</returns>
        public static int And(this IEnumerable<int> source)
        {
            int total = 0;

            foreach (int element in source)
                total = checked(total & element);
            return total;
        }

        /// <summary>
        /// Computes Xor of the sequence of int values that are obtained by invoking a transform function on each element of the input sequence.
        /// Looked into mono sources for the implementation of Sum, modified it to Xor
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Xor of the sequence of int values</returns>
        public static int Xor(this IEnumerable<int> source)
        {
            int total = 0;

            foreach (int element in source)
                total = checked(total ^ element);
            return total;
        }

        #endregion
    }
}