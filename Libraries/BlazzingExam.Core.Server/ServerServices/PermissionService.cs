using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazzingExam.Core.Server.ServerServices.Interfaces;
using BlazzingExam.DataLibrary.Contexts;
using BlazzingExam.DataLibrary.Entities.Permissions;
using BlazzingExam.DataLibrary.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BlazzingExam.Core.Server.ServerServices
{
    public class PermissionService : IPermissionService
    {
        private readonly ExamDbContext _db;
        private readonly IUserService _userService;

        public PermissionService(ExamDbContext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        #region Role

        public async Task<List<Role>> GetAllRolesAsync()
            => await _db.Roles.ToListAsync();

        public async Task<Role> GetRoleByIdAsync(int roleId)
            => await _db.Roles.FindAsync(roleId);

        public async Task<List<Role>> GetAllUserRolesAsync(int userId)
            => await _db.UserRoles.Where(p => p.UserId == userId).Select(p => p.Role).ToListAsync();

        public async Task<List<Role>> GetAllUserRolesAsync(string userName)
            => await GetAllUserRolesAsync(await _userService.GetUserIdByUserNameAsync(userName));

        public async Task AddRoleToUserAsync(int userId, int roleId)
        {
            await _db.UserRoles.AddAsync(new UserRole(userId, roleId));
            await _db.SaveChangesAsync();
        }

        public async Task AddRolesToUserAsync(int userId, List<int> roleList)
        {
            var userRoles = roleList.Select(roleId => new UserRole(userId, roleId)).ToList();
            await _db.UserRoles.AddRangeAsync(userRoles);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAllUserRoles(int userId)
        {
            var currentRoles = await _db.UserRoles.Where(p => p.UserId == userId).ToListAsync();
            _db.UserRoles.RemoveRange(currentRoles);
            await _db.SaveChangesAsync();
        }

        public async Task EditUserRolesAsync(int userId, List<int> roleList)
        {
            await DeleteAllUserRoles(userId);
            await AddRolesToUserAsync(userId, roleList);
        }

        public async Task<int> GetUserInRoleCountAsync(int roleId)
            => await _db.UserRoles.CountAsync(p => p.RoleId == roleId);

        public async Task<int> AddRoleAsync(Role role)
        {
            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();
            return role.RoleId;
        }

        public async Task UpdateRoleAsync(Role role)
        {
            _db.Roles.Update(role);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await GetRoleByIdAsync(roleId);
            role.IsDeleted = true;
            await UpdateRoleAsync(role);
        }

        public async Task<List<User>> GetUsersInRoleAsync(int roleId) =>
            await _db.UserRoles.Where(p => p.RoleId == roleId).Select(p => p.User).ToListAsync();

        public async Task<int> GetRolesCountAsync() => await _db.Roles.CountAsync();

        public async Task<List<Role>> VirtualLoadRoles(int startIndex, int count)
            => await _db.Roles.OrderByDescending(r => r.RoleId).Skip(startIndex).Take(count).ToListAsync();

        #endregion

        #region Permissions

        public async Task<List<Permission>> GetAllPermissionsAsync()
            => await _db.Permissions.ToListAsync();

        public async Task AddPermissionToRoleAsync(int roleId, int permissionId)
        {
            await _db.RolePermissions.AddAsync(new RolePermission(roleId, permissionId));
            await _db.SaveChangesAsync();
        }

        public async Task AddPermissionsToRoleAsync(int roleId, List<int> permissionList)
        {
            var rolePermissions = permissionList.Select(permissionId => new RolePermission(roleId, permissionId)).ToList();
            await _db.RolePermissions.AddRangeAsync(rolePermissions);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAllRolePermissionsAsync(int roleId)
        {
            var currentRoles = await _db.RolePermissions.Where(p => p.RoleId == roleId).ToListAsync();
            _db.RolePermissions.RemoveRange(currentRoles);
            await _db.SaveChangesAsync();
        }

        public async Task EditRolePermissionsAsync(int roleId, List<int> permissionList)
        {
            await DeleteAllRolePermissionsAsync(roleId);
            await AddPermissionsToRoleAsync(roleId, permissionList);
        }

        public async Task<List<RolePermission>> GetRolePermissionsAsync(int roleId)
            => await _db.RolePermissions.Where(p => p.RoleId == roleId).ToListAsync();

        public async Task<List<int>> GetRolePermissionIdsAsync(int roleId)
            => await _db.RolePermissions.Where(p => p.RoleId == roleId)
                .Select(p => p.PermissionId).ToListAsync();

        public async Task<bool> IsUserInPermissionAsync(int userId, int permissionId)
            => await _db.UserRoles.AnyAsync(role => role.UserId == userId &&
                                                    role.Role.RolePermissions.Any(permission => permission.PermissionId == permissionId));

        public async Task<bool> IsUserInPermissionAsync(string userName, int permissionId)
            => await IsUserInPermissionAsync(await _userService.GetUserIdByUserNameAsync(userName), permissionId);

        #endregion
    }
}