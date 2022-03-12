using System.Collections.Generic;

namespace TrzyszczCMS.Client.Data.Enums
{
    /// <summary>
    /// Method of predicate compliance check for items of <see cref="IEnumerable{T}"/>.
    /// </summary>
    public enum EnumerableItemsComplianceCheckMethod
    {
        /// <summary>
        /// Require any of the collection items to be compliant with the predicate.
        /// </summary>
        Any,
        /// <summary>
        /// Require all of the collection items to be compliant with the predicate.
        /// </summary>
        All
    }
}
