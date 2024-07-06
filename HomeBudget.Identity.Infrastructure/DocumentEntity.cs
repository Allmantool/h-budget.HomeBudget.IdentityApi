using MongoDB.Bson;

namespace HomeBudget.Identity.Infrastructure
{
    public abstract class DocumentEntity<T>
        where T : class
    {
        public ObjectId Id { get; set; }

        public T Payload { get; init; }
    }
}
