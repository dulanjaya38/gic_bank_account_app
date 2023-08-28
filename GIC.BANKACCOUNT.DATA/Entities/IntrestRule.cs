namespace GIC.BANKACCOUNT.DATA.Entities
{
    public class IntrestRule
    {
        public int IntrestRuleId { get; set; }
        public string RuleId { get; set; }
        public decimal Rate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
    }
}
