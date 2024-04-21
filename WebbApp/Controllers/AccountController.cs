using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using WebbApp.ViewModels;

namespace WebbApp.Controllers;
[Authorize]
public class AccountController(UserManager<UserEntity> userManager, ApplicationContext context, HttpClient httpClient) : Controller
{

    private readonly UserManager<UserEntity> _userManager = userManager;

    private readonly ApplicationContext _context = context;
    private readonly HttpClient _httpClient = httpClient;


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



    

    public IActionResult Security()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Security(AccountSecurityViewModel model)
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
                    return View();
                }
                else
                {
                    TempData["StatusMessage"] = "Something went wrong";
                }
            }
            else
            {
                TempData["StatusMessage"] = "User not found.";
            }
        }
        return View();
    }



    [HttpPost]
    public async Task<IActionResult> DeleteAccount()
    {
        if (ModelState.IsValid)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var deleteUser = await _userManager.DeleteAsync(user);
                if (deleteUser.Succeeded)
                {
                    TempData["StatusMessage"] = "User successfully removed";
                    return RedirectToAction("SignOut", "Auth");
                }
                else
                {
                    foreach (var error in deleteUser.Errors)
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
        return RedirectToAction("Auth", "SignOut");

    }



    private bool ValidateAddressInfo(AccountAddressInfo addressInfo)
    {
        if (addressInfo == null)
            return false;

        if (string.IsNullOrEmpty(addressInfo.Addressline_1) || string.IsNullOrEmpty(addressInfo.PostalCode) || string.IsNullOrEmpty(addressInfo.City))
            return false;


        return true;
    }


    public async Task<IActionResult> SavedCourses(int id)
    {
        try
        {
            // Check if a course with the same CourseId already exists in the database
            var existingCourse = await _context.SavedCourses.FirstOrDefaultAsync(c => c.CourseId == id);

            if (existingCourse != null)
            {
                // If the course already exists, return a message indicating that it already exists
                return BadRequest("Course with the same CourseId already exists.");
            }

            // Create an instance of SavedCourseEntity
            var courseEntity = new SavedCourseEntity
            {
                CourseId = id
            };

            // Add the entity to the context
            _context.SavedCourses.Add(courseEntity);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return success response
            return Ok();
        }
        catch (Exception ex)
        {
            // Handle exceptions if any
            return StatusCode(500, "An error occurred while saving the course: " + ex.Message);
        }
    }

    public async Task<IActionResult> SavedCoursesContent()
    {
        try
        {
            // Retrieve CourseId values from the SavedCourses database table
            var courseIds = await _context.SavedCourses
                                          .Select(t => t.CourseId)
                                          .ToListAsync();

            // Create a list to store the retrieved course details
            var savedCourses = new List<SavedCourseViewModel>();

            // Iterate over each courseId and make individual requests
            foreach (var courseId in courseIds)
            {
                var response = await _httpClient.GetAsync($"https://localhost:7199/api/courses/{courseId}?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1");
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<SavedCourseViewModel>(jsonContent);

                    // Add the retrieved course details to the list
                    savedCourses.Add(model);
                }
                else
                {
                    // Handle the case where the request was not successful
                    // You might want to log or handle this scenario appropriately
                }
            }

            // Pass the list of retrieved course details to the view
            return View(savedCourses);
        }
        catch (Exception ex)
        {
            // Handle exceptions if any
            return StatusCode(500, "An error occurred while retrieving saved course details: " + ex.Message);
        }
    }


    [HttpPost]
    public async Task<IActionResult> SavedCoursesContentDelete(int id)
    {
        try
        {
            var courseToDelete = await _context.SavedCourses.FirstOrDefaultAsync(c => c.CourseId == id);

            if (courseToDelete == null)
            {
                return NotFound("Course not found.");
            }

            _context.SavedCourses.Remove(courseToDelete);

            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting the course: " + ex.Message);
        }
    }



    [HttpPost]
    public async Task<IActionResult> SavedCoursesContentDeleteAll()
    {
        try
        {
            var allCourses = await _context.SavedCourses.ToListAsync();

            if (allCourses == null || allCourses.Count == 0)
            {
                return NotFound("No saved courses found.");
            }

            _context.SavedCourses.RemoveRange(allCourses);

            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting all saved courses: " + ex.Message);
        }
    }




    [HttpPost]
    public async Task<IActionResult> JoinCourse(int id)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var existingRegistration = await _context.RegisteredCourses.FirstOrDefaultAsync(rc => rc.CourseId == id && rc.UserId == user.Id);

            if (existingRegistration != null)
            {
                return BadRequest("User is already registered for this course.");
            }

            var registration = new RegisteredCoursesEntity
            {
                CourseId = id,
                UserId = user.Id 
            };

            _context.RegisteredCourses.Add(registration);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Courses");

        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while registering for the course: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> UnregisterFromCourse(int id)
    {
        try
        {
            var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var registeredCourse = await _context.RegisteredCourses.FirstOrDefaultAsync(c => c.CourseId == id && c.UserId == nameIdentifier);

            if (registeredCourse == null)
            {
                return NotFound("User is not registered for the course.");
            }

            _context.RegisteredCourses.Remove(registeredCourse);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Courses");

        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while unregistering from the course: " + ex.Message);
        }
    }





}








