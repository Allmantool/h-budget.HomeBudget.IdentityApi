using System;
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

            var userDocument = userDocumentResult.Payload;
            var user = userDocument?.Payload;

            return user;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            if (user != null)
            {
                user.Key = Guid.NewGuid();
            }

            var userDocumentResult = await userDocumentsClient
                .AddUserAsync(
                    new UserDocument
                    {
                        Payload = user
                    });

            var userDocument = userDocumentResult.Payload;

            return userDocument?.Payload;
        }
    }
}
