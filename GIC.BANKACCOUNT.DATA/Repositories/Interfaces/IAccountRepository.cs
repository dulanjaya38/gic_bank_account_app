using GIC.BANKACCOUNT.DATA.Entities;

namespace GIC.BANKACCOUNT.DATA.Repositories.Interfaces
{
    public interface IAccountRepository : IBaseRepository
    {
        int Create(Account account);
        bool IsExistAccount(string accountNo);
        Account? GetAccount(string accountNo);
    }
}
