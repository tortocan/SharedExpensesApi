using SharedExpenses.Storage.Models;

namespace SharedExpenses.Storage.Abstraction
{
    public interface IExpensesService
    {
        Task<IEnumerable<Expense>> GetExpensesOrderedByPaymentDateAsync(int expenseGroupId);
        Task<bool> AddUserToExpenseGroupAsync(int userId, int expenseGroupId);
        Task<bool> AddExpenseAsync(int userId, int expenseGroupId, Expense expense);
        Task<BalanceSummary> GetExpenseGroupBalanceAsync(int expenseGroupId);
        Task<IEnumerable<ExpenseGroup>> GetExpenseGroupUsersAsync(int expenseGroupId);
    }
}