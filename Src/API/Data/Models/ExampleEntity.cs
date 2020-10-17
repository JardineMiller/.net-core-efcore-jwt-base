using API.Data.Models.Base;

namespace API.Data.Models
{
    public class ExampleEntity : DeletableEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
