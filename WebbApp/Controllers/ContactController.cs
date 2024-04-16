using Microsoft.AspNetCore.Mvc;
using WebbApp.ViewModels;
using Infrastructure.Entities;
using static WebbApp.Services.ContactService;

namespace WebbApp.Controllers
{


    public class ContactController(IContactService contactService) : Controller
    {

        private readonly IContactService _contactService = contactService;

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

            return View(model);
        }

    }
}
