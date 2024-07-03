using System.Threading.Tasks;

using HomeBudget.Identity.Domain.Models;

namespace HomeBudget.Components.Users.Services.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<User> RegisterUserAsync(User user);
    }
}
