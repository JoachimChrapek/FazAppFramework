using System;
using System.Collections.Generic;
using System.Linq;

namespace FazAppFramework.Extensions
{
    public static class CollectionsExtensions
    {
        public static T GetRandom<T>(this IList<T> list)
        {
            var rand = UnityEngine.Random.Range(0, list.Count);
            return list[rand];
        }

        public static void Swap<T>(this IList<T> list, int firstIndex, int secondIndex)
        {
            if (firstIndex < 0 || firstIndex >= list.Count)
            {
                throw new Exception($"Swap in list function: First index [{firstIndex}] is out of range. Total Count: {list.Count}");
            }
            if (secondIndex < 0 || secondIndex >= list.Count)
            {
                throw new Exception($"Swap in list function: Second index [{secondIndex}] is out of range. Total Count: {list.Count}");
            }

            if(firstIndex == secondIndex)
                return;

            var tmp = list[firstIndex];
            list[firstIndex] = list[secondIndex];
            list[secondIndex] = tmp;
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            Random rnd = new Random();
            return source.OrderBy(item => rnd.Next());
        }

        public static void AddToArray<T>(this T[] array, T objectToAdd)
        {
            var tmp = array.ToList();
            tmp.Add(objectToAdd);
            array = tmp.ToArray();
        }

        public static void RemoveAtFromArray<T>(this T[] array, int index)
        {
            var tmp = array.ToList();
            tmp.RemoveAt(index);
            array = tmp.ToArray();
        }
    }
}
