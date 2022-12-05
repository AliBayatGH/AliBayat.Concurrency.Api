using AliBayat.Concurrency.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace AliBayat.Concurrency.Api.Data;

public class ApplicatrionDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ApplicatrionDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<User>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<User>().HasData(new User { Id = 1, Balance = 1000, Withdrawn = 0 });

        base.OnModelCreating(modelBuilder);
    }
}
