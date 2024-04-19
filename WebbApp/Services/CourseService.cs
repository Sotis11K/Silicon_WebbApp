using Newtonsoft.Json;
using WebbApp.ViewModels;

namespace WebbApp.Services;

public class CourseService(HttpClient http, IConfiguration configuration)
{
    private readonly HttpClient _http = http;
    private readonly IConfiguration _configuration = configuration;


    public async Task<CourseResult> GetCoursesAsync(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 10)
    {
        //var response = await _http.GetAsync($"{_configuration["ApiUris:Courses"]}?category={category}&key={_configuration["ApiUris:ApiKey"]}");

        var response = await _http.GetAsync($"{_configuration["ApiUris:Courses"]}?category={Uri.UnescapeDataString(category)}&searchQuery={Uri.UnescapeDataString(searchQuery)}&pageNumber={pageNumber}&pageSize={pageSize}&key={_configuration["ApiUris:ApiKey"]}");



        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<CourseResult>(await response.Content.ReadAsStringAsync());

            if(result != null && result.Succeeded == true)
            {
                return result;
            }


        }

        return null!;
    }

}
