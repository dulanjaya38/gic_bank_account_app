using GIC.BANKACCOUNT.DATA.Entities;
using GIC.BANKACCOUNT.MODELS.DTOs;

namespace GIC.BANKACCOUNT.SERVICES.Interfaces
{
    public interface IIntrestRuleService : IBaseService
    {
        bool CreateIntrestRule(CreateIntrestRuleDto ruleDto);
        List<IntrestRule>? GetIntrestRules();
    }
}
