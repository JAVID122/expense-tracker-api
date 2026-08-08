using Budget.Api.Data.Entities;

namespace Budget.Api.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllForUserAsync(string userId);
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
    }
}