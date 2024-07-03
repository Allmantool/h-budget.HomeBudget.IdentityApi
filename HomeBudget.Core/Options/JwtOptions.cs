namespace HomeBudget.Core.Options
{
    public record JwtOptions
    {
        public string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
