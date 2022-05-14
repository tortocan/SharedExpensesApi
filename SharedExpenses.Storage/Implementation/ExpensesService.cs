using Microsoft.EntityFrameworkCore;
using SharedExpenses.Storage.Abstraction;
using SharedExpenses.Storage.Models;

namespace SharedExpenses.Storage.Implementation;
public class ExpensesService : IExpensesService
{
    private readonly ApplicationDbContext applicationDbContext;

    public ExpensesService(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public async Task<bool> AddExpenseAsync(int userId, int expenseGroupId, Expense expense)
    {
        expense.UserId = userId;
        expense.ExpenseGroupId = expenseGroupId;
        if (expense.Payment.Date == default(DateTime))
        {
            expense.Payment.Date = DateTime.UtcNow;
        };
        await applicationDbContext.Expense.AddAsync(expense);
        var result = await applicationDbContext.SaveChangesAsync();
        return result > -1;
    }

    public async Task<bool> AddUserToExpenseGroupAsync(int userId, int expenseGroupId)
    {
        var user = await applicationDbContext.User.Include(x => x.ExpenseGroup).FirstOrDefaultAsync(x => x.Id == userId);
        if (user.ExpenseGroup.Any(x => x.Id == expenseGroupId)) { return true; }
        var expenseGroup = await applicationDbContext.ExpenseGroup.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == expenseGroupId);
        if (expenseGroup == null)
        {
            expenseGroup = new ExpenseGroup { Id = expenseGroupId };
            applicationDbContext.ExpenseGroup.Add(expenseGroup);
            await applicationDbContext.SaveChangesAsync();
        }
        expenseGroup.User.Add(user);
        var result = await applicationDbContext.SaveChangesAsync();

        return result > -1;
    }

    public async Task<BalanceSummary> GetExpenseGroupBalanceAsync(int expenseGroupId)
    {
        var expenses = await applicationDbContext.Expense.Include(x => x.User).Include(x => x.Payment).Where(x => x.ExpenseGroupId == expenseGroupId).ToListAsync();
        var users = await applicationDbContext.ExpenseGroup.Include(x => x.User).Where(x => x.Id == expenseGroupId).SelectMany(x => x.User).ToListAsync();
        var devider = users.Count();
        var totalAmount = expenses.Sum(x => x.Payment.Amount);
        var payAmount = totalAmount / devider;
        //Positive balance
        var usersWithExpenses = expenses.GroupBy(x => x.UserId).Select(x => x.FirstOrDefault());
        var result = new BalanceSummary
        {
            Balance = new List<Balance>(),
            BalanceDue = new List<BalanceDue>()
        };
        result.Balance = usersWithExpenses.Select(x =>
        {
            var userTotalAmount = expenses.Where(e => e.UserId == x.User.Id).Sum(s => s.Payment.Amount);
            var balanceAmount = userTotalAmount - payAmount;
            var balance = new Balance
            {
                User = x.User,
                Amount = Math.Round(balanceAmount, 2),
                IsWithExpense = true
            };
            return balance;
        }).ToList();
        //Negative balance
        var userWithoutExpenses = users.Where(x => !usersWithExpenses.Any(e => e.UserId == x.Id)).ToList();
        userWithoutExpenses.ForEach(x =>
        {
            var balance = new Balance
            {
                User = x,
                Amount = Math.Round(-payAmount, 2)
            };
            result.Balance.Add(balance);
        });
        //Balance due
        if (usersWithExpenses.Any())
        {
            var balance = result.Balance.MaxBy(x => x.Amount);
            var substractUserAmount = balance.Amount;
            var usersPayed = new List<ApplicationUser>();
            result.Balance.Where(b => b.Amount < 0).ToList().ForEach(x =>
            {
                var balanceDueAmount = x.Amount;
                if (substractUserAmount < Math.Abs(x.Amount))
                {
                    var remainAmount = substractUserAmount - Math.Abs(x.Amount);
                    if(substractUserAmount > 0) {
                        result.BalanceDue.Add(new BalanceDue
                        {
                            To = balance.User,
                            From = x.User,
                            Amount = Math.Round(-substractUserAmount,2)
                        });
                    }
                    usersPayed.Add(balance.User);
                    balance = result.Balance.Where(b => !usersPayed.Any(up => up.Id == b.User.Id)).MaxBy(x => x.Amount);
                    if(remainAmount > 0) {
                        result.BalanceDue.Add(new BalanceDue
                        {
                            To = balance.User,
                            From = x.User,
                            Amount = Math.Round(remainAmount,2)
                        });
                    }
                    substractUserAmount = 0;
                }
                else
                {
                    result.BalanceDue.Add(new BalanceDue
                    {
                        To = balance.User,
                        From = x.User,
                        Amount = Math.Round(balanceDueAmount,2)
                    });
                    substractUserAmount += balanceDueAmount;
                }
            });
            if (Math.Abs(Math.Round(result.BalanceDue.Sum(x => x.Amount),2)) != result.Balance.Where(x=>x.Amount > 0).Sum(x=>x.Amount)) throw new ArgumentOutOfRangeException(nameof(result.BalanceDue));
        }
        return result;
    }

    public async Task<IEnumerable<ExpenseGroup>> GetExpenseGroupUsersAsync(int expenseGroupId)
    {
        var result = await applicationDbContext.ExpenseGroup.Include(x => x.User).Where(x => x.Id == expenseGroupId).ToListAsync();
        return result;
    }

    public async Task<IEnumerable<Expense>> GetExpensesOrderedByPaymentDateAsync(int expenseGroupId)
    {
        var result = await applicationDbContext.Expense
        .Where(x => x.ExpenseGroupId == expenseGroupId).Include(x => x.ExpenseGroup).Include(x => x.Payment).Include(x => x.User)
        .OrderByDescending(x => x.Payment.Date).ToListAsync();
        return result;
    }
}
