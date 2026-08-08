using Budget.Api.Data;
using Budget.Api.Data.Entities;
using Budget.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _db;
        public CategoryRepository(AppDbContext db) => _db = db;

        public async Task<List<Category>> GetAllForUserAsync(string userId) =>
            await _db.Categories.Where(c => c.UserId == userId).ToListAsync();

        public async Task<Category?> GetByIdAsync(int id) =>
            await _db.Categories.FindAsync(id);

        public async Task AddAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category) =>
            await _db.SaveChangesAsync();

        public async Task DeleteAsync(Category category)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }
    }
}