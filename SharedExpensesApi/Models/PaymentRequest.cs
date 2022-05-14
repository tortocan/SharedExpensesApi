using System.ComponentModel.DataAnnotations;

namespace SharedExpensesApi.Models
{
    public class PaymentRequest
    {
        public DateTime? Date { get;  set; }
        [Required,Range(0.1d,double.MaxValue)]
        public double Amount { get; set; }
        [Required]
        public string Description { get; set; }
    }
}