﻿using Biblioteca.Web.Data;
using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Helpers;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly ICityRepository _cityRepository;

        public AccountController(
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            DataContext dataContext,
            UserManager<User> userManager,
            ICityRepository cityRepository
            )
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _configuration = configuration;
            _roleManager = roleManager;
            _dataContext = dataContext;
            _userManager = userManager;
            _cityRepository = cityRepository;
        }

        public IActionResult Login()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
       
       
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                if (user != null && await _userManager.IsInRoleAsync(user, "Admin") ||
                                    await _userManager.IsInRoleAsync(user, "Staff") ||
                                    await _userManager.IsInRoleAsync(user, "Reader"))
                {
                    var result = await _userHelper.LoginAsync(model);
                    if (result.Succeeded)
                    {
                        if (this.Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(this.Request.Query["ReturnUrl"].First());
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Add a new action to display the account creation form
        [RoleAuthorization("Admin")]
        public IActionResult Create()
        {
            var model = new CreateUserViewModel
            {
                Cities = _cityRepository.GetComboCities(),
                Libraries = _cityRepository.GetComboLibraries(0)
            };

            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("", "The password and confirmation password do not match.");
                    var rolesList = await _roleManager.Roles.ToListAsync();
                    ViewBag.Roles = new SelectList(rolesList, "Name", "Name");
                    return View(model);
                }

                var library = await _cityRepository.GetLibraryAsync(model.LibraryId);

                var user = new Data.Entities.User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    LibraryId = model.LibraryId,
                    Library = library,
                    RegistrationDate = DateTime.Now
                };

                var result = await _userHelper.CreateUserAsync(user, model.Password, model.SelectedRole);

                if (result.Succeeded)
                {
                    // Generate email confirmation token
                    var emailConfirmationToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                    // Create confirmation link
                    //var confirmationLink = Url.Action("ConfirmEmail", "Account", new
                    //{
                    //    userId = user.Id,
                    //    token = emailConfirmationToken
                    //}, protocol: HttpContext.Request.Scheme);

                    // Send confirmation email
                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userId = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    Response response =  _mailHelper.SendEmail(user.Email, "Email confirmation",
                        $"<h1>Email Confirmation</h1>To allow the user, please click in this link: </br></br><a href=\"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    {
                        TempData["EmailConfirmationMessage"] = "Email confirmation link has been sent to the user's email.";
                        return RedirectToAction("Create", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to send confirmation email.");
                    }
                }
                else
                {
                    // Handle user creation failure
                    // Show error messages
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            // Repopulate the roles dropdown in case of validation errors
            ViewBag.Roles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
            return View(model);
        }







        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                Cities = _cityRepository.GetComboCities(),
                Libraries = _cityRepository.GetComboLibraries(0)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    var library = await _cityRepository.GetLibraryAsync(model.LibraryId);

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        LibraryId = model.LibraryId,
                        Library = library,
                        RegistrationDate = DateTime.Now
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    // Check if the "Reader" role exists
                    await _userHelper.CheckRoleAsync("Reader");

                    // Add the user to the "Reader" role
                    await _userHelper.AddUserToRoleAsync(user, "Reader");

                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userId = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    // Use the SendEmail method and await its response
                    Response response = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                        $"To allow the user, " +
                        $"please click in this link: </br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    {
                        ViewBag.Message = "The instructions to allow your user has been sent to your email.";
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The email couldn't be sent.");
                    }
                }
            }
            return View(model);
        }



        public async Task<IActionResult> UserList()
        {
            var users = await _userHelper.GetAllUsersAsync();

            // Create a URL that includes the current URL as a query parameter
            var currentUrl = Url.Action("UserList", "Account", null, Request.Scheme);

            // Get all roles to be used in the view
            var allRoles = await _roleManager.Roles.ToListAsync();

            var usersWithRoles = new List<UserWithRolesViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Map roles to their names and filter out empty names
                var roleNames = roles.Select(role => allRoles.FirstOrDefault(r => r.Name == role)?.Name)
                                     .Where(name => !string.IsNullOrEmpty(name))
                                     .ToList();

                usersWithRoles.Add(new UserWithRolesViewModel
                {
                    User = user,
                    Roles = roleNames,
                    UserListUrl = currentUrl // Pass the URL to the view model
                });
            }

            return View(usersWithRoles);
        }


        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;
                model.RegistrationDate = user.RegistrationDate;

                var library = await _cityRepository.GetLibraryAsync(user.LibraryId);
                if (library != null)
                {
                    var city = await _cityRepository.GetCityAsync(library);
                    if (city != null)
                    {
                        model.CityId = city.Id;
                        // Set the LibraryId and Libraries based on the selected city
                        model.LibraryId = user.LibraryId;
                        model.Libraries = _cityRepository.GetComboLibraries(city.Id);
                    }
                }
            }

            model.Cities = _cityRepository.GetComboCities();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var library = await _cityRepository.GetLibraryAsync(model.LibraryId);

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    user.LibraryId = model.LibraryId;
                    user.Library = library;

                    var response = await _userHelper.UpdateUserAsync(user);

                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid .NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(string.IsNullOrWhiteSpace(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }
        public IActionResult NotAuthorized()
        {
            return View();
        }

        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userHelper.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            var user = await _userHelper.GetUserByIdAsync(Id);

            if (user != null)
            {
                var result = await _userHelper.DeleteUserAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); // Redirect to the desired page
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found.");
            }

            // Redirect back to the Delete view with the user's details
            return View("Delete", user);
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Libraby Password Reset", $"<h1>Library Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                }
                return this.View();
            }
            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return View();
                }
                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }
            this.ViewBag.Message = "User not found.";
            return View(model);
        }

        [HttpPost]
        [Route("Account/GetLibrariesAsync")]
        public async Task<JsonResult> GetLibrariesAsync(int cityId)
        {
            try
            {
                var city = await _cityRepository.GetCityWithLibrariesAsync(cityId);

                if (city != null)
                {
                    var libraries = city.Libraries.OrderBy(l => l.Name);
                    return Json(libraries);
                }
                else
                {
                    // Handle the case where the city is not found.
                    return Json(new { error = "City not found" });
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that may occur.
                return Json(new { error = "An error occurred: " + ex.Message });
            }
        }

        private async Task<DateTime?> GetRegistrationDate(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);

            if (user != null)
            {
                return user.RegistrationDate;
            }

            return null;
        }


    }
}
