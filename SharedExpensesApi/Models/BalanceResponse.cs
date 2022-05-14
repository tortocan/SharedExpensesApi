namespace SharedExpensesApi.Models
{
    public class BalanceResponse
    {
        public ApplicationUserResponse User { get; internal set; }
        public double Amount { get; internal set; }
    }
}