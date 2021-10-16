using Core.Application.Services.Interfaces;
using Core.Shared.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TrzyszczCMS.Client.Other
{
    /// <summary>
    /// Handling of access tokens.
    /// </summary>
    /// <seealso cref="DelegatingHandler" />
    public class TokenHeaderHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Instantiates a new class of <see cref="TokenHeaderHandler"/>.
        /// </summary>
        /// <param name="tokenService">The token service</param>
        public TokenHeaderHandler(ITokenService tokenService)
        {
            this._tokenService = tokenService;
        }

        /// <summary>
        /// Sends an HTTP request to the internal procedure in order to send to the server as an async operation.
        /// </summary>
        /// <param name="request">HTTP resuest sent to the server</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        /// <returns>
        /// The response object representing an async operation
        /// </returns>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(CommonConstants.HEADER_AUTHORIZATION_NAME))
            {
                var token = await _tokenService.GetTokenAsync();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Add(CommonConstants.HEADER_AUTHORIZATION_NAME, token);
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
