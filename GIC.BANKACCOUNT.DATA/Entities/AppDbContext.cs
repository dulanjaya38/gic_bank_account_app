using Microsoft.EntityFrameworkCore;

namespace GIC.BANKACCOUNT.DATA.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<IntrestRule> IntrestRules { get; set; }
        public DbSet<RunningNumber> RunningNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AcccountId);

                entity.Property(e => e.AcccountNo)
                      .IsRequired();

                entity.Property(e => e.IsActive)
                      .IsRequired();

                entity.Property(e => e.DateCreated)
                      .HasColumnType("datetime")
                      .IsRequired();

                entity.HasMany(t => t.Transactions)
                      .WithOne(a => a.Account)
                      .HasForeignKey(a => a.AccountId)
                      .IsRequired();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.HasIndex(e => e.TransactionNo)
                      .IsUnique();

                entity.Property(e => e.Type)
                      .HasMaxLength(1)
                      .IsUnicode(false)
                      .IsFixedLength()
                      .IsRequired();

                entity.Property(e => e.TransactionNo)
                      .IsRequired();

                entity.Property(e => e.Amount)
                      .HasPrecision(38, 2)
                      .IsRequired();

                entity.Property(e => e.TransactionDate)
                      .HasColumnType("datetime")
                      .IsRequired();
            });

            modelBuilder.Entity<IntrestRule>(entity =>
            {
                entity.HasKey(e => e.IntrestRuleId);

                entity.HasIndex(e => e.RuleId)
                      .IsUnique();

                entity.Property(e => e.RuleId)
                      .IsRequired();

                entity.Property(e => e.Rate)
                     .HasPrecision(38, 2)
                     .IsRequired();

                entity.Property(e => e.EffectiveDate)
                      .HasColumnType("datetime")
                      .IsRequired();

                entity.Property(e => e.DateCreated)
                      .HasColumnType("datetime")
                      .IsRequired();

                entity.Property(e => e.IsActive)
                      .IsRequired();
            });

            modelBuilder.Entity<RunningNumber>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.DateStr)
                      .IsUnique();

                entity.Property(e => e.Value)
                      .IsRequired()
                      .IsConcurrencyToken();
            });
        }
    }
}