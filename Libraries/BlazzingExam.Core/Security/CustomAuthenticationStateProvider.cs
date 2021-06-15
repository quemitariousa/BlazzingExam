using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

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
            //User user = null;
            //try
            //{
            //    user = await _httpClient.GetFromJsonAsync<User>("getme");
            //}
            //catch
            //{

            //}
            //if (user != null)
            //{
            //    var claims = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            //        new Claim(ClaimTypes.Name, user.UserName),
            //        new Claim(ClaimTypes.Email, user.Email),
            //        new Claim("FullName", $"{user.FirstName} {user.LastName}"),
            //        new Claim("FirstName", user.FirstName),
            //        new Claim("LastName", user.LastName),
            //    };

            //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //    var principal = new ClaimsPrincipal(identity);
            //    return new AuthenticationState(principal);
            //}
            //TODO: Check this
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}