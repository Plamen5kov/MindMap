using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MindMap.Helpers.Extensions
{
    public static class CustonExtensions
    {
        public static void AddRange<T>(this ICollection<T> originalCollection, ICollection<T> newItems)
        {
            foreach (var item in newItems)
            {
                originalCollection.Add(item);
            }
        }
    }
}
