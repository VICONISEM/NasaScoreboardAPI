

using Microsoft.EntityFrameworkCore;
using ScoreboardAPI.Models;

namespace ScoreboardAPI.Data
{
    public class ScoreboardContext:DbContext
    {
        public ScoreboardContext(DbContextOptions<ScoreboardContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().Property(t=>t.Id)
                                    .UseIdentityColumn(1,1);

            modelBuilder.Entity<Team>().Property(t=>t.Name)
                .IsRequired();
            modelBuilder.Entity<Team>().Ignore(t => t.PhotoBase64);
            modelBuilder.Entity<Team>().Property(t => t.PhotoPath).IsRequired(false);


            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Team> Teams { get; set;}

    }
}
