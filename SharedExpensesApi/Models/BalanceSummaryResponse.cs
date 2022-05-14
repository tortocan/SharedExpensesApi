namespace SharedExpensesApi.Models
{
    public class BalanceSummaryResponse
    {
        public IEnumerable<BalanceResponse> Balance { get; set; }
        public IEnumerable<BalanceDueResponse> BalanceDue { get; set; }
    }
}