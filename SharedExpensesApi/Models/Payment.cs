namespace SharedExpensesApi.Models
{
    public class Payment
    {
        public DateTime Date { get;  set; }
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}