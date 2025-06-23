using AutoMapper;
using FinanceApp.Api.Model;
using FinanceApp.Api.Model.DTO;

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
