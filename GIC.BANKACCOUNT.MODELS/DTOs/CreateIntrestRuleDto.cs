namespace GIC.BANKACCOUNT.MODELS.DTOs
{
    public class CreateIntrestRuleDto
    {
        public DateTime Date { get; set; }
        public string RuleId { get; set; }
        public decimal Rate { get; set; }
    }
}
