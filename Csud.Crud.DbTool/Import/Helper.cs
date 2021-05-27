using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Csud.Crud.DbTool.Import
{
    internal static class Helper
    {
        internal static IEnumerable<T> Traverse<T>(this T item, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(new[] { item });
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                {
                    stack.Push(child);
                }
            }
        }

        internal static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        internal static IEnumerable<XElement> GetItems(this XElement node, string type, string guid = "") => node.Elements()
            .Where(a => a.Name == type && (guid == "" || a.Guid() == guid));

        internal static XElement GetItem(this XElement node, string type, string guid) =>
            node.GetItems(type, guid).First();

        internal static string Guid(this XElement el) => el.Attribute("Guid")?.Value;

        internal static IEnumerable<XElement> Expand(this XElement rootNode, XElement node, string link, string type)
        {
            var links = node.GetItems(link);
            var nodes = rootNode.GetItems(type);
            nodes = nodes.Where(a => links.Any(b => b.Value == a.Guid()));
            return nodes;
        }

        internal static string ExtractArgument(this string value, string arg, bool takeAll)
        {
            arg = arg.ToLowerInvariant().Trim();
            var p = value.Split(';');
            foreach (var q in p)
            {
                var z = q.Split('=');
                if (z[0].ToLower().ToLowerInvariant().Trim() == arg)
                {
                    return z[1];
                }
            }
            return takeAll ? value : "";
        }

        internal static int? ExtractArgumentInt(this string value, string arg, bool takeAll)
        {
            var val = ExtractArgument(value, arg, takeAll);
            if (string.IsNullOrEmpty(val))
                return null;
            if (int.TryParse(val, out var intVal))
                return intVal;
            return null;
        }
  

        internal static string ExtractArgument(this string value, int index)
        {
            var p = value.Split(';');
            if (p.Length == 1)
                return "";
            if (index < 0)
                index = p.Length + index;
            if (index < 0 || index > p.Length - 1) return "";
            var res = p[index];
            res = res.Replace(">>", "").Replace("<<", "")
                .Replace("&lt", "").Replace("&gt;", "");
            return res;
        }
    }
}
