using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace QuickProFixer.Models.Context
{
    public class QuickProFixerContextFactory : IDesignTimeDbContextFactory<QuickProFixerContext>
    {
        QuickProFixerContext IDesignTimeDbContextFactory<QuickProFixerContext>.CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<QuickProFixerContext>();
            builder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;Database=QuickProFixer_DB; Integrated Security=true; MultipleActiveResultSets=true;");
            return new QuickProFixerContext(builder.Options);
        }
    }
}
