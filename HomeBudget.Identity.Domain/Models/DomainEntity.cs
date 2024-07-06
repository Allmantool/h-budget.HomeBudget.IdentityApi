using System;

namespace HomeBudget.Identity.Domain.Models
{
    public abstract class DomainEntity
    {
        public Guid Key { get; set; }

        public long OperationUnixTime { get; protected set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
