namespace HomeBudget.Identity.Api.Models
{
    public record RegisterRequest
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
