namespace HomeBudget.Identity.Domain.Interfaces
{
    public interface IEncryptor
    {
        string GetSalt();
        string GetHash(string value, string salt);
    }
}
