using System;
using System.Collections.Generic;
using API.Data.Models.Base;
using Microsoft.AspNetCore.Identity;

namespace API.Data.Models
{
    public class User : IdentityUser, IAuditEntity
    {
        public IEnumerable<ExampleEntity> ExampleEntities { get; } = new HashSet<ExampleEntity>();

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
