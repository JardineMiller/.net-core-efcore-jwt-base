using System;
using API.Data;

namespace API.UnitTests.Common
{
    public class QueryTestBase : IDisposable
    {
        public QueryTestBase()
        {
            this.Context = DbContextFactory.Create();
        }

        public ApplicationDbContext Context { get; }

        public void Dispose()
        {
            DbContextFactory.Destroy(this.Context);
        }
    }
}
