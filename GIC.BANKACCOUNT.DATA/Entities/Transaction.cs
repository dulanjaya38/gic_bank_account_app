namespace GIC.BANKACCOUNT.DATA.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string TransactionNo { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;
    }
}
