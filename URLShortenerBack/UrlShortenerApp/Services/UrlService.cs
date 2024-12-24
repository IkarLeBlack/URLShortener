using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data;
using URL_Shortener.Models;
using System.Linq;
using System.Threading.Tasks;

namespace URL_Shortener.Services
{
    public class UrlService
    {
        private readonly ApplicationDbContext _context;

        public UrlService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<UrlModel>> GetUrlsAsync()
        {
            return await Task.FromResult(_context.Urls.AsQueryable());
        }

        public async Task<UrlModel> GetUrlByIdAsync(int id)
        {
            return await _context.Urls.FindAsync(id);
        }

        public async Task AddUrlAsync(UrlModel urlModel)
        {
            await _context.Urls.AddAsync(urlModel);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteUrlAsync(int id)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(u => u.Id == id);
            if (url != null)
            {
                _context.Urls.Remove(url);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<bool> IsUrlExistsAsync(string originalUrl)
        {
            return await _context.Urls.AnyAsync(u => u.OriginalUrl == originalUrl);
        }
        
        
    }
}