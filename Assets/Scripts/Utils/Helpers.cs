using System;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Helpers
{
    public static T FindInArrayByName<T>(T[] array, string name) where T : Object
    {
        return Array.Find(array, gameObject => gameObject.name == name);
    }

    public static IEnumerable<int> RandomNumberWithoutDuplicate(int to, int numberOfElement, int from = 0)
    {
        var numbers = new HashSet<int>();
        while (numbers.Count < numberOfElement)
        {
            numbers.Add(Random.Range(from, to));
        }

        return numbers.ToArray();
    }

    public static bool GenerateOneOrZeroBool() => Random.Range(0, 2) != 0;
}