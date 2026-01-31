using System;
using System.Collections.Generic;
using System.Linq;

namespace GameSystems.Implementation.BattleSystem
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> AppendMany<T>(this IEnumerable<T> enumerable, IEnumerable<T> other)
        {
            foreach (var value in enumerable)
            {
                yield return value;
            }

            foreach (var value in other)
            {
                yield return value;
            }
        }

        public static ICollection<T> RemoveMany<T>(this ICollection<T> collection, IEnumerable<T> other)
        {
            foreach (var value in other)
            {
                collection.Remove(value);
            }
            return collection;
        }
        
        public static IDictionary<TKey, TValue> RemoveMany<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> other)
        {
            foreach (var value in other)
            {
                dictionary.Remove(value);
            }
            return dictionary;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            var forEach = enumerable as T[] ?? enumerable.ToArray();
            foreach (var value in forEach)
            {
                action(value);
            }
            return forEach;
        }
    }
}