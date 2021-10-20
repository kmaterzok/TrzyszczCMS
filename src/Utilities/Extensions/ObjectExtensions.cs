namespace Utilities.Extensions
{
    /// <summary>
    /// The class containing additional helper methods to ease usage of objects
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Is the <paramref name="source"/> object castable to <typeparamref name="TOut"/> type?
        /// If it is then return it by <paramref name="outObject"/>.
        /// </summary>
        /// <typeparam name="TFrom">Source type of the casted object</typeparam>
        /// <typeparam name="TOut">Desired type of the casted object</typeparam>
        /// <param name="source">T casted object</param>
        /// <param name="outObject">The object after casting</param>
        /// <returns>Was casting successful</returns>
        public static bool As<TFrom, TOut>(this TFrom source, out TOut outObject) where TOut : class
        {
            outObject = source as TOut;
            return outObject != null;
        }
    }
}
