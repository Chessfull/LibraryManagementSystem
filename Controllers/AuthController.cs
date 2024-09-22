using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Models.Repositories;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace LibraryManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly IDataProtector _dataProtector;

        public AuthController(IDataProtectionProvider dataProtectionProvider,IWebHostEnvironment environment )
        {
            _dataProtector = dataProtectionProvider.CreateProtector("security");

            string csvFilePath = Path.Combine(environment.ContentRootPath, "App_Data", "LibraryManagementUsersDB.csv");
            _userRepository = new UserRepository(csvFilePath);
        }



        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SignUp(SignUpViewModel signUpForm)
        {

            if (!ModelState.IsValid)
            {
                return View(signUpForm);
            }

            var user = _userRepository.GetAll().FirstOrDefault(I => I.Email.ToLower() == signUpForm.Email.ToLower());

            if (user is not null)
            {
                ViewBag.ErrorMessage = "Sorry. This email is already registered.";

                return View(signUpForm);
            }

            var newUser = new User()
            {
                Name = signUpForm.Name,
                Surname = signUpForm.Surname,
                Email = signUpForm.Email,
                Password = _dataProtector.Protect(signUpForm.Password),
                PhoneNumber = signUpForm.PhoneNumber,
            };

            _userRepository.Add(newUser);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInForm)
        {

            var user = _userRepository.GetAll().FirstOrDefault(I => I.Email.ToLower() == signInForm.Email.ToLower());

            if (user is null)
            {
                ViewBag.ErrorMessage = "Sorry. Username of Password is incorrect.";

                return View(signInForm);
            }

            var rawPassword = _dataProtector.Unprotect(user.Password);

            if (rawPassword == signInForm.Password)
            {
                var claims=new List<Claim>();

                claims.Add(new Claim("email", user.Email));
                claims.Add(new Claim("id", user.Id.ToString()));
                claims.Add(new Claim("fullname", user.Name+" "+user.Surname));

                var claimIdendity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                var authFeatures = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddHours(1))
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimIdendity),authFeatures);


            }

            else
            {
                ViewBag.ErrorMessage = "Sorry. Username of Password is incorrect.";

                return View(signInForm);
            }

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }


    }
}
