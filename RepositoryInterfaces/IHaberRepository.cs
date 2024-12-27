using HaberPortal.Models;

namespace HaberPortal.RepositoryInterfaces
{
    public interface IHaberRepository
    {
        Task<Haber> GetByIdAsync(int id);
        Task<IEnumerable<Haber>> GetAllAsync();
        Task AddAsync(Haber haber);
        Task UpdateAsync(Haber haber);
        Task DeleteAsync(int id);
    }
}
