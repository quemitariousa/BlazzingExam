using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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

        public async Task<bool> HasPermission(int permissionId)
        {
            try
            {
                var result = await _client.GetAsync($"/perm/{permissionId}");
                if (result.IsSuccessStatusCode)
                    return await result.Content.ReadFromJsonAsync<bool>();
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