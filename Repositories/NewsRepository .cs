using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using NewsAPI.Data;
namespace NewsAPI.Repositories
{  
    public class NewsRepository : INewsRepository
    {
        private readonly IDataContext _context;
    public NewsRepository(IDataContext context)
    {
      _context = context;
 
    }
    public async Task Add(News news)
    {     
        _context.AllNews.Add(news);
        await _context.SaveChangesAsync();
    }
 
    public async Task Delete(Guid id)
    {
        var itemToRemove = await _context.AllNews.FindAsync(id);
        if (itemToRemove == null)
            throw new NullReferenceException();
         
        _context.AllNews.Remove(itemToRemove);
        await _context.SaveChangesAsync();
    }
 
    public async Task<News> Get(Guid id)
    {
        return await _context.AllNews.FindAsync(id);
    }
 
    public async Task<IEnumerable<News>> GetAll()
    {
        return await _context.AllNews.ToListAsync();
    }


        public async Task Update(News news)
    {
        var itemToUpdate = await _context.AllNews.FindAsync(news.NewsId);
        if (itemToUpdate == null)
            throw new NullReferenceException();
        itemToUpdate.Title = news.Title;
        itemToUpdate.Description = news.Description;
        await _context.SaveChangesAsync();
 
    }
    }
}