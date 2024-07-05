using ListaDeTarefasProjeto.Models;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefasProjeto.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasData(
                    new Categoria { CategoriaId = "trabalho", Nome = "Trabalho" },
                    new Categoria { CategoriaId = "casa", Nome = "Casa" },
                    new Categoria { CategoriaId = "faculdade", Nome = "Faculdade" },
                    new Categoria { CategoriaId = "compras", Nome = "Compras" },
                    new Categoria { CategoriaId = "academia", Nome = "Academia" }

                );
            modelBuilder.Entity<Status>().HasData(
                    new Status { StatusId = "aberto", Nome = "Aberto" },
                    new Status { StatusId = "completo", Nome = "Completo" }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
