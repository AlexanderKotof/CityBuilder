using System.Collections.Generic;

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
    }
}