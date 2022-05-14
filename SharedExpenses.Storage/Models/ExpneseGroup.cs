namespace SharedExpenses.Storage.Models
{
    public class ExpenseGroup
    {
        public int Id { get; set; }
        public ICollection<Expense> Expense { get; set; }
        public ICollection<ApplicationUser> User { get; set; }
    }
}