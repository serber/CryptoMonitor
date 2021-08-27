using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptoMonitor.DataAccess.Common.Repositories;
using CryptoMonitor.WebApp.Identity;
using Microsoft.AspNetCore.Mvc;
using CryptoMonitor.WebApp.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace CryptoMonitor.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userRepository.GetAsync(model.Login, HashHelper.Hash(model.Password));

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Login),
                    new(ClaimTypes.NameIdentifier, user.UserId)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Market");
            }

            ModelState.TryAddModelError("", "Invalid login or password");

            return View();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Create(CreateAccountModel model)
        {
            if (await _userRepository.ExistAsync(model.Login))
            {
                return BadRequest();
            }

            await _userRepository.AddAsync(model.Login, HashHelper.Hash(model.Password));

            return Ok();
        }
    }
}