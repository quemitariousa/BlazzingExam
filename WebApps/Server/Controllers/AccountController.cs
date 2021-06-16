using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazzingExam.Core.DTOs;
using BlazzingExam.Core.Server.Security;
using BlazzingExam.Core.Server.ServerServices.Interfaces;

namespace BlazzingExam.WebApps.Server.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var loginUser = await _userService.LoginUserAsync(model);
            if (loginUser == null)
                return BadRequest();

            await HttpContext.LoginAsync(loginUser, model.RememberMe);

            return Ok(loginUser);
        }
    }
}
