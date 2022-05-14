using SharedExpenses.Storage.Models;

namespace SharedExpenses.Storage.Abstraction
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetUserAsync();
    }
}