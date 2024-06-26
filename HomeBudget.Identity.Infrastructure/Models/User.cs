using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using HomeBudget.Identity.Domain.Interfaces;

namespace HomeBudget.Identity.Infrastructure.Models;

public class User
{
    public static readonly string DocumentName = nameof(User);

    required public string Email { get; init; }
    required public string Password { get; set; }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; }
    public string Salt { get; set; }
    public bool IsAdmin { get; init; }

    public void SetPassword(string password, IEncryptor encryptor)
    {
        Salt = encryptor.GetSalt();
        Password = encryptor.GetHash(password, Salt);
    }

    public bool ValidatePassword(string password, IEncryptor encryptor) => Password == encryptor.GetHash(password, Salt);
}