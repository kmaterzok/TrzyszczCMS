namespace Core.Shared.Models
{
    /// <summary>
    /// A simple struct for storing ranges of values for comparison or return.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public struct ValueRange<T>
    {
        /// <summary>
        /// Value that starts the range
        /// </summary>
        public T Start { get; set; }
        /// <summary>
        /// Value that ends the range
        /// </summary>
        public T End { get; set; }
    }
}
