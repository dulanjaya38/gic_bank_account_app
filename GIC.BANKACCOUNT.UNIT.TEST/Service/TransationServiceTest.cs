using FakeItEasy;
using GIC.BANKACCOUNT.COMMON;
using GIC.BANKACCOUNT.DATA.Repositories.Implementations;
using GIC.BANKACCOUNT.MODELS.DTOs;
using GIC.BANKACCOUNT.SERVICES.Implementations;
using GIC.BANKACCOUNT.UNIT.TEST.Fixture;
using Microsoft.Extensions.Logging;

namespace GIC.BANKACCOUNT.UNIT.TEST.Service
{
    public class TransationServiceTest
    {

        private TestAppDbContext _dbContext = null!;
        private TransationService _transationService = null!;

        public TransationServiceTest()
        {
            Setup();
        }

        private void Setup()
        {
            var _loggerTransationService = A.Fake<ILogger<TransationService>>();
            var _loggerAccountRepository = A.Fake<ILogger<AccountRepository>>();
            var _loggerTransactionRepository = A.Fake<ILogger<TransactionRepository>>();


            _dbContext = TestAppDbContext.GetTestDatabaseContext();
            var _transactionRepository = new TransactionRepository(_loggerTransactionRepository, _dbContext);
            var _accountRepository = new AccountRepository(_loggerAccountRepository, _dbContext);

            _transationService = new TransationService(_loggerTransationService, _accountRepository, _transactionRepository);
        }

        [Fact]
        public void CreateTransation_Positive_CreateNewAccount_ReturnTrue()
        {
            //Arrange
            var dto = new CreateTransationDto
            {
                Account = "AC001",
                Type = TransactionType.DEPOSIT,
                Amount = 100,
                Date = DateTime.Now.Date,

            };

            //Act
            var result = _transationService.CreateTransation(dto);

            //Assert
            var createTransationResult = _dbContext.Accounts.FirstOrDefault(x => x.AcccountNo == dto.Account);

            Assert.True(result);
            Assert.NotNull(createTransationResult);
        }

        [Fact]
        public void CreateTransation_Positive_WithdrawalScenario_ReturnTrue()
        {

            //Arrange
            _dbContext.AddRange(TransactionsFixture.GetTransactionsRecords());
            _dbContext.SaveChanges();

            var dto = new CreateTransationDto
            {
                Account = "AC002",
                Type = TransactionType.WITHDRAWAL,
                Amount = 100,
                Date = DateTime.Now.Date,

            };

            //Act
            var result = _transationService.CreateTransation(dto);

            //Assert
            var createTransationResult = _dbContext.Accounts.FirstOrDefault(x => x.AcccountNo == dto.Account);

            Assert.True(result);
            Assert.NotNull(createTransationResult);
        }

        [Fact]
        public void CreateTransation_Negative_WithInvalidTransactionType_ReturnFalse()
        {

            //Arrange
            var dto = new CreateTransationDto
            {
                Account = "1000893",
                Type = "Z",
                Amount = 100,
                Date = DateTime.Now.Date,

            };

            //Act
            var result = _transationService.CreateTransation(dto);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateTransaction_Negative_WithdrawMoreThanAccBalance_ReturnFalse()
        {
            //Arrange 

            _dbContext.AddRange(TransactionsFixture.GetTransactionsRecords());
            _dbContext.SaveChanges();

            var dto = new CreateTransationDto
            {
                Account = "AC002",
                Type = TransactionType.WITHDRAWAL,
                Amount = 151,
                Date = DateTime.Now.Date,

            };

            //Act
            var result = _transationService.CreateTransation(dto);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateTransaction_Positive_WithdrawValidAmount_ReturnTrue()
        {
            //Arrange 

            _dbContext.AddRange(TransactionsFixture.GetTransactionsRecords());
            _dbContext.SaveChanges();

            var dto = new CreateTransationDto
            {
                Account = "AC002",
                Type = TransactionType.WITHDRAWAL,
                Amount = 150,
                Date = DateTime.Now.Date,

            };

            //Act
            var result = _transationService.CreateTransation(dto);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CreateTransation_Negative_WithNullDto_ReturnFalse()
        {

            //Arrange //Act
            var result = _transationService.CreateTransation(null);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void GetTransactionsByAccountNo_Positive_Return_TransactionList()
        {
            //Arrange 
            string acccountNo = "20230909";
            _dbContext.AddRange(TransactionsFixture.GetSecondTransactionsRecords());
            _dbContext.SaveChanges();

            //Act
            var result = _transationService.GetTransactionsByAccountNo(acccountNo);

            //Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetTransactionsByAccountNo_Negative_Return_EmptyResult()
        {
            //Arrange 
            string acccountNo = int.MaxValue.ToString();

            //Act
            var result = _transationService.GetTransactionsByAccountNo(acccountNo);

            //Assert
            Assert.Empty(result);
        }



    }
}
