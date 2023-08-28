namespace GIC.BANKACCOUNT.COMMON
{
    public static class AppLogEvent
    {
        public const int ERROR = 0;
        public const int SUCCESS = 1;
        public const int WARNING = 2;
        public const int INFORMATION = 3;
    }

    public static class AppLogMessage
    {
        public const string EXCEPTION = "An exception occurred.";
    }

    public static class TransactionType
    {
        public const string WITHDRAWAL = "W";
        public const string DEPOSIT = "D";
        public const string INTEREST = "I";
    }

    public static class MenuOption
    {
        public const string INPUT_TRANSACTIONS = "I";
        public const string DEFINE_INTEREST_RULES = "D";
        public const string PRINT_STATEMENT = "P";
        public const string QUIT = "Q";
    }
}