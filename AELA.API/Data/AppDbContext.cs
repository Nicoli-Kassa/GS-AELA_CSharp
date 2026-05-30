using AELA.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AELA.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Astronauta> Astronautas { get; set; }
        public DbSet<OperadorTerrestre> OperadoresTerrestres { get; set; }
        public DbSet<Baseline> Baselines { get; set; }
        public DbSet<LeituraFisiologica> Leituras { get; set; }
        public DbSet<ReadinessScore> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Astronauta>().ToTable("Astronautas");
            modelBuilder.Entity<OperadorTerrestre>().ToTable("OperadoresTerrestres");

            // O enum TipoTarefa é salvo como texto (mais legível e seguro no Oracle).
            modelBuilder.Entity<ReadinessScore>()
                .Property(s => s.Tarefa)
                .HasConversion<string>();
        }
    }
}