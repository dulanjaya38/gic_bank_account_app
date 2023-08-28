using GIC.BANKACCOUNT.DATA.Entities;

namespace GIC.BANKACCOUNT.DATA.Repositories.Interfaces
{
    public interface IIntrestRuleRepository : IBaseRepository
    {
        int Create(IntrestRule rule);
        List<IntrestRule> GetIntrestRules();
        List<IntrestRule> GetIntrestRules(int tillMonth);

    }
}
