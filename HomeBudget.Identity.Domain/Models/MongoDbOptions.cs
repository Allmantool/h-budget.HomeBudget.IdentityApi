namespace HomeBudget.Identity.Domain.Models
{
    public record MongoDbOptions
    {
        public string ConnectionString { get; init; }
        public string UsersDatabaseName { get; init; }
    }
}
