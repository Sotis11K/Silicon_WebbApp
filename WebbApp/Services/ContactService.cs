using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace WebbApp.Services;

public class ContactService
{
    public interface IContactService
    {
        Task<bool> CreateContactAsync(ContactEntity contact);
        // Add other methods as needed
    }


    public class ContactServices : IContactService
    {
        private readonly ApplicationContext _dbContext;

        public ContactServices(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateContactAsync(ContactEntity contact)
        {
            try
            {
                _dbContext.Contacts.Add(contact);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
