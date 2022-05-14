namespace SharedExpenses.Storage.Models
{
    public class Expense
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ExpenseGroupId { get; set; }
        public ExpenseGroup ExpenseGroup { get; set; }
    }
}