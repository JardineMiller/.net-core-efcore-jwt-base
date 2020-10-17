using System;

namespace API.Data.Models.Base
{
    public interface IAuditEntity
    {
        DateTimeOffset CreatedOn { get; set; }

        string CreatedBy { get; set; }

        DateTimeOffset? ModifiedOn { get; set; }

        string ModifiedBy { get; set; }
    }
}
