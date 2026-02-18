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
        }
    }
}
