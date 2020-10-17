using System;

namespace API.Data.Models.Base
{
    public abstract class AuditEntity : IAuditEntity
    {
        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
