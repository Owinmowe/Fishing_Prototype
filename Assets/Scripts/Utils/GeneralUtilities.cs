using System.Collections.Generic;
using UnityEngine;

namespace FishingPrototype.Utils
{
    public static class GeneralUtilities
    {
        public static void ShuffleList<T> (List<T> list)
        {
            int count = list.Count;
            int last = count - 1;
            for (int i = 0; i < last; ++i)
            {
                int r = Random.Range(i, count);
                (list[i], list[r]) = (list[r], list[i]);
            }
        }
    }
}