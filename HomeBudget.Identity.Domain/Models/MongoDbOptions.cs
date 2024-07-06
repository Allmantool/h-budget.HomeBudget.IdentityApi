namespace HomeBudget.Identity.Domain.Models
{
    public record MongoDbOptions
    {
        public string ConnectionString { get; set; }
        public string UsersDatabaseName { get; set; }
    }
}
