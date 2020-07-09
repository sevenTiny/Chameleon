using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Infrastructure.Extensions
{
    public static class CollectionsExtension
    {
        public static List<List<T>> SplitRange<T>(this IEnumerable<T> list, int pageSize)
        {
            int pageCount = (int)Math.Ceiling(list.Count() / (double)pageSize);

            List<List<T>> result = new List<List<T>>(pageCount);

            for (int pageNum = 0; pageNum < pageCount; pageNum++)
            {
                result.Add(list.Skip(pageNum * pageSize).Take(pageSize).ToList());
            }

            return result;
        }
    }
}
