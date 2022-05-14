namespace SharedExpensesApi.Models
{
    public class ExpenseGroupUsersResponse
    {
        public int Id { get; set; }
        public IEnumerable<ApplicationUserResponse> User { get; set; }
    }
}