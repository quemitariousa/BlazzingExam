using Microsoft.AspNetCore.Builder;

namespace BlazzingExam.Core.Server.Security.Middlewares.Extensions
{
    public static class UpdateIdentityMiddlewareExtensions
    {
        public static IApplicationBuilder UseIdentityUpdater(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UpdateIdentityMiddleware>();
        }
    }
}