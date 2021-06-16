using System.Collections.Generic;
using System.Threading.Tasks;
using BlazzingExam.Core.DTOs;
using BlazzingExam.DataLibrary.Entities.User;

namespace BlazzingExam.Core.Server.ServerServices.Interfaces
{
    public interface IUserService
    {
        #region Public

        Task<bool> IsUserNameExistAsync(string username);

        Task<bool> IsExistEmailAsync(string email);

        Task<int> AddUserAsync(User user);

        Task<User> LoginUserAsync(LoginViewModel model);

        Task<bool> RegisterUserAsync(RegisterViewModel registerViewModel);

        Task<bool> ActiveAccountAsync(string activeCode);

        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByUserNameAsync(string userName);

        Task<User> GetUserByActiveCode(string activeCode);

        Task<bool> UpdateUserAsync(User user, bool editActiveCode = false);

        Task<int> GetUserIdByUserNameAsync(string userName);

        Task<string> GetUserEmailAsync(string userName);

        Task<User> GetUserByIdAsync(int userId);

        #endregion

        #region Admin

        Task<int> GetUsersCountAsync();

        Task<List<User>> VirtualLoadUsers(int startIndex, int userCount);

        Task<bool> DeleteUserAsync(int id);

        #endregion
    }
}
