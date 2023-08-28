using GIC.BANKACCOUNT.COMMON;
using GIC.BANKACCOUNT.DATA.Entities;
using GIC.BANKACCOUNT.DATA.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace GIC.BANKACCOUNT.DATA.Repositories.Implementations
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _context;

        public TransactionRepository(ILogger<TransactionRepository> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public int Create(Transaction transaction)
        {
            int result = -1;

            try
            {
                _context.Transactions.Add(transaction);
                _context.Entry(transaction).State = EntityState.Added;

                if (_context.SaveChanges() > 0)
                {
                    result = transaction.TransactionId;
                }

                _logger.LogInformation($"New Transaction created, TransactionId: {result} TransactionNo: {transaction.TransactionNo}");

            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
            }

            return result;
        }

        public List<Transaction> GetTransactionsByAccountNo(string accountNo)
        {
            var result = new List<Transaction>();

            try
            {
                result = _context.Transactions
                                 .AsNoTracking()
                                 .Where(x => x.Account.AcccountNo == accountNo)
                                 .OrderBy(x => x.TransactionDate)
                                 .ThenBy(x => x.TransactionNo)
                                 .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }

        public List<Transaction> GetTransactionsByMonth(string accountNo, int month)
        {
            var result = new List<Transaction>();

            try
            {
                result = _context.Transactions
                                 .AsNoTracking()
                                 .Where(x => x.Account.AcccountNo == accountNo &&
                                             x.TransactionDate.Year == DateTime.Today.Year &&
                                             x.TransactionDate.Month == month)
                                 .OrderBy(x => x.TransactionDate)
                                 .ThenBy(x => x.TransactionNo)
                                 .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }

        public decimal GetTransactionAmountSum(string accountNo, int tillMonth)
        {
            var result = decimal.Zero;

            try
            {
                result = _context.Transactions
                                 .AsNoTracking()
                                 .Where(x => x.Account.AcccountNo == accountNo &&
                                             x.TransactionDate.Year == DateTime.Now.Year &&
                                             x.TransactionDate.Month <= tillMonth)
                                 .Sum(x => x.Amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }

        public string GenerateRunningNumber(string date)
        {
            string result = string.Empty;

            try
            {
                var entity = _context.RunningNumbers.FirstOrDefault();

                if (entity is null)
                {
                    entity = new RunningNumber
                    {
                        DateStr = date,
                        Value = $"{date}-01"
                    };

                    _context.RunningNumbers.Add(entity);
                    _context.Entry(entity).State = EntityState.Added;
                }
                else
                {
                    var splitVal = entity.Value.Split('-');
                    var dateStr = splitVal[0];
                    var currentNumber = Convert.ToInt32(splitVal[1]);

                    if (dateStr.Equals(date))
                    {
                        currentNumber++;
                        entity.Value = $"{date}-{currentNumber.ToString("D2")}";
                    }
                    else
                    {
                        entity.Value = $"{date}-01";
                    }

                    _context.Entry(entity).State = EntityState.Modified;

                }

                _context.SaveChanges();

                result = entity.Value;

                _logger.LogInformation($"RunninNumber Value: {entity.Value}");

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