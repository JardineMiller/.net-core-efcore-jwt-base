using System;
using API.Data;
using API.Data.Models;
using API.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace API.UnitTests.Common
{
    public static class DbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var user1 = new User() {Id = "0001", UserName = "User 1"};
            var user2 = new User() {Id = "0002", UserName = "User 2"};

            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.GetId()).Returns(user1.Id);
            currentUserServiceMock.Setup(m => m.GetUserName()).Returns(user1.UserName);

            var context = new ApplicationDbContext(options, currentUserServiceMock.Object);

            context.Database.EnsureCreated();

            context.Users.AddRange(new[]
            {
                user1,
                user2,
            });

            context.ExampleEntities.AddRange(new[]
            {
                new ExampleEntity {Id = 1, Description = "ExampleEntity 1", ImageUrl = "example1.jpg", UserId = user1.Id},
                new ExampleEntity {Id = 2, Description = "ExampleEntity 2", ImageUrl = "example2.jpg", UserId = user1.Id},
                new ExampleEntity {Id = 3, Description = "ExampleEntity 3", ImageUrl = "example3.jpg", UserId = user2.Id}
            });

            context.SaveChanges();

            return context;
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
