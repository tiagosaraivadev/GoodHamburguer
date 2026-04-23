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
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString()))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Preco, opt => opt.MapFrom(src => src.Preco));

            CreateMap<Pedido, PedidoRespostaDto>();
        }
    }
}
