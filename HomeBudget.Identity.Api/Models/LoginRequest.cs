namespace HomeBudget.Identity.Api.Models
{
    public record LoginRequest
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
