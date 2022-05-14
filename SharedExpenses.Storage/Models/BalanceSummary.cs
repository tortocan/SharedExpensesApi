namespace SharedExpenses.Storage.Models
{
    public class BalanceSummary
    {
        public IList<Balance> Balance { get; set; }
        public IList<BalanceDue> BalanceDue { get;  set; }
    }
}