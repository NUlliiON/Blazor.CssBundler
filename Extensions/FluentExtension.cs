using System;

namespace Blazor.CssBundler.Extensions
{
    static class FluentExtension
    {
        /// <summary>
        /// Fluent helper for easy set property values in object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="act"></param>
        public static void Set<T>(this T obj, Action<T> act)
        {
            act.Invoke(obj);
        }
    }
}
