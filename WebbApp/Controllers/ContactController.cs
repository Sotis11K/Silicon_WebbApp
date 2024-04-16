using Microsoft.AspNetCore.Mvc;
using WebbApp.ViewModels;
using Infrastructure.Entities;
using static WebbApp.Services.ContactService;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace WebbApp.Controllers
{


    public class ContactController(HttpClient httpClient) : Controller
    {

        private readonly HttpClient _httpClient = httpClient;


        //var response = await _httpClient.PostAsync("https://localhost:7199/api/subscribe?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1", content);

        [Route("/contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("/contact")]
        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("https://localhost:7199/api/contact?key=ZWM5MTYxNmQtNzE0Mi00NDU3LTg4ZjgtYjIwYmFhODZkMjQ1", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["StatusMessage"] = "Contact successfully created";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        TempData["StatusMessage"] = "Contact already created";
                    }
                }
                catch
                {
                    ViewData["StatusMessage"] = "Connection Failed";
                }
            }
            else
            {
                TempData["StatusMessage"] = "Invalid email address";
            }
            return RedirectToAction("Contact", "Contact", "Contact");


























            /*if (ModelState.IsValid)
            {
                var contactEntity = new ContactEntity
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Service = model.Service,
                    Message = model.Message
                };

                if (await _contactService.CreateContactAsync(contactEntity))
                {
                    // Redirect to a success page or return a success message
                    return RedirectToAction("Success");
                }
                else
                {
                    // Handle failure to save to the database
                    ModelState.AddModelError(string.Empty, "Failed to save contact.");
                    return View(model);
                }
            }

            return View(model);*/
        }

    }
}
