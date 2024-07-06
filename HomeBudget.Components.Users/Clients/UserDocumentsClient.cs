using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using MongoDB.Driver;

using HomeBudget.Components.Users.Clients.Interfaces;
using HomeBudget.Components.Users.Models;
using HomeBudget.Core.Models;
using HomeBudget.Identity.Domain.Models;
using HomeBudget.Identity.Infrastructure.Clients;

namespace HomeBudget.Components.Users.Clients
{
    internal class UserDocumentsClient(IOptions<MongoDbOptions> dbOptions)
        : BaseDocumentClient(dbOptions.Value.ConnectionString, dbOptions.Value.UsersDatabaseName),
        IUserDocumentsClient
    {
        public async Task<Result<IReadOnlyCollection<UserDocument>>> GetAsync()
        {
            var targetCollection = await GetUsersCollectionAsync();

            var payload = await targetCollection.FindAsync(_ => true);

            return Result<IReadOnlyCollection<UserDocument>>.Succeeded(await payload.ToListAsync());
        }

        public async Task<Result<UserDocument>> GetByEmailAsync(string email)
        {
            var targetCollection = await GetUsersCollectionAsync();

            var user = await targetCollection
                .Find(u => string.Equals(u.Payload.Email, email, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync();

            return Result<UserDocument>.Succeeded(user);
        }

        public async Task<Result<UserDocument>> AddUserAsync(UserDocument user)
        {
            var targetCollection = await GetUsersCollectionAsync();

            await targetCollection.InsertOneAsync(user);

            return Result<UserDocument>.Succeeded(user);
        }

        private async Task<IMongoCollection<UserDocument>> GetUsersCollectionAsync()
        {
            var collection = MongoDatabase.GetCollection<UserDocument>("users");

            var collectionIndexes = await collection.Indexes.ListAsync();

            if (await collectionIndexes.AnyAsync())
            {
                return collection;
            }

            var indexKeysDefinition = Builders<UserDocument>.IndexKeys
                .Ascending(c => c.Payload.Key)
                .Ascending(c => c.Payload.Email);

            await collection.Indexes.CreateOneAsync(new CreateIndexModel<UserDocument>(indexKeysDefinition));

            return collection;
        }
    }
}