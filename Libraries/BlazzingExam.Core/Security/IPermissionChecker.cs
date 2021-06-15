namespace BlazzingExam.Core.Security
{
    public interface IPermissionChecker
    {
        bool HasPermission(int permissionId);
    }
}