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
    public class FoodOrderConfiguration : IEntityTypeConfiguration<FoodOrder>
    {
        public void Configure(EntityTypeBuilder<FoodOrder> builder)
        {
            builder.ToTable("food_order", "couple_app");

            builder.HasKey(fo => fo.OrderId)
                .HasName("food_order_pkey");

            builder.Property(fo => fo.OrderId)
                .HasColumnName("order_id")
                .ValueGeneratedOnAdd();

            builder.Property(fo => fo.Description)
                .HasColumnName("description")
                .IsRequired();

            builder.Property(fo => fo.Price)
                .HasColumnName("price")
                .IsRequired();

            builder.Property(fo => fo.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasConversion<string>();

            builder.Property(fo => fo.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("now()");

            builder.Property(fo => fo.ApprovedAt)
                .HasColumnName("approved_at");

            builder.Property(fo => fo.InitiatorId)
                .HasColumnName("initiator_id")
                .IsRequired();

            builder.HasOne(fo => fo.Initiator)
                .WithMany(u => u.InitiatedOrders)
                .HasForeignKey(fo => fo.InitiatorId)
                .HasConstraintName("fk_order_initiator")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasCheckConstraint("ck_order_approval_date",
                "approved_at >= created_at OR approved_at IS NULL");

            builder.HasIndex(fo => fo.InitiatorId)
                .HasDatabaseName("idx_food_order_initiator");

            builder.HasIndex(fo => fo.Status)
                .HasDatabaseName("idx_food_order_status");
        }
    }
}
