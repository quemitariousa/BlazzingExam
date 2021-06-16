using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BlazzingExam.Core.Generators;

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

        public LoginViewModel(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> LoginUser()
        {
            var result = await _client.PostAsJsonAsync($"Login", this);

            return result.IsSuccessStatusCode;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}