using TrzyszczCMS.Core.Shared.Exceptions;
using System;

namespace TrzyszczCMS.Core.Shared.Helpers
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
            /// Throw <see cref="ArgumentException"/> when <paramref name="value"/> is not supported (implemented)
            /// or <paramref name="value"/> cannot be handled because if invalid.
            /// </summary>
            /// <typeparam name="T">Type of the value</typeparam>
            /// <param name="value">Unhandled value</param>
            /// <param name="argumentName">Name of the argument holding <paramref name="value"/></param>
            /// <returns>Prepared exception for throwing</returns>
            public static ArgumentException Unsupported<T>(T value, string argumentName) =>
                new ($"The argument's value {value} of type {typeof(T).Name} is not supported or it is invalid.", argumentName);

            /// <summary>
            /// Throw <see cref="ArgumentException"/> when <paramref name="value"/> is invalid for proper handling.
            /// </summary>
            /// <typeparam name="T">Type of the value</typeparam>
            /// <param name="value">Invalid value</param>
            /// <param name="argumentName">Name of the argument holding <paramref name="value"/></param>
            /// <returns>Prepared exception for throwing</returns>
            public static ArgumentException Invalid<T>(T value, string argumentName) =>
                new($"The specific argument's value {value} of type {typeof(T).Name} is invalid and cannot be handled properly.", argumentName);
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
                new($"The {memberName} member's specific value {value} of type {typeof(T).Name} is invalid and cannot be handled properly.");

            /// <summary>
            /// Throw <see cref="InvalidMemberException"/> when <paramref name="value"/> is null and cannot be handled this way.
            /// </summary>
            /// <typeparam name="T">Type of the value</typeparam>
            /// <param name="value">Unhandled value</param>
            /// <param name="memberName">Name of the argument holding <paramref name="value"/></param>
            /// <returns>Prepared exception for throwing</returns>
            public static InvalidMemberException IsNull<T>(T value, string memberName) =>
                new($"The {memberName} member's value of type {typeof(T).Name} is null and cannot be handled this way.");

            /// <summary>
            /// Throw <see cref="InvalidMemberException"/> when <paramref name="value"/> is not supported (implemented)
            /// or <paramref name="value"/> cannot be handled because if invalid.
            /// </summary>
            /// <typeparam name="T">Type of the value</typeparam>
            /// <param name="value">Unhandled value</param>
            /// <param name="memberName">Name of the member holding <paramref name="value"/></param>
            /// <returns>Prepared exception for throwing</returns>
            public static InvalidMemberException Unsupported<T>(T value, string memberName) =>
                new($"The {memberName} member's value {value} of type {typeof(T).Name} is not supported or it is invalid.");
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
                new NotImplementedException($"Handling of the value {variableName} of type {typeof(T).Name} has not been implemented.");
        }
    }
}
