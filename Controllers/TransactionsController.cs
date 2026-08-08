using Budget.Api.Data;
using Budget.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Budget.Api.Repositories;

namespace Budget.Api.Controllers
{
	[ApiController]
	[Route("api/transactions")]
	[Authorize]
	public class TransactionsController : ControllerBase
	{
		private readonly ITransactionRepository _transactionRepository;

		public TransactionsController(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		private string CurrentUserId =>
			User.FindFirst(ClaimTypes.NameIdentifier)?.Value
			?? User.FindFirst("sub")?.Value
			?? throw new UnauthorizedAccessException();

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var transactions = await _transactionRepository.GetAllForUserAsync(CurrentUserId);
			return Ok(transactions);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateTransactionDto dto)
		{
			var transaction = new Transaction
			{
				UserId = CurrentUserId,
				Amount = dto.Amount,
				Description = dto.Description,
				Date = dto.Date
			};

			await _transactionRepository.AddAsync(transaction);
			return Ok(transaction);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, UpdateTransactionDto dto)
		{
			var transaction = await _transactionRepository.GetByIdAsync(id);
			if (transaction == null)
				return NotFound();

			if (transaction.UserId != CurrentUserId)
				return Forbid();

			transaction.Amount = dto.Amount;
			transaction.Description = dto.Description;
			transaction.Date = dto.Date;

			await _transactionRepository.UpdateAsync(transaction);
			return Ok(transaction);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var transaction = await _transactionRepository.GetByIdAsync(id);
			if (transaction == null)
				return NotFound();

			if (transaction.UserId != CurrentUserId)
				return Forbid();

			await _transactionRepository.DeleteAsync(transaction);
			return NoContent();
		}
	}
}