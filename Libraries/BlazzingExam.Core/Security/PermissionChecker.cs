using System.Net.Http;
using System.Net.Http.Json;

namespace BlazzingExam.Core.Security
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly HttpClient _client;

        public PermissionChecker(HttpClient client)
        {
            _client = client;
        }

        public bool HasPermission(int permissionId)
        {
            try
            {
                //TODO: Check this in backend
                var result = _client.GetFromJsonAsync<bool>($"/perm/{permissionId}").Result;
                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}