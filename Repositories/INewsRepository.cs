using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAPI.Models;

namespace NewsAPI.Repositories
{
    public interface INewsRepository
    {
        Task<News> Get(Guid id);
        Task<IEnumerable<News>> GetAll();
        Task Add(News news);
        Task Delete(Guid id);
        Task Update(News news);
    }
}