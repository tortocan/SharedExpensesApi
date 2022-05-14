namespace SharedExpenses.Storage.Models
{
    public class Balance
    {
        public ApplicationUser User { get; internal set; }
        public double Amount { get; internal set; }
        public bool IsWithExpense { get; set; }
    }
}