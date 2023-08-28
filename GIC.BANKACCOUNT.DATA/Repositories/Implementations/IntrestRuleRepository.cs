using GIC.BANKACCOUNT.COMMON;
using GIC.BANKACCOUNT.DATA.Entities;
using GIC.BANKACCOUNT.DATA.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GIC.BANKACCOUNT.DATA.Repositories.Implementations
{
    public class IntrestRuleRepository : IIntrestRuleRepository
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _context;

        public IntrestRuleRepository(ILogger<IntrestRuleRepository> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public int Create(IntrestRule rule)
        {
            int result = -1;

            try
            {
                _context.IntrestRules.Add(rule);
                _context.Entry(rule).State = EntityState.Added;

                if (_context.SaveChanges() > 0)
                {
                    result = rule.IntrestRuleId;
                }

                _logger.LogInformation($"New IntrestRule created, IntrestRuleId: {result} RuleId: {rule.RuleId}");

            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }

        public List<IntrestRule> GetIntrestRules()
        {
            var result = new List<IntrestRule>();

            try
            {
                result = _context.IntrestRules
                                 .AsNoTracking()
                                 .OrderBy(x => x.EffectiveDate)
                                 .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }

        public List<IntrestRule> GetIntrestRules(int tillMonth)
        {
            var result = new List<IntrestRule>();

            try
            {
                var tillDate = new DateTime(DateTime.Now.Year, tillMonth, DateTime.DaysInMonth(DateTime.Now.Year, tillMonth), 0, 0, 0, DateTimeKind.Local);

                result = _context.IntrestRules
                                 .AsNoTracking()
                                 .Where(x => x.EffectiveDate <= tillDate && x.IsActive)
                                 .ToList();
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