using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoTools.Helpers
{
    /// <summary>
    /// Simple class to wrap around Type, so that .ToString() returns the Name of the type and
    /// not the fullname
    /// </summary>
    public class TypeWrapper
    {
        /// <summary>
        /// Method wrap a collection of Types to a Collecion of TypeWrapper
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IEnumerable<TypeWrapper> Wrap(IEnumerable<Type> types)
        {
            return types.Select(type => new TypeWrapper(type)).ToList();
        }

        private readonly Type _type;
        public TypeWrapper(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// Gets the Type
        /// </summary>
        public Type Type
        {
            get
            {
                return _type;
            }
        }
        
        /// <summary>
        /// Creates an instance of the Type
        /// </summary>
        /// <typeparam name="T">Returned Type</typeparam>
        /// <param name="args">constructor arguments</param>
        /// <returns></returns>
        public T Instance<T>(params object[] args)
        {
            return (T)Activator.CreateInstance(Type, args);
        }
        public override string ToString()
        {
            return _type.Name;
        }

        
    }
}
