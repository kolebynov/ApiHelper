using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TestRestApi
{
    public class TestContext : DbContext
    {
        public DbSet<TestEntity> TestEntities { get; set; }

        public TestContext(DbContextOptions options) : base(options)
        { }
    }
}
