using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebbApp.ViewModels;

namespace WebbApp.Controllers
{
    public class AuthController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, ApplicationContext context) : Controller
    {


        private readonly UserManager<UserEntity> _userManager = userManager;
        private readonly SignInManager<UserEntity> _signInManager = signInManager;
        private readonly ApplicationContext _context = context;


        #region Sign Up
        [Route("/signup")]
        public IActionResult SignUp()
        {
            //return RedirectToAction("Home", "Default");
            return View();
        }

        [HttpPost]
        [Route("/signup")]

        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(!await _context.Users.AnyAsync(x => x.Email == model.Email))
                {
                    var userEntity = new UserEntity
                    {
                        Email = model.Email,
                        UserName = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    };


                    if((await _userManager.CreateAsync(userEntity, model.Password)).Succeeded)
                    {
                        if ((await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false)).Succeeded)
                        {
                            return LocalRedirect("/");
                        }
                        else
                        {
                            return LocalRedirect("/signin");
                        }
                    }

                    else
                    {
                        ViewData["StatusMesssage"] = "Something went wrong. Try again later or contact customer service";
                    }

                }
                else
                {
                    ViewData["StatusMesssage"] = "User with the same email already exists";
                }
            }
            return View(model);

        }

        #endregion


        #region Sign In
        [Route("/signin")]

        public IActionResult SignIn(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl ?? "/";
            return View();
        }
        [HttpPost]
        [Route("/signin")]

        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if ((await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.IsPresistent, false)).Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
            }
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["StatusMessage"] = "Incorrect email or password";
            return View(model);
        }
        #endregion

        #region Sign Out
        [Route("/signout")]

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "Default");
        }

        #endregion


        #region External Account Facebok

        [HttpGet]

        public IActionResult Facebook()
        {
            var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("FacebookCallback"));
            return new ChallengeResult("Facebook", authProps);
        }





        [HttpGet]
        public async Task<IActionResult> FacebookCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info != null)
            {
                // Create a new UserEntity using information from Facebook
                var userEntity = new UserEntity
                {
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!,
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!,
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                };
                var user = await _userManager.FindByEmailAsync(userEntity.Email);
                if (user == null)
                {
                    var result = await _userManager.CreateAsync(userEntity);
                    if (result.Succeeded)
                    {
                        user = await _userManager.FindByEmailAsync(userEntity.Email);
                    }
                }
                if(user != null)
                {
                    if(user.FirstName != userEntity.FirstName || user.LastName != userEntity.LastName || user.Email != userEntity.Email)
                    {
                        user.FirstName = userEntity.FirstName;
                        user.LastName = userEntity.LastName;
                        user.Email = userEntity.Email;



                        await _userManager.UpdateAsync(user);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if(HttpContext.User != null)
                    {
                        return RedirectToAction("Details", "Account");
                    }
                }
            }
            return RedirectToAction("SignIn", "Auth");
        }

        #endregion


        #region External Account Google

        [HttpGet]
        public IActionResult Google()
        {
            var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("GoogleCallback"));
            return new ChallengeResult("Google", authProps);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info != null)
            {
                // Retrieve user information from the external login provider
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                // Check if a user with the same email already exists in the database
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    // If the user doesn't exist, create a new user entity
                    var newUser = new UserEntity
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        UserName = email,
                    };

                    // Attempt to create the user in the database
                    var createUserResult = await _userManager.CreateAsync(newUser);
                    if (createUserResult.Succeeded)
                    {
                        // If user creation is successful, sign in the user
                        await _signInManager.SignInAsync(newUser, isPersistent: false);
                    }
                    else
                    {
                        // Handle the case where user creation fails
                        ViewData["ErrorMessage"] = "Failed to create user account.";
                        return View("Error");
                    }
                }
                else
                {
                    // If the user already exists, sign in the existing user
                    await _signInManager.SignInAsync(existingUser, isPersistent: false);
                }
            }
            else
            {
                // Handle the case where external login information is not available
                ViewData["ErrorMessage"] = "External login information not available.";
                return View("Error");
            }

            // Redirect to a suitable action after successful authentication
            return RedirectToAction("Details", "Account");
        }



        #endregion


    }
}
