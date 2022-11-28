using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace StockPOS.CustomTokenAuthProvider
{
    #region snippet1
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenProviderMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenProviderMiddleware>();
        }
    }
    #endregion
}
