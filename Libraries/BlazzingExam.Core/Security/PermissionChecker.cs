using System;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace BlazzingExam.Core.Security
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly HttpClient _client;
        private readonly ILogger<PermissionChecker> _logger;

        public PermissionChecker(HttpClient client, ILogger<PermissionChecker> logger)
        {
            _client = client;
            _logger = logger;
        }

        public bool HasPermission(int permissionId)
        {
            try
            {
                //TODO: Check this in backend
                var result = _client.GetAsync($"/perm/{permissionId}").Result;
                if (result.IsSuccessStatusCode)
                    return result.Content.ReadFromJsonAsync<bool>().Result;
                else
                    _logger.LogError($"Error while checking permission {permissionId}");
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error on Permission Checker class");
            }

            return false;
        }
    }
}