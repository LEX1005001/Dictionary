using Microsoft.EntityFrameworkCore;

namespace DictionaryWebApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Theme> Themes => Set<Theme>();
        public DbSet<Word_Tr> Words_Themes => Set<Word_Tr>();

    }
}
