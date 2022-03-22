namespace TrzyszczCMS.Core.Application.Helpers
{
    /// <summary>
    /// Methods for easing management of number types.
    /// </summary>
    public static class NumberHelper
    {
        /// <summary>
        /// Return <paramref name="value"/> or <paramref name="max"/> when this value is exceeded.
        /// </summary>
        /// <param name="value">Returned and checked value</param>
        /// <param name="max">Max available value to return</param>
        /// <returns>Passed value</returns>
        public static int ValueOrMax(int value, int max) => value > max ? max : value;
    }
}
