using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LnxArch
{
    public static class CollectionParameterCaster
    {
        private static readonly MethodInfo _enumerableCastToListMethod = typeof(CollectionParameterCaster)
            .GetMethod(nameof(EnumerableCastToList), BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo _enumerableCastToArrayMethod = typeof(CollectionParameterCaster)
            .GetMethod(nameof(EnumerableCastToArray), BindingFlags.NonPublic | BindingFlags.Static);

        public static object ForceCastToList<T>(Type targetType, IEnumerable<T> enumerable)
        {
            MethodInfo castMethod =_enumerableCastToListMethod
            .MakeGenericMethod(typeof(T), targetType);
            return castMethod.Invoke(null, new object[] { enumerable });
        }
        public static object ForceCastToArray<T>(Type targetType, IEnumerable<T> enumerable)
        {
            MethodInfo castMethod = _enumerableCastToArrayMethod
            .MakeGenericMethod(typeof(T), targetType);
            return castMethod.Invoke(null, new object[] { enumerable });
        }

        private static List<K> EnumerableCastToList<T, K>(IEnumerable<T> enumerable)
        {
            return enumerable.Cast<K>().ToList();
        }
        private static K[] EnumerableCastToArray<T, K>(IEnumerable<T> enumerable)
        {
            return enumerable.Cast<K>().ToArray();
        }
    }
}
