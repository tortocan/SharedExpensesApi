using Microsoft.EntityFrameworkCore;
using SharedExpenses.Storage.Abstraction;
using SharedExpenses.Storage.Models;

namespace SharedExpenses.Storage.Implementation
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUserAsync()
        {
            var result = await applicationDbContext.User.ToListAsync();
            return result;
        }
    }
}