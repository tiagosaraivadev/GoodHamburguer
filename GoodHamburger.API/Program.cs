using FluentValidation;
using GoodHamburger.API.Data;
using GoodHamburger.API.Filters;
using GoodHamburger.API.Repositories;
using GoodHamburger.API.Repositories.Interfaces;
using GoodHamburger.API.Services;
using GoodHamburger.API.Services.Interfaces;
using GoodHamburger.API.Validators;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<CreatePedidoValidator>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidacaoFilter>();
});
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=goodhamburger.db"));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();