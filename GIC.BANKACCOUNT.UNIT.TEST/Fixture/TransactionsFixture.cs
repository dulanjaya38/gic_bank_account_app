using GIC.BANKACCOUNT.COMMON;
using GIC.BANKACCOUNT.DATA.Entities;

namespace GIC.BANKACCOUNT.UNIT.TEST.Fixture
{
    public class TransactionsFixture
    {
        protected TransactionsFixture()
        {

        }

        public static List<Transaction> GetTransactionsRecords()
        {
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    TransactionId = 1,
                    TransactionNo = "20230505-01",
                    Type = TransactionType.DEPOSIT,
                    Amount = 150,
                    TransactionDate = ValidationHelper.ParseDateTime("20230505") ?? DateTime.MinValue,
                    AccountId =2 ,
                    Account = new Account
                    {
                        AcccountId = 2,
                        AcccountNo = "AC002",
                        DateCreated = DateTime.Now.Date.AddDays(-5),
                        IsActive =  true
                    },
                }
            };

            return transactions;
        }

        public static List<Transaction> GetSecondTransactionsRecords()
        {
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    TransactionId = 3000,
                    TransactionNo = "20230909-02",
                    Type = TransactionType.DEPOSIT,
                    Amount = 150,
                    TransactionDate = DateTime.Now.Date,
                    AccountId = 1002 ,
                    Account = new Account
                    {
                        AcccountId = 1002,
                        AcccountNo = "20230909",
                        DateCreated = DateTime.Now.Date.AddDays(-5),
                        IsActive =  true
                    },
                }
            };

            return transactions;
        }

        public static List<Transaction> GetThirdTransactionsRecords()
        {
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    TransactionId = 2000,
                    TransactionNo = "20230505-01",
                    Type = TransactionType.DEPOSIT,
                    Amount = 100,
                    TransactionDate = ValidationHelper.ParseDateTime("20230505") ?? DateTime.MinValue,
                    AccountId = 1
                },
                new Transaction
                {
                    TransactionId = 2002,
                    TransactionNo = "20230601-01",
                    Type = TransactionType.DEPOSIT,
                    TransactionDate = ValidationHelper.ParseDateTime("20230601") ?? DateTime.MinValue,
                    Amount = 150,
                    AccountId = 1
                },
                new Transaction
                {
                    TransactionId = 2003,
                    TransactionNo = "20230626-01",
                    Type = TransactionType.WITHDRAWAL,
                    Amount = -20,
                    TransactionDate = ValidationHelper.ParseDateTime("20230626") ?? DateTime.MinValue,
                    AccountId = 1
                },
                new Transaction
                {
                    TransactionId = 2004,
                    TransactionNo = "20230626-02",
                    Type = TransactionType.WITHDRAWAL,
                    Amount = -100,
                    TransactionDate = ValidationHelper.ParseDateTime("20230626") ?? DateTime.MinValue,
                    AccountId = 1
                }
            };

            return transactions;
        }

        public static List<IntrestRule> GetIntrestRuleRecords()
        {
            var rules = new List<IntrestRule>
            {
                new IntrestRule
                {
                  IntrestRuleId = 1000,
                  RuleId = "RULE01",
                  EffectiveDate = ValidationHelper.ParseDateTime("20230101") ?? DateTime.MinValue,
                  Rate = 1.95m,
                  DateCreated =  DateTime.Now.Date.AddDays(-5),
                  IsActive = true,
                },
                new IntrestRule
                {
                  IntrestRuleId = 1001,
                  RuleId = "RULE02",
                  EffectiveDate = ValidationHelper.ParseDateTime("20230520") ?? DateTime.MinValue,
                  Rate = 1.90m,
                  DateCreated =  DateTime.Now.Date.AddDays(-5),
                  IsActive = true,
                },
                new IntrestRule
                {
                  IntrestRuleId = 1002,
                  RuleId = "RULE03",
                  EffectiveDate = ValidationHelper.ParseDateTime("20230615") ?? DateTime.MinValue,
                  Rate = 2.20m,
                  DateCreated =  DateTime.Now.Date.AddDays(-5),
                  IsActive = true,
                }
            };

            return rules;
        }

        public static List<Account> GetAccountRecords()
        {
            var rules = new List<Account>
            {
                new Account
                {
                  AcccountId = 1,
                  AcccountNo = "AC001",
                  DateCreated =  DateTime.Now.Date.AddDays(-5),
                  IsActive = true,
                }
            };

            return rules;
        }


    }
}
