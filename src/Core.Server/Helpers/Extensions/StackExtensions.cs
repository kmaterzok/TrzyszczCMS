using System.Collections.Generic;

namespace TrzyszczCMS.Core.Server.Helpers.Extensions
{
    /// <summary>
    /// The extension methods easing writing code and usage of structures.
    /// </summary>
    public static class StackExtensions
    {
        /// <summary>
        /// Push a range of elements to a stack. Enumeration of range is done by a foreach loop.
        /// </summary>
        /// <typeparam name="T">Type of the elements stored in <paramref name="range"/></typeparam>
        /// <param name="source"></param>
        /// <param name="range">Range of added elements</param>
        public static void PushRange<T>(this Stack<T> source, IEnumerable<T> range)
        {
            foreach(var item in range)
            {
                source.Push(item);
            }
        }
    }
}
