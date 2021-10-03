using System;

namespace TrzyszczCMS.Client.Other
{
    /// <summary>
    /// The exception raised when something wrong occurs in the running project.
    /// Used for distinguishing local code from the remote one.
    /// </summary>
    public class ApplicationException : Exception
    {
        /// <summary>
        /// Create an exception.
        /// </summary>
        /// <param name="message">Message with information.</param>
        public ApplicationException(string message) : base(message)
        {
            // Do nothing
        }
    }
}
