using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;

namespace NewsAPI.Data
{
    public class DataContext: DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
             
        }
 
        public DbSet<News> AllNews { get; init; }

    
    }
}