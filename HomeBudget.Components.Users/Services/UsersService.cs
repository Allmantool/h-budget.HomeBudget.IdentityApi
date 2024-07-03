using System.Threading.Tasks;

using HomeBudget.Components.Users.Clients.Interfaces;
using HomeBudget.Components.Users.Models;
using HomeBudget.Components.Users.Services.Interfaces;
using HomeBudget.Identity.Domain.Models;

namespace HomeBudget.Components.Users.Services
{
    internal class UsersService(IUserDocumentsClient userDocumentsClient)
        : IUsersService
    {
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var userDocumentResult = await userDocumentsClient.GetByEmailAsync(email);

            var userDocument = userDocumentResult.payload;

            return userDocument.Payload;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            var userDocumentResult = await userDocumentsClient
                .AddUserAsync(
                    new UserDocument
                    {
                        Payload = user
                    });

            var userDocument = userDocumentResult.payload;

            return userDocument.Payload;
        }
    }
}
