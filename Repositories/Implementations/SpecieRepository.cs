using Fischt.Models;
using Microsoft.EntityFrameworkCore;

namespace Fischt.Repositories
{
    public class SpecieRepository : ISpecieRepository
    {
        private readonly FischtDbContext _context;

        public SpecieRepository(FischtDbContext context)
        {
            _context = context;
        }

        public async Task<List<Specie>> GetAllAsync()
        {
            return await _context.Species.ToListAsync();
        }

        public async Task<Specie?> GetByIdAsync(int id)
        {
            return await _context.Species.FindAsync(id);
        }

        public async Task CreateAsync(string name)
        {
            _context.Species.Add(new Specie { Name = name });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, string name)
        {
            var specie = await _context.Species.FindAsync(id);
            if (specie == null) return;
            specie.Name = name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var profiles = await _context.Profiles
                .Where(p => p.SpecieId == id)
                .ToListAsync();

            foreach (var profile in profiles)
                profile.SpecieId = null;

            await _context.SaveChangesAsync();

            var specie = await _context.Species.FindAsync(id);
            if (specie == null) return;

            _context.Species.Remove(specie);
            await _context.SaveChangesAsync();
        }
    }
}