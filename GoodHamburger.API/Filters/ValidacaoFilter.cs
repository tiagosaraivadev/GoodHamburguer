using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoodHamburger.API.Filters;

public class ValidacaoFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argumento in context.ActionArguments.Values)
        {
            if (argumento is null) continue;

            var tipoValidator = typeof(IValidator<>).MakeGenericType(argumento.GetType());

            if (context.HttpContext.RequestServices.GetService(tipoValidator) is not IValidator validator) continue;

            var resultado = await validator.ValidateAsync(new ValidationContext<object>(argumento));

            if (!resultado.IsValid)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    mensagens = resultado.Errors.Select(e => e.ErrorMessage)
                });
                return;
            }
        }

        await next();
    }
}