using System.ComponentModel.DataAnnotations;

namespace SharedExpensesApi.Models
{
    public class ExpenseRequest
    {
        [Required]
        public PaymentRequest Payment { get; set; }
    }
}