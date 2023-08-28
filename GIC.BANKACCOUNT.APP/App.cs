using ConsoleTables;
using GIC.BANKACCOUNT.COMMON;
using GIC.BANKACCOUNT.DATA.Entities;
using GIC.BANKACCOUNT.MODELS.DTOs;
using GIC.BANKACCOUNT.MODELS.ResultModels;
using GIC.BANKACCOUNT.SERVICES.Interfaces;
using Microsoft.Extensions.Logging;

namespace GIC.BANKACCOUNT.APP
{
    public class App
    {
        private readonly ILogger _logger;
        private readonly ITransationService _transationService;
        private readonly IIntrestRuleService _intrestRuleService;
        private readonly IStatementService _statementService;

        public App(
            ILogger<App> logger,
            ITransationService transationService,
            IIntrestRuleService intrestRuleService,
            IStatementService statementService)
        {
            _logger = logger;
            _transationService = transationService;
            _intrestRuleService = intrestRuleService;
            _statementService = statementService;
        }

        internal Task<bool> Run(bool isFistRun)
        {
            var result = true;

            try
            {
                ShowMenuSelection(isFistRun ? "Welcome to AwesomeGIC Bank! What would you like to do?"
                                            : Environment.NewLine + "Is there anything else you'd like to do?");

                string menuItem = Console.ReadLine() ?? string.Empty;

                switch (menuItem?.ToUpper())
                {
                    case MenuOption.INPUT_TRANSACTIONS:
                        InputTransaction();
                        break;
                    case MenuOption.DEFINE_INTEREST_RULES:
                        DefineInterestRules();
                        break;
                    case MenuOption.PRINT_STATEMENT:
                        PrinStatement();
                        break;
                    case MenuOption.QUIT:
                        result = false;
                        Console.WriteLine("Thank you for banking with AwesomeGIC Bank." + Environment.NewLine + "Have a nice day!");
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                result = false;
            }

            return Task.FromResult(result);

        }

        #region DefineInterestRules
        private void InputTransaction()
        {

            try
            {
                Console.WriteLine("Please enter transaction details in <Date>|<Account>|<Type>|<Amount> format" + Environment.NewLine + "(or enter blank to go back to main menu):");

                string? input = Console.ReadLine();

                var createTransationDto = GetCreateTransationDto(input);

                if (createTransationDto != null)
                {
                    var isCreatedTransaction = _transationService.CreateTransation(createTransationDto);

                    if (isCreatedTransaction)
                    {
                        Console.WriteLine("Transaction successully completed.");

                        var transactons = _transationService.GetTransactionsByAccountNo(createTransationDto.Account);

                        if (transactons?.Count > 0)
                        {
                            Console.WriteLine($"Account: {createTransationDto.Account}");

                            GenerateTransactionHistoryTable(transactons);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Transaction failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

        }

        private static void GenerateTransactionHistoryTable(List<Transaction> transactons)
        {
            if (transactons?.Count > 0)
            {
                var table = new ConsoleTable();
                table.Columns.Add("Date");
                table.Columns.Add("Txn Id");
                table.Columns.Add("Type");
                table.Columns.Add("Amount");

                foreach (var item in transactons)
                {
                    table.AddRow(
                        item.TransactionDate.ToString("yyyyMMdd"),
                        item.TransactionNo,
                        item.Type,
                        Math.Abs(item.Amount).ToString());
                }

                table.Write();
            }
        }

        public static CreateTransationDto? GetCreateTransationDto(string? input)
        {
            CreateTransationDto? result = null;
            bool isValidate = true;
            DateTime? date = null;
            string account = string.Empty;
            string type = string.Empty;
            decimal amount = 0;

            var inputs = input?.Split('|');

            if (inputs?.Length == 4)
            {
                date = ValidationHelper.ParseDateTime(inputs[0]);
                account = inputs[1].ToString();
                type = inputs[2].ToString();
                amount = ValidationHelper.ParseDecimal(inputs[3]);
                isValidate = ValidateTransationDto(date, account, type, amount);

            }
            else
            {
                Console.WriteLine("Invalid input format.");
                isValidate = false;
            }

            if (isValidate)
            {
                result = new CreateTransationDto
                {
                    Date = date.GetValueOrDefault(),
                    Account = account,
                    Type = type,
                    Amount = amount
                };
            }

            return result;
        }

        private static bool ValidateTransationDto(DateTime? date, string account, string type, decimal amount)
        {
            var isValidate = true;

            if (date is null)
            {
                Console.WriteLine("Invalid input format <Date>.");
                isValidate = false;
            }

            if (string.IsNullOrEmpty(account))
            {
                Console.WriteLine("Invalid input. Null or empty <Account>.");
                isValidate = false;
            }

            if (string.IsNullOrEmpty(type) || !(type.Equals(TransactionType.WITHDRAWAL) || type.Equals(TransactionType.DEPOSIT)))
            {
                Console.WriteLine("Invalid input. <Type>.");
                isValidate = false;
            }

            if (amount <= 0 || decimal.IsNegative(amount) || (amount * 100 % 1 != 0))
            {
                Console.WriteLine("Invalid input <Amount>.");
                isValidate = false;
            }

            return isValidate;
        }

        #endregion


        #region DefineInterestRules
        private void DefineInterestRules()
        {
            try
            {
                Console.WriteLine("Please enter interest rules details in <Date>|<RuleId>|<Rate in %> format" + Environment.NewLine + "(or enter blank to go back to main menu):");

                string? input = Console.ReadLine();

                var createIntrestRuleDto = GetCreateIntrestRuleDto(input);

                if (createIntrestRuleDto != null)
                {
                    var isRuleTransaction = _intrestRuleService.CreateIntrestRule(createIntrestRuleDto);

                    if (isRuleTransaction)
                    {
                        Console.WriteLine("Intrest Rule has been successully created.");

                        var rulesHistory = _intrestRuleService.GetIntrestRules();

                        if (rulesHistory?.Count > 0)
                        {
                            Console.WriteLine($"Interest rules:");
                            GenerateRulesHistoryTable(rulesHistory);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to create Intrest Rule");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }
        }
        private static void GenerateRulesHistoryTable(List<IntrestRule> rules)
        {
            if (rules?.Count > 0)
            {
                var table = new ConsoleTable();
                table.Columns.Add("Date");
                table.Columns.Add("RuleId");
                table.Columns.Add("Rate (%)");

                foreach (var item in rules)
                {
                    table.AddRow(
                        item.EffectiveDate.ToString("yyyyMMdd"),
                        item.RuleId,
                        item.Rate.ToString());
                }

                table.Write();
            }
        }

        public static CreateIntrestRuleDto? GetCreateIntrestRuleDto(string? input)
        {
            CreateIntrestRuleDto? result = null;
            bool isValidate;
            DateTime? date = null;
            string ruleId = string.Empty;
            decimal rate = 0;

            var inputs = input?.Split('|');

            if (inputs?.Length == 3)
            {
                date = ValidationHelper.ParseDateTime(inputs[0]);
                ruleId = inputs[1].ToString();
                rate = ValidationHelper.ParseDecimal(inputs[2]);

                isValidate = ValidateCreateIntrestRuleDto(date, ruleId, rate);
            }
            else
            {
                Console.WriteLine("Invalid input format.");
                isValidate = false;
            }

            if (isValidate)
            {
                result = new CreateIntrestRuleDto
                {
                    Date = date.GetValueOrDefault(),
                    Rate = rate,
                    RuleId = ruleId
                };
            }

            return result;
        }

        private static bool ValidateCreateIntrestRuleDto(DateTime? date, string ruleId, decimal rate)
        {
            var isValidate = true;

            if (date is null)
            {
                Console.WriteLine("Invalid input format <Date>.");
                isValidate = false;
            }

            if (string.IsNullOrEmpty(ruleId))
            {
                Console.WriteLine("Invalid input. Null or empty <RuleId>.");
                isValidate = false;
            }

            if (rate <= 0 || rate > 100 || decimal.IsNegative(rate) || (rate * 100 % 1 != 0))
            {
                Console.WriteLine("Invalid input <Rate>.");
                isValidate = false;
            }

            return isValidate;
        }


        #endregion


        #region PrinStatement
        private void PrinStatement()
        {
            try
            {
                Console.WriteLine("Please enter account and month to generate the statement <Account>|<Month>" + Environment.NewLine + "(or enter blank to go back to main menu):");

                string? input = Console.ReadLine();

                var getStatementDto = GetGetStatementDto(input);

                if (getStatementDto != null)
                {
                    var statementData = _statementService.GetStatement(getStatementDto);

                    if (statementData?.Count > 0)
                    {
                        GenerateStatementTable(statementData);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }
        }

        private static void GenerateStatementTable(List<StatementResultModel> transactons)
        {
            if (transactons?.Count > 0)
            {
                var table = new ConsoleTable();
                table.Columns.Add("Date");
                table.Columns.Add("Txn Id");
                table.Columns.Add("Type");
                table.Columns.Add("Amount");
                table.Columns.Add("Balance");

                foreach (var item in transactons)
                {
                    table.AddRow(
                        item.Date.ToString("yyyyMMdd"),
                        item.TxnId,
                        item.Type,
                        Math.Abs(item.Amount).ToString(),
                        item.Balance.ToString());
                }

                table.Write();
            }
        }

        public static GetStatementDto? GetGetStatementDto(string? input)
        {
            GetStatementDto? result = null;
            bool isValidate;
            string account = string.Empty;
            int month = 0;

            var inputs = input?.Split('|');

            if (inputs?.Length == 2)
            {
                account = inputs[0].ToString();
                month = ValidationHelper.ParseInt(inputs[1]);

                isValidate = ValidateStatementDto(account, month);
            }
            else
            {
                Console.WriteLine("Invalid input format.");
                isValidate = false;
            }

            if (isValidate)
            {
                result = new GetStatementDto
                {
                    Account = account,
                    Month = month
                };
            }

            return result;
        }

        private static bool ValidateStatementDto(string account, int month)
        {
            var isValidate = true;

            if (string.IsNullOrEmpty(account))
            {
                Console.WriteLine("Invalid input. Null or empty <Account>.");
                isValidate = false;
            }

            if (month <= 0 || month > 12)
            {
                Console.WriteLine("Invalid input <Rate>.");
                isValidate = false;
            }

            return isValidate;
        }

        #endregion


        #region Common Methods

        private static void ShowMenuSelection(string message)
        {
            Console.WriteLine(message + Environment.NewLine +
                              "[I]nput transactions" + Environment.NewLine +
                              "[D]efine interest rules" + Environment.NewLine +
                              "[P]rint statement" + Environment.NewLine +
                              "[Q]uit" + Environment.NewLine
                              );
        }

        #endregion
    }
}
