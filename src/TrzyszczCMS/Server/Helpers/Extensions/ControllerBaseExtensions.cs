using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TrzyszczCMS.Server.Helpers.Extensions
{
    /// <summary>
    /// The helper methods executed from inside of classes that inherit <see cref="ControllerBase"/> class.
    /// </summary>
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// Create an object as a response with code 201 (Created).
        /// </summary>
        /// <param name="_">Controller instance that invokes this method</param>
        /// <param name="payload">Returned value in the response</param>
        /// <returns>Result object with payload</returns>
        public static ObjectResult ObjectCreated(this ControllerBase _, object payload) =>
            new ObjectResult(payload) { StatusCode = (int)HttpStatusCode.Created };

        /// <summary>
        /// Create an object as a response with code 201 (Created).
        /// </summary>
        /// <param name="_">Controller instance that invokes this method</param>
        /// <returns>Result object with no payload</returns>
        public static ObjectResult ObjectCreated(this ControllerBase _) => ObjectCreated(_, null);
    }
}
