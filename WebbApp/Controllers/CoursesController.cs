

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using WebbApp.ViewModels;

namespace WebbApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly HttpClient _httpClient;

        public CoursesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        [Route("/courses")]
        public async Task<IActionResult> Index()
        {

            var viewModel = new CourseIndexViewModel();

            var response = await _httpClient.GetAsync("https://localhost:7199/api/courses?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1");
            if (response.IsSuccessStatusCode)
            {
                var courses = JsonConvert.DeserializeObject<IEnumerable<CourseViewModel>>(await response.Content.ReadAsStringAsync());
                if (courses != null && courses.Any())
                {
                    viewModel.Courses = courses;
                }
            }
            return View(viewModel);

        }



        [Route("/Courses/SingleCourse")]
        public async Task<IActionResult> SingleCourse(int id)
        {
            // Fetch the specific course details based on the id
            var response = await _httpClient.GetAsync($"https://localhost:7199/api/courses/{id}?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1");
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response into a CourseViewModel object
                var courseViewModel = JsonConvert.DeserializeObject<CourseViewModel>(await response.Content.ReadAsStringAsync());

                // Pass the courseViewModel to the view
                return View(courseViewModel); // This should return the view with the model data
            }
            else
            {
                // Handle the case where the course with the specified id is not found
                return NotFound();
            }
        }


    }
}





































/*
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebbApp.ViewModels;

namespace WebbApp.Controllers;



[Authorize]
public class CoursesController(HttpClient httpClient) : Controller
{

    
    
    private readonly HttpClient _httpClient = httpClient;



    [Route("/courses")]
    public async Task<IActionResult> Index()
    {

        var viewModel = new CourseIndexViewModel();

        var response = await _httpClient.GetAsync("https://localhost:7199/api/courses?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1");
        if (response.IsSuccessStatusCode)
        {
            var courses = JsonConvert.DeserializeObject<IEnumerable<CourseViewModel>>(await response.Content.ReadAsStringAsync());
            if(courses != null && courses.Any())
            {
                viewModel.Courses = courses;
            }
        }
        return View(viewModel);

    }

    viewModel = new CourseViewModel();


    [Route("/singlecourse/{_courseViewModel.id}")]
    public async Task<IActionResult> SingleCourse()
    {
        return View();
    }
}
*/