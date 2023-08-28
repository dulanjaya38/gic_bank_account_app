namespace GIC.BANKACCOUNT.MODELS.ResultModels
{
    public class StatementResultModel
    {
        public DateTime Date { get; set; }
        public string TxnId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }

    }
}
