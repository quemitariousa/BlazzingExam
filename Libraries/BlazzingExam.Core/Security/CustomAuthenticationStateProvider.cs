using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using BlazzingExam.Core.DTOs;
using Microsoft.Extensions.Logging;

namespace BlazzingExam.Core.Security
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomAuthenticationStateProvider> _logger;

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILogger<CustomAuthenticationStateProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var result = await _httpClient.GetAsync("/GetMe");

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var user = await result.Content.ReadFromJsonAsync<LoggedInUserViewModel>();
                    if (user != null)
                    {
                        var claims = new List<Claim>()
                        {
                            new(ClaimTypes.NameIdentifier, user.Id),
                            new(ClaimTypes.Name, user.Username),
                            new(ClaimTypes.Email, user.Email),
                            new("FullName", user.FullName)
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        return new AuthenticationState(principal);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while loggin in user");
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}