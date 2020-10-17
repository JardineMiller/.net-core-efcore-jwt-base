using System;

namespace API.Data.Models.Base
{
    public interface IDeletableEntity
    {
        DateTimeOffset? DeletedOn { get; set; }

        string DeletedBy { get; set; }

        bool IsDeleted { get; set; }
    }
}
