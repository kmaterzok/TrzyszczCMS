using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Shared.Models
{
    /// <summary>
    /// The result of executing an operation
    /// </summary>
    /// <typeparam name="T">The object to process in case of success</typeparam>
    /// <typeparam name="E">The object to process in case of failure</typeparam>
    public class Result<T, E>
        where T : class
        where E : class
    {
        #region Fields
        /// <summary>
        /// The object set for properly done operation
        /// </summary>
        private readonly T _successObject;
        /// <summary>
        /// The object set for improperly done operation
        /// </summary>
        private readonly E _errorObject;
        #endregion

        #region Ctor
        private Result(T successObject, E errorObject)
        {
            if (!(successObject == null ^ errorObject == null))
            {
                throw new ArgumentException("Both arguments are null or have a reference.",
                    $"{nameof(successObject)} or {nameof(errorObject)}");
            }

            this._successObject = successObject;
            this._errorObject = errorObject;
        }
        #endregion

        #region Factory methods
        /// <summary>
        /// Create a result of successful executing of operation.
        /// </summary>
        /// <param name="successObject">Object for processing</param>
        /// <returns>Object with result information</returns>
        public static Result<T, E> MakeSuccess([NotNull] T successObject) =>
            new Result<T, E>(successObject, null);
        
        /// <summary>
        /// Create a result of faulty executing of operation.
        /// </summary>
        /// <param name="successObject">Object for processing</param>
        /// <returns>Object with result information</returns>
        public static Result<T, E> MakeError([NotNull] E errorObject) =>
            new Result<T, E>(null, errorObject);
        #endregion

        #region Public methods
        /// <summary>
        /// Get object for processing and determine if the data applies to a failure
        /// </summary>
        /// <param name="successObject">The object returned for successful execution of operation</param>
        /// <param name="errorObject">The object returned for faulty execution of operation</param>
        /// <returns>Indicates whether the operation was finished successfully</returns>
        public bool GetValue(out T successObject, out E errorObject)
        {
            successObject = this._successObject;
            errorObject = this._errorObject;
            return this._successObject != null;
        }
        #endregion
    }
}
