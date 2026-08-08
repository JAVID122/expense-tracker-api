namespace Budget.Api.Models.Transactions
{
    public class CreateTransactionDto
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int? CategoryId { get; set; }
    }
}
