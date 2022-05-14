using AutoMapper;
using SharedExpenses.Storage.Models;

namespace SharedExpensesApi.Profiles
{
    public class ExpensesProfile : Profile
    {
        public ExpensesProfile()
        {
            CreateMap<ExpenseGroup, Models.ExpenseGroupUsersResponse>().ReverseMap();
            CreateMap<ExpenseGroup, Models.ExpenseGroupResponse>().ReverseMap();
            CreateMap<Expense, Models.ExpenseResponse>().ReverseMap();
            CreateMap<Expense, Models.ExpenseRequest>().ReverseMap();
        }
    }
}