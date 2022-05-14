namespace SharedExpensesApi.Models
{
    public class ExpenseResponse
    {
        public PaymentResponse Payment { get; set; }
        public ExpenseGroupResponse ExpenseGroup { get; set; }
        public ApplicationUserResponse User { get;  set; }
    }
}