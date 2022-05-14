using AutoMapper;
using SharedExpenses.Storage.Models;

namespace SharedExpensesApi.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, Models.PaymentResponse>().ReverseMap();
            CreateMap<Payment, Models.PaymentRequest>().ReverseMap();
        }
    }
}