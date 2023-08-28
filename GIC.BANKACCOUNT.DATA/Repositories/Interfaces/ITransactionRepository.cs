using GIC.BANKACCOUNT.DATA.Entities;

namespace GIC.BANKACCOUNT.DATA.Repositories.Interfaces
{
    public interface ITransactionRepository : IBaseRepository
    {
        int Create(Transaction transaction);
        List<Transaction> GetTransactionsByAccountNo(string accountNo);
        List<Transaction> GetTransactionsByMonth(string accountNo, int month);
        decimal GetTransactionAmountSum(string accountNo, int tillMonth);
        string GenerateRunningNumber(string date);
    }
}
