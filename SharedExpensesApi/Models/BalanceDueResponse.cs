namespace SharedExpensesApi.Models
{
    public class BalanceDueResponse
    {
        public ApplicationUserResponse To { get;  set; }
        public ApplicationUserResponse From { get;  set; }
        public double Amount { get;  set; }
    }
}