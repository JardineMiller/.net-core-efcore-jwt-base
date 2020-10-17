using System;
using API.Data;

namespace API.UnitTests.Common
{
    public class CommandTestBase : IDisposable
    {
        protected readonly ApplicationDbContext Context;

        protected CommandTestBase()
        {
            this.Context = DbContextFactory.Create();
        }

        public void Dispose()
        {
            DbContextFactory.Destroy(this.Context);
        }
    }
}
