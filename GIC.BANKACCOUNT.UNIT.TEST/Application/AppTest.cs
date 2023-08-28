using GIC.BANKACCOUNT.APP;

namespace GIC.BANKACCOUNT.UNIT.TEST.Application
{
    public class AppTest
    {
        #region Test GetCreateTransationDto

        [Fact]
        public void GetCreateTransationDto_Positive_Return_ValidDto()
        {
            //Arrange
            string inputString = "20230505|AC001|D|100.00";

            //Act
            var dto = App.GetCreateTransationDto(inputString);

            //Assert
            Assert.NotNull(dto);
            Assert.NotNull(dto.Account);
            Assert.NotNull(dto.Type);
            Assert.NotNull(dto?.Amount);
            Assert.NotNull(dto?.Date);
        }

        [Fact]
        public void GetCreateTransationDto_Negative_InvalidInputNoOfParameters_ReturnNullDto()
        {
            //Arrange
            string inputString = "20230505|AC001|W";

            //Act
            var dto = App.GetCreateTransationDto(inputString);

            //Assert
            Assert.Null(dto);
        }

        [Fact]
        public void GetCreateTransationDto_Negative_InvalidType_ReturnNullDto()
        {
            //Arrange
            string inputString = "20230505|AC001|A|100";

            //Act
            var dto = App.GetCreateTransationDto(inputString);

            //Assert
            Assert.Null(dto);
        }

        [Fact]
        public void GetCreateTransationDto_Positive_ValidTypes_ReturnDto()
        {
            //Arrange
            string inputString1 = "20230505|AC001|D|100";
            string inputString2 = "20230505|AC001|W|100";
            //Act
            var dto1 = App.GetCreateTransationDto(inputString1);
            var dto2 = App.GetCreateTransationDto(inputString2);

            //Assert
            Assert.NotNull(dto1);
            Assert.NotNull(dto2);
        }

        [Fact]
        public void GetCreateTransationDto_Negative_WithInvalidDecimalPointsAmount_ReturnNullDto()
        {
            //Arrange
            string inputString = "20230505|AC001|W|100.123";

            //Act
            var dto = App.GetCreateTransationDto(inputString);

            //Assert
            Assert.Null(dto);
        }

        [Fact]
        public void GetCreateTransationDto_Negative_WithInvalidMinAmount_ReturnNullDto()
        {
            //Arrange
            string inputString = "20230505|AC001|W|0";

            //Act
            var dto = App.GetCreateTransationDto(inputString);

            //Assert
            Assert.Null(dto);
        }

        #endregion

        #region Test GetCreateIntrestRuleDto

        [Fact]
        public void GetCreateIntrestRuleDto_Positive_Return_ValidDto()
        {
            //Arrange
            string inputString = "20230101|RULE01|1.95";

            //Act
            var dto = App.GetCreateIntrestRuleDto(inputString);

            //Assert
            Assert.NotNull(dto);
            Assert.NotNull(dto?.Date);
            Assert.NotNull(dto.RuleId);
            Assert.NotNull(dto?.Rate);

        }

        [Fact]
        public void GetCreateIntrestRuleDto_Negative_InvalidNoOfParameters_ReturnNullDto()
        {
            //Arrange
            string inputString = "20230505|AC001|W";

            //Act
            var dto = App.GetCreateIntrestRuleDto(inputString);

            //Assert
            Assert.Null(dto);
        }

        [Fact]
        public void GetCreateIntrestRuleDto_Negative_InvalidDecimalPointsRate_ReturnNullDto()
        {
            //Arrange
            string inputString = "20230505|AC001|W|100.123";

            //Act
            var dto = App.GetCreateIntrestRuleDto(inputString);

            //Assert
            Assert.Null(dto);
        }

        [Fact]
        public void GetCreateIntrestRuleDto_Negative_InvalidMinRate_ReturnNullDto()
        {
            //Arrange
            string inputString = "20230505|AC001|W|0";

            //Act
            var dto = App.GetCreateIntrestRuleDto(inputString);

            //Assert
            Assert.Null(dto);
        }

        [Fact]
        public void GetCreateIntrestRuleDto_Negative_InvalidMaxRate_ReturnNullDto()
        {
            //Arrange
            string inputString = "20230505|AC001|W|100.01";

            //Act
            var dto = App.GetCreateIntrestRuleDto(inputString);

            //Assert
            Assert.Null(dto);
        }

        #endregion

        #region Test GetGetStatementDto

        [Fact]
        public void GetGetStatementDto_Positive_Return_ValidDto()
        {
            //Arrange
            string inputString = "AC001|6";

            //Act
            var dto = App.GetGetStatementDto(inputString);

            //Assert
            Assert.NotNull(dto);
            Assert.NotNull(dto.Account);
            Assert.NotNull(dto?.Month);

        }

        [Fact]
        public void GetGetStatementDto_Negative_InvalidInput_ReturnNullDto()
        {
            //Arrange
            string inputString = "20230505|AC001|D";

            //Act
            var dto = App.GetGetStatementDto(inputString);

            //Assert
            Assert.Null(dto);
        }

        [Fact]
        public void GetGetStatementDto_Negative_InvalidMonthMin_ReturnNullDto()
        {
            //Arrange
            string inputString = "AC001|0";

            //Act
            var dto = App.GetGetStatementDto(inputString);

            //Assert
            Assert.Null(dto);
        }

        [Fact]
        public void GetGetStatementDto_Negative_InvalidMonthMax_ReturnNullDto()
        {
            //Arrange
            string inputString = "AC001|13";

            //Act
            var dto = App.GetGetStatementDto(inputString);

            //Assert
            Assert.Null(dto);
        }


        #endregion
    }
}
