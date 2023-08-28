using GIC.BANKACCOUNT.COMMON;
using GIC.BANKACCOUNT.DATA.Entities;
using GIC.BANKACCOUNT.DATA.Repositories.Interfaces;
using GIC.BANKACCOUNT.MODELS.DTOs;
using GIC.BANKACCOUNT.SERVICES.Interfaces;
using Microsoft.Extensions.Logging;

namespace GIC.BANKACCOUNT.SERVICES.Implementations
{
    public class IntrestRuleService : IIntrestRuleService
    {
        private readonly ILogger _logger;
        private readonly IIntrestRuleRepository _intrestRuleRepository;

        public IntrestRuleService(ILogger<IntrestRuleService> logger, IIntrestRuleRepository intrestRuleRepository)
        {
            _logger = logger;
            _intrestRuleRepository = intrestRuleRepository;
        }

        public bool CreateIntrestRule(CreateIntrestRuleDto ruleDto)
        {
            bool result = false;
            try
            {

                if (ruleDto != null)
                {
                    var newRuleModel = new IntrestRule
                    {
                        RuleId = ruleDto.RuleId,
                        Rate = ruleDto.Rate,
                        EffectiveDate = ruleDto.Date,
                        DateCreated = DateTime.Now,
                        IsActive = true
                    };

                    result = _intrestRuleRepository.Create(newRuleModel) > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppLogEvent.ERROR, ex, AppLogMessage.EXCEPTION);
                throw;
            }

            return result;
        }

        public List<IntrestRule>? GetIntrestRules()
        {
            var result = new List<IntrestRule>();

            try
            {
                result = _intrestRuleRepository.GetIntrestRules();
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