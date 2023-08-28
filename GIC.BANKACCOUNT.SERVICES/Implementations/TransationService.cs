using GIC.BANKACCOUNT.COMMON;
using GIC.BANKACCOUNT.DATA.Entities;
using GIC.BANKACCOUNT.DATA.Repositories.Interfaces;
using GIC.BANKACCOUNT.MODELS.DTOs;
using GIC.BANKACCOUNT.SERVICES.Interfaces;
using Microsoft.Extensions.Logging;

namespace GIC.BANKACCOUNT.SERVICES.Implementations
{
    public class TransationService : ITransationService
    {
        private readonly ILogger _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransationService(
            ILogger<TransationService> logger,
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository
            )
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public bool CreateTransation(CreateTransationDto tansactionDto)
        {
            bool result = false;

            try
            {

                if (tansactionDto is not null)
                {
                    var account = _accountRepository.GetAccount(tansactionDto.Account);
                    var accountId = -1;
                    bool isValidTransation = true;

                    if (account is not null)
                    {
                        accountId = account.AcccountId;

                        if (tansactionDto.Type.Equals(TransactionType.WITHDRAWAL))
                        {
                            var currenrBalance = _transactionRepository.GetTransactionAmountSum(tansactionDto.Account, tansactionDto.Date.Month);

                            if (tansactionDto.Amount > currenrBalance)
                            {
                                Console.WriteLine("Insuffcient account balance. Please try again.");
                                isValidTransation = false;
                            }

                        }
                    }
                    else
                    {
                        if (tansactionDto.Type.Equals(TransactionType.DEPOSIT))
                        {
                            var newAccountModel = new GIC.BANKACCOUNT.DATA.Entities.Account
                            {
                                AcccountNo = tansactionDto.Account,
                                IsActive = true,
                                DateCreated = DateTime.Now,

                            };

                            accountId = _accountRepository.Create(newAccountModel);
                        }
                        else
                        {
                            Console.WriteLine("Account does not exists and please make transaction for deeposit to open a new account.");
                        }
                    }

                    if (accountId > 0 && isValidTransation)
                    {
                        var runingNumber = _transactionRepository.GenerateRunningNumber(tansactionDto.Date.ToString("yyyyMMdd"));

                        if (!string.IsNullOrEmpty(runingNumber))
                        {
                            var model = new Transaction
                            {
                                Type = tansactionDto.Type,
                                Amount = tansactionDto.Type.Equals(TransactionType.DEPOSIT) ? tansactionDto.Amount : decimal.Negate(tansactionDto.Amount),
                                AccountId = accountId,
                                TransactionDate = tansactionDto.Date,
                                TransactionNo = runingNumber
                            };

                            result = _transactionRepository.Create(model) > 0;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }

        public List<Transaction> GetTransactionsByAccountNo(string accountNo)
        {
            var result = new List<Transaction>();

            try
            {
                if (!string.IsNullOrEmpty(accountNo))
                {
                    result = _transactionRepository.GetTransactionsByAccountNo(accountNo);
                }
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