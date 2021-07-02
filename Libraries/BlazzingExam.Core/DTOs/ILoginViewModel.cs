using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BlazzingExam.Core.Generators;
using BlazzingExam.DataLibrary.Entities.User;

namespace BlazzingExam.Core.DTOs
{
    public interface ILoginViewModel
    {
        [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
        [MaxLength(60, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا بخاطر بسپار")] public bool RememberMe { get; set; }

        Task<bool> LoginUser();
    }

    public class LoginViewModel : ILoginViewModel
    {
        private readonly HttpClient _client;

        public LoginViewModel()
        {

        }

        public LoginViewModel(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> LoginUser()
        {
            var result = await _client.PostAsJsonAsync<User>($"{_client.BaseAddress}/Login/{RememberMe}", this);

            return result.IsSuccessStatusCode;
        }

        [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
        [MaxLength(60, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(8, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; } = true;

        public static implicit operator User(LoginViewModel loginViewModel)
        {
            return new User()
            {
                UserName = loginViewModel.UserName,
                Password = loginViewModel.Password,
            };
        }

        public static implicit operator LoginViewModel(User user)
        {
            return new LoginViewModel()
            {
                UserName = user.UserName,
                Password = user.Password,
            };
        }
    }
}