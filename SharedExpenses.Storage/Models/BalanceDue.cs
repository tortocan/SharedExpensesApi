namespace SharedExpenses.Storage.Models
{
    public class BalanceDue
    {
        public ApplicationUser To { get; internal set; }
        public ApplicationUser From { get; internal set; }
        public double Amount { get; internal set; }
    }
}