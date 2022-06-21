using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    private static Random _random = new Random(); 
    public static void Add<T>(this List<T> list, T element, int count)
    {
        for (int i = 0; i < count; i++)
        {
            list.Add(element);
        }
    }
    
    public static void Shuffle<T>(this List<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int tempIndex = _random.Next(n + 1);  
            (list[tempIndex], list[n]) = (list[n], list[tempIndex]);
        }  
    }

    public static void FirstToBack<T>(this List<T> list, int count)
    {
        if(count <= 0)
            return;

        if (count > list.Count)
        {
            count -= (count / list.Count * list.Count);
        }
        
        var slice = list.Take(count).ToList();
        list.RemoveRange(0, count);
        list.AddRange(slice);
    }
}
