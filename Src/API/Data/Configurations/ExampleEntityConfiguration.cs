using API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static API.Data.ValidationConstants.ExampleEntity;

namespace API.Data.Configurations
{
    public class ExampleEntityConfiguration : IEntityTypeConfiguration<ExampleEntity>
    {
        public void Configure(EntityTypeBuilder<ExampleEntity> builder)
        {

            builder
                .HasKey(c => c.Id);

            builder
                .HasQueryFilter(c => !c.IsDeleted);

            builder
                .HasOne(c => c.User)
                .WithMany(u => u.ExampleEntities)
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(c => c.Description)
                .HasMaxLength(MaxDescriptionLength)
                .IsRequired();

            builder
                .Property(c => c.ImageUrl)
                .IsRequired();
        }
    }
}
