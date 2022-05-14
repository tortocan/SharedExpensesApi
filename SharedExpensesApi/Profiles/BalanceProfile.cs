using AutoMapper;
using SharedExpenses.Storage.Models;
using SharedExpensesApi.Models;

namespace SharedExpensesApi.Profiles
{
    public class BalanceProfile : Profile
    {
        public BalanceProfile()
        {
            CreateMap<BalanceDue,BalanceDueResponse>().ReverseMap();
            CreateMap<Balance,BalanceResponse>().ReverseMap();
            CreateMap<BalanceSummary,BalanceSummaryResponse>().ReverseMap();
        }
    }
}