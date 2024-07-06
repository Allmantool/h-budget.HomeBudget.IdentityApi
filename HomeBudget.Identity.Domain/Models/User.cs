using HomeBudget.Identity.Domain.Interfaces;

namespace HomeBudget.Identity.Domain.Models;

public class User : DomainEntity
{
    required public string Email { get; init; }
    required public string Password { get; set; }

    public string Salt { get; set; }
    public bool IsAdmin { get; init; }

    public void SetPassword(string password, IEncryptor encryptor)
    {
        Salt = encryptor.GetSalt();
        Password = encryptor.GetHash(password, Salt);
    }

    public bool ValidatePassword(string password, IEncryptor encryptor) => Password == encryptor.GetHash(password, Salt);
}