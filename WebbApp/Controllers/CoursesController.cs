using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using WebbApp.ViewModels;
using WebbApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

namespace WebbApp.Controllers;
[Authorize]

public class CoursesController(CategoryService categoryService, CourseService courseService, HttpClient httpClient) : Controller
{
    private readonly CategoryService _categoryService = categoryService;
    private readonly CourseService _courseService = courseService;
    
    private readonly HttpClient _httpClient = httpClient;

    //[Route("/courses")]
    public async Task<IActionResult> Index(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 6)
    {

        var courseResult = await _courseService.GetCoursesAsync(category, searchQuery, pageNumber, pageSize);


        var viewModel = new CourseViewModel()
        {
            Categories = await _categoryService.GetCategoriesAsync(),
            Courses = courseResult.Courses,
            Pagination =  new Pagination
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = courseResult.TotalPages,
                TotalItems = courseResult.TotalItems
            }

        };

        return View(viewModel);

    }


    [Route("/Courses/SingleCourse/{id}")]
    public async Task<IActionResult> SingleCourse(int id)
    {
        // You may need to adjust the URL to match your API endpoint
        var response = await _httpClient.GetAsync($"https://localhost:7199/api/courses/{id}?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1");
        if (response.IsSuccessStatusCode)
        {
            var courseViewModel = JsonConvert.DeserializeObject<CourseViewModel>(await response.Content.ReadAsStringAsync());

            // Redirect to a new action method that will display the details for the course
            return RedirectToAction("CourseDetails", new { id = id });
        }
        else
        {
            return NotFound();
        }
    }

    // New action method to display course details
    [Route("/Courses/CourseDetails/{id}")]
    public async Task<IActionResult> CourseDetails(int id)
    {
        // Retrieve course details based on the ID
        var response = await _httpClient.GetAsync($"https://localhost:7199/api/courses/{id}?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1");
        if (response.IsSuccessStatusCode)
        {
            var courseViewModel = JsonConvert.DeserializeObject<CourseViewModel>(await response.Content.ReadAsStringAsync());

            // Pass the course details to the view
            return View(courseViewModel);
        }
        else
        {
            return NotFound();
        }
    }



    /*
    [Route("/Courses/SingleCourse")]
    public async Task<IActionResult> SingleCourse(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7199/api/courses/{id}?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1");
        if (response.IsSuccessStatusCode)
        {
            var courseViewModel = JsonConvert.DeserializeObject<CourseViewModel>(await response.Content.ReadAsStringAsync());

            return View(courseViewModel);
        }
        else
        {
            return NotFound();
        }
    }*/



    /*public async Task<IActionResult> SaveCourse(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7199/api/courses/{id}?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1");

        return RedirectToAction("Index", "Courses");

    }*/


    public IActionResult SaveCourse(int id, string returnUrl)
    {
        // Save the course logic here...

        // Redirect back to the original page
        return Redirect(returnUrl);
    }



}
