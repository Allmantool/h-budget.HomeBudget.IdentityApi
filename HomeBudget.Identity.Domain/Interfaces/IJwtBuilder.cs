namespace HomeBudget.Identity.Domain.Interfaces
{
    public interface IJwtBuilder
    {
        string GetToken(string userId);
        string ValidateToken(string token);
    }
}
