using FakeItEasy;
using GIC.BANKACCOUNT.DATA.Repositories.Implementations;
using GIC.BANKACCOUNT.MODELS.DTOs;
using GIC.BANKACCOUNT.SERVICES.Implementations;
using GIC.BANKACCOUNT.UNIT.TEST.Fixture;
using Microsoft.Extensions.Logging;

namespace GIC.BANKACCOUNT.UNIT.TEST.Service
{
    public class StatementServiceTest
    {

        private TestAppDbContext _dbContext = null!;
        private StatementService _statementService = null!;

        public StatementServiceTest()
        {
            Setup();
        }

        private void Setup()
        {
            var _loggerStatementService = A.Fake<ILogger<StatementService>>();
            var _loggerIntrestRuleRepository = A.Fake<ILogger<IntrestRuleRepository>>();
            var _loggerTransactionRepository = A.Fake<ILogger<TransactionRepository>>();
            var _loggerAccountRepository = A.Fake<ILogger<AccountRepository>>();

            _dbContext = TestAppDbContext.GetTestDatabaseContext();

            var _transactionRepository = new TransactionRepository(_loggerTransactionRepository, _dbContext);
            var _intrestRuleRepository = new IntrestRuleRepository(_loggerIntrestRuleRepository, _dbContext);
            var _accountRepository = new AccountRepository(_loggerAccountRepository, _dbContext);

            _statementService = new StatementService(_loggerStatementService, _intrestRuleRepository, _transactionRepository, _accountRepository);

            //insert data
            _dbContext.AddRange(TransactionsFixture.GetAccountRecords());
            _dbContext.AddRange(TransactionsFixture.GetThirdTransactionsRecords());
            _dbContext.AddRange(TransactionsFixture.GetIntrestRuleRecords());
            _dbContext.SaveChanges();
        }

        [Fact]
        public void GetStatement_Positive_GetStatementForGivenSenario01_ReturnObject()
        {
            //Arrange
            var dto = new GetStatementDto
            {
                Account = "AC001",
                Month = 6
            };

            //Act
            var result = _statementService.GetStatement(dto);

            //Assert          

            Assert.True(result.Count == 4);
            Assert.Equal(250, result[0].Balance);
            Assert.Equal(230, result[1].Balance);
            Assert.Equal(130, result[2].Balance);
            Assert.Equal(0.39m, result[3].Amount);
            Assert.Equal(130.39m, result[3].Balance);
        }

        [Fact]
        public void GetStatement_Negative_GetStatementForMonthBeforeCreateAccount_ReturnNullObject()
        {
            //Arrange
            var dto = new GetStatementDto
            {
                Account = "AC001",
                Month = 1
            };

            //Act
            var result = _statementService.GetStatement(dto);

            //Assert          

            Assert.True(result.Count == 1);
            Assert.Equal(0, result[0].Amount);
            Assert.Equal(0, result[0].Balance);
        }

        [Fact]
        public void GetStatement_Negative_GetStatementFrorInvalidAccount_ReturnNullObject()
        {
            //Arrange
            var dto = new GetStatementDto
            {
                Account = "AC00100",
                Month = 6
            };

            //Act
            var result = _statementService.GetStatement(dto);

            //Assert          

            Assert.Empty(result);
        }

    }
}
