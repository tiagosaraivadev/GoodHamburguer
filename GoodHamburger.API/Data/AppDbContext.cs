using GoodHamburger.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Item> Itens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Itens)
                .WithMany(i => i.Pedidos)
                .UsingEntity(j => j.ToTable("PedidoItens"));

            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, Nome = "X Burger", Preco = 5.00m, Tipo = TipoItem.Sanduiche },
                new Item { Id = 2, Nome = "X Egg", Preco = 4.50m, Tipo = TipoItem.Sanduiche },
                new Item { Id = 3, Nome = "X Bacon", Preco = 7.00m, Tipo = TipoItem.Sanduiche },
                new Item { Id = 4, Nome = "Batata Frita", Preco = 2.00m, Tipo = TipoItem.Batata },
                new Item { Id = 5, Nome = "Coca-Cola 250ml", Preco = 2.50m, Tipo = TipoItem.Refrigerante }
            );
        }
    }
}
