using Fischt.Models;

namespace Fischt.Repositories
{
    public interface ISpecieRepository
    {
        Task<List<Specie>> GetAllAsync();
        Task<Specie?> GetByIdAsync(int id);
        Task CreateAsync(string name);
        Task UpdateAsync(int id, string name);
        Task DeleteAsync(int id);
    }
}