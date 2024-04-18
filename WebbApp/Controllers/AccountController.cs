using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebbApp.ViewModels;

namespace WebbApp.Controllers;
[Authorize]
public class AccountController(UserManager<UserEntity> userManager, ApplicationContext context) : Controller
{

    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly ApplicationContext _context = context;

    public async Task<IActionResult> Details()
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var user = await _context.Users.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == nameIdentifier);

        var viewModel = new AccountDetailsViewModel
        {
            BasicInfo = new AccountBasicInfo
            {
                FirstName = user!.FirstName,
                LastName = user!.LastName,
                Email = user.Email!,
                Phone = user.PhoneNumber,
                Bio = user.Bio
            },

            AddressInfo = new AccountAddressInfo
            {
                Addressline_1 = user.Address?.AddressLine_1!,
                Addressline_2 = user.Address?.AddressLine_2!,
                PostalCode = user.Address?.PostalCode!,
                City = user.Address?.City!,
            }
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBasicInfo(AccountDetailsViewModel model) 
    {
        if (TryValidateModel(model.BasicInfo!))
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.FirstName = model.BasicInfo!.FirstName;
                user.LastName = model.BasicInfo!.LastName;
                user.Email = model.BasicInfo!.Email;
                user.PhoneNumber = model.BasicInfo!.Phone;
                user.UserName = model.BasicInfo!.Email;
                user.Bio = model.BasicInfo!.Bio;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["StatusMessage"] = "Successfully updated basic information.";
                }
                else
                {
                    TempData["StatusMessage"] = "Unable to save basic information";
                }

            }
        }
        else
        {
            TempData["StatusMessage"] = "Unable to save basic information.";
        }
        return RedirectToAction("Details", "Account");
    }



    [HttpPost]
    public async Task<IActionResult> UpdateAddressInfo(AccountDetailsViewModel model)
    {
        if (ValidateAddressInfo(model.AddressInfo!))
        {
            var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _context.Users.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == nameIdentifier);
            if (user != null)
            {
                try
                {
                    if (user.Address != null)
                    {
                        user.Address.AddressLine_1 = model.AddressInfo!.Addressline_1;
                        user.Address.AddressLine_2 = model.AddressInfo.Addressline_2;
                        user.Address.PostalCode = model.AddressInfo!.PostalCode;
                        user.Address.City = model.AddressInfo!.City;
                    }
                    else
                    {
                        user.Address = new AddressEntity
                        {
                            AddressLine_1 = model.AddressInfo!.Addressline_1,
                            AddressLine_2 = model.AddressInfo.Addressline_2,
                            PostalCode = model.AddressInfo.PostalCode,
                            City = model.AddressInfo.City
                        };
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    TempData["StatusMessage"] = "Successfully updated address information.";
                }
                catch
                {
                    TempData["StatusMessage"] = "Unable to save address information";
                }
            }
        }
        else
        {
            TempData["StatusMessage"] = "Unable to save address information";
        }

        return RedirectToAction("Details", "Account");
    }










    [HttpPost]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        var user = await _userManager.GetUserAsync(User);
        if(user != null && file != null && file.Length != 0)
        {
            var fileName = $"p_{user.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath  = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/uploads/profiles", fileName);
            using var fs = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fs);
            user.ProfileImage = fileName;
            await _userManager.UpdateAsync(user);
        }
        else
        {
            TempData["StatusMessage"] = "Unable to upload profile image";
        }
        return RedirectToAction("Details", "Account");
    }



    public async Task<IActionResult> Security()
    {
        return View();
    }





    /*
    [HttpPost]
    public async Task<IActionResult> ChangePassword(AccountSecurityViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (changePasswordResult.Succeeded)
                {
                    TempData["StatusMessage"] = "Password changed successfully.";
                    return RedirectToAction("Security");
                }
                else
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                TempData["StatusMessage"] = "User not found.";
            }
        }
        // If we got this far, something failed, redisplay the form
        return View("Security", model);
    }
    */



    [HttpPost]
    public async Task<IActionResult> ChangePassword(AccountSecurityViewModel model)
    {
        if (ModelState.IsValid)
        {












            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (changePasswordResult.Succeeded)
                {
                    TempData["StatusMessage"] = "Password changed successfully.";
                    return RedirectToAction("Security");
                }
                else
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                TempData["StatusMessage"] = "User not found.";
            }
        }
        // If we got this far, something failed, redisplay the form
        return View("Security", model);
    }













    private bool ValidateAddressInfo(AccountAddressInfo addressInfo)
    {
        if (addressInfo == null)
            return false;

        if (string.IsNullOrEmpty(addressInfo.Addressline_1) || string.IsNullOrEmpty(addressInfo.PostalCode) || string.IsNullOrEmpty(addressInfo.City))
            return false;


        return true;
    }

}
