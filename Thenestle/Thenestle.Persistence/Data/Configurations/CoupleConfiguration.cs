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
    public class CoupleConfiguration : IEntityTypeConfiguration<Couple>
    {
        public void Configure(EntityTypeBuilder<Couple> builder)
        {
            builder.ToTable("couple", "couple_app");

            builder.HasKey(c => c.CoupleId)
                .HasName("couple_pkey");

            builder.Property(c => c.CoupleId)
                .HasColumnName("couple_id")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");

            builder.Property(c => c.User1Id)
                .HasColumnName("user1_id");

            builder.Property(c => c.User2Id)
                .HasColumnName("user2_id");

            builder.HasOne(c => c.User1)
                .WithMany()
                .HasForeignKey(c => c.User1Id)
                .HasConstraintName("fk_couple_user1")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.User2Id)
                .HasConstraintName("fk_couple_user2")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasCheckConstraint("ck_couple_users", "user1_id < user2_id");

            builder.HasIndex(c => new { c.User1Id, c.User2Id })
                .IsUnique()
                .HasDatabaseName("idx_couple_users_unique");
        }
    }
}
