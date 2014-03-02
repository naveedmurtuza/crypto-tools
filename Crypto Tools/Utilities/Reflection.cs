using System;
using System.Collections.Generic;
using System.Linq;
using CryptoTools.Helpers;

namespace CryptoTools.Utilities
{
    public class Reflection
    {
        /// <summary>
        /// Finds all implementations of the given Interface
        /// </summary>
        /// <typeparam name="T">Implementations of Interface to find</typeparam>
        /// <returns>a collection of all implementations found wrapped in TypeWrapper</returns>
        public static IEnumerable<TypeWrapper> FindImplementations<T>(params Type[] args)
        {
            if (args == null)
                args = Type.EmptyTypes;

            Type type = typeof (T);
            IEnumerable<TypeWrapper> types = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract).Select(p => new TypeWrapper(p));

            return types.Where(p => p.Type.GetConstructor(args) != null);
        }
    }
}