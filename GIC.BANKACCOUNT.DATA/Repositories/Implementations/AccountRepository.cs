using GIC.BANKACCOUNT.COMMON;
using GIC.BANKACCOUNT.DATA.Entities;
using GIC.BANKACCOUNT.DATA.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GIC.BANKACCOUNT.DATA.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _context;
        public AccountRepository(ILogger<AccountRepository> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public int Create(Account account)
        {
            int result = -1;

            try
            {
                _context.Accounts.Add(account);
                _context.Entry(account).State = EntityState.Added;

                if (_context.SaveChanges() > 0)
                {
                    result = account.AcccountId;
                }
                _logger.LogInformation($"New Account created, AccountId: {result}");

            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }

        public bool IsExistAccount(string accountNo)
        {
            bool result = false;

            try
            {
                result = _context.Accounts
                                 .AsNoTracking()
                                 .Any(x => x.AcccountNo == accountNo);
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }
        public Account? GetAccount(string accountNo)
        {
            Account? result = null;

            try
            {
                result = _context.Accounts
                                 .AsNoTracking()
                                 .SingleOrDefault(x => x.AcccountNo == accountNo);
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