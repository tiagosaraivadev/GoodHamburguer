using FluentValidation;
using GoodHamburger.API.DTOs;

namespace GoodHamburger.API.Validators
{
    public class CreatePedidoValidator : AbstractValidator<PedidoRequestDto>
    {
        public CreatePedidoValidator() 
        {
            RuleFor(i => i.ItensId)
                .NotNull().WithMessage("A lista de itens não pode ser nula.")
                .NotEmpty().WithMessage("O pedido deve conter ao menos um item.");

            RuleForEach(x => x.ItensId)
                .GreaterThan(0).WithMessage("Os IDs dos itens devem ser válidos.");
        }
    } 
}
