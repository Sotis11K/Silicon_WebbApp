using Newtonsoft.Json;
using WebbApp.ViewModels;

namespace WebbApp.Services;

public class CategoryService(HttpClient httpClient, IConfiguration configuration)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;


    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync(_configuration["ApiUris:Categories"]);

        if(response.IsSuccessStatusCode)
        {
            var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(await response.Content.ReadAsStringAsync());
            return categories ??= null!;
        }

        return null!;
    }
        
}

        
//var response = await _httpClient.GetAsync("https://localhost:7199/api/courses?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1");
