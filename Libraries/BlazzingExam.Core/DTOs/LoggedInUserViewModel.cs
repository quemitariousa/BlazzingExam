using BlazzingExam.DataLibrary.Entities.User;

namespace BlazzingExam.Core.DTOs
{
    public class LoggedInUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }
        
        public string Email { get; set; }

        public string FullName { get; set; }

        public static implicit operator User(LoggedInUserViewModel model)
        {
            return new()
            {
                UserId = int.Parse(model.Id),
                UserName = model.Username,
                Email = model.Email
            };
        }

        public static implicit operator LoggedInUserViewModel(User model)
        {
            return new()
            {
                Id = model.UserId.ToString(),
                Username = model.UserName,
                Email = model.Email,
                FullName = $"{model.FirstName} {model.LastName}"
            };
        }
    }
}
