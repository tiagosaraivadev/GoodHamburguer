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
    }
}
