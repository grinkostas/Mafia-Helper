using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class Wrapper<T>
{
    public T[] Items;

    public Wrapper()
    {
        Items = Array.Empty<T>();
    }

    public Wrapper(T[] items)
    {
        Items = items;
    }

    public List<T> ToList()
    {
        if (Items.Length == 0)
        {
            return new List<T>();
        }
        return Items.ToList();
    }
}