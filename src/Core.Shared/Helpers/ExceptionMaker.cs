using Core.Shared.Exceptions;
using System;

namespace Core.Shared.Helpers
{
    /// <summary>
    /// Class preparing and throwing Exceptions with specified messages and info.
    /// </summary>
    public static class ExceptionMaker
    {
        /// <summary>
        /// Throwing exception when arguments of methods are invalid.
        /// </summary>
        public static class Argument
        {
            /// <summary>
            /// Throw <see cref="ArgumentException"/> when <paramref name="value"/> is not supported or cannot be handled.
            /// </summary>
            /// <typeparam name="T">Type of the value</typeparam>
            /// <param name="value">Unhandled value</param>
            /// <param name="argumentName">Name of the argument holding <paramref name="value"/></param>
            /// <returns>Prepared exception for throwing</returns>
            public static ArgumentException Unsupported<T>(T value, string argumentName) =>
                new ($"The argument's value {value} of type {nameof(T)} is not supported.", argumentName);
        }

        /// <summary>
        /// Throwing exception when a member of a class is invalid.
        /// </summary>
        public static class Member
        {
            /// <summary>
            /// Throw <see cref="InvalidMemberException"/> when <paramref name="value"/> is not supported or cannot be handled.
            /// </summary>
            /// <typeparam name="T">Type of the value</typeparam>
            /// <param name="value">Unhandled value</param>
            /// <param name="memberName">Name of the argument holding <paramref name="value"/></param>
            /// <returns>Prepared exception for throwing</returns>
            public static InvalidMemberException Invalid<T>(T value, string memberName) =>
                new($"The {memberName} member's value {value} of type {nameof(T)} is not supported.");

            /// <summary>
            /// Throw <see cref="InvalidMemberException"/> when <paramref name="value"/> is null and cannot be handled this way.
            /// </summary>
            /// <typeparam name="T">Type of the value</typeparam>
            /// <param name="value">Unhandled value</param>
            /// <param name="memberName">Name of the argument holding <paramref name="value"/></param>
            /// <returns>Prepared exception for throwing</returns>
            public static InvalidMemberException IsNull<T>(T value, string memberName) =>
                new($"The {memberName} member's value of type {nameof(T)} is null and cannot be handled this way.");
        }

        /// <summary>
        /// Throwing exception with a specific information when something was not implemented.
        /// </summary>
        public static class NotImplemented
        {
            /// <summary>
            /// Throw <see cref="NotImplementedException"/> when value was not handled and there must be a trace about it.
            /// </summary>
            /// <typeparam name="T">Type of the variable</typeparam>
            /// <param name="value">Unhandled value</param>
            /// <param name="variableName">Name of the variable holding <paramref name="value"/></param>
            /// <returns>Prepared exception for throwing</returns>
            public static NotImplementedException ForHandling<T>(T value, string variableName) =>
                new NotImplementedException($"Handling of the value {variableName} of type {nameof(T)} has not been implemented.");
        }
    }
}
