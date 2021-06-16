using System.Collections.Generic;
using System.Threading.Tasks;
using BlazzingExam.DataLibrary.Entities.Permissions;
using BlazzingExam.DataLibrary.Entities.User;

namespace BlazzingExam.Core.Server.ServerServices.Interfaces
{
    public interface IPermissionService
    {
        #region Roles

        Task<List<Role>> GetAllRolesAsync();

        Task<Role> GetRoleByIdAsync(int roleId);

        Task<List<Role>> GetAllUserRolesAsync(int userId);
        Task<List<Role>> GetAllUserRolesAsync(string userName);
        Task AddRoleToUserAsync(int userId, int roleId);

        Task AddRolesToUserAsync(int userId, List<int> roleList);

        Task DeleteAllUserRoles(int userId);

        Task EditUserRolesAsync(int userId, List<int> roleList);

        Task<int> GetUserInRoleCountAsync(int roleId);

        Task<int> AddRoleAsync(Role role);

        Task UpdateRoleAsync(Role role);

        Task DeleteRoleAsync(int roleId);

        Task<List<User>> GetUsersInRoleAsync(int roleId);

        Task<int> GetRolesCountAsync();

        Task<List<Role>> VirtualLoadRoles(int startIndex, int count);
        #endregion

        #region Permission

        Task<List<Permission>> GetAllPermissionsAsync();
        Task AddPermissionToRoleAsync(int roleId, int permissionId);
        Task AddPermissionsToRoleAsync(int roleId, List<int> permissionList);
        Task DeleteAllRolePermissionsAsync(int roleId);
        Task EditRolePermissionsAsync(int roleId, List<int> permissionList);
        Task<List<RolePermission>> GetRolePermissionsAsync(int roleId);
        Task<List<int>> GetRolePermissionIdsAsync(int roleId);
        Task<bool> IsUserInPermissionAsync(int userId, int permissionId);
        Task<bool> IsUserInPermissionAsync(string userName, int permissionId);

        #endregion
    }
}