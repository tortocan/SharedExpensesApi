using AutoMapper;
using SharedExpenses.Storage.Models;

namespace SharedExpensesApi.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, Models.ApplicationUserResponse>().ReverseMap();
        }
    }
}