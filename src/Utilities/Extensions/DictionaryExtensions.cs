using System.Collections.Generic;

namespace Utilities.Extensions
{
    /// <summary>
    /// Extension methods for easy handling Dictionaries
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Add a value to the <paramref name="source"/> if not inserted earlier. Key exists -> the value is updated.
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="source">Handled dictionary</param>
        /// <param name="key">Key value</param>
        /// <param name="value">Value assigned to the key</param>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }
        }



        /// <summary>
        /// Get the value by the <paramref name="key"/>. If does not exist then return coalesce <paramref name="defaultValue"/>.
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="source">Handled dictionary</param>
        /// <param name="key">Key value</param>
        /// <param name="defaultValue">Default value if key value was not provided into the Dictionary</param>
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            try
            {
                return source.TryGetValue(key, out TValue outValue) ? outValue : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

    }
}
