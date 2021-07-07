using System.Threading.Tasks;

namespace BlazzingExam.Core.Security
{
    public interface IPermissionChecker
    {
        Task<bool> HasPermission(int permissionId);
    }
}