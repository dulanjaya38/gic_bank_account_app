namespace GIC.BANKACCOUNT.MODELS.DTOs
{
    public class CreateTransationDto
    {
        public string Account { get; set; }
        public string Type { get; set; }
        public Decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
