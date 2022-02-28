using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TrzyszczCMS.Server.Helpers.Extensions
{
    /// <summary>
    /// Eased usage of reflection mechanism.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Return all constants of the <paramref name="type"/>.
        /// </summary>
        /// <typeparam name="T">Type of the constants' value</typeparam>
        /// <param name="type">The enumerated type</param>
        /// <returns>Dictionary of constants' information - name and value</returns>
        public static Dictionary<string, T> GetConstants<T>(this Type type)
        {
            var properties = type.GetFields(BindingFlags.Public |
                                            BindingFlags.Static |
                                            BindingFlags.FlattenHierarchy);

            return properties.Where(i => i.IsLiteral && !i.IsInitOnly)
                             .ToDictionary(i => i.Name, i => (T)i.GetValue(null));
        }
    }
}
