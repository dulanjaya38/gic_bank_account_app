using FakeItEasy;
using GIC.BANKACCOUNT.DATA.Repositories.Implementations;
using GIC.BANKACCOUNT.MODELS.DTOs;
using GIC.BANKACCOUNT.SERVICES.Implementations;
using GIC.BANKACCOUNT.UNIT.TEST.Fixture;
using Microsoft.Extensions.Logging;

namespace GIC.BANKACCOUNT.UNIT.TEST.Service
{
    public class IntrestRuleServiceTest
    {

        private TestAppDbContext _dbContext = null!;
        private IntrestRuleService _intrestRuleService = null!;

        public IntrestRuleServiceTest()
        {
            Setup();
        }

        private void Setup()
        {
            var _loggerIntrestRuleRepository = A.Fake<ILogger<IntrestRuleRepository>>();
            var _loggerIntrestRuleService = A.Fake<ILogger<IntrestRuleService>>();
            _dbContext = TestAppDbContext.GetTestDatabaseContext();
            var _intrestRuleRepository = new IntrestRuleRepository(_loggerIntrestRuleRepository, _dbContext);
            _intrestRuleService = new IntrestRuleService(_loggerIntrestRuleService, _intrestRuleRepository);
        }

        [Fact]
        public void CreateIntrestRule_Positive_AddRecord_ReturnTrue()
        {
            //Arrange
            var dto = new CreateIntrestRuleDto
            {
                Date = DateTime.Now,
                Rate = 10,
                RuleId = Guid.NewGuid().ToString(),
            };

            //Act
            var result = _intrestRuleService.CreateIntrestRule(dto);

            //Assert
            var createIntrestRuleResult = _dbContext.IntrestRules.FirstOrDefault(x => x.RuleId == dto.RuleId);

            Assert.True(result);
            Assert.NotNull(createIntrestRuleResult);
        }

        [Fact]
        public void CreateIntrestRule_Negative_ReturnFalse()
        {
            //Arrange & Act
            var result = _intrestRuleService.CreateIntrestRule(null);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void GetIntrestRules_Positive_ReturnGetIntrestRulesList()
        {
            //Arrange
            var dto = new CreateIntrestRuleDto
            {
                Date = DateTime.Now,
                Rate = 10,
                RuleId = Guid.NewGuid().ToString(),
            };

            //Act
            _intrestRuleService.CreateIntrestRule(dto);
            var result = _intrestRuleService.GetIntrestRules();

            //Assert
            if (result is not null)
            {
                var createIntrestRuleResult = result.First(x => x.RuleId == dto.RuleId);

                Assert.NotEmpty(result);
                Assert.NotNull(createIntrestRuleResult);
            }

        }

    }
}
