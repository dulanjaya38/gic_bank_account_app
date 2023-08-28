using GIC.BANKACCOUNT.MODELS.DTOs;
using GIC.BANKACCOUNT.MODELS.ResultModels;

namespace GIC.BANKACCOUNT.SERVICES.Interfaces
{
    public interface IStatementService : IBaseService
    {
        List<StatementResultModel> GetStatement(GetStatementDto statementDto);
    }
}
