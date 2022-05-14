namespace SharedExpenses.Storage.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public ICollection<ExpenseGroup> ExpenseGroup { get;  set; }
    }
}