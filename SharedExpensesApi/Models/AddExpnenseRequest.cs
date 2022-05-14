using System.ComponentModel.DataAnnotations;

namespace SharedExpensesApi.Models
{
    public class AddExpnenseRequest
    {
        [Required,Range(1,int.MaxValue)]
        public int UserId { get; set; }
        [Required,Range(1,int.MaxValue)]
        public int ExpenseGroupId { get; set; }
        [Required]
        public ExpenseRequest Expense { get;  set; }
    }
}