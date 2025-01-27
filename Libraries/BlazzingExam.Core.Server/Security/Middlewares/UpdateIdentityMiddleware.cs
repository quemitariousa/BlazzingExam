﻿using System.Threading.Tasks;
using BlazzingExam.Core.Server.ServerServices.Interfaces;
using BlazzingExam.DataLibrary.Entities.User;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace BlazzingExam.Core.Server.Security.Middlewares
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UpdateIdentityMiddleware
    {
        private readonly RequestDelegate _next;

        public UpdateIdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService isUserService)
        {
            var userClaim = context.User;
            var identity = userClaim.Identity;
            if (identity is {IsAuthenticated: true})
            {
                var userName = identity.Name;
                var identityCode = userClaim.FindFirstValue(nameof(User.IdentityCode));
                var user = await isUserService.GetUserByUserNameAsync(userName);
                if (user.IdentityCode != identityCode)
                {
                    var activeCode = userClaim.FindFirstValue(nameof(User.ActiveCode));
                    await context.LogoutAsync();
                    if (user.ActiveCode == activeCode)
                        await context.LoginAsync(user, true);
                }
            }
            await _next(context);
        }
    }
}