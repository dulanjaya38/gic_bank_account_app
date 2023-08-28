using GIC.BANKACCOUNT.DATA.Entities;
using GIC.BANKACCOUNT.MODELS.DTOs;


namespace GIC.BANKACCOUNT.SERVICES.Interfaces
{
    public interface ITransationService : IBaseService
    {
        bool CreateTransation(CreateTransationDto tansactionDto);
        List<Transaction> GetTransactionsByAccountNo(string accountNo);
    }
}
