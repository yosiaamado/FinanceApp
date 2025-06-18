using AutoMapper;
using FinanceApp.Api.Model.DTO;
using FinanceApp.Api.Model.Transaction;

namespace FinanceApp.Api.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TransactionRequest, Transaction>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity ?? 1));
        }
    }
}
