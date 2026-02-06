using System;
using System.Collections.Generic;
using System.Linq;

namespace CityBuilder.Utilities.Extensions
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

        public static ICollection<T> RemoveManyWhere<T>(this ICollection<T> collection, int amountToRemove,
            Func<T, bool> predicate)
        {
            for (int i = 0; i < amountToRemove; i++)
            {
                var elementAt = collection.ElementAt(0);
                if (predicate(elementAt))
                    collection.Remove(elementAt);
            }
            
            return collection;
        }
        
        public static IEnumerable<T> Take<T>(this IReadOnlyCollection<T> collection, int amount,
            Func<T, bool> predicate)
        {
            var taken = 0;
            for (int i = 0; i < collection.Count; i++)
            {
                var elementAt = collection.ElementAt(i);
                if (predicate(elementAt))
                {
                    taken++;
                    yield return elementAt;
                }

                if (taken >= amount)
                {
                    yield break;
                }
            }
        }
    }
}