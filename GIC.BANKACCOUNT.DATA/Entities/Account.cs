namespace GIC.BANKACCOUNT.DATA.Entities
{
    public class Account
    {
        public int AcccountId { get; set; }
        public string AcccountNo { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Transaction> Transactions { get; } = new List<Transaction>();
    }
}
