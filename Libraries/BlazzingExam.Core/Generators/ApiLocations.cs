namespace BlazzingExam.Core.Generators
{
    public struct ApiLocations
    {
        private static readonly string _apiBaseUrl = "api/v1";

        public static string AccountUrl = $"{_apiBaseUrl}/Account";

        private static readonly string _admin = $"{_apiBaseUrl}/admin";

        public static string UserManagement = $"{_admin}/UserManagement";

        public static string RoleManagement = $"{_admin}/rolemanagement";
    }
}