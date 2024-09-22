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
        private readonly UserRepository _userRepository; // > Using user repository for user database processes

        private readonly IDataProtector _dataProtector; // -> Its a data protection interface for password ( 1234 -> dqwdqweqkls crypto )

        // ▼ ctor ▼
        public AuthController(IDataProtectionProvider dataProtectionProvider,IWebHostEnvironment environment )
        {
            _dataProtector = dataProtectionProvider.CreateProtector("security"); // -> Instance for data protection

            // ▼ For dynamic my csv database root path I used IWebHostEnvironment.contentrootpath, its useful for downloading this project from anyone my datasource path will be dynamic in app_data folder ▼
            string csvFilePath = Path.Combine(environment.ContentRootPath, "App_Data", "LibraryManagementUsersDB.csv");
            
            _userRepository = new UserRepository(csvFilePath); // -> Instance for user repository to use data actions 
        }

        [HttpGet] // -> With click signup button to view httpget
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost] // -> with signup form-post datas and creating new user after controls
        public IActionResult SignUp(SignUpViewModel signUpForm)
        {

            // ▼ Checking modelstate valid from form ▼
            if (!ModelState.IsValid)
            {
                return View(signUpForm);
            }

            // ▼ If modelstate valid calling my database from repository and get user list and checking signup form email exist or not
            var user = _userRepository.GetAll().FirstOrDefault(I => I.Email.ToLower() == signUpForm.Email.ToLower());

            // ▼ Above email exist check if exist going this and error message to view
            if (user is not null)
            {
                ViewBag.ErrorMessage = "Sorry. This email is already registered.";

                return View(signUpForm);
            }

            // ▼ After email exist check is ok creating new user and adding to database with user repository
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

        [HttpGet] // -> With click signin button to view httpget
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost] // -> Signin post method form
        public async Task<IActionResult> SignIn(SignInViewModel signInForm)
        {
            // ▼ Calling database from repository and get user list with checking signin form email exist or not
            var user = _userRepository.GetAll().FirstOrDefault(I => I.Email.ToLower() == signInForm.Email.ToLower());

            // ▼ If exist manage error
            if (user is null)
            {
                ViewBag.ErrorMessage = "Sorry. Username of Password is incorrect.";

                return View(signInForm);
            }

            // ▼ If not unprotect password which we protect before when sign up processes ▼
            var rawPassword = _dataProtector.Unprotect(user.Password);

            // ▼ checking password and login
            if (rawPassword == signInForm.Password)
            {

                // ▼ Add user claims email, id and user name & user surname for identify and displaying ▼
                var claims=new List<Claim>();

                claims.Add(new Claim("email", user.Email));
                claims.Add(new Claim("id", user.Id.ToString()));
                claims.Add(new Claim("fullname", user.Name+" "+user.Surname));

                // ▼ Create a ClaimsIdentity using the list of claims and specifying the authentication scheme ▼
                var claimIdendity =new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                // ▼ Define authentication properties ▼
                var authFeatures = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddHours(1))
                };

                // ▼ Sign the user in with claims, setting the authentication scheme and properties ▼
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimIdendity),authFeatures);


            }

            // ▼ If password does not match managing error
            else
            {
                ViewBag.ErrorMessage = "Sorry. Username of Password is incorrect.";

                return View(signInForm);
            }

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> SignOut()
        {
            // ▼ Sign out the user by clearing their authentication cookie and ending their session ▼
            await HttpContext.SignOutAsync();

            // ▼ After sign out rediret to user homepage
            return RedirectToAction("Index", "Home");

        }
    }
}
