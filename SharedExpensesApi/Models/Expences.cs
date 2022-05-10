namespace SharedExpensesApi.Models
{
    public class Expences
    {
        public int Id { get; set; }
        public Payment? Payment { get; set; }
        public ApplicationUser User { get;  set; }
    }
}