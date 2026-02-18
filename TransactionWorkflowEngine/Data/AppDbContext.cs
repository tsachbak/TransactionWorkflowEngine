using Microsoft.EntityFrameworkCore;
using TransactionWorkflowEngine.Models;

namespace TransactionWorkflowEngine.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionStatus> TransactionStatuses => Set<TransactionStatus>();
        public DbSet<TransactionStatusTransition> TransactionStatusTransitions => Set<TransactionStatusTransition>();
        public DbSet<TransactionStatusHistory> TransactionStatusHistories => Set<TransactionStatusHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TransactionStatus>()
                .HasIndex(ts => ts.Name)
                .IsUnique();

            modelBuilder.Entity<TransactionStatusTransition>()
                .HasIndex(tst => new {tst.FromStatusId, tst.ToStatusId })
                .IsUnique();

            modelBuilder.Entity<TransactionStatusTransition>()
                .HasOne(tst => tst.FromStatus)
                .WithMany()
                .HasForeignKey(tst => tst.FromStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionStatusTransition>()
                .HasOne(tst => tst.ToStatus)
                .WithMany()
                .HasForeignKey(tst => tst.ToStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CurrentStatus)
                .WithMany()
                .HasForeignKey(t => t.CurrentStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            var createdAt = new DateTime(2026, 1, 1);

            modelBuilder.Entity<TransactionStatus>().HasData(
                new TransactionStatus { Id = 1, Name = "CREATED", CreatedAt = createdAt, IsActive = true },
                new TransactionStatus { Id = 2, Name = "VALIDATED", CreatedAt = createdAt, IsActive = true },
                new TransactionStatus { Id = 3, Name = "PROCESSING", CreatedAt = createdAt, IsActive = true },
                new TransactionStatus { Id = 4, Name = "COMPLETED", CreatedAt = createdAt, IsActive = true },
                new TransactionStatus { Id = 5, Name = "FAILED", CreatedAt = createdAt, IsActive = true }
            );

            modelBuilder.Entity<TransactionStatusTransition>().HasData(
                new TransactionStatusTransition { Id = 1, FromStatusId = 1, ToStatusId = 2, CreatedAt = createdAt},
                new TransactionStatusTransition { Id = 2, FromStatusId = 2, ToStatusId = 3, CreatedAt = createdAt},
                new TransactionStatusTransition { Id = 3, FromStatusId = 3, ToStatusId = 4, CreatedAt = createdAt},
                new TransactionStatusTransition { Id = 4, FromStatusId = 2, ToStatusId = 5, CreatedAt = createdAt},
                new TransactionStatusTransition { Id = 5, FromStatusId = 5, ToStatusId = 2, CreatedAt = createdAt}
            );
        }
    }
}
