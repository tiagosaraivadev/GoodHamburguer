using AutoMapper;
using GoodHamburger.API.DTOs;
using GoodHamburger.API.Models;

namespace GoodHamburger.API.Mappers
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<Item, ItemRespostaDto>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString()));

            CreateMap<Pedido, PedidoRespostaDto>();
        }
    }
}
