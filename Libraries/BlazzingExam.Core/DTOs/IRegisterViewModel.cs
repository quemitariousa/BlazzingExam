using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using BlazzingExam.DataLibrary.Entities.User;

namespace BlazzingExam.Core.DTOs
{
    public interface IRegisterViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        Task<bool> Register();
        Task<bool> IsUsernameExist();
        Task<bool> IsEmailExists();
    }

    public class RegisterViewModel : IRegisterViewModel
    {
        private readonly HttpClient _client;

        public RegisterViewModel()
        {

        }

        public RegisterViewModel(HttpClient client)
        {
            _client = client;
        }

        [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
        [MaxLength(60, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "ایمیل شما معتبر نمیباشد")]
        [Display(Name = "ایمیل", Prompt = "Name@Example.com")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور", Prompt = "تکرار رمز عبور")]
        [PasswordPropertyText]
        public string RepeatPassword { get; set; }

        [Display(Name = "نام", Prompt = "علی")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی", Prompt = "محمدی")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
        public string LastName { get; set; }

        [Display(Name = "شماره همراه", Prompt = "09131234566")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "شماره تلفن وارد شده معتبر نیست")]
        [MaxLength(11, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
        public string PhoneNumber { get; set; }

        public async Task<bool> Register()
        {
            var result = await _client.PostAsJsonAsync<User>($"{_client.BaseAddress}/register", this);
            if (!result.IsSuccessStatusCode)
                return false;

            return await result.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> IsUsernameExist()
        {
            try
            {
                return await _client.GetFromJsonAsync<bool>($"{_client.BaseAddress}/IsUsernameExist/{this.UserName}");
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsEmailExists()
        {
            try
            {
                return await _client.GetFromJsonAsync<bool>($"{_client.BaseAddress}/IsEmailExists/{this.Email}");
            }
            catch
            {
                return false;
            }
        }


        public static implicit operator User(RegisterViewModel model)
        {
            return new User()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
            };
        }

        public static implicit operator RegisterViewModel(User model)
        {
            return new RegisterViewModel()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName, 
                Password = model.Password, 
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
            };
        }
    }
}