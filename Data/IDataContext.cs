using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;

namespace NewsAPI.Data
{
    public interface IDataContext
    {
        DbSet<News> AllNews { get; init; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
         
}