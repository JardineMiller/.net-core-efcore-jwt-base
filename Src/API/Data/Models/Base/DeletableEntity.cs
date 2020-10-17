using System;

namespace API.Data.Models.Base
{
    public class DeletableEntity : AuditEntity, IDeletableEntity
    {
        public DateTimeOffset? DeletedOn { get; set; }

        public string DeletedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
