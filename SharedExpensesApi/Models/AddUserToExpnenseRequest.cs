using System.ComponentModel.DataAnnotations;

namespace SharedExpensesApi.Models
{
    public class AddUserToExpnenseGroupRequest
    {
        [Required,Range(1,int.MaxValue)]
        public int UserId { get;  set; }
        [Range(1,int.MaxValue)]
        public int ExpenseGroupId { get; set; }
    }
}