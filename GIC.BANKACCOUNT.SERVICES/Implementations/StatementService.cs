using GIC.BANKACCOUNT.COMMON;
using GIC.BANKACCOUNT.DATA.Entities;
using GIC.BANKACCOUNT.DATA.Repositories.Interfaces;
using GIC.BANKACCOUNT.MODELS.DTOs;
using GIC.BANKACCOUNT.MODELS.ResultModels;
using GIC.BANKACCOUNT.SERVICES.Interfaces;
using Microsoft.Extensions.Logging;

namespace GIC.BANKACCOUNT.SERVICES.Implementations
{
    public class StatementService : IStatementService
    {
        private readonly ILogger _logger;
        private readonly IIntrestRuleRepository _intrestRuleRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;


        public StatementService(
            ILogger<StatementService> logger,
            IIntrestRuleRepository intrestRuleRepository,
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository)
        {
            _logger = logger;
            _intrestRuleRepository = intrestRuleRepository;
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public List<StatementResultModel> GetStatement(GetStatementDto statementDto)
        {
            var statementModelList = new List<StatementResultModel>();

            try
            {
                if (_accountRepository.IsExistAccount(statementDto.Account))
                {
                    var rules = _intrestRuleRepository.GetIntrestRules(statementDto.Month);
                    var transactionsForSelectedMonth = _transactionRepository.GetTransactionsByMonth(statementDto.Account, statementDto.Month);

                    var lastMonthBalance = GetLastMonthBalance(statementDto.Account, statementDto.Month - 1);

                    statementModelList = GetStatement(lastMonthBalance, transactionsForSelectedMonth, rules, statementDto.Month);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return statementModelList;
        }

        private decimal GetLastMonthBalance(string acountNo, int tillMonth)
        {
            return _transactionRepository.GetTransactionAmountSum(acountNo, tillMonth);
        }

        private List<StatementResultModel> GetStatement(decimal accountBalance, List<Transaction> transactions, List<IntrestRule> rules, int month)
        {
            var result = new List<StatementResultModel>();

            try
            {
                var interest = GetInterest(accountBalance, transactions, rules, month);

                foreach (var transaction in transactions)
                {
                    accountBalance += transaction.Amount;

                    result.Add(new()
                    {
                        TxnId = transaction.TransactionNo,
                        Amount = transaction.Amount,
                        Balance = accountBalance,
                        Date = transaction.TransactionDate,
                        Type = transaction.Type
                    });
                }

                interest.Balance = accountBalance + interest.Amount;
                result.Add(interest);
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }

        private StatementResultModel GetInterest(decimal accountBalance, List<Transaction> transactions, List<IntrestRule> rules, int month)
        {
            var result = new StatementResultModel();

            try
            {
                transactions.Insert(0, new Transaction
                {
                    Type = TransactionType.DEPOSIT,
                    Amount = accountBalance,
                    TransactionDate = new DateTime(DateTime.Today.Year, month, 1, 0, 0, 0, DateTimeKind.Local)
                });

                var daysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, month);
                var interest = decimal.Zero;
                var balance = decimal.Zero;

                var transactionsByEoD = transactions.GroupBy(x => x.TransactionDate)
                                                    .Select(x => new TransactionDto
                                                    {
                                                        TransactionDate = x.Key,
                                                        Amount = x.Sum(s => s.Amount)
                                                    })
                                                    .ToList();

                foreach (var transactionByEoD in transactionsByEoD)
                {
                    var date = transactionByEoD.TransactionDate;
                    balance += transactionByEoD.Amount;

                    var rule = rules.LastOrDefault(x => x.EffectiveDate <= date);

                    if (rule is not null)
                    {
                        var lastDateFromRules = rules.Find(x => x.EffectiveDate > date);
                        var lastDateFromTransaction = transactionsByEoD.Find(x => x.TransactionDate > date);

                        var effectiveUntil = new[]
                        {
                            lastDateFromRules?.EffectiveDate,
                            lastDateFromTransaction?.TransactionDate
                        }
                        .Min();

                        var effectiveDaysCount = (effectiveUntil?.Day ?? daysInMonth + 1) - transactionByEoD.TransactionDate.Day;

                        interest += (balance * rule.Rate * effectiveDaysCount) / 100;

                        if (lastDateFromTransaction is not null && lastDateFromRules is not null && lastDateFromTransaction.TransactionDate > lastDateFromRules.EffectiveDate)
                        {
                            var rule2 = rules.LastOrDefault(x => x.EffectiveDate <= lastDateFromTransaction.TransactionDate);

                            if (rule2 is not null)
                            {
                                interest += (balance * rule2.Rate * (lastDateFromTransaction.TransactionDate.Day - rule2.EffectiveDate.Day)) / 100;
                            }
                        }
                    }
                }

                transactions.RemoveAt(0);

                result = new StatementResultModel
                {
                    Amount = Math.Round(interest / 365, 2),
                    Type = TransactionType.INTEREST,
                    Date = new DateTime(DateTime.Today.Year, month, daysInMonth, 0, 0, 0, DateTimeKind.Local)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }
    }
}