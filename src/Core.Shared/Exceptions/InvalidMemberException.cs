using System;

namespace TrzyszczCMS.Core.Shared.Exceptions
{
    /// <summary>
    /// The exception thrown when a member of a class does not have a valid value and it is noticed by a checker method.
    /// </summary>
    public sealed class InvalidMemberException : Exception
    {
        /// <summary>
        /// Create an exception.
        /// </summary>
        /// <param name="message">Message with information.</param>
        public InvalidMemberException(string message) : base(message)
        {
            // Do nothing
        }
    }
}
