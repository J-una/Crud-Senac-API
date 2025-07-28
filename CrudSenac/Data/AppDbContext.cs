using System.Security.Cryptography.X509Certificates;
using CrudSenac.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudSenac.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
    }
}
