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


        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="model">Login View Model</param>
        /// <returns></returns>
        /// <response code="200">User is successfully loged in.</response>
        /// <response code="400">Username or password wrong.</response>      
        /// <remarks>
        /// Post:
        ///    {
        ///        "userName": "Your username",
        ///        "password": "Your password",
        ///        "rememberMe": true
        ///    }
        /// </remarks>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var loginUser = await _userService.LoginUserAsync(model);
            if (loginUser == null)
                return BadRequest();

            await HttpContext.LoginAsync(loginUser, model.RememberMe);

            return Ok();
        }
    }
}
