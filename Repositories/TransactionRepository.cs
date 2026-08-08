using Budget.Api.Data;
using Budget.Api.Data.Entities;
using Budget.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;
        public TransactionRepository(AppDbContext db) => _db = db;

        public async Task<List<Transaction>> GetAllForUserAsync(string userId) =>
            await _db.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

        public async Task<Transaction?> GetByIdAsync(int id) =>
            await _db.Transactions.FindAsync(id);

        public async Task AddAsync(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction) =>
            await _db.SaveChangesAsync();

        public async Task DeleteAsync(Transaction transaction)
        {
            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();
        }
    }
}