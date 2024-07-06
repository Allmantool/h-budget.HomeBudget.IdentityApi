using System.Collections.Generic;
using System.Threading.Tasks;

using HomeBudget.Components.Users.Models;
using HomeBudget.Core.Models;
using HomeBudget.Identity.Infrastructure.Clients.Interfaces;

namespace HomeBudget.Components.Users.Clients.Interfaces
{
    internal interface IUserDocumentsClient : IDocumentClient
    {
        Task<Result<IReadOnlyCollection<UserDocument>>> GetAsync();

        Task<Result<UserDocument>> GetByEmailAsync(string email);

        Task<Result<UserDocument>> AddUserAsync(UserDocument user);
    }
}
