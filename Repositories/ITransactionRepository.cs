using Budget.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Repositories
{
	public interface ITransactionRepository
	{
		Task<List<Transaction>> GetAllForUserAsync(string userId);
		Task<Transaction?> GetByIdAsync(int id);
		Task AddAsync(Transaction transaction);
		Task UpdateAsync(Transaction transaction);
		Task DeleteAsync(Transaction transaction);
	}
}