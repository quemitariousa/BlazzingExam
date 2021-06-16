using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazzingExam.Core.Convertors;
using BlazzingExam.Core.DTOs;
using BlazzingExam.Core.Generators;
using BlazzingExam.Core.Server.Security;
using BlazzingExam.Core.Server.ServerServices.Interfaces;
using BlazzingExam.DataLibrary.Contexts;
using BlazzingExam.DataLibrary.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BlazzingExam.Core.Server.ServerServices
{
    public class UserService : IUserService
    {
        private readonly ExamDbContext _db;

        public UserService(ExamDbContext db)
        {
            _db = db;
        }

        #region Public

        public async Task<bool> IsUserNameExistAsync(string username) =>
            await _db.Users.AnyAsync(p => p.FixedUserName == TextFixer.FixUserName(username));

        public async Task<bool> IsExistEmailAsync(string email) =>
            await _db.Users.AnyAsync(p => p.FixedEmail == TextFixer.FixEmail(email));

        public async Task<int> AddUserAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user.UserId;
        }

        public async Task<User> LoginUserAsync(LoginViewModel model)
        {
            var hashedPassword = PasswordHelper.EncodePasswordMd5(model.Password);
            var fixedUserName = TextFixer.FixUserName(model.UserName);

            return await _db.Users.SingleOrDefaultAsync(p =>
                p.FixedUserName == fixedUserName &&
                p.Password == hashedPassword);
        }

        public async Task<bool> RegisterUserAsync(RegisterViewModel registerViewModel)
        {
            if (await IsUserNameExistAsync(registerViewModel.UserName) || await IsExistEmailAsync(registerViewModel.Email))
                return false;

            var id = await AddUserAsync(new()
            {
                Email = registerViewModel.Email,
                LastName = registerViewModel.LastName,
                FirstName = registerViewModel.FirstName,
                Password = PasswordHelper.EncodePasswordMd5(registerViewModel.Password),
                PhoneNumber = registerViewModel.PhoneNumber,
                UserName = registerViewModel.UserName,
            });

            return id != 0;
        }

        public async Task<bool> ActiveAccountAsync(string activeCode)
        {
            var user = await _db.Users.SingleOrDefaultAsync(p => p.ActiveCode == activeCode);
            if (user == null || user.IsEmailConfirmed)
                return false;
            user.IsEmailConfirmed = true;
            user.ActiveCode = NameGenerator.GenerateUniqueCode();
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserByEmailAsync(string email) =>
            await _db.Users.SingleOrDefaultAsync(p => p.Email == TextFixer.FixEmail(email));

        public async Task<User> GetUserByUserNameAsync(string userName)
            => await _db.Users.SingleOrDefaultAsync(p => p.UserName == userName);

        public async Task<User> GetUserByActiveCode(string activeCode)
            => await _db.Users.SingleOrDefaultAsync(p => p.ActiveCode == activeCode);

        public async Task<bool> UpdateUserAsync(User user, bool editActiveCode = false)
        {
            try
            {
                if (editActiveCode)
                    user.ActiveCode = NameGenerator.GenerateUniqueCode();
                user.IdentityCode = NameGenerator.GenerateUniqueCode();
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<int> GetUserIdByUserNameAsync(string userName)
        {
            var user = await _db.Users.Select(p => new
            {
                p.UserId,
                p.FixedUserName,
            }).SingleOrDefaultAsync(p => p.FixedUserName == TextFixer.FixUserName(userName));

            return user.UserId;
        }

        public async Task<string> GetUserEmailAsync(string userName)
        {
            var user = await _db.Users.Select(p => new
            {
                p.Email,
                p.FixedUserName
            }).SingleOrDefaultAsync(p => p.FixedUserName == TextFixer.FixUserName(userName));

            return user.Email;
        }

        public async Task<User> GetUserByIdAsync(int userId)
            => await _db.Users.FindAsync(userId);

        #endregion

        #region Admin

        public async Task<int> GetUsersCountAsync() => await _db.Users.CountAsync();

        public async Task<List<User>> VirtualLoadUsers(int startIndex, int userCount) => 
            await _db.Users.OrderByDescending(p => p.RegisterTime).Skip(startIndex).Take(userCount).ToListAsync();

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null)
                return false;
            user.IsDeleted = true;
            await UpdateUserAsync(user, true);
            return true;
        }

        #endregion
    }
}