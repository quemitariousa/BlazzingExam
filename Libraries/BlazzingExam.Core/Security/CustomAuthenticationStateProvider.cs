using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using BlazzingExam.Core.DTOs;

namespace BlazzingExam.Core.Security
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                LogedInUserViewModel user = await _httpClient.GetFromJsonAsync<LogedInUserViewModel>("getme");

                if (user != null)
                {
                    var claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("FullName", user.FullName)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    return new AuthenticationState(principal);
                }
            }
            finally
            {
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}