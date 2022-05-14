namespace SharedExpensesApi.Models
{
    public class PaymentResponse
    {
        public DateTime? Date { get;  set; }
        public int Id { get; internal set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}