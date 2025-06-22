using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Thenestle.Domain.Models;

namespace Thenestle.Persistence.Data.Configurations
{
    public class CurrencyTransactionConfiguration : IEntityTypeConfiguration<CurrencyTransaction>
    {
        public void Configure(EntityTypeBuilder<CurrencyTransaction> builder)
        {
            builder.ToTable("currency_transaction", "couple_app");

            builder.HasKey(ct => ct.TransactionId)
                .HasName("currency_transaction_pkey");

            builder.Property(ct => ct.TransactionId)
                .HasColumnName("transaction_id")
                .ValueGeneratedOnAdd();

            builder.Property(ct => ct.Amount)
                .HasColumnName("amount")
                .IsRequired();

            builder.Property(ct => ct.Type)
                .HasColumnName("type")
                .IsRequired()
                .HasMaxLength(20)
                .HasConversion<string>();

            builder.Property(ct => ct.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(ct => ct.OrderId)
                .HasColumnName("order_id");

            builder.Property(ct => ct.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("now()");

            builder.HasOne(ct => ct.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(ct => ct.UserId)
                .HasConstraintName("fk_transaction_user")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ct => ct.Order)
                .WithOne(o => o.Transaction)
                .HasForeignKey<CurrencyTransaction>(ct => ct.OrderId)
                .HasConstraintName("fk_transaction_order")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasCheckConstraint("ck_transaction_amount", "amount != 0");

            builder.HasIndex(ct => ct.UserId)
                .HasDatabaseName("idx_transaction_user");

            builder.HasIndex(ct => ct.OrderId)
                .IsUnique()
                .HasDatabaseName("idx_transaction_order_unique");
        }
    }
}
