using HaberPortal.Hubs;
using HaberPortal.Models;
using HaberPortal.RepositoryInterfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HaberPortal.Repositories
{
    public class HaberRepository : IHaberRepository
    {
        private readonly ApplicationDbContext _context;
        
        public HaberRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }

        public async Task<Haber> GetByIdAsync(int id)
        {
            return await _context.Haberler.Include(h => h.Yazar).FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<Haber>> GetAllAsync()
        {
            return await _context.Haberler.Include(h => h.Yazar).ToListAsync();
        }

        public async Task AddAsync(Haber haber)
        {
            _context.Haberler.Add(haber);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Haber haber)
        {
            _context.Haberler.Update(haber);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var haber = await _context.Haberler.FindAsync(id);
            if (haber != null)
            {
                _context.Haberler.Remove(haber);
                await _context.SaveChangesAsync();
            }
        }
    }

}
