using GIC.BANKACCOUNT.DATA.Entities;
using Microsoft.EntityFrameworkCore;

namespace GIC.BANKACCOUNT.UNIT.TEST.Fixture
{
    public class TestAppDbContext : AppDbContext
    {
        public TestAppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public static TestAppDbContext GetTestDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbcontext = new TestAppDbContext(options);
            dbcontext.Database.EnsureCreated();

            return dbcontext;
        }
    }
}
