namespace Budget.Api.Models
{
	public class CreateTransactionDto
	{
		public decimal Amount { get; set; }
		public string? Description { get; set; }
		public DateTime Date { get; set; }
	}

	public class UpdateTransactionDto
	{
		public decimal Amount { get; set; }
		public string? Description { get; set; }
		public DateTime Date { get; set; }
	}
}